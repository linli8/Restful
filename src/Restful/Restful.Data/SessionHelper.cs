using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Restful.Data.Common;
using Restful.Data.Entity;
using Restful.Extensions;

namespace Restful.Data
{
    /// <summary>
    /// ISession 辅助类
    /// </summary>
    public static class SessionHelper
    {
        #region ExecuteScalar
        /// <summary>
        /// 执行带参数的 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
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
        /// 执行带参数的 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        public static T ExecuteScalar<T>( string sql, IDictionary<string, object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteScalar<T>( sql, parameters );
            }
        }
        #endregion

        #region ExecuteDataReader
        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataReader 对象
        /// </summary>
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
        /// 执行带参数 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataReader 对象</returns>
        public static IDataReader ExecuteDataReader( string sql, IDictionary<string, object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataReader( sql, parameters );
            }
        }
        #endregion

        #region ExecuteDataTable
        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
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
        /// 执行带参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable ExecuteDataTable( string sql, IDictionary<string, object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataTable( sql, parameters );
            }
        }
        #endregion

        #region ExecuteDataSet
        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataSet 对象</returns>
        public static DataSet ExecuteDataSet( string sql )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataSet( sql );
            }
        }

        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataSet 对象</returns>
        public static DataSet ExecuteDataSet( string sql, IDictionary<string, object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteDataSet( sql, parameters );
            }
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// 执行带参数的非查询 SQL 语句，返回受影响的行数
        /// </summary>
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
        /// 执行带参数的非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数列表</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery( string sql, IDictionary<string, object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecuteNonQuery( sql, parameters );
            }
        }
        #endregion

        #region ExecutePageQuery
        /// <summary>
        /// 执行分页查询
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns>分页查询结果</returns>
        public static PageQueryResult ExecutePageQuery( string sql, int pageIndex, int pageSize, string orderBy )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecutePageQuery( sql, pageIndex, pageSize, orderBy );
            }
        }

        /// <summary>
        /// 执行分页查询
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>分页查询结果</returns>
        public static PageQueryResult ExecutePageQuery( string sql, int pageIndex, int pageSize, string orderBy, IDictionary<string, object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.ExecutePageQuery( sql, pageIndex, pageSize, orderBy, parameters );
            }
        }
        #endregion

        #region ExecuteStoredProcedure
        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="storedProcedureName">存储过程名</param>
        /// <param name="parameters">参数集合</param>
        public static void ExecuteStoredProcedure( string storedProcedureName, IList<DbParameter> parameters )
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
        /// <param name="object">实体对象</param>
        public static int Insert( EntityObject @object )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Insert( @object );
            }
        }

        /// <summary>
        /// 将实体对象插入到数据库，并返回 Indentifer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object">实体对象</param>
        /// <param name="returnIndentifer">是否返回 Indentifer</param>
        /// <returns></returns>
        public static T Insert<T>( EntityObject @object, bool returnIndentifer )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                int i = session.Insert( @object );

                if( returnIndentifer )
                {
                    return session.GetIndentifer<T>();
                }

                return i.Cast<T>();
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// 将实体对象更新到数据库，调用该方法时实体对象必须具备主键属性
        /// </summary>
        /// <param name="object">实体对象</param>
        public static int Update( EntityObject @object )
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
        /// <param name="object">实体对象</param>
        public static int Delete( EntityObject @object )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Delete( @object );
            }
        }
        #endregion

        #region Find<T>
        /// <summary>
        /// 执行 SQL 语句，并返回查询结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<T> Find<T>( string sql )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Find<T>( sql );
            }
        }

        /// <summary>
        /// 执行 SQL 语句，并返回查询结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<T> Find<T>( string sql, IDictionary<string, object> parameters )
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                return session.Find<T>( sql, parameters );
            }
        }
        #endregion
    }
}
