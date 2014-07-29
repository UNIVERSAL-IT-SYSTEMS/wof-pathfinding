using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PathFinding
{
    [TestClass]
    public class EdgeTests
    {
        [TestMethod]
        public void UpdateWeightTests()
        {
            Node a = new Node();
            Node b = new Node();
            int weight = 20;
            Edge ab = new Edge(a, b, 20);
            Assert.AreEqual(weight, ab.Weight, "Weight not being set correctly.");
        }
        [TestMethod]
        public void OtherNodeTest()
        {
            Node a = new Node();
            Node b = new Node();
            Edge ab = new Edge(a, b, 20);
            if (ab.otherNode(a) != b) Console.WriteLine("Othernode not working. AB.otherNode(A) should be B.");
            if (ab.otherNode(b) != a) Console.WriteLine("Othernode not working. AB.otherNode(B) should be A.");

            //check that throws expecption with bad input
        }
        
        [TestMethod, ExpectedException(typeof(Exception), "otherNode should throw an exception when a node is passed in that isn't one of the endpoints of that edge.")]
        public void InappropriateNodePassedToOtherNode()
        {
            Node a = new Node();
            Node b = new Node();
            Node c = new Node();
            Edge ab = new Edge(a, b, 20);
            ab.otherNode(c);
        }


        [TestMethod]
        public void CommonNodeTest()
        {
            Node a = new Node();
            Node b = new Node();
            Edge ab = new Edge(a, b, 20);

            Node c = new Node();
            Edge ac = new Edge(a, c, 10);

            Assert.AreEqual(a, ab.commonNode(ac), "CommonNode function not returning A as the common node between Edge AB and Edge AC.");
        }

        [TestMethod, ExpectedException(typeof(Exception), "commonNode should throw an exception when two edges are passed in that don't have a common node.")]
        public void InappropriateEdgesPassedToCommonNode()
        {
            Node a = new Node();
            Node b = new Node();
            Node c = new Node();
            Edge ab = new Edge(a, b, 20);
            ab.otherNode(c);
        }

        [TestMethod]
        public void AddEdgeToNodesListOfEdgesTest()
        {
            Node a = new Node();
            Node b = new Node();
            Edge ab = new Edge(a, b, 20);
            if (!a.Edges.Contains(ab)) Console.WriteLine("Edge AB not added to A.Edges.");
            if (!b.Edges.Contains(ab)) Console.WriteLine("Edge AB not added to B.Edges.");
            Node c = new Node();
            Edge ac = new Edge(a, c, 10);
            if (!a.Edges.Contains(ac)) Console.WriteLine("Edge AC not added to A.Edges.");
            if (!a.Edges.Contains(ab)) Console.WriteLine("Edge AB no longer in A.Edges after AC added.");
        }
        [TestMethod]
        public void UndirectedEdgeTest()
        {
            Node a = new Node();
            Node b = new Node();
            Edge ab = new Edge(a, b, 20);
            Edge ba = new Edge(b, a, 20);

            Assert.AreEqual(ab, ba, "Edges should be undirected.");
        }

        [TestMethod]
        public void ToStringTest()
        {
            Node a = new Node();
            Node b = new Node();
            Edge ab = new Edge(a, b, 20);
            String expectedString = "Edge: [" + ab.N1.ToString() + " - " + ab.N2.ToString() + "]";
            Assert.AreEqual(expectedString, ab.ToString(), "ToString() not outputting expected result.");
        }

        [TestMethod]
        public void ContainsNodeTest()
        {
            Node a = new Node();
            Node b = new Node();
            Edge ab = new Edge(a, b, 20);

            Assert.AreEqual(true, ab.containsNode(a), "Edge with endpoints A and B should contain A.");
            Assert.AreEqual(true, ab.containsNode(b), "Edge with endpoints A and B should contain B.");
        }
    }
}
