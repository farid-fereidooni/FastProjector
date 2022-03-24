using System.Collections.Generic;
using FastProjector.Analyzing;

namespace FastProjector.Contracts
{
    internal interface IProjectionRequestProcessor
    {
        string ProcessProjectionRequest(IEnumerable<ProjectionRequest> requests);
    }
}