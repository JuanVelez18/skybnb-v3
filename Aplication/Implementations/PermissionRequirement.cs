using Microsoft.AspNetCore.Authorization;

namespace application.Implementations
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
