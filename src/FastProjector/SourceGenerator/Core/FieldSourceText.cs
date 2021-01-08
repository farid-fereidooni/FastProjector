using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FastProjector.MapGenerator.SourceGenerator.Interfaces
{   
    internal class FieldSourceText: IFieldSourceText
    {
        private readonly AccessModifier accessModifier;
        private readonly string type;
        private readonly string name;
        private readonly Expression<Func<object>> initializer;

        public FieldSourceText(AccessModifier accessModifier, string type, string name, Expression<Func<object>> initializer = null)
        {
            this.accessModifier = accessModifier;
            this.type = type;
            this.name = name;
            this.initializer = initializer;
        }

        public string Text => BuildSource();

        public string BuildSource()
        {
            var declaration =  $"{accessModifier} {type} {name}";
            if(initializer != null)
                return declaration + " = " + initializer.Body.ToString();
            else 
                return declaration + ";";
        }
    }
}