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
            NNInputFormatter formatter = new NNInputFormatter(2000);
            TransferFunctions tf = new TransferFunctions();
            NeuralNetwork Shrek = new NeuralNetwork(new int[] { formatter.InputNeuronSize, 5,  1}, tf.Hyperbolic, tf.HyperbolicDerivative, tf.Logistic, tf.LogistikDerivative);
            Shrek.Training(formatter.TrainingTestCases, formatter.TestingTestCases);
            Shrek.CalulateAccurracy(formatter.TestingTestCases);
            #region end
            Console.WriteLine("Goodbye Cruel World.");
            Console.ReadLine();
            #endregion
        }
    }
}
