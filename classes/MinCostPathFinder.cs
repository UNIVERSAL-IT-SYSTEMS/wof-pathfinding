using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    public class MinCostPathFinder
    {
        //The list of paths we could explore.
        private List<Path> paths;
        
        /*
         * The list of nodes we have already explored. This means we have already
         * found the shortest path to them so any path that leads to one of the
         * finished nodes is longer than it needs to be (and therefore couldn't be
         * the shortest path to whatever node we are going toward).
         */
        private List<Node> finishedNodes;

        private Node startNode;
        private Node endNode;

        /**
         * Initializes a MinCostPathFinder instance.
         */
        public MinCostPathFinder()
        {
            refreshPathFinder();
        }

        /**
         * Resets paths and finishedNodes so a fresh search can be run.
         */
        private void refreshPathFinder()
        {
            paths = new List<Path>();
            finishedNodes = new List<Node>();
        }

        /**
         * Returns the shortest path from startNode to endNode.
         * 
         * @param startNode is the node from which the path-finding search will begin.
         * @param endNode is the node the path-finding search is trying to find a path to.
         * @return returns the shortest path from startNode to endNode.
         */
        public Path findPath(Node startNode, Node endNode)
        {
            refreshPathFinder();
            this.startNode = startNode;
            this.endNode = endNode;
            Path initialPath = new Path(startNode);
            paths.Add(initialPath);
            return explorePath(initialPath);
        }

        /**
         * Finds the shortest path starting with Path p. If path p already leads to
         * end node, then we have found the shortest path, so return that. Otherwise, it
         * recursively explores the shortest path in paths until it finds a path that
         * ends with endNode.
         * 
         * @param p the path we are exploring. Actually, last node of path p is what we are exploring.
         *          Path p is the shortest path in paths (the list of paths we know of) so we want to explore outward
         *          from there to garuantee that the path to endNode that we return at the end is really the shortest path.
         * @return returns the shortest path to endNode, starting at p.
         * */
        private Path explorePath(Path p)
        {
            Node lastNode = p.LastNode;
            if (lastNode.OfficeLocation == endNode.OfficeLocation) return p;
            foreach (Edge e in lastNode.Edges)
            {
                if (!finishedNodes.Contains(e.otherNode(lastNode))) //if a shorter path to that node has not already been found...
                {
                    addEdgeToPath(e, p);
                }
            }
            paths.Remove(p);
            finishedNodes.Add(lastNode);
            return explorePath(nextLowestPath());
        }

        /**
         * Returns the current shortest path in paths.
         * 
         * @return returns the current shorteset path in paths.
         */
        private Path nextLowestPath()
        {
            paths.Sort();
            return paths[0];
        }

        /**
         * Creates a new path from Path p and adds Edge e added to the newly created path.
         * If the newly created path ends in the same node as another path in paths, it either
         * replaces that path (if the new path has a lower cost) or doesn't get added to paths (if
         * the new path has a higher cost). Otherwise, the path simply gets added to paths.
         * 
         * @param e is the Edge we are adding to the new path
         * @param p is the Path that the new path is copying. We don't just add e to p since that would
         *        affect the p in explorePath function, which might not be finished.
         */
        private void addEdgeToPath(Edge e, Path p)
        {
            Node newLastNode = e.otherNode(p.LastNode);
            double newCost = p.Cost + e.Weight;
            Path newPath = new Path(p);
            newPath.addEdgeToPath(e);
            //Check if path to node already exists
            foreach (Path oldPath in paths)
            {
                if (oldPath.LastNode == newLastNode)
                {
                    //there is already a path in paths that leads to newLastNode
                    if (oldPath.Cost > newCost)
                    {
                        //if new path is shorter, remove old path and add new one.
                        paths.Add(newPath);
                        paths.Remove(oldPath);
                        return; //there will be at most one path lastNode == newLastNode so we can stop looking

                    }
                    return;  //if there is already a shorter path that leads to this node, don't do anything.
                }
            }
            //there isn't a path that leads to newLastNode in paths yet, so add newPath
            paths.Add(newPath);
            return;
        }
    }
}
