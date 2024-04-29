using System;
using wcs.core.@interface;
using wcs.core.common;

namespace wcs.core.service
{
    /// <summary>
    /// 传输带对象
    /// </summary>
    public class ConveyorObject
    {
        /// <summary>
        /// 入库方向转动
        /// </summary>
        public void PutIn()
        {
            CConsole.WriteLine("传输带命令发送 入库方向旋转...", OutPut.normal);
        }

        /// <summary>
        /// 出库方向转动
        /// </summary>
        public void PutOut()
        {
            CConsole.WriteLine("传输带命令发送 出库方向旋转...", OutPut.normal);
        }
    }
}

