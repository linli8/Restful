using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Restful.Data.SqlServer.Linq;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq;
using Restful.Data.SqlServer.CommandBuilders;
using Restful.Data.SqlServer.Visitors;
using Restful.Linq;
using System.Data.SqlClient;

namespace Restful.Data.SqlServer
{
    public class SqlServerSessionProviderFactory : ISessionProviderFactory
    {
        public CommandBuilder CreateCommandBuilder()
        {
            return new Restful.Data.SqlServer.CommandBuilders.SqlServerCommandBuilder();
        }

        public IUpdateProvider CreateUpdateProvider( ISessionProvider provider )
        {
            return new SqlServerUpdateProvider( provider );
        }

        public IInsertProvider CreateInsertProvider( ISessionProvider provider )
        {
            return new SqlServerInsertProvider( provider );
        }

        public IDeleteProvider CreateDeleteProvider( ISessionProvider provider )
        {
            return new SqlServerDeleteProvider( provider );
        }

        public ISession CreateSession( string connectionStr )
        {
            return new SqlServerSession( this, connectionStr );
        }

        public DbConnection CreateConnection( string connectionStr )
        {
            return new SqlConnection( connectionStr );
        }

        public DbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        public IQueryable<T> CreateQueryable<T>( ISessionProvider provider )
        {
            return new SqlServerQueryable<T>( QueryParser.CreateDefault(), new SqlServerQueryExecutor( provider ) );
        }
    }
}
