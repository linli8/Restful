using System;
using Restful.Linq;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Data.MySql.Visitors;
using Restful.Data.MySql.CommandBuilders;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Restful.Data.MySql.Common;

namespace Restful.Data.MySql.Linq
{
    public class MySqlUpdateProvider : IUpdateProvider
    {
        private readonly ISessionProvider sessionProvider;

        public MySqlUpdateProvider( ISessionProvider sessionProvider )
        {
            this.sessionProvider = sessionProvider;
        }

        public IUpdateable CreateUpdate( Type elementType )
        {
            return (IUpdateable)Activator.CreateInstance( typeof( MySqlUpdateable<> ).MakeGenericType( elementType ), new object[] { this, elementType } );
        }

        public IUpdateable<T> CreateUpdate<T>()
        {
            return new MySqlUpdateable<T>( this );
        }

        public int Execute( Type elementType, IDictionary<MemberExpression, object> properties, Expression predicate )
        {
            MySqlUpdateCommandBuilder updateBuilder = new MySqlUpdateCommandBuilder( elementType.Name );

            foreach( var item in properties )
            {
                updateBuilder.AddColumn( item.Key.Member.Name, item.Value );
            }

            if( predicate != null )
            {
                MySqlWherePartsCommandBuilder whereBuider = new MySqlWherePartsCommandBuilder( updateBuilder.Parameters );

                Expression expression = PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees( predicate );

                MySqlWhereClauseVisitor visitor = new MySqlWhereClauseVisitor();

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

