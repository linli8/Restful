using System;
using System.Text;

namespace Restful.Data.MySql.CommandBuilders
{
    public class MySqlSelectPartsCommandBuilder : MySqlCommandBuilder
    {
        public StringBuilder SelectPartsBuilder { get; private set; }

        public MySqlSelectPartsCommandBuilder() : base()
        {
            this.SelectPartsBuilder = new StringBuilder();
        }

        public override string ToString()
        {
            return this.SelectPartsBuilder.ToString();
        }
    }
}

