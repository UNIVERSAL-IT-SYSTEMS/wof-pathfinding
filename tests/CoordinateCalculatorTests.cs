using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PathFinding;

namespace calcTest
{
    [TestClass]
    public class calculatorTests
    {
        [TestMethod]
        public void ifStartEndPointSameThenScaleIsZero()
        {
            Assert.AreEqual<double>(0, CoordinateCalculator.getScale(new Point(0, 0), new Point(0, 0), 20),
            "The scale of zero is correctely calculated");

            Assert.AreEqual<double>(0, CoordinateCalculator.getScale(new Point((float)5.14, (float)3.11), new Point((float)5.14, (float)3.11), 11.3),
            "The scale of zero is correctely calculated");

        }

        [TestMethod]
        public void ifLengthIsZeroThenScaleIsZero()
        {
             Assert.AreEqual<double>(0, CoordinateCalculator.getScale(new Point(0, 0), new Point(0, 0), 0),
            "The scale of zero is correctely calculated");

             Assert.AreEqual<double>(0, CoordinateCalculator.getScale(new Point((float)3.25, (float)6.6666), new Point((float)8.99, (float)0.0005), 0),
            "The scale of zero is correctely calculated");     

        }

        [TestMethod]
        public void ifDistanceIsVerticalOrHorizontal()
        {
            Assert.AreEqual<double>(0.5, CoordinateCalculator.getScale(new Point(0, 0), new Point(1, 0), 2),
            "The scale of 1/2 is correctely calculated");
            
            Assert.AreEqual<double>(0.5, CoordinateCalculator.getScale(new Point(0, 0), new Point(0, 1), 2),
            "The scale of 1/2 is correctely calculated");

        }


        [TestMethod]
        public void testDistanceCalculation()
        {
            Assert.AreEqual<double>(5, CoordinateCalculator.euclideanDistance(new Point(1, 2), new Point(4, 6)),
            "the distance of 5 between points (1,2), and (4,6) is calculated correctely");
        }


        [TestMethod]
        public void testScaleCalculation()
        {
            Assert.AreEqual<double>(2.5, CoordinateCalculator.getScale(new Point(0, 0), new Point(3, 4), 2),
            "The scale of 5/2 is correctely calculated");
        }

        
     
       
    }
}
