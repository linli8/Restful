using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Remotion.Linq.Parsing.Structure;
using Restful;
using Restful.Data;
using Restful.Data.SqlServer.Common;
using Restful.Data.SqlServer.Linq;

namespace Restful.Data.SqlServer
{
    public class SqlServerSessionProvider : SessionProvider
    {
        #region SqlServerSessionProvider

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionStr"></param>
        public SqlServerSessionProvider( ISessionProviderFactory factory, string connectionStr ) : base( factory, connectionStr )
        {

        }

        #endregion

        #region ExecuteDataPage

        /// <summary>
        /// 执行分页查询
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>分页查询结果</returns>
        public override DataPage ExecuteDataPage( CommandBuilder command, int pageIndex, int pageSize, string orderBy )
        {
            if( string.IsNullOrEmpty( orderBy ) )
            {
                throw new ArgumentNullException( "orderBy" );
            }

            DataPage dataPage = new DataPage( pageIndex, pageSize );

            #region 查询满足条件的条目数量
            string queryItemCountSql = string.Format( "SELECT COUNT(*) FROM ( {0} ) T1", command.ToString() );

            dataPage.ItemCount = this.ExecuteScalar<int>( new Restful.Data.SqlServer.CommandBuilders.SqlServerCommandBuilder( queryItemCountSql, command.Parameters ) );

            if( dataPage.ItemCount == 0 ) // 如果满足条件的数据条目数量为零，则重置页索引为 1
            {
                dataPage.PageIndex = 1;
            }
            else if( dataPage.PageIndex > dataPage.PageCount )  // 如果指定的页索引大于查询直接总页数，则重置页索引为总页数
            {
                dataPage.PageIndex = dataPage.PageCount;
            }
            #endregion

            #region 查询最终的结果集

            string limitFrom = string.Format( "{0}LimitFrom", Constants.ParameterPrefix );
            string limitTo = string.Format( "{0}LimitTo", Constants.ParameterPrefix );

            command.Parameters.Add( limitFrom, ( dataPage.PageIndex - 1 ) * dataPage.PageSize + 1 );
            command.Parameters.Add( limitTo, dataPage.PageIndex * dataPage.PageSize );

            string queryItemSql = string.Format( "select * from ( select *, row_number() over( order by {0} ) as RowNumber from ( {1} ) a ) t where RowNumber between {2} and {3} ", orderBy, command.ToString(), limitFrom, limitTo );

            dataPage.Data = this.ExecuteDataTable( new Restful.Data.SqlServer.CommandBuilders.SqlServerCommandBuilder( queryItemSql, command.Parameters ) );
            #endregion

            return dataPage;
        }

        #endregion

        #region GetIdentifier

        /// <summary>
        /// 获取最新插入数据的自增ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T GetIdentifier<T>()
        {
            return this.ExecuteScalar<T>( new Restful.Data.SqlServer.CommandBuilders.SqlServerCommandBuilder( "select @@identity" ) );
        }

        #endregion
    }
}
