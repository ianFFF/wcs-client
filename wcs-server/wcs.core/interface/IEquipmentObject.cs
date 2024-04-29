using System;
using wcs.core.model;

namespace wcs.core.@interface
{
    public interface IEquipmentObject
    {
        public const string SSX = "SSX";
        public const string DDJ = "DDJ";
        public const string HW = "HW";

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

        /// <summary>
        /// 输送线货物就位信号
        /// </summary>
        /// <returns></returns>
        public bool GoodIsReady();

        /// <summary>
        /// 堆垛机是否忙碌
        /// </summary>
        /// <returns></returns>
        public bool CanMove();

        /// <summary>
        /// 堆垛机入库信号
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool MoveInSignal(Point p);

        /// <summary>
        /// 堆垛机出库信号
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool MoveOutSignal(Point p);

        /// <summary>
        /// 堆垛机获取当前位置
        /// </summary>
        /// <returns></returns>
        public Point? GetDDJCurPoint();

        /// <summary>
        /// 是否能链接
        /// </summary>
        /// <returns></returns>
        public bool CanConnect();

        public bool CloseConnect();
    }
}

