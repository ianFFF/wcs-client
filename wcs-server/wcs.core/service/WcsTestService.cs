using System;
using wcs.core.common;
using wcs.core.model;

namespace wcs.core.service
{
    public class WcsTestService
    {
        public void Test()
        {
            DbContext.db.Insertable<WcsTestEntity>(new WcsTestEntity() { CreateTime = DateTime.Now }).ExecuteCommand();
        }
    }
}

