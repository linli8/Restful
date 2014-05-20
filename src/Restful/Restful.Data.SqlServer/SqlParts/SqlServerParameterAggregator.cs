using System.Collections.Generic;
using Restful.Data.SqlServer.Common;

namespace Restful.Data.SqlServer.SqlParts
{
    /// <summary>
    /// 查询参数聚合器
    /// </summary>
    internal class SqlServerParameterAggregator
    {
        /// <summary>
        /// 参数字典
        /// </summary>
        private readonly IDictionary<string, object> parameters;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlServerParameterAggregator()
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
