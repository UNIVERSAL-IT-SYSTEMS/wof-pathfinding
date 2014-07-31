using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PathFinding;

namespace calcTest
{
    [TestClass]
    public class converterTests
    {
     

        [TestMethod]
        public void testStartNewLine()
        {
            Assert.AreEqual<bool>(true, Converter.startNewLine("M0 345"), 
                "The new line is correctely told to be started");
            Assert.AreEqual<bool>(false, Converter.startNewLine("L85787.55 345"), 
                "The new line is correctely told not to be started");
            Assert.AreEqual<bool>(false, Converter.startNewLine("54567567 888"), 
                "The new line is correctely told not to be started");
        }

        [TestMethod]
        public void testGetCoordinate()
        {
            Assert.AreEqual(10.9, Converter.getCoordinateFromString("M10.9"), 
                0.01, "The positive coordinate is correctely calculated");
            Assert.AreEqual(-33.4, Converter.getCoordinateFromString("-33.4"), 
                0.01, "The negative coordinate is correctely calculated");
        }

        [TestMethod]
        public void testExtract()
        {
            Assert.AreEqual<int>(7050, Converter.extractNumberFromParentheses("VT4(7050)"),
                "The office number is correctely extracted");
        }

        [TestMethod]
        public void testConvertALine()
        {
            
            Graph my_graph = Converter.convert("../../tests/line.svg");
            Assert.AreEqual<int>(0, my_graph.Edges.Count, "resulting graph contains wrong number of edges");
            Assert.AreEqual<int>(0, my_graph.Nodes.Count, "resulting graph contains wrong number of nodes");
            
        }


        [TestMethod]
        public void testConvertWithNoOffices()
        {
            
            Graph my_graph = Converter.convert("../../tests/1node0edges.svg");
            Assert.AreEqual<int>(0, my_graph.Edges.Count, "resulting graph contains wrong number of edges");
            Assert.AreEqual<int>(1, my_graph.Nodes.Count, "resulting graph contains wrong number of nodes");

            my_graph = Converter.convert("../../tests/3node3edges.svg");
            Assert.AreEqual<int>(3, my_graph.Edges.Count, "resulting graph contains wrong number of edges");
            Assert.AreEqual<int>(3, my_graph.Nodes.Count, "resulting graph contains wrong number of nodes");
  
        }


        [TestMethod]
        public void testConvertWithOffices()
        {
            Graph my_graph = Converter.convert("../../tests/5node5edges.svg");
            Assert.AreEqual<int>(5, my_graph.Edges.Count, "resulting graph contains wrong number of edges");
            Assert.AreEqual<int>(5, my_graph.Nodes.Count, "resulting graph contains wrong number of nodes");
            Assert.AreEqual<int>(1, my_graph.findNodeByOfficeNumber(1).OfficeLocation, "resulting graph does not contain the node with office 1");
            Assert.AreEqual<int>(2, my_graph.findNodeByOfficeNumber(2).OfficeLocation, "resulting graph does not contain the node with office 2");
        }


        [TestMethod]
        public void testConvertWithTryinToAddDuplicateNodes()
        {
            Graph my_graph = Converter.convert("../../tests/1nodeWith4EdgesAnd4OfficeNodes.svg");
            Assert.AreEqual<int>(4, my_graph.Edges.Count, "resulting graph contains wrong number of edges");
            Assert.AreEqual<int>(5, my_graph.Nodes.Count, "resulting graph contains wrong number of nodes");
            Assert.AreEqual<int>(1, my_graph.findNodeByOfficeNumber(1).OfficeLocation, "resulting graph does not contain the node with office 1");
            Assert.AreEqual<int>(2, my_graph.findNodeByOfficeNumber(2).OfficeLocation, "resulting graph does not contain the node with office 2");
            Assert.AreEqual<int>(3, my_graph.findNodeByOfficeNumber(3).OfficeLocation, "resulting graph does not contain the node with office 3");
            Assert.AreEqual<int>(4, my_graph.findNodeByOfficeNumber(4).OfficeLocation, "resulting graph does not contain the node with office 4");
            Assert.AreEqual<int>(4, my_graph.findNodeByOfficeNumber(-1).Edges.Count, "resulting graph does not have three edges connected together");
        }


    }
}
