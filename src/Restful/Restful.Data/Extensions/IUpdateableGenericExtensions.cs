using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Restful.Data;

namespace Restful.Linq
{
    public static class IUpdateableGenericExtensions
    {
        /// <summary>
        /// 设置需更新的实体对象
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="object">需更新的实体对象</param>
        /// <typeparam name="T">元素类型</typeparam>
        public static IUpdateable<T> Set<T>( this IUpdateable<T> source, object @object )
        {
            source.Properties.Clear();

            IEntityObject entity = (IEntityObject)@object;

            Type elementType = @object.GetType().BaseType;

            var properties = @object.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance );

            properties.Where( property => property.IsPrimaryKey() == false ).Each( property =>
            {
                if( entity.ChangedProperties.Contains( property.Name ) )
                {
                    ParameterExpression pe = Expression.Parameter( @object.GetType(), "s" );
                    MemberExpression me = Expression.MakeMemberAccess( pe, property );

                    source.Set( me, property.EmitGetValue( @object ) );
                }
            } );

            return source;
        }
    }
}

