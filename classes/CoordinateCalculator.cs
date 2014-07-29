using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    /*
     * Contains necessary functions that perform calculations with coordinates, points, and lines
     */
    public static class CoordinateCalculator
    {

        /*
         * Return the scale in coordinates/unit based on the line with known length on a map
         * For example, if I know that a room is 5 meters long, 
         * I cam submit its coordinates and get the scale in coordinates/meter
         * 
         * @param start_pt starting point of th line
         * @param end_pt end point of th line
         * @param length length of the line (in units you want to use, e.g. meters, feet, inches)
         * 
         * @return scale in coordinates/unit
         */
        public static double getScale(Point start_pt, Point end_pt, double length)
        {
            if (length == 0)
                return 0;

            double scale = euclideanDistance(start_pt, end_pt) / length;
            return scale;
        }


        /*
         * Calculates the eucledian distance between two points
         * 
         * @param a first point
         * @param b second point
         * 
         * @return distance
         */ 
        public static double euclideanDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        /*
         * Returns true if two points are closer than difference to each other
         * 
         * @param a first point
         * @param b second point
         * @param difference maximal difference
         */
        public static bool hasDifferenceLessThan(float a, float b, double difference)
        {
            return (Math.Abs(a - b) < difference);
        }


        /*
         * Returns true if coord_a <= coord <= coord_b or  coord_a >= coord >= coord_b
         * Two coordinates are assumed to be equal if they are closer than epsilon to each other
         * 
         * @param coord coordinate that is either between two other or not
         * @param coord_a first coordinate
         * @param coord_b second coordinate
         * @param epsilon maximal difference between two coordinates to be considered equal
         */
        public static bool isBetween(float coord, float coord_a, float coord_b, double epsilon)
        {
            return ((coord > coord_a || CoordinateCalculator.hasDifferenceLessThan(coord, coord_a, epsilon))
                    && (coord < coord_b || CoordinateCalculator.hasDifferenceLessThan(coord, coord_b, epsilon)));
        }

    }
}
