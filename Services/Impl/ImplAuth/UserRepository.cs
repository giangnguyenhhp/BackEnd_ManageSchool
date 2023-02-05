using ASP_Web_API.Models;
using ASP_Web_API.Models.AuthModels;
using ASP_Web_API.Models.AuthModels.Request;
using ASP_Web_API.Models.AuthModels.Request.Users;
using ASP_Web_API.Services.Interface.InterfaceAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_API.Services.Impl.ImplAuth;

public class UserRepository : ControllerBase, IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly MasterDbContext _dbContext;

    public UserRepository(UserManager<User> userManager, RoleManager<Role> roleManager, MasterDbContext dbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
    }

    public async Task<List<User>> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var listRoles = await _roleManager.Roles
                .Where(r => r.Name != null && roles.Contains(r.Name))
                .ToListAsync();
            user.Roles = listRoles;
        }

        return users;
    }

    public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
    {
        var checkUser = await _userManager.FindByNameAsync(request.UserName);
        if (checkUser != null)
        {
            return StatusCode(StatusCodes.Status200OK,
                new Response()
                {
                    Message = "User already exists",
                    Status = "Error"
                });
        }

        var newUser = new User()
        {
            Id = Guid.NewGuid().ToString(),
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = request.Email,
            UserName = request.UserName,
            PhoneNumber = request.PhoneNumber
        };
        //Tạo user mới bằng method : _userManager.CreateAsync(user,password)
        var result = await _userManager.CreateAsync(newUser, request.Password);

        //Kiểm tra xem đã có Role User chưa,nếu chưa có thì tạo Role User
        if (!await _roleManager.RoleExistsAsync("User"))
            await _roleManager.CreateAsync(new Role()
            {
                Name = "User",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            });

        //Nếu đã có role User thì add role User cho user mặc định
        if (await _roleManager.RoleExistsAsync("User"))
            await _userManager.AddToRoleAsync(newUser, "User");

        return !result.Succeeded
            ? StatusCode(StatusCodes.Status500InternalServerError, new Response()
            {
                Status = "Error",
                Message = "User creation failed ! Please check user details and try again ! "
            })
            : Ok(new Response()
            {
                Status = "Success",
                Message = "User created successfully!"
            });
    }

    public async Task<IActionResult> RegisterAdmin(RegisterUserRequest request)
    {
        var checkUser = await _userManager.FindByNameAsync(request.UserName);
        if (checkUser != null)
        {
            return StatusCode(StatusCodes.Status200OK,
                new Response()
                {
                    Status = "Error",
                    Message = "User already exists"
                });
        }

        var newUser = new User()
        {
            Id = Guid.NewGuid().ToString(),
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        //Khởi tạo user
        var result = await _userManager.CreateAsync(newUser, request.Password);

        //Kiểm tra xem trong Roles đã có role Admin chưa,nếu chưa có sẽ tạo role Admin
        if (!await _roleManager.RoleExistsAsync("Admin"))
            await _roleManager.CreateAsync(new Role()
            {
                Name = "Admin",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            });

        //Nếu đã có role Admin thì add role Admin cho userAdmin vừa tạo
        if (await _roleManager.RoleExistsAsync("Admin"))
            await _userManager.AddToRoleAsync(newUser, "Admin");

        //Kiểm tra kết quả tạo User
        return !result.Succeeded
            ? StatusCode(StatusCodes.Status500InternalServerError, new Response()
            {
                Status = "Error",
                Message = "User creation failed ! Please check user details and try again !"
            })
            : Ok(new Response()
            {
                Status = "Success",
                Message = "User created successfully"
            });
    }

    public async Task<IActionResult> RegisterUserForAdmin(RegisterUserRequest request)
    {
        var checkUser = await _userManager
            .FindByNameAsync(request.UserName);
        if (checkUser != null)
        {
            return StatusCode(StatusCodes.Status200OK, new Response()
            {
                Status = "Error",
                Message = "User already exists"
            });
        }

        var newUser = new User()
        {
            UserName = request.UserName,
            Email = request.Email,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Id = Guid.NewGuid().ToString(),
            PhoneNumber = request.PhoneNumber,
        };

        var result = await _userManager.CreateAsync(newUser, request.Password);

        //Kiểm tra xem role vừa nhập có trong database không
        var listRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        if (request.RoleNames != null && request.RoleNames.Any(roleName => !listRoles.Contains(roleName)))
        {
            throw new Exception("Role does not existed");
        }

        //Add các role cần thiết cho user vừa khởi tạo
        if (request.RoleNames != null) await _userManager.AddToRolesAsync(newUser, request.RoleNames);

        return !result.Succeeded
            ? StatusCode(StatusCodes.Status500InternalServerError, new Response()
            {
                Status = "Error",
                Message = "Create user failed ! Please check user details and try again !"
            })
            : Ok(new Response()
            {
                Status = "Success",
                Message = "User created successfully !"
            });
    }

    public async Task<IActionResult> UpdateUser(string id, UpdateUserRequest request)
    {
        if (request.UserName == null)
        {
            throw new Exception("User name cannot be empty");
        }

        var checkUser = await _userManager.FindByNameAsync(request.UserName);
        if (checkUser == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new Response()
            {
                Status = "Error",
                Message = "User not found"
            });
        }

        checkUser.UserName = request.UserName;
        checkUser.Email = request.Email;
        checkUser.PhoneNumber = request.PhoneNumber;

        //Thay đổi password
        if (request is { CurrentPassword: { }, NewPassword: { } })
        {
            if (await _userManager.CheckPasswordAsync(checkUser, request.CurrentPassword))
            {
                await _userManager.ChangePasswordAsync(checkUser, request.CurrentPassword, request.NewPassword);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response()
                {
                    Status = "Error",
                    Message = "Current password is wrong"
                });
            }
        }

        //Update Role cho user
        var roleNames = await _userManager.GetRolesAsync(checkUser);
        await _userManager.RemoveFromRolesAsync(checkUser, roleNames);
        if (request.RoleNames != null && request.RoleNames.Any())
        {
            await _userManager.AddToRolesAsync(checkUser, request.RoleNames);
        }

        await _userManager.UpdateAsync(checkUser);
        return Ok(new Response()
        {
            Status = "Success",
            Message = "User updated successfully"
        });
    }

    public async Task<IActionResult> DeleteUser(string id)
    {
        var checkUser = await _userManager.FindByIdAsync(id);
        if (checkUser == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new Response()
            {
                Status = "Error",
                Message = "User not found"
            });
        }

        await _userManager.DeleteAsync(checkUser);
        return Ok(new Response()
        {
            Status = "Success",
            Message = "User deleted successfully"
        });
    }
}