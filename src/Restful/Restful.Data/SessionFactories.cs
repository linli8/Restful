using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Data
{
    public class SessionFactories
    {
        #region Member
        /// <summary>
        /// 存储已注册的 Session 工厂
        /// </summary>
        private static IDictionary<string, ISessionFactory> factories = new Dictionary<string, ISessionFactory>();
        #endregion

        #region Register<T>
        /// <summary>
        /// 注册 Session 工厂
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        public static void Register<T>() where T : ISessionFactory
        {
            string providerName = typeof( T ).Assembly.GetName().Name;

            if( factories.ContainsKey( providerName ) )
            {
                return;
            }

            T factory = Activator.CreateInstance<T>();

            factories.Add( providerName, factory );
        }
        #endregion

        #region GetFactory
        /// <summary>
        /// 根据指定提供程序的 Session 工厂
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static ISessionFactory GetFactory( string providerName )
        {
            if( factories.ContainsKey( providerName ) == false )
            {
                throw new ArgumentException( string.Format( "提供程序 {0} 未注册。", providerName ), "providerName" );
            }

            return factories[providerName];
        }
        #endregion
    }
}
