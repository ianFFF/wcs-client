using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wcs.Framework.Common.IOCOptions;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Common.QueueModel;
using Wcs.Framework.Core;
using Wcs.Framework.Interface;

namespace Wcs.Framework.ElasticSearchProcessor
{
    public class WarmupESIndexWorker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<WarmupESIndexWorker> _logger;
        private readonly RabbitMQInvoker _RabbitMQInvoker;
        private readonly ElasticSearchInvoker _elasticSearchInvoker;
        private readonly IOptionsMonitor<ElasticSearchOptions> _ElasticSearchOptions = null;


        public WarmupESIndexWorker(ILogger<WarmupESIndexWorker> logger, RabbitMQInvoker rabbitMQInvoker, IConfiguration configuration, ElasticSearchInvoker elasticSearchInvoker, IOptionsMonitor<ElasticSearchOptions> optionsMonitor)
        {
            this._logger = logger;
            this._RabbitMQInvoker = rabbitMQInvoker;
            this._configuration = configuration;
            this._elasticSearchInvoker = elasticSearchInvoker;
            this._ElasticSearchOptions = optionsMonitor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RabbitMQConsumerModel rabbitMQConsumerModel = new RabbitMQConsumerModel()
            {
                ExchangeName = RabbitMQExchangeQueueName.SKUWarmup_Exchange,
                QueueName = RabbitMQExchangeQueueName.SKUWarmup_Queue_ESIndex
            };
            HttpClient _HttpClient = new HttpClient();
            this._RabbitMQInvoker.RegistReciveAction(rabbitMQConsumerModel, message =>
            {
                //SKUWarmupQueueModel skuWarmupQueueModel = JsonConvert.DeserializeObject<SKUWarmupQueueModel>(message);
                //���õ���Ϣ����ģ�͡�
                #region ��ɾ��Index---�½�Index---�ٽ���ȫ����������
                {
                    try
                    {
                        this._elasticSearchInvoker.DropIndex(this._ElasticSearchOptions.CurrentValue.IndexName);
                        //this._searchService.ImpDataBySpu();
                        //������es���ݵ������
                        this._logger.LogInformation($"{nameof(WarmupESIndexWorker)}.InitAll succeed");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        var logModel = new LogModel()
                        {
                            OriginalClassName = this.GetType().FullName,
                            OriginalMethodName = nameof(ExecuteAsync),
                            Remark = "��ʱ��ҵ������־"
                        };
                        this._logger.LogError(ex, $"{nameof(WarmupESIndexWorker)}.Warmup ESIndex failed message={message}, Exception:{ex.Message}", JsonConvert.SerializeObject(logModel));
                        return false;
                    }
                }
                #endregion
            });
            await Task.CompletedTask;
        }
    }
}
