using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Restful.Linq;

namespace Restful.Data
{
    public static class SessionHelper
    {
        #region ExecuteScalar<T>

        /// <summary>
        /// 执行 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        public static T ExecuteScalar<T>( string sql )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteScalar<T>( sql );
            }
        }

        /// <summary>
        /// 执行带匿名参数的 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        public static T ExecuteScalar<T>( string sql, params object[] values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteScalar<T>( sql, values );
            }
        }

        /// <summary>
        /// 执行带匿名参数的 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数值列表</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        public static T ExecuteScalar<T>( string sql, IList<object> values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteScalar<T>( sql, values );
            }
        }

        /// <summary>
        /// 执行带命名参数的 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        public static T ExecuteScalar<T>( string sql, IDictionary<string,object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteScalar<T>( sql, parameters );
            }
        }

        #endregion

        #region ExecuteDataReader

        /// <summary>
        /// 执行 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <returns>DataReader 对象</returns>
        public static IDataReader ExecuteDataReader( string sql )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataReader( sql );
            }
        }

        /// <summary>
        /// 执行带匿名参数 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>DataReader 对象</returns>
        public static IDataReader ExecuteDataReader( string sql, params object[] values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataReader( sql, values );
            }
        }

        /// <summary>
        /// 执行带匿名参数 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>DataReader 对象</returns>
        public static IDataReader ExecuteDataReader( string sql, IList<object> values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataReader( sql, values );
            }
        }

        /// <summary>
        /// 执行带命名参数 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataReader 对象</returns>
        public static IDataReader ExecuteDataReader( string sql, IDictionary<string,object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataReader( sql, parameters );
            }
        }

        #endregion

        #region ExecuteDataTable

        /// <summary>
        /// 执行 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable ExecuteDataTable( string sql )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataTable( sql );
            }
        }

        /// <summary>
        /// 执行带匿名参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable ExecuteDataTable( string sql, params object[] values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataTable( sql, values );
            }
        }

        /// <summary>
        /// 执行带匿名参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable ExecuteDataTable( string sql, IList<object> values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataTable( sql, values );
            }
        }

        /// <summary>
        /// 执行带命名参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable ExecuteDataTable( string sql, IDictionary<string,object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataTable( sql, parameters );
            }
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 执行 SQL 语句，返回一个 DataSet 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <returns>DataSet 对象</returns>
        public static DataSet ExecuteDataSet( string sql )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataSet( sql );
            }
        }

        /// <summary>
        /// 执行带匿名参数 SQL 语句，返回一个 DataSet 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>DataSet 对象</returns>
        public static DataSet ExecuteDataSet( string sql, params object[] values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataSet( sql, values );
            }
        }

        /// <summary>
        /// 执行带匿名参数 SQL 语句，返回一个 DataSet 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>DataSet 对象</returns>
        public static DataSet ExecuteDataSet( string sql, IList<object> values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataSet( sql, values );
            }
        }

        /// <summary>
        /// 执行带命名参数 SQL 语句，返回一个 DataSet 对象
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataSet 对象</returns>
        public static DataSet ExecuteDataSet( string sql, IDictionary<string,object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataSet( sql, parameters );
            }
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery( string sql )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteNonQuery( sql );
            }
        }

        /// <summary>
        /// 执行带匿名参数的非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery( string sql, params object[] values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteNonQuery( sql, values );
            }
        }

        /// <summary>
        /// 执行带匿名参数的非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery( string sql, IList<object> values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteNonQuery( sql, values );
            }
        }

        /// <summary>
        /// 执行带命名参数的非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery( string sql, IDictionary<string,object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteNonQuery( sql, parameters );
            }
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
        public static DataPage ExecuteDataPage( string sql, int pageIndex, int pageSize, string orderBy )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataPage( sql, pageIndex, pageSize, orderBy );
            }
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
        public static DataPage ExecuteDataPage( string sql, int pageIndex, int pageSize, string orderBy, params object[] values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataPage( sql, pageIndex, pageSize, orderBy, values );
            }
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
        public static DataPage ExecuteDataPage( string sql, int pageIndex, int pageSize, string orderBy, IList<object> values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataPage( sql, pageIndex, pageSize, orderBy, values );
            }
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
        public static DataPage ExecuteDataPage( string sql, int pageIndex, int pageSize, string orderBy, IDictionary<string,object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataPage( sql, pageIndex, pageSize, orderBy, parameters );
            }
        }

        #endregion

        #region ExecuteStoredProcedure

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="storedProcedureName">存储过程名</param>
        public static void ExecuteStoredProcedure( string storedProcedureName )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                session.ExecuteStoredProcedure( storedProcedureName );
            }
        }

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="storedProcedureName">存储过程名</param>
        /// <param name="parameters">参数列表</param>
        public static void ExecuteStoredProcedure( string storedProcedureName, IList<CommandParameter> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                session.ExecuteStoredProcedure( storedProcedureName, parameters );
            }
        }

        #endregion

        #region Insert

        /// <summary>
        /// 将实体对象插入到数据库
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="object">实体对象</param>
        public static int Insert( object @object )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Insert( @object );
            }
        }

        #endregion

        #region Insert<T>

        /// <summary>
        /// 创建指定元素类型的可新增对象
        /// </summary>
        /// <param name="session">session</param>
        /// <typeparam name="T">元素类型</typeparam>
        public static IInsertable<T> Insert<T>()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Insert<T>();
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// 将实体对象更新到数据库，调用该方法时实体对象必须具备主键属性
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="object">实体对象</param>
        public static int Update( object @object )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Update( @object );
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// 将实体对象从数据库中删除，调用该方法时实体对象必须具备主键属性
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="object">实体对象</param>
        public static int Delete( object @object )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Delete( @object );
            }
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
        public static IUpdateable<T> Update<T>()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Update<T>();
            }
        }

        #endregion

        #region Delete<T>

        /// <summary>
        /// 创建指定元素类型的可删除对象
        /// </summary>
        /// <param name="session">Session.</param>
        /// <typeparam name="T">元素类型</typeparam>
        public static IDeleteable<T> Delete<T>()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Delete<T>();
            }
        }

        #endregion

        #region Find<T>

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="session">session</param>
        /// <returns>IQueryable 对象</returns>
        public static IQueryable<T> Find<T>()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Find<T>();
            }
        }

        /// <summary>
        /// 执行 SQL 查询，并返回查询结果
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <returns>对象列表</returns>
        public static IEnumerable<T> Find<T>( string sql )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Find<T>( sql );
            }
        }

        /// <summary>
        /// 执行带匿名参数的 SQL 查询，并返回查询结果
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        public static IEnumerable<T> Find<T>( string sql, params object[] values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Find<T>( sql, values );
            }
        }

        /// <summary>
        /// 执行带匿名参数的 SQL 查询，并返回查询结果
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数值列表</param>
        public static IEnumerable<T> Find<T>( string sql, IList<object> values )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Find<T>( sql, values );
            }
        }

        /// <summary>
        /// 执行带命名参数的 SQL 查询，并返回查询结果
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="session">session</param>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        public static IEnumerable<T> Find<T>( string sql, IDictionary<string,object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Find<T>( sql, parameters );
            }
        }

        #endregion
    }
}

