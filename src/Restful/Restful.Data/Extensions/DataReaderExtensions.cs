using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Restful.Data.Dapper;
using Restful.Data.Entity;
using Restful.Extensions;

namespace Restful.Data.Extensions
{
    public static class DataReaderExtensions
    {
        /// <summary>
        /// 获取 DeserializerState 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static SqlMapper.DeserializerState GetDeserializerState<T>(this IDataReader reader)
        {
            int hash = SqlMapper.GetColumnHash( reader );

            return new SqlMapper.DeserializerState( hash, SqlMapper.GetDeserializer( typeof( T ), reader, 0, -1, false ) );
        }
    }
}
