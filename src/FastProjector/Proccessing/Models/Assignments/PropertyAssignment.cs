using System.Runtime.CompilerServices;
using FastProjector.MapGenerator.Proccessing.Contracts;
using SourceCreationHelper.Interfaces;

namespace FastProjector.MapGenerator.Proccessing.Models.Assignments
{
    internal abstract class PropertyAssignment
    {
        public abstract IAssignmentSourceText CreateAssignment();

        public abstract bool CanMapLater();

    }
    
    
}