using System.Reflection;
using Restful.Reflection;

namespace Restful.Extensions
{
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// 获取属性访问器
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static PropertyAccessor GetEmitAccessor( this PropertyInfo propertyInfo )
        {
            PropertyAccessor accessor = PropertyAccessorCache.Get( propertyInfo );

            if( accessor == null )
            {
                accessor = new PropertyAccessor( propertyInfo );

                PropertyAccessorCache.Set( propertyInfo, accessor );
            }

            return accessor;
        }

        /// <summary>
        /// 使用 Emit 方式获取属性值
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static object EmitGetValue( this PropertyInfo propertyInfo, object instance )
        {
            return propertyInfo.GetEmitAccessor().Get( instance );
        }

        /// <summary>
        /// 通过 Emit 方式设置属性值
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public static void EmitSetValue( this PropertyInfo propertyInfo, object instance, object value )
        {
            propertyInfo.GetEmitAccessor().Set( instance, value );
        }
    }
}
