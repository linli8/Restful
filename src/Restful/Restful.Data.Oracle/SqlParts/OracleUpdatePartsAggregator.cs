using System.Text;
using Restful.Data.Oracle.Common;

namespace Restful.Data.Oracle.SqlParts
{
    internal class OracleUpdatePartsAggregator
    {
        public string TableName { get; set; }

        public StringBuilder Set { get; set; }

        public StringBuilder Where { get; private set; }

        public OracleUpdatePartsAggregator()
        {
            this.Where = new StringBuilder();
            this.Set = new StringBuilder();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append( "UPDATE " );
            builder.AppendFormat( "{0}{1}{2} ", Constants.LeftQuote, this.TableName, Constants.RightQuote );
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
