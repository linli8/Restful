using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Restful.Linq
{
    /// <summary>
    /// 定义用于创建和执行 IUpdateable 对象所描述的更新的方法。
    /// </summary>
    public interface IUpdateProvider
    {
        /// <summary>
        /// 构造一个 IUpdateable 对象，该对象可计算指定表达式树所表示的更新。
        /// </summary>
        /// <returns>一个 IUpdateable，它可计算指定表达式树所表示的更新。</returns>
        IUpdateable CreateUpdate( Type elementType );

        /// <summary>
        /// 构造一个 IUpdateable<T> 对象，该对象可计算指定表达式树所表示的更新。
        /// </summary>
        /// <returns>一个 IUpdateable<T>，它可计算指定表达式树所表示的更新。</returns>
        /// <typeparam name="T">需更新的元素类型</typeparam>
        IUpdateable<T> CreateUpdate<T>();

        /// <summary>
        /// 执行更新操作。
        /// </summary>
        /// <param name="properties">更新时需设置的属性集合</param>
        /// <param name="whereExpression">where 表达式树</param>
        int Execute( Type elementType, IDictionary<MemberExpression,object> properties, Expression predicate );

        /// <summary>
        /// 执行更新操作。
        /// </summary>
        /// <param name="properties">更新时需设置的属性集合</param>
        /// <param name="whereExpression">where 表达式树</param>
        int Execute<T>( IDictionary<MemberExpression,object> properties, Expression predicate );
    }
}

