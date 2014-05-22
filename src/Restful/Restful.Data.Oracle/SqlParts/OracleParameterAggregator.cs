using System.Collections.Generic;
using Restful.Data.Oracle.Common;

namespace Restful.Data.Oracle.SqlParts
{
    /// <summary>
    /// 查询参数聚合器
    /// </summary>
    internal class OracleParameterAggregator
    {
        /// <summary>
        /// 参数字典
        /// </summary>
        private readonly IDictionary<string, object> parameters;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OracleParameterAggregator()
        {
            this.parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string AddParameter( object value )
        {
            string parameterName = string.Format( "{0}P{1}", Constants.ParameterPrefix, this.parameters.Count );

            this.parameters.Add( parameterName, value );

            return parameterName;
        }

        /// <summary>
        /// 获取聚合器中所有的参数
        /// </summary>
        public IDictionary<string, object> Parameters
        {
            get
            {
                return this.parameters;
            }
        }
    }
}
