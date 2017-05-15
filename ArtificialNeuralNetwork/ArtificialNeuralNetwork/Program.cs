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
            NeuralNetwork Shrek = new NeuralNetwork(new int[] { 1268, 2536, 1268, 634, 1 }, tf.Hyperbolic, tf.Logistic, tf.HyperbolicDeivative, tf.LogistikDerivative);
            Shrek.Training(formatter.testCases.GetRange(0,50).ToArray());
            #region end
            Console.WriteLine("Goodbye Cruel World.");
            Console.ReadLine();
            #endregion
        }
    }
}
