using AutoMapper;
using UserAuthAPI.Models;
using UserAuthAPI.DTOs;

namespace UserAuthAPI.Mappings;

public class UserDTOMappingProfile : Profile
{
    public UserDTOMappingProfile()
    {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<UserRegisterDTO, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt
            .MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
    }
}
