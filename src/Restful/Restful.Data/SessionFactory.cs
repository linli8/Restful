using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Restful.Data
{
    public static class SessionFactory
    {
        #region CreateDefaultSession
        /// <summary>
        /// 创建默认的 Session
        /// </summary>
        /// <returns></returns>
        public static ISession CreateDefaultSession()
        {
            string name = ConfigurationManager.ConnectionStrings[0].Name;

            return CreateSession( name );
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
            ISessionFactory factory = SessionFactories.GetFactory( providerName );

            return factory.CreateSession( connectionStr );
        }
        #endregion
    }
}
