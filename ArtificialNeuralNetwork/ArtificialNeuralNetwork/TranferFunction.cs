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

	/*public double[] Tahn(double[] inputArray, double[,] weightArray)
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
	}*/

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

	p/*ublic double[] Linear(double[] inputArray, double[,] weightArray)
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
		}

		return result;
	}*/

	/*public double[] SoftMax(double[] inputArray, double[,] weightArray)
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

        double sumExp = 0;

		foreach (double input in result)
		{
			sumExp += Exp(input);
		}

		foreach (double res in result)
		{
			k = 0;
			result[k] = Exp(k) / sumExp;
			k++;
		}

		return result;
	}
*/
	//Deretive

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

	/*public double[] TahnDerivative(double[] inputArray, double[,] weightArray)
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
			result[k] = -(4 * Exp(-2 * res)) / (Pow((1 + Exp(-2 * res)),2));
			k++;
		}

		return result;
	}*/

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

	/*public double[] LinearDeivative(double[] inputArray, double[,] weightArray)
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
	}*/
}
