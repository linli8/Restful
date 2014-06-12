using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Remotion.Linq;
using Restful.Data.MySql.Visitors;

namespace Restful.Data.MySql.Linq
{
    public class MySqlQueryExecutor : IQueryExecutor
    {
        protected ISessionProvider provider;

        public MySqlQueryExecutor( ISessionProvider provider )
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

            this.provider.ExecutedCommandBuilder = command;

            using( IDataReader reader = this.provider.ExecuteDataReader( command ) )
            {
                var tuple = reader.GetDeserializerState<T>();

                while( reader.Read() )
                {
                    yield return (T)tuple.Func( reader );
                }
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

            this.provider.ExecutedCommandBuilder = command;

            return this.provider.ExecuteScalar<T>( command );
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

            this.provider.ExecutedCommandBuilder = command;

            using( IDataReader reader = this.provider.ExecuteDataReader( command ) )
            {
                if( reader.Read() == false && returnDefaultWhenEmpty )
                {
                    return default( T );
                }

                var tuple = reader.GetDeserializerState<T>();

                return (T)tuple.Func( reader );
            }
        }

        #endregion
    }
}
