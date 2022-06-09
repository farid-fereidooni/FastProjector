using System;
using System.Collections.Generic;

namespace FastProjector.Models.Casting
{
    internal interface ICanSetCastFunctionWithMultipleTo : ICanSetMultipleTo
    {
        IEnumerable<CastingConfiguration> With(Func<string, string> castFunction);
    }
}