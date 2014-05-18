using System;

namespace Restful.Extensions
{
    public static class ObjectExtensions
    {
        #region Cast
        /// <summary>
        /// 将对象转换为指定类型的对象
        /// </summary>
        /// <param name="object">原对象</param>
        /// <param name="targetType">目标类型</param>
        /// <returns>转换后的对象</returns>
        public static object Cast( this object @object, Type targetType )
        {
            if( ( @object == null ) || ( @object.GetType() == typeof( DBNull ) ) )
            {
                return null;
            }

            if( targetType.IsGenericType && targetType.GetGenericTypeDefinition().Equals( typeof( Nullable<> ) ) )
            {
                targetType = Nullable.GetUnderlyingType( targetType );
            }

            return Convert.ChangeType( @object, targetType );
        }
        #endregion

        #region Cast<T>
        /// <summary>
        /// 将对象转换成指定对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="object">原对象</param>
        /// <returns>转换后的对象</returns>
        public static T Cast<T>( this object @object )
        {
            return (T)@object.Cast( typeof( T ) );
        }
        #endregion

        #region IsNullOrEmpty
        /// <summary>
        /// 判断对象是否为空对象、空字符串、DBNull
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty( this object @object )
        {
            if( @object == null )
            {
                return true;
            }

            if( @object is string )
            {
                return string.IsNullOrEmpty( @object.ToString() );
            }

            if( @object is DBNull )
            {
                return @object == DBNull.Value;
            }

            return false;
        }
        #endregion
    }
}
