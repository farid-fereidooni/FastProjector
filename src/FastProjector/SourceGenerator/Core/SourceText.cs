using System.Collections.Generic;
using System.Linq;

namespace FastProjector.MapGenerator.SourceGenerator.Interfaces
{   
    internal class SourceText: ISourceText
    {
        public SourceText(string text)
        {
            _Text = text;
        }
        private readonly string _Text;
        public string Text =>_Text;
    }
}