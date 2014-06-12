using System;
using System.Linq;
using System.Linq.Expressions;

namespace Restful.Linq
{
    /// <summary>
    /// 提供对未指定数据类型的特定数据源的删除进行计算的功能。
    /// </summary>
    public interface IDeleteable
    {
        /// <summary>
        /// 获取元素类型
        /// </summary>
        Type ElementType { get; }

        /// <summary>
        /// 获取过滤表达式树
        /// </summary>
        Expression Predicate { get; set; }

        /// <summary>
        /// 获取与此数据源关联的删除提供程序。
        /// </summary>
        IDeleteProvider Provider { get; }
    }

    /// <summary>
    /// 提供对数据类型已知的特定数据源的删除进行计算的功能。
    /// </summary>
    public interface IDeleteable<T> : IDeleteable
    {

    }
}

