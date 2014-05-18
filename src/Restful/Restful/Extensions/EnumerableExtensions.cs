using System;
using System.Collections.Generic;

namespace Restful.Extensions
{
    public static class EnumerableExtensions
    {
        #region Each
        /// <summary>
        /// 遍历 IEnumerable 对象，并执行操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void Each<T>( this IEnumerable<T> enumerable, Action<T> action )
        {
            foreach( var item in enumerable )
            {
                action( item );
            }
        }
        #endregion
    }
}
