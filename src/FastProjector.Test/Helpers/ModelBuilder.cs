using SourceCreationHelper;
using SourceCreationHelper.Core;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Test.Helpers;

public class ModelBuilder
{
    private readonly IClassSourceText _classSourceText;
    public ModelBuilder(string modelName)
    {
        _classSourceText = SourceCreator.CreateClass(modelName, AccessModifier.@public);
    }

    public ModelBuilder AddProperty(AccessModifier accessModifier, string type, string name)
    {
        _classSourceText.AddProperty(SourceCreator.CreateProperty(accessModifier, type, name));
        return this;
    }
    

    public IClassSourceText GetSource()
    {
        return _classSourceText;
    }
}