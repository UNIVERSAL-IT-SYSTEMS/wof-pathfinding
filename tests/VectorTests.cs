using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PathFinding
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void MagnitudeTest()
        {
            Vector v = new Vector(3, 4);
            Assert.AreEqual(5, v.magnitude(), "Vector Magnitude not being calculated correctly.");
        }

        [TestMethod]
        public void DotProductTest()
        {
            Vector v1 = new Vector(2, 3);
            Vector v2 = new Vector(5, 7);
            double expectedDotProduct = 2 * 5 + 3 * 7;
            Assert.AreEqual(expectedDotProduct, v1.dotProduct(v2), "Dot product not being calculated correctly.");
            Assert.AreEqual(expectedDotProduct, v2.dotProduct(v1), "Order of vectors shouldn't matter when calculating dot product.");
        }

        [TestMethod]
        public void DeterminantTest()
        {
            Vector v1 = new Vector(2, 3);
            Vector v2 = new Vector(5, 7);
            double expectedDeterminant = -1;
            Assert.AreEqual(expectedDeterminant, v1.determinant(v2));
            Assert.AreEqual(-expectedDeterminant, v2.determinant(v1));
        }

        [TestMethod]
        public void RadianAngleToTest()
        {
            Vector v1 = new Vector(0, 1);
            Vector v2 = new Vector(1, 0);
            double expectedRadianAngle = -Math.PI/2.0;
            Assert.AreEqual(expectedRadianAngle, v1.radianAngleTo(v2), 0.001, "Vectors not correctly calculating angle between themselves.");
            Assert.AreEqual(-expectedRadianAngle, v2.radianAngleTo(v1), 0.001, "The angle in the opposite direction should be the negative of the angle in the original direction.");

            v1 = new Vector(3, 4.001);
            v2 = new Vector(-3, -4);
            expectedRadianAngle = Math.PI;
            Assert.AreEqual(expectedRadianAngle, v1.radianAngleTo(v2), 0.001, "Vectors not correctly calculating angle between themselves.");
            Assert.AreEqual(-expectedRadianAngle, v2.radianAngleTo(v1), 0.001, "Vectors not correctly calculating angle between themselves.");

            //When angles are exactly opposite, both orderings are equal to positive Pi.
        }

        [TestMethod]
        public void DegreeAngleToTest()
        {
            Vector v1 = new Vector(0, 1);
            Vector v2 = new Vector(1, 0);
            double expectedDegreeAngle = -90;
            Assert.AreEqual(expectedDegreeAngle, v1.degreeAngleTo(v2), 0.01, "Vectors not correctly calculating angle between themselves.");
            Assert.AreEqual(-expectedDegreeAngle, v2.degreeAngleTo(v1), 0.01, "The angle in the opposite direction should be the negative of the angle in the original direction.");

            v1 = new Vector(3, 4.001);
            v2 = new Vector(-3, -4);
            expectedDegreeAngle = 180;
            Assert.AreEqual(expectedDegreeAngle, v1.degreeAngleTo(v2), 0.01, "Vectors not correctly calculating angle between themselves.");
            Assert.AreEqual(-expectedDegreeAngle, v2.degreeAngleTo(v1), 0.01, "Vectors not correctly calculating angle between themselves.");

            //When angles are exactly opposite, both orderings are equal to positive 180.
        }
    }
}
