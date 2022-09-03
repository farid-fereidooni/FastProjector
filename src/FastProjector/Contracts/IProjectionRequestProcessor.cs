using System.Collections.Generic;
using FastProjector.Analyzing;

namespace FastProjector.Contracts
{
    internal interface IProjectionRequestProcessor
    {
        void ProcessProjectionRequest(IEnumerable<ProjectionRequest> requests);
    }
}