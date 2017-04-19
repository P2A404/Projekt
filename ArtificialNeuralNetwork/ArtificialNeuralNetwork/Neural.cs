using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace ArtificialNeuralNetwork
{
    class NeuralNetwork
    {
        //Variables
        public WeightLayer[] layers;
        public int inputSize;
        private TranferFunction _tf;
        private Random rand = new Random();

        //Constructor
        public NeuralNetwork(int[] size, TranferFunction TF)
        {
            _tf = TF;
            inputSize = size[0];
            layers = new WeightLayer[size.Length-1];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new WeightLayer(size[i+1], size[i], rand);
            }
        }

        //Functions

        public delegate double[] TranferFunction(double[] input);

        public void Training (double[][] trainingData) // All training data as input
        {
            //Cycle
            //Learning Function

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

                for (int k = 0; k < trainingData.Length; k++)
                {
                    Cycle(trainingData[k]);

                    CalculateErrorTerm(errorTerm, CycleInfo); // CycleInfo = neuron output, zum, matchResult ...
                    CalculateSumError(errorTerm, CycleInfo, sumOfOutputError);
                }

                UpdateWeights(sumOfOutputError, trainingsRate, weightDecay, trainingData.Length);

                // find totalErrorTerm ...

            } while (totalErrorTerm > 0.2); // Changeable Error term
        }

        private void CalculateErrorTerm(double[][] errorTerm, object cycleInfo)
        {
            double sumError = 0.0;

            for (int l = layers.Length; l >= 0; l--)
            {
                for (int i = 0; i < layers[l].weights.GetLength(1); i++)
                {
                    if (l != layers.Length)
                    {
                        sumError = 0.0;
                        for (int j = 1; j < layers[l].weights.GetLength(0); j++)    // bias update ??
                        {
                            sumError += layers[l].weights[j, i] * errorTerm[l + 1][j];
                        }

                        errorTerm[l][i] += sumError * gradientDescent_Of_Zum_i;
                    }
                    else
                    {
                        // Last layer
                        errorTerm[l][0] += -(MatchResult_i - a_i) * gradientDescent_Of_Zum_i;
                    }
                }
            }
        }

        private void CalculateSumError(double[][] errorTerm, object cycleInfo, double[][,] sumOfOutputError)
        {
            for (int l = 0; l < layers.Length; l++)
            {
                for (int j = 1; j < layers[l].weights.GetLength(0); j++)        // bias update ??
                {
                    for (int i = 0; i < layers[l].weights.GetLength(1); i++)
                    {
                        sumOfOutputError[l][j, i] += a_i * errorTerm[l + 1][j];
                    }
                }
            }
        }

        private void UpdateWeights(double[][,] sumOfOutputError, double trainingsRate, double weightDecay, int trainingsDataLength)
        {
            for (int l = 0; l < layers.Length; l++)
            {
                for (int j = 0; j < layers[j].weights.GetLength(0); j++)
                {
                    for (int i = 0; i < layers[j].weights.GetLength(1); i++)
                    {
                        if (i != 0)
                        {
                            layers[l].weights[j, i] -= trainingsRate * (1/trainingsDataLength * sumOfOutputError[l][j, i] + weightDecay * layers[l].weights[j, i]);
                        }
                        else
                        {
                            // Bias
                            layers[l].weights[j, i] -= trainingsRate * 1/ trainingsDataLength * sumOfOutputError[l][j, i];
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
                Console.WriteLine($"Layer size is {layers.Length}");
                for (int i = 0; i < layers.Length; i++)
                {
                    if (i != layers.Length - 1)
                    {
                        //sum
                        data = Sum(data, layers[i].weights);
                        //activation
                        data = _tf(data);
                        PrintArray("layer output:", data);
                    }
                    else
                    {
                        data = SoftMax(data, layers[i].weights);
                        PrintArray("layer output:", data);
                        Console.WriteLine($"Softmax: {data[0]+data[1]}");
                    }
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

        public double[] SoftMax(double[] inputArray, double[,] weightArray)
        {
            double[] result = Sum(inputArray, weightArray);

            double sumExp = 0;

            foreach (double input in result)
            {
                sumExp += Exp(input);
            }

            for(int i = 0; i < result.Length; i++)
            {
                result[i] = Exp(result[i]) / sumExp;
            }

            return result;
        }


    }
}
