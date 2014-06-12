using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Restful.Linq;
using System.Data.SqlTypes;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Restful.Data
{
    public static class ISessionExtensions
    {
        #region Transaction

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns>事务</returns>
        /// <param name="session">session</param>
        public static DbTransaction BeginTransaction( this ISession session )
        {
            return session.Provider.BeginTransaction();
        }

        #endregion

        #region ConvertToCommandBuilder

        /// <summary>
        /// Converts to command builder.
        /// </summary>
        /// <returns>The to command builder.</returns>
        /// <param name="session">Session.</param>
        /// <param name="sql">Sql.</param>
        /// <param name="parameters">Parameters.</param>
        private static CommandBuilder ConvertToCommandBuilder( this ISession session, string sql )
        {
            CommandBuilder builder = session.Provider.Factory.CreateCommandBuilder();

            builder.Sql = sql;

            return builder;
        }

        /// <summary>
        /// Converts to command builder.
        /// </summary>
        /// <returns>The to command builder.</returns>
        /// <param name="session">Session.</param>
        /// <param name="sql">Sql.</param>
        /// <param name="parameters">Parameters.</param>
        private static CommandBuilder ConvertToCommandBuilder( this ISession session, string sql, IDictionary<string,object> parameters )
        {
            CommandBuilder builder = session.Provider.Factory.CreateCommandBuilder();

            builder.Sql = sql;

            if( parameters != null )
            {
                foreach( var item in parameters )
                {
                    builder.AddParameter( item.Key, item.Value );
                }
            }

            return builder;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <returns>The to command builder.</returns>
        /// <param name="sql">Sql.</param>
        /// <param name="values">Values.</param>
        private static CommandBuilder ConvertToCommandBuilder( this ISession session, string sql, object[] values )
        {
            string[] parts = sql.Split( '?' );

            int count = values == null ? 0 : values.Length;

            if( parts.Length - 1 != count )
            {
                throw new ArgumentOutOfRangeException( "参数数量与参数值数量不一致。" );
            }

            CommandBuilder commandBuilder = session.Provider.Factory.CreateCommandBuilder();

            StringBuilder builder = new StringBuilder();
            
            for( int i = 0; i < parts.Length; i++ )
            {
                builder.Append( parts[i] );
            
                if( i < parts.Length - 1 )
                {
                    builder.Append( commandBuilder.AddParameter( values[i] ) );
                }
            }

            commandBuilder.Sql = builder.ToString();

            return commandBuilder;
        }

        #endregion

        #region ExecuteScalar<T>

        /// <summary>
        /// 执行 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        public static T ExecuteScalar<T>( this ISession session, string sql )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql );

            return session.Provider.ExecuteScalar<T>( builder );
        }

        /// <summary>
        /// 执行带匿名参数的 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        public static T ExecuteScalar<T>( this ISession session, string sql, params object[] values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values );

            return session.Provider.ExecuteScalar<T>( builder );
        }

        /// <summary>
        /// 执行带匿名参数的 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数值列表</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        public static T ExecuteScalar<T>( this ISession session, string sql, IList<object> values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values.ToArray() );

            return session.Provider.ExecuteScalar<T>( builder );
        }

        /// <summary>
        /// 执行带命名参数的 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        public static T ExecuteScalar<T>( this ISession session, string sql, IDictionary<string,object> parameters )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, parameters );

            return session.Provider.ExecuteScalar<T>( builder );
        }

        #endregion

        #region ExecuteDataReader

        /// <summary>
        /// 执行 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <returns>DataReader 对象</returns>
        public static IDataReader ExecuteDataReader( this ISession session, string sql )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql );

            return session.Provider.ExecuteDataReader( builder );
        }

        /// <summary>
        /// 执行带匿名参数 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>DataReader 对象</returns>
        public static IDataReader ExecuteDataReader( this ISession session, string sql, params object[] values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values );

            return session.Provider.ExecuteDataReader( builder );
        }

        /// <summary>
        /// 执行带匿名参数 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>DataReader 对象</returns>
        public static IDataReader ExecuteDataReader( this ISession session, string sql, IList<object> values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values.ToArray() );

            return session.Provider.ExecuteDataReader( builder );
        }

        /// <summary>
        /// 执行带命名参数 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataReader 对象</returns>
        public static IDataReader ExecuteDataReader( this ISession session, string sql, IDictionary<string,object> parameters )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, parameters );

            return session.Provider.ExecuteDataReader( builder );
        }

        #endregion

        #region ExecuteDataTable

        /// <summary>
        /// 执行 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable ExecuteDataTable( this ISession session, string sql )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql );

            return session.Provider.ExecuteDataTable( builder );
        }

        /// <summary>
        /// 执行带匿名参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable ExecuteDataTable( this ISession session, string sql, params object[] values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values );

            return session.Provider.ExecuteDataTable( builder );
        }

        /// <summary>
        /// 执行带匿名参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable ExecuteDataTable( this ISession session, string sql, IList<object> values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values.ToArray() );

            return session.Provider.ExecuteDataTable( builder );
        }

        /// <summary>
        /// 执行带命名参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable ExecuteDataTable( this ISession session, string sql, IDictionary<string,object> parameters )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, parameters );

            return session.Provider.ExecuteDataTable( builder );
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 执行 SQL 语句，返回一个 DataSet 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <returns>DataSet 对象</returns>
        public static DataSet ExecuteDataSet( this ISession session, string sql )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql );

            return session.Provider.ExecuteDataSet( builder );
        }

        /// <summary>
        /// 执行带匿名参数 SQL 语句，返回一个 DataSet 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>DataSet 对象</returns>
        public static DataSet ExecuteDataSet( this ISession session, string sql, params object[] values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values );

            return session.Provider.ExecuteDataSet( builder );
        }

        /// <summary>
        /// 执行带匿名参数 SQL 语句，返回一个 DataSet 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>DataSet 对象</returns>
        public static DataSet ExecuteDataSet( this ISession session, string sql, IList<object> values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values.ToArray() );

            return session.Provider.ExecuteDataSet( builder );
        }

        /// <summary>
        /// 执行带命名参数 SQL 语句，返回一个 DataSet 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataSet 对象</returns>
        public static DataSet ExecuteDataSet( this ISession session, string sql, IDictionary<string,object> parameters )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, parameters );

            return session.Provider.ExecuteDataSet( builder );
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery( this ISession session, string sql )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql );

            return session.Provider.ExecuteNonQuery( builder );
        }

        /// <summary>
        /// 执行带匿名参数的非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery( this ISession session, string sql, params object[] values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values );

            return session.Provider.ExecuteNonQuery( builder );
        }

        /// <summary>
        /// 执行带匿名参数的非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery( this ISession session, string sql, IList<object> values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values.ToArray() );

            return session.Provider.ExecuteNonQuery( builder );
        }

        /// <summary>
        /// 执行带命名参数的非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery( this ISession session, string sql, IDictionary<string,object> parameters )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, parameters );

            return session.Provider.ExecuteNonQuery( builder );
        }

        #endregion

        #region ExecuteDataPage

        /// <summary>
        /// 执行分页查询，并返回分页查询结果
        /// </summary>
        /// <returns>分页查询结果</returns>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        public static DataPage ExecuteDataPage( this ISession session, string sql, int pageIndex, int pageSize, string orderBy )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql );

            return session.Provider.ExecuteDataPage( builder, pageIndex, pageSize, orderBy );
        }

        /// <summary>
        /// 执行带匿名参数的分页查询，并返回分页查询结果
        /// </summary>
        /// <returns>分页查询结果</returns>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="values">参数值列表</param>
        public static DataPage ExecuteDataPage( this ISession session, string sql, int pageIndex, int pageSize, string orderBy, params object[] values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values );

            return session.Provider.ExecuteDataPage( builder, pageIndex, pageSize, orderBy );
        }

        /// <summary>
        /// 执行带匿名参数的分页查询，并返回分页查询结果
        /// </summary>
        /// <returns>分页查询结果</returns>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="values">参数值列表</param>
        public static DataPage ExecuteDataPage( this ISession session, string sql, int pageIndex, int pageSize, string orderBy, IList<object> values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values.ToArray() );

            return session.Provider.ExecuteDataPage( builder, pageIndex, pageSize, orderBy );
        }

        /// <summary>
        /// 执行带命令参数的分页查询，并返回分页查询结果
        /// </summary>
        /// <returns>分页查询结果</returns>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="parameters">参数列表</param>
        public static DataPage ExecuteDataPage( this ISession session, string sql, int pageIndex, int pageSize, string orderBy, IDictionary<string,object> parameters )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, parameters );

            return session.Provider.ExecuteDataPage( builder, pageIndex, pageSize, orderBy );
        }

        #endregion

        #region ExecuteStoredProcedure

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="storedProcedureName">存储过程名</param>
        public static void ExecuteStoredProcedure( this ISession session, string storedProcedureName )
        {
            session.Provider.ExecuteStoredProcedure( storedProcedureName, null );
        }

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="storedProcedureName">存储过程名</param>
        /// <param name="parameters">参数列表</param>
        public static void ExecuteStoredProcedure( this ISession session, string storedProcedureName, IList<CommandParameter> parameters )
        {
            session.Provider.ExecuteStoredProcedure( storedProcedureName, parameters );
        }

        #endregion

        #region Insert

        /// <summary>
        /// 将实体对象插入到数据库
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="object">实体对象</param>
        public static int Insert( this ISession session, object @object )
        {
            return session.Provider.Insert( @object );
        }

        #endregion

        #region Insert<T>

        /// <summary>
        /// 创建指定元素类型的可新增对象
        /// </summary>
        /// <param name="session">session</param>
        /// <typeparam name="T">元素类型</typeparam>
        public static IInsertable<T> Insert<T>( this ISession session )
        {
            return session.Provider.Insert<T>();
        }

        #endregion

        #region GetIndentifer<T>

        /// <summary>
        /// 获取 insert 语句执行后自动生成的 id
        /// </summary>
        /// <typeparam name="T">id的类型</typeparam>
        /// <param name="session">session</param>
        /// <returns>id</returns>
        public static T GetIndentifer<T>( this ISession session )
        {
            return session.Provider.GetIdentifier<T>();
        }

        #endregion

        #region Update

        /// <summary>
        /// 将实体对象更新到数据库，调用该方法时实体对象必须具备主键属性
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="object">实体对象</param>
        public static int Update( this ISession session, object @object )
        {
            return session.Provider.Update( @object );
        }

        #endregion

        #region Delete

        /// <summary>
        /// 将实体对象从数据库中删除，调用该方法时实体对象必须具备主键属性
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="object">实体对象</param>
        public static int Delete( this ISession session, object @object )
        {
            return session.Provider.Delete( @object );
        }

        #endregion

        #region Update<T>

        /// <summary>
        /// 创建指定元素类型的可更新对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="session">session</param>
        /// <param name="object">实体对象</param>
        /// <returns>可更新对象</returns>
        public static IUpdateable<T> Update<T>( this ISession session )
        {
            return session.Provider.Update<T>();
        }

        #endregion

        #region Delete<T>

        /// <summary>
        /// 创建指定元素类型的可删除对象
        /// </summary>
        /// <param name="session">Session.</param>
        /// <typeparam name="T">元素类型</typeparam>
        public static IDeleteable<T> Delete<T>( this ISession session )
        {
            return session.Provider.Delete<T>();
        }

        #endregion

        #region Find<T>

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="session">session</param>
        /// <returns>IQueryable 对象</returns>
        public static IQueryable<T> Find<T>( this ISession session )
        {
            return session.Provider.Find<T>();
        }

        /// <summary>
        /// 执行 SQL 查询，并返回查询结果
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <returns>对象列表</returns>
        public static IEnumerable<T> Find<T>( this ISession session, string sql )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql );

            return session.Provider.Find<T>( builder );
        }

        /// <summary>
        /// 执行带匿名参数的 SQL 查询，并返回查询结果
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        public static IEnumerable<T> Find<T>( this ISession session, string sql, params object[] values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values );

            return session.Provider.Find<T>( builder );
        }

        /// <summary>
        /// 执行带匿名参数的 SQL 查询，并返回查询结果
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        public static IEnumerable<T> Find<T>( this ISession session, string sql, IList<object> values )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, values.ToArray() );

            return session.Provider.Find<T>( builder );
        }

        /// <summary>
        /// 执行带命名参数的 SQL 查询，并返回查询结果
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        public static IEnumerable<T> Find<T>( this ISession session, string sql, IDictionary<string,object> parameters )
        {
            CommandBuilder builder = session.ConvertToCommandBuilder( sql, parameters );

            return session.Provider.Find<T>( builder );
        }

        #endregion
    }
}
