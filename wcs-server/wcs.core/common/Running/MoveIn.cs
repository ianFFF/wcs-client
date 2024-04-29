using System;
using wcs.core.@interface;
using wcs.core.model;

namespace wcs.core.common.Running
{
    /// <summary>
    /// 入库操作
    /// </summary>
    public class MoveIn
    {
        private const string SSX_IS_READY = "SSX_IS_READY";
        private const string DDJ_IS_READY = "DDJ_IS_READY";
        private const string DDJ_RK = "DDJ_RK";

        /// <summary>
        /// 输送线
        /// </summary>
        private IEquipmentObject _ssx;

        /// <summary>
        /// 堆垛机
        /// </summary>
        private IEquipmentObject _ddj;

        /// <summary>
        /// 货位
        /// </summary>
        private ShelvesPosition _hw;

        private Queue<string> taskQueue;
        /// <summary>
        /// 构造，初始化任务队列
        /// </summary>
        public MoveIn(IEquipmentObject issx, IEquipmentObject iddj, ShelvesPosition ihw)
        {
            // 入库操作队列
            // 1、输送线监测货物
            // 2、堆垛机是否空闲
            // 3、堆垛机入库指令
            // 4、堆垛机是否空闲
            this.taskQueue = new Queue<string>();
            this.taskQueue.Enqueue(SSX_IS_READY);
            this.taskQueue.Enqueue(DDJ_IS_READY);
            this.taskQueue.Enqueue(DDJ_RK);
            this.taskQueue.Enqueue(DDJ_IS_READY);

            this._ssx = issx;
            this._ddj = iddj;
            this._hw = ihw;
        }

        /// <summary>
        /// 入库操作开始
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="result"></param>
        public void Start(string taskId, out RunningResult result)
        {
            result = new RunningResult();
            _ssx.is_busy = true;
            _ddj.is_busy = true;

            CConsole.WriteLine($"堆垛机:{_ddj.name}开始运行入库", OutPut.normal, "", taskId);
            RunningPoints.instance.Rk(new Point($"{_hw.point_x}", $"{_hw.point_y}", $"{_hw.point_z}"));
            //do
            //{
            //    if (this.taskQueue.Count > 0)
            //    {
            //        // 常驻：先读异常块
            //        //if (false)
            //        //{
            //        //    result.result = false;
            //        //    result.message = "error";
            //        //    break;
            //        //}

            //        bool isReady = false;

            //        string actionType = this.taskQueue.First();
            //        switch (actionType)
            //        {
            //            case SSX_IS_READY:
            //                {
            //                    isReady = _ssx.GoodIsReady();
            //                }
            //                break;
            //            case DDJ_IS_READY:
            //                {
            //                    isReady = _ddj.CanMove();
            //                }
            //                break;
            //            case DDJ_RK:
            //                {
            //                    isReady = _ddj.MoveInSignal(new Point(_hw.point_x.ToString(), _hw.point_y.ToString(), _hw.point_z.ToString()));
            //                }
            //                break;

            //            default:
            //                break;
            //        }
            //        if (isReady)
            //        {
            //            this.taskQueue.Dequeue();
            //        }
            //    }
            //} while (this.taskQueue.Count > 0);
            CConsole.WriteLine($"堆垛机:{_ddj.name}入库操作完毕", OutPut.normal, "", taskId);

            _ssx.is_busy = false;
            _ddj.is_busy = false;
        }
    }
}

