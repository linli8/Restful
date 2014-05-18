
namespace Restful.Data.Linq
{
    public interface IExecuteable
    {
        /// <summary>
        /// 获取 Session 提供程序
        /// </summary>
        ISessionProvider Provider { get; }

        /// <summary>
        /// 执行数据库操作
        /// </summary>
        int Execute();
    }
}
