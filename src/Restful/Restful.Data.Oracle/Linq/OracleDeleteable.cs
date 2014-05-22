using System;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Data.Common;
using Restful.Data.Entity;
using Restful.Data.Linq;
using Restful.Data.Oracle.SqlParts;
using Restful.Data.Oracle.Visitors;
using System.Collections.Generic;

namespace Restful.Data.Oracle.Linq
{
    public class OracleDeleteable<T> : IDeleteable<T>
    {
        #region Member
        /// <summary>
        /// Session 提供程序
        /// </summary>
        private OracleSessionProvider provider;

        /// <summary>
        /// 参数聚合器
        /// </summary>
        private IList<object> parameters;

        /// <summary>
        /// DELETE 语句
        /// </summary>
        private OracleDeletePartsAggregator deletePartsAggregator;
        #endregion

        #region OracleDeleteable
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="provider"></param>
        public OracleDeleteable( OracleSessionProvider provider)
        {
            this.provider = provider;
            this.parameters = new List<object>();
            this.deletePartsAggregator = new OracleDeletePartsAggregator();
            this.deletePartsAggregator.TableName = typeof( T ).Name;
        }
        #endregion

        #region Where
        /// <summary>
        /// 设置 where 过滤条件
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public IDeleteable<T> Where( Expression<Func<T, bool>> func )
        {
            Expression expression = PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees( func );

            OracleWhereClauseVisitor visitor = new OracleWhereClauseVisitor( this.parameters );

            string whereSqlParts = visitor.Translate( expression );

            if( this.deletePartsAggregator.Where.Length == 0 )
            {
                this.deletePartsAggregator.Where.AppendFormat( " ( {0} )", whereSqlParts );
            }
            else
            {
                this.deletePartsAggregator.Where.AppendFormat( " AND ( {0} )", whereSqlParts );
            }

            return this;
        }
        #endregion

        #region Execute
        /// <summary>
        /// 执行
        /// </summary>
        public int Execute()
        {
            SqlCmd command = new SqlCmd( this.deletePartsAggregator.ToString(), this.parameters );

            SqlCmd.Current = command;

            return this.provider.ExecuteNonQuery( command.Sql, command.Parameters );
        }
        #endregion

        #region Provider
        public ISessionProvider Provider
        {
            get { return this.provider; }
        }
        #endregion
    }
}
