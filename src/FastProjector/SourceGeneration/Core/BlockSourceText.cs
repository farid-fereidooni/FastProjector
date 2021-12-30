using System.Collections.Generic;
using System.Linq;
using System.Text;
using FastProjector.SourceGeneration.Core;

namespace FastProjector.MapGenerator.SourceGeneration.Interfaces
{   
    internal class BlockSourceText: SourceTextBase, IBlockSourceText
    {
        private readonly string blockExpression;
        private readonly List<ISourceText> members;

        public BlockSourceText(string blockExpression)
        {
            this.blockExpression = blockExpression;
            members = new List<ISourceText>();
        }
        
        public BlockSourceText()
        {
            members = new List<ISourceText>();
        }

        protected override string BuildSource()
        {
             var sourceStringBuilder = new StringBuilder();
            sourceStringBuilder.AppendLine(blockExpression);
            sourceStringBuilder.AppendLine("{");
            foreach(var memberItem in members)
            {
                sourceStringBuilder.Append(memberItem.Text);
            }
            sourceStringBuilder.AppendLine("}");
            return sourceStringBuilder.ToString();
        }


        public IBlockSourceText AddSource(ISourceText source)
        {
            members.Add(source);
            return this;
        }

        public IBlockSourceText AddSource(string source)
        {
            members.Add(new SourceText(source));
            return this;
        }
    }
}