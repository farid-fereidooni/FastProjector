using System;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models
{
    internal class SubTypeInformation
    {

        public SubTypeInformation(ITypeSymbol typeSymbol)
        {
            TypeSymbol = typeSymbol;
            FullName = typeSymbol.GetFullName();
        }
        public string FullName { get; set; }
        
        public ITypeSymbol TypeSymbol { get; set; }
        public override int GetHashCode()
        {
            return FullName.ToLower().GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is SubTypeInformation res)
            {
                return string.Equals(FullName, res.FullName, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }
    }
}