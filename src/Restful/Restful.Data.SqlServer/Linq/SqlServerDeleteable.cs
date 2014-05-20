using System;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Data.Common;
using Restful.Data.Entity;
using Restful.Data.Linq;
using Restful.Data.SqlServer.SqlParts;
using Restful.Data.SqlServer.Visitors;

namespace Restful.Data.SqlServer.Linq
{
    public class SqlServerDeleteable<T> : IDeleteable<T> where T : EntityObject
    {
        #region Member
        /// <summary>
        /// Session 提供程序
        /// </summary>
        private SqlServerSessionProvider provider;

        /// <summary>
        /// 参数聚合器
        /// </summary>
        private SqlServerParameterAggregator parameterAggregator;

        /// <summary>
        /// DELETE 语句
        /// </summary>
        private SqlServerDeletePartsAggregator deletePartsAggregator;
        #endregion

        #region SqlServerDeleteable
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="provider"></param>
        public SqlServerDeleteable( SqlServerSessionProvider provider)
        {
            this.provider = provider;
            this.parameterAggregator = new SqlServerParameterAggregator();
            this.deletePartsAggregator = new SqlServerDeletePartsAggregator();
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

            SqlServerWhereClauseVisitor visitor = new SqlServerWhereClauseVisitor( this.parameterAggregator );

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
            SqlCmd command = new SqlCmd( this.deletePartsAggregator.ToString(), this.parameterAggregator.Parameters );

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
