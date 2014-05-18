
namespace Restful.Reflection
{
    public interface IPropertyAccessor
    {
        /// <summary>
        /// 获取实例对象中指定属性的值
        /// </summary>
        /// <param name="instance">实例对象</param>
        /// <returns></returns>
        object Get( object instance );

        /// <summary>
        /// 设置实例对象中指定属性的值
        /// </summary>
        /// <param name="instance">实例对象</param>
        /// <param name="value">目标值</param>
        void Set( object instance, object value );
    }
}
