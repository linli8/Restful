using System;
using System.Linq.Expressions;
using System.Linq;
using Restful;

namespace Restful.Linq
{
    public static class IDeleteableExtensions
    {
        /// <summary>
        /// 设置筛选函数
        /// </summary>
        /// <param name="source">IDeleteable 对象</param>
        /// <param name="predicate">用于测试每个源元素是否满足条件的函数</param>
        public static IDeleteable Where( this IDeleteable source, Expression predicate )
        {
            if( predicate == null )
                return source;

            if( source.Predicate == null )
            {
                source.Predicate = predicate;
            }
            else
            {
                source.Predicate = Expression.AndAlso( source.Predicate, predicate );
            }

            return source;
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <param name="source">IDeleteable 对象</param>
        /// <param name="elementType">元素类型</param>
        public static int Execute( this IDeleteable source, Type elementType )
        {
            return source.Provider.Execute( elementType, source.Predicate );
        }
    }
}

