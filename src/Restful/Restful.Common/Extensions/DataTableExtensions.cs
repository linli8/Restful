using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Restful.Common.Extensions
{
    public static class DataTableExtensions
    {
        /// <summary>
<<<<<<< HEAD
        /// 将 DataTable 转换成指定类型的对象列表
=======
        /// 将 DataTable 转换成指定类型的模型列表
>>>>>>> FETCH_HEAD
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IList<T> ToObjects<T>( this DataTable source )
        {
            IList<T> target = new List<T>();

<<<<<<< HEAD
            foreach( DataRow dr in source.Rows )
            {
                target.Add( dr.ToObject<T>() );
            }
=======
            source.AsEnumerable().Each( s =>
                {
                    target.Add( s.ToObject<T>() );
                } );
>>>>>>> FETCH_HEAD

            return target;
        }
    }
}
