using SourceCreationHelper.Interfaces;

namespace SourceCreationHelper.Core
{   
    internal class FieldSourceText: SourceTextBase, IFieldSourceText
    {
        private readonly AccessModifier _accessModifier;
        private readonly string _type;
        private readonly string _name;
        private ISourceText _initializer;

        public FieldSourceText(AccessModifier accessModifier, string type, string name)
        {
            _accessModifier = accessModifier;
            _type = type;
            _name = name;
        }
        
        public IFieldSourceText AddInitializer(ISourceText source)
        {
            _initializer = source;
            return this;
        }

        protected override string BuildSource()
        {
            var declaration =  $"{_accessModifier} {_type} {_name}";
            if(_initializer != null)
                return declaration + " = " + _initializer + ";";
            else 
                return declaration + ";";
        }
    }
}