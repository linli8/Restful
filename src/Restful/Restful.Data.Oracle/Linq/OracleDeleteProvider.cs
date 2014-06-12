using System;
using Restful.Linq;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Data.Oracle.Visitors;
using Restful.Data.Oracle.CommandBuilders;

namespace Restful.Data.Oracle.Linq
{
    public class OracleDeleteProvider : IDeleteProvider
    {
        private readonly ISessionProvider sessionProvider;

        public OracleDeleteProvider( ISessionProvider sessionProvider )
        {
            this.sessionProvider = sessionProvider;
        }

        #region IDeleteProvider implementation

        public IDeleteable CreateDelete( Type elementType )
        {
            return (IDeleteable)Activator.CreateInstance( typeof( OracleDeleteable<> ).MakeGenericType( elementType ), new object[] { this, elementType } );
        }

        public int Execute( Type elementType, Expression predicate )
        {
            OracleDeleteCommandBuilder deleteBuilder = new OracleDeleteCommandBuilder( elementType.Name );

            if( predicate != null )
            {
                OracleWherePartsCommandBuilder whereBuilder = new OracleWherePartsCommandBuilder( deleteBuilder.Parameters );

                Expression expression = PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees( predicate );

                OracleWhereClauseVisitor visitor = new OracleWhereClauseVisitor();

                visitor.Translate( expression, whereBuilder );

                deleteBuilder.WhereParts = whereBuilder.WherePartsBuilder.ToString();
            }

            this.sessionProvider.ExecutedCommandBuilder = deleteBuilder;

            return this.sessionProvider.ExecuteNonQuery( deleteBuilder );
        }

        public IDeleteable<T> CreateDelete<T>()
        {
            return new OracleDeleteable<T>( this );
        }

        public int Execute<T>( Expression predicate )
        {
            return Execute( typeof( T ), predicate );
        }

        #endregion
    }
}

