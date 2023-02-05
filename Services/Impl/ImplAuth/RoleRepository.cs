using System.Security.Claims;
using ASP_Web_API.Models;
using ASP_Web_API.Models.AuthModels;
using ASP_Web_API.Models.AuthModels.Request.Roles;
using ASP_Web_API.Services.Interface.InterfaceAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_API.Services.Impl.ImplAuth;

public class RoleRepository : ControllerBase, IRoleRepository
{
    private readonly RoleManager<Role> _roleManager;
    private readonly MasterDbContext _dbContext;

    public RoleRepository(RoleManager<Role> roleManager, MasterDbContext dbContext)
    {
        _roleManager = roleManager;
        _dbContext = dbContext;
    }

    public async Task<List<Role>> GetAllRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        foreach (var role in roles)
        {
            var listClaims = await _roleManager.GetClaimsAsync(role);
            var claims = await _dbContext.RoleClaims
                .Where(x => x.RoleId == role.Id).ToListAsync();
            role.RoleClaims = claims;
        }

        return roles;
    }

    public Task<List<string>> GetPermissions()
    {
        return Task.FromResult(SystemPermission.DefaultPermissions.ToList());
    }

    public async Task<IActionResult> MapPermissions(string id, MapPermissionRequest request)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return Ok(new Response()
            {
                Status = "Error",
                Message = "Role not found."
            });
        }

        var systemPermissions = SystemPermission.DefaultPermissions;
        //Lấy ra các claims cũ trong role và xóa đi
        var listClaims = await _roleManager.GetClaimsAsync(role);
        foreach (var claim in listClaims)
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }

        //Add các claim mới vào role
        if (request.ListClaims == null || !request.ListClaims.Any())
            return Ok(StatusCode(StatusCodes.Status200OK, new Response()
            {
                Status = "Success",
                Message = "None Permissions Added."
            }));

        foreach (var claim in request.ListClaims)
        {
            if (!systemPermissions.Contains(claim)) //Kiểm tra các Permission nhập vào có đúng không
            {
                throw new Exception("Permission is not existed!");
            }

            await _roleManager.AddClaimAsync(role, new Claim(ClaimTypes.Role, claim));
        }

        return Ok(StatusCode(StatusCodes.Status200OK, new Response()
        {
            Status = "Success",
            Message = "Permission added."
        }));
    }


    public async Task<IActionResult> CreateRole(RoleRequest request)
    {
        var checkRole = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == request.Name);
        if (checkRole != null)
        {
            return StatusCode(StatusCodes.Status200OK, new Response()
            {
                Status = "Error",
                Message = $"Role {request.Name} already existed !"
            });
        }

        var newRole = new Role()
        {
            Name = request.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };

        await _roleManager.CreateAsync(newRole);
        return Ok(StatusCode(StatusCodes.Status201Created, new Response()
        {
            Status = "Success",
            Message = $"Role {request.Name} created successfully !"
        }));
    }

    public async Task<IActionResult> UpdateRole(RoleRequest request, string id)
    {
        var checkRole = await _roleManager.FindByIdAsync(id);
        if (checkRole == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new Response()
            {
                Status = "Error",
                Message = $"Role {request.Name} not found"
            });
        }

        checkRole.Name = request.Name;
        await _roleManager.UpdateAsync(checkRole);
        return Ok(StatusCode(StatusCodes.Status200OK, new Response()
        {
            Status = "Success",
            Message = $"Role {request.Name} updated successfully !"
        }));
    }

    public async Task<IActionResult> DeleteRole(string id)
    {
        var checkRole = await _roleManager.FindByIdAsync(id);
        if (checkRole == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new Response()
            {
                Status = "Error",
                Message = $"Role {id} not found"
            });
        }

        await _roleManager.DeleteAsync(checkRole);
        return Ok(StatusCode(StatusCodes.Status204NoContent, new Response()
        {
            Status = "Success",
            Message = "Role deleted successfully !"
        }));
    }
}