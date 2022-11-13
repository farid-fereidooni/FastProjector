using System;
using System.Linq;
using FastProjector.Shared.Contracts;

namespace FastProjector
{
    public static class ProjectorExtensions
    {
        public static IQueryable<TDestination> Project<TDestination>(
            this IQueryable sourceQuery,
            IProjectionConfiguration configuration)
        {
            return new Projector(configuration).Project<TDestination>(sourceQuery);
        }
        
        public static IQueryable Project(
            this IQueryable sourceQuery,
            Type destinationType,
            IProjectionConfiguration configuration)
        {
            return new Projector(configuration).Project(sourceQuery, destinationType);
        }
    }
}