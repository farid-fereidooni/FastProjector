using System.Linq.Expressions;

namespace FastProjector.Shared.Contracts
{
    public interface IQueryableProjectionMetadata
    {
        Expression QueryableExpression { get; set; }
    }
}