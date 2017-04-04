using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNeuralNetwork
{
    class Synapse
    {
        //Variables
        public double weight;
        public Neuron fromNeuron;
        public Neuron toNeuron;

        //Constructor
        public Synapse(Neuron _fromNeuron, Neuron _toNeuron)
        {
            fromNeuron = _fromNeuron;
            toNeuron = _toNeuron;
        }
    }
}
