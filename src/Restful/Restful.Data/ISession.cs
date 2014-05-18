using System;

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
