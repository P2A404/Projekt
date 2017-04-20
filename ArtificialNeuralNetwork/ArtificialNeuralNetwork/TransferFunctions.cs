﻿using System;
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
        //make them work without weightArray
        public double[] LogistikDerivative(double[] inputArray, double[,] weightArray)
        {
            double[] result = new double[weightArray.GetLength(0)];

            int k = 0;

            for (int i = 0; i < weightArray.GetLength(0); i++)
            {
                foreach (double input in inputArray)
                {
                    result[k] += input * weightArray[i, k];
                    k++;
                }
            }

            foreach (double res in result)
            {
                k = 0;
                result[k] = -Exp(res) / Pow((1 + Exp(res)), 2);
                k++;
            }

            return result;
        }

        public double[] TahnDerivative(double[] inputArray, double[,] weightArray)
        {
            double[] result = new double[weightArray.GetLength(0)];

            int k = 0;

            for (int i = 0; i < weightArray.GetLength(0); i++)
            {
                foreach (double input in inputArray)
                {
                    result[k] += input * weightArray[i, k];
                    k++;
                }
            }

            foreach (double res in result)
            {
                k = 0;
                result[k] = -(4 * Exp(-2 * res)) / (Pow((1 + Exp(-2 * res)), 2));
                k++;
            }

            return result;
        }

        public double[] HyperbolicDeivative(double[] inputArray, double[,] weightArray)
        {
            double[] result = new double[weightArray.GetLength(0)];

            int k = 0;

            for (int i = 0; i < weightArray.GetLength(0); i++)
            {
                foreach (double input in inputArray)
                {
                    result[k] += input * weightArray[i, k];
                    k++;
                }
            }

            foreach (double res in result)
            {
                k = 0;
                result[k] = 4 * Exp(res) / Pow((Exp(2 * res) + 1), 2);
                k++;
            }

            return result;
        }

        public double[] LinearDeivative(double[] inputArray, double[,] weightArray)
        {
            double[] result = new double[weightArray.GetLength(0)];

            int k = 0;

            for (int i = 0; i < weightArray.GetLength(0); i++)
            {
                foreach (double input in inputArray)
                {
                    result[k] += input * weightArray[i, k];
                    k++;
                }
            }

            foreach (double res in result)
            {
                k = 0;
                result[k] = 1;
                k++;
            }

            return result;
        }
        #endregion
    }
}
