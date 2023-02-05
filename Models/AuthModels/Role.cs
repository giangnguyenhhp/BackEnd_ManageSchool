using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ASP_Web_API.Models.AuthModels;

public class Role : IdentityRole
{
    [NotMapped] public ICollection<User> Users { get; set; }
    [NotMapped] public ICollection<IdentityRoleClaim<string>> RoleClaims { get; set; }
}