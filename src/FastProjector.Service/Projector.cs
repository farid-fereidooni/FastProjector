using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FastProjector.Shared.Contracts;

namespace FastProjector
{
    public class Projector : IProjector
    {
        private readonly IProjectionConfiguration _configuration;

        public Projector(IProjectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IQueryable<TDestination> Project<TDestination>(IQueryable sourceQuery)
        {
            return Project(sourceQuery, typeof(TDestination)) as IQueryable<TDestination>;
        }
        
        public IQueryable Project(IQueryable sourceQuery, Type destinationType)
        {
            var queryableMetadata = _configuration
                .From(sourceQuery.ElementType)?
                .To(destinationType)?.QueryableProjectionMetadata;

            var sourceFullname = sourceQuery.ElementType.FullName;
            var destinationFullName = destinationType.FullName;

            if (queryableMetadata is null)
            {
                throw new InvalidOperationException(
                    $"No configuration found for projecting {sourceFullname} to {destinationFullName}");
            }
            
            var selectExpression = Expression.Call(typeof(Queryable),
                "Select",
                new[] { sourceQuery.ElementType, destinationType },
                sourceQuery.Expression,
                Expression.Quote(queryableMetadata.QueryableExpression));

            return sourceQuery.Provider.CreateQuery(selectExpression);
        }
    }
}