using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Data
{
    public interface ISessionFactory
    {
        /// <summary>
        /// 创建 ISession 对象
        /// </summary>
        /// <param name="connectionStr">连接字符串</param>
        /// <returns>ISession 对象</returns>
        ISession CreateSession( string connectionStr );
    }
}
