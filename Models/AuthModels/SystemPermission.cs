namespace ASP_Web_API.Models.AuthModels;

public static class SystemPermission
{
    public const string AdminPermission = "Admin.Access";
    public const string CreatePermission = "Create";
    public const string ReadPermission = "Read";
    public const string UpdatePermission = "Update";
    public const string DeletePermission = "Delete";

    public static readonly List<string> DefaultPermissions = new()
    {
        AdminPermission,
        CreatePermission,
        ReadPermission,
        UpdatePermission,
        DeletePermission
    };
}