namespace FastProjector.Models.Casting
{
    internal interface ICanSetMultipleTo
    {
        ICanSetCastFunctionWithMultipleTo To(PropertyType type);
    }
}