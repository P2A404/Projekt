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
                //Cycle through network
                Console.WriteLine($"Layer size is {layers.Length}");
                for (int i = 0; i < layers.Length; i++)
                {
                    //Use Transferfunction on all layers except the last
                    if (i != layers.Length - 1)
                    {
                        //Find sums for each neuron
                        data = Sum(data, layers[i].weights);
                        //Use Transferfunction on each sum
                        data = _tf(data);
                        PrintArray("layer output:", data);
                    }
                    //Use Softmax on last layer
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
