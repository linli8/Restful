using System;
using Restful.Data;

namespace System
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 将对象转换为实体代理
        /// </summary>
        /// <returns>实体代理</returns>
        /// <param name="object">对象</param>
        /// <typeparam name="T"></typeparam>
        public static T ToEntityProxy<T>( this T @object ) where T : class
        {
            return EntityHelper.CreateProxy<T>( @object );
        }
    }
}

