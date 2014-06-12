using System;
using Restful.Linq;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Data.SqlServer.Visitors;
using Restful.Data.SqlServer.CommandBuilders;

namespace Restful.Data.SqlServer.Linq
{
    public class SqlServerDeleteProvider : IDeleteProvider
    {
        private readonly ISessionProvider sessionProvider;

        public SqlServerDeleteProvider( ISessionProvider sessionProvider )
        {
            this.sessionProvider = sessionProvider;
        }

        #region IDeleteProvider implementation

        public IDeleteable CreateDelete( Type elementType )
        {
            return (IDeleteable)Activator.CreateInstance( typeof( SqlServerDeleteable<> ).MakeGenericType( elementType ), new object[] { this, elementType } );
        }

        public int Execute( Type elementType, Expression predicate )
        {
            SqlServerDeleteCommandBuilder deleteBuilder = new SqlServerDeleteCommandBuilder( elementType.Name );

            if( predicate != null )
            {
                SqlServerWherePartsCommandBuilder whereBuilder = new SqlServerWherePartsCommandBuilder( deleteBuilder.Parameters );

                Expression expression = PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees( predicate );

                SqlServerWhereClauseVisitor visitor = new SqlServerWhereClauseVisitor();

                visitor.Translate( expression, whereBuilder );

                deleteBuilder.WhereParts = whereBuilder.WherePartsBuilder.ToString();
            }

            this.sessionProvider.ExecutedCommandBuilder = deleteBuilder;

            return this.sessionProvider.ExecuteNonQuery( deleteBuilder );
        }

        public IDeleteable<T> CreateDelete<T>()
        {
            return new SqlServerDeleteable<T>( this );
        }

        public int Execute<T>( Expression predicate )
        {
            return Execute( typeof( T ), predicate );
        }

        #endregion
    }
}

