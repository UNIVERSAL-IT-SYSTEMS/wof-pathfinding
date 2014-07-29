using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PathFinding
{
    public class Edge
    {
        //Endpoints of the edge.
        private Node n1, n2;

        public Node N1
        {
            get { return n1; }
        }
        public Node N2
        {
            get { return n2; }
        }

        //Cost of traversing the edge.
        private double weight;
        public double Weight
        {
            get { return weight; }
        }

        public Edge()
        {
            this.weight = 0;
        }

        /**
         * Creates an edge with endpoints n1 and n2, and the given weight.
         * Adds this edge to the list of edges that branch off of n1, and n2.
         * Order of n1 and n2 doesn't matter for Equivalence tests.
         * 
         * @param n1 is one of the endpoints.
         */
        public Edge(Node n1, Node n2, double weight)
        {
            this.n1 = n1;
            this.n1.addEdge(this);
            this.n2 = n2;
            this.n2.addEdge(this);
            this.weight = weight;
        }

        /**
         * Checks if n is one of the endpoints of this edge.
         * 
         * @param n the Node we are checking
         * @return whether n is an endpoint.
         */
        public bool containsNode(Node n)
        {
            if ((n == this.N1) || (n == this.N2)) 
                return true;
            return false;
        }

        /**
         * If n is an endpoint of this edge, returns the other endpoint.
         * Otherwise, throws an expception.
         * 
         * @param n one endpoint of this edge.
         * @return the other endpoint.
         */
        public Node otherNode(Node n)
        {
            if (n == this.N1)
            {
                return this.N2;
            }
            if (n == this.N2)
            {
                return this.N1;
            }
            throw new Exception(this + " does not contain Node " + n + ".");
        }

        /**
         * Returns the endpoint that this edge and the given edge have in common. Throws an exception if no edge in common.
         * 
         * @param e the edge getting compared to.
         * @return the endpiont that this edge and the given edge have in common. Or throws an exception if there is no endpoint in common.
         */
        public Node commonNode(Edge e)
        {
            if (this.containsNode(e.N1))
            {
                return e.N1;
            }
            if (this.containsNode(e.N2))
            {
                return e.N2;
            }
            throw new Exception("Edges to not connect.");
        }

        /**
         * Overrides the ToString() function to list the two endpoints of the edge.
         * 
         * @return a string noting the two endpoints of the edge.
         */
        public override string ToString()
        {
            return "Edge: [" + this.N1.ToString() + " - " + this.N2.ToString() + "]";
        }

        /**
         * Overrides the Equals function. For one edge to equal another, their endpoints have to match.
         */
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Edge e = (Edge)obj;
            bool sameEndpoints = (this.N1.Equals(e.N1)) && (this.N2.Equals(e.N2)) || (this.N2.Equals(e.N1)) && (this.N1.Equals(e.N2));
            bool sameWeight = this.Weight.Equals(e.Weight);
            return sameEndpoints && sameWeight;
        }

        /**
        * Returns true if the crossing points of two nodes are closer than epsilon
        * 
        * @param node the node to compare to
        * @param epsilon the acceptable error
        */
        public bool isCloseTo(Edge e, double epsilon)
        {
            if (e == null)
                return false;
            bool closeEndpoints = (this.N1.isCloseTo(e.N1, epsilon)) && (this.N2.isCloseTo(e.N2, epsilon)) || (this.N2.isCloseTo(e.N1, epsilon)) && (this.N1.isCloseTo(e.N2, epsilon));
            bool closeWeight = CoordinateCalculator.hasDifferenceLessThan((float)this.Weight, (float)e.Weight, epsilon);
            return closeEndpoints && closeWeight;
        }
    }
}
