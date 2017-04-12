using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNeuralNetwork
{
    class Layer
    {
        //Variables
        public double[,] weights;
        public double[] neurons;

        //Constructor
        public Layer(int size, int prevSize, Random rand)
        {
            neurons = new double[size];
            for (int i = 0; i < neurons.Length; i++)
            {
                neurons[i] = 0;
            }
            weights = new double[size, prevSize+1];
            for(int i = 0; i < weights.GetLength(0); i++)
            {
                for (int i2 = 0; i2 < weights.GetLength(1); i2++)
                {
                    //weights[i,i2] = rand.NextDouble();
                    weights[i, i2] = 1;
                }
            }
            Console.WriteLine($"Made new layer with {weights.GetLength(0)} and {weights.GetLength(1)}");
        }
    }
}
