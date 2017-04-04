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
        public Neuron[] neurons;
        public Layer nextLayer;

        //Constructor
        public Layer(int size, Layer _nextLayer)
        {
            neurons = new Neuron[size];
            nextLayer = _nextLayer;
            //make better
        }
    }
}
