using System.Text;
using Restful.Data.MySql.Common;

namespace Restful.Data.MySql.CommandBuilders
{
    public class MySqlDeleteCommandBuilder : MySqlCommandBuilder
    {
        private string tableName { get; set; }

        public string WhereParts { get; set; }

        public MySqlDeleteCommandBuilder( string tableName ) : base()
        {
            this.tableName = tableName;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append( "delete from " );
            builder.AppendFormat( "{0}{1}{2} ", Constants.LeftQuote, this.tableName, Constants.RightQuote );

            if( string.IsNullOrEmpty( this.WhereParts ) == false )
            {
                builder.AppendFormat( "where {0}", this.WhereParts );
            }

            builder.Append( ";" );

            return builder.ToString();
        }
    }
}
