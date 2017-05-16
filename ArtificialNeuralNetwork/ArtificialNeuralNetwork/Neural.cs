using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace ArtificialNeuralNetwork
{
    //make layers a class consturct med activering og størrelse
    class NeuralNetwork
    {
        #region Variables
        public Layer[] layers;
        public int inputSize;
        private double[] latestInput;
        private TranferFunction _activationFunction;
        private TranferFunction _outputFunction;
        private TranferFunction _derivativeActivationFunction;
        private TranferFunction _derivativeOutputFunction;
        private Random rand = new Random();
        #endregion

        #region Constructors
        //neuralt netværk constructor med bestemt størrelse
        public NeuralNetwork(int[] size, TranferFunction activationFunction, TranferFunction outputFunction, TranferFunction derivativeActivationFunction, TranferFunction derivativeOutputFunction)
        {
            _activationFunction = activationFunction;
            _outputFunction = outputFunction;
            _derivativeActivationFunction = derivativeActivationFunction;
            _derivativeOutputFunction = derivativeOutputFunction;
            inputSize = size[0];
            layers = new Layer[size.Length - 1];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new Layer(size[i + 1], size[i], rand);
            }
        }

        //Tomt neuralt netværk constructor til objectorienteret/dynamisk skabelse
        public NeuralNetwork(TranferFunction activationFunction, TranferFunction activationFunctionDerivative, TranferFunction outputFunction, TranferFunction outputFunctionDerivative)
        {
            _activationFunction = activationFunction;
            _outputFunction = outputFunction;
            layers = new Layer[0];
        }
        #endregion

        #region Functions

        public delegate double[] TranferFunction(double[] input);

        public void AddLayer(Layer lay)
        {
            //Work in progress
            Layer[] newLayers = new Layer[layers.Length + 1];
            for (int i = 0; i < newLayers.Length; i++)
            {
                if (i == newLayers.Length - 1)
                {
                    newLayers[i] = lay;
                }
                else
                {
                    newLayers[i] = layers[i];
                }
            }
            layers = newLayers;
        }

        //maybie not have this?
        public void ChangeTranferFunction(TranferFunction newTransfer)
        {
            _activationFunction = newTransfer;
        }

        public void Training(NNTestCase[] testCases)
        {
            // layers[i].weight.GetLength(0); row
            // layers[i].weight.GetLength(1); column
            foreach (NNTestCase testCase in testCases)
            {
                if (testCase.inputNeurons.Length != inputSize)
                {
                    throw new Exception($"wrong input size, expected {inputSize} but was given {testCase.inputNeurons.GetLength(1)}");
                }
            }

            double totalErrorTerm = 0.0, trainingsRateBegin = 0.01, trainingsRate, weightDecay = 0.5;
            double[][] neuronErrorTerm = new double[layers.Length][];
            double[][,] updateSumError = new double[layers.Length][,];

            for (int l = 0; l < layers.GetLength(0); l++)
            {
                neuronErrorTerm[l] = new double[layers[l].weights.GetLength(0)];
                updateSumError[l] = new double[layers[l].weights.GetLength(0), layers[l].weights.GetLength(1)];
            }
            trainingsRate = trainingsRateBegin;

            int test = 0;
            do
            {
                // Clear the neuronErrorTerm and sumOfOutputError
                for (int i = 0; i < layers.Length; i++)
                {
                    Array.Clear(neuronErrorTerm[i], 0, neuronErrorTerm[i].Length);
                    for (int i2 = 0; i2 < updateSumError[i].GetLength(0); i2++)
                    {
                        for (int i3 = 0; i3 < updateSumError[i].GetLength(1); i3++)
                        {
                            updateSumError[i][i2, i3] = 0;
                        }
                    }
                }

                for (int k = 0; k < testCases.Length; k++)
                {
                    // Run the neuron network
                    Cycle(testCases[k].inputNeurons);

                    // Calculate the errors
                    CalculateErrorTerm(neuronErrorTerm, testCases[k].winningTeam);
                    CalculateUpdateSumError(neuronErrorTerm, updateSumError);
                    //Console.WriteLine($"test case {k}.");
                }

                UpdateWeights(updateSumError, trainingsRate, weightDecay, inputSize);

                // Find totalErrorTerm
                totalErrorTerm = 0;

                totalErrorTerm = neuronErrorTerm[layers.Length - 1][0];

                if (totalErrorTerm < 10 && trainingsRate == trainingsRateBegin)
                {
                    trainingsRate /= 10;
                }
                else if (totalErrorTerm < 5 && trainingsRate == trainingsRateBegin / 10)
                {
                    trainingsRate /= 10;
                }
                else if (totalErrorTerm < 1 && trainingsRate == trainingsRateBegin / 100)
                {
                    trainingsRate /= 10;
                }

                // test
                Console.WriteLine($"totalErrorTerm: {totalErrorTerm}      test: {test}");
                test++;
                if (test > 100)
                {
                    test = 0;
                }
                
            } while (totalErrorTerm > 0.02 || totalErrorTerm < -0.02); // Changeable total error term
        }

        public void CalculateErrorTerm(double[][] neuronErrorTerm, int resultMatch)
        {
            double sumError = 0.0;

            for (int l = layers.Length - 1; l >= 0; l--)
            {
                if (l != layers.Length - 1)
                {
                    // j start at 1 because bias neuron don't have an error
                    for (int j = 0; j < layers[l].weights.GetLength(0); j++)
                    {
                        sumError = 0.0;
                        // Check for not second last layer
                        // int k = l != layers.Length - 2 ? 1 : 0;
                        for (int k = 0; k < layers[l+1].weights.GetLength(0); k++)
                        {
                            sumError += neuronErrorTerm[l + 1][k] * layers[l+1].weights[k, j];
                        }
                        neuronErrorTerm[l][j] += sumError * _derivativeActivationFunction(layers[l].sums)[j];
                    }
                }
                else
                {
                    // Last layer
                    neuronErrorTerm[l][0] += (resultMatch - layers[l].activations[0]) * _derivativeOutputFunction(layers[l].sums)[0];
                }
            }
        }

        public void CalculateUpdateSumError(double[][] neuronErrorTerm, double[][,] updateSumError)
        {
            for (int l = 0; l < layers.Length; l++)
            {
                // int j = ((l != layers.Length - 2) ? 1 : 0);
                for (int j = 0; j < layers[l].weights.GetLength(0); j++)
                {
                    if (l != 0)
                    {
                        for (int i = 0; i < layers[l].weights.GetLength(1); i++)
                        {
                            if (i == 0)
                            {
                                updateSumError[l][j, i] += neuronErrorTerm[l][j];
                            }
                            else
                            {
                                updateSumError[l][j, i] += neuronErrorTerm[l][j] * layers[l - 1].activations[i - 1];
                            }
                        }
                    }
                    else
                    {
                        // Input layer
                        for (int i = 0; i < latestInput.Length; i++)
                        {
                            updateSumError[l][j, i] += neuronErrorTerm[l][j] * latestInput[i];
                        }
                    }
                }
            }
        }

        public void UpdateWeights(double[][,] updateSumError, double trainingsRate, double weightDecay, int trainingsDataLength)
        {
            double failRate;
            for (int l = 0; l < layers.Length; l++)
            {
                for (int j = 0; j < layers[l].weights.GetLength(0); j++)
                {
                    for (int i = 0; i < layers[l].weights.GetLength(1); i++)
                    {
                        if (i != 0)
                        {
                            failRate = updateSumError[l][j, i] + weightDecay * layers[l].weights[j, i];
                        }
                        else
                        {
                            // Bias
                            failRate = updateSumError[l][j, i];
                        }
                        layers[l].weights[j, i] += trainingsRate * failRate / trainingsDataLength;
                    }
                }
            }
        }

        public double[] Cycle(double[] input)
        {
            if (input.Length != inputSize)
            {
                throw new Exception($"Wrong input array size, expected {inputSize}, but got {input.Length}.");
            }
            else
            {
                latestInput = input;
                double[] data = input;
                //Cycle through network
                for (int i = 0; i < layers.Length; i++)
                {
                    //Set up bias node for input
                    //make into function
                    double[] newdata = new double[data.Length + 1];
                    for (int j = 0; j < newdata.Length; j++)
                    {
                        if (j == 0)
                        {
                            newdata[j] = 1;
                        }
                        else
                        {
                            newdata[j] = data[j - 1];
                        }
                    }
                    data = newdata;
                    //Find sums for each neuron
                    data = Sum(data, layers[i].weights);
                    layers[i].sums = data;
                    //Use Transferfunction / Outputfunction on each sum
                    if (i != layers.Length - 1)
                    {
                        data = _activationFunction(data);
                        layers[i].activations = _activationFunction(data);
                    }
                    else
                    {
                        data = _outputFunction(data);
                        layers[i].activations = _outputFunction(data);
                    }
                }
                //Possibility Tree
                return data;
            }
        }

        public void PrintArray(string message, double[] input)
        {
            Console.Write(message + " ");
            for (int i = 0; i < input.Length; i++)
            {
                if (i == input.Length - 1)
                {
                    Console.Write(input[i].ToString() + ".");
                }
                else
                {
                    Console.Write(input[i].ToString() + ", ");
                }
            }
            Console.Write("\n");
        }

        public double[] Sum(double[] input, double[,] weights)
        {
            double[] returnArray = new double[weights.GetLength(0)];

            for (int outputIndex = 0; outputIndex < returnArray.Length; outputIndex++)
            {
                for (int inputIndex = 0; inputIndex < input.Length; inputIndex++)
                {
                    returnArray[outputIndex] += input[inputIndex] * weights[outputIndex, inputIndex];
                }
            }
            return returnArray;
        }
        #endregion
    }
}
