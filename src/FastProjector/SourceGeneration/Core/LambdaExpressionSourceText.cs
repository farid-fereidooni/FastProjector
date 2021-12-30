using System;
using System.Collections.Generic;
using FastProjector.SourceGeneration.Core;

namespace FastProjector.MapGenerator.SourceGeneration.Interfaces
{   
    internal class LambdaExpressionSourceText: SourceTextBase, ILambdaExpressionSourceText
    {
        private ISourceText expressionBody;
   
        private readonly List<string> parameters;
        
        public LambdaExpressionSourceText()
        {
            parameters = new List<string>();
            expressionBody = new BlockSourceText();
        }
        
        protected override string BuildSource()
        {
            var parametersJoined = string.Join(", ", parameters);
            return $"({parametersJoined}) => {expressionBody.Text}";
        }

        public ILambdaExpressionSourceText AddParameter(string paramName)
        {
            parameters.Add(paramName);
            return this;
        }

        public ILambdaExpressionSourceText AddParameters(params string[] paramName)
        {
            parameters.AddRange(paramName);
            return this;
        }

        public ILambdaExpressionSourceText AssignBodyExpression(ISourceText body)
        {
            expressionBody = body;
            return this;
        }

        public ILambdaExpressionSourceText AssignBodyBlock(IBlockSourceText bodyBlock)
        {
            expressionBody = bodyBlock;
            return this;
        }
    }
}