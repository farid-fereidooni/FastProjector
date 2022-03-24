using FastProjector.Contracts;
using FastProjector.Models.PropertyMetaDatas;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal abstract class PropertyAssignment
    {
        public abstract IAssignmentSourceText CreateAssignment(IModelMapService mapService);

        public abstract bool CanMapLater();
        
        public static PropertyAssignment CreateAssignmentMetadata(PropertyMetaData sourceProperty,
            PropertyMetaData destinationProperty, int level)
        {
            if (!SameCategoryType(sourceProperty, destinationProperty))
            {
                return null;
            }
            
            return sourceProperty switch
            {
                PrimitivePropertyMetaData primitiveSource => new PrimitivePropertyAssignment(primitiveSource,
                    destinationProperty as PrimitivePropertyMetaData, level),

                _ => null
            };

        }

        private static bool SameCategoryType(PropertyMetaData sourceProperty, PropertyMetaData destinationProperty)
        {
            return sourceProperty.GetType() == destinationProperty.GetType();
        }

    }
    
    
}