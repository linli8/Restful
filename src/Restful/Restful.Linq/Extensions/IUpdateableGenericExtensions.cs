using System;
using System.Linq;
using System.Linq.Expressions;

namespace Restful.Linq
{
    public static class IUpdateableGenericExtensions
    {
        /// <summary>
        /// 设置需更新的字段和值
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="property">字段</param>
        /// <param name="value">值</param>
        /// <typeparam name="T">元素类型</typeparam>
        public static IUpdateable<T> Set<T>( this IUpdateable<T> source, Expression<Func<T,object>> property, object value )
        {
            MemberExpression me = property.Body.ToMemberExpression();

            var key = source.Properties.Keys.Where( s => s.MemberExpressionEqual( me ) ).FirstOrDefault();

            if( key == null )
            {
                source.Properties.Add( me, value );
            }
            else
            {
                source.Properties[key] = value;
            }

            return source;
        }

        /// <summary>
        /// 设置过滤函数
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="predicate">过滤函数</param>
        /// <typeparam name="T">元素类型</typeparam>
        public static IUpdateable<T> Where<T>( this IUpdateable<T> source, Expression<Func<T,bool>> predicate )
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
        /// 执行指定元素类型的更新操作
        /// </summary>
        /// <param name="source">source</param>
        /// <typeparam name="T">元素类型</typeparam>
        public static int Execute<T>( this IUpdateable<T> source )
        {
            return source.Provider.Execute<T>( source.Properties, source.Predicate );
        }
    }
}

