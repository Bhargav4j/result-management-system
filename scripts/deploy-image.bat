@echo off
setlocal enabledelayedexpansion

echo ==========================================
echo   RMS AWS ECS Fargate Deployment Script
echo ==========================================
echo.

set /p AWS_REGION="Enter AWS Region (e.g., us-east-1): "
set /p CLUSTER_NAME="Enter ECS Cluster Name (e.g., rms-cluster): "
set /p VPC_ID="Enter VPC ID (e.g., vpc-0abc123def456): "
set /p SUBNETS_INPUT="Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): "
set /p SECURITY_GROUP="Enter Security Group ID (e.g., sg-0abc123def): "
set /p IMAGE_URI="Enter Docker Image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/rms-web:latest): "

for /f "tokens=1,2 delims=," %%a in ("!SUBNETS_INPUT!") do (
    set SUBNET_1=%%a
    set SUBNET_2=%%b
)
if "!SUBNET_2!"=="" set SUBNET_2=!SUBNET_1!

echo.
echo Configuration Summary:
echo   Region: !AWS_REGION!
echo   Cluster: !CLUSTER_NAME!
echo   VPC: !VPC_ID!
echo   Subnets: !SUBNET_1!, !SUBNET_2!
echo   Security Group: !SECURITY_GROUP!
echo   Image: !IMAGE_URI!
echo.

echo Retrieving AWS Account ID...
for /f "delims=" %%i in ('aws sts get-caller-identity --query Account --output text') do set ACCOUNT_ID=%%i
echo Account ID: !ACCOUNT_ID!
echo.

echo Checking if ECS cluster exists...
aws ecs describe-clusters --clusters !CLUSTER_NAME! --region !AWS_REGION! >nul 2>&1
if !ERRORLEVEL! neq 0 (
    echo Cluster does not exist. Creating cluster: !CLUSTER_NAME!
    aws ecs create-cluster --cluster-name !CLUSTER_NAME! --region !AWS_REGION!
    echo Cluster created successfully.
)
echo.

set /p NEED_LB="Do you need a load balancer for this service? (y/n): "
set TARGET_GROUP_ARN=

if /i "!NEED_LB!"=="y" (
    echo.
    echo Creating Application Load Balancer and Target Group...
    
    set LB_NAME=rms-web-alb
    set TG_NAME=rms-web-tg
    
    echo Creating ALB: !LB_NAME!
    aws elbv2 create-load-balancer --name !LB_NAME! --subnets !SUBNET_1! !SUBNET_2! --security-groups !SECURITY_GROUP! --scheme internet-facing --type application --region !AWS_REGION! >nul 2>&1
    
    for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --names !LB_NAME! --region !AWS_REGION! --query "LoadBalancers[0].LoadBalancerArn" --output text 2^>nul') do set LOAD_BALANCER_ARN=%%i
    for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --names !LB_NAME! --region !AWS_REGION! --query "LoadBalancers[0].DNSName" --output text 2^>nul') do set LB_DNS=%%i
    
    echo Load Balancer ARN: !LOAD_BALANCER_ARN!
    echo Load Balancer DNS: !LB_DNS!
    
    echo Creating Target Group: !TG_NAME!
    aws elbv2 create-target-group --name !TG_NAME! --protocol HTTP --port 8080 --vpc-id !VPC_ID! --target-type ip --health-check-enabled --health-check-path "/health" --region !AWS_REGION! >nul 2>&1
    
    for /f "delims=" %%i in ('aws elbv2 describe-target-groups --names !TG_NAME! --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text 2^>nul') do set TARGET_GROUP_ARN=%%i
    
    echo Target Group ARN: !TARGET_GROUP_ARN!
    
    echo Creating ALB Listener...
    aws elbv2 create-listener --load-balancer-arn !LOAD_BALANCER_ARN! --protocol HTTP --port 80 --default-actions Type=forward,TargetGroupArn=!TARGET_GROUP_ARN! --region !AWS_REGION! >nul 2>&1
    
    echo Load balancer setup complete.
    echo.
)

echo Preparing ECS task definition...
set TASK_DEF_FILE=ecs\task-definition.json

copy !TASK_DEF_FILE! !TASK_DEF_FILE!.backup >nul

powershell -Command "(Get-Content !TASK_DEF_FILE!) -replace '{{IMAGE_URI}}', '!IMAGE_URI!' | Set-Content !TASK_DEF_FILE!"
powershell -Command "(Get-Content !TASK_DEF_FILE!) -replace '{{AWS_REGION}}', '!AWS_REGION!' | Set-Content !TASK_DEF_FILE!"
powershell -Command "(Get-Content !TASK_DEF_FILE!) -replace '{{ACCOUNT_ID}}', '!ACCOUNT_ID!' | Set-Content !TASK_DEF_FILE!"

echo Registering ECS task definition...
for /f "delims=" %%i in ('aws ecs register-task-definition --cli-input-json file://!TASK_DEF_FILE! --region !AWS_REGION! --query "taskDefinition.taskDefinitionArn" --output text') do set TASK_DEF_ARN=%%i

echo Task Definition ARN: !TASK_DEF_ARN!
echo.

move /y !TASK_DEF_FILE!.backup !TASK_DEF_FILE! >nul

echo Preparing ECS service definition...
set SERVICE_DEF_FILE=ecs\service-definition.json

copy !SERVICE_DEF_FILE! !SERVICE_DEF_FILE!.backup >nul

powershell -Command "(Get-Content !SERVICE_DEF_FILE!) -replace '{{CLUSTER_NAME}}', '!CLUSTER_NAME!' | Set-Content !SERVICE_DEF_FILE!"
powershell -Command "(Get-Content !SERVICE_DEF_FILE!) -replace '{{SUBNET_1}}', '!SUBNET_1!' | Set-Content !SERVICE_DEF_FILE!"
powershell -Command "(Get-Content !SERVICE_DEF_FILE!) -replace '{{SUBNET_2}}', '!SUBNET_2!' | Set-Content !SERVICE_DEF_FILE!"
powershell -Command "(Get-Content !SERVICE_DEF_FILE!) -replace '{{SECURITY_GROUP}}', '!SECURITY_GROUP!' | Set-Content !SERVICE_DEF_FILE!"

if not "!TARGET_GROUP_ARN!"==" " (
    powershell -Command "(Get-Content !SERVICE_DEF_FILE!) -replace '{{TARGET_GROUP_ARN}}', '!TARGET_GROUP_ARN!' | Set-Content !SERVICE_DEF_FILE!"
)

set SERVICE_NAME=rms-web-service
echo Checking if service exists...
for /f "delims=" %%i in ('aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].serviceName" --output text 2^>nul') do set EXISTING_SERVICE=%%i

if "!EXISTING_SERVICE!"=="None" (
    echo Service does not exist. Creating new service...
    aws ecs create-service --cli-input-json file://!SERVICE_DEF_FILE! --region !AWS_REGION!
    echo Service created successfully.
) else (
    echo Service exists. Updating service...
    aws ecs update-service --cluster !CLUSTER_NAME! --service !SERVICE_NAME! --task-definition !TASK_DEF_ARN! --force-new-deployment --region !AWS_REGION!
    echo Service updated successfully.
)

move /y !SERVICE_DEF_FILE!.backup !SERVICE_DEF_FILE! >nul

echo.
echo Waiting for service to reach stable state...
aws ecs wait services-stable --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION!

echo.
echo ==========================================
echo   DEPLOYMENT SUCCESSFUL!
echo ==========================================
echo.
echo Service Details:
aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].[serviceName,status,runningCount,desiredCount]" --output table

echo.
echo CloudWatch Logs:
echo   Log Group: /ecs/rms-web
echo   Region: !AWS_REGION!
echo.

if not "!LB_DNS!"=="" (
    echo Application URL: http://!LB_DNS!
    echo.
)

echo Deployment complete!
echo.

endlocal