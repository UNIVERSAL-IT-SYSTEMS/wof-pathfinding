using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace PathFinding
{

   /*
    * OPTIONAL:
    * this class is for testing the results of the serach. It graphically draws the path or the graph on a map
    */
    class SVGcreator
    {

        /*
         * Draws the path on an SVG map and stores the new map in the destination_file
         * @param destination_file path to store the resulting image
         * @param original_map path to map to draw upon
         * @param shortest_path path to draw
         */
        public static void drawPath(string destination_file, string original_map, Path shortest_path)
        {
            if (shortest_path.ListOfNodes.Count > 0)
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Ignore;
                XmlReader reader = XmlReader.Create(original_map, settings);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);

                XmlNodeList g_tags = doc.GetElementsByTagName("g");
                if (g_tags.Count == 0)
                    throw new Exception("SVG map in the wrong format");
                XmlElement g_tag = (XmlElement)g_tags[0];

                string path = "M" + shortest_path.ListOfNodes.ElementAt(0).CrossingPoint.X +
                    " " + shortest_path.ListOfNodes.ElementAt(0).CrossingPoint.Y;

                for (int i = 1; i < shortest_path.ListOfNodes.Count; i++)
                {
                    path += " L" + shortest_path.ListOfNodes.ElementAt(i).CrossingPoint.X
                        + " " + shortest_path.ListOfNodes.ElementAt(i).CrossingPoint.Y;
                }


                XmlElement new_elem = doc.CreateElement("g", "http://www.w3.org/2000/svg");
                XmlElement path_tag = doc.CreateElement("path", "http://www.w3.org/2000/svg");
                path_tag.SetAttribute("d", path);
                path_tag.SetAttribute("style", "stroke:red");
                new_elem.AppendChild(path_tag);
                g_tag.AppendChild(new_elem);

                doc.Save(destination_file);
                reader.Close();
            }
        }

        /*
        * Draws the nodes on an SVG map and stores the new map in the destination_file
        * @param destination_file path to store the resulting image
        * @param original_map path to map to draw upon
        * @param my_graph graph containing nodes to draw
        */
        public static void drawNodes(string destination_file, string original_map, Graph my_graph)
        {
            XmlDocument doc = getDocument(original_map);

            addNodesToSVG(doc, my_graph);

            doc.Save(destination_file);
        }

        private static XmlDocument getDocument(string path)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            XmlReader reader = XmlReader.Create(path, settings);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            reader.Close();

            return doc;
        }

        private static void addNodesToSVG(XmlDocument doc, Graph my_graph)
        {
            XmlNodeList g_tags = doc.GetElementsByTagName("g");
            if (g_tags.Count == 0)
                throw new Exception("SVG map in the wrong format");
            XmlElement g_tag = (XmlElement)g_tags[0];

            for (int i = 0; i < my_graph.Nodes.Count; i++)
            {
                XmlElement new_elem = doc.CreateElement("g", "http://www.w3.org/2000/svg");
                XmlElement rect = doc.CreateElement("rect", "http://www.w3.org/2000/svg");
                rect.SetAttribute("x", Convert.ToString(my_graph.Nodes[i].CrossingPoint.X - 2));
                rect.SetAttribute("y", Convert.ToString(my_graph.Nodes[i].CrossingPoint.Y - 2));
                rect.SetAttribute("width", "5");
                rect.SetAttribute("height", "5");
                rect.SetAttribute("style", "fill:red");
                new_elem.AppendChild(rect);
                g_tag.AppendChild(new_elem);
            }
        }

        /*
        * Draws the nodes and edges on an SVG map and stores the new map in the destination_file
        * @param destination_file path to store the resulting image
        * @param original_map path to map to draw upon
        * @param my_graph graph containing nodes and edges to draw
        */
        public static void drawGraph(string destination_file, string original_map, Graph my_graph)
        {
            XmlDocument doc = getDocument(original_map);

            addEdgesToSVG(doc, my_graph);
            addNodesToSVG(doc, my_graph);

            doc.Save(destination_file);
        }

        /*
        * Draws the edges on an SVG map and stores the new map in the destination_file
        * @param destination_file path to store the resulting image
        * @param original_map path to map to draw upon
        * @param my_graph graph containing edges to draw
        */
        public static void drawEdges(string destination_file, string original_map, Graph my_graph)
        {

            XmlDocument doc = getDocument(original_map);

            addEdgesToSVG(doc, my_graph);

            doc.Save(destination_file);
        }

        private static void addEdgesToSVG(XmlDocument doc, Graph my_graph)
        {
            XmlNodeList g_tags = doc.GetElementsByTagName("g");
            if (g_tags.Count == 0)
                throw new Exception("SVG map in the wrong format");
            XmlElement g_tag = (XmlElement)g_tags[0];

            for (int i = 0; i < my_graph.Edges.Count; i++)
            {
                string path = "M" + my_graph.Edges[i].N1.CrossingPoint.X +
                " " + my_graph.Edges[i].N1.CrossingPoint.Y
                + " L" + my_graph.Edges[i].N2.CrossingPoint.X
                + " " + my_graph.Edges[i].N2.CrossingPoint.Y;

                XmlElement new_elem = doc.CreateElement("g", "http://www.w3.org/2000/svg");
                XmlElement path_tag = doc.CreateElement("path", "http://www.w3.org/2000/svg");
                path_tag.SetAttribute("d", path);
                path_tag.SetAttribute("style", "stroke:blue");
                new_elem.AppendChild(path_tag);
                g_tag.AppendChild(new_elem);

            }
        }
      
    }
}
