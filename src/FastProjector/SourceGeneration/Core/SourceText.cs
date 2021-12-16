using System.Collections.Generic;
using System.Linq;
using FastProjector.SourceGeneration.Core;

namespace FastProjector.MapGenerator.SourceGeneration.Interfaces
{   
    internal class SourceText: SourceTextBase
    {
        private readonly string text;
        public SourceText(string text)
        {
            this.text = text;
        }
        protected override string BuildSource()
        {
            return text;
        }
    }
}