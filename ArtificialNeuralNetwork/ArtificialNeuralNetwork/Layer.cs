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

        //Constructor
        public Layer(int size, int prevSize)
        {
            weights = new double[size, prevSize];
            Console.WriteLine($"Made new layer with {weights.GetLength(0)} and {weights.GetLength(1)}");
        }
    }
}
