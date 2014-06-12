using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Linq;
using Restful.Data.MySql.CommandBuilders;
using Restful.Data.MySql.Common;
using Restful.Data.MySql.Visitors;

namespace Restful.Data.MySql.Linq
{
    public class MySqlInsertProvider : IInsertProvider
    {
        private readonly ISessionProvider sessionProvider;

        public MySqlInsertProvider( ISessionProvider sessionProvider )
        {
            this.sessionProvider = sessionProvider;
        }

        public IInsertable CreateInsert( Type elementType )
        {
            return (IInsertable)Activator.CreateInstance( typeof( MySqlInsertable<> ).MakeGenericType( elementType ), new object[] { this, elementType } );
        }

        public int Execute( Type elementType, IDictionary<MemberExpression, object> properties )
        {
            MySqlInsertCommandBuilder insertBuilder = new MySqlInsertCommandBuilder( elementType.Name );

            foreach( var item in properties )
            {
                insertBuilder.AddColumn( item.Key.Member.Name, item.Value );
            }

            this.sessionProvider.ExecutedCommandBuilder = insertBuilder;

            return this.sessionProvider.ExecuteNonQuery( insertBuilder );
        }

        public IInsertable<T> CreateInsert<T>()
        {
            return new MySqlInsertable<T>( this );
        }

        public int Execute<T>( IDictionary<MemberExpression, object> properties )
        {
            return this.Execute( typeof( T ), properties );
        }
    }
}

