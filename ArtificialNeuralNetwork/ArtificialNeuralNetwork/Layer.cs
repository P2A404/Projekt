using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNeuralNetwork
{
    class Layer
    {
        #region Variables
        public double[,] weights;
        public double[] activations;
        public double[] sums;
        #endregion

        #region Constructor
        public Layer(int size, int prevSize, Random rand)
        {
            activations = new double[size];
            sums = new double[size];
            weights = new double[size, prevSize+1];
            for(int i = 0; i < weights.GetLength(0); i++)
            {
                for (int i2 = 0; i2 < weights.GetLength(1); i2++)
                {
                    double random = ((rand.NextDouble() * 2) - 1) / Math.Sqrt(prevSize + 1);
                    while (random == 0.0)
                    {
                        random = ((rand.NextDouble() * 2) - 1) / Math.Sqrt(prevSize + 1);
                    }
                    weights[i, i2] = random;
                }
            }
        }
        #endregion
    }
}
