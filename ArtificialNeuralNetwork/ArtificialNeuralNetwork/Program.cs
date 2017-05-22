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
            Shrek.CalculateAccuracy(formatter.TestingTestCases);
            #region prediction
            while (true)
            {
                Console.WriteLine("Insert information on the following formula:");
                Console.WriteLine("{Team 1 Name} {Team 2 Name} {Best of}");
                string str = Console.ReadLine();
                string[] strValues = str.Split(' ');
                if (strValues.Length == 3)
                {
                    int BoX;
                    if (formatter.teams.ContainsKey(strValues[0]) && formatter.teams.ContainsKey(strValues[1]) && Int32.TryParse(strValues[2], out BoX))
                    {
                        TestCase tc = formatter.predictTestCase(strValues[0], strValues[1]);
                        formatter.CalculateTestCasesInputNeurons(tc);
                        Shrek.Prediction(tc, BoX, 0, 0);
                    }
                    else
                    {
                        Console.WriteLine("Wrong team names or Best of was not a integer");
                    }
                }
                else
                {
                    Console.WriteLine("wrong amount of arguments");
                }
            }
            #endregion

            #region end
            Console.WriteLine("Goodbye Cruel World.");
            Console.ReadLine();
            #endregion
        }
    }
}
