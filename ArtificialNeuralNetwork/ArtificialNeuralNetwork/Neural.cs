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
            layers = new Layer[size.Length-1];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new Layer(size[i+1], size[i], rand);
            }
        }

        //Tomt neuralt netværk constructor til objectorienteret/dynamisk skabelse
        public NeuralNetwork(TranferFunction activationFunction, TranferFunction outputFunction)
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
                if (i == newLayers.Length-1)
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

        public void Training(double[][] inputData, double[][] outputData) // All training data as input
        {
            // layers[i].weight.GetLength(0); row
            // layers[i].weight.GetLength(1); column

            double totalErrorTerm = 0.0, trainingsRate = 0.001, weightDecay = 0.5;

            double[][] errorTerm = new double[layers.Length][];
            double[][,] sumOfOutputError = new double[layers.Length][,];

            for (int l = 0; l < layers.GetLength(0); l++)
            {
                errorTerm[l] = new double[layers[l].weights.GetLength(0)];
                sumOfOutputError[l] = new double[layers[l].weights.GetLength(0), layers[l].weights.GetLength(0)];
            }

            do
            {
                Array.Clear(errorTerm, 0, errorTerm.Length);
                Array.Clear(sumOfOutputError, 0, sumOfOutputError.Length);

                for (int k = 0; k < inputData.Length; k++)
                {
                    Cycle(inputData[k]);

                    CalculateErrorTerm(errorTerm, outputData[k]); // CycleInfo = neuron output, zum, matchResult ...
                    CalculateSumError(errorTerm, sumOfOutputError);
                }

                UpdateWeights(sumOfOutputError, trainingsRate, weightDecay, inputData.Length);

                // Find totalErrorTerm
                for (int l = 0; l < layers.GetLength(0); l++)
                {
                    for (int i = 0; i < layers[l].weights.GetLength(0); i++)
                    {
                        totalErrorTerm += errorTerm[l][i];
                    }
                }

            } while (totalErrorTerm > 0.2); // Changeable Error term
        }

        private void CalculateErrorTerm(double[][] errorTerm, double[] resultMatch)
        {
            double sumError = 0.0;

            for (int l = layers.Length - 1; l >= 0; l--)
            {
                for (int i = 0; i < layers[l].weights.GetLength(1); i++)
                {
                    if (l != layers.Length - 1)
                    {
                        sumError = 0.0;
                        for (int j = 1; j < layers[l].weights.GetLength(0); j++)    // bias update ??
                        {
                            sumError += layers[l].weights[j, i] * errorTerm[l + 1][j];
                        }
                        errorTerm[l][i] += sumError * _derivativeActivationFunction(layers[l].sums)[i];
                    }
                    else
                    {
                        // Last layer
                        for (int j = 0; j < layers[l].activations.Length; j++)
                        {
                            errorTerm[l][j] -= (resultMatch[j] - layers[l].activations[j]) * _derivativeOutputFunction(layers[l].sums)[i];
                        }
                    }
                }
            }
        }

        private void CalculateSumError(double[][] errorTerm, double[][,] sumOfOutputError)
        {
            for (int l = 0; l < layers.Length; l++)
            {
                for (int j = ((l != layers.Length - 1) ? 1 : 0); j < layers[l].weights.GetLength(0); j++)
                {
                    for (int i = 0; i < layers[l].weights.GetLength(1); i++)
                    {
                        sumOfOutputError[l][j, i] += layers[l].activations[i] * errorTerm[l + 1][j];
                    }
                }
            }
        }

        private void UpdateWeights(double[][,] sumOfOutputError, double trainingsRate, double weightDecay, int trainingsDataLength)
        {
            for (int l = 0; l < layers.Length; l++)
            {
                for (int j = 1; j < layers[j].weights.GetLength(0); j++)
                {
                    for (int i = 0; i < layers[j].weights.GetLength(1); i++)
                    {
                        if (i != 0)
                        {
                            layers[l].weights[j, i] -= trainingsRate * (1 / trainingsDataLength * sumOfOutputError[l][j, i] + weightDecay * layers[l].weights[j, i]);
                        }
                        else
                        {
                            // Bias
                            layers[l].weights[j, i] -= trainingsRate * 1 / trainingsDataLength * sumOfOutputError[l][j, i];
                        }
                    }
                }
            }
        }

        public double[] Cycle (double[] input)
        {
            if (input.Length != inputSize)
            {
                throw new Exception($"Wrong input array size, expected {inputSize}, but got {input.Length}.");
            }
            else
            {
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
                    PrintArray($"Data Layer {i} Input:", data);
                    //Find sums for each neuron
                    data = Sum(data, layers[i].weights);
                    layers[i].sums = Sum(data, layers[i].weights);
                    //Use Transferfunction / Outputfunction on each sum
                    if(i != layers.Length-1)
                    {
                        data = _activationFunction(data);
                        layers[i].activations = _activationFunction(data);
                    }
                    else
                    {
                        data = _outputFunction(data);
                        layers[i].activations = _outputFunction(data);
                    }
                    PrintArray($"Data Layer {i} Output:", data);
                }
                //Possibility Tree
                return data;
            }
        }

        public void PrintArray(string message, double[] input)
        {
            Console.Write(message + " ");
            for(int i = 0; i < input.Length; i++)
            {
                if (i == input.Length-1)
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

        public double[] Sum (double[] input, double[,] weights)
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
