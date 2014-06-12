using System;
using System.Text;

namespace Restful.Data.SqlServer.CommandBuilders
{
    public class SqlServerSelectPartsCommandBuilder : SqlServerCommandBuilder
    {
        public StringBuilder SelectPartsBuilder { get; private set; }

        public SqlServerSelectPartsCommandBuilder() : base()
        {
            this.SelectPartsBuilder = new StringBuilder();
        }

        public override string ToString()
        {
            return this.SelectPartsBuilder.ToString();
        }
    }
}

