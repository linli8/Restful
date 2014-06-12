using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Data.Oracle
{
    public class OracleSession : ISession
    {
        private OracleSessionProvider provider;

        public OracleSession( ISessionProviderFactory factory, string connectionStr )
        {
            this.provider = new OracleSessionProvider( factory, connectionStr );
        }

        public ISessionProvider Provider
        {
            get { return this.provider; }
        }

        public void Dispose()
        {
            this.provider.Dispose();
        }
    }
}
