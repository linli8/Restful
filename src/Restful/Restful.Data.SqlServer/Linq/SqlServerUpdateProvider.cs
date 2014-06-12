using System;
using Restful.Linq;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Data.SqlServer.Visitors;
using Restful.Data.SqlServer.CommandBuilders;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Restful.Data.SqlServer.Common;

namespace Restful.Data.SqlServer.Linq
{
    public class SqlServerUpdateProvider : IUpdateProvider
    {
        private readonly ISessionProvider sessionProvider;

        public SqlServerUpdateProvider( ISessionProvider sessionProvider )
        {
            this.sessionProvider = sessionProvider;
        }

        public IUpdateable CreateUpdate( Type elementType )
        {
            return (IUpdateable)Activator.CreateInstance( typeof( SqlServerUpdateable<> ).MakeGenericType( elementType ), new object[] { this, elementType } );
        }

        public IUpdateable<T> CreateUpdate<T>()
        {
            return new SqlServerUpdateable<T>( this );
        }

        public int Execute( Type elementType, IDictionary<MemberExpression, object> properties, Expression predicate )
        {
            SqlServerUpdateCommandBuilder updateBuilder = new SqlServerUpdateCommandBuilder( elementType.Name );

            foreach( var item in properties )
            {
                updateBuilder.AddColumn( item.Key.Member.Name, item.Value );
            }

            if( predicate != null )
            {
                SqlServerWherePartsCommandBuilder whereBuider = new SqlServerWherePartsCommandBuilder( updateBuilder.Parameters );

                Expression expression = PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees( predicate );

                SqlServerWhereClauseVisitor visitor = new SqlServerWhereClauseVisitor();

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

