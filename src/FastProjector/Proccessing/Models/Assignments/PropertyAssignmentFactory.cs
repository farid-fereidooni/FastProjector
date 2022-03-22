using System;
using Microsoft.CodeAnalysis;

namespace FastProjector.MapGenerator.Proccessing.Models.Assignments
{
    internal class PropertyAssignmentFactory
    {
        public PropertyAssignment CreateAssignmentMetadata(PropertyMetaData sourceProperty,
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