using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastProjector.MapGenerator.SourceGenerator.Interfaces
{   
    internal class BlockSourceText: IBlockSourceText
    {
        private readonly string blockExpression;
        private readonly List<ISourceText> members;

        public string Text => BuildSource();

        public BlockSourceText(string blockExpression)
        {
            this.blockExpression = blockExpression;
            members = new List<ISourceText>();
        }

        public string BuildSource()
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