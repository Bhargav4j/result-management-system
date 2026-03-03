using AutoMapper;
using RMS.Application.DTOs;
using RMS.Domain.Entities;

namespace RMS.Application.Mappings;

/// <summary>
/// AutoMapper profile for entity to DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Student mappings
        CreateMap<Student, StudentDto>();
        CreateMap<StudentCreateDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<StudentUpdateDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
            .ForMember(dest => dest.UserCreated, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());

        // Course mappings
        CreateMap<Course, CourseDto>();
        CreateMap<CourseCreateDto, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<CourseUpdateDto, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());
    }
}
