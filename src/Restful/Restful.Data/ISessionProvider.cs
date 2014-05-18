using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Restful.Data.Common;
using Restful.Data.Entity;
using Restful.Data.Linq;

namespace Restful.Data
{
    public interface ISessionProvider : IDisposable
    {
        #region Transaction
        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        DbTransaction BeginTransaction();
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 执行带参数的 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        T ExecuteScalar<T>( string sql, IDictionary<string, object> parameters );
        #endregion

        #region ExecuteDataReader
        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataReader 对象</returns>
        DbDataReader ExecuteDataReader( string sql, IDictionary<string, object> parameters );
        #endregion

        #region ExecuteDataTable
        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataTable 对象</returns>
        DataTable ExecuteDataTable( string sql, IDictionary<string, object> parameters );
        #endregion

        #region ExecuteDataSet
        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>DataSet 对象</returns>
        DataSet ExecuteDataSet( string sql, IDictionary<string, object> parameters );
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// 执行带参数的非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="values">参数列表</param>
        /// <returns>受影响的行数</returns>
        int ExecuteNonQuery( string sql, IDictionary<string, object> parameters );
        #endregion

        #region ExecutePageQuery
        /// <summary>
        /// 执行分页查询
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>分页查询结果</returns>
        PageQueryResult ExecutePageQuery( string sql, int pageIndex, int pageSize, string orderBy, IDictionary<string, object> parameters );
        #endregion

        #region ExecuteStoredProcedure
        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="storedProcedureName">存储过程名</param>
        /// <param name="parameters">参数集合</param>
        void ExecuteStoredProcedure( string storedProcedureName, IList<DbParameter> parameters );
        #endregion

        #region Insert
        /// <summary>
        /// 将实体对象插入到数据库
        /// </summary>
        /// <param name="object">实体对象</param>
        int Insert( EntityObject @object );
        #endregion

        #region GetIdentifier<T>
        /// <summary>
        /// 获取最新插入数据的自增ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetIdentifier<T>();
        #endregion

        #region Update
        /// <summary>
        /// 将实体对象更新到数据库，调用该方法时实体对象必须具备主键属性
        /// </summary>
        /// <param name="object">实体对象</param>
        int Update( EntityObject @object );
        #endregion

        #region Delete
        /// <summary>
        /// 将实体对象从数据库中删除，调用该方法时实体对象必须具备主键属性
        /// </summary>
        /// <param name="object">实体对象</param>
        int Delete( EntityObject @object );
        #endregion

        #region Update<T>
        /// <summary>
        /// 创建更新执行器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>IUpdator 对象</returns>
        IUpdateable<T> Update<T>() where T : EntityObject;
        #endregion

        #region Delete<T>
        /// <summary>
        /// 创建删除执行器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>IDeletor 对象</returns>
        IDeleteable<T> Delete<T>() where T : EntityObject;
        #endregion

        #region Find<T>
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> Find<T>();

        /// <summary>
        /// 执行 SQL 语句，并返回查询结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IList<T> Find<T>( string sql, IDictionary<string, object> parameters );
        #endregion
    }
}
