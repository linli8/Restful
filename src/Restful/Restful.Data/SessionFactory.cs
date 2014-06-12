using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace Restful.Data
{
    public static class SessionFactory
    {
        /// <summary>
        /// 获取或设置 Web.config 或 App.config 文件中配置的默认连接字符串节点名称
        /// </summary>
        public static string Default { get; set; }

        #region CreateDefaultSession

        /// <summary>
        /// 创建默认的 Session
        /// </summary>
        /// <returns></returns>
        public static ISession CreateDefaultSession()
        {
            if( string.IsNullOrEmpty( Default ) == false )
            {
                return CreateSession( Default );
            }

            string providerName = ConfigurationManager.ConnectionStrings[0].ProviderName;
            string connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;

            return CreateSession( providerName, connectionString );
        }

        #endregion

        #region CreateSession

        /// <summary>
        /// 根据配置文件中指定的名称创建 Session
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ISession CreateSession( string name )
        {
            string providerName = ConfigurationManager.ConnectionStrings[name].ProviderName;
            string connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;

            return CreateSession( providerName, connectionString );
        }

        /// <summary>
        /// 根据指定提供程序和连接字符串创建 Session
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionStr"></param>
        /// <returns></returns>
        public static ISession CreateSession( string providerName, string connectionStr )
        {
            ISessionProviderFactory factory = SessionProviderFactories.GetFactory( providerName );

            return factory.CreateSession( connectionStr );
        }

        #endregion
    }
}
