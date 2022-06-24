using System;
using FastProjector.Contracts;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeInformations;
using FastProjector.Models.TypeMetaDatas;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal class PrimitiveProjection : Projection
    {
        private readonly CollectionTypeInformation _destinationTypeInformation;
        private readonly CollectionTypeInformation _sourceTypeInformation;

        public PrimitiveProjection(CollectionTypeInformation sourceTypeInformation,
            CollectionTypeInformation destinationTypeInformation
        )
            : base(destinationTypeInformation)
        {
            _destinationTypeInformation = destinationTypeInformation;
            _sourceTypeInformation = sourceTypeInformation;
            ValidateMetaData();
        }

        private void ValidateMetaData()
        {
            if (_destinationTypeInformation.GetCollectionType() is not PrimitiveTypeInformation)
            {
                throw new ArgumentException("Only Primitive Collections are allowed");
            }

            if (_sourceTypeInformation.GetCollectionType() is not PrimitiveTypeInformation)
            {
                throw new ArgumentException("Only Primitive Collections are allowed");
            }
        }

        public override ISourceText CreateProjection(IModelMapService mapService, ISourceText parameterName)
        {
            var castResult = mapService.CastType(_sourceTypeInformation,
                _destinationTypeInformation);

            return castResult.IsUnMapable ? null : SourceCreator.CreateSource(castResult.Cast($"{parameterName}"));
        }
    }
}