using FastProjector.Contracts;
using FastProjector.Models.PropertyMetadatas;
using FastProjector.Models.TypeMetaDatas;
using SourceCreationHelper.Interfaces;

namespace FastProjector.Models.Assignments
{
    internal abstract class PropertyAssignment
    {
        public abstract IAssignmentSourceText CreateAssignmentSource(IModelMapService mapService, ISourceText parameterName);

        public abstract bool CanMapLater();
        
        public static PropertyAssignment Create(PropertyMetadata sourceType,
            PropertyMetadata destinationType)
        {
            if (!SameCategoryType(sourceType, destinationType))
            {
                return null;
            }
            
            return sourceType switch
            {
                PrimitivePropertyMetadata primitiveSource => new PrimitivePropertyAssignment(primitiveSource,
                    destinationType as PrimitivePropertyMetadata),
                
                ClassPropertyMetadata classSource => new ClassPropertyAssignment(classSource,
                    destinationType as ClassPropertyMetadata), 

                _ => null
            };

        }

        private static bool SameCategoryType(PropertyMetadata sourceType, PropertyMetadata destinationType)
        {
            return sourceType.TypeMetaData.GetType() == destinationType.TypeMetaData.GetType();
        }

    }
    
    
}