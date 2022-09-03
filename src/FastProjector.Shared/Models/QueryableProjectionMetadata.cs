using System.Linq.Expressions;
using FastProjector.Shared.Contracts;

namespace FastProjector.Shared.Models
{
    public class QueryableProjectionMetadata: IQueryableProjectionMetadata
    {
        public QueryableProjectionMetadata(Expression queryableExpression)
        {
            QueryableExpression = queryableExpression;
        }
        public Expression QueryableExpression { get; set; }
    }
}