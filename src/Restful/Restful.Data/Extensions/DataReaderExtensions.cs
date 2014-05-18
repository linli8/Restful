using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Restful.Data.Entity;
using Restful.Extensions;

namespace Restful.Data.Extensions
{
    public static class DataReaderExtensions
    {
        #region ToObject
        /// <summary>
        /// 将 DbDataReader 当前行转换成指定类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static T ToObject<T>( this DbDataReader dataReader )
        {
            if( typeof( T ).IsAnonymousType() )
            {
                return dataReader.ToAnonymousTypeObject<T>();
            }
            else
            {
                return dataReader.ToNonAnonymousTypeObject<T>();
            }
        }
        #endregion

        #region ToNonAnonymousTypeObject
        /// <summary>
        /// 将 DbDataReader 当前行转换成非匿名类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static T ToNonAnonymousTypeObject<T>( this DbDataReader dataReader )
        {
            var properties = typeof( T ).GetProperties( BindingFlags.Public | BindingFlags.Instance );

            T instance = Activator.CreateInstance<T>();

            for( int i = 0; i < dataReader.VisibleFieldCount; i++ )
            {
                var property = properties.Where( s => s.Name == dataReader.GetName( i ) ).FirstOrDefault();

                if( property == null ) continue;

                object value = dataReader[i];

                if( value != DBNull.Value )
                {
                    value = value.Cast( property.PropertyType );

                    property.EmitSetValue( instance, value );
                }
            }

            if( instance is EntityObject )
            {
                ( instance as EntityObject ).Reset();
            }

            return instance;
        }
        #endregion

        #region ToAnonymousTypeObject
        /// <summary>
        /// 将 DbDataReader 当前行转换成匿名对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static T ToAnonymousTypeObject<T>( this DbDataReader dataReader )
        {
            var constructor = typeof( T ).GetConstructors( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
                .OrderBy( c => c.GetParameters().Length ).First();

            var parameters = constructor.GetParameters();

            var values = new object[parameters.Length];

            for( int i = 0; i < dataReader.VisibleFieldCount; i++ )
            {
                ParameterInfo parameter = parameters.Where( s => s.Name == dataReader.GetName( i ) ).FirstOrDefault();

                if( parameter == null ) continue;

                object value = dataReader[i];

                if( value == null || value == DBNull.Value ) continue;

                values[i] = value.Cast( parameter.ParameterType );
            }

            return constructor.Invoke( values ).Cast<T>();
        }
        #endregion

        #region ToObjects<T>
        /// <summary>
        /// 将 DataReader 对象转换成实体列表对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static IList<T> ToObjects<T>( this DbDataReader dataReader )
        {
            if( typeof(T).IsAnonymousType() )
            {
                return dataReader.ToAnonymousTypeObjects<T>();
            }
            else
            {
                return dataReader.ToNonAnonymousTypeObjects<T>();
            }
        }
        #endregion

        #region ToNonAnonymousTypeObjects
        /// <summary>
        /// 将 DbDataReader 对象转换成非匿名类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static IList<T> ToNonAnonymousTypeObjects<T>( this DbDataReader dataReader )
        {
            if( dataReader.HasRows == false ) return null;

            IList<T> @objects = new List<T>();

            var properties = typeof( T ).GetProperties( BindingFlags.Public | BindingFlags.Instance );

            while( dataReader.Read() )
            {
                T instance = Activator.CreateInstance<T>();

                for( int i = 0; i < dataReader.VisibleFieldCount; i++ )
                {
                    var property = properties.Where( s => s.Name == dataReader.GetName( i ) ).FirstOrDefault();

                    if( property == null ) continue;

                    object value = dataReader[i];

                    if( value != DBNull.Value )
                    {
                        value = value.Cast( property.PropertyType );

                        property.EmitSetValue( instance, value );
                    }
                }

                if( instance is EntityObject )
                {
                    ( instance as EntityObject ).Reset();
                }

                @objects.Add( instance );
            }

            return @objects;
        }
        #endregion

        #region ToAnonymousTypeObjects
        /// <summary>
        /// 将 DbDataReader 对象转换成匿名类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static IList<T> ToAnonymousTypeObjects<T>( this DbDataReader dataReader )
        {
            if( dataReader.HasRows == false ) return null;

            IList<T> @objects = new List<T>();

            var constructor = typeof( T ).GetConstructors( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
                .OrderBy( c => c.GetParameters().Length ).First();

            var parameters = constructor.GetParameters();

            while( dataReader.Read() )
            {
                var values = new object[parameters.Length];

                for( int i = 0; i < dataReader.VisibleFieldCount; i++ )
                {
                    ParameterInfo parameter = parameters.Where( s => s.Name == dataReader.GetName( i ) ).FirstOrDefault();

                    if( parameter == null ) continue;

                    object value = dataReader[i];

                    if( value == null || value == DBNull.Value ) continue;

                    values[i] = value.Cast( parameter.ParameterType );
                }

                T instance = constructor.Invoke( values ).Cast<T>();

                @objects.Add( instance );
            }

            return @objects;
        }
        #endregion
    }
}
