using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Restful.Data.Common;
using Restful.Data.Entity;
using Restful.Data.Linq;

namespace Restful.Data
{
    public static class SessionExtensions
    {
        #region Transaction
        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public static DbTransaction BeginTransaction( this ISession session )
        {
            return session.Provider.BeginTransaction();
        }
        #endregion

        #region ExecuteScalar<T>
        /// <summary>
        /// 执行 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        public static T ExecuteScalar<T>( this ISession session, string sql )
        {
            return session.Provider.ExecuteScalar<T>( sql, null );
        }

        /// <summary>
        /// 执行带参数的 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        public static T ExecuteScalar<T>( this ISession session, string sql, IDictionary<string, object> parameters )
        {
            return session.Provider.ExecuteScalar<T>( sql, parameters );
        }
        #endregion

        #region ExecuteDataReader
        /// <summary>
        /// 执行 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <returns>DataReader 对象</returns>
        public static DbDataReader ExecuteDataReader( this ISession session, string sql )
        {
            return session.Provider.ExecuteDataReader( sql, null );
        }

        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataReader 对象</returns>
        public static DbDataReader ExecuteDataReader( this ISession session, string sql, IDictionary<string, object> parameters )
        {
            return session.Provider.ExecuteDataReader( sql, parameters );
        }
        #endregion

        #region ExecuteDataTable
        /// <summary>
        /// 执行 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable ExecuteDataTable( this ISession session, string sql )
        {
            return session.Provider.ExecuteDataTable( sql, null );
        }

        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataTable 对象</returns>
        public static DataTable ExecuteDataTable( this ISession session, string sql, IDictionary<string, object> parameters )
        {
            return session.ExecuteDataTable( sql, parameters );
        }
        #endregion

        #region ExecuteDataSet
        /// <summary>
        /// 执行 SQL 语句，返回一个 DataSet 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <returns>DataSet 对象</returns>
        public static DataSet ExecuteDataSet( this ISession session, string sql )
        {
            return session.ExecuteDataSet( sql, null );
        }

        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataSet 对象</returns>
        public static DataSet ExecuteDataSet( this ISession session, string sql, IDictionary<string, object> parameters )
        {
            return session.ExecuteDataSet( sql, parameters );
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// 执行非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery( this ISession session, string sql )
        {
            return session.Provider.ExecuteNonQuery( sql, null );
        }

        /// <summary>
        /// 执行带参数的非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数列表</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery( this ISession session, string sql, IDictionary<string, object> parameters )
        {
            return session.Provider.ExecuteNonQuery( sql, parameters );
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
        public static PageQueryResult ExecutePageQuery( this ISession session, string sql, int pageIndex, int pageSize, string orderBy )
        {
            return session.Provider.ExecutePageQuery( sql, pageIndex, pageSize, orderBy, null );
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
        public static PageQueryResult ExecutePageQuery( this ISession session, string sql, int pageIndex, int pageSize, string orderBy, IDictionary<string, object> parameters )
        {
            return session.Provider.ExecutePageQuery( sql, pageIndex, pageSize, orderBy, parameters );
        }
        #endregion

        #region ExecuteStoredProcedure
        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="session"></param>
        /// <param name="storedProcedureName">存储过程名</param>
        public static void ExecuteStoredProcedure( this ISession session, string storedProcedureName )
        {
            session.Provider.ExecuteStoredProcedure( storedProcedureName, null );
        }

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="session"></param>
        /// <param name="storedProcedureName">存储过程名</param>
        /// <param name="parameters">参数集合</param>
        public static void ExecuteStoredProcedure( this ISession session, string storedProcedureName, IList<DbParameter> parameters )
        {
            session.Provider.ExecuteStoredProcedure( storedProcedureName, parameters );
        }
        #endregion

        #region Insert
        /// <summary>
        /// 将实体对象插入到数据库
        /// </summary>
        /// <param name="object">实体对象</param>
        public static int Insert( this ISession session, EntityObject @object )
        {
            return session.Provider.Insert( @object );
        }
        #endregion

        #region GetIndentifer<T>
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <returns></returns>
        public static T GetIndentifer<T>( this ISession session )
        {
            return session.Provider.GetIdentifier<T>();
        }
        #endregion

        #region Update
        /// <summary>
        /// 将实体对象更新到数据库，调用该方法时实体对象必须具备主键属性
        /// </summary>
        /// <param name="object">实体对象</param>
        public static int Update( this ISession session, EntityObject @object )
        {
            return session.Provider.Update( @object );
        }
        #endregion

        #region Delete
        /// <summary>
        /// 将实体对象从数据库中删除，调用该方法时实体对象必须具备主键属性
        /// </summary>
        /// <param name="object">实体对象</param>
        public static int Delete( this ISession session, EntityObject @object )
        {
            return session.Provider.Delete( @object );
        }
        #endregion

        #region Update<T>
        /// <summary>
        /// 创建更新执行器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="session">session</param>
        /// <param name="object">实体对象</param>
        /// <returns>更新执行器</returns>
        public static IUpdateable<T> Update<T>( this ISession session ) where T : EntityObject
        {
            return session.Provider.Update<T>();
        }
        #endregion

        #region Delete<T>
        /// <summary>
        /// 创建删除执行器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="session">session</param>
        /// <returns>删除执行器</returns>
        public static IDeleteable<T> Delete<T>( this ISession session ) where T : EntityObject
        {
            return session.Provider.Delete<T>();
        }
        #endregion

        #region Find<T>
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <returns></returns>
        public static IQueryable<T> Find<T>( this ISession session )
        {
            return session.Provider.Find<T>();
        }

        /// <summary>
        /// 执行 SQL 查询，并返回查询结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IList<T> Find<T>( this ISession session, string sql )
        {
            return session.Provider.Find<T>( sql, null );
        }

        /// <summary>
        /// 执行 SQL 查询，并返回查询结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IList<T> Find<T>( this ISession session, string sql, IDictionary<string, object> parameters )
        {
            return session.Provider.Find<T>( sql, parameters );
        }
        #endregion


    }
}
