using ASP_Web_API.Models.AuthModels;
using ASP_Web_API.Models.AuthModels.Request.Roles;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Web_API.Services.Interface.InterfaceAuth;

public interface IRoleRepository
{
    public Task<List<Role>> GetAllRoles();
    public Task<IActionResult> CreateRole(RoleRequest request);
    public Task<IActionResult> UpdateRole(RoleRequest request, string id);
    public Task<IActionResult> DeleteRole(string id);
    public Task<List<string>> GetPermissions();
    public Task<IActionResult> MapPermissions(string id, MapPermissionRequest request);
}