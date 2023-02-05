using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ASP_Web_API.Models.AuthModels;

public class User : IdentityUser
{
    [NotMapped] public ICollection<Role> Roles { get; set; }
}