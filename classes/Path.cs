using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PathFinding
{
    /**
     * Keeps track of the list of nodes that make up the path and
     * the total cost of traversing the entire path in order.
     */
    public class Path : IComparable<Path>
    {
        //Total cost of traversing entire path.
        private double cost;
        public double Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        //Current last node in the path.
        private Node lastNode;
        public Node LastNode
        {
            get { return lastNode; }
            set { lastNode = value; }
        }

        //List of nodes that make up the path.
        private LinkedList<Node> listOfNodes;
        public LinkedList<Node> ListOfNodes
        {
            get { return listOfNodes; }
            set { listOfNodes = value; }
        }

        /**
         * Constructor. Returns an instance of a new path that contains only Node n.
         * Since there is only one node, there is no cost to traverse the path from
         * beginning to end.
         *  
         * @param n the starting node of the new path.
         */
        public Path(Node n)
        {
            this.Cost = 0;
            this.LastNode = n;
            listOfNodes = new LinkedList<Node>();
            listOfNodes.AddLast(n);
        }

        /**
         * Constructor. Returns a new instance of a Path the same as Path p.
         * This creates a copy of Path p that can be passed around independent
         * of p, not a reference to Path p.
         * 
         * @param p the Path that will be coppied.
         */
        public Path(Path p)
        {
            this.Cost = p.Cost;
            this.LastNode = p.LastNode;
            listOfNodes = new LinkedList<Node>(p.ListOfNodes);
        }


        /**
         * Returns a list of Directions. The angle of the first direction is always zero, because if there is no
         * previous point or node passed in, the robot assumes that it is starting off facing the
         * correct direction.
         * 
         * @param scale The coordinates/unit of measurement as determined by the map. 
         * @return A LinkedList of Direction objects specifying the correct order of direction.
         */
        public LinkedList<Direction> getListOfDirections(double scale)
        {
            LinkedList<Direction> listOfDirections = new LinkedList<Direction>();
            Node previousNode = null;

            LinkedListNode<Node> currentLinkedListNode = listOfNodes.First;
            while(!currentLinkedListNode.Equals(listOfNodes.Last))
            {
                LinkedListNode<Node> nextLinkedListNode = currentLinkedListNode.Next;
                Node currentNode = currentLinkedListNode.Value;
                Node nextNode = nextLinkedListNode.Value;
                listOfDirections.AddLast(new Direction(previousNode, currentNode, nextNode, scale));
                previousNode = currentNode;
                currentLinkedListNode = nextLinkedListNode;
            }
            return listOfDirections;
        }

        /**
         * Adds an edge to the Path. The reason we add an Edge
         * instead of a Node is because we need to ensure that the
         * node being added is connected to the previous lastNode in the
         * path. We also need to update the cost of the path and lastNode.
         * 
         * @param e the Edge being added to the path.
         */
        public void addEdgeToPath(Edge e)
        {
            Node newLastNode = e.otherNode(this.LastNode);
            //TODO: Make sure that otherNode doesn't throw an exception. Check to make sure e.containsNode(newLastNode) == true.
            this.Cost = this.Cost + e.Weight;
            this.LastNode = newLastNode;
            listOfNodes.AddLast(newLastNode);
        }

        /**
         * Implements CompareTo function as required by IComparable interface.
         * This allows a list of paths to be sorted based on cost. The path
         * with the lower cost is less than the path with the higher cost.
         * 
         * @param p the Path this path is being compared to.
         * @return a number less than 0 if this path's cost is less than p's cost,
         *         a number greater than 0 if this path's cost is pubgreater than p's cost,
         *         or 0 if the costs are the same.
         */
        public int CompareTo(Path p)
        {
            return this.Cost.CompareTo(p.Cost);
        }

        /**
         * Overrides the Equals functions and tests for value equality.
         * Returns true if obj is also a Path with the same nodes in the same order, with the same total cost.
         * 
         * @param obj the Object being compared to this instance.
         * @return whether obj represents the same path as this instance.
         */
        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Path p = (Path)obj;
            if (!this.cost.Equals(p.cost)) return false;
            LinkedListNode<Node> pCurrentLinkedListNode = p.ListOfNodes.First;
            foreach (Node n in this.listOfNodes)
            {
                if (!n.Equals(pCurrentLinkedListNode.Value))
                {
                    return false;
                }
                pCurrentLinkedListNode = pCurrentLinkedListNode.Next;
            }
            return true;
        }

        /**
         * Overrides the ToString() function to return a list of the nodes the path contains.
         * 
         * @return a string listing out the nodes that make up the path.
         */
        public override string ToString()
        {
            String s = "Path: <";
            foreach (Node n in listOfNodes)
            {
                s += " (" + n.ToString() + ")";
            }
            s += " >";
            return s;
        }

        /**
         * Returns the JSON representation of the list of directions.
         */
        public string getJSONDirections(double scale)
        {
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(this.getListOfDirections(scale));
            jsonString = "{\"commandList\":" + jsonString + "}";
            return jsonString;
        }


    }
}
