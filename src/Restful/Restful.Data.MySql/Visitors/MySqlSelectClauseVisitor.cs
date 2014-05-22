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
    internal class MySqlSelectClauseVisitor : ThrowingExpressionTreeVisitor
    {
        private bool isMember;
        private readonly StringBuilder builder;
        private readonly IList<object> parameters;

        #region MySqlSelectClauseVisitor
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterAggregator"></param>
        public MySqlSelectClauseVisitor( IList<object> parameters )
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
            string itemName = expression.ReferencedQuerySource.ItemName == "<generated>_0" ? "T" : expression.ReferencedQuerySource.ItemName.ToUpper();

            if( this.isMember == false )
            {
                builder.Append( string.Format( "{0}.*", itemName ) );
            }
            else
            {
                builder.Append( string.Format( "{0}.", itemName ) );
            }

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
            this.isMember = true;

            this.VisitExpression( expression.Expression );

            builder.AppendFormat( "{0}{1}{2}", Constants.LeftQuote, expression.Member.Name, Constants.RightQuote );

            this.isMember = false;

            return expression;
        }
        #endregion

        #region VisitNewExpression
        /// <summary>
        /// 解析 New 表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitNewExpression( NewExpression expression )
        {
            for( int i = 0; i < expression.Members.Count; i++ )
            {
                if( this.builder.Length > 0 )
                {
                    this.builder.Append( ", " );
                }

                VisitExpression( expression.Arguments[i] );

                this.builder.AppendFormat( " AS {0}{1}{2}", Constants.LeftQuote, expression.Members[i].Name, Constants.RightQuote );
            }

            return expression;
        }
        #endregion

        #region VisitMemberInitExpression
        /// <summary>
        /// 解析 MemberInit 表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitMemberInitExpression( MemberInitExpression expression )
        {
            foreach( var binding in expression.Bindings )
            {
                switch( binding.BindingType )
                {
                    case MemberBindingType.Assignment:
                        MemberAssignment ma = (MemberAssignment)binding;
                        this.VisitExpression( ma.Expression );
                        break;
                    default:
                        break;
                }

                this.builder.AppendFormat( " AS {0}{1}{2}", Constants.LeftQuote, binding.Member.Name, Constants.RightQuote );
            }

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
