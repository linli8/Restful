using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Common.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 判断类型是否为匿名类型
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsAnonymousType( this Type source )
        {
            return source.Namespace == null;
        }
    }
}
