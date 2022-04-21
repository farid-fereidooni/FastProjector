using System;
using System.ComponentModel;
using FastProjector.Contracts;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeMetaDatas;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Projections
{
    internal class PrimitiveProjection: Projection
    {
        private readonly CollectionPropertyMetadata _sourcePropertyMetaData;
        private readonly CollectionPropertyMetadata _destinationPropertyMetaData;

        public PrimitiveProjection(CollectionPropertyMetadata destinationMetaData, CollectionPropertyMetadata sourcePropertyMetaData)
        {
            _destinationPropertyMetaData = destinationMetaData;
            _sourcePropertyMetaData = sourcePropertyMetaData;
            ValidateMetaData();
        }

        private void ValidateMetaData()
        {
            if (_destinationPropertyMetaData.TypeMetaData.GetCollectionType() is not PrimitiveTypeMetaData)
            {
                throw new ArgumentException("Only Primitive Collections are allowed");
            }
        }

        public override ISourceText CreateProjection(IModelMapService mapService, ISourceText parameterName)
        {
            var castResult = mapService.CastType(_sourcePropertyMetaData.TypeMetaData.TypeInformation,
                _destinationPropertyMetaData.TypeMetaData.TypeInformation);
            
            return castResult.IsUnMapable ? null :
                SourceCreator.CreateSource(castResult.Cast($"{parameterName}.{_sourcePropertyMetaData.PropertyName}"));
        }
    }
}