using NUnit.Framework;
using ArtificialNeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNeuralNetwork.Tests
{
    //Arrange - Gør test cases klar
    //Act - Kør koden med test cases
    //Assert - Tjek om antagelser omkring test cases er korrekte
    [TestFixture()]
    public class NeuralNetworkTests
    {
        [TestFixtureSetUp()]
        public void SetupSuite()
        {

        }
        [Test()]
        public void PrintArrayTest()
        {
            Assert.Fail();
        }

        private double[] tf(double[] input) { return input; }
        [Test()]
        public void SumTest()
        {
            //Arrage
            double[] input = new double[] { 0 };
            double[,] weights = new double[1, 1] { { 0 } };
            NeuralNetwork nn = new NeuralNetwork(new int[] { 0 }, tf, tf, tf, tf);
            //Act
            double[] result = nn.Sum(input, weights);
            //Asser
            Assert.AreEqual(new double[]{ 0 }, result);
        }

        [Test()]
        public void CycleTest()
        {
            Assert.Fail();
        }
    }
}