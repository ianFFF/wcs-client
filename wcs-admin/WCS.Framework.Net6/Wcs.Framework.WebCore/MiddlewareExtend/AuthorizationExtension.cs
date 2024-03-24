using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs.Framework.WebCore.AuthorizationPolicy;

namespace Wcs.Framework.WebCore.MiddlewareExtend
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddAuthorizationService(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyName.Sid, polic =>
                {
                    polic.AddRequirements(new CustomAuthorizationRequirement(PolicyEnum.MenuPermissions));
                });
            });

            services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
            return services;
        }
    }
}
