using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Restful
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

        public static Type GetElementType( this Type seqType )
        {
            Type ienum = seqType.FindIEnumerable();

            if( ienum == null )
                return seqType;

            return ienum.GetGenericArguments()[0];
        }

        public static Type FindIEnumerable( this Type seqType )
        {
            if( seqType == null || seqType == typeof( string ) )
                return null;

            if( seqType.IsArray )
            {
                return typeof( IEnumerable<> ).MakeGenericType( seqType.GetElementType() );
            }

            if( seqType.IsGenericType )
            {
                foreach( Type arg in seqType.GetGenericArguments() )
                {
                    Type ienum = typeof( IEnumerable<> ).MakeGenericType( arg );

                    if( ienum.IsAssignableFrom( seqType ) )
                    {
                        return ienum;
                    }
                }
            }

            Type[] ifaces = seqType.GetInterfaces();

            if( ifaces != null && ifaces.Length > 0 )
            {
                foreach( Type iface in ifaces )
                {
                    Type ienum = FindIEnumerable( iface );

                    if( ienum != null )
                        return ienum;
                }
            }

            if( seqType.BaseType != null && seqType.BaseType != typeof( object ) )
            {
                return FindIEnumerable( seqType.BaseType );
            }

            return null;
        }
    }
}
