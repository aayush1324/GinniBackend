using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos
{
    public class PolicyHandler
    {
        public static void AddCustomPolicies(IServiceCollection services)
        {

            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim("Role");
                    policy.AddRequirements(new CustomAuthorizationRequirement("Admin"));
                });
            });

            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("User", policy =>
                {
                    policy.RequireClaim("Role");
                    policy.AddRequirements(new CustomAuthorizationRequirement("User"));
                });
            });
        }
    }
}
