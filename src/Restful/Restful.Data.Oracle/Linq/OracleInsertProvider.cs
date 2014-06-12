using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Linq;
using Restful.Data.Oracle.CommandBuilders;
using Restful.Data.Oracle.Common;
using Restful.Data.Oracle.Visitors;

namespace Restful.Data.Oracle.Linq
{
    public class OracleInsertProvider : IInsertProvider
    {
        private readonly ISessionProvider sessionProvider;

        public OracleInsertProvider( ISessionProvider sessionProvider )
        {
            this.sessionProvider = sessionProvider;
        }

        public IInsertable CreateInsert( Type elementType )
        {
            return (IInsertable)Activator.CreateInstance( typeof( OracleInsertable<> ).MakeGenericType( elementType ), new object[] { this, elementType } );
        }

        public int Execute( Type elementType, IDictionary<MemberExpression, object> properties )
        {
            OracleInsertCommandBuilder insertBuilder = new OracleInsertCommandBuilder( elementType.Name );

            foreach( var item in properties )
            {
                insertBuilder.AddColumn( item.Key.Member.Name, item.Value );
            }

            this.sessionProvider.ExecutedCommandBuilder = insertBuilder;

            return this.sessionProvider.ExecuteNonQuery( insertBuilder );
        }

        public IInsertable<T> CreateInsert<T>()
        {
            return new OracleInsertable<T>( this );
        }

        public int Execute<T>( IDictionary<MemberExpression, object> properties )
        {
            return this.Execute( typeof( T ), properties );
        }
    }
}

