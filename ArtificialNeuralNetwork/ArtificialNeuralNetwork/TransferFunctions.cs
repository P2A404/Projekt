using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace ArtificialNeuralNetwork
{
    class TransferFunctions
    {
        public double Logistic (double input)
        {
            return 1 / (1 + Exp(-input));
        }

        public double Tahn (double input)
        {
            return 2 / (1 + Exp(-2 * input)) - 1;
        }

        public double Hyperbolic (double input)
        {
            return (Exp(input) - Exp(-input)) / (Exp(input) + Exp(-input));
        }

        public double Linear (double input)
        {
            return input;
        }
    }
}
