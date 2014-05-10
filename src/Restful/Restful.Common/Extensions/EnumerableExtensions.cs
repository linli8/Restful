using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Common.Extensions
{
    public static class EnumerableExtensions
    {
        #region Each
        /// <summary>
        /// 遍历 IEnumerable 对象，并执行执行的操作
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
