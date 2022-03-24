namespace FastProjector.MapGenerator.Proccessing.Models.Assignments
{
    internal interface IPropertyAssignmentFactory
    {
        PropertyAssignment CreateAssignmentMetadata(PropertyMetaData sourceProperty,
            PropertyMetaData destinationProperty, int level);
    }
}