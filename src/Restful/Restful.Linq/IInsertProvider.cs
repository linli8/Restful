using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Restful.Linq
{
    /// <summary>
    /// 定义用于创建和执行 IInsertable 对象所描述的插入的方法。
    /// </summary>
    public interface IInsertProvider
    {
        /// <summary>
        /// 构造一个 IInsertable 对象，该对象可计算指定表达式树所表示的插入。
        /// </summary>
        /// <returns>一个 IInsertable，它可计算指定表达式树所表示的插入。</returns>
        IInsertable CreateInsert( Type elementType );

        /// <summary>
        /// 构造一个 IInsertable<T> 对象，该对象可计算指定表达式树所表示的插入。
        /// </summary>
        /// <returns>一个 IInsertable<T>，它可计算指定表达式树所表示的插入。</returns>
        /// <typeparam name="T">需插入的元素类型</typeparam>
        IInsertable<T> CreateInsert<T>();

        /// <summary>
        /// 执行删除操作。
        /// </summary>
        /// <param name="elementType">元素类型</param>
        /// <param name="predicate">where 表达式树</param>
        int Execute( Type elementType, IDictionary<MemberExpression,object> properties );

        /// <summary>
        /// 执行插入操作。
        /// </summary>
        /// <param name="properties">插入时需设置的属性集合</param>
        int Execute<T>( IDictionary<MemberExpression,object> properties );
    }
}

