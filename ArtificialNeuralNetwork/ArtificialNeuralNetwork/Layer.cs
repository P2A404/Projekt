using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNeuralNetwork
{
    class WeightLayer
    {
        //Variables
        public double[,] weights;

        //Constructor
        public WeightLayer(int size, int prevSize, Random rand)
        {
            weights = new double[size, prevSize];
            for(int i = 0; i < weights.GetLength(0); i++)
            {
                for (int i2 = 0; i2 < weights.GetLength(1); i2++)
                {
                    weights[i,i2] = rand.NextDouble();
                }
            }
            Console.WriteLine($"Made new layer with {weights.GetLength(0)} and {weights.GetLength(1)}");
        }
    }
}
