namespace FastProjector.Models.Casting
{
    internal interface ICanSetCastFrom
    {
        ICanSetCastTo From(PropertyType type);
    }
}