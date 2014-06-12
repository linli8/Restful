using System;
using Restful.Data.SqlServer.Common;
using System.Collections.Generic;

namespace Restful.Data.SqlServer.CommandBuilders
{
    public class SqlServerCommandBuilder : CommandBuilder
    {
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

        public override string AddParameter( object value )
        {
            string parameterName = string.Format( "{0}P{1}", Constants.ParameterPrefix, this.parameters.Count );

            this.AddParameter( parameterName, value );

            return parameterName;
        }

        #endregion

        public SqlServerCommandBuilder() : base()
        {
        }

        public SqlServerCommandBuilder( string sql ) : this()
        {
            this.sql = sql;
        }

        public SqlServerCommandBuilder( IDictionary<string, object> parameters ) : base( parameters )
        {
        }

        public SqlServerCommandBuilder( string sql, IDictionary<string, object> parameters ) : this( parameters )
        {
            this.sql = sql;
        }
    }
}

