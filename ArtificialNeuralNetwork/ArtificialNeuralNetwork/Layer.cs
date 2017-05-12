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
            for (int i = 0; i < size; i++)
            {
                activations[i] = 0;
                sums[i] = 0;
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
        }
        #endregion
    }
}
