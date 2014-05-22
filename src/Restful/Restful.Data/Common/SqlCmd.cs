using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Data.Common
{
    public class SqlCmd
    {
        /// <summary>
        /// 获取或设置当前的 SQL 执行命令
        /// </summary>
        public static SqlCmd Current { get; set; }

        /// <summary>
        /// SQL 语句
        /// </summary>
        public string Sql { get; private set; }

        /// <summary>
        /// 参数集合
        /// </summary>
        public IList<object> Parameters { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数集合</param>
        public SqlCmd( string sql, IList<object> parameters )
        {
            this.Sql = sql;
            this.Parameters = parameters;
        }
    }
}
