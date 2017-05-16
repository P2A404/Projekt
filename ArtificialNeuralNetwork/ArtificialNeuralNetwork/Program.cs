using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            #region start
            Console.Clear();
            Console.WriteLine("Hello World!");
            #endregion
            NNInputFormatter formatter = new NNInputFormatter();
            TransferFunctions tf = new TransferFunctions();
            NeuralNetwork Shrek = new NeuralNetwork(new int[] { formatter.InputNeuronSize, 100, 10, 10, 1 }, tf.Logistic, tf.LogistikDerivative, tf.Logistic, tf.LogistikDerivative);
            
            Shrek.Training(formatter.testCases.GetRange(0,2000).ToArray());
            #region end
            Console.WriteLine("Goodbye Cruel World.");
            Console.ReadLine();
            #endregion
        }
    }
}
