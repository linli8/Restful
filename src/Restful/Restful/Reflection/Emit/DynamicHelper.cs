using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Restful.Collections.Generic;
using System.Collections;

namespace Restful.Reflection.Emit
{
    /// <summary>
    /// 动态映射对象属性的委托
    /// </summary>
    public delegate void DynamicMapHandler( object source, object target );

    /// <summary>
    /// 动态设置对象属性值的委托
    /// </summary>
    public delegate void DynamicPropertySetHandler( object instance, object value );

    /// <summary>
    /// 动态获取对象属性值的委托
    /// </summary>
    public delegate object DynamicPropertyGetHandler( object instance );

    /// <summary>
    /// 动态代码辅助类
    /// </summary>
    public static class DynamicHelper
    {
        private static Hashtable typeMap;

        private static IDictionary<Type,DynamicMapHandler> dynamicMapHandlers = new ThreadSafeDictionary<Type,DynamicMapHandler>();
        private static IDictionary<PropertyInfo,DynamicPropertyGetHandler> dynamicPropertyGetHandlers = new ThreadSafeDictionary<PropertyInfo,DynamicPropertyGetHandler>();
        private static IDictionary<PropertyInfo,DynamicPropertySetHandler> dynamicPropertySetHandlers = new ThreadSafeDictionary<PropertyInfo,DynamicPropertySetHandler>();

        #region DynamicHelper
        /// <summary>
        /// 
        /// </summary>
        static DynamicHelper()
        {
            InitTypeMap();
        }
        #endregion

        #region InitTypeMap
        /// <summary>
        /// Inits the type map.
        /// </summary>
        private static void InitTypeMap()
        {
            typeMap = new Hashtable();

            typeMap[typeof( sbyte )] = OpCodes.Ldind_I1;
            typeMap[typeof( byte )] = OpCodes.Ldind_U1;
            typeMap[typeof( char )] = OpCodes.Ldind_U2;
            typeMap[typeof( short )] = OpCodes.Ldind_I2;
            typeMap[typeof( ushort )] = OpCodes.Ldind_U2;
            typeMap[typeof( int )] = OpCodes.Ldind_I4;
            typeMap[typeof( uint )] = OpCodes.Ldind_U4;
            typeMap[typeof( long )] = OpCodes.Ldind_I8;
            typeMap[typeof( ulong )] = OpCodes.Ldind_I8;
            typeMap[typeof( bool )] = OpCodes.Ldind_I1;
            typeMap[typeof( double )] = OpCodes.Ldind_R8;
            typeMap[typeof( float )] = OpCodes.Ldind_R4;
        }
        #endregion

        #region CreateDynamicMapHandler
        /// <summary>
        /// 创建动态映射对象属性的委托
        /// </summary>
        /// <returns>动态映射对象属性的委托</returns>
        /// <param name="sourceType">对象类型</param>
        /// <param name="targetType">目标类型</param>
        public static DynamicMapHandler CreateDynamicMapHandler(Type sourceType,Type targetType)
        {
            // 如果缓存中存在
            if (dynamicMapHandlers.ContainsKey(sourceType))
            {
                return dynamicMapHandlers[sourceType];
            }

            DynamicMethod method = new DynamicMethod(string.Format("DynamicMap{0}", Guid.NewGuid()), null, new[] { typeof(object), typeof(object) }, true);

            ILGenerator gen = method.GetILGenerator();

            PropertyInfo[] properties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                // Writing body
                gen.Emit(OpCodes.Nop);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Callvirt, property.GetGetMethod());
                gen.Emit(OpCodes.Callvirt, targetType.GetProperty(property.Name).GetSetMethod());
            }

            gen.Emit(OpCodes.Nop);
            gen.Emit(OpCodes.Ret);

            DynamicMapHandler handler = (DynamicMapHandler)method.CreateDelegate(typeof(DynamicMapHandler));

            dynamicMapHandlers.Add(sourceType, handler);

            return handler;
        }
        #endregion

        #region CreateDynamicPropertyGetHandler
        /// <summary>
        /// 创建动态属性取值的委托
        /// </summary>
        /// <returns>动态属性取值的委托</returns>
        /// <param name="property">属性</param>
        public static DynamicPropertyGetHandler CreateDynamicPropertyGetHandler(PropertyInfo property)
        {
            // 如果缓存中存在
            if (dynamicPropertyGetHandlers.ContainsKey(property))
            {
                return dynamicPropertyGetHandlers[property];
            }

            DynamicMethod method = new DynamicMethod(string.Format("DynamicPropertyGet{0}", Guid.NewGuid()), typeof(object), new[] { typeof(object) }, true);

            ILGenerator gen = method.GetILGenerator();

            gen.DeclareLocal( typeof( object ) );

            gen.Emit( OpCodes.Ldarg_0 );
            gen.Emit( OpCodes.Castclass, property.DeclaringType );

            gen.EmitCall( OpCodes.Call, property.GetGetMethod(), null );

            if( property.GetGetMethod().ReturnType.IsValueType )
            {
                gen.Emit( OpCodes.Box, property.GetGetMethod().ReturnType );
            }

            gen.Emit( OpCodes.Stloc_0 );
            gen.Emit( OpCodes.Ldloc_0 );

            gen.Emit(OpCodes.Ret);

            DynamicPropertyGetHandler handler = (DynamicPropertyGetHandler)method.CreateDelegate(typeof(DynamicPropertyGetHandler));

            //DynamicPropertyGetHandler handler = (DynamicPropertyGetHandler)Delegate.CreateDelegate(typeof(DynamicPropertyGetHandler), newType.GetMethod("DynamicPropertyGet"));

            dynamicPropertyGetHandlers.Add(property, handler);

            return handler;
        }
        #endregion

        #region CreateDynamicPropertySetHandler
        /// <summary>
        /// 创建动态属性赋值的委托
        /// </summary>
        /// <returns>动态属性赋值的委托</returns>
        /// <param name="property">属性</param>
        public static DynamicPropertySetHandler CreateDynamicPropertySetHandler(PropertyInfo property)
        {
            // 如果缓存中存在
            if (dynamicPropertySetHandlers.ContainsKey(property))
            {
                return dynamicPropertySetHandlers[property];
            }

            DynamicMethod method = new DynamicMethod(string.Format("DynamicPropertySet{0}", Guid.NewGuid()), null, new[] { typeof(object), typeof(object) }, true);

            ILGenerator gen = method.GetILGenerator();

            Type parameterType = property.GetSetMethod().GetParameters()[0].ParameterType;

            gen.DeclareLocal(parameterType);

            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Castclass, property.DeclaringType);
            gen.Emit(OpCodes.Ldarg_1);

            if (parameterType.IsValueType)
            {
                gen.Emit(OpCodes.Unbox, parameterType);

                if (typeMap[parameterType] == null)
                {
                    gen.Emit(OpCodes.Ldobj, parameterType);
                }
                else
                {
                    OpCode load = (OpCode)typeMap[parameterType];
                    gen.Emit(load);
                }
            }
            else
            {
                gen.Emit(OpCodes.Castclass, parameterType);
            }

            gen.EmitCall(OpCodes.Callvirt, property.GetSetMethod(), null);

            gen.Emit(OpCodes.Ret);

            DynamicPropertySetHandler handler = (DynamicPropertySetHandler)method.CreateDelegate(typeof(DynamicPropertySetHandler));

            dynamicPropertySetHandlers.Add(property, handler);

            return handler;
        }
        #endregion


    }
}
