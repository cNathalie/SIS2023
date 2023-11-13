using AutoMapper;
using SIS.API.DTO;
using SIS.Domain;
using SISAPI.DTO;

namespace SIS.API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Teacher, TeacherDTO>().ReverseMap();
            CreateMap<TeacherPreference, TeacherPreferenceDTO>().ReverseMap(); // BertEnErnie 
            CreateMap<CoordinationRole, CoordinationRoleDTO>().ReverseMap(); // BertEnErnie 
            CreateMap<TimeOnly, DateTime>().ConvertUsing<TimeOnlyToDateTimeConverter>(); // BertEnErnie 
            CreateMap<DateTime, TimeOnly>().ConvertUsing<DateTimeToTimeOnlyConverter>(); // BertEnErnie            
            CreateMap<DateTime, DateOnly>().ConvertUsing<DateTimeToDateOnlyConverter>(); // BertEnErnie 
            CreateMap<DateOnly, DateTime>().ConvertUsing<DateOnlyToDateTimeConverter>(); // BertEnErnie 
            CreateMap<ShedulingTimeslot, ShedulingTimeslotDTO>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.StopTime, opt => opt.MapFrom(src => src.StopTime))
                .ReverseMap(); // BertEnErnie 
            CreateMap<TeacherLocationInterest, TeacherLocationInterestDTO>()
                .ForMember(dto => dto.AcademicYearStart, options => options.MapFrom(src => src.AcademicYearStart.Date))
                .ForMember(dto => dto.AcademicYearStop, options => options.MapFrom(src => src.AcademicYearStop.Date))
                .ReverseMap(); //BertEnErnie
            CreateMap<TeacherCoordinationRoleInterest, TeacherCoordinationRoleInterestDTO>()
                .ForMember(dto => dto.AcademicYearStart, options => options.MapFrom(src => src.AcademicYearStart.Date))
                .ForMember(dto => dto.AcademicYearStop, options => options.MapFrom(src => src.AcademicYearStop.Date))
                .ReverseMap(); //BertEnErnie
            CreateMap<Period, PeriodDTO>().ReverseMap(); //BertEnErnie



            CreateMap<Room, RoomDTO>().ReverseMap(); // Da engineering
            CreateMap<RoomType, RoomTypeDTO>().ReverseMap();// Da engineering
            CreateMap<RoomKind, RoomKindDTO>().ReverseMap();// Da engineering
            CreateMap<Building, BuildingDTO>().ReverseMap();// Da engineering
            CreateMap<Location, LocationDTO>().ReverseMap();// Da engineering
            CreateMap<Campus, CampusDTO>().ReverseMap();// Da engineering

        }
    }

    //Custom Converters for DateTime - DateOnly - TimeOnly

    public class TimeOnlyToDateTimeConverter : ITypeConverter<TimeOnly, DateTime>
    {
        public DateTime Convert(TimeOnly source, DateTime destination, ResolutionContext context)
        {
            return new DateTime(1, 1, 1, source.Hour, source.Minute, source.Second);
        }
    }

    public class DateTimeToTimeOnlyConverter : ITypeConverter<DateTime, TimeOnly>
    {
        public TimeOnly Convert(DateTime source, TimeOnly destination, ResolutionContext context)
        {
            return new TimeOnly(source.Hour, source.Minute, source.Second);
        }
    }

    public class DateTimeToDateOnlyConverter : ITypeConverter<DateTime, DateOnly>
    {
        public DateOnly Convert(DateTime source, DateOnly destination, ResolutionContext context)
        {
            return new DateOnly(source.Year, source.Month, source.Day);
        }
    }

    public class DateOnlyToDateTimeConverter : ITypeConverter<DateOnly, DateTime>
    {
        public DateTime Convert(DateOnly source, DateTime destination, ResolutionContext context)
        {
            return new DateTime(source.Year, source.Month, source.Day, 0, 0, 0);
        }
    }

}

