using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PathFinding
{
    [TestClass]
    public class LineTests
    {
        [TestMethod]
        public void testRotate()
        {
            Line test = new Line(new Point(-2, 3), new Point(2,1));
            test.rotate(90);
            Assert.AreEqual(-1, test.getEndPoint().X, 0.1, "the resulting end x coordinate is correct");
            Assert.AreEqual(2, test.getEndPoint().Y, 0.1, "the resulting end y coordinate is correct");
            Assert.AreEqual(-3, test.getStartPoint().X, 0.1, "the resulting start x coordinate is correct");
            Assert.AreEqual(-2, test.getStartPoint().Y, 0.1, "the resulting start y coordinate is correct");
        }

        [TestMethod]
        public void testTranslate()
        {
            Line test = new Line(new Point((float)101.11, 3), new Point((float)33.3333, (float)-36.38));
            test.translate(10, -1);
            Assert.AreEqual(43.3333, test.getEndPoint().X, 0.0001, "the resulting end x coordinate is correct");
            Assert.AreEqual(-37.38, test.getEndPoint().Y, 0.0001, "the resulting end y coordinate is correct");
            Assert.AreEqual(111.11, test.getStartPoint().X, 0.0001, "the resulting start x coordinate is correct");
            Assert.AreEqual(2, test.getStartPoint().Y, 0.0001, "the resulting start y coordinate is correct");
        }

        [TestMethod]
        public void ifLinesDoNotIntersectThenReturnsNull()
        {
            Line a = new Line(new Point(0, 0), new Point(1, 1));
            Line b = new Line(new Point(-3, -3), new Point(-1, -1));
            
            Assert.AreEqual(null, a.crosses(b, 0.01), "the crossing point is correctely null");
            
        }

        [TestMethod]
        public void ifLinesAreParallelAndDoNotInterSectThenReturnsNull()
        {
            Line a = new Line(new Point(0, 0), new Point(1, 1));
            Line b = new Line(new Point(-2, -2), new Point(-1, -1));

            Assert.AreEqual(null, a.crosses(b, 0.01), "the crossing point is correctely null");

        }

        [TestMethod]
        public void ifLinesAreParallelAndInterSectThenReturnsIntersection()
        {
            Line a = new Line(new Point(0, 0), new Point(1, 1));
            Line b = new Line(new Point(-1, -1), new Point(0, 0));

            Assert.AreEqual(0, a.crosses(b, 0.01).X, "the crossing point x is correct");
            Assert.AreEqual(0, a.crosses(b, 0.01).Y, "the crossing point y is correct");

        }

        public void ifLinesCrossThenReturnsCrossingPt()
        {
            Line a = new Line(new Point(0, 0), new Point(2, 2));
            Line b = new Line(new Point(0, 2), new Point(2, 0));

            Assert.AreEqual(new Point(1, 1), a.crosses(b, 0.01), "the crossing point is correctely the intersection point");

            a = new Line(new Point(1, 0), new Point(1, 2));
            b = new Line(new Point(0, 0), new Point(2, 2));

            Assert.AreEqual(new Point(1, 1), a.crosses(b, 0.01), "the crossing point is correctely the intersection point for vertical line");


        }

        [TestMethod]
        public void testGetSlope()
        {
            Line test = new Line(new Point(10, 10), new Point(15, 20));   

            Assert.AreEqual(2, test.getSlope(), "the slope is correct");

        }

        [TestMethod]
        public void testGetConst()
        {
            Line test = new Line(new Point(10, 10), new Point(15, 20));

            Assert.AreEqual(-10, test.getConstant(), "the constant is correct");

        }

        [TestMethod]
        public void testContains()
        {
            Line test = new Line(new Point(0, 0), new Point(1, 1));

            Assert.AreEqual(true, test.contains(new Point((float)0.5, (float)0.5), 0.01), 
                "the line is correctely judged to contain the point");
            Assert.AreEqual(false, test.contains(new Point(-1, -1), 0.01),
                "the line is correctely judged to not contain the point");
            Assert.AreEqual(true, test.contains(new Point(1, 1), 0.01),
                "the line is correctely judged to contain the point"); 

        }



    }
}