using System;

namespace FastProjector.Models.Projections
{
    internal sealed class ClassNestedProjection: MapBasedProjection
    {
        private readonly TypeInformation _sourceType;
        private readonly TypeInformation _destinationType;
        private readonly MapBasedProjection _innerProjection;

        public ClassNestedProjection(TypeInformation sourceType, TypeInformation destinationType,
            MapBasedProjection innerProjection)
            : base(sourceType, destinationType)
        {
            _sourceType = sourceType;
            _destinationType = destinationType;
            _innerProjection = innerProjection;
        }



    }
}