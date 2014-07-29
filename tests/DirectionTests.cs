using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PathFinding
{
    [TestClass]
    public class DirectionTests
    {
        
        [TestMethod]
        public void DirectionInitTests()
        {
            //Check initializing with two edges, three points, three nodes
            double scale = 10; //units per meter (or whatever system of measurement you are using)


            double expectedAngle = -90;
            double expectedDistance = 1;
            float x1, x2, x3, y1, y2, y3;
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = (float)(expectedDistance * scale);
            x3 = (float)(expectedDistance * scale);
            y3 = (float)(expectedDistance * scale);

            Point p1 = new Point(x1, y1);
            Point p2 = new Point(x2, y2);
            Point p3 = new Point(x3, y3);
            Direction directionFromPoints = new Direction(p1, p2, p3, scale);
            Assert.AreEqual(expectedAngle, directionFromPoints.angle, "Angle not being calculated correctly when init with points.");
            Assert.AreEqual(expectedDistance, directionFromPoints.distance, "Distance not being calculated correctly when init with points.");

            Node n1 = new Node(7421, x1, y1);
            Node n2 = new Node(7422, x2, y2);
            Node n3 = new Node(7423, x3, y3);
            Direction directionFromNodes = new Direction(n1, n2, n3, scale);
            Assert.AreEqual(expectedAngle, directionFromNodes.angle, "Angle not being calculated correctly when init with nodes.");
            Assert.AreEqual(expectedDistance, directionFromNodes.distance, "Distance not being calculated correctly when init with nodes.");

            Edge e1 = new Edge(n1, n2, 3);
            Edge e2 = new Edge(n2, n3, 3);
            Direction directionFromEdges = new Direction(n1, n2, n3, scale);
            Assert.AreEqual(expectedAngle, directionFromEdges.angle, "Angle not being calculated correctly when init with edges.");
            Assert.AreEqual(expectedDistance, directionFromEdges.distance, "Distance not being calculated correctly when init with edges.");
        }

        [TestMethod]
        public void GetAngleTest()
        {
            double scale = 10;

            Point p1 = new Point(0, -1);
            Point p2 = new Point(0, 0);
            Point p3 = new Point(1, 1);
            double expectedAngle = -45;

            Vector currentHeading = new Vector(p2.X - p1.X, p2.Y - p1.Y);
            Vector newHeading = new Vector(p3.X - p2.X, p3.Y - p2.Y);
            Assert.AreEqual(0, currentHeading.X, "Current heading x not correct.");
            Assert.AreEqual(1, currentHeading.Y, "Current heading y not correct.");
            Assert.AreEqual(1, newHeading.X, "New heading x not correct.");
            Assert.AreEqual(1, newHeading.Y, "New heading y not correct.");


            Direction directionFromPoints = new Direction(p1, p2, p3, scale);
            Assert.AreEqual(expectedAngle, directionFromPoints.angle, "Angle 1 not being calculated correctly.");
            
            Point p4 = new Point(0, 1);
            expectedAngle = 135;
            directionFromPoints = new Direction(p2, p3, p4, scale);
            Assert.AreEqual(expectedAngle, directionFromPoints.angle, "Angle 2 not being calculated correctly.");

            Point p5 = new Point(1, 1);
            expectedAngle = -180;
            directionFromPoints = new Direction(p3, p4, p5, scale);
            Assert.AreEqual(expectedAngle, directionFromPoints.angle, "Angle 3 not being calculated correctly.");
        }

        [TestMethod]
        public void DistanceTest()
        {
            double scale = 10;
            Point p1 = new Point(0, -1);
            Point p2 = new Point(0, 0);
            Point p3 = new Point(1, 1);
            double expectedDistance = Math.Sqrt(2)/scale;
            Direction directionFromPoints = new Direction(p1, p2, p3, scale);
            Assert.AreEqual(expectedDistance, directionFromPoints.distance, "Distance 1 not being calculated correctly.");
            
            Point p4 = new Point(0, 1);
            expectedDistance = 1/scale;
            directionFromPoints = new Direction(p2, p3, p4, scale);
            Assert.AreEqual(expectedDistance, directionFromPoints.distance, "Distance 2 not being calculated correctly.");

            Point p5 = new Point(1, 1);
            expectedDistance = 1/scale;
            directionFromPoints = new Direction(p3, p4, p5, scale);
            Assert.AreEqual(expectedDistance, directionFromPoints.distance, "Distance 3 not being calculated correctly.");
        }

        [TestMethod]
        public void DistanceJSONSerializationTest()
        {
            double scale = 10;

            Point p1 = new Point(0, -1);
            Point p2 = new Point(0, 0);
            Point p3 = new Point(1, 0);
            Direction direction = new Direction(p1, p2, p3, scale);

            String expectedJSON = "{\"angle\":" + direction.angle +",\"distance\":" + direction.distance+ "}";
            String jsonDirection = direction.getJSONDirection();
            Assert.AreEqual(expectedJSON, jsonDirection, "Distance not being serialized to JSON correctly.");
        }

        [TestMethod]
        public void DirectionEqualsOverrideTest()
        {
            double scale = 10;

            Point p11 = new Point(0, -1);
            Point p12 = new Point(0, 0);
            Point p13 = new Point(1, 0);
            Direction direction1 = new Direction(p11, p12, p13, scale);

            Point p21 = new Point(0, -10); //different first point, but still same angle and distance
            Point p22 = new Point(0, 0);
            Point p23 = new Point(1, 0);
            Direction direction2 = new Direction(p21, p22, p23, scale);

            Point p31 = new Point(-1, -1); //different first point, causing different angle
            Point p32 = new Point(0, 0);
            Point p33 = new Point(1, 0);
            Direction direction3 = new Direction(p31, p32, p33, scale);

            Point p41 = new Point(0, -1); 
            Point p42 = new Point(0, 0);
            Point p43 = new Point(2, 0); //different last point, causing different distance
            Direction direction4 = new Direction(p41, p42, p43, scale);

            Assert.AreEqual(direction1, direction2, "Directions with the same angle and distance should be considered equal.");
            Assert.AreNotEqual(direction1, direction3, "Directions with different angles should not be considered equal.");
            Assert.AreNotEqual(direction1, direction4, "Directions with different distances should not be considered equal.");
        }
        
    }
}
