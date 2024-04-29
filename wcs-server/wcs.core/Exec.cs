using System;
using wcs.core.common;
using wcs.core.common.Running;
using wcs.core.service;
using wcs.core.model;
using wcs.core.model.Dto;
using wcs.core.@interface;

namespace wcs.core
{
    /// <summary>
    /// 执行机
    /// </summary>
    public class Exec
    {
        public const string wait = "等待";
        public const string running = "运行中";
        public const string complete = "结束";
        public const string error = "异常中止";

        public bool isBusy = false;
        public bool isReady = false;

        /// <summary>
        /// 任务类型
        /// </summary>
        private readonly List<string> status = new List<string>()
        {
            // 入库
            "002cfc70-89a2-11ed-a5d9-00163e10b48b",
            // 出库
            "ba45546e-915b-11ed-a5d9-00163e10b48b"
        };

        /// <summary>
        /// 支持的设备类型
        /// </summary>
        private readonly List<string> equipmentType = new List<string>()
        {
            // 输送系统
            "99ae0692-85b3-11ed-a5d9-00163e10b48b",
            // 堆垛机
            "cfee1e01-7ddd-11ed-a5d9-00163e10b48b"
        };

        /// <summary>
        /// 设备操作集合
        /// </summary>
        private List<IEquipmentObject> _all_equipment;
        public List<IEquipmentObject> All_Equipment
        {
            get { return this._all_equipment; }
        }
        /// <summary>
        /// 所有货位信息
        /// </summary>
        private List<ShelvesPosition> _all_shelves_position;
        public List<ShelvesPosition> All_Shelves_Position
        {
            get { return this._all_shelves_position; }
        }

        /// <summary>
        /// 所有货架信息-大屏使用
        /// </summary>
        private List<IEquipmentObject> _all_shelves;
        public List<IEquipmentObject> All_Shelves
        {
            get { return this._all_shelves; }
        }

        /// <summary>
        /// 所有映射点位
        /// </summary>
        private List<PointMapEntity> _pointMaps;
        public List<PointMapEntity> PointMaps
        {
            get { return this._pointMaps; }
        }

        private static Exec _instance;
        private Exec()
        {
            _all_equipment = new List<IEquipmentObject>();
            _all_shelves_position = new List<ShelvesPosition>();
            _all_shelves = new List<IEquipmentObject>();
        }
        public static Exec instance
        {
            get
            {
                if (Exec._instance == null)
                {
                    Exec._instance = new Exec();
                }

                return Exec._instance;
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            InitPointMap();
            InitObject();
        }

        /// <summary>
        /// 初始化点位映射
        /// </summary>
        private void InitPointMap()
        {
            this._pointMaps = Factory<PointMapService>.instance.GetAllPointMap();
            if (this._pointMaps.Count > 0)
            {
                CConsole.WriteLine("点位映射完毕...", OutPut.warning);
            }
            else
            {
                isReady = false;
                CConsole.WriteLine("无点位映射", OutPut.error);
                // todo 记录日志
                LLog.log.Error("无点位映射");
            }
        }

        /// <summary>
        /// 初始化所有设备
        /// </summary>
        public void InitObject()
        {
            isReady = true;
            this._all_equipment.Clear();
            this._all_shelves.Clear();
            this._all_shelves_position.Clear();

            // 获取所有设备初始化进入内存
            List<EquipmentEntity> equipments = Factory<EquipmentService>.instance.GetAllEquipment();

            equipments.ForEach(x =>
            {
                if (x.equipment_type == this.equipmentType[0])
                {
                    // 输送线
                    if (!string.IsNullOrEmpty(x.ip))
                    {
                        IEquipmentObject e = new SiemensPlc(x.ip);

                        e.id = x.Id;
                        e.relation_object_id = string.IsNullOrEmpty(x.relation_object_id) ? "" : x.relation_object_id;
                        e.type = IEquipmentObject.SSX;
                        e.IOPoint = string.IsNullOrEmpty(x.io_point) ? "" : x.io_point;
                        // todo 其他属性初始化进入对象
                        e.point.x = $"{x.point_x}";
                        e.point.y = $"{x.point_y}";
                        e.point.z = $"{x.point_z}";
                        e.point.layout_x = $"{Factory<PointMapService>.instance.GetPoint("x", float.Parse(e.point.x), _pointMaps)}";
                        e.point.layout_y = $"{Factory<PointMapService>.instance.GetPoint("y", float.Parse(e.point.z), _pointMaps)}";
                        e.name = string.IsNullOrEmpty(x.equipment_name) ? "" : x.equipment_name;

                        this._all_equipment.Add(e);
                    }
                }
                else if (x.equipment_type == this.equipmentType[1])
                {
                    // 堆垛机
                    if (!string.IsNullOrEmpty(x.ip))
                    {
                        IEquipmentObject e = new SiemensPlc(x.ip);

                        e.id = x.Id;
                        e.relation_object_id = string.IsNullOrEmpty(x.relation_object_id) ? "" : x.relation_object_id;
                        e.type = IEquipmentObject.DDJ;
                        // todo 其他属性初始化进入对象
                        e.point.x = $"{x.point_x}";
                        e.point.y = $"{x.point_y}";
                        e.point.z = $"{x.point_z}";
                        e.point.layout_x = $"{Factory<PointMapService>.instance.GetPoint("x", float.Parse(e.point.x), _pointMaps)}";
                        e.point.layout_y = $"{Factory<PointMapService>.instance.GetPoint("y", float.Parse(e.point.z), _pointMaps)}";
                        e.name = string.IsNullOrEmpty(x.equipment_name) ? "" : x.equipment_name;

                        this._all_equipment.Add(e);
                    }
                }
            });

            // 获取货架信息初始化进入内存
            List<ShelvesPositionDto> shelves = Factory<ShelvesService>.instance.GetAllSheivesPosition();

            shelves.ForEach(x =>
            {
                IEquipmentObject? singleHj = _all_shelves.Find(item => item.shelves_id == x.shelves_id && int.Parse(item.point.x) == x.point_x && int.Parse(item.point.z) == x.point_z);
                if (singleHj == null)
                {
                    singleHj = new Shelves();
                    singleHj.id = Guid.NewGuid().ToString();
                    singleHj.point.x = $"{x.point_x}";
                    singleHj.point.y = $"{x.point_y}";
                    singleHj.point.z = $"{x.point_z}";
                    singleHj.point.layout_x = $"{Factory<PointMapService>.instance.GetPoint("x", float.Parse(singleHj.point.x), _pointMaps)}";
                    singleHj.point.layout_y = $"{Factory<PointMapService>.instance.GetPoint("y", float.Parse(singleHj.point.z), _pointMaps)}";
                    singleHj.shelves_id = string.IsNullOrEmpty(x.shelves_id) ? "" : x.shelves_id;
                    singleHj.name = string.IsNullOrEmpty(x.shelves_code) ? "" : x.shelves_code;

                    singleHj.type = IEquipmentObject.HW;

                    _all_shelves.Add(singleHj);
                }

                if (x.point_x != null && x.point_y != null && x.point_z != null)
                {
                    this._all_shelves_position.Add(new ShelvesPosition()
                    {
                        id = x.id,
                        relation_object_id = string.IsNullOrEmpty(x.relation_object_id) ? "" : x.relation_object_id,
                        shelves_id = string.IsNullOrEmpty(x.shelves_id) ? "" : x.shelves_id,
                        shelves_name = string.IsNullOrEmpty(x.shelves_code) ? "" : x.shelves_code,
                        point_x = (int)x.point_x,
                        point_y = (int)x.point_y,
                        point_z = (int)x.point_z,
                        is_loaded = (bool)(x.is_loaded == null ? false : x.is_loaded),
                        is_locked = (bool)(x.is_locked == null ? false : x.is_locked),
                        layout_id = singleHj.id,
                        layout_x = $"{Factory<PointMapService>.instance.GetPoint("x", float.Parse(((int)x.point_x).ToString()), _pointMaps)}",
                        layout_y = $"{Factory<PointMapService>.instance.GetPoint("y", float.Parse(((int)x.point_z).ToString()), _pointMaps)}",
                        status = (int)(x.status == null ? 0 : x.status)
                    });
                }

            });


            if (this._all_equipment.Count > 0)
            {
                CConsole.WriteLine("设备操作对象初始化完毕...", OutPut.warning);
            }
            else
            {
                isReady = false;
                CConsole.WriteLine("无可用设备", OutPut.error);
                // todo 记录日志
                LLog.log.Error("无可用设备");
            }

            if (this._all_shelves_position.Count > 0)
            {
                CConsole.WriteLine("设备操作对象初始化完毕...", OutPut.warning);
            }
            else
            {
                isReady = false;
                CConsole.WriteLine("无可用货架", OutPut.error);
                // todo 记录日志
                LLog.log.Error("无可用货架");
            }
        }

        /// <summary>
        /// 任务处理
        /// </summary>
        public void DealTask()
        {
            // 1 获取等待中的任务            
            TaskEntity task = Factory<TaskService>.instance.GetTask();
            if (task != null && this.status.Exists(x => x == task.task_type))
            {
                this.isBusy = true;
                switch (task.task_type)
                {
                    // 入库操作
                    case "002cfc70-89a2-11ed-a5d9-00163e10b48b":
                        {
                            this.Rk(task);
                        }
                        break;
                    case "ba45546e-915b-11ed-a5d9-00163e10b48b":
                        {
                            this.Ck(task);
                        }
                        break;
                    default: break;
                }
                this.isBusy = false;
            }
            else
            {
                // todo 记录日志暂时无任务
                CConsole.WriteLine("记录日志暂时无任务", OutPut.warning);
            }
        }

        /// <summary>
        /// 入库操作
        /// </summary>
        /// <param name="entity"></param>
        private void Rk(TaskEntity entity)
        {
            IEquipmentObject? ssx = null;
            IEquipmentObject? ddj = null;
            ShelvesPosition? hw = null;

            // todo 日志入库操作
            // 1.读取起始点信息
            if (entity.start_object != null)
            {
                ssx = this._all_equipment.Find(x => x.id == entity.start_object);
            }

            // 2.读取操作者信息
            if (entity.process_object != null)
            {
                ddj = this._all_equipment.Find(x => x.id == entity.process_object);
            }

            // 3.读取终点信息
            if (entity.end_object != null)
            {
                hw = this._all_shelves_position.Find(x => x.id == entity.end_object);
            }

            if (ssx != null && ddj != null && hw != null)
            {
                Factory<TaskService>.instance.UpdateTaskStatus(entity.Id, Exec.running, "");

                MoveIn m = new MoveIn(ssx, ddj, hw);

                RunningResult result = new RunningResult();

                m.Start(entity.Id, out result);
                //Thread.Sleep(2000);

                if (result.result)
                {
                    hw.is_loaded = true;

                    Factory<ShelvesService>.instance.UpdateSheviesPostionLoad(hw.id, true);
                    Factory<TaskService>.instance.UpdateTaskStatus(entity.Id, Exec.complete, "");
                }
                else
                {
                    Factory<TaskService>.instance.UpdateTaskStatus(entity.Id, Exec.error, result.message);
                    CConsole.WriteLine($"入库操作失败:{result.message}", OutPut.warning, "", entity.Id);
                }

                // 货位释放
                hw.is_locked = false;
                Factory<ShelvesService>.instance.UpdateSheviesPositionLock(hw.id, false);
            }
            else
            {
                // todo 记录错误日志
                CConsole.WriteLine("入库操作失败，设备获取错误", OutPut.warning, "", entity.Id);
                Factory<TaskService>.instance.UpdateTaskStatus(entity.Id, Exec.error, "入库操作失败，设备获取错误");
            }
        }

        /// <summary>
        /// 出库操作
        /// </summary>
        /// <param name="entity"></param>
        private void Ck(TaskEntity entity)
        {
            IEquipmentObject? ssx = null;
            IEquipmentObject? ddj = null;
            ShelvesPosition? hw = null;

            // todo 日志出库操作
            // 1.读取起始点信息
            if (entity.start_object != null)
            {
                hw = this._all_shelves_position.Find(x => x.id == entity.start_object);
            }

            // 2.读取操作者信息
            if (entity.process_object != null)
            {
                ddj = this._all_equipment.Find(x => x.id == entity.process_object);
            }

            // 3.读取终点信息
            if (entity.end_object != null)
            {
                ssx = this._all_equipment.Find(x => x.id == entity.end_object);
            }

            if (ssx != null && ddj != null && hw != null)
            {
                Factory<TaskService>.instance.UpdateTaskStatus(entity.Id, Exec.running, "");

                MoveOut m = new MoveOut(ssx, ddj, hw);

                RunningResult result = new RunningResult();

                m.Start(out result);
                //Thread.Sleep(2000);

                if (result.result)
                {
                    hw.is_loaded = false;

                    Factory<ShelvesService>.instance.UpdateSheviesPostionLoad(hw.id, false);
                    Factory<TaskService>.instance.UpdateTaskStatus(entity.Id, Exec.complete, "");
                }
                else
                {
                    Factory<TaskService>.instance.UpdateTaskStatus(entity.Id, Exec.error, result.message);
                    CConsole.WriteLine($"出库操作失败:{result.message}", OutPut.warning, "", entity.Id);
                }

                // 货位释放
                hw.is_locked = false;
                Factory<ShelvesService>.instance.UpdateSheviesPositionLock(hw.id, false);
            }
            else
            {
                // todo 记录错误日志
                CConsole.WriteLine("出库操作失败，设备获取错误", OutPut.warning, "", entity.Id);
                Factory<TaskService>.instance.UpdateTaskStatus(entity.Id, Exec.error, "出库操作失败，设备获取错误");
            }
        }
    }
}
