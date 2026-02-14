using System;
using Microsoft.AspNetCore.Identity;

namespace ApiEcommerce.Models;

public class AplicationUser:IdentityUser
{

public string? Name { get; set; }
}
