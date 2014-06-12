using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Linq;
using Restful.Data.SqlServer.CommandBuilders;
using Restful.Data.SqlServer.Visitors;

namespace Restful.Data.SqlServer.Linq
{
    public class SqlServerDeleteable<T> : IDeleteable<T>
    {
        private readonly Type elementType;
        private readonly IDeleteProvider provider;

        public SqlServerDeleteable( IDeleteProvider provider )
        {
            this.provider = provider;
            this.elementType = typeof( T );
        }

        public SqlServerDeleteable( IDeleteProvider provider, Type elementType )
        {
            this.provider = provider;
            this.elementType = elementType;
        }

        #region IDeleteable implementation

        public Type ElementType
        {
            get
            {
                return this.elementType;
            }
        }

        public Expression Predicate { get; set; }

        public IDeleteProvider Provider
        {
            get
            {
                return this.provider;
            }
        }

        #endregion
    }
}
