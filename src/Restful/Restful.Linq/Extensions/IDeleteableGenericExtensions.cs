using System;
using System.Linq.Expressions;

namespace Restful.Linq
{
    public static class IDeleteableGenericExtensions
    {
        /// <summary>
        /// 设置筛选函数
        /// </summary>
        /// <param name="source">IDeleteable<T> 对象</param>
        /// <param name="predicate">用于测试每个源元素是否满足条件的函数</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IDeleteable<T> Where<T>( this IDeleteable<T> source, Expression<Func<T,bool>> predicate )
        {
            if( predicate == null )
                return source;

            if( source.Predicate == null )
            {
                source.Predicate = predicate.Body;
            }
            else
            {
                source.Predicate = Expression.AndAlso( source.Predicate, predicate.Body );
            }

            return source;
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <param name="source">IDeleteable 对象</param>
        /// <typeparam name="T">元素类型</typeparam>
        public static int Execute<T>( this IDeleteable<T> source )
        {
            return source.Provider.Execute<T>( source.Predicate );
        }
    }
}

