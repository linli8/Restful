using System;
using System.Linq.Expressions;
using System.Linq;
using Restful;

namespace Restful.Linq
{
    public static class IUpdateableExtensions
    {
        public static IUpdateable Set( this IUpdateable source, MemberExpression me, object value )
        {
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

        public static IUpdateable Where( this IUpdateable source, Expression predicate )
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

        public static int Execute( this IUpdateable source, Type elementType )
        {
            return source.Provider.Execute( elementType, source.Properties, source.Predicate );
        }
    }
}

