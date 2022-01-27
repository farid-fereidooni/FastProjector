using System.Collections.Generic;
using FastProjector.MapGenerator.Analyzing;

namespace FastProjector.MapGenerator.Proccessing
{
    internal interface IProjectionRequestProcessor
    {
        string ProcessProjectionRequest(IEnumerable<ProjectionRequest> requests);
    }
}