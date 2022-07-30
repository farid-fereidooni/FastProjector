using SourceCreationHelper.Core;

namespace SourceCreationHelper.Interfaces
{   
    public interface IPropertySourceText: ISourceText
    {
        IPropertySourceText AddInitializer(ISourceText source);

        IPropertySourceText ConfigGetter(AccessModifier accessModifier, IBlockSourceText source = null);

        IPropertySourceText ConfigSetter(AccessModifier accessModifier, IBlockSourceText source = null);
    }
}