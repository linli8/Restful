using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Restful.Data.SqlServer
{
    public class SqlServerSessionFactory : ISessionFactory
    {
        /// <summary>
        /// 创建 Session 对象
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <returns></returns>
        public ISession CreateSession( string connectionStr )
        {
            return new SqlServerSession( connectionStr );
        }
    }
}
