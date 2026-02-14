using System;
using ApiEcommerce.Models;
using ApiEcommerce.Models.DTOs;

namespace ApiEcommerce.Repository.IRepository;

public interface IUserRepository
{
ICollection<AplicationUser> GetUsers();
AplicationUser? GetUser(string userId);

  Task<bool> IsUniqueUser(string username);
 Task<UserLoginResponseDto> Login(UserLoginDto userLogindto);
 Task<UserDataDto> Register(CreateUserDto createuserdto);

}
