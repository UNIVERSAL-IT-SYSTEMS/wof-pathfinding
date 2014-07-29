using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PathFinding
{
    [TestClass]
    public class PathTests
    {
        [TestMethod]
        public void PathFromNodeTest()
        {
            Node a = new Node();
            Node b = new Node();

            Path path = new Path(a);

            Assert.AreEqual(0, path.Cost, "Initial path cost should be 0.");
            Assert.AreEqual(a, path.LastNode, "Path with single node A should have LastNode = A.");
            //Test that list is accurate?
        }

        [TestMethod]
        public void PathFromPathTest()
        {
            Node a = new Node();
            Node b = new Node();
            Edge ab = new Edge(a, b, 20);
            Path path = new Path(a);
            path.addEdgeToPath(ab);

            Path newPath = new Path(path);
            Assert.AreEqual(path.Cost, newPath.Cost, "Initial path cost should be 0.");
            Assert.AreEqual(path.LastNode, newPath.LastNode, "Path with single node A should have LastNode = A.");
            Assert.AreNotSame(path, newPath);
            Assert.AreNotSame(path.ListOfNodes, newPath.ListOfNodes);
            //Test that list is accurate?
        }

        [TestMethod]
        public void AddEdgeToPathTest()
        {
            Node a = new Node();
            Node b = new Node();
            Edge ab = new Edge(a, b, 20);            
            Path path = new Path(a);
            path.addEdgeToPath(ab);

            Assert.AreEqual(b, path.LastNode, "addEdgeToPath not editing path's LastNode correctly.");
            Assert.AreEqual(ab.Weight, path.Cost, "Initial path cost should be 0.");
            //Test that list is accurate?
        }

        [TestMethod, ExpectedException(typeof(Exception), "addEdgeToPath should throw an exception when an edge is added that doesn't connect to the current last node of the path.")]
        public void InappropriateEdgeAddedToPath()
        {
            Node a = new Node();
            Node b = new Node();
            Node c = new Node();

            Edge ab = new Edge(a, b, 20);
            Edge ac = new Edge(a, c, 10);

            Path path = new Path(a);
            path.addEdgeToPath(ab);
            path.addEdgeToPath(ac);
        }

        [TestMethod]
        public void PathEqualTest()
        {
            Node a = new Node(1, 0, 0);
            Node b = new Node(2, 0, 0);
            Node c = new Node(3, 0, 0);

            Edge ab = new Edge(a, b, 10);
            Edge ac = new Edge(a, c, 10);
            Edge abLonger = new Edge(a, b, 20);

            Path samePath1 = new Path(a);
            Path samePath2 = new Path(a);

            Assert.AreEqual(samePath1, samePath2, "Two paths with the same nodes in the same order should be equal.");

            samePath1.addEdgeToPath(ab);
            samePath2.addEdgeToPath(ab);

            Assert.AreEqual(samePath1, samePath2, "Two paths with the same nodes in the same order should be equal.");
            
            Path differentNodesPath1 = new Path(a);
            differentNodesPath1.addEdgeToPath(ac);
            Assert.AreNotEqual(samePath1, differentNodesPath1, "Two paths with different nodes should not be equal.");

            Path differentCostPath1 = new Path(a);
            differentCostPath1.addEdgeToPath(abLonger);
            Assert.AreNotEqual(samePath1, differentCostPath1, "Two paths with different costs should not be equal.");

            Path differentOrderPath1 = new Path(b);
            differentOrderPath1.addEdgeToPath(ab);
            Assert.AreNotEqual(samePath1, differentOrderPath1, "Two paths with nodes in different orders should not be equal.");
            
            Path longerPath1 = new Path(a);
            longerPath1.addEdgeToPath(ab);
            Edge bc = new Edge(b, c, 10);
            longerPath1.addEdgeToPath(bc);

            Assert.AreNotEqual(samePath1, longerPath1, "Two paths with different costs and nodes should not be equal.");
        }

        [TestMethod]
        public void PathCompareToTest()
        {
            Node a = new Node();

            Path path = new Path(a);
            Node b = new Node();
            Edge ab = new Edge(a, b, 20);
            path.addEdgeToPath(ab);

            Path shorterPath = new Path(a);
            Node c = new Node();
            Edge ac = new Edge(a, c, 10);
            shorterPath.addEdgeToPath(ac);

            Path equalPath = new Path(c);
            Node d = new Node();
            Edge equalEdge = new Edge(c, d, ab.Weight);
            equalPath.addEdgeToPath(equalEdge);

            Assert.IsTrue(path.CompareTo(shorterPath) > 0, "Shorter path should be less than longer path when being compared.");
            Assert.IsTrue(path.CompareTo(equalPath) == 0, "Paths with the same cost should be equal when being compared.");
        }

        [TestMethod]
        public void PathToStringTest()
        {
            //Testing Path ToString()
            Node one = new Node(1, 0, 0);
            Node two = new Node(2, 0, 0);
            Node three = new Node(3, 0, 0);
            Node four = new Node(4, 0, 0);
            Edge onetwo = new Edge(one, two, 5);
            Edge onethree = new Edge(one, three, 10);
            Edge twothree = new Edge(two, three, 15);
            Edge twofour = new Edge(two, four, 20);
            Path longPath = new Path(one);
            //Console.WriteLine("LongPath: " + longPath);
            longPath.addEdgeToPath(onethree);
            //Console.WriteLine("LongPath: " + longPath);
            longPath.addEdgeToPath(twothree);
            //Console.WriteLine("LongPath: " + longPath);
            longPath.addEdgeToPath(twofour);
            //Console.WriteLine("LongPath: "+longPath);
            Assert.AreEqual("Path: < (1) (3) (2) (4) >", longPath.ToString(), "ToString override not working as expected.");
        }

        
        [TestMethod]
        public void GetDirectionsFromPathTest()
        {
            double scale = 10;

            Point p1 = new Point(0, -1);
            Point p2 = new Point(0, 0);
            Point p3 = new Point(1, 1);
            Point p4 = new Point(0, 1);
            Point p5 = new Point(1, 1);

            Node n1 = new Node(1, p1);
            Node n2 = new Node(2, p2);
            Node n3 = new Node(3, p3);
            Node n4 = new Node(4, p4);
            Node n5 = new Node(5, p5);

            Edge onetwo = new Edge(n1, n2, 3);
            Edge twothree = new Edge(n2, n3, 5);
            Edge threefour = new Edge(n3, n4, 7);
            Edge fourfive = new Edge(n4, n5, 9);

            Path p = new Path(n1);
            p.addEdgeToPath(onetwo);
            p.addEdgeToPath(twothree);
            p.addEdgeToPath(threefour);
            p.addEdgeToPath(fourfive);

            LinkedList<Direction> listOfDirections = p.getListOfDirections(scale);

            Point p0 = new Point(2*p1.X - p2.X, 2*p1.Y - p2.Y); //immaginary previous point. Assuming the robot is already facing the right direction.
            Direction expectedFirstDirection = new Direction(p0, p1, p2, scale);
            Direction firstDirection = listOfDirections.First.Value;
            Assert.AreEqual(expectedFirstDirection, firstDirection, "First direction not calculated properly.");

            Direction expectedSecondDirection = new Direction(p1, p2, p3, scale);
            Direction secondDirection = listOfDirections.First.Next.Value;
            Assert.AreEqual(expectedSecondDirection, secondDirection, "Second direction not calculated properly.");

            Direction expectedThirdDirection = new Direction(p2, p3, p4, scale);
            Direction thirdDirection = listOfDirections.First.Next.Next.Value;
            Assert.AreEqual(expectedThirdDirection, thirdDirection, "Third direction not calculated properly.");

            Direction expectedFourthDirection = new Direction(p3, p4, p5, scale);
            Direction fourthDirection = listOfDirections.First.Next.Next.Next.Value;
            Assert.AreEqual(expectedFourthDirection, fourthDirection, "Fourth direction not calculated properly.");

            Assert.AreEqual(listOfDirections.Last.Value, fourthDirection, "Fourth direction should be the last direction. Too many directions created.");
        }

        [TestMethod]
        public void GetJSONDirectionsFromPathTest()
        {
            double scale = 1;
            Point p1 = new Point(0, -1);
            Point p2 = new Point(0, 0);
            Point p3 = new Point(1, 1);
            Point p4 = new Point(0, 1);
            Point p5 = new Point(1, 1);

            Node n1 = new Node(1, p1);
            Node n2 = new Node(2, p2);
            Node n3 = new Node(3, p3);
            Node n4 = new Node(4, p4);
            Node n5 = new Node(5, p5);

            Edge onetwo = new Edge(n1, n2, 3);
            Edge twothree = new Edge(n2, n3, 5);
            Edge threefour = new Edge(n3, n4, 7);
            Edge fourfive = new Edge(n4, n5, 9);

            Path p = new Path(n1);
            p.addEdgeToPath(onetwo);
            p.addEdgeToPath(twothree);
            p.addEdgeToPath(threefour);
            p.addEdgeToPath(fourfive);

            LinkedList<Direction> listOfDirections = p.getListOfDirections(scale);
            String jsonDirections = p.getJSONDirections(scale);
            String expectedDirections = "{\"commandList\":[{\"angle\":0,\"distance\":1},{\"angle\":-45,\"distance\":1.4142135623730952},{\"angle\":135,\"distance\":1},{\"angle\":-180,\"distance\":1}]}";
            Assert.AreEqual(expectedDirections, jsonDirections, "JSON directions incorrect.");
        }
    }
}
