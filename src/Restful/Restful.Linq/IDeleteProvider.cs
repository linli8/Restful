using System;
using System.Linq.Expressions;

namespace Restful.Linq
{
    /// <summary>
    /// 定义用于创建和执行 IDeleteable 对象所描述的删除的方法。
    /// </summary>
    public interface IDeleteProvider
    {
        #region IDeleteable Support

        /// <summary>
        /// 构造一个 IDeleteable 对象，该对象可计算指定表达式树所表示的删除。
        /// </summary>
        /// <returns>一个 IDeleteable，它可计算指定表达式树所表示的删除。</returns>
        /// <param name="elementType">元素类型</param>
        IDeleteable CreateDelete( Type elementType );

        /// <summary>
        /// 执行删除操作。
        /// </summary>
        /// <param name="elementType">元素类型</param>
        /// <param name="predicate">where 表达式树</param>
        int Execute( Type elementType, Expression predicate );

        #endregion

        #region IDeleteable<T> Support

        /// <summary>
        /// 构造一个 IDeleteable<T> 对象，该对象可计算指定表达式树所表示的删除。
        /// </summary>
        /// <returns>一个 IDeleteable<T>，它可计算指定表达式树所表示的删除。</returns>
        /// <typeparam name="T">需删除的元素类型</typeparam>
        IDeleteable<T> CreateDelete<T>();

        /// <summary>
        /// 执行删除操作。
        /// </summary>
        /// <param name="predicate">where 表达式树</param>
        int Execute<T>( Expression predicate );

        #endregion
    }
}

