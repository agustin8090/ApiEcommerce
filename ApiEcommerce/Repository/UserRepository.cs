using System;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiEcommerce.Models;
using ApiEcommerce.Models.DTOs;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiEcommerce.Repository;

public class UserRepository : IUserRepository
{

    public readonly ApplicationDbContext _db;

    private string? secretKey;

    private readonly UserManager<AplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    public UserRepository(ApplicationDbContext db, IConfiguration configuration, UserManager<AplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
    {
        _db=db;
        secretKey= configuration.GetValue<string>("ApiSettings:SecretKey");
        _userManager=userManager;
        _roleManager=roleManager;
        _mapper=mapper;
    }
   public AplicationUser? GetUser(string  id)
    {
     return _db.AplicationUsers.FirstOrDefault(u => u.Id == id);
    }

    public ICollection<AplicationUser> GetUsers()
    {
        return _db.AplicationUsers.OrderBy(u=> u.UserName).ToList();
    }

    public async Task<bool> IsUniqueUser(string username)
    {
        return ! await _db.Users.AnyAsync(u => u.Username.ToLower().Trim() == username.ToLower().Trim());
    }

    public async Task<UserLoginResponseDto> Login(UserLoginDto userLogindto)
    {
        
        if (string.IsNullOrEmpty(userLogindto.Username)){
            return new UserLoginResponseDto()
            {
                
                Token= "",
                User= null,
                Message= "Username es requerido"
            };
        }

        var user = _db.AplicationUsers.FirstOrDefault<AplicationUser>(u => u.UserName != null && u.UserName.ToLower().Trim() == userLogindto.Username.ToLower().Trim());
        if (user == null)
        {
            return new UserLoginResponseDto()
            {
                
                Token= "",
                User= null,
                Message= "Username no encontrado"
            };
        }

        if(userLogindto.Password == null)
        {
            return new UserLoginResponseDto()
            {
                
                Token= "",
                User= null,
                Message= "Password es requerido"
            };
        }

        bool isvalid= await _userManager.CheckPasswordAsync(user, userLogindto.Password);


        if(!isvalid)
        {
            return new UserLoginResponseDto()
            {
                
                Token= "",
                User= null,
                Message= "Credenciales incorrectas"
            };
        }

        
        //generamos token JWT
        var handlerToken= new JwtSecurityTokenHandler();
        if(string.IsNullOrWhiteSpace(secretKey))
        {
            throw new InvalidOperationException("Secret key no configurada");
        }
        var roles= await _userManager.GetRolesAsync(user);
        var key= Encoding.UTF8.GetBytes(secretKey);

        var tokenDescriptor= new SecurityTokenDescriptor
        {
            Subject= new ClaimsIdentity(new Claim[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty)
            }),
            Expires= DateTime.UtcNow.AddHours(2),
            SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token= handlerToken.CreateToken(tokenDescriptor);

        return new UserLoginResponseDto()
        {
            Token= handlerToken.WriteToken(token),
            User= _mapper.Map<UserDataDto>(user),
            Message= "Usuario logueado correctamente"
        };

    }

    async Task<UserDataDto> IUserRepository.Register(CreateUserDto createuserdto)
    {
         if (string.IsNullOrEmpty(createuserdto.Username)){
            throw new ArgumentNullException("Username es requerido");
        }

        

        if(createuserdto.Password == null)
        {
            throw new ArgumentNullException("Password es requerido");
        }

        var user= new AplicationUser()
        {
            UserName= createuserdto.Username,
            Email= createuserdto.Username,
            NormalizedEmail= createuserdto.Username.ToUpper(),
            Name= createuserdto.Name

        };  
        
        var result= await _userManager.CreateAsync(user, createuserdto.Password);
        
        if(result.Succeeded)
        {
            var userRole= createuserdto.Role ?? "User";
            var roleExists= await _roleManager.RoleExistsAsync(userRole);
            if(!roleExists)
            {
                var identityRole= new IdentityRole(userRole);
                await _roleManager.CreateAsync(identityRole);
            }
            await _userManager.AddToRoleAsync(user, userRole);

            var createdUser= _db.AplicationUsers.FirstOrDefault(u => u.UserName == createuserdto.Username);
            return _mapper.Map<UserDataDto>(createdUser);
        }
        var errors= string.Join(", ", result.Errors.Select(e => e.Description));
        throw new ApplicationException($"No se puedo realizar el registro del usuario. Errores: {errors}");
    
    }
}
