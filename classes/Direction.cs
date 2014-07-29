using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    public class Direction
    {
        private Point previousPoint, currentPoint, nextPoint;

        public double angle
        {
            get
            {
                return getDegreeAngle();
            }
        }

        private double _distance;
        public double distance
        {
            get { return _distance; }
        }

        /**
         * Creates a new Direction from the three given points.
         * 
         * @param previousPoint The point the robot was at before.
         * @param currentPoint The point the robot is currently at.
         * @param nextPoint The point the robot is trying to get to next.
         * @param scale Coordinates/Unit of Measurement (ex: coordinates/meter) This is how the Direction
         * can take two points with x and y coordinates and determine how many meters (or other unit of measurement)
         * that represents. You must be consistent with your scale throughout the project or you will be very unhappy.
         */
        public Direction(Point previousPoint, Point currentPoint, Point nextPoint, double scale)
        {
            this.previousPoint = previousPoint;
            this.currentPoint = currentPoint;
            this.nextPoint = nextPoint;
            _distance = CoordinateCalculator.euclideanDistance(currentPoint, nextPoint) / scale;
        }

        /**
         * Creates a new Direction from the three given nodes.
         * 
         * @param previousNode The point the robot was at before.
         * @param currentNode The poNodeint the robot is currently at.
         * @param nextNode The Node the robot is trying to get to next.
         * @param scale Coordinates/Unit of Measurement (ex: coordinates/meter) This is how the Direction
         * can take two Nodes' points with x and y coordinates and determine how many meters (or other unit of measurement)
         * that represents. You must be consistent with your scale throughout the project or you will be very unhappy.
         */
        public Direction(Node previousNode, Node currentNode, Node nextNode, double scale)
        {
            this.previousPoint = previousNode.CrossingPoint;
            this.currentPoint = currentNode.CrossingPoint;
            this.nextPoint = nextNode.CrossingPoint;
            _distance = CoordinateCalculator.euclideanDistance(currentPoint, nextPoint) / scale;
        }

        /**
         * Order of edges matters!
         * 
         * @param edge1 is the edge that the robot just finished traversing. We need this to get current heading.
         * @param edge2 is the edge that the robot wants to travel on next.
         */
        public Direction(Edge edge1, Edge edge2)
        {
            _distance = edge2.Weight;
            Node currentNode = edge1.commonNode(edge2);
            currentPoint = currentNode.CrossingPoint;
            previousPoint = edge1.otherNode(currentNode).CrossingPoint;
            nextPoint = edge2.otherNode(currentNode).CrossingPoint;
        }

        /**
         * Returns the angle (in degrees) the robot needs to turn to continue on the right path.
         * 
         * Negative angle = turn clockwise that many degrees.
         * Positive angle = turn counterclockwise that many degrees.
         */
        private double getDegreeAngle(){
            //what if there is no angle1 (like for the first section of path?)
            if (previousPoint == null) { return 0; }//if there is no previousPoint (like at the start of a path), the robot doesn't have to turn first.
            Vector currentHeading = new Vector(currentPoint.X - previousPoint.X, currentPoint.Y - previousPoint.Y);
            Vector newHeading = new Vector(nextPoint.X - currentPoint.X, nextPoint.Y - currentPoint.Y);
            return currentHeading.degreeAngleTo(newHeading);
        }

        /**
         * Returns the angle (in radians) the robot needs to turn to continue on the right path.
         * 
         * Negative angle = turn clockwise that many degrees.
         * Positive angle = turn counterclockwise that many degrees.
         */
        private double getRadianAngle()
        {
            //what if there is no angle1 (like for the first section of path?)
            if (previousPoint == null) { return 0; }//if there is no previousPoint (like at the start of a path), the robot doesn't have to turn first.
            Vector currentHeading = new Vector(currentPoint.X - previousPoint.X, currentPoint.Y - previousPoint.Y);
            Vector newHeading = new Vector(nextPoint.X - currentPoint.X, nextPoint.Y - currentPoint.Y);
            return -currentHeading.radianAngleTo(newHeading);
        }

        /**
         * Returns the json direction encoding the angle and distance the robot needs to travel to get to the next point.
         */
        public string getJSONDirection()
        {
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(this);
            return jsonString;
        }

        /**
         * Overrides the ToString() function to return the JSON encoding of the direction.
         * This includes both the angle and the direction.
         * 
         * @return a string listing out the nodes that make up the path.
         */
        public override string ToString()
        {
            return getJSONDirection();
        }

        /**
         * Overrides the Equals function.
         * Returns true if the angle and distance values are equal.
         * 
         * @param obj the Object being compared to this instance.
         * @return whether obj has the same angle and distance as this instance.
         */

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Direction d = (Direction)obj;
            return (this.angle.Equals(d.angle) && this.distance.Equals(d.distance));
        }


        /**
         * Returns true if the angles and distances of the two directions are within epsilon of each other.
         * 
         * @param point the point to compare to
         * @param epsilon the acceptable error
         */
        public bool isCloseTo(Direction direction, double epsilon)
        {
            if (direction == null)
                return false;
            return (CoordinateCalculator.hasDifferenceLessThan((float)this.angle, (float)direction.angle, epsilon)
                && CoordinateCalculator.hasDifferenceLessThan((float)this.distance, (float)direction.distance, epsilon));
        }
            


    }
}
