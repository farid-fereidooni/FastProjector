using SourceCreationHelper.Interfaces;

namespace SourceCreationHelper.Core
{   
    internal class PropertySourceText: SourceTextBase, IPropertySourceText
    {
        private readonly AccessModifier _accessModifier;
        private AccessModifier _getterAccess = AccessModifier.@public;
        private AccessModifier _setterAccess = AccessModifier.@public;
        private readonly string _type;
        private readonly string _name;
        private ISourceText _initializer;
        private readonly bool _getOnly;
        private IBlockSourceText _getterSource;
        private IBlockSourceText _setterSource;

        public PropertySourceText(AccessModifier accessModifier, string type, string name, bool getOnly = false)
        {
            
            _accessModifier = accessModifier;
            _type = type;
            _name = name; 
            _getOnly = getOnly;
        }

        public IPropertySourceText AddInitializer(ISourceText source)
        {
            _initializer = source;
            return this;
        }

        public IPropertySourceText ConfigGetter(AccessModifier accessModifier, IBlockSourceText source = null)
        {
            _getterAccess = accessModifier;
            _getterSource = source;
            return this;
        }
        
        public IPropertySourceText ConfigSetter(AccessModifier accessModifier, IBlockSourceText source = null)
        {
            _setterAccess = accessModifier;
            _setterSource = source;
            return this;
        }
        
        protected override string BuildSource()
        {
            var declaration =  $"{_accessModifier} {_type} {_name} {{{GetGetterSource()} {GetSetterSource()}}}";
            if(_initializer != null)
                return declaration + " = " + _initializer + ";";
             
            return declaration + "\n";
        }

        private string GetGetterSource()
        {
            var access = _getterAccess != AccessModifier.@public ? _getterAccess.ToString() : " ";

            var source = _getterSource != null ? ";" : " " + _getterSource;

            return $"{access} get{source}";
        }
        
        private string GetSetterSource()
        {
            if (_getOnly)
                return "";
            var access = _setterAccess != AccessModifier.@public ? _setterAccess.ToString() : " ";

            var source = _setterSource != null ? ";" : " " + _setterSource;

            return $"{access} set{source}";
        }
    }
}