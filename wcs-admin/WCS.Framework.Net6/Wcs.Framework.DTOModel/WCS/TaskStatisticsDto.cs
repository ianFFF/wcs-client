using System;
namespace Wcs.Framework.DTOModel
{
    /// <summary>
    /// 统计属性
    /// </summary>
    public class StatisticsItem
    {
        public int rkCount { get; set; }
        public int ckCount { get; set; }
    }

    public class TaskStatisticsDto
    {
        public StatisticsItem WeeklyStatistics { get; set; }
        public StatisticsItem MonthStatistics { get; set; }
        public StatisticsItem YearStatistics { get; set; }
    }
}

