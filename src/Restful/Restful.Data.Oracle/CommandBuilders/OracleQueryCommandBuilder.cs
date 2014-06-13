using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Data.Oracle.CommandBuilders
{
    internal class OracleQueryCommandBuilder : OracleCommandBuilder
    {
        public bool IsDistinct { get; set; }

        public bool IsCount { get; set; }

        public string SelectPart { get; set; }

        public IList<string> FromParts { get; private set; }

        public IList<string> WhereParts { get; private set; }

        public IList<string> OrderByParts { get; private set; }

        public OracleLimitPartsAggregator LimitParts { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public OracleQueryCommandBuilder() : base()
        {
            this.FromParts = new List<string>();
            this.WhereParts = new List<string>();
            this.OrderByParts = new List<string>();
            this.LimitParts = new OracleLimitPartsAggregator();
        }

        #region OnPageQuery

        /// <summary>
        /// 
        /// </summary>
        private string OnPageQuery()
        {
            if( OrderByParts.Count == 0 )
                throw new NotSupportedException( "必须指定排序字段。" );

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

            string orderBy = string.Empty;

            for( int i = 0; i < OrderByParts.Count; i++ )
            {
                if( i == 0 )
                {
                    orderBy = orderBy + OrderByParts[i].Remove( 0, 2 );
                }
                else
                {
                    orderBy = orderBy + ", " + OrderByParts[i].Remove( 0, 2 );
                }
            }

            string fields = this.IsCount ? "count(*)" : "*";

            string sql = string.Format( "SELECT {0} FROM ( SELECT T.*, ROWNUM RN FROM ( {1} ) T ORDER BY {2} ) WHERE RN BETWEEN {3} AND {4}", fields, builder.ToString(), orderBy, this.LimitParts.From + 1, this.LimitParts.From + this.LimitParts.Count );

            return sql;
        }

        #endregion

        #region OnNonPageQuery

        private string OnNonPageQuery()
        {
            var builder = new StringBuilder();

            builder.Append( "select " );

            if( this.IsDistinct )
            {
                builder.Append( "distinct " );
            }

            builder.Append( this.IsCount ? "count(*)" : this.SelectPart );

            builder.AppendFormat( " from {0} ", string.Join( ", ", this.FromParts ) );

            if( this.WhereParts.Count > 0 )
            {
                builder.AppendFormat( "where {0} ", string.Join( " and ", this.WhereParts ) );
            }

            if( OrderByParts.Count > 0 )
            {
                builder.AppendFormat( "order by {0} ", string.Join( ", ", OrderByParts ) );
            }

            return builder.ToString();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Restful.Data.SqlServer.CommandBuilders.SqlServerQueryCommandBuilder"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Restful.Data.SqlServer.CommandBuilders.SqlServerQueryCommandBuilder"/>.</returns>

        public override string ToString()
        {
            // 如果是分页查询
            if( this.LimitParts.From != 0 || this.LimitParts.Count != 0 )
            {
                return OnPageQuery();
            }

            return OnNonPageQuery();
        }

        #endregion
    }
}
