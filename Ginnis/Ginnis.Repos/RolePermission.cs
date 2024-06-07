using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos
{
    internal class RolePermission:AuthorizationHandler<CustomAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
        {
            var permissionsClaim = context.User.FindFirst("Role")?.Value;

            if (!string.IsNullOrEmpty(permissionsClaim))
            {

                if (permissionsClaim.Contains(requirement.Permission1))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }


    }
    public class CustomAuthorizationRequirement : IAuthorizationRequirement
    {
        public string Permission1 { get; }

        public CustomAuthorizationRequirement(string permission)
        {
            Permission1 = permission;
        }
    }

}
