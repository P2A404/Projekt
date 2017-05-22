using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace ArtificialNeuralNetwork
{
    //make layers a class consturct med activering og størrelse
    public class NeuralNetwork
    {
        #region Variables
        public Layer[] layers;
        public int inputSize;
        private double[] latestInput;
        private TransferFunction _activationFunction;
        private TransferFunction _outputFunction;
        private TransferFunction _derivativeActivationFunction;
        private TransferFunction _derivativeOutputFunction;
        private Random rand = new Random(DateTime.Now.Millisecond);
        #endregion

        #region Constructors
        // Neural network constructor with specific sizes
        public NeuralNetwork(int[] size, TransferFunction activationFunction, TransferFunction derivativeActivationFunction, TransferFunction outputFunction, TransferFunction derivativeOutputFunction)
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

        // Empty neural network constructor for object oriented/dynamic construction
        public NeuralNetwork(TransferFunction activationFunction, TransferFunction activationFunctionDerivative, TransferFunction outputFunction, TransferFunction outputFunctionDerivative)
        {
            _activationFunction = activationFunction;
            _outputFunction = outputFunction;
            layers = new Layer[0];
        }
        #endregion

        #region Functions

        public delegate double[] TransferFunction(double[] input);

        public void AddLayer(Layer lay)
        {
            // Work in progress
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

        // Maybie not have this?
        public void ChangeTransferFunction(TransferFunction newTransfer)
        {
            _activationFunction = newTransfer;
        }

        public void Training(TestCase[] trainingCases, TestCase[] testCases)
        {
            foreach (TestCase testCase in trainingCases)
            {
                if (testCase.inputNeurons.Length != inputSize)
                {
                    throw new Exception($"wrong input size, expected {inputSize} but was given {testCase.inputNeurons.GetLength(1)}");
                }
            }

            double totalErrorTerm = 0.0, trainingsRate = 0.01, weightDecay = 0.05, previousErrorTerm = 0.0;
            double[][] neuronErrorTerm = new double[layers.Length][];
            double[][,] updateSumError = new double[layers.Length][,];
            bool done = false;

            for (int l = 0; l < layers.GetLength(0); l++)
            {
                neuronErrorTerm[l] = new double[layers[l].weights.GetLength(0)];
                updateSumError[l] = new double[layers[l].weights.GetLength(0), layers[l].weights.GetLength(1)];
            }

            int test = 1;
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

                totalErrorTerm = 0;

                for (int k = 0; k < trainingCases.Length; k++)
                {
                    // Run the neuron network
                    Cycle(trainingCases[k].inputNeurons);

                    // Calculate the errors
                    CalculateErrorTerm(neuronErrorTerm, trainingCases[k].winningTeam);

                    // Update the sum with the errors
                    CalculateUpdateSumError(neuronErrorTerm, updateSumError);
                    totalErrorTerm += (trainingCases[k].winningTeam * Log(layers[layers.Length - 1].activations[0], 10) + (1 - trainingCases[k].winningTeam) * Log(1 - layers[layers.Length - 1].activations[0], 10)) / trainingCases.Length;
                }

                UpdateWeights(updateSumError, trainingsRate, weightDecay, inputSize);

                // Result for the current test
                Console.WriteLine($"totalErrorTerm: {totalErrorTerm}      test: {test}");
                if (test%50 == 0)
                {
                    CalculateAccuracy(testCases);
                }
                test++;

                // Change in total error term
                if (((previousErrorTerm - totalErrorTerm) < 0.000001) && ((previousErrorTerm - totalErrorTerm) > -0.000001))
                {
                    done = true;
                }
                previousErrorTerm = totalErrorTerm;
            } while (!done);
        }

        public void CalculateAccuracy(TestCase[] testCases)
        {
            List<double> listOfPredict = new List<double>();

            int Right = 0;
            int Wrong = 0;
            double Result = 0.0;

            for (int i = 0; i < testCases.Length; i++)
            {
                Cycle(testCases[i].inputNeurons);
                double Predict = layers[layers.Length - 1].activations[0];

                listOfPredict.Add(Predict);

                if ((listOfPredict[i] < 0.5 && testCases[i].winningTeam == 0) || (listOfPredict[i] >= 0.5 && testCases[i].winningTeam == 1))
                {
                    Right++;
                }
                else
                {
                    Wrong++;
                }
            }
            Result = (double)Right / (double)(Right + Wrong) * 100;

            Console.WriteLine($"Right: {Right}, Wrong: {Wrong}, Result: {Result}%");
        }

        public void CalculateErrorTerm(double[][] neuronErrorTerm, int resultMatch)
        {
            double sumError = 0.0;
            double[] derivativeActivation;

            for (int l = layers.Length - 1; l >= 0; l--)
            {
                if (l != layers.Length - 1)
                {
                    derivativeActivation = _derivativeActivationFunction(layers[l].sums);

                    for (int j = 0; j < layers[l].weights.GetLength(0); j++)
                    {
                        sumError = 0.0;

                        for (int k = 0; k < layers[l + 1].weights.GetLength(0); k++)
                        {
                            sumError += neuronErrorTerm[l + 1][k] * layers[l + 1].weights[k, j + 1];
                        }
                        neuronErrorTerm[l][j] = sumError * derivativeActivation[j];
                    }
                }
                else
                {
                    // Last layer
                    neuronErrorTerm[l][0] = resultMatch - layers[l].activations[0];
                }
            }
        }

        public void CalculateUpdateSumError(double[][] neuronErrorTerm, double[][,] updateSumError)
        {
            for (int l = 0; l < layers.Length; l++)
            {
                for (int j = 0; j < layers[l].weights.GetLength(0); j++)
                {
                    if (l != 0)
                    {
                        // Bias
                        updateSumError[l][j, 0] += neuronErrorTerm[l][0];

                        for (int i = 1; i < layers[l].weights.GetLength(1); i++)
                        {
                            updateSumError[l][j, i] += neuronErrorTerm[l][j] * layers[l - 1].activations[i - 1];
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
            for (int l = 0; l < layers.Length; l++)
            {
                for (int j = 0; j < layers[l].weights.GetLength(0); j++)
                {
                    // Bias
                    layers[l].weights[j, 0] += trainingsRate * (updateSumError[l][j, 0] / trainingsDataLength);

                    for (int i = 1; i < layers[l].weights.GetLength(1); i++)
                    {
                        layers[l].weights[j, i] += trainingsRate * (updateSumError[l][j, i] / trainingsDataLength + weightDecay * Abs(layers[l].weights[j, i]));
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
                // Cycle through network
                for (int i = 0; i < layers.Length; i++)
                {
                    // Set up bias node for input
                    // Make into function
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
                    // Find sums for each neuron
                    data = Sum(data, layers[i].weights);
                    layers[i].sums = data;
                    // Use Transferfunction / Outputfunction on each sum
                    if (i != layers.Length - 1)
                    {
                        data = _activationFunction(data);
                        layers[i].activations = data;
                    }
                    else
                    {
                        data = _outputFunction(data);
                        layers[i].activations = data;
                    }
                }

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
            if (input == null || weights == null)
            { throw new ArgumentNullException(); }
            else if (input.Length != weights.GetLength(1))
            { throw new ArgumentOutOfRangeException(); }
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

        public void Prediction (TestCase pcase, int BoX, int firstTeamHandicap, int secondTeamHandicap)
        {
            double[] result = Cycle(pcase.inputNeurons);
            result = GetChances(BoX, firstTeamHandicap, secondTeamHandicap, result[0], 1 - result[0]);
            Console.WriteLine($"Chance of {pcase.blueTeamLatestGames[0].teamName} winning: {result[0]*100}%");
            Console.WriteLine($"Chance of {pcase.redTeamLatestGames[0].teamName} winning: {result[1] * 100}%");
            Console.WriteLine($"Chance of draw: {result[2] * 100}%");
        }

        double[] GetChances(int BestOf, int HandicapHome, int HandicapOut, double ChanceHome, double ChanceOut)
        {
            double homeChance = 0, outChance = 0, drawChance = 0;
            int TempBestOf = BestOf + HandicapHome + HandicapOut;
            int PseudoRound = HandicapOut + HandicapHome;
            homeChance = GetSingleTeamChance(HandicapHome, HandicapOut, TempBestOf, ChanceHome, ChanceOut, PseudoRound);
            outChance = GetSingleTeamChance(HandicapOut, HandicapHome, TempBestOf, ChanceOut, ChanceHome, PseudoRound);
            drawChance = 1 - homeChance - outChance;
            double[] Chances = new double[3] { homeChance, outChance, drawChance };
            return Chances;
        }

        double GetSingleTeamChance(int HomeWon, int OutWon, int BestOf, double ChanceHome, double ChanceOut, int Round)
        {
            if (HomeWon == (BestOf / 2) + 1)
            { return 1; }
            else if (OutWon == (BestOf / 2) + 1 || Round == BestOf)
            { return 0; }
            else
            {
                return ChanceHome * GetSingleTeamChance(HomeWon + 1, OutWon, BestOf, ChanceHome, ChanceOut, Round+1)
                    + ChanceOut * GetSingleTeamChance(HomeWon, OutWon + 1, BestOf, ChanceHome, ChanceOut, Round+1);
            }
        }
        #endregion
    }
}
