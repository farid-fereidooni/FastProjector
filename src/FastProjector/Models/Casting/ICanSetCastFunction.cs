using System;

namespace FastProjector.Models.Casting
{
    internal interface ICanSetCastFunction : ICanSetMultipleTo
    {
        CastingConfiguration With(Func<string, string> castFunction);
    }
}