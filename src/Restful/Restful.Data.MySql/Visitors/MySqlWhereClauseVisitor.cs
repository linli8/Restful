using System;
using System.Linq.Expressions;
using System.Text;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Parsing;
using Restful.Data.MySql.Common;
using Restful.Data.MySql.SqlParts;
using Restful.Extensions;

namespace Restful.Data.MySql.Visitors
{
    internal class MySqlWhereClauseVisitor : ThrowingExpressionTreeVisitor
    {
        private readonly StringBuilder builder;
        private readonly MySqlParameterAggregator parameterAggregator;

        #region MySqlWhereClauseVisitor
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterAggregator"></param>
        public MySqlWhereClauseVisitor( MySqlParameterAggregator parameterAggregator )
        {
            this.builder = new StringBuilder();
            this.parameterAggregator = parameterAggregator;
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

        #region VisitQuerySourceReferenceExpression
        /// <summary>
        /// 解析 QuerySourceReference 表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitQuerySourceReferenceExpression( QuerySourceReferenceExpression expression )
        {
            builder.Append( string.Format( "{0}.", expression.ReferencedQuerySource.ItemName.ToUpper() ) );

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
            this.VisitExpression( expression.Left );

            switch( expression.NodeType )
            {
                case ExpressionType.Equal:
                    if( expression.Right.IsConstantNull() == false )
                    {
                        this.builder.Append( " = " );
                    }
                    else
                    {
                        this.builder.Append( " IS NULL" );
                    }
                    break;
                case ExpressionType.GreaterThan:
                    this.builder.Append( " > " );
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    this.builder.Append( " >= " );
                    break;
                case ExpressionType.LessThan:
                    this.builder.Append( " < " );
                    break;
                case ExpressionType.LessThanOrEqual:
                    this.builder.Append( " <= " );
                    break;
                case ExpressionType.AndAlso:
                    this.builder.Append( " AND " );
                    break;
                case ExpressionType.NotEqual:
                    if( expression.Right.IsConstantNull() == false )
                    {
                        this.builder.Append( " <> " );
                    }
                    else
                    {
                        this.builder.Append( " IS NOT NULL" );
                    }
                    break;
                case ExpressionType.OrElse:
                    this.builder.Append( " OR " );
                    break;
                default:
                    throw new NotSupportedException( string.Format( "{0} statement is not supported", expression.NodeType.ToString() ) );
            }

            if( expression.Right.IsConstantNull() == false )
            {
                this.VisitExpression( expression.Right );
            }

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
            string parameterName = this.parameterAggregator.AddParameter( expression.Value );

            this.builder.Append( parameterName );

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

            builder.AppendFormat( "{0}{1}{0}", Constants.Quote, expression.Member.Name );

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
                    this.ProcessStringLike( expression, "LIKE", expression.Arguments[0].ToString().Replace( "\"", "" ) + "%" );
                    break;
                case "EndsWith":
                    this.ProcessStringLike( expression, "LIKE", "%" + expression.Arguments[0].ToString().Replace( "\"", "" ) );
                    break;
                case "Equals":
                    this.ProcessStringLike( expression, "=", expression.Arguments[0].ToString().Replace( "\"", "" ) );
                    break;
                case "Contains":
                    this.ProcessStringLike( expression, "LIKE", "%" + expression.Arguments[0].ToString().Replace( "\"", "" ) + "%" );
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
            this.builder.AppendFormat( " {0} ", @operator );
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
                this.builder.AppendFormat( " ( {0}{1}{0} IS NOT NULL && {0}{1}{0} <> '' ) ", Constants.Quote, me.Member.Name );
            }
            else
            {
                this.builder.AppendFormat( " ( {0}{1}{0} IS NULL OR {0}{1}{0} = '' ) ", Constants.Quote, me.Member.Name );
            }
        }
        #endregion
    }
}
