using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wcs.Framework.Common.Const;
using Wcs.Framework.Common.IOCOptions;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Common.QueueModel;
using Wcs.Framework.Core;
using Wcs.Framework.Core.ConsulExtend;
using Wcs.Framework.Core.SMS;

namespace Wcs.Framework.SMSProcessor
{
    public class SendWorker : BackgroundService
    {
        private readonly ILogger<SendWorker> _logger;
        private readonly RabbitMQInvoker _RabbitMQInvoker;
        private readonly AliyunSMSInvoker _aliyunSMSInvoker;
        public SendWorker(ILogger<SendWorker> logger, RabbitMQInvoker rabbitMQInvoker, AliyunSMSInvoker aliyunSMSInvoker)
        {
            this._logger = logger;
            this._RabbitMQInvoker = rabbitMQInvoker;
            _aliyunSMSInvoker = aliyunSMSInvoker;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RabbitMQConsumerModel rabbitMQConsumerModel = new RabbitMQConsumerModel()
            {
                ExchangeName = RabbitConst.SMS_Exchange,
                QueueName = RabbitConst.SMS_Queue_Send
            };
            HttpClient _HttpClient = new HttpClient();
            this._RabbitMQInvoker.RegistReciveAction(rabbitMQConsumerModel, message =>
            {
                var data = Common.Helper.JsonHelper.StrToObj<SMSQueueModel>(message);
                try
                {
                    _aliyunSMSInvoker.SendCode(data.code, data.phone);

                    return true;
                }
                catch (Exception ex)
                {
                    var logModel = new LogModel()
                    {
                        OriginalClassName = this.GetType().FullName,
                        OriginalMethodName = nameof(ExecuteAsync),
                        Remark = "��Ϣ���д�����־"
                    };
                    this._logger.LogError(ex, $"{nameof(SendWorker)}, Exception:{ex.Message}", JsonConvert.SerializeObject(logModel));
                    return false;
                }
            });
            await Task.CompletedTask;
        }
    }
}
