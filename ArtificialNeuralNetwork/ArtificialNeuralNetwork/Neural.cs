using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace ArtificialNeuralNetwork
{
    //make layers a class consturct med activering og størrelse
    //
    class NeuralNetwork
    {
        //Variables
        public Layer[] layers;
        public int inputSize;
        private TranferFunction _activationFunction;
        private TranferFunction _outputFunction;
        private Random rand = new Random();

        //Constructor
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

        //Functions

        public void AddLayer(Layer lay)
        //Work in progress
        {
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
        
        public delegate double[] TranferFunction(double[] input);

        public void Training ()
        {
            //Cycle
            //Learning Function
        }

        public double[] Cycle (double[] input)
        {
            //double check if bias works!
            if (input.Length != inputSize)
            {
                throw new Exception($"Wrong input array size, expected {inputSize}, but got {input.Length}.");
            }
            else
            {
                //Set up bias node for input
                double[] data = new double[input.Length + 1];
                for (int i = 0; i < data.Length; i++)
                {
                    if (i == 0)
                    {
                        data[i] = 1;
                    }
                    else
                    {
                        data[i] = input[i-1];
                    }
                }
                PrintArray("Data:",data);
                //Cycle through network
                for (int i = 0; i < layers.Length; i++)
                {
                    //Use Transferfunction on all layers except the last
                    if (i != layers.Length - 1)
                    {
                        PrintArray($"Pre-Data Layer {i}:", data);
                        //Find sums for each neuron
                        data = Sum(data, layers[i].weights);
                        //Use Transferfunction on each sum
                        data = _activationFunction(data);
                        PrintArray($"Data Layer {i}:", data);
                    }
                    //Use Softmax on last layer
                    else
                    {
                        data = Sum(data, layers[i].weights);
                        data = _outputFunction(data);
                        PrintArray("Data output:", data);
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

    }
}
