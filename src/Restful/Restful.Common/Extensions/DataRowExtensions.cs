using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Restful.Common.Extensions
{
    public static class DataRowExtensions
    {
        #region ToObject<T>
        /// <summary>
        /// 将 DataRow 转换成指定类型的模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T ToObject<T>( this DataRow source )
        {
            // 判断 T 是否为匿名类型
            if( typeof( T ).IsAnonymousType() )
            {
                return source.AnonymousTypeToObject<T>();
            }
            else
            {
                return source.NonanonymousTypeToObject<T>();
            }
        }
        #endregion

        #region AnonymousTypeToObject
        /// <summary>
        /// 将 DataRow 转换为匿名类型对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="source">DataRow 对象</param>
        /// <returns>匿名类型对象</returns>
        private static T AnonymousTypeToObject<T>( this DataRow source )
        {
            var constructor = typeof( T ).GetConstructors( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
                .OrderBy( c => c.GetParameters().Length )
                .First();

            var parameters = constructor.GetParameters();

            var values = new object[parameters.Length];

            for( int i = 0; i < parameters.Length; i++ )
            {
                ParameterInfo info = parameters[i];

                if( source.Table.Columns.Contains( info.Name ) == false ) continue;

                if( source[parameters[i].Name] == DBNull.Value ) continue;

                values[i] = source[info.Name].Cast( info.ParameterType );
            }

            return constructor.Invoke( values ).Cast<T>();
        }
        #endregion

        #region NonanonymousTypeToObject
        /// <summary>
        /// 将 DataRow 转换为非匿名类型对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="source">DataRow 对象</param>
        /// <returns>非匿名类型对象</returns>
        private static T NonanonymousTypeToObject<T>( this DataRow source )
        {
            T target = (T)Activator.CreateInstance( typeof( T ) );

            foreach( DataColumn column in source.Table.Columns )
            {
                PropertyInfo info = typeof( T ).GetProperty( column.ColumnName );

                if( info == null ) continue;

                if( source[column.ColumnName] != DBNull.Value )
                {
                    object value = source[column.ColumnName].Cast( info.PropertyType );
                    info.SetValue( target, value, null );
                }
            }

            return target;
        }
        #endregion
    }
}
