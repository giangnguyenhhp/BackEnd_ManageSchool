using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ASP_Web_API.Models.AuthModels;
using ASP_Web_API.Models.AuthModels.Request.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ASP_Web_API.Controllers.AuthControllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly SignInManager<User> _signInManager;

    public AuthenticateController(RoleManager<Role> roleManager, UserManager<User> userManager,
        IConfiguration configuration, SignInManager<User> signInManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _configuration = configuration;
        _signInManager = signInManager;
    }

    //Method GetToken when login
    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? string.Empty));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            claims: authClaims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var checkUser = await _userManager.FindByNameAsync(request.UserName);
        if (checkUser == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new Response()
            {
                Status = "Error",
                Message = "User not found"
            });
        }

        if (!await _userManager.CheckPasswordAsync(checkUser, request.Password)) return Unauthorized();
        
        //Lấy ra tên các role của user đăng nhập:
        var roleNames = await _userManager.GetRolesAsync(checkUser);
        //Lấy ra list các role từ list roleNames ở trên:
        var listRoles = await _roleManager.Roles
            .Where(x => x.Name != null && roleNames.Contains(x.Name)).ToListAsync();

        if (checkUser.UserName == null) return Unauthorized();

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, checkUser.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        //Add các roleNames của user đăng nhập vào authClaims:
        authClaims.AddRange(roleNames.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        //Lấy ra các claims trong các role của user để add vào authClaims:
        var listClaims = new List<Claim>();
        foreach (var role in listRoles)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            if (claims.Any())
            {
                listClaims.AddRange(claims);
            }
        }

        if (listClaims.Any())
        {
            authClaims.AddRange(listClaims);
        }

        //Tạo token khi đăng nhập
        var token = GetToken(authClaims);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo,
            claims = listClaims.Select(x => x.Value),
            user = checkUser
        });
    }
}