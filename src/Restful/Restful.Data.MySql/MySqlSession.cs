using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Data.MySql
{
    public class MySqlSession : ISession
    {
        private MySqlSessionProvider provider;

        public MySqlSession( string connectionStr )
        {
            this.provider = new MySqlSessionProvider( connectionStr );
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
