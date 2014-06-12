using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Data.Attributes
{
    /// <summary>
    /// 标记属性对应的数据库字段是否为主键
    /// </summary>
    [AttributeUsage( AttributeTargets.Property, Inherited = true )]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}
