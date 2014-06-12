using System;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Data.SqlServer.CommandBuilders;
using Restful.Data.SqlServer.Visitors;
using System.Collections.Generic;
using Restful.Linq;

namespace Restful.Data.SqlServer.Linq
{
    public class SqlServerInsertable<T> : IInsertable<T>
    {
        private readonly Type elementType;
        private readonly IDictionary<MemberExpression, object> properties;
        private readonly IInsertProvider provider;

        public SqlServerInsertable( IInsertProvider provider )
        {
            this.provider = provider;
            this.elementType = typeof( T );
            this.properties = new Dictionary<MemberExpression, object>();
        }

        public SqlServerInsertable( IInsertProvider provider, Type elementType )
        {
            this.provider = provider;
            this.elementType = elementType;
            this.properties = new Dictionary<MemberExpression, object>();
        }

        #region IInsertable implementation

        public Type ElementType
        {
            get
            {
                return this.elementType;
            }
        }

        public IDictionary<MemberExpression, object> Properties
        {
            get
            {
                return this.properties;
            }
        }


        public IInsertProvider Provider
        {
            get
            {
                return this.provider;
            }
        }

        #endregion
    }
}
