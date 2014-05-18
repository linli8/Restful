using System;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Data.Common;
using Restful.Data.Entity;
using Restful.Data.Linq;
using Restful.Data.MySql.SqlParts;
using Restful.Data.MySql.Visitors;

namespace Restful.Data.MySql.Linq
{
    public class MySqlDeleteable<T> : IDeleteable<T> where T : EntityObject
    {
        #region Member
        /// <summary>
        /// Session 提供程序
        /// </summary>
        private MySqlSessionProvider provider;

        /// <summary>
        /// 参数聚合器
        /// </summary>
        private MySqlParameterAggregator parameterAggregator;

        /// <summary>
        /// DELETE 语句
        /// </summary>
        private MySqlDeletePartsAggregator deletePartsAggregator;
        #endregion

        #region MySqlDeleteable
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="provider"></param>
        public MySqlDeleteable( MySqlSessionProvider provider)
        {
            this.provider = provider;
            this.parameterAggregator = new MySqlParameterAggregator();
            this.deletePartsAggregator = new MySqlDeletePartsAggregator();
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

            MySqlWhereClauseVisitor visitor = new MySqlWhereClauseVisitor( this.parameterAggregator );

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
