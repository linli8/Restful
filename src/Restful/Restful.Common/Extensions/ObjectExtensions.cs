using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Common.Extensions
{
    public static class ObjectExtensions
    {
        #region Cast
        /// <summary>
        /// 将对象转换为指定类型的对象
        /// </summary>
        /// <param name="source">原对象</param>
        /// <param name="targetType">目标类型</param>
        /// <returns>转换后的对象</returns>
        public static object Cast( this object source, Type targetType )
        {
            if( ( source == null ) || ( source.GetType() == typeof( DBNull ) ) )
            {
                return null;
            }

            if( targetType.IsGenericType && targetType.GetGenericTypeDefinition().Equals( typeof( Nullable<> ) ) )
            {
                targetType = Nullable.GetUnderlyingType( targetType );
            }

            return Convert.ChangeType( source, targetType );
        }
        #endregion

        #region Cast<T>
        /// <summary>
        /// 将对象转换成指定对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="source">原对象</param>
        /// <returns>转换后的对象</returns>
        public static T Cast<T>( this object source )
        {
            return (T)source.Cast( typeof( T ) );
        }
        #endregion

        #region IsNullOrEmpty
        /// <summary>
        /// 判断对象是否为空对象、空字符串、DBNull
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty( this object source )
        {
            if( source == null )
            {
                return true;
            }

            if( source is string )
            {
                return string.IsNullOrEmpty( source.ToString() );
            }

            if( source is DBNull )
            {
                return source == DBNull.Value;
            }

            return false;
        }
        #endregion
    }
}
