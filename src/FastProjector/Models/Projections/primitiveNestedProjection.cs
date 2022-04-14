namespace FastProjector.Models.Projections
{
    internal sealed class primitiveNestedProjection
    {
        private readonly Projection _innerProjection;

        public primitiveNestedProjection(Projection innerProjection)
        {
            _innerProjection = innerProjection;
        }
    }
}