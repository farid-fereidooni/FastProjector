using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceCreationHelper;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Test.Helpers;

public class SourceBuilder
{
    private readonly INamespaceSourceText _namespaceSourceText;
    private readonly List<ModelBuilder> _modelBuilders;
    public SourceBuilder()
    {
        _namespaceSourceText = SourceCreator.CreateNamespace("ProjectionTest");
        _namespaceSourceText.AddUsing("System");
        _namespaceSourceText.AddUsing("System.Collections.Generic");
        _modelBuilders = new List<ModelBuilder>();
    }
    
    public string GetSource()
    {
        foreach (var model in _modelBuilders)
        {
            _namespaceSourceText.AddClass(model.GetSource());
        }
        return _namespaceSourceText.Text;
    }

    public ModelBuilder AddClass(string name)
    {
        var model = new ModelBuilder(name);
        _modelBuilders.Add(model);
        return model;
    }
    
    public CSharpCompilation GetCompilation()
    {
        
        var compilation = CSharpCompilation.Create("compilation",
            new[] { CSharpSyntaxTree.ParseText(GetSource()) },
            new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.NetModule));
        return compilation;
    }
}