using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PathFinding
{
    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void NodeInstantiationTest()
        {
            //No Arguments
            Node n = new Node();
            Assert.AreEqual(0, n.CrossingPoint.X, "Node instantiated without parameters should have x set to zero.");
            Assert.AreEqual(0, n.CrossingPoint.Y, "Node instantiated without parameters should have y set to zero.");
            Assert.AreEqual(-1, n.OfficeLocation, "Node instantiated without parameters should have OfficeLocation set to -1.");

            float x = 1;
            float y = 2;
            int officeNum = 7424;
            //officeNumber, x-coordinate, y-coordinate
            n = new Node(officeNum, x, y);
            Assert.AreEqual(x, n.CrossingPoint.X, "Node not properly setting X when instanciated with Node(officeNum, X, Y).");
            Assert.AreEqual(y, n.CrossingPoint.Y, "Node not properly setting X when instanciated with Node(officeNum, X, Y).");
            Assert.AreEqual(officeNum, n.OfficeLocation, "Node not properly setting officeLocation when instanciated with Node(officeNum, X, Y).");

            //officeNumber, point
            Point p = new Point(x, y);
            n = new Node(officeNum, p);
            Assert.AreEqual(p, n.CrossingPoint, "Node not properly setting CrossingPoint when instanciated with Node(officeNum, crossingPoint).");
            Assert.AreEqual(officeNum, n.OfficeLocation, "Node not properly setting officeLocation when instanciated with Node(officeNum, crossingPoint).");
        }
        [TestMethod]
        public void propertyAssignmentTest()
        {
            
            Point newCrossingPoint = new Point(1,2);
            int newOfficeLocation = 7424;
            
            Node n = new Node();
            
            n.CrossingPoint = newCrossingPoint;
            Assert.AreEqual(newCrossingPoint, n.CrossingPoint, "CrossingPoint not being reassigned properly.");
            
            n.OfficeLocation = newOfficeLocation;
            Assert.AreEqual(newOfficeLocation, n.OfficeLocation, "OfficeLocation not being reassigned properly.");
        }

        [TestMethod]
        public void ToStringTest()
        {
            Node n = new Node();
            int newOfficeLocation = 7424;
            n.OfficeLocation = newOfficeLocation;
            Assert.AreEqual(newOfficeLocation.ToString(), n.ToString(), "ToString() should print out the office number.");
        }

        [TestMethod]
        public void NodeAddEdgeTest()
        {
            Node a = new Node(1, 0, 0);
            Node b = new Node(2, 0, 0);
            Node c = new Node(3, 0, 0);

            Edge ab = new Edge(a, b, 10);

            Assert.AreEqual(true, a.Edges.Contains(ab), "Edge ab should have been added to a's list of edges.");
            Assert.AreEqual(true, b.Edges.Contains(ab), "Edge ab should have been added to b's list of edges.");
            Assert.AreEqual(1, a.Edges.Count, "Node a should only have one edge (Edge ab) in its list of edges.");
            Assert.AreEqual(1, b.Edges.Count, "Node b should only have one edge (Edge ab) in its list of edges.");

            Edge ac = new Edge(a, c, 10);

            Assert.AreEqual(true, a.Edges.Contains(ac), "Edge ac should have been added to a's list of edges.");
            Assert.AreEqual(true, a.Edges.Contains(ab), "Edge ab should still be in a's list of edges.");
            Assert.AreEqual(false, b.Edges.Contains(ac), "Node b should not have Edge ac in its list of edges.");
            Assert.AreEqual(true, c.Edges.Contains(ac), "Edge ab should have been added to c's list of edges.");
            Assert.AreEqual(2, a.Edges.Count, "Node a should only have ab and ac in its list of edges.");
            Assert.AreEqual(1, b.Edges.Count, "Node b should only have one edge (Edge ab) in its list of edges.");
            Assert.AreEqual(1, c.Edges.Count, "Node c should only have one edge (Edge ac) in its list of edges.");
        }

        [TestMethod]
        public void NodeEqualsTest()
        {
            Point p = new Point(1, 2);
            Node equal1 = new Node(7424, 1, 2);
            Node equal2 = new Node(7424, p);
            Node difCrossingPoint1 = new Node(7424, 1, 3);
            Node difRoomNum2 = new Node(7000, p);
            Assert.AreEqual(equal1, equal2, "Nodes with the same crosspoint and office number should be considered equal.");
            Assert.AreNotEqual(equal2, difRoomNum2, "Nodes with different room numbers should not be considered equal.");
            Assert.AreNotEqual(equal1, difCrossingPoint1, "Nodes with different crossing points should not be considered equal.");
        }

        [TestMethod]
        public void NodeIsCloseToTest()
        {
            double epsilon = 0.01;
            float x = 1;
            float y = 2;
            Point p = new Point(x, y);
            Node equal1 = new Node(7424, (float)(x + epsilon), (float)(y - epsilon));
            Node equal2 = new Node(7424, p);
            Node offByTooMuch = new Node(7424, (float)(x+2*epsilon), y);
            Assert.AreEqual(true, equal1.isCloseTo(equal2, epsilon), "Nodes with coordinates within the given epsilon of eachother should be considered close.");
            Assert.AreNotEqual(true, equal2.isCloseTo(offByTooMuch, epsilon), "Nodes with differences greater than epsilon should not be considered close.");
        }
    }
}
