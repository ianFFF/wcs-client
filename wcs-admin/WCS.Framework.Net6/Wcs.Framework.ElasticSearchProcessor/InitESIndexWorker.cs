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
using Wcs.Framework.Common.IOCOptions;
using Wcs.Framework.Common.Models;
using Wcs.Framework.Core;

namespace Wcs.Framework.ElasticSearchProcessor
{
    public class InitESIndexWorker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<InitESIndexWorker> _logger;
        private readonly RabbitMQInvoker _RabbitMQInvoker;
        private readonly ElasticSearchInvoker _elasticSearchInvoker;

        public InitESIndexWorker(ILogger<InitESIndexWorker> logger, RabbitMQInvoker rabbitMQInvoker, IConfiguration configuration, ElasticSearchInvoker elasticSearchInvoker)
        {
            this._logger = logger;
            this._RabbitMQInvoker = rabbitMQInvoker;
            this._configuration = configuration;
            this._elasticSearchInvoker = elasticSearchInvoker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RabbitMQConsumerModel rabbitMQConsumerModel = new RabbitMQConsumerModel()
            {
                ExchangeName = RabbitMQExchangeQueueName.SKUCQRS_Exchange,
                QueueName = RabbitMQExchangeQueueName.SKUCQRS_Queue_ESIndex
            };
            HttpClient _HttpClient = new HttpClient();
            this._RabbitMQInvoker.RegistReciveAction(rabbitMQConsumerModel, message =>
            {
                try
                {
                    //���õ�ģ�͡�
                    //SPUCQRSQueueModel spuCQRSQueueModel = JsonConvert.DeserializeObject<SPUCQRSQueueModel>(message);

                    //���ж���ɾ�ģ�es������Ӧ�Ĳ�����
                    //switch (spuCQRSQueueModel.CQRSType)
                    //{
                    //    case (int)SPUCQRSQueueModelType.Insert:
                    //    case (int)SPUCQRSQueueModelType.Update:
                    //        {
                    //            Goods goods = this._ISearchService.GetGoodsBySpuId(spuCQRSQueueModel.SpuId);
                    //            this._IElasticSearchService.InsertOrUpdata<Goods>(goods);
                    //            break;
                    //        }
                    //    case (int)SPUCQRSQueueModelType.Delete:
                    //        this._IElasticSearchService.Delete<Goods>(spuCQRSQueueModel.SpuId.ToString());
                    //        break;
                    //    default:
                    //        throw new Exception("wrong spuCQRSQueueModel.CQRSType");
                    //}

                    this._logger.LogInformation($"{nameof(InitESIndexWorker)}.Init ESIndex succeed SpuId");
                    return true;
                }
                catch (Exception ex)
                {
                    LogModel logModel = new LogModel()
                    {
                        OriginalClassName = this.GetType().FullName,
                        OriginalMethodName = nameof(ExecuteAsync),
                        Remark = "��ʱ��ҵ������־"
                    };
                    this._logger.LogError(ex, $"{nameof(InitESIndexWorker)}.Init ESIndex failed message={message}, Exception:{ex.Message}", JsonConvert.SerializeObject(logModel));
                    return false;
                }
            });
            await Task.CompletedTask;
        }
    }
}
