using System.Linq.Expressions;

namespace Restful.Extensions
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
    }
}
