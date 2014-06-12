using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Linq;
using Restful.Data.MySql.CommandBuilders;
using Restful.Data.MySql.Visitors;

namespace Restful.Data.MySql.Linq
{
    public class MySqlDeleteable<T> : IDeleteable<T>
    {
        private readonly Type elementType;
        private readonly IDeleteProvider provider;

        public MySqlDeleteable( IDeleteProvider provider )
        {
            this.provider = provider;
            this.elementType = typeof( T );
        }

        public MySqlDeleteable( IDeleteProvider provider, Type elementType )
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
