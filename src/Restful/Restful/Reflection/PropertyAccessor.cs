using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace Restful.Reflection
{
    public class PropertyAccessor : IPropertyAccessor
    {
        private IPropertyAccessor accessor;

        private Hashtable opCodes;

        public PropertyInfo PropertyInfo { get; private set; }

        #region PropertyAccessor
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="propertyInfo"></param>
        public PropertyAccessor( PropertyInfo propertyInfo  )
        {
            // 如果属性信息不存在
            if( propertyInfo == null )
            {
                throw new ArgumentNullException( "propertyInfo" );
            }

            this.PropertyInfo = propertyInfo;
        }
        #endregion

        #region Init
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.opCodes = new Hashtable();

            this.opCodes[typeof( sbyte )] = OpCodes.Ldind_I1;
            this.opCodes[typeof( byte )] = OpCodes.Ldind_U1;
            this.opCodes[typeof( char )] = OpCodes.Ldind_U2;
            this.opCodes[typeof( short )] = OpCodes.Ldind_I2;
            this.opCodes[typeof( ushort )] = OpCodes.Ldind_U2;
            this.opCodes[typeof( int )] = OpCodes.Ldind_I4;
            this.opCodes[typeof( uint )] = OpCodes.Ldind_U4;
            this.opCodes[typeof( long )] = OpCodes.Ldind_I8;
            this.opCodes[typeof( ulong )] = OpCodes.Ldind_I8;
            this.opCodes[typeof( bool )] = OpCodes.Ldind_I1;
            this.opCodes[typeof( double )] = OpCodes.Ldind_R8;
            this.opCodes[typeof( float )] = OpCodes.Ldind_R4;

            this.accessor = this.CreateDynamicAssembly().CreateInstance( "Property" ) as IPropertyAccessor;

            if( this.accessor == null )
            {
                throw new Exception( "不能创建属性访问器。" );
            }
        }
        #endregion

        #region InitGet
        /// <summary>
        /// 初始化 Get 方法
        /// </summary>
        /// <param name="typeBuilder"></param>
        private void InitGet( TypeBuilder typeBuilder )
        {
            // 给类型定义 Get 方法
            MethodBuilder methodBuilder = typeBuilder.DefineMethod( "Get", MethodAttributes.Public | MethodAttributes.Virtual, typeof( object ), new Type[] { typeof( object ) } );

            // 获取 Get 方法的中间语言生成器
            ILGenerator ilgenerator = methodBuilder.GetILGenerator();

            // 获取属性的 Get 方法
            MethodInfo method = this.PropertyInfo.DeclaringType.GetMethod( "get_" + this.PropertyInfo.Name );

            if( method == null )
            {
                ilgenerator.ThrowException( typeof( MissingMethodException ) );
            }
            else
            {
                ilgenerator.DeclareLocal( typeof( object ) );

                ilgenerator.Emit( OpCodes.Ldarg_1 );
                ilgenerator.Emit( OpCodes.Castclass, this.PropertyInfo.DeclaringType );

                ilgenerator.EmitCall( OpCodes.Call, method, null );

                if( method.ReturnType.IsValueType )
                {
                    ilgenerator.Emit( OpCodes.Box, method.ReturnType );
                }

                ilgenerator.Emit( OpCodes.Stloc_0 );
                ilgenerator.Emit( OpCodes.Ldloc_0 );
            }

            ilgenerator.Emit( OpCodes.Ret );
        }
        #endregion

        #region InitSet
        /// <summary>
        /// 初始化 Set 方法
        /// </summary>
        /// <param name="typeBuilder"></param>
        private void InitSet( TypeBuilder typeBuilder )
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod( "Set", MethodAttributes.Public | MethodAttributes.Virtual, null, new Type[] { typeof( object ), typeof( object ) } );

            ILGenerator ilgenerator = methodBuilder.GetILGenerator();

            MethodInfo method = this.PropertyInfo.DeclaringType.GetMethod( "set_" + this.PropertyInfo.Name );

            if( method == null )
            {
                ilgenerator.ThrowException( typeof( MissingMethodException ) );
            }
            else
            {
                Type parameterType = method.GetParameters()[0].ParameterType;

                ilgenerator.DeclareLocal( parameterType );

                ilgenerator.Emit( OpCodes.Ldarg_1 );
                ilgenerator.Emit( OpCodes.Castclass, this.PropertyInfo.DeclaringType );
                ilgenerator.Emit( OpCodes.Ldarg_2 );

                if( parameterType.IsValueType )
                {
                    ilgenerator.Emit( OpCodes.Unbox, parameterType );

                    if( this.opCodes[parameterType] == null )
                    {
                        ilgenerator.Emit( OpCodes.Ldobj, parameterType );
                    }
                    else
                    {
                        OpCode load = (OpCode)this.opCodes[parameterType];
                        ilgenerator.Emit( load );
                    }
                }
                else
                {
                    ilgenerator.Emit( OpCodes.Castclass, parameterType );
                }

                ilgenerator.EmitCall( OpCodes.Callvirt, method, null );
            }

            ilgenerator.Emit( OpCodes.Ret );
        }
        #endregion

        #region CreateDynamicAssembly
        /// <summary>
        /// 创建动态的程序集
        /// </summary>
        /// <returns></returns>
        private Assembly CreateDynamicAssembly()
        {
            AssemblyName assemblyName = new AssemblyName( "PropertyAccessorAssembly" );

            // 定义动态程序集
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly( assemblyName, AssemblyBuilderAccess.Run );

            // 定义动态模块
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule( "PropertyAccessorModule" );

            // 定义一个名为 “Property” 的公共类
            TypeBuilder typeBuilder = moduleBuilder.DefineType( "Property", TypeAttributes.Public );

            // 添加接口实现
            typeBuilder.AddInterfaceImplementation( typeof( IPropertyAccessor ) );

            // 定义默认构造函数
            ConstructorBuilder constructorBuilder = typeBuilder.DefineDefaultConstructor( MethodAttributes.Public );

            this.InitGet( typeBuilder );

            this.InitSet( typeBuilder );

            typeBuilder.CreateType();

            return assemblyBuilder;
        }
        #endregion

        #region Get
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public object Get( object instance )
        {
            // 如果属性没有实现 Get 方法
            if( this.PropertyInfo.CanRead == false )
            {
                throw new Exception( string.Format( "属性 {0} 没有实现 Get 方法。", this.PropertyInfo.Name ) );
            }

            if( this.accessor == null )
            {
                this.Init();
            }

            return this.accessor.Get( instance );
        }
        #endregion

        #region Set
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public void Set( object instance, object value )
        {
            if( this.PropertyInfo.CanWrite == false )
            {
                throw new Exception( string.Format( "属性 {0} 没有实现 Set 方法。", this.PropertyInfo.Name ) );
            }
            else
            {
                if( this.accessor == null )
                {
                    this.Init();
                }

                this.accessor.Set( instance, value );
            }
        }
        #endregion
    }
}