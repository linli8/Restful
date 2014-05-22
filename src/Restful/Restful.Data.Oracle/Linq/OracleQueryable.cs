using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace Restful.Data.Oracle.Linq
{
    public class OracleQueryable<T> : QueryableBase<T>
    {
        public OracleQueryable( IQueryParser queryParser, IQueryExecutor executor )
            : base( new DefaultQueryProvider( typeof( OracleQueryable<> ), queryParser, executor ) )
        {
        }

        public OracleQueryable( IQueryProvider provider, Expression expression )
            : base( provider, expression )
        {
        }
    }
}
