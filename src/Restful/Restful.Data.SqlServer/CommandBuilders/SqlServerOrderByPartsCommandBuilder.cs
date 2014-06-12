using System;
using System.Text;

namespace Restful.Data.SqlServer.CommandBuilders
{
    public class SqlServerOrderByPartsCommandBuilder : SqlServerCommandBuilder
    {
        public StringBuilder OrderByParts { get; private set; }

        public SqlServerOrderByPartsCommandBuilder() : base()
        {
            this.OrderByParts = new StringBuilder();
        }

        public override string ToString()
        {
            return this.OrderByParts.ToString();
        }
    }
}

