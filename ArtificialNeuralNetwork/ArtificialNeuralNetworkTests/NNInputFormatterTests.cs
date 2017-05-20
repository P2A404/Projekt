using NUnit.Framework;
using ArtificialNeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNeuralNetwork.Tests
{
    [TestFixture()]
    public class NNInputFormatterTests
    {
        private NNInputFormatter formatter = new NNInputFormatter(2000);

        [TestCase(0, 0, 0, 0)]
        [TestCase(1, 0, 0, 0)]
        [TestCase(1, 0, 1, 1)]
        [TestCase(0, 0, 1, -1)]
        public void NormalizationTest(double curr, double min, double max, double expected)
        {
            double actual = formatter.Normalization(curr, min, max);
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void NormilizationTest_WrongInput()
        {
            Assert.Multiple(() =>
            {
                Assert.Throws(typeof(ArgumentOutOfRangeException), delegate () { formatter.Normalization(0, 1, 0); });
                Assert.Throws(typeof(ArgumentOutOfRangeException), delegate () { formatter.Normalization(1, 1, 0); });
                Assert.Throws(typeof(ArgumentOutOfRangeException), delegate () { formatter.Normalization(-1, 1, 0); });
                Assert.Throws(typeof(ArgumentOutOfRangeException), delegate () { formatter.Normalization(0, 100, 99); });
                Assert.Throws(typeof(ArgumentOutOfRangeException), delegate () { formatter.Normalization(0, 100, 0); });
                Assert.Throws(typeof(ArgumentOutOfRangeException), delegate () { formatter.Normalization(0, 100, -100); });
            });
        }
    }
}