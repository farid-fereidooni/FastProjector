namespace FastProjector.Models.Projections
{
    internal sealed class ClassProjection: MapBasedProjection
    {
        public ClassProjection(TypeInformation sourceType, TypeInformation destinationType)
            : base(sourceType, destinationType)
        {
        }
     
    }
}