namespace ASP_Web_API.Models.AuthModels.Request.Users;

public class UpdateUserRequest
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
    
    public List<string>? RoleNames { get; set; }
}