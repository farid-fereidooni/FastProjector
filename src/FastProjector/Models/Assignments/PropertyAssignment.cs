using FastProjector.Contracts;
using FastProjector.Models.PropertyMetaDatas;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal abstract class PropertyAssignment
    {
        public abstract IAssignmentSourceText CreateAssignmentSource(IModelMapService mapService, ISourceText parameterName);

        public abstract bool CanMapLater();
        
        public static PropertyAssignment Create(PropertyMetaData sourceProperty,
            PropertyMetaData destinationProperty)
        {
            if (!SameCategoryType(sourceProperty, destinationProperty))
            {
                return null;
            }
            
            return sourceProperty switch
            {
                PrimitivePropertyMetaData primitiveSource => new PrimitivePropertyAssignment(primitiveSource,
                    destinationProperty as PrimitivePropertyMetaData),
                
                ClassPropertyMetaData classSource => new ClassPropertyAssignment(classSource,
                    destinationProperty as ClassPropertyMetaData),

                _ => null
            };

        }

        private static bool SameCategoryType(PropertyMetaData sourceProperty, PropertyMetaData destinationProperty)
        {
            return sourceProperty.GetType() == destinationProperty.GetType();
        }

    }
    
    
}