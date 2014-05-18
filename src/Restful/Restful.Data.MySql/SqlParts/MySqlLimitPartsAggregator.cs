using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.Data.MySql.SqlParts
{
    internal class MySqlLimitPartsAggregator
    {
        public int From { get; set; }

        public int Count { get; set; }

        public MySqlLimitPartsAggregator()
        {
        }
    }
}
