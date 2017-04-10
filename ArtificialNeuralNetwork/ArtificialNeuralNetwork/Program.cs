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
            NeuralNetwork NN1 = new NeuralNetwork(new int[] { 2, 3, 3, 2 }, tf.Logistic, tf.SoftMax);
            NN1.PrintArray("Cycle:", NN1.Cycle(new double[] { 33.99521, 47.5 }));
            NeuralNetwork NN2 = new NeuralNetwork(new int[] { 2, 3, 3, 2 }, tf.Tahn, tf.SoftMax);
            NN2.PrintArray("Cycle:", NN2.Cycle(new double[] { 33.99521, 47.5 }));
            NeuralNetwork NN3 = new NeuralNetwork(new int[] { 2, 3, 3, 2 }, tf.Hyperbolic, tf.SoftMax);
            NN3.PrintArray("Cycle:", NN3.Cycle(new double[] { 33.99521, 47.5 }));
            #region end
            Console.WriteLine("Goodbye Cruel World.");
            Console.ReadLine();
            #endregion
        }
    }
}
