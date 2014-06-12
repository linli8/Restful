using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Restful.Data.Oracle.Linq;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq;
using Restful.Data.Oracle.CommandBuilders;
using Restful.Data.Oracle.Visitors;
using Restful.Linq;
using Oracle.DataAccess.Client;

namespace Restful.Data.Oracle
{
    public class OracleSessionProviderFactory : ISessionProviderFactory
    {
        public CommandBuilder CreateCommandBuilder()
        {
            return new Restful.Data.Oracle.CommandBuilders.OracleCommandBuilder();
        }

        public IUpdateProvider CreateUpdateProvider( ISessionProvider provider )
        {
            return new OracleUpdateProvider( provider );
        }

        public IInsertProvider CreateInsertProvider( ISessionProvider provider )
        {
            return new OracleInsertProvider( provider );
        }

        public IDeleteProvider CreateDeleteProvider( ISessionProvider provider )
        {
            return new OracleDeleteProvider( provider );
        }

        public ISession CreateSession( string connectionStr )
        {
            return new OracleSession( this, connectionStr );
        }

        public DbConnection CreateConnection( string connectionStr )
        {
            return new OracleConnection( connectionStr );
        }

        public DbDataAdapter CreateDataAdapter()
        {
            return new OracleDataAdapter();
        }

        public IQueryable<T> CreateQueryable<T>( ISessionProvider provider )
        {
            return new OracleQueryable<T>( QueryParser.CreateDefault(), new OracleQueryExecutor( provider ) );
        }
    }
}
