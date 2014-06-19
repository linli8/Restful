using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Restful.Linq;

namespace Restful.Data
{
    /// <summary>
    /// session 提供程序接口
    /// </summary>
    public interface ISessionProvider : IDisposable
    {
        /// <summary>
        /// session 提供程序工厂
        /// </summary>
        ISessionProviderFactory Factory { get; }

        /// <summary>
        /// 获取当前执行的 SQL 命令
        /// </summary>
        /// <value>The current sql cmd.</value>
        CommandBuilder ExecutedCommandBuilder { get; set; }

        /// <summary>
        /// 获取和设置执行 SQL 命令的超时时间，单位秒
        /// </summary>
        /// <value>The timeout.</value>
        int CommandTimeout { get; set; }

        #region Transaction

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        DbTransaction BeginTransaction();

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="builder">SQL 命令</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        T ExecuteScalar<T>( CommandBuilder builder );

        #endregion

        #region ExecuteDataReader

        /// <summary>
        /// 执行 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="builder">SQL 命令</param>
        /// <returns>DataReader 对象</returns>
        IDataReader ExecuteDataReader( CommandBuilder builder );

        #endregion

        #region ExecuteDataTable

        /// <summary>
        /// 执行 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="builder">SQL 命令</param>
        /// <returns>DataTable 对象</returns>
        DataTable ExecuteDataTable( CommandBuilder builder );

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 执行 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="builder">SQL 命令</param>
        /// <returns>DataSet 对象</returns>
        DataSet ExecuteDataSet( CommandBuilder builder );

        #endregion

        #region ExecuteDataPage

        /// <summary>
        /// 执行分页查询
        /// </summary>
        /// <returns>分页查询结果</returns>
        /// <param name="builder">SQL 命令</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        DataPage ExecuteDataPage( CommandBuilder builder, int pageIndex, int pageSize, string orderBy );

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="builder">SQL 命令</param>
        /// <returns>受影响的行数</returns>
        int ExecuteNonQuery( CommandBuilder builder );

        #endregion

        #region ExecuteStoredProcedure

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="storedProcedureName">存储过程名</param>
        /// <param name="parameters">参数集合</param>
        void ExecuteStoredProcedure( string storedProcedureName, IList<CommandParameter> parameters );

        #endregion

        #region Insert

        /// <summary>
        /// 将实体对象插入到数据库
        /// </summary>
        /// <param name="object">实体对象</param>
        int Insert( object @object );

        #endregion

        #region Insert<T>

        /// <summary>
        /// Insert this instance.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        IInsertable<T> Insert<T>();

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
        int Update( object @object );

        #endregion

        #region Delete

        /// <summary>
        /// 将实体对象从数据库中删除，调用该方法时实体对象必须具备主键属性
        /// </summary>
        /// <param name="object">实体对象</param>
        int Delete( object @object );

        #endregion

        #region Update<T>

        /// <summary>
        /// 创建更新执行器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>IUpdator 对象</returns>
        IUpdateable<T> Update<T>();

        #endregion

        #region Delete<T>

        /// <summary>
        /// 创建删除执行器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>IDeletor 对象</returns>
        IDeleteable<T> Delete<T>();

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
        /// <param name="builder"></param>
        /// <returns></returns>
        IEnumerable<T> Find<T>( CommandBuilder builder );

        #endregion
    }
}
