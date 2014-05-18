using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Restful.Data.MySql.Common;

namespace Restful.Data.MySql.SqlParts
{
    public class MySqlDeletePartsAggregator
    {
        public string TableName { get; set; }

        public StringBuilder Where { get; private set; }

        public MySqlDeletePartsAggregator()
        {
            this.Where = new StringBuilder();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append( "DELETE FROM " );
            builder.AppendFormat( "{0}{1}{0} ", Constants.Quote, this.TableName );

            if( this.Where.Length > 0 )
            {
                builder.AppendFormat( "WHERE {0}", this.Where );
            }

            builder.Append( ";" );

            return builder.ToString();
        }
    }
}
