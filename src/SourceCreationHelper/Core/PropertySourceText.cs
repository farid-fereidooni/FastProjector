using SourceCreationHelper.Interfaces;

namespace SourceCreationHelper.Core
{   
    internal class PropertySourceText: SourceTextBase, IPropertySourceText
    {

        
        private readonly AccessModifier accessModifier;
        private readonly string type;
        private readonly string name;
        private readonly string initializer;

        public PropertySourceText(AccessModifier accessModifier, string type, string name, string initializer = null)
        {
            
            this.accessModifier = accessModifier;
            this.type = type;
            this.name = name; 
            this.initializer = initializer;
        }
        
        protected override string BuildSource()
        {
            var declaration =  $"{accessModifier} {type} {name} {{get; set;}}";
            if(initializer != null)
                return declaration + " = " + initializer + ";";
            else 
                return declaration + ";\n";
        }
    }
}