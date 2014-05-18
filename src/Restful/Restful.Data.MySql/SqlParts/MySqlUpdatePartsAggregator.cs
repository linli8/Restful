using System.Text;
using Restful.Data.MySql.Common;

namespace Restful.Data.MySql.SqlParts
{
    internal class MySqlUpdatePartsAggregator
    {
        public string TableName { get; set; }

        public StringBuilder Set { get; set; }

        public StringBuilder Where { get; private set; }

        public MySqlUpdatePartsAggregator()
        {
            this.Where = new StringBuilder();
            this.Set = new StringBuilder();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append( "UPDATE " );
            builder.AppendFormat( "{0}{1}{0} ", Constants.Quote, this.TableName );
            builder.Append( "SET " );
            builder.Append( this.Set );

            if( this.Where.Length > 0 )
            {
                builder.AppendFormat( " WHERE {0}", this.Where );
            }

            builder.Append( ";" );

            return builder.ToString();
        }
    }
}
