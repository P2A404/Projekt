using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public delegate double TranferFunction(double input);

        public void Training ()
        {

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
                    //sum
                    data = Sum(data, layers[i].weights);
                    //activation
                    for(int j = 0; j < data.Length; j++)
                    {
                        data[j] = _tf(data[j]);
                    }
                    PrintArray("layer output:", data);
                }
                //Softmax
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
