using System;
using SqlSugar;

namespace wcs.core.common
{
    public class DbContext
    {
        private readonly string ConnectString = "server=127.0.0.1;port=3306;database=wcs;user id=root;password=123456";
        private readonly SqlSugarScope _client;
        private static DbContext _instance;

        private DbContext()
        {
            this._client = new SqlSugarScope(
                new ConnectionConfig()
                {
                    ConnectionString = this.ConnectString,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true
                }
            );
        }

        public static SqlSugarScope db
        {
            get
            {
                if (DbContext._instance == null)
                {
                    DbContext._instance = new DbContext();
                }

                return DbContext._instance._client;
            }
        }
    }
}

