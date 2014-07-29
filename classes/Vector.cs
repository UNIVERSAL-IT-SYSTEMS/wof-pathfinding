using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    public class Vector
    {
        private double x, y;
        public double X
        {
            get { return x; }
        }
        public double Y
        {
            get { return y; }
        }

        /**
         * Creates a vector pointing to (x,y) from the origin.
         */
        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /**
         * Returns this dot v.
         */
        public double dotProduct(Vector v)
        {
            return (this.X * v.X) + (this.Y * v.Y);
        }

        /**
         * Returns the determinant of this and v.
         */
        public double determinant(Vector v)
        {
            return (this.X * v.Y) - (this.Y * v.X);
        }

        /**
         * Returns the magnitude of the current vector.
         */
        public double magnitude()
        {
            double sumOfSquares = (this.X * this.X)+ (this.Y * this.Y);
            return Math.Sqrt(sumOfSquares);
        }

        /**
         * Returns the angle from the current vector to v in radians.
         */
        public double radianAngleTo(Vector v)
        {
            return Math.Atan2(this.determinant(v), this.dotProduct(v));
        }

        /**
         * Returns the angle from the current vector to v in degrees.
         */
        public double degreeAngleTo(Vector v)
        {
            return radianAngleTo(v) * 180 / Math.PI;
        }
    }
}
