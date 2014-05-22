using System;
using System.Linq.Expressions;
using System.Text;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Parsing;
using Restful.Data.MySql.Common;
using Restful.Data.MySql.SqlParts;
using System.Collections.Generic;

namespace Restful.Data.MySql.Visitors
{
    internal class MySqlOrderByClauseVisitor : ThrowingExpressionTreeVisitor
    {
        private readonly StringBuilder builder;
        private readonly IList<object> parameters;

        #region MySqlOrderByClauseVisitor
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterAggregator"></param>
        public MySqlOrderByClauseVisitor( IList<object> parameters )
        {
            this.builder = new StringBuilder();
            this.parameters = parameters;
        }
        #endregion

        #region Translate
        /// <summary>
        /// 翻译表达式
        /// </summary>
        /// <returns></returns>
        public string Translate( Expression expression )
        {
            this.VisitExpression( expression );

            return this.builder.ToString();
        }
        #endregion

        #region VisitQuerySourceReferenceExpression
        /// <summary>
        /// 解析查询源引用表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitQuerySourceReferenceExpression( QuerySourceReferenceExpression expression )
        {
            builder.Append( expression.ReferencedQuerySource.ItemName.ToUpper() );

            return expression;
        }
        #endregion

        #region VisitMemberExpression
        /// <summary>
        /// 解析成员访问表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitMemberExpression( MemberExpression expression )
        {
            this.VisitExpression( expression.Expression );

            builder.AppendFormat( ".{0}{1}{2}", Constants.LeftQuote, expression.Member.Name, Constants.RightQuote );

            return expression;
        }
        #endregion

        #region CreateUnhandledItemException
        protected override Exception CreateUnhandledItemException<T>( T unhandledItem, string visitMethod )
        {
            string itemText = FormatUnhandledItem( unhandledItem );
            var message = string.Format( "The expression '{0}' (type: {1}) is not supported by this LINQ provider.", itemText, typeof( T ) );
            return new NotSupportedException( message );
        }
        #endregion

        #region FormatUnhandledItem<T>
        private string FormatUnhandledItem<T>( T unhandledItem )
        {
            var itemAsExpression = unhandledItem as Expression;
            return itemAsExpression != null ? FormattingExpressionTreeVisitor.Format( itemAsExpression ) : unhandledItem.ToString();
        }
        #endregion
    }
}
