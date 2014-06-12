using System;
using Restful.Linq;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Data.Oracle.Visitors;
using Restful.Data.Oracle.CommandBuilders;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Restful.Data.Oracle.Common;

namespace Restful.Data.Oracle.Linq
{
    public class OracleUpdateProvider : IUpdateProvider
    {
        private readonly ISessionProvider sessionProvider;

        public OracleUpdateProvider( ISessionProvider sessionProvider )
        {
            this.sessionProvider = sessionProvider;
        }

        public IUpdateable CreateUpdate( Type elementType )
        {
            return (IUpdateable)Activator.CreateInstance( typeof( OracleUpdateable<> ).MakeGenericType( elementType ), new object[] { this, elementType } );
        }

        public IUpdateable<T> CreateUpdate<T>()
        {
            return new OracleUpdateable<T>( this );
        }

        public int Execute( Type elementType, IDictionary<MemberExpression, object> properties, Expression predicate )
        {
            OracleUpdateCommandBuilder updateBuilder = new OracleUpdateCommandBuilder( elementType.Name );

            foreach( var item in properties )
            {
                updateBuilder.AddColumn( item.Key.Member.Name, item.Value );
            }

            if( predicate != null )
            {
                OracleWherePartsCommandBuilder whereBuider = new OracleWherePartsCommandBuilder( updateBuilder.Parameters );

                Expression expression = PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees( predicate );

                OracleWhereClauseVisitor visitor = new OracleWhereClauseVisitor();

                visitor.Translate( expression, whereBuider );

                updateBuilder.WhereParts = whereBuider.WherePartsBuilder.ToString();
            }

            this.sessionProvider.ExecutedCommandBuilder = updateBuilder;

            return this.sessionProvider.ExecuteNonQuery( updateBuilder );
        }

        public int Execute<T>( IDictionary<MemberExpression, object> properties, Expression predicate )
        {
            return this.Execute( typeof( T ), properties, predicate );
        }

    }
}

