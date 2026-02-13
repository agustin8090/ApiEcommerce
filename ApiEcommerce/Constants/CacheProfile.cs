using System;
using Microsoft.AspNetCore.Mvc;
namespace ApiEcommerce.Constants;

public class CacheProfiles
{

public  const string Default10  = "Default10";
public const string Default20  = "Default20";
public int Duration { get; set; }

public static readonly CacheProfiles Profile10 = new()
{
    Duration = 10
};
public static readonly CacheProfiles Profile20 = new()
{
    Duration = 20
};

}
