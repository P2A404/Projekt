using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

class TransferFunction
{
	public double[] Logistic(double[] inputArray, double[,] weightArray)
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
			result[k] = 1 / (1 + Exp(-res));
			k++;
		}

		return result;
	}

	public double[] Tahn(double[] inputArray, double[,] weightArray)
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
			result[k] = 2 / (1 + Exp(-2 * res)) - 1;
			k++;
		}

		return result;
	}

	public double[] Hyperbolic(double[] inputArray, double[,] weightArray)
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
			result[k] = (Exp(res) - Exp(-res)) / (Exp(res) + Exp(-res));
			k++;
		}

		return result;
	}

	public double[] Linear(double[] inputArray, double[,] weightArray)
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

		/*foreach (double res in result)
		{
			k = 0;
			result[k] = 1 / (1 + Exp(-res));
			k++;
		}*/

		return result;
	}
}