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
        private double[] tf(double[] input) { return input; }
        private NeuralNetwork nn;
        

        [TestFixtureSetUp()]
        public void SetupSuite()
        {
            nn = new NeuralNetwork(new int[] { 0 }, tf, tf, tf, tf);
        }

        [Test()]
        public void PrintArrayTest()
        {
            Assert.Pass();
        }

        
        [Test()]
        public void Sum_Empty()
        {
            //Arrage
            double[] input = new double[0];
            double[,] weights = new double[0,0];
            //Act
            double[] result = nn.Sum(input, weights);
            //Asser
            Assert.AreEqual(new double[0], result);
        }

        [Test()]
        public void Sum_Difference()
        {
            //Arrage
            double[] largeInput = new double[1000000];
            double[,] smallWeights = new double[,] { { 0 } };
            //Act
            double[] result = nn.Sum(largeInput, smallWeights);
            //Asser
            Assert.AreEqual(new double[] { 0 }, result);
        }

        [Test()]
        public void Sum_Null_Test()
        {
            Assert.Multiple(()=>
            {
                Assert.Throws(typeof(ArgumentNullException), delegate () { nn.Sum(null, null); });
                Assert.Throws(typeof(ArgumentNullException), delegate () { nn.Sum(new double[0], null); });
                Assert.Throws(typeof(ArgumentNullException), delegate () { nn.Sum(null, new double[0,0]); });
                Assert.Throws(typeof(ArgumentNullException), delegate () { nn.Sum(new double[] { 0 }, null); });
                Assert.Throws(typeof(ArgumentNullException), delegate () { nn.Sum(null, new double[,] { { 0 } }); });
            });
        }

        [Test()]
        public void CycleTest()
        {
            Assert.Pass();
        }
    }

}