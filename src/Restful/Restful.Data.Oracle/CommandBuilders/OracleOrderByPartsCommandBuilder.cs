using System;
using System.Text;

namespace Restful.Data.Oracle.CommandBuilders
{
    public class OracleOrderByPartsCommandBuilder : OracleCommandBuilder
    {
        public StringBuilder OrderByParts { get; private set; }

        public OracleOrderByPartsCommandBuilder() : base()
        {
            this.OrderByParts = new StringBuilder();
        }

        public override string ToString()
        {
            return this.OrderByParts.ToString();
        }
    }
}

