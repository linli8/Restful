using System.Text;
using Restful.Data.Oracle.Common;

namespace Restful.Data.Oracle.SqlParts
{
    internal class OracleDeletePartsAggregator
    {
        public string TableName { get; set; }

        public StringBuilder Where { get; private set; }

        public OracleDeletePartsAggregator()
        {
            this.Where = new StringBuilder();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append( "DELETE FROM " );
            builder.AppendFormat( "{0}{1}{2} ", Constants.LeftQuote, this.TableName, Constants.RightQuote );

            if( this.Where.Length > 0 )
            {
                builder.AppendFormat( "WHERE {0}", this.Where );
            }

            builder.Append( ";" );

            return builder.ToString();
        }
    }
}
