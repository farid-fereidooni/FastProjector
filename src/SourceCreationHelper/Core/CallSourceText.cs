using System.Collections.Generic;
using SourceCreationHelper.Interfaces;

namespace SourceCreationHelper.Core
{   
    internal class CallSourceText: SourceTextBase, ICallSourceText
    {
        private readonly string _methodName;
        private readonly List<string> _arguments;

        public CallSourceText(string methodName)
        {
            _methodName = methodName;
            _arguments = new List<string>();
        }
        
        protected override string BuildSource()
        {
            var argumentsJoined = string.Join(", ", _arguments);
            return $"{_methodName}({argumentsJoined})";
        }

        public ICallSourceText AddArgument(ISourceText argument)
        {
            _arguments.Add(argument.Text);
            return this;
        }
    }
}