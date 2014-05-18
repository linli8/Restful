using System;
using System.Linq.Expressions;

namespace Restful.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 判断类型是否为匿名类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAnonymousType( this Type @type )
        {
            return @type.Namespace == null;
        }

        /// <summary>
        /// 返回创建指定类型实例对象的委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Func<object> CreateInstanceHandler( this Type @type )
        {
            NewExpression ne = Expression.New( @type );

            Expression<Func<object>> expression = Expression.Lambda<Func<object>>( ne, null );

            Func<object> func = expression.Compile();

            return func;
        }
    }
}
