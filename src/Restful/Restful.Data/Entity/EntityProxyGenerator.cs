using System;
using Castle.DynamicProxy;
using Restful.Data.Entity;
using System.Reflection;
using Restful.Reflection.Emit;

namespace Restful.Data.Entity
{
    public static class EntityProxyGenerator
    {
        private static ProxyGenerator generator;
        private static ProxyGenerationOptions options;

        static EntityProxyGenerator()
        {
            generator = new ProxyGenerator();
            options = new ProxyGenerationOptions(new EntityInterceptorFilter()) { Selector = new EntityInterceptorSelector() };
            options.AddMixinInstance(new EntityObject());
        }

        /// <summary>
        /// 创建指定类型的实体代理对象
        /// </summary>
        /// <returns>实体代理对象</returns>
        /// <param name="type">类型</param>
        public static object CreateProxy( Type @type)
        {
            return generator.CreateClassProxy(@type, options, new EntityInterceptor());
        }

        /// <summary>
        /// 创建指定类型的实体代理对象
        /// </summary>
        /// <returns>实体代理对象</returns>
        /// <typeparam name="T">类型</typeparam>
        public static T CreateProxy<T>()
        {
            return (T)generator.CreateClassProxy( typeof(T), options, new EntityInterceptor());
        }

        /// <summary>
        /// 创建指定对象的实体代理对象
        /// </summary>
        /// <returns>实体代理对象</returns>
        /// <param name="object">对象</param>
        public static object CreateProxy(object @object)
        {
            object target = CreateProxy(@object.GetType());

            var handler = DynamicHelper.CreateDynamicMapHandler(@object.GetType(), target.GetType());

            handler(@object, target);

            ((IEntityObject)target).Reset();

            return target;
        }
            
        /// <summary>
        /// 创建指定对象的实体代理对象
        /// </summary>
        /// <returns>实体代理对象</returns>
        /// <param name="object">对象</param>
        /// <typeparam name="T"></typeparam>
        public static T CreateProxy<T>( T @object )
        {
            T target = CreateProxy<T>();

            var handler = DynamicHelper.CreateDynamicMapHandler(typeof(T), target.GetType());

            handler(@object, target);

            ((IEntityObject)target).Reset();

            return target;
        }
    }
}

