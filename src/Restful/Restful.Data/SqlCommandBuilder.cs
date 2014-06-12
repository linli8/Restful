using System;
using System.Collections.Generic;

namespace Restful.Data
{
    public class SqlCommandBuilder : CommandBuilder
    {
        #region implemented abstract members of CommandBuilder

        public override string AddParameter( object value )
        {
            throw new NotImplementedException();
        }

        #endregion

        private string sql;

        #region implemented abstract members of CommandBuilder

        public override string Sql
        {
            get
            {
                return this.sql;
            }
            set
            {
                this.sql = value;
            }
        }

        #endregion

        private SqlCommandBuilder() : base()
        {
        }

        private SqlCommandBuilder( string sql ) : this()
        {
            this.sql = sql;
        }

        private SqlCommandBuilder( string sql, IDictionary<string, object> parameters ) : this( sql )
        {
            foreach( var item in parameters )
            {
                this.AddParameter( item.Key, item.Value );
            }
        }
    }
}

