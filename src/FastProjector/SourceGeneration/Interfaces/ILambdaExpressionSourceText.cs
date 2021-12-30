using System.Collections.Generic;
using System.Linq;

namespace FastProjector.MapGenerator.SourceGeneration.Interfaces
{   
    internal interface ILambdaExpressionSourceText: ISourceText
    {
        ILambdaExpressionSourceText AddParameter(string paramName);
        ILambdaExpressionSourceText AddParameters(params string[] paramName);

        ILambdaExpressionSourceText AssignBodyExpression(ISourceText body);
        ILambdaExpressionSourceText AssignBodyBlock(IBlockSourceText bodyBlock);
    }
}