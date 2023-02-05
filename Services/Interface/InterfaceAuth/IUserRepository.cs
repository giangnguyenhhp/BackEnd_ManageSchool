using ASP_Web_API.Models.AuthModels;
using ASP_Web_API.Models.AuthModels.Request;
using ASP_Web_API.Models.AuthModels.Request.Users;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Web_API.Services.Interface.InterfaceAuth;

public interface IUserRepository
{
    public Task<List<User>> GetAllUsers();
    public Task<IActionResult> RegisterUser(RegisterUserRequest request);
    public Task<IActionResult> RegisterAdmin(RegisterUserRequest request);
    public Task<IActionResult> UpdateUser(string id, UpdateUserRequest request);
    public Task<IActionResult> DeleteUser(string id);
    public Task<IActionResult> RegisterUserForAdmin(RegisterUserRequest request);
}