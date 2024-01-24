using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace MGroup.AISolve.Core
{
    public class AISolver : IEnumerable<double[]>
    {
        private int currentSolutionNumber = 0;

        public int TrainingSolutions { get; }
        public IEnumerable<double[]> ModelParameters { get; }
        public IModelResponse ModelResponse { get; }
        public IAIResponse AIResponse { get; }

        public AISolver(int trainingSolutions, IEnumerable<double[]> modelParameters, IModelResponse modelResponse, IAIResponse aiResponse)
        {
            this.TrainingSolutions = trainingSolutions;
            this.ModelParameters = modelParameters;
            this.ModelResponse = modelResponse;
            this.AIResponse = aiResponse;
        }

        public IEnumerator<double[]> GetEnumerator()
        {
            foreach (var parameterValues in ModelParameters)
            {
                currentSolutionNumber++;
                double[] responseValues;

                if (currentSolutionNumber <= TrainingSolutions)
                {
                    Debug.WriteLine($"*************** Analysis {currentSolutionNumber} (deterministic solutions remaining {TrainingSolutions - currentSolutionNumber}) ***************");
                    responseValues = ModelResponse.GetModelResponse(parameterValues);
                    AIResponse.RegisterModelResponse(parameterValues, responseValues);

                    if (currentSolutionNumber == TrainingSolutions)
                    {
                        Debug.WriteLine($"*************** Training AI model ***************");
                        AIResponse.TrainWithRegisteredModelResponses();
                    }
                }
                else
                {
                    Debug.WriteLine($"*************** Analysis {currentSolutionNumber} (AI enhanced solution: {currentSolutionNumber - TrainingSolutions}) ***************");
                    responseValues = AIResponse.GetModelResponse(parameterValues);
                }

                yield return responseValues;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
