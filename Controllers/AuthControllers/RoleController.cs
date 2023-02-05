using ASP_Web_API.Models.AuthModels.Request.Roles;
using ASP_Web_API.Services.Interface.InterfaceAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Web_API.Controllers.AuthControllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;

    public RoleController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllRoles()
    {
        return Ok(await _roleRepository.GetAllRoles());
    }

    [HttpGet("permissions")]
    public async Task<ActionResult> GetPermissions()
    {
        return Ok(await _roleRepository.GetPermissions());
    }

    [HttpPost("map-permission/{id}")]
    public async Task<ActionResult> MapPermissions(string id,[FromBody]MapPermissionRequest request)
    {
        return Ok(await _roleRepository.MapPermissions(id, request));
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleRequest request)
    {
        return Ok(await _roleRepository.CreateRole(request));
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateRole([FromBody] RoleRequest request, string id)
    {
        return Ok(await _roleRepository.UpdateRole(request,id));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        return Ok(await _roleRepository.DeleteRole(id));
    }
}