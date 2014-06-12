using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using Restful;
using Restful.Data.SqlServer.CommandBuilders;
using Restful.Data.SqlServer.Common;

namespace Restful.Data.SqlServer.Visitors
{
    internal class SqlServerWhereClauseVisitor : ThrowingExpressionTreeVisitor
    {
        protected SqlServerWherePartsCommandBuilder commandBuilder;

        #region SqlServerWhereClauseVisitor

        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlServerWhereClauseVisitor()
        {
        }

        #endregion

        #region Translate

        /// <summary>
        /// 翻译表达式
        /// </summary>
        /// <returns></returns>
        public void Translate( Expression expression, SqlServerWherePartsCommandBuilder commandBuilder )
        {
            this.commandBuilder = commandBuilder;

            this.VisitExpression( expression );
        }

        #endregion

        #region CreateUnhandledItemException

        protected override Exception CreateUnhandledItemException<T>( T unhandledItem, string visitMethod )
        {
            string itemText = FormatUnhandledItem( unhandledItem );
            var message = string.Format( "The expression '{0}' (type: {1}) is not supported by this Linq provider.", itemText, typeof( T ) );
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

        #region VisitQuerySourceReferenceExpression

        /// <summary>
        /// 解析 QuerySourceReference 表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitQuerySourceReferenceExpression( QuerySourceReferenceExpression expression )
        {
            this.commandBuilder.WherePartsBuilder.Append( string.Format( "{0}.", expression.ReferencedQuerySource.ItemName.ToLower() ) );

            return expression;
        }

        #endregion

        #region VisitLambdaExpression

        /// <summary>
        /// 解析 Lambda 表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitLambdaExpression( LambdaExpression expression )
        {
            this.VisitExpression( expression.Body );

            return expression;
        }

        #endregion

        #region VisitBinaryExpression

        /// <summary>
        /// 解析二元表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitBinaryExpression( BinaryExpression expression )
        {
            this.commandBuilder.WherePartsBuilder.Append( "( " );

            this.VisitExpression( expression.Left );

            switch( expression.NodeType )
            {
                case ExpressionType.Equal:
                    if( expression.Right.IsConstantNull() == false )
                    {
                        this.commandBuilder.WherePartsBuilder.Append( " = " );
                    }
                    else
                    {
                        this.commandBuilder.WherePartsBuilder.Append( " is null" );
                    }
                    break;
                case ExpressionType.GreaterThan:
                    this.commandBuilder.WherePartsBuilder.Append( " > " );
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    this.commandBuilder.WherePartsBuilder.Append( " >= " );
                    break;
                case ExpressionType.LessThan:
                    this.commandBuilder.WherePartsBuilder.Append( " < " );
                    break;
                case ExpressionType.LessThanOrEqual:
                    this.commandBuilder.WherePartsBuilder.Append( " <= " );
                    break;
                case ExpressionType.AndAlso:
                    this.commandBuilder.WherePartsBuilder.Append( " and " );
                    break;
                case ExpressionType.NotEqual:
                    if( expression.Right.IsConstantNull() == false )
                    {
                        this.commandBuilder.WherePartsBuilder.Append( " <> " );
                    }
                    else
                    {
                        this.commandBuilder.WherePartsBuilder.Append( " is not null" );
                    }
                    break;
                case ExpressionType.OrElse:
                    this.commandBuilder.WherePartsBuilder.Append( " or " );
                    break;
                default:
                    throw new NotSupportedException( string.Format( "{0} statement is not supported", expression.NodeType.ToString() ) );
            }

            if( expression.Right.IsConstantNull() == false )
            {
                this.VisitExpression( expression.Right );
            }

            this.commandBuilder.WherePartsBuilder.Append( " )" );

            return expression;
        }

        #endregion

        #region VisitConstantExpression

        /// <summary>
        /// 解析常量表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitConstantExpression( ConstantExpression expression )
        {
            string parameterName = this.commandBuilder.AddParameter( expression.Value );

            this.commandBuilder.WherePartsBuilder.Append( parameterName );

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

            this.commandBuilder.WherePartsBuilder.AppendFormat( "{0}{1}{2}", Constants.LeftQuote, expression.Member.Name, Constants.RightQuote );

            return expression;
        }

        #endregion

        #region VisitMethodCallExpression

        /// <summary>
        /// 解析方法调用表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCallExpression( MethodCallExpression expression )
        {
            string name = expression.Method.Name;

            switch( expression.Method.Name )
            {
                case "StartsWith":
                    this.ProcessStringLike( expression, "like", expression.Arguments[0].ToString().Replace( "\"", "" ) + "%" );
                    break;
                case "EndsWith":
                    this.ProcessStringLike( expression, "like", "%" + expression.Arguments[0].ToString().Replace( "\"", "" ) );
                    break;
                case "Equals":
                    this.ProcessStringLike( expression, "=", expression.Arguments[0].ToString().Replace( "\"", "" ) );
                    break;
                case "Contains":
                    this.ProcessStringLike( expression, "like", "%" + expression.Arguments[0].ToString().Replace( "\"", "" ) + "%" );
                    break;
                case "IsNullOrEmpty":
                    this.ProcessStringIsNullOrEmpty( expression, false );
                    break;
                default:
                    throw new NotImplementedException( string.Format( "没有实现 {0} 方法的解析。", expression.Method.Name ) );
            }

            return expression;
        }

        #endregion

        #region VisitUnaryExpression

        /// <summary>
        /// 解析一元表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitUnaryExpression( UnaryExpression expression )
        {
            switch( expression.NodeType )
            {
                case ExpressionType.Not:
                    if( expression.Operand is MemberExpression )
                    {
                        BinaryExpression be = Expression.MakeBinary( ExpressionType.Equal, expression.Operand, Expression.Constant( false ) );
                        this.VisitExpression( be );
                    }
                    else if( expression.Operand is MethodCallExpression )
                    {
                        MethodCallExpression mce = (MethodCallExpression)expression.Operand;

                        switch( mce.Method.Name )
                        {
                            case "IsNullOrEmpty":
                                this.ProcessStringIsNullOrEmpty( mce, true );
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            return expression;
        }

        #endregion

        #region VisitParameterExpression

        /// <summary>
        /// 解析 Parameter 表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitParameterExpression( ParameterExpression expression )
        {
            return expression;
        }

        #endregion

        #region ProcessStringLike

        /// <summary>
        /// 处理针对字符串模糊查询的方法调用
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="operator"></param>
        /// <param name="argument"></param>
        private void ProcessStringLike( MethodCallExpression expression, string @operator, string argument )
        {
            this.VisitExpression( expression.Object );
            this.commandBuilder.WherePartsBuilder.AppendFormat( " {0} ", @operator );
            ConstantExpression ce = Expression.Constant( argument );
            this.VisitExpression( ce );
        }

        #endregion

        #region ProcessStringIsNullOrEmpty

        /// <summary>
        /// 解析 IsNullOrEmpty 方法调用表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="notEquals"></param>
        private void ProcessStringIsNullOrEmpty( MethodCallExpression expression, bool notEquals )
        {
            MemberExpression me = (MemberExpression)expression.Arguments[0];

            if( notEquals )
            {
                this.commandBuilder.WherePartsBuilder.AppendFormat( " ( {0}{1}{2} is not null && {0}{1}{2} <> '' ) ", Constants.LeftQuote, me.Member.Name, Constants.RightQuote );
            }
            else
            {
                this.commandBuilder.WherePartsBuilder.AppendFormat( " ( {0}{1}{2} is null or {0}{1}{2} = '' ) ", Constants.LeftQuote, me.Member.Name, Constants.RightQuote );
            }
        }

        #endregion

    }
}
