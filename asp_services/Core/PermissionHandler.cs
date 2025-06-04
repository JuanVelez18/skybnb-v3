using System.Security.Claims;
using application.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;

namespace asp_services.Core
{
    public class PermissionHandler: AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PermissionHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                context.Fail(new AuthorizationFailureReason(this, "User not authenticated"));
                return;
            }

            var userId = Guid.Parse(userIdClaim.Value);

            using var scope = _serviceScopeFactory.CreateScope();
            var conexion = scope.ServiceProvider.GetRequiredService<DbConexion>();

            var user = await conexion.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                context.Fail(new AuthorizationFailureReason(this, "User not found"));
                return;
            }

            if (user.IsActive == false)
            {
                context.Fail(new AuthorizationFailureReason(this, "User is inactive"));
                return;
            }

            if (!user.HasPermission(requirement.Permission))
            {
                context.Fail(new AuthorizationFailureReason(this, "User does not have the required permission"));
                return;
            }

            context.Succeed(requirement);
        }
    }
}
