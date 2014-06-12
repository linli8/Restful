using System;
using System.Linq.Expressions;
using System.Linq;
using Restful;

namespace Restful.Linq
{
    public static class IInsertableExtensions
    {
        /// <summary>
        /// 设置需新增的字段和值
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="property">字段</param>
        /// <param name="value">值</param>
        public static IInsertable Set( this IInsertable source, MemberExpression property, object value )
        {
            var key = source.Properties.Keys.Where( s => s.MemberExpressionEqual( property ) ).FirstOrDefault();

            if( key == null )
            {
                source.Properties.Add( property, value );
            }
            else
            {
                source.Properties[key] = value;
            }

            return source;
        }

        /// <summary>
        /// 执行未指定元素类型的插入操作
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="elementType">Element type.</param>
        public static int Execute( this IInsertable source, Type elementType )
        {
            return source.Provider.Execute( elementType, source.Properties );
        }
    }
}

