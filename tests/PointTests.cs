using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PathFinding;

namespace pointTest
{
    [TestClass]
    public class pointTests
    {
        [TestMethod]
        public void xAndYAssignmentTest()
        {
            float x = (float)3.0;
            float y = (float)4.0;
            Point p = new Point(x, y);
            Assert.AreEqual(x, p.X, "X-Coordinate not being set properly at instantiation.");
            Assert.AreEqual(y, p.Y, "Y-Coordinate not being set properly at instantiation.");
        }

        [TestMethod]
        public void xAndYReassignmentTest()
        {
            Point p = new Point(0, 0);
            int newX = 1;
            int newY = 2;
            p.X = newX;
            Assert.AreEqual(newX, p.X, "X-Coordinate property not being re-set properly.");
            p.Y = newY;
            Assert.AreEqual(newY, p.Y, "Y-Coordinate property not being re-set properly.");
        }

        [TestMethod]
        public void TranslateTest()
        {
            Point p = new Point(0, 0);
            double translateX = 4.5;
            double translateY = 3.5;
            p.translate((float)translateX, (float)translateY);
            Assert.AreEqual(translateX, p.X, "X-Coordinate not being translated properly.");
            Assert.AreEqual(translateY, p.Y, "Y-Coordinate not being translated properly.");
            p.translate((float)translateX, (float)translateY);
            Assert.AreEqual(2*translateX, p.X, "X-Coordinate not being translated properly.");
            Assert.AreEqual(2*translateY, p.Y, "Y-Coordinate not being translated properly.");
        }

        [TestMethod]
        public void RotateTest()
        {
            Point test = new Point(2, 1);
            test.rotate(90);
            Assert.AreEqual(-1, test.X, 0.001, "X-Coordinate not being rotated correctly.");
            Assert.AreEqual(2, test.Y, 0.001, "Y-Coordinate not being rotated correctly.");
        }

        [TestMethod]
        public void TransformTest()
        {
            float startX = 1;
            float startY = 2;
            float degreeRotation = 90;
            float translateX = 1;
            float translateY = 2;

            float expectedX = -1;
            float expectedY = 3;

            //Basically checking that expectedX and expectedY are right.
            Point newP = new Point(startX, startY);
            newP.rotate(degreeRotation);
            newP.translate(translateX, translateY);
            Assert.AreEqual(expectedX, newP.X, 0.001, "Tranform function not behaving like rotate and then translate with X-Coordinate.");
            Assert.AreEqual(expectedY, newP.Y, 0.001, "Tranform function not behaving like rotate and then translate with Y-Coordinate.");

            Point p = new Point(startX, startY);
            p.transform(translateX, translateY, degreeRotation);
            Assert.AreEqual(expectedX, p.X, 0.001, "X-Coordinate not being transformed correctly.");
            Assert.AreEqual(expectedY, p.Y, 0.001, "Y-Coordinate not being transformed correctly.");
        }

        [TestMethod]
        public void EqualsTest()
        {
            Point a = new Point((float)1.11, (float)1.11);
            Point b = new Point((float)1.11, (float)1.11);
            Point c = new Point(2, (float)1.11);

            Assert.AreEqual(a, a, "The Equals function is incorrectly indicating that the a point is not equal to itself.");
            Assert.AreEqual(a, b, "The Equals function is incorrectly indicating that two points with the same coordiantes are not equal.");
            Assert.AreNotEqual(a, c, "The Equals function is incorrectly indicating that two points with different coordinates are not equal.");
        }
    }
}
