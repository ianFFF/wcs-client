using System;
using System.Reflection;
//using wcs_console_server.@interface;

namespace wcs.webapi
{
    public class WebApi : IWebApi
    {
        private static readonly string AssemblyName = typeof(WebApi).Assembly.GetName().Name;

        public void Init()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddControllers();

            builder.WebHost.UseUrls("http://*:5005");

            builder.Services.AddControllers()
                .AddApplicationPart(Assembly.Load(new AssemblyName(AssemblyName)));

            var app = builder.Build();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.Run();
        }
    }
}

