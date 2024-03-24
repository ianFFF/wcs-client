using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using Wcs.Framework.Common.IOCOptions;
using Wcs.Framework.Core;
using Wcs.Framework.Core.SMS;

namespace Wcs.Framework.WebCore.MiddlewareExtend
{
    /// <summary>
    /// Redis扩展
    /// </summary>
    public static class SMSExtension
    {
        public static IServiceCollection AddSMSService(this IServiceCollection services)
        {
            if (Appsettings.appBool("SMS_Enabled"))
            {
               
                services.Configure<SMSOptions>(Appsettings.appConfiguration("SMS"));
                services.AddTransient<AliyunSMSInvoker>();
            }
            return services;

        }
    }
}
