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
        #region Activations
        public double[] Logistic (double[] input)
        {
            double[] ret = new double[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                ret[i] = 1 / (1 + Exp(-input[i]));
            }
            return ret;
        }

        public double[] Tahn (double[] input)
        {
            double[] ret = new double[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                ret[i] = 2 / (1 + Exp(-2 * input[i])) - 1;
            }
            return ret;
        }

        public double[] Hyperbolic (double[] input)
        {
            double[] ret = new double[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                ret[i] = (Exp(input[i]) - Exp(-input[i])) / (Exp(input[i]) + Exp(-input[i]));
            }
            return ret;
        }

        public double[] Linear (double[] input)
        {
            return input;
        }

        public double[] SoftMax(double[] input)
        {
            double sumExp = 0;

            foreach (double num in input)
            {
                sumExp += Exp(num);
            }

            for (int i = 0; i < input.Length; i++)
            {
                input[i] = Exp(input[i]) / sumExp;
            }

            return input;
        }
        #endregion

        #region Derivatives
        public double[] LogistikDerivative(double[] inputArray)
        {
            double[] result = new double[inputArray.Length];
            for (int i = 0; i < inputArray.Length; i++)
            {
                result[i] = -Exp(inputArray[i]) / Pow((1 + Exp(inputArray[i])), 2);
            }
            return result;
        }
        
        public double[] TahnDerivative(double[] inputArray)
        {
            double[] result = new double[inputArray.Length];
            for (int i = 0; i < inputArray.Length; i++)
            {
                result[i] = -(4 * Exp(-2 * inputArray[i])) / (Pow((1 + Exp(-2 * inputArray[i])), 2));
            }
            return result;
        }
        
        public double[] HyperbolicDeivative(double[] inputArray)
        {
            double[] result = new double[inputArray.Length];
            for (int i = 0; i < inputArray.Length; i++)
            {
                result[i] = 4 * Exp(inputArray[i]) / Pow((Exp(2 * inputArray[i]) + 1), 2);
            }
            return result;
        }
        
        public double[] LinearDeivative(double[] inputArray)
        {
            double[] result = new double[inputArray.Length];
            for (int i = 0; i < inputArray.Length; i++)
            {
                result[i] = 1;
            }
            return result;
        }
        #endregion
    }
}
