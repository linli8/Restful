using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Linq;
using Restful.Data.SqlServer.CommandBuilders;
using Restful.Data.SqlServer.Common;
using Restful.Data.SqlServer.Visitors;

namespace Restful.Data.SqlServer.Linq
{
    public class SqlServerInsertProvider : IInsertProvider
    {
        private readonly ISessionProvider sessionProvider;

        public SqlServerInsertProvider( ISessionProvider sessionProvider )
        {
            this.sessionProvider = sessionProvider;
        }

        public IInsertable CreateInsert( Type elementType )
        {
            return (IInsertable)Activator.CreateInstance( typeof( SqlServerInsertable<> ).MakeGenericType( elementType ), new object[] { this, elementType } );
        }

        public int Execute( Type elementType, IDictionary<MemberExpression, object> properties )
        {
            SqlServerInsertCommandBuilder insertBuilder = new SqlServerInsertCommandBuilder( elementType.Name );

            foreach( var item in properties )
            {
                insertBuilder.AddColumn( item.Key.Member.Name, item.Value );
            }

            this.sessionProvider.ExecutedCommandBuilder = insertBuilder;

            return this.sessionProvider.ExecuteNonQuery( insertBuilder );
        }

        public IInsertable<T> CreateInsert<T>()
        {
            return new SqlServerInsertable<T>( this );
        }

        public int Execute<T>( IDictionary<MemberExpression, object> properties )
        {
            return this.Execute( typeof( T ), properties );
        }
    }
}

