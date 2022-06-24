using FastProjector.Contracts;
using FastProjector.Models.TypeInformations;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal sealed class PrimitiveNestedProjection : NestedProjection
    {
        public PrimitiveNestedProjection(IProjection innerProjection,
            CollectionTypeInformation destinationTypeInformation) : base(innerProjection, destinationTypeInformation)
        { }
    }
}