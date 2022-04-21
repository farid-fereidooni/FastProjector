using System;
using FastProjector.Contracts;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeInformations;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal sealed class NestedProjection: Projection
    {

        private readonly Projection _innerProjection;

        public NestedProjection(Projection innerProjection)
        {
            _innerProjection = innerProjection;
        }

        public override ISourceText CreateProjection(IModelMapService mapService, ISourceText parameterName)
        {
            throw new NotImplementedException();
        }
    }
}