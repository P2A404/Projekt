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

        //Constructor
        public NeuralNetwork(int[] size)
        {
            inputSize = size[0];
            layers = new WeightLayer[size.Length-1];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new WeightLayer(size[i+1], size[i]);
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
                for (int i = 0; i < layers.Length; i++)
                {
                    data = Forward(data, layers[i].weights);
                }
                return data;
            }
        }

        public double[] Forward (double[] input, double[,] weights)
        {
            double[] ret = new double[weights.GetLength(0)];
            for (int i = 0; i < ret.Length; i++)
            {
                double curr = 0;
                for (int i2 = 0; i2 < input.Length; i++)
                {
                    curr += input[i2] * weights[i, i2];
                }
                ret[i] = curr;
            }
            return ret;
        }
    }
}
