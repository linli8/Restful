using System;
using Restful.Data.Entity;

namespace Restful.Data.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 将对象转换为实体代理
        /// </summary>
        /// <returns>实体代理</returns>
        /// <param name="object">对象</param>
        public static object ToEntityProxy(this object @object)
        {
            return EntityProxyGenerator.CreateProxy(@object);
        }

        /// <summary>
        /// 将对象转换为实体代理
        /// </summary>
        /// <returns>实体代理</returns>
        /// <param name="object">对象</param>
        /// <typeparam name="T"></typeparam>
        public static T ToEntityProxy<T>(this T @object)
        {
            return EntityProxyGenerator.CreateProxy<T>(@object);
        }
    }
}

