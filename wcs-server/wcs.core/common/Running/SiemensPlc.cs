using System;
using System.Net.NetworkInformation;
using System.Text;
using S7.Net;
using wcs.core.@interface;
using wcs.core.model;

namespace wcs.core.common.Running
{
    /// <summary>
    /// 西门子PLC电控对象
    /// </summary>
    public class SiemensPlc : IEquipmentObject
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

        private Plc _plc;

        public SiemensPlc(string ip)
        {
            point = new Point();
            size = new Size() { height = "1", width = "1" };
            _plc = new Plc(CpuType.S71500, ip, 0, 0);
            _plc.ReadTimeout = 3;
        }

        public bool CloseConnect()
        {
            if (this._plc.IsConnected)
            {
                this._plc.Close();
            }
            return true;
        }

        /// <summary>
        /// 输送线货物就位信号
        /// </summary>
        /// <returns></returns>
        public bool GoodIsReady()
        {
            try
            {
                //if (!this._plc.IsConnected)
                //{
                //    this._plc.Open();
                //}

                //var result = this._plc.Read(this.IOPoint);

                //return Convert.ToBoolean(result);
                return true;
            }
            catch (Exception ex)
            {
                LLog.log.Error($"输送线货物就绪信号下发异常:{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 堆垛机是否忙碌
        /// </summary>
        public bool CanMove()
        {
            try
            {
                if (!this._plc.IsConnected)
                {
                    this._plc.Open();
                }

                var result = this._plc.Read(DataType.DataBlock, 2, 14, VarType.Int, 1);

                return Convert.ToInt16(result) == 0;
            }
            catch (Exception ex)
            {
                LLog.log.Error($"堆垛机忙碌状态查询信号下发异常:{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 堆垛机入库信号
        /// </summary>
        /// <param name="p"></param>
        public bool MoveInSignal(Point p)
        {
            try
            {
                if (!this._plc.IsConnected)
                {
                    this._plc.Open();
                }

                // 信号说明
                this._plc.Write(DataType.DataBlock, 2, 0, ushort.Parse("100"));
                // x坐标写入
                this._plc.Write(DataType.DataBlock, 2, 2, float.Parse(p.x));
                // y坐标写入
                this._plc.Write(DataType.DataBlock, 2, 6, float.Parse(p.y));
                // z坐标写入
                this._plc.Write(DataType.DataBlock, 2, 10, float.Parse(p.z));

                // 开始信号
                this._plc.Write(DataType.DataBlock, 2, 14, ushort.Parse("50"));

                return true;
            }
            catch (Exception ex)
            {
                LLog.log.Error($"堆垛机入库信号下发异常:{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 堆垛机出库信号
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool MoveOutSignal(Point p)
        {
            try
            {
                if (!this._plc.IsConnected)
                {
                    this._plc.Open();
                }

                // 信号说明
                //this._plc.Write(DataType.DataBlock, 2, 0, ushort.Parse("100"));
                //// x坐标写入
                //this._plc.Write(DataType.DataBlock, 2, 2, float.Parse(p.x));
                //// y坐标写入
                //this._plc.Write(DataType.DataBlock, 2, 6, float.Parse(p.y));
                //// z坐标写入
                //this._plc.Write(DataType.DataBlock, 2, 10, float.Parse(p.z));

                //// 开始信号
                //this._plc.Write(DataType.DataBlock, 2, 14, ushort.Parse("50"));

                return true;
            }
            catch (Exception ex)
            {
                LLog.log.Error($"堆垛机出库信号下发异常:{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 堆垛机获取当前位置
        /// </summary>
        /// <returns></returns>
        public Point? GetDDJCurPoint()
        {
            Point point = new Point();

            try
            {
                if (!this._plc.IsConnected)
                {
                    this._plc.Open();
                }

                var x = this._plc.Read(DataType.DataBlock, 2, 16, VarType.Real, 1);

                //var y = this._plc.Read(DataType.DataBlock, 2, 20, VarType.Real, 1);

                //var z = this._plc.Read(DataType.DataBlock, 2, 24, VarType.Real, 1);

                if (x != null)
                {
                    string? _x = Convert.ToString(x);
                    point.x = _x != null ? _x : "0";
                    //point.y = (string)y;
                    //point.z = (string)z;
                }
            }
            catch (Exception ex)
            {
                LLog.log.Error($"堆垛机获取定位信号下发异常:{ex.Message}");

                return null;
            }

            return point;
        }

        /// <summary>
        /// 是否能链接
        /// </summary>
        /// <returns></returns>
        public bool CanConnect()
        {
            return true;

            //Ping objPingSender = new Ping();

            //PingOptions objPingOptions = new PingOptions();

            //objPingOptions.DontFragment = true;

            //string data = "";
            //byte[] buffer = Encoding.UTF8.GetBytes(data);
            //int iniTimeout = 5;
            //PingReply objPinReplay = objPingSender.Send(_plc.IP, iniTimeout, buffer, objPingOptions);

            //string strInfo = objPinReplay.Status.ToString();

            //return strInfo == "Success";
        }
    }
}

