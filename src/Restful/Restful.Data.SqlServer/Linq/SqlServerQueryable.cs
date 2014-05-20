using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace Restful.Data.SqlServer.Linq
{
    public class SqlServerQueryable<T> : QueryableBase<T>
    {
        public SqlServerQueryable( IQueryParser queryParser, IQueryExecutor executor )
            : base( new DefaultQueryProvider( typeof( SqlServerQueryable<> ), queryParser, executor ) )
        {
        }

        public SqlServerQueryable( IQueryProvider provider, Expression expression )
            : base( provider, expression )
        {
        }
    }
}
