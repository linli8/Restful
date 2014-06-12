using System;
using System.ComponentModel;
using System.Data;

namespace Restful.Data
{
    /// <summary>
    /// 命令参数
    /// </summary>
    public class CommandParameter
    {
        #region Properties

        /// <summary>
        /// 获取或设置参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 获取或设置参数方向
        /// </summary>
        public ParameterDirection Direction { get; set; }

        /// <summary>
        /// 获取或设置参数大小
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 获取或设置参数值
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommandParameter()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        public CommandParameter( string parameterName )
        {
            this.ParameterName = parameterName;
        }

        public CommandParameter( string parameterName, object value )
        {
            this.ParameterName = parameterName;
            this.Value = value;
        }

        #endregion
    }
}

