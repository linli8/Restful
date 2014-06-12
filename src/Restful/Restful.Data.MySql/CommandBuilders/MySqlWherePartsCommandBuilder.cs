using System;
using System.Collections.Generic;
using Restful.Data.MySql.Common;
using System.Text;

namespace Restful.Data.MySql.CommandBuilders
{
    public class MySqlWherePartsCommandBuilder : MySqlCommandBuilder
    {
        public StringBuilder WherePartsBuilder { get; private set; }

        public MySqlWherePartsCommandBuilder() : base()
        {
            this.WherePartsBuilder = new StringBuilder();
        }

        public MySqlWherePartsCommandBuilder( IDictionary<string,object> parameters ) : base( parameters )
        {
            this.WherePartsBuilder = new StringBuilder();
        }

        public override string ToString()
        {
            return this.WherePartsBuilder.ToString();
        }
    }
}

