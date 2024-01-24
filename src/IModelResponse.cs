using System;

namespace MGroup.AISolve.Core
{
    public interface IModelResponse : IResponse
    {
        double[] GetModelResponse(double[] parameterValues);
    }
}
