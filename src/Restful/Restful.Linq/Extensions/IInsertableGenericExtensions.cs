using System;
using System.Linq;
using System.Linq.Expressions;

namespace Restful.Linq
{
    public static class IInsertableGenericExtensions
    {
        /// <summary>
        /// 设置需新增的字段和值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="property">字段</param>
        /// <param name="value">值</param>
        /// <typeparam name="T">元素类型</typeparam>
        public static IInsertable<T> Set<T>( this IInsertable<T> source, Expression<Func<T,object>> property, object value )
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
        /// 执行指定类型元素的删除操作
        /// </summary>
        /// <param name="source">可新增的对象</param>
        /// <typeparam name="T">受影响的行数</typeparam>
        public static int Execute<T>( this IInsertable<T> source )
        {
            return source.Provider.Execute<T>( source.Properties );
        }
    }
}

