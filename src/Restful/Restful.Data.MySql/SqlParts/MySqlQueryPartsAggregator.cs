using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remotion.Linq.Clauses;
using Remotion.Linq.Utilities;

namespace Restful.Data.MySql.SqlParts
{
    internal class MySqlQueryPartsAggregator
    {
        #region Properties

        public bool IsDistinct { get; set; }

        public string SelectPart { get; set; }

        public IList<string> FromParts { get; private set; }

        public IList<string> WhereParts { get; private set; }

        public IList<string> OrderByParts { get; private set; }

        public MySqlLimitPartsAggregator LimitParts { get; private set; }
        #endregion

        #region QueryPartsAggregator
        /// <summary>
        /// 
        /// </summary>
        public MySqlQueryPartsAggregator()
        {
            this.FromParts = new List<string>();
            this.WhereParts = new List<string>();
            this.OrderByParts = new List<string>();
            this.LimitParts = new MySqlLimitPartsAggregator();
        }
        #endregion

        #region ToString
        /// <summary>
        /// 重写 ToString 方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append( "SELECT " );
            
            if( this.IsDistinct )
            {
                builder.Append( "DISTINCT " );
            }

            builder.Append( this.SelectPart );

            builder.AppendFormat( " FROM {0} ", string.Join( ", ", this.FromParts ) );

            if( this.WhereParts.Count > 0 )
            {
                builder.AppendFormat( "WHERE {0} ", string.Join( " AND ", this.WhereParts ) );
            }
                
            if( OrderByParts.Count > 0 )
            {
                builder.AppendFormat( "ORDER BY {0} ", string.Join( ", ", OrderByParts ) );
            }

            if( this.LimitParts.From != 0 || this.LimitParts.Count != 0 )
            {
                builder.AppendFormat( "LIMIT {0},{1}", this.LimitParts.From, this.LimitParts.Count );
            }
                
            return builder.ToString();
        }
        #endregion
    }
}
