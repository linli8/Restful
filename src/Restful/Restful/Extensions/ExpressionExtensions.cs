using System;


namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        #region IsConstantNull

        /// <summary>
        /// 指示表达式是否为常量 null
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public static bool IsConstantNull( this Expression expression )
        {
            return ( ( expression is ConstantExpression ) && ( (ConstantExpression)expression ).Value == null );
        }

        #endregion

        #region ToMemberExpression

        /// <summary>
        /// 将表达式转换为 MemberExpression
        /// </summary>
        /// <returns>The to member expression.</returns>
        /// <param name="expression">Expression.</param>
        public static MemberExpression ToMemberExpression( this Expression expression )
        {
            switch( expression.NodeType )
            {
                case ExpressionType.MemberAccess:
                    return (MemberExpression)expression;
                case ExpressionType.Lambda:
                    return ( (LambdaExpression)expression ).Body.ToMemberExpression();
                case ExpressionType.Convert:
                    return ( (UnaryExpression)expression ).Operand.ToMemberExpression();
                default:
                    throw new NotImplementedException( expression.NodeType.ToString() );
            }
        }

        #endregion

        #region MemberExpressionEqual

        /// <summary>
        /// 判断两个 MemberExpression 是否相同
        /// </summary>
        /// <returns><c>true</c>, if expression equal was membered, <c>false</c> otherwise.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public static bool MemberExpressionEqual( this MemberExpression x, MemberExpression y )
        {
            if( ReferenceEquals( x, y ) )
                return true;

            if( x == null || y == null )
                return false;

            if( x.NodeType != y.NodeType || x.Type != y.Type )
                return false;

            return x.Member == y.Member;
        }

        #endregion
    }
}
