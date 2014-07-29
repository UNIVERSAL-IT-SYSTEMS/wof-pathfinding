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


    }
}
