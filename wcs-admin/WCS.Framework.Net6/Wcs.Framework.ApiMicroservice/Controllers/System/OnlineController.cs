using Hei.Captcha;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wcs.Framework.Common.Const;
using Wcs.Framework.Common.Enum;
using Wcs.Framework.Common.Helper;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Core;
using Wcs.Framework.DTOModel;
using Wcs.Framework.Interface;
using Wcs.Framework.Model.Models;
using Wcs.Framework.Repository;
using Wcs.Framework.WebCore;
using Wcs.Framework.WebCore.AttributeExtend;
using Wcs.Framework.WebCore.AuthorizationPolicy;
using Wcs.Framework.WebCore.SignalRHub;

namespace Wcs.Framework.ApiMicroservice.Controllers
{
    /// <summary>
    /// 在线管理
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class OnlineController : ControllerBase
    {
        private ILogger<OnlineController> _logger;
        private IHubContext<MainHub> _hub;
        public OnlineController(ILogger<OnlineController> logger, IHubContext<MainHub> hub)
        {
            _logger = logger;
            _hub = hub;
        }

        /// <summary>
        /// 动态条件获取当前在线用户
        /// </summary>
        /// <param name="online"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public Result PageList([FromQuery] OnlineUser online, [FromQuery] PageParModel page)
        {
            var data = MainHub.clientUsers;
            IEnumerable<OnlineUser> dataWhere = data.AsEnumerable();

            if (!string.IsNullOrEmpty(online.Ipaddr))
            {
                dataWhere = dataWhere.Where((u) => u.Ipaddr.Contains(online.Ipaddr));
            }
            if (!string.IsNullOrEmpty(online.UserName))
            {
                dataWhere = dataWhere.Where((u) => u.UserName.Contains(online.UserName));
            }
            return Result.Success().SetData(new PageModel<List<OnlineUser>>() { Total = data.Count, Data = dataWhere.ToList() });
        }


        /// <summary>
        /// 强制退出用户
        /// </summary>
        /// <param name="connnectionId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{connnectionId}")]
        public async Task<Result> ForceOut(string connnectionId)
        {
            if (MainHub.clientUsers.Exists(u => u.ConnnectionId == connnectionId))
            {
                //前端接受到这个事件后，触发前端自动退出
                await _hub.Clients.Client(connnectionId).SendAsync(HubTypeEnum.forceOut.ToString(),"你已被强制退出！");
                return Result.Success(); 
            }
            return Result.Error("操作失败！未发现该连接！");
        }
    }
}
