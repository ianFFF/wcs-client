using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Ocelot.Cache.CacheManager;
using Wcs.Framework.WebCore.MiddlewareExtend;
using Ocelot.Provider.Polly;

namespace Wcs.Framework.OcelotGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            #region
            //�����������
            #endregion
            services.AddCorsService();

            #region
            //���ط�������
            #endregion
            services.AddOcelot().AddConsul().AddCacheManager(x => { x.WithDictionaryHandle(); }).AddPolly();

            #region
            //Swagger��������
            #endregion
            services.AddSwaggerService<Program>("Wcs.Framework.OcelotGateway");
            #region
            //Jwt��Ȩ����
            #endregion
            services.AddJwtService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                #region
                //Swagger����ע��
                #endregion
                app.UseSwaggerService(new SwaggerModel("api/api/swagger/v1/swagger.json", "API����"), new SwaggerModel("api/item/swagger/v1/swagger.json", "��̬ҳ����"));
            }
            #region
            //���ط���ע��
            #endregion
            app.UseOcelot();

            #region
            //��Ȩע��
            #endregion
            app.UseAuthentication();
        }
    }
}
