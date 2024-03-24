using System;
namespace Wcs.Framework.DTOModel
{
    public class EquipmentRecordsExcelDto
    {
        public string Id { get; set; }

        public string? 异常等级 { get; set; }

        public string? 异常类型 { get; set; }

        public string? 异常编码 { get; set; }

        public string? 日志信息 { get; set; }

        public string 任务流水号 { get; set; }

        public string 托盘码 { get; set; }

        public string 执行结果 { get; set; }

        public string? 日志时间 { get; set; }

        public string? 任务发起时间 { get; set; }

        public string? 任务完成时间 { get; set; }
    }
}

