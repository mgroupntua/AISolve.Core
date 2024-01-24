using System;

namespace MGroup.AISolve.Core
{
    public interface IAIResponse : IResponse
    {
        void RegisterModelResponse(double[] parameterValues, double[] response);
        void TrainWithRegisteredModelResponses();
        double[] GetModelResponse(double[] parameterValues);
    }
}
