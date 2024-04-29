using System;
using wcs.core.@interface;
using wcs.core.model;

namespace wcs.core.common.Running
{
    /// <summary>
    /// 出库操作
    /// </summary>
    public class MoveOut
    {
        private const string SSX_IS_READY = "SSX_IS_READY";
        private const string DDJ_IS_READY = "DDJ_IS_READY";
        private const string DDJ_CK = "DDJ_CK";

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
        public MoveOut(IEquipmentObject issx, IEquipmentObject iddj, ShelvesPosition ihw)
        {
            // 入库操作队列
            // 1、输送线监测货物
            // 2、堆垛机是否空闲
            // 3、堆垛机出库指令
            // 4、堆垛机是否空闲
            this.taskQueue = new Queue<string>();
            this.taskQueue.Enqueue(SSX_IS_READY);
            this.taskQueue.Enqueue(DDJ_IS_READY);
            this.taskQueue.Enqueue(DDJ_CK);
            this.taskQueue.Enqueue(DDJ_IS_READY);

            this._ssx = issx;
            this._ddj = iddj;
            this._hw = ihw;
        }

        /// <summary>
        /// 出库操作开始
        /// </summary>
        /// <param name="result"></param>
        public void Start(out RunningResult result)
        {
            result = new RunningResult();
            _ssx.is_busy = true;
            _ddj.is_busy = true;

            do
            {
                if (this.taskQueue.Count > 0)
                {
                    // 常驻：先读异常块
                    //if (false)
                    //{
                    //    result.result = false;
                    //    result.message = "error";
                    //    break;
                    //}

                    bool isReady = false;

                    string actionType = this.taskQueue.First();
                    switch (actionType)
                    {
                        case SSX_IS_READY:
                            {
                                isReady = _ssx.GoodIsReady();
                            }
                            break;
                        case DDJ_IS_READY:
                            {
                                isReady = _ddj.CanMove();
                            }
                            break;
                        case DDJ_CK:
                            {
                                //isReady = _ddj.MoveInSignal(new Point(_hw.point_x.ToString(), _hw.point_y.ToString(), _hw.point_z.ToString()));
                            }
                            break;

                        default:
                            break;
                    }
                    if (isReady)
                    {
                        this.taskQueue.Dequeue();
                    }
                }
            } while (this.taskQueue.Count > 0);

            _ssx.is_busy = false;
            _ddj.is_busy = false;
        }
    }
}

