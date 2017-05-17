using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNeuralNetwork
{
    public class Layer
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
                    weights[i,i2] = (rand.NextDouble()*2)-1;
                }
            }
        }
        #endregion
    }
}
