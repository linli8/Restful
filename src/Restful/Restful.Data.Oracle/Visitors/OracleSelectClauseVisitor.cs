using System;
using System.Linq.Expressions;
using System.Text;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Parsing;
using Restful.Data.Oracle.Common;
using Restful.Data.Oracle.CommandBuilders;
using System.Collections.Generic;

namespace Restful.Data.Oracle.Visitors
{
    internal class OracleSelectClauseVisitor : ThrowingExpressionTreeVisitor
    {
        private bool isMember;

        private OracleSelectPartsCommandBuilder commandBuilder;

        #region OracleSelectClauseVisitor

        /// <summary>
        /// 构造函数
        /// </summary>
        public OracleSelectClauseVisitor()
        {
        }

        #endregion

        #region Translate

        /// <summary>
        /// 翻译表达式
        /// </summary>
        /// <returns></returns>
        public void Translate( Expression expression, OracleSelectPartsCommandBuilder commandBuilder )
        {
            this.commandBuilder = commandBuilder;

            this.VisitExpression( expression );
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
            string itemName = expression.ReferencedQuerySource.ItemName == "<generated>_0" ? "T" : expression.ReferencedQuerySource.ItemName.ToLower();

            if( this.isMember == false )
            {
                this.commandBuilder.SelectPartsBuilder.Append( string.Format( "{0}.*", itemName ) );
            }
            else
            {
                this.commandBuilder.SelectPartsBuilder.Append( string.Format( "{0}.", itemName ) );
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

            this.commandBuilder.SelectPartsBuilder.AppendFormat( "{0}{1}{2}", Constants.LeftQuote, expression.Member.Name, Constants.RightQuote );

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
                if( this.commandBuilder.SelectPartsBuilder.Length > 0 )
                {
                    this.commandBuilder.SelectPartsBuilder.Append( ", " );
                }

                VisitExpression( expression.Arguments[i] );

                this.commandBuilder.SelectPartsBuilder.AppendFormat( " as {0}{1}{2}", Constants.LeftQuote, expression.Members[i].Name, Constants.RightQuote );
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

                this.commandBuilder.SelectPartsBuilder.AppendFormat( " as {0}{1}{2}", Constants.LeftQuote, binding.Member.Name, Constants.RightQuote );
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
