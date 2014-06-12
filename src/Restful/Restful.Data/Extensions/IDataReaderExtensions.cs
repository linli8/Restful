using System;
using System.Data;
using Restful.Data.Dapper;

namespace System.Data
{
    public static class IDataReaderExtensions
    {
        /// <summary>
        /// 获取 DeserializerState 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static SqlMapper.DeserializerState GetDeserializerState<T>( this IDataReader reader )
        {
            int hash = SqlMapper.GetColumnHash( reader );

            return new SqlMapper.DeserializerState( hash, SqlMapper.GetDeserializer( typeof( T ), reader, 0, -1, false ) );
        }
    }
}

