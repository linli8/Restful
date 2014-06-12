using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Restful.Data.MySql.Common;

namespace Restful.Data.MySql.CommandBuilders
{
    public class MySqlInsertCommandBuilder : MySqlCommandBuilder
    {
        private string tableName { get; set; }

        private readonly IList<string> columns;

        public MySqlInsertCommandBuilder( string tableName ) : base()
        {
            this.tableName = tableName;
            this.columns = new List<string>();
        }

        public void AddColumn( string columnName, object value )
        {
            this.AddParameter( value );

            this.columns.Add( string.Format( "{0}{1}{2}", Constants.LeftQuote, columnName, Constants.RightQuote ) );
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat( "insert into {0}{1}{2} ", Constants.LeftQuote, this.tableName, Constants.RightQuote );
            builder.AppendFormat( "( {0} ) ", string.Join( ", ", this.columns ) );
            builder.AppendFormat( "values ( {0} );", string.Join( ", ", this.parameters.Keys ) );

            return builder.ToString();
        }
    }
}

