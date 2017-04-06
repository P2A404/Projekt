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
    }
}
