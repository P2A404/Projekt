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
            TransferFunctions tf = new TransferFunctions();
            NeuralNetwork NN = new NeuralNetwork(new int[] { 2, 3, 3, 2 }, tf.Logistic);
            NN.PrintArray("Cycle:", NN.Cycle(new double[] { 33.99521, 47.5 }));
            #region end
            Console.WriteLine("Goodbye Cruel World.");
            Console.ReadLine();
            #endregion
        }
    }
}
