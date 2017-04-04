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

        //Constructor
        public NeuralNetwork(int[] size)
        {
            layers = new Layer[size.Length];
            for (int i = layers.Length; i > 0; i--)
            {
                Layer nextLayer;
                if (i == layers.Length)
                {
                    nextLayer = null;
                }
                else
                {
                    nextLayer = layers[i + 1];
                }
                layers[i] = new Layer(size[i], nextLayer);
            }
        }
    }
}
