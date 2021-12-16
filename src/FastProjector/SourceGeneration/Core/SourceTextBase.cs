
using FastProjector.MapGenerator.SourceGeneration.Interfaces;

namespace FastProjector.SourceGeneration.Core
{
    internal abstract class SourceTextBase: ISourceText
    {
        public string Text => BuildSource();
        protected abstract string BuildSource();

        public override string ToString()
        {
            return BuildSource();
        } 
        
    }
}