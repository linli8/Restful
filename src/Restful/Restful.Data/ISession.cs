using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Restful.Data
{
    public interface ISession : IDisposable
    {
        /// <summary>
        /// 获取 Session 对象的提供程序
        /// </summary>
        ISessionProvider Provider { get; }
    }
}
