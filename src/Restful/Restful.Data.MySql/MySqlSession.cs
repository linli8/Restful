using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Data.MySql
{
    public class MySqlSession : ISession
    {
        private MySqlSessionProvider provider;

        public MySqlSession( ISessionProviderFactory factory, string connectionStr )
        {
            this.provider = new MySqlSessionProvider( factory, connectionStr );
        }

        public int CommandTimeout
        {
            get
            {
                return this.provider.CommandTimeout;
            }
            set
            {
                this.provider.CommandTimeout = value;
            }
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
