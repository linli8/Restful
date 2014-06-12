using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Restful.Data.MySql.Linq;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq;
using Restful.Data.MySql.CommandBuilders;
using Restful.Data.MySql.Visitors;
using Restful.Linq;

namespace Restful.Data.MySql
{
    public class MySqlSessionProviderFactory : ISessionProviderFactory
    {
        public CommandBuilder CreateCommandBuilder()
        {
            return new Restful.Data.MySql.CommandBuilders.MySqlCommandBuilder();
        }

        public IUpdateProvider CreateUpdateProvider( ISessionProvider provider )
        {
            return new MySqlUpdateProvider( provider );
        }

        public IInsertProvider CreateInsertProvider( ISessionProvider provider )
        {
            return new MySqlInsertProvider( provider );
        }

        public IDeleteProvider CreateDeleteProvider( ISessionProvider provider )
        {
            return new MySqlDeleteProvider( provider );
        }

        public ISession CreateSession( string connectionStr )
        {
            return new MySqlSession( this, connectionStr );
        }

        public DbConnection CreateConnection( string connectionStr )
        {
            return new MySqlConnection( connectionStr );
        }

        public DbDataAdapter CreateDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        public IQueryable<T> CreateQueryable<T>( ISessionProvider provider )
        {
            return new MySqlQueryable<T>( QueryParser.CreateDefault(), new MySqlQueryExecutor( provider ) );
        }
    }
}
