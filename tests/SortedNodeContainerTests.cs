using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PathFinding
{
    [TestClass]
    public class sortedNodeContainerTests
    {
        [TestMethod]
        public void ifEmptyAddNode()
        {
            Converter.SortedNodeContainer test = new Converter.SortedNodeContainer(0.01);
            Node node = new Node();
            node.OfficeLocation = 666;
            node.CrossingPoint = new Point(5, 5);
            test.Add(node);
            Assert.AreEqual(1, test.Count, "the resulting container correctly has 1 element");
            Assert.AreEqual(node, test[0], "the resulting container has the correct node");

        }

        [TestMethod]
        public void ifHasOneNodeAddNode()
        {
            Converter.SortedNodeContainer test = new Converter.SortedNodeContainer(0.01);
            Node node1 = new Node();
            node1.OfficeLocation = 666;
            node1.CrossingPoint = new Point(5, 5);
            test.Add(node1);
            Node node2 = new Node();
            node2.CrossingPoint = new Point(7, 13);
            test.Add(node2);
            Assert.AreEqual(2, test.Count, "the resulting container correctly has 2 elements");
            Assert.AreEqual(node1, test[0], "the resulting container has the correct node");
            Assert.AreEqual(node2, test[1], "the resulting container has the correct node");

        }

        [TestMethod]
        public void testAddingTotheBeginning()
        {
            Converter.SortedNodeContainer test = new Converter.SortedNodeContainer(0.01);
            Node node1 = new Node();
            node1.OfficeLocation = 666;
            node1.CrossingPoint = new Point(5, 5);
            test.Add(node1);
            Node node2 = new Node();
            node2.CrossingPoint = new Point(7, 7);
            test.Add(node2);
            Node node3 = new Node();
            node3.CrossingPoint = new Point((float)0.5, (float)0.5);
            test.Add(node3);
            Assert.AreEqual(3, test.Count, "the resulting container correctly has 3 elements");
            Assert.AreEqual(node3, test[0], "the node has been correctly added to the beginning");
            
        }

        [TestMethod]
        public void testAddingTotheEnd()
        {
            Converter.SortedNodeContainer test = new Converter.SortedNodeContainer(0.01);
            Node node1 = new Node();
            node1.OfficeLocation = 666;
            node1.CrossingPoint = new Point(5, 5);
            test.Add(node1);
            Node node2 = new Node();
            node2.CrossingPoint = new Point(7, 7);
            test.Add(node2);
            Node node3 = new Node();
            node3.CrossingPoint = new Point((float)7.5, (float)7.5);
            test.Add(node3);
            Assert.AreEqual(3, test.Count, "the resulting container correctly has 3 elements");
            Assert.AreEqual(node3, test[2], "the node has been correctly added to the end");

        }

        [TestMethod]
        public void testAddingTotheMiddle()
        {
            Converter.SortedNodeContainer test = new Converter.SortedNodeContainer(0.01);
            Node node1 = new Node();
            node1.OfficeLocation = 666;
            node1.CrossingPoint = new Point(0, 0);
            test.Add(node1);
            Node node2 = new Node();
            node2.CrossingPoint = new Point(7, 7);
            test.Add(node2);
            Node node3 = new Node();
            node3.CrossingPoint = new Point((float)5.4, (float)5.4);
            test.Add(node3);
            Assert.AreEqual(3, test.Count, "the container correctly has 3 elements");
            Assert.AreEqual(node3, test[1], "the node has been correctly added to the middle");

            Node node4 = new Node();
            node4.CrossingPoint = new Point((float)3.1, (float)3.1);
            test.Add(node4);
            Assert.AreEqual(4, test.Count, "the container correctly has 4 elements");
            Assert.AreEqual(node4, test[1], "the node has been correctly added to position 1");

            Node node5 = new Node();
            node5.CrossingPoint = new Point(6, 6);
            test.Add(node5);
            Assert.AreEqual(5, test.Count, "the container correctly has 5 elements");
            Assert.AreEqual(node5, test[3], "the node has been correctly added to position 3");

            Node node6 = new Node();
            node6.CrossingPoint = new Point((float)6.5, (float)6.5);
            test.Add(node6);
            Assert.AreEqual(6, test.Count, "the container correctly has 6 elements");
            Assert.AreEqual(node6, test[4], "the node has been correctly added to position 4");
        }
    }
}
