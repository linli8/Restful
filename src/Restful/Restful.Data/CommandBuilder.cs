using System;
using System.Text;
using System.Collections.Generic;
using Remotion.Linq.Clauses.ResultOperators;
using System.Collections;

namespace Restful.Data
{
    /// <summary>
    /// SQL 命令构造器
    /// </summary>
    public abstract class CommandBuilder
    {
        #region Members

        /// <summary>
        /// 参数列表
        /// </summary>
        protected IDictionary<string,object> parameters;

        #endregion

        #region Properties

        /// <summary>
        /// 获取 SQL 语句
        /// </summary>
        /// <value>The sql.</value>
        public abstract string Sql { get; set; }

        /// <summary>
        /// 获取 SQL 命令构造器中的参数列表
        /// </summary>
        /// <value>The parameters.</value>
        public IDictionary<string,object> Parameters { get { return this.parameters; } }

        #endregion

        #region Constructor

        public CommandBuilder()
        {
            this.parameters = new Dictionary<string,object>();
        }

        public CommandBuilder( IDictionary<string,object> parameters )
        {
            this.parameters = parameters;
        }

        #endregion

        #region AddParameter

        /// <summary>
        /// 添加命名参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">参数值</param>
        public virtual void AddParameter( string parameterName, object value )
        {
            this.parameters.Add( parameterName, value );
        }

        /// <summary>
        /// 添加匿名参数
        /// </summary>
        /// <returns>参数名</returns>
        /// <param name="value">参数值</param>
        public abstract string AddParameter( object value );

        #endregion

        #region ToString

        /// <summary>
        /// 重载基类 ToString 方法
        /// </summary>
        /// <returns>SQL 语句</returns>
        public override string ToString()
        {
            return this.Sql;
        }

        #endregion
    }
}

