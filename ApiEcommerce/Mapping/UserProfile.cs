using System;
using AutoMapper;
namespace ApiEcommerce.Mapping;

using ApiEcommerce.Models;
using Models.DTOs;

public class UserProfile:Profile
{
public UserProfile()
    {
    CreateMap<User, UserDto>().ReverseMap();
    CreateMap<User, CreateUserDto>().ReverseMap();
    CreateMap<User, UserLoginDto>().ReverseMap();
    CreateMap<User, UserLoginResponseDto>().ReverseMap();
    CreateMap<AplicationUser, UserDataDto>().ReverseMap();
    CreateMap<User, UserDto>().ReverseMap();
    }

}
