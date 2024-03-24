﻿using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Core;

namespace Wcs.Framework.WebCore.MiddlewareExtend
{
  public static class CacheInitExtend
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CacheInitExtend));
        public  static  void UseRedisSeedInitService(this IApplicationBuilder app )
        {

            if (Appsettings.appBool("CacheSeed_Enabled"))
            {
                var _cacheClientDB = app.ApplicationServices.GetService<CacheInvoker>();

                try
                {
                    //RedisInit.Seed(_cacheClientDB);
                }
                catch (Exception e)
                {
                    log.Error($"Error occured seeding the RedisInit.\n{e.Message}");
                    throw;
                }
            }
        }
    }
}
