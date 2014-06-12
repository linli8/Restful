using System;
using System.Text;

namespace Restful.Data.Oracle.CommandBuilders
{
    public class OracleSelectPartsCommandBuilder : OracleCommandBuilder
    {
        public StringBuilder SelectPartsBuilder { get; private set; }

        public OracleSelectPartsCommandBuilder() : base()
        {
            this.SelectPartsBuilder = new StringBuilder();
        }

        public override string ToString()
        {
            return this.SelectPartsBuilder.ToString();
        }
    }
}

