using System;
using Restful.Linq;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Data.MySql.Visitors;
using Restful.Data.MySql.CommandBuilders;

namespace Restful.Data.MySql.Linq
{
    public class MySqlDeleteProvider : IDeleteProvider
    {
        private readonly ISessionProvider sessionProvider;

        public MySqlDeleteProvider( ISessionProvider sessionProvider )
        {
            this.sessionProvider = sessionProvider;
        }

        #region IDeleteProvider implementation

        public IDeleteable CreateDelete( Type elementType )
        {
            return (IDeleteable)Activator.CreateInstance( typeof( MySqlDeleteable<> ).MakeGenericType( elementType ), new object[] { this, elementType } );
        }

        public int Execute( Type elementType, Expression predicate )
        {
            MySqlDeleteCommandBuilder deleteBuilder = new MySqlDeleteCommandBuilder( elementType.Name );

            if( predicate != null )
            {
                MySqlWherePartsCommandBuilder whereBuilder = new MySqlWherePartsCommandBuilder( deleteBuilder.Parameters );

                Expression expression = PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees( predicate );

                MySqlWhereClauseVisitor visitor = new MySqlWhereClauseVisitor();

                visitor.Translate( expression, whereBuilder );

                deleteBuilder.WhereParts = whereBuilder.WherePartsBuilder.ToString();
            }

            this.sessionProvider.ExecutedCommandBuilder = deleteBuilder;

            return this.sessionProvider.ExecuteNonQuery( deleteBuilder );
        }

        public IDeleteable<T> CreateDelete<T>()
        {
            return new MySqlDeleteable<T>( this );
        }

        public int Execute<T>( Expression predicate )
        {
            return Execute( typeof( T ), predicate );
        }

        #endregion
    }
}

