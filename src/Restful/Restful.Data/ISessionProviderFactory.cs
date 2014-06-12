using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using Restful.Linq;

namespace Restful.Data
{
    public interface ISessionProviderFactory
    {
        /// <summary>
        /// 创建 ISession 对象
        /// </summary>
        /// <param name="connectionStr">连接字符串</param>
        /// <returns>ISession 对象</returns>
        ISession CreateSession( string connectionStr );

        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <returns>数据库连接对象</returns>
        /// <param name="connectionStr">连接字符串</param>
        DbConnection CreateConnection( string connectionStr );

        /// <summary>
        /// 创建数据适配器
        /// </summary>
        DbDataAdapter CreateDataAdapter();

        /// <summary>
        /// Creates the command builder.
        /// </summary>
        /// <returns>The command builder.</returns>
        CommandBuilder CreateCommandBuilder();

        /// <summary>
        /// Creates the insert provider.
        /// </summary>
        /// <returns>The insert provider.</returns>
        /// <param name="provider">Provider.</param>
        IInsertProvider CreateInsertProvider( ISessionProvider provider );

        /// <summary>
        /// Creates the delete provider.
        /// </summary>
        /// <returns>The delete provider.</returns>
        /// <param name="provider">Provider.</param>
        IDeleteProvider CreateDeleteProvider( ISessionProvider provider );

        /// <summary>
        /// Creates the update provider.
        /// </summary>
        /// <returns>The update provider.</returns>
        /// <param name="provider">Provider.</param>
        IUpdateProvider CreateUpdateProvider( ISessionProvider provider );

        /// <summary>
        /// Creates the queryable.
        /// </summary>
        /// <returns>The queryable.</returns>
        /// <param name="queryParser">Query parser.</param>
        /// <param name="executor">Executor.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        IQueryable<T> CreateQueryable<T>( ISessionProvider provider );
    }
}
