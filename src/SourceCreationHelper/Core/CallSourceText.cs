using System.Collections.Generic;
using SourceCreationHelper.Interfaces;

namespace SourceCreationHelper.Core
{   
    internal class CallSourceText: SourceTextBase, ICallSourceText
    {

        
        private readonly string methodName;
        private readonly List<string> arguments;

        public CallSourceText(string methodName)
        {
            this.methodName = methodName;
            this.arguments = new List<string>();
        }
        
        protected override string BuildSource()
        {
            var argumentsJoined = string.Join(", ", arguments);
            return $"{methodName}({argumentsJoined})";
        }

        public ICallSourceText AddArgument(ISourceText argument)
        {
            arguments.Add(argument.Text);
            return this;
        }
    }
}