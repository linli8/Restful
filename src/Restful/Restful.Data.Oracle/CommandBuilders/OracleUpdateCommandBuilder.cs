using System;
using System.Collections.Generic;
using Restful.Data.Oracle.Common;
using System.Text;

namespace Restful.Data.Oracle.CommandBuilders
{
    public class OracleUpdateCommandBuilder : OracleCommandBuilder
    {
        private string tableName { get; set; }

        private readonly IList<string> columns;

        public string WhereParts { get; set; }

        public OracleUpdateCommandBuilder( string tableName ) : base()
        {
            this.tableName = tableName;
            this.columns = new List<string>();
        }

        public void AddColumn( string columnName, object value )
        {
            string parameterName = this.AddParameter( value );

            this.columns.Add( 
                string.Format( "{0}{1}{2} = {3}", Constants.LeftQuote, columnName, Constants.RightQuote, parameterName
                ) );
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat( "update {0}{1}{2} set ", Constants.LeftQuote, this.tableName, Constants.RightQuote );
            builder.AppendFormat( "{0} ", string.Join( ", ", this.columns ) );

            if( string.IsNullOrEmpty( this.WhereParts ) == false )
            {
                builder.AppendFormat( "where {0}", this.WhereParts );
            }

            return builder.ToString();
        }
    }
}

