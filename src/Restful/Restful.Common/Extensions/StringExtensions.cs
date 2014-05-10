using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Restful.Common.Extensions
{
    public static class StringExtensions
    {
        #region SplitPascalCase
        /// <summary>
        /// 将符合Pascal命名标准的字符串分割为多个单词
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SplitPascalCase( this string source )
        {
            if( string.IsNullOrEmpty( source ) ) return source;

            if( source.ToUpper() == "ID" ) return source;

            return Regex.Replace( source, "([A-Z])", " $1", RegexOptions.Compiled ).Trim();
        }
        #endregion
    }
}
