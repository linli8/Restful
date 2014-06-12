using System;
using System.Text;

namespace Restful.Data.MySql.CommandBuilders
{
    public class MySqlOrderByPartsCommandBuilder : MySqlCommandBuilder
    {
        public StringBuilder OrderByParts { get; private set; }

        public MySqlOrderByPartsCommandBuilder() : base()
        {
            this.OrderByParts = new StringBuilder();
        }

        public override string ToString()
        {
            return this.OrderByParts.ToString();
        }
    }
}

