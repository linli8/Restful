using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Data.MySql.Linq
{
    public class MySqlSqlCommand
    {
        /// <summary>
        /// 查询语句
        /// </summary>
        public string Sql { get; private set; }

        /// <summary>
        /// 查询所用参数字典
        /// </summary>
        public IDictionary<string,object> Parameters { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public MySqlSqlCommand( string sql, IDictionary<string, object> parameters )
        {
            this.Sql = sql;
            this.Parameters = parameters;
        }
    }
}
