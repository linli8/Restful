using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Restful.Linq
{
    /// <summary>
    /// 提供对未指定数据类型的特定数据源的插入进行计算的功能。
    /// </summary>
    public interface IInsertable
    {
        /// <summary>
        /// 获取插入时需设置的属性集合
        /// </summary>
        IDictionary<MemberExpression,object> Properties { get; }

        /// <summary>
        /// 获取与此数据源关联的更新提供程序。
        /// </summary>
        IInsertProvider Provider { get; }
    }

    /// <summary>
    /// 提供对数据类型已知的特定数据源的插入进行计算的功能。
    /// </summary>
    public interface IInsertable<T> : IInsertable
    {
    }
}

