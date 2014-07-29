using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace PathFinding
{
    /*
     * For now, this class supports only svg files created by Microsoft Visio, in that specific format.
     * The class is not generic and does not support SVG maps created in any other way, 
     * than specified in Windows on Fridges project documentation
     */
    public class Converter
    {
        private double epsilon;

        private Graph graph;
        public Graph Graph
        {
            get { return graph; }
        }

        private List<Line> lines;
        private double scale;

        public double Scale
        {
            get { return scale; }
            set { scale = value;  }
        }

        public Converter()
        {
            graph = new Graph();
            lines = new List<Line>();
        }

        /*
         * Converts an SVG map into a graph
         * Each crossing point becomes a node. Each line between two crossing points becomes an edge.
         * @param filePath the path to the file containing the map
         * @param scale scale of the map in coordinates/units 
         * (ex. if a line started at (0,0) and ended at (5,5) and had an actual length of 3 inches, 
         * its scale would be 5/3 coordinates/inch)
         * @param epsilon the amount of error that is permittable for two points to be considered equal
         * (two points that are closer than epsilon will be considered equal)
         * 
         * @return resulting graph
         */
        public static Graph convert(string path_to_map, double scale, double epsilon)
        {
            
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            XmlReader reader = XmlReader.Create(path_to_map, settings);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            Converter converter = new Converter();
            converter.Scale = scale;
            converter.epsilon = epsilon;
            converter.transferData(doc);
            converter.generateEdges();
            reader.Close();           
            return converter.graph;
        }

        private void transferData(XmlDocument doc)
        {

            XmlNode main = doc.LastChild;
            XmlNodeList main_nodes = main.ChildNodes;
            XmlElement g_tag = (XmlElement)main_nodes[5];
            XmlNodeList g_tags = g_tag.GetElementsByTagName("g");
            
            processGTags(g_tags);

        }

        private void processGTags(XmlNodeList g_tags)
        {

            for (int i = 0; i < g_tags.Count; i++)
            {
                XmlElement g_tag =(XmlElement) g_tags[i];

                string transform = "";
                if (g_tag.HasAttribute("transform"))
                {
                    transform = g_tag.GetAttribute("transform");
                }

                int officeNumber = -1;
                XmlNodeList v_tags = g_tag.GetElementsByTagName("v:cp");
                if( v_tags.Count != 0)
                {
                    XmlElement v_tag = (XmlElement)v_tags[0];
                    officeNumber = extractNumberFromParentheses(v_tag.GetAttribute("v:val"));
                }

                XmlNodeList path_tags = g_tag.GetElementsByTagName("path");
                XmlElement path = (XmlElement)path_tags[0];                                  
                string path_data = path.GetAttribute("d");
                processPath(path_data, transform, officeNumber);
               
            }
        }



        private void processPath(string path, string transformationData, int officeNumber)
        {
            string[] coordinates = path.Split(' ');

            if (coordinates.Length <= 1)
            {
                throw new Exception();
            }

            Point current_pt = new Point(getCoordinateFromString(coordinates[0]), getCoordinateFromString(coordinates[1]));
            transform(transformationData, current_pt);


            for (int i = 2; i < coordinates.Length; i += 2)
            {
                Point end_pt = new Point(getCoordinateFromString(coordinates[i]), getCoordinateFromString(coordinates[i + 1]));
                transform(transformationData, end_pt);

                if (!startNewLine(coordinates[i]))
                { 

                    Line curr_line = new Line(current_pt, end_pt, officeNumber);
                    

                    for (int j = 0; j < lines.Count; j++ )
                    {
                        Point crossing_pt = curr_line.crosses(lines[j], epsilon);
                        if (!string.IsNullOrEmpty(Convert.ToString(crossing_pt)))
                        {
                            Node new_node = new Node();
                            new_node.CrossingPoint = crossing_pt;
                            if (curr_line.getOfficeNumber() != -1) 
                            {
                                new_node.OfficeLocation = curr_line.getOfficeNumber();
                            }
                            else if (lines[j].getOfficeNumber() != -1)
                            {
                                new_node.OfficeLocation = lines[j].getOfficeNumber();
                            }
                            graph.addNode(new_node);
                        }
                    }
                    lines.Add(curr_line);
                }
                
                current_pt = end_pt;
            }
        }


        /*
         * Extracts the number enclosed in parentheses from a string
         * @param string word
         * @return extracted number
         */
        public static int extractNumberFromParentheses(string word)
        {
            int result = -1;
            string[] words = word.Split('(', ')');
            if (words.Length >= 2)
            {
                result = Convert.ToInt32(words[1]);
            }

            return result;
            
        }


        /*
         * Applies the transformation to the point, as specified in the "transform" attribute 
         * (!) Only support "translate" and "rotate" transformation
         * @param point point to transform
         * @param transformData contents of the "transform" tag in SVG image
         */
        public static void transform(string transformData, Point point)
        {
            float transform_x = 0;
            float transform_y = 0;
            float rotation = 0;

            string[] words = transformData.Split(' ', ')', '(', ',');

            if (words.Count() >= 3 && words[0] == "translate")
            {
                transform_x = ((float)Convert.ToDouble(words[1]));
                transform_y = ((float)Convert.ToDouble(words[2]));
            }
            if (words.Count() >= 7 && words[4] == "rotate")
            {
                rotation = (float)Convert.ToDouble(words[5]);    
            }

            point.transform(transform_x, transform_y, rotation);
        }

        /*
         * Returns true if the string containing a coordinate contains the instruction "Move to" 
         * as specified by SVG conventions
         * @param coordinate_text string containing the coordinates in SVG image [(Letter)?(Coordinate)]
         */
        public static bool startNewLine(string coordinate_text)
        {
            if (coordinate_text.Length == 0)
            {
                return false;
            }
            return (coordinate_text.Substring(0, 1) == "M");
        }

        /*
         * Extracts the coordinate from a string 
         * @param coordinate_text string containing the coordinates in SVG image [(Letter)?(Coordinate)]
         * @return coordinate
         */
        public static float getCoordinateFromString(string coordinate_text)
        {
            if (coordinate_text.Length == 0)
            {
                throw new Exception();
            }

            if (Char.IsLetter(coordinate_text, 0))
            {
                coordinate_text = coordinate_text.Substring(1, coordinate_text.Length - 1);
                if (coordinate_text.Length == 0)
                {
                    throw new Exception();
                }
            }

            return (float)Convert.ToDouble(coordinate_text);

        }

        private void generateEdges()
        {
            for (int i = 0; i < lines.Count; i++)
            {

                SortedNodeContainer container = new SortedNodeContainer(epsilon);
                for (int j = 0; j < graph.Nodes.Count; j++)
                {
                    if (lines[i].contains(graph.Nodes[j].CrossingPoint, epsilon))
                    {
                        container.Add(graph.Nodes[j]);
                    }
                }
                if (container.Count > 1)
                {
                    for (int j = 0; j < container.Count - 1; j++)
                    {
                        graph.addEdge(container[j], container[j + 1], scale);
                    }
                }
            }
        }

        /*
         * The container that stores nodes that belong to the same straigt line on a map in order
         */ 
        public class SortedNodeContainer : List<Node>
        {
            private double epsilon;

            /*
             * creates a new instance of the container
             * @param epsilon the amount of error that is permittable for two points to be considered equal
             * (two points that are closer than epsilon will be considered equal)
             */ 
            public SortedNodeContainer(double epsilon) 
            {
                this.epsilon = epsilon;
            }

            /*
             * Puts a new node to the container accordingly to its position on a line
             * For example, if the container has nodes a and c,
             * and node b is between nodes a and c on the line (a)--(b)--(c),
             * the function will add b to position 1 in the container 
             * [a c] -> [a b c]
             *  0 1      0 1 2
             * @param newNode node to add
             */ 
            public new void Add(Node newNode)
            {
                if (Count <= 1 )
                {
                    base.Add(newNode);
                }
                else
                {
                    int first_ind = 0;
                    int last_ind = Count - 1;

                    //check if the node should be the firts or the last one in the container
                    if (!(new Line(this[first_ind].CrossingPoint, this[last_ind].CrossingPoint).contains(newNode.CrossingPoint, epsilon)))
                    {
                        if (new Line(newNode.CrossingPoint, this[last_ind].CrossingPoint).contains(this[first_ind].CrossingPoint, epsilon))
                            Insert(0, newNode);
                        else
                            base.Add(newNode);
                    }
                    else
                    {
                       //do binary serach to find the place for the node
                        while (last_ind - first_ind > 1)
                        {
                            int mid_ind = first_ind + ((last_ind - first_ind) / 2);
                            if (new Line(this[first_ind].CrossingPoint, this[mid_ind].CrossingPoint).contains(newNode.CrossingPoint, epsilon))
                            {
                                last_ind = mid_ind;
                            }
                            else
                            {
                                first_ind = mid_ind;
                            }

                        }
                        Insert(last_ind, newNode);
                    }
                    
                }
               
            }
        }

        

    }


}
