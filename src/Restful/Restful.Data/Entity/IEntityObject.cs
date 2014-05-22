using System;
using System.Collections.Generic;

namespace Restful.Data.Entity
{
    public interface IEntityObject
    {
        /// <summary>
        /// 获取已改变的属性集合
        /// </summary>
        /// <value>The changed properties.</value>
        IList<string> ChangedProperties { get;}

        /// <summary>
        /// 当属性改变时记录属性名称
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        void OnPropertyChanged( string propertyName );

        /// <summary>
        /// 将已改变属性集合清空
        /// </summary>
        void Reset();
    }
}

