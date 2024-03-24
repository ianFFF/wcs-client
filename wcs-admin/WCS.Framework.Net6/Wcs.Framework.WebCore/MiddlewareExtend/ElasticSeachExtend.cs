using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using Wcs.Framework.Common.IOCOptions;
using Wcs.Framework.Core;

namespace Wcs.Framework.WebCore.MiddlewareExtend
{
    /// <summary>
    /// Redis扩展
    /// </summary>
    public static class ElasticSeachExtend
    {
        public static IServiceCollection AddElasticSeachService(this IServiceCollection services)
        {
            if (Appsettings.appBool("ElasticSeach_Enabled"))
            {
                services.Configure<ElasticSearchOptions>(Appsettings.appConfiguration("ElasticSeachConn"));
                services.AddTransient<ElasticSearchInvoker>();
            }
            return services;

        }
    }
}
