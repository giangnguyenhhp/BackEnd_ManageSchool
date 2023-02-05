using ASP_Web_API.Models.AuthModels;
using ASP_Web_API.Models.AuthModels.Request;
using ASP_Web_API.Models.AuthModels.Request.Users;
using ASP_Web_API.Services.Interface.InterfaceAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Web_API.Controllers.AuthControllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _userRepository.GetAllUsers());
    }

    [HttpPost("register-user")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
    {
        return Ok(await _userRepository.RegisterUser(request));
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserRequest request)
    {
        return Ok(await _userRepository.RegisterAdmin(request));
    }

    [HttpPost("register-user-for-admin")]
    public async Task<IActionResult> RegisterUserForAdmin([FromBody] RegisterUserRequest request)
    {
        return Ok(await _userRepository.RegisterUserForAdmin(request));
    }

    [HttpPut("update-user/{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
    {
        return Ok(await _userRepository.UpdateUser(id, request));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        return Ok(await _userRepository.DeleteUser(id));
    }
}