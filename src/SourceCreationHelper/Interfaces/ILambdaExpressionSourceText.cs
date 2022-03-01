namespace SourceCreationHelper.Interfaces
{   
    public interface ILambdaExpressionSourceText: ISourceText
    {
        ILambdaExpressionSourceText AddParameter(string paramName);
        ILambdaExpressionSourceText AddParameters(params string[] paramName);

        ILambdaExpressionSourceText AssignBodyExpression(ISourceText body);
        ILambdaExpressionSourceText AssignBodyBlock(IBlockSourceText bodyBlock);
    }
}