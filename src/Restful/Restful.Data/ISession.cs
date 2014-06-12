using System;

namespace Restful.Data
{
    /// <summary>
    /// session 接口
    /// </summary>
    public interface ISession : IDisposable
    {
        /// <summary>
        /// 获取 Session 提供程序
        /// </summary>
        ISessionProvider Provider { get; }
    }
}
