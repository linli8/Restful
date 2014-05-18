using System.Collections.Generic;
using System.Data.Common;
using Remotion.Linq;
using Restful.Data.Extensions;
using Restful.Data.MySql.Visitors;

namespace Restful.Data.MySql.Linq
{
    public class MySqlQueryExecutor : IQueryExecutor
    {
        private MySqlSessionProvider provider;

        /// <summary>
        /// SQL 命令
        /// </summary>
        public MySqlSqlCommand SqlCommand { get; private set; }

        public MySqlQueryExecutor( MySqlSessionProvider provider )
        {
            this.provider = provider;
        }

        #region ExecuteCollection<T>
        /// <summary>
        ///  执行查询，并返回 IEnumerable<T> 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteCollection<T>( QueryModel queryModel )
        {
            MySqlQueryModelVisitor queryModelVisitor = new MySqlQueryModelVisitor();

            var command = queryModelVisitor.Translate( queryModel );

            this.SqlCommand = command;

            using( DbDataReader dataReader = this.provider.ExecuteDataReader( command.Sql.ToString(), command.Parameters ) )
            {
                return dataReader.ToObjects<T>();
            }
        }
        #endregion

        #region ExecuteScalar<T>
        /// <summary>
        /// 执行查询，并返回第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>( QueryModel queryModel )
        {
            MySqlQueryModelVisitor queryModelVisitor = new MySqlQueryModelVisitor();

            var command = queryModelVisitor.Translate( queryModel );

            this.SqlCommand = command;

            return this.provider.ExecuteScalar<T>( command.Sql, command.Parameters );
        }
        #endregion

        #region ExecuteSingle<T>
        /// <summary>
        /// 执行查询，并返回单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryModel"></param>
        /// <param name="returnDefaultWhenEmpty"></param>
        /// <returns></returns>
        public T ExecuteSingle<T>( QueryModel queryModel, bool returnDefaultWhenEmpty )
        {
            MySqlQueryModelVisitor queryModelVisitor = new MySqlQueryModelVisitor();

            var command = queryModelVisitor.Translate( queryModel );

            this.SqlCommand = command;

            using( DbDataReader dataReader = this.provider.ExecuteDataReader( command.Sql.ToString(), command.Parameters ) )
            {
                if( dataReader.HasRows == false && returnDefaultWhenEmpty )
                {
                    return default( T );
                }

                dataReader.Read();

                return dataReader.ToObject<T>();
            }
        }
        #endregion
    }
}
