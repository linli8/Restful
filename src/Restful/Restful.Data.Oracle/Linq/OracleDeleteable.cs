using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Linq;
using Restful.Data.Oracle.CommandBuilders;
using Restful.Data.Oracle.Visitors;

namespace Restful.Data.Oracle.Linq
{
    public class OracleDeleteable<T> : IDeleteable<T>
    {
        private readonly Type elementType;
        private readonly IDeleteProvider provider;

        public OracleDeleteable( IDeleteProvider provider )
        {
            this.provider = provider;
            this.elementType = typeof( T );
        }

        public OracleDeleteable( IDeleteProvider provider, Type elementType )
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
