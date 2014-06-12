using System.Collections.Generic;
using System.Text;

namespace Restful.Data.MySql.CommandBuilders
{
    internal class MySqlQueryCommandBuilder : MySqlCommandBuilder
    {
        public bool IsDistinct { get; set; }

        public string SelectPart { get; set; }

        public IList<string> FromParts { get; private set; }

        public IList<string> WhereParts { get; private set; }

        public IList<string> OrderByParts { get; private set; }

        public MySqlLimitPartsAggregator LimitParts { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public MySqlQueryCommandBuilder() : base()
        {
            this.FromParts = new List<string>();
            this.WhereParts = new List<string>();
            this.OrderByParts = new List<string>();
            this.LimitParts = new MySqlLimitPartsAggregator();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append( "select " );

            if( this.IsDistinct )
            {
                builder.Append( "distinct " );
            }

            builder.Append( this.SelectPart );

            builder.AppendFormat( " from {0} ", string.Join( ", ", this.FromParts ) );

            if( this.WhereParts.Count > 0 )
            {
                builder.AppendFormat( "where {0} ", string.Join( " and ", this.WhereParts ) );
            }

            if( OrderByParts.Count > 0 )
            {
                builder.AppendFormat( "order by {0} ", string.Join( ", ", OrderByParts ) );
            }

            if( this.LimitParts.From != 0 || this.LimitParts.Count != 0 )
            {
                builder.AppendFormat( "limit {0},{1}", this.LimitParts.From, this.LimitParts.Count );
            }

            return builder.ToString();
        }
    }
}
