using System;
using Restful.Data.Oracle.Common;
using System.Collections.Generic;

namespace Restful.Data.Oracle.CommandBuilders
{
    public class OracleCommandBuilder : CommandBuilder
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

        public OracleCommandBuilder() : base()
        {
        }

        public OracleCommandBuilder( string sql ) : this()
        {
            this.sql = sql;
        }

        public OracleCommandBuilder( IDictionary<string, object> parameters ) : base( parameters )
        {
        }

        public OracleCommandBuilder( string sql, IDictionary<string, object> parameters ) : this( parameters )
        {
            this.sql = sql;
        }
    }
}

