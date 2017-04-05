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
        public Layer[] layers;
        public int inputSize;

        //Constructor
        public NeuralNetwork(int[] size)
        {
            inputSize = size[0];
            layers = new Layer[size.Length-1];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new Layer(size[i+1], size[i]);
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
                    data = Forward(data, layers[i]);
                }
                return data;
            }
        }

        public double[] Forward (double[] input, Layer currLayer)
        {
            double[] ret = new double[currLayer.weights.GetLength(0)];
            for (int i = 0; i < ret.Length; i++)
            {
                double curr = 0;
                for (int i2 = 0; i2 < input.Length; i++)
                {
                    curr += input[i2] * currLayer.weights[i, i2];
                }
                ret[i] = curr;
            }
            return ret;
        }
    }
}
