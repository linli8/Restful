using System;
using System.Reflection;
using Restful.Data.Attributes;

namespace System.Reflection
{
    public static class PropertyInfoExtensions
    {

        #region PropertyInfo Extensions

        /// <summary>
        /// 判断属性是否为主键属性
        /// </summary>
        /// <returns><c>true</c> if is primary key the specified property; otherwise, <c>false</c>.</returns>
        /// <param name="property">Property.</param>
        public static bool IsPrimaryKey( this PropertyInfo property )
        {
            return Attribute.GetCustomAttributes( property, typeof( PrimaryKeyAttribute ), true ).Length > 0;
        }

        /// <summary>
        /// 判断属性是否为自增长属性
        /// </summary>
        /// <returns><c>true</c> if is auto increase the specified property; otherwise, <c>false</c>.</returns>
        /// <param name="property">Property.</param>
        public static bool IsAutoIncrease( this PropertyInfo property )
        {
            return Attribute.GetCustomAttributes( property, typeof( AutoIncreaseAttribute ), true ).Length > 0;
        }

        #endregion
    }
}

