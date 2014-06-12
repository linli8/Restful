using System;
using System.Collections.Generic;
using Restful.Data.Oracle.Common;
using System.Text;

namespace Restful.Data.Oracle.CommandBuilders
{
    public class OracleWherePartsCommandBuilder : OracleCommandBuilder
    {
        public StringBuilder WherePartsBuilder { get; private set; }

        public OracleWherePartsCommandBuilder() : base()
        {
            this.WherePartsBuilder = new StringBuilder();
        }

        public OracleWherePartsCommandBuilder( IDictionary<string,object> parameters ) : base( parameters )
        {
            this.WherePartsBuilder = new StringBuilder();
        }

        public override string ToString()
        {
            return this.WherePartsBuilder.ToString();
        }
    }
}

