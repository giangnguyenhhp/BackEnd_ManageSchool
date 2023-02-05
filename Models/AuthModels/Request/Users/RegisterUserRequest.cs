using System.ComponentModel.DataAnnotations;

namespace ASP_Web_API.Models.AuthModels.Request.Users;

public class RegisterUserRequest
{
    [Required(ErrorMessage = "Yêu cầu nhập Email")]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required(ErrorMessage="Yêu cầu nhập tên đăng nhập")]
    public string UserName { get; set; }

    [Required(ErrorMessage="Yêu cầu nhập mật khẩu")]
    public string Password { get; set; }

    [Required(ErrorMessage="Yêu cầu nhập số điện thoại")]
    [Phone]
    public string PhoneNumber { get; set; }

    [Compare("Password", ErrorMessage="Mật khẩu lặp lại không chính xác")]
    public string ConfirmPassword { get; set; }

    public List<string>? RoleNames { get; set; }
}