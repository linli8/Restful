using System.Collections.Generic;
using System.Reflection;
using Restful.Collections.Generic;

namespace Restful.Reflection
{
    internal static class PropertyAccessorCache
    {
        /// <summary>
        /// 属性访问器集合
        /// </summary>
        private static IDictionary<PropertyInfo, PropertyAccessor> accessors;

        static PropertyAccessorCache()
        {
            accessors = new ThreadSafeDictionary<PropertyInfo, PropertyAccessor>();
        }

        public static PropertyAccessor Get( PropertyInfo propertyInfo )
        {
            if( accessors.ContainsKey( propertyInfo ) )
            {
                return accessors[propertyInfo];
            }

            return null;
        }

        public static void Set( PropertyInfo propertyInfo, PropertyAccessor propertyAccessor )
        {
            accessors.Add( propertyInfo, propertyAccessor );
        }
    }
}
