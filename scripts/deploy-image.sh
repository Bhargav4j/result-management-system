#!/bin/bash
set -e
set -o pipefail

echo "=========================================="
echo "  RMS AWS ECS Fargate Deployment Script"
echo "=========================================="
echo ""

# Prompt for AWS configuration
read -p "Enter AWS Region (e.g., us-east-1): " AWS_REGION
read -p "Enter ECS Cluster Name (e.g., rms-cluster): " CLUSTER_NAME
read -p "Enter VPC ID (e.g., vpc-0abc123def456): " VPC_ID
read -p "Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): " SUBNETS_INPUT
read -p "Enter Security Group ID (e.g., sg-0abc123def): " SECURITY_GROUP
read -p "Enter Docker Image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/rms-web:latest): " IMAGE_URI

# Convert comma-separated subnets to array
IFS=',' read -ra SUBNETS <<< "$SUBNETS_INPUT"
SUBNET_1=${SUBNETS[0]}
SUBNET_2=${SUBNETS[1]:-$SUBNET_1}

echo ""
echo "Configuration Summary:"
echo "  Region: $AWS_REGION"
echo "  Cluster: $CLUSTER_NAME"
echo "  VPC: $VPC_ID"
echo "  Subnets: $SUBNET_1, $SUBNET_2"
echo "  Security Group: $SECURITY_GROUP"
echo "  Image: $IMAGE_URI"
echo ""

# Get AWS Account ID
echo "Retrieving AWS Account ID..."
ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
echo "Account ID: $ACCOUNT_ID"
echo ""

# Check if cluster exists, create if not
echo "Checking if ECS cluster exists..."
aws ecs describe-clusters --clusters "$CLUSTER_NAME" --region "$AWS_REGION" >/dev/null 2>&1 || {
    echo "Cluster does not exist. Creating cluster: $CLUSTER_NAME"
    aws ecs create-cluster --cluster-name "$CLUSTER_NAME" --region "$AWS_REGION"
    echo "Cluster created successfully."
}
echo ""

# Load balancer configuration
read -p "Do you need a load balancer for this service? (y/n): " NEED_LB
LOAD_BALANCER_ARN=""
TARGET_GROUP_ARN=""

if [ "$NEED_LB" = "y" ] || [ "$NEED_LB" = "Y" ]; then
    echo ""
    echo "Creating Application Load Balancer and Target Group..."
    
    LB_NAME="rms-web-alb"
    TG_NAME="rms-web-tg"
    
    # Create Application Load Balancer
    echo "Creating ALB: $LB_NAME"
    LB_RESULT=$(aws elbv2 create-load-balancer \
        --name "$LB_NAME" \
        --subnets "$SUBNET_1" "$SUBNET_2" \
        --security-groups "$SECURITY_GROUP" \
        --scheme internet-facing \
        --type application \
        --ip-address-type ipv4 \
        --region "$AWS_REGION" \
        --output json 2>/dev/null || echo '{}')
    
    LOAD_BALANCER_ARN=$(echo "$LB_RESULT" | jq -r '.LoadBalancers[0].LoadBalancerArn // empty')
    LB_DNS=$(echo "$LB_RESULT" | jq -r '.LoadBalancers[0].DNSName // empty')
    
    if [ -z "$LOAD_BALANCER_ARN" ]; then
        echo "Checking if ALB already exists..."
        LB_RESULT=$(aws elbv2 describe-load-balancers --names "$LB_NAME" --region "$AWS_REGION" --output json 2>/dev/null || echo '{}')
        LOAD_BALANCER_ARN=$(echo "$LB_RESULT" | jq -r '.LoadBalancers[0].LoadBalancerArn // empty')
        LB_DNS=$(echo "$LB_RESULT" | jq -r '.LoadBalancers[0].DNSName // empty')
    fi
    
    echo "Load Balancer ARN: $LOAD_BALANCER_ARN"
    echo "Load Balancer DNS: $LB_DNS"
    
    # Create Target Group with target-type ip (required for Fargate awsvpc)
    echo "Creating Target Group: $TG_NAME"
    TG_RESULT=$(aws elbv2 create-target-group \
        --name "$TG_NAME" \
        --protocol HTTP \
        --port 8080 \
        --vpc-id "$VPC_ID" \
        --target-type ip \
        --health-check-enabled \
        --health-check-path "/health" \
        --health-check-interval-seconds 30 \
        --health-check-timeout-seconds 5 \
        --healthy-threshold-count 2 \
        --unhealthy-threshold-count 3 \
        --region "$AWS_REGION" \
        --output json 2>/dev/null || echo '{}')
    
    TARGET_GROUP_ARN=$(echo "$TG_RESULT" | jq -r '.TargetGroups[0].TargetGroupArn // empty')
    
    if [ -z "$TARGET_GROUP_ARN" ]; then
        echo "Checking if Target Group already exists..."
        TG_RESULT=$(aws elbv2 describe-target-groups --names "$TG_NAME" --region "$AWS_REGION" --output json 2>/dev/null || echo '{}')
        TARGET_GROUP_ARN=$(echo "$TG_RESULT" | jq -r '.TargetGroups[0].TargetGroupArn // empty')
    fi
    
    echo "Target Group ARN: $TARGET_GROUP_ARN"
    
    # Create listener if it doesn't exist
    echo "Creating ALB Listener..."
    aws elbv2 create-listener \
        --load-balancer-arn "$LOAD_BALANCER_ARN" \
        --protocol HTTP \
        --port 80 \
        --default-actions Type=forward,TargetGroupArn="$TARGET_GROUP_ARN" \
        --region "$AWS_REGION" >/dev/null 2>&1 || echo "Listener may already exist."
    
    echo "Load balancer setup complete."
    echo ""
fi

# Update ECS task definition JSON
echo "Preparing ECS task definition..."
TASK_DEF_FILE="ecs/task-definition.json"

cp "$TASK_DEF_FILE" "${TASK_DEF_FILE}.backup"

sed -i "s|{{IMAGE_URI}}|${IMAGE_URI}|g" "$TASK_DEF_FILE"
sed -i "s|{{AWS_REGION}}|${AWS_REGION}|g" "$TASK_DEF_FILE"
sed -i "s|{{ACCOUNT_ID}}|${ACCOUNT_ID}|g" "$TASK_DEF_FILE"

echo "Registering ECS task definition..."
TASK_DEF_ARN=$(aws ecs register-task-definition \
    --cli-input-json file://${TASK_DEF_FILE} \
    --region "$AWS_REGION" \
    --query 'taskDefinition.taskDefinitionArn' \
    --output text)

echo "Task Definition ARN: $TASK_DEF_ARN"
echo ""

# Restore original task definition file
mv "${TASK_DEF_FILE}.backup" "$TASK_DEF_FILE"

# Update ECS service definition JSON
echo "Preparing ECS service definition..."
SERVICE_DEF_FILE="ecs/service-definition.json"

cp "$SERVICE_DEF_FILE" "${SERVICE_DEF_FILE}.backup"

sed -i "s|{{CLUSTER_NAME}}|${CLUSTER_NAME}|g" "$SERVICE_DEF_FILE"
sed -i "s|{{SUBNET_1}}|${SUBNET_1}|g" "$SERVICE_DEF_FILE"
sed -i "s|{{SUBNET_2}}|${SUBNET_2}|g" "$SERVICE_DEF_FILE"
sed -i "s|{{SECURITY_GROUP}}|${SECURITY_GROUP}|g" "$SERVICE_DEF_FILE"

if [ -n "$TARGET_GROUP_ARN" ]; then
    sed -i "s|{{TARGET_GROUP_ARN}}|${TARGET_GROUP_ARN}|g" "$SERVICE_DEF_FILE"
else
    # Remove loadBalancers section if no load balancer
    TMP_FILE=$(mktemp)
    jq 'del(.loadBalancers)' "$SERVICE_DEF_FILE" > "$TMP_FILE"
    mv "$TMP_FILE" "$SERVICE_DEF_FILE"
fi

# Check if service exists
SERVICE_NAME="rms-web-service"
echo "Checking if service exists..."
EXISTING_SERVICE=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].serviceName' \
    --output text 2>/dev/null || echo "None")

if [ "$EXISTING_SERVICE" = "None" ] || [ -z "$EXISTING_SERVICE" ]; then
    echo "Service does not exist. Creating new service..."
    aws ecs create-service \
        --cli-input-json file://${SERVICE_DEF_FILE} \
        --region "$AWS_REGION"
    echo "Service created successfully."
else
    echo "Service exists. Updating service..."
    aws ecs update-service \
        --cluster "$CLUSTER_NAME" \
        --service "$SERVICE_NAME" \
        --task-definition "$TASK_DEF_ARN" \
        --force-new-deployment \
        --region "$AWS_REGION"
    echo "Service updated successfully."
fi

# Restore original service definition file
mv "${SERVICE_DEF_FILE}.backup" "$SERVICE_DEF_FILE"

echo ""
echo "Waiting for service to reach stable state..."
aws ecs wait services-stable \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION"

echo ""
echo "=========================================="
echo "  DEPLOYMENT SUCCESSFUL!"
echo "=========================================="
echo ""
echo "Service Details:"
aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].[serviceName,status,runningCount,desiredCount]' \
    --output table

echo ""
echo "CloudWatch Logs:"
echo "  Log Group: /ecs/rms-web"
echo "  Region: $AWS_REGION"
echo ""

if [ -n "$LB_DNS" ]; then
    echo "Application URL: http://$LB_DNS"
    echo ""
fi

echo "Deployment complete!"
echo ""