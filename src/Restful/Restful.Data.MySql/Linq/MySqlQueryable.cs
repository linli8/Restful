using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace Restful.Data.MySql.Linq
{
    public class MySqlQueryable<T> : QueryableBase<T>
    {
        public MySqlQueryable( IQueryParser queryParser, IQueryExecutor executor )
            : base( new DefaultQueryProvider( typeof( MySqlQueryable<> ), queryParser, executor ) )
        {
        }

        public MySqlQueryable( IQueryProvider provider, Expression expression )
            : base( provider, expression )
        {
        }
    }
}
