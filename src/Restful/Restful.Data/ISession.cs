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

        /// <summary>
        /// 获取和设置执行 SQL 命令的超时时间，单位秒
        /// </summary>
        /// <value>The timeout.</value>
        int CommandTimeout { get; set; }
    }
}
