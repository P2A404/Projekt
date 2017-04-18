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
        private Random rand = new Random();
        #endregion

        #region Constructors
        //neuralt netværk constructor med bestemt størrelse
        public NeuralNetwork(int[] size, TranferFunction activationFunction, TranferFunction outputFunction)
        {
            _activationFunction = activationFunction;
            _outputFunction = outputFunction;
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
        
        public void Training ()
        {
            //Cycle
            //Learning Function
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
                    PrintArray($"Data Layer {i}:", data);
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
                    PrintArray("Data output:", data);
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
