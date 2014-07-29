using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    /**
     * The Node object should be used to mark a corner or an office. Basically any point
     * at which the PathFinding might need to make a decision when finding the shortest path from
     * one office to another.
     */
    public class Node: Object
    {
        //Office location property (room number of office)
        private int officeLocation;
        public int OfficeLocation
        {
            get { return officeLocation; }
            set { officeLocation = value; }
        }

        //List of edges that include this node as one of the end points.
        private List<Edge> edges;
        public List<Edge> Edges
        {
            get { return edges; }
        }

        //Point that determines the coordinate location of the office in the map.
        private Point crossingPoint;
        public Point CrossingPoint
        {
            get { return crossingPoint; }
            set { crossingPoint = value; }
        }

        /**
         * Constructor. When no officeLocation or crossing point is specified, sets office location
         * to -1 and crossing point to a new Point with x = 0, y=0. An office location of -1
         * indicates that there is no office at this node. Only a corner.
         */
        public Node()
        {
            officeLocation = -1; //An office location of -1 indicates that this is just a corner. Not an office.
            edges = new List<Edge>();
            crossingPoint = new Point(0,0);
        }

        /**
         * Returns a Node instance with the given office location, and x and y coordinates. An office
         * location of -1 indicates that there is no office at this node. Only a corner.
         * 
         * @param officeLocation the room number of the office. -1 if there is no office at this node.
         * @param x the x-coordinate of the node on the map.
         * @param y the y-coordiante of the node of the map.
         */
        public Node(int officeLocation, float x, float y)
        {
            this.OfficeLocation = officeLocation;
            edges = new List<Edge>();
            crossingPoint = new Point(x, y);
        }

        /**
         * Returns a Node instance with the given office location, and x and y coordinates. An office
         * location of -1 indicates that there is no office at this node. Only a corner.
         * 
         * @param officeLocation the room number of the office. -1 if there is no office at this node.
         * @param crossingPoint the point that determines where the node is on the map.
         */
        public Node(int officeLocation, Point crossingPoint)
        {
            this.OfficeLocation = officeLocation;
            edges = new List<Edge>();
            this.crossingPoint = crossingPoint;
        }

        /**
         * Adds Edge e to the list of edges that include this Node.
         * 
         * @param e is the Edge that includes this node as one of its endpoints.
         */
        public void addEdge(Edge e)
        {
            //TODO: Check if this node is one of e's endpoints. If not, throw an exception.
            edges.Add(e);
        }

        /**
         * Overrides the ToString() function. Mostly for testing purposes, Node.ToString()
         * returns the office location.
         * 
         * @return the string representation of the office location.
         */
        public override string ToString()
        {
            //Should this also include the crossing point? Might be too cluttered if try to include everything.
            return this.OfficeLocation.ToString();
        }

        /**
         * Overrides the Equals function. For a node to equal another node, both the office location
         * and the crossing point need to be equal.
         */
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Node n = (Node)obj;
            return (this.CrossingPoint.Equals(n.CrossingPoint)) && (this.OfficeLocation.Equals(n.OfficeLocation));
        }

        /**
         * Returns true if the crossing points of two nodes are closer than epsilon
         * 
         * @param node the node to compare to
         * @param epsilon the acceptable error
         */
        public bool isCloseTo(Node node, double epsilon)
        {
            if (node == null)
                return false;
            return (this.CrossingPoint.isCloseTo(node.CrossingPoint, epsilon)) && (this.OfficeLocation == node.OfficeLocation);
        }
    }
}