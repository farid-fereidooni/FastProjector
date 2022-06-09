namespace FastProjector.Models.Casting
{
    internal interface ICanSetCastTo : ICanSetCastFrom
    {
        ICanSetCastFunction To(PropertyType type);
    }
}