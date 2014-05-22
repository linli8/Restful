using System.Reflection;
using Restful.Reflection;
using Restful.Reflection.Emit;

namespace Restful.Extensions
{
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// 使用 Emit 方式获取属性值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static object EmitGetValue( this PropertyInfo property, object instance )
        {
            DynamicPropertyGetHandler handler = DynamicHelper.CreateDynamicPropertyGetHandler(property);

            return handler(instance);
        }

        /// <summary>
        /// 通过 Emit 方式设置属性值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public static void EmitSetValue( this PropertyInfo property, object instance, object value )
        {
            DynamicPropertySetHandler handler = DynamicHelper.CreateDynamicPropertySetHandler(property);

            handler(instance, value);
        }
    }
}
