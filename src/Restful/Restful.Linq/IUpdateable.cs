using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;

namespace Restful.Linq
{
    /// <summary>
    /// 提供对未指定数据类型的特定数据源的更新进行计算的功能。
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// 获取更新时需设置的属性集合
        /// </summary>
        IDictionary<MemberExpression,object> Properties { get; }

        /// <summary>
        /// 获取过滤表达式树
        /// </summary>
        Expression Predicate { get; set; }

        /// <summary>
        /// 获取与此数据源关联的更新提供程序。
        /// </summary>
        IUpdateProvider Provider { get; }
    }

    /// <summary>
    /// 提供对数据类型已知的特定数据源的更新进行计算的功能。
    /// </summary>
    public interface IUpdateable<T> : IUpdateable
    {
    }
}

