using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace PathFinding
{
    public class Graph 
    {
        
        private Edgelist edges;

        public Edgelist Edges
        {
            get { return edges; }
        }
      
        private Nodelist nodes;

        public Nodelist Nodes
        {
            get { return nodes; }
        }
        

        public Graph() 
        {
            edges = new Edgelist();
            nodes = new Nodelist();
        }


        /*
         * Adds a new node into the graph
         * Does not allow to add duplicate nodes
         * @param new_node node to add
         */
        public void addNode(Node new_node)
        {
            if (!nodes.Contains(new_node))
            {
                nodes.Add(new_node);
            }
        }

        /*
         * Creates new edge between two nodes and adds it into the graph
         * @param n1 first node
         * @param n2 second node
         * @param scale
         * @param scale scale of the map in coordinates/units 
         * (ex. if a line started at (0,0) and ended at (5,5) and had an actual length of 3 inches, 
         * its scale would be 5/3 coordinates/inch)
         */
        public void addEdge(Node n1, Node n2, double scale)
        {
            double weight = CoordinateCalculator.euclideanDistance(n1.CrossingPoint, n2.CrossingPoint)/scale;
            Edge new_edge = new Edge(n1, n2, weight);
            addEdge(new_edge);
        }

        /*
        * Adds a new edge into the graph
        * Does not allow to add duplicate edges
        * @param new_edge edge to add
        */
        public void addEdge(Edge new_edge)
        {
            if (!edges.Contains(new_edge))
            {
                edges.Add(new_edge);
            }
        }
      

        public class Nodelist : List<Node>
        {
            public Nodelist()
            { }

            public new bool Contains(Node node)
            {
                for (int i = 0 ; i < Count; i++)
                {
                    if (node == this[i])
                    {
                        return true;
                    }
                }

                return false;
            }


        }

        public class Edgelist : List<Edge>
        {
            public Edgelist()
            { }

            public new bool Contains(Edge edge)
            {
                for (int i = 0 ; i < Count; i++)
                {
                    if (edge == this[i])
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /*
         * Finds the node containing the specified office number or returns null
         * @number office number
         * @return node containing offie number or null if no nodes contain it
         */

        public Node findNodeByOfficeNumber(int number)
        {
            return nodes.Find(x => (x.OfficeLocation == number));


        }



    }
}
