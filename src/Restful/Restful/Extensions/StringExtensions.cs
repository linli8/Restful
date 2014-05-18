using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Restful.Extensions
{
    public static class StringExtensions
    {
        #region SplitPascalCase
        /// <summary>
        /// 将符合Pascal命名标准的字符串分割为多个单词
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string SplitPascalCase( this string @object )
        {
            if( string.IsNullOrEmpty( @object ) ) return @object;

            if( @object.ToUpper() == "ID" ) return @object;

            return Regex.Replace( @object, "([A-Z])", " $1", RegexOptions.Compiled ).Trim();
        }
        #endregion
    }
}
