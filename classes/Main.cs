using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathFinding;
using System.Xml;

namespace run_ns { 
    class run
    {

        static int Main(string[] args)
        {
            string filePath = "";

            if (args.Length == 0)
            {
                filePath = askPath();
            }
            else
            {
                filePath = args[0];
            }

            Graph my_graph = convert(filePath);

            if (my_graph.Nodes.Count != 0 && my_graph.Edges.Count != 0)
            {
                getNewCommands(my_graph, filePath);
            }              

            return 0;
        }

        static string askPath()
        {
            Console.WriteLine("Please input the path to your map");
            return Console.In.ReadLine();
        }

        static Graph convert(string filePath)
        {
            Graph result = new Graph();
            try
            {
                result = Converter.convert(filePath);
                SVGcreator.drawGraph("graph.svg", filePath, result);
                Console.WriteLine("Your map has been converted. The resulting graph is in graph.svg");
            }
            catch
            {
                Console.WriteLine("Error converting the map into a graph");
                string response = "";
                do
                {
                    Console.WriteLine("Try again? Y/N");
                    response = Console.In.ReadLine();
                    if (response == "Y" || response == "y")
                    {
                        filePath = askPath();
                        result = convert(filePath);
                    }
                }
                while (response != "Y" && response != "N" && response != "y" && response != "n");
            }

            return result;
        }

        static void getNewCommands(Graph my_graph, string filePath)
        {
            string response = "";
            do
            {
                Console.WriteLine("Do you want to do pathfinding with this map? Y/N");
                response = Console.In.ReadLine();
                if (response == "Y" || response == "y")
                {
                    int end_office = askOfficeNumber();
                    search(end_office, my_graph, filePath);
                }
            }
            while (response != "Y" && response != "N" && response != "y" && response != "n");
        }

        static int askOfficeNumber()
        {
            try
            {
                Console.WriteLine("Please input the destination room number");
                return Convert.ToInt32(Console.In.ReadLine());
            }
            catch
            {
                askOfficeNumber();
            }

            return 0;
        }

        static void search(int end_office, Graph my_graph, string filePath)
        {

            int start_office = 0;

            try
            {
                Node start_node = my_graph.findNodeByOfficeNumber(start_office);
                Node end_node = my_graph.findNodeByOfficeNumber(end_office);
                MinCostPathFinder pathfinder = new MinCostPathFinder();
                Path shortest_path = pathfinder.findPath(start_node, end_node);
                SVGcreator.drawPath("path.svg", filePath, shortest_path);
                Console.WriteLine("Your path has been calculated. The resulting path is in path.svg");
            }
            catch
            {
                Console.WriteLine("The path cannot be calculated");
            }

            getNewCommands(my_graph, filePath);
        }
    }

}