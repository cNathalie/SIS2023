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
            CreateMap<TeacherLocationInterest, TeacherLocationInterestDTO>()
                .ForMember(dto => dto.AcademicYearStart, options => options.MapFrom(src => src.AcademicYearStart.Date))
                .ForMember(dto => dto.AcademicYearStop, options => options.MapFrom(src => src.AcademicYearStop.Date))
                .ReverseMap(); //BertEnErnie
            CreateMap<TeacherLocationInterest, TeacherLocationInterestDTO>().ReverseMap(); //BertEnErnie
            CreateMap<Room, RoomDTO>().ReverseMap(); // Da engineering
            CreateMap<RoomType, RoomTypeDTO>().ReverseMap();// Da engineering
            CreateMap<RoomKind, RoomKindDTO>().ReverseMap();// Da engineering
            CreateMap<Building, BuildingDTO>().ReverseMap();// Da engineering
            CreateMap<Location, LocationDTO>().ReverseMap();// Da engineering
            CreateMap<Campus, CampusDTO>().ReverseMap();// Da engineering

        }
    }
}

