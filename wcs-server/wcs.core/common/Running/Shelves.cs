using System;
using S7.Net;
using wcs.core.@interface;
using wcs.core.model;

namespace wcs.core.common.Running
{
    /// <summary>
    /// 货架对象
    /// </summary>
    public class Shelves : IEquipmentObject
    {
        public string id { get; set; }
        public string relation_object_id { get; set; }
        public string type { get; set; }
        public string IOPoint { get; set; }
        public Point point { get; set; }
        public int status { get; set; }
        public Size size { get; set; }
        public string shelves_id { get; set; }
        public int cur_loaded { get; set; }
        public string deep { get; set; }
        public string name { get; set; }
        public bool is_busy { get; set; }

        public Shelves()
        {
            point = new Point();
            size = new Size() { height = "1", width = "1" };
        }

        public bool CloseConnect() { return true; }
        /// <summary>
        /// 输送线货物就位信号
        /// </summary>
        /// <returns></returns>
        public bool GoodIsReady() { return true; }

        /// <summary>
        /// 堆垛机是否忙碌
        /// </summary>
        public bool CanMove() { return true; }

        /// <summary>
        /// 堆垛机入库信号
        /// </summary>
        /// <param name="p"></param>
        public bool MoveInSignal(Point p) { return true; }

        /// <summary>
        /// 堆垛机出库信号
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool MoveOutSignal(Point p) { return true; }

        /// <summary>
        /// 堆垛机获取当前位置
        /// </summary>
        /// <returns></returns>
        public Point? GetDDJCurPoint() { return new Point(); }

        /// <summary>
        /// 是否能链接
        /// </summary>
        /// <returns></returns>
        public bool CanConnect() { return true; }
    }
}

