using System;
using Castle.DynamicProxy;
using System.Reflection;
using Restful.Reflection.Emit;
using System.Security;

namespace Restful.Data
{
    /// <summary>
    /// 实体代理对象生成器
    /// </summary>
    public static class EntityHelper
    {
        #region Constants

        private const string ASSEMBLY_NAME = "Restful.Data.Entity.Proxies.dll";
        private const string MODULE_PATH = "Restful.Data.Entity.Proxies";

        #endregion

        #region Members

        private static ProxyGenerator generator;
        private static ProxyGenerationOptions options;

        #endregion

        #region Constructor

        static EntityHelper()
        {
            var scope = new ModuleScope( false, false, new EntityProxyNameScope(), ASSEMBLY_NAME, MODULE_PATH, ASSEMBLY_NAME, MODULE_PATH );
            
            generator = new ProxyGenerator( new DefaultProxyBuilder( scope ) );

            options = new ProxyGenerationOptions( new EntityInterceptorFilter() ) { Selector = new EntityInterceptorSelector() };

            options.AddMixinInstance( new EntityObject() );
        }

        #endregion

        #region CreateProxy

        /// <summary>
        /// 创建指定类型的实体代理对象
        /// </summary>
        /// <returns>实体代理对象</returns>
        /// <param name="type">类型</param>
        public static object CreateProxy( Type @type )
        {
            return generator.CreateClassProxy( @type, options, new EntityInterceptor() );
        }

        /// <summary>
        /// 创建指定类型的实体代理对象
        /// </summary>
        /// <returns>实体代理对象</returns>
        /// <typeparam name="T">类型</typeparam>
        public static T CreateProxy<T>()
        {
            return (T)CreateProxy( typeof( T ) );
            //return (T)generator.CreateClassProxy( typeof( T ), options, new EntityInterceptor() );
        }

        /// <summary>
        /// 创建指定对象的实体代理对象
        /// </summary>
        /// <returns>实体代理对象</returns>
        /// <param name="object">对象</param>
        public static object CreateProxy( object @object )
        {
            object target = CreateProxy( @object.GetType() );

            var handler = DynamicHelper.CreateDynamicMapHandler( @object.GetType(), target.GetType() );

            handler( @object, target );

            ( (IEntityObject)target ).Reset();

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

            var handler = DynamicHelper.CreateDynamicMapHandler( typeof( T ), target.GetType() );

            handler( @object, target );

            ( (IEntityObject)target ).Reset();

            return target;
        }

        #endregion

        #region CompileDynamicProxyTypes

        /// <summary>
        /// 编译实体动态代理并缓存
        /// </summary>
        /// <param name="types">Types.</param>
        public static void CompileDynamicProxyTypes( params Type[] types )
        {
            if( types == null || types.Length == 0 )
            {
                return;
            }

            foreach( var type in types )
            {
                var @object = Activator.CreateInstance( type );

                var proxy = CreateProxy( @object );

                @object = null;

                proxy = null;
            }
        }

        #endregion
    }
}

