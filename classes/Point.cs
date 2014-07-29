using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace PathFinding
{
    public class Point: Object
    {
        private float x, y;

        //X-Coordinate property
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        //Y-Coordinate property
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        /**
         * Initializes a point instance with X-Coordinate equal to x, and Y-Coordinate equal to y.
         * 
         * @param x is the float that becomes the X-Coordinate of the new Point instance.
         * @param y is the float that becomes the Y-Coordinate of the new Point instance.
         */
        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Point()
        {
            this.x = 0;
            this.y = 0;
        }

        /**
         * Shifts the point by translateX and translateY.
         * 
         * @param translateX is the amount the point should be shifted in the X direction.
         * @param translateY is the amount the point should be shifted in the Y direction.
         */
        public void translate(float translateX, float translateY) 
        {
            x += translateX;
            y += translateY;

        }

        /**
         * Shifts the point by rotating it about the origin by angle degrees.
         * (Imagine a line segment from the point to the origin, and rotating
         * that line segment angle degrees. This method just updates the new 
         * position of our point after being rotated.)
         * 
         * @param angle the amount by which the point is rotated about the origin (specified in degrees).
         */
        public void rotate(double angle)
        {
            double radianAngle = angle * Math.PI / 180;
            double temp_x = Math.Cos(radianAngle) * x - Math.Sin(radianAngle) * y;
            double temp_y = Math.Sin(radianAngle) * x + Math.Cos(radianAngle) * y;
            x = (float)temp_x;
            y = (float)temp_y;
        }

        /**
         * Rotates and then translates the point.
         * 
         * @param translateX the amount by which the point is shifted in the X-direction.
         * @param translateY the amount by which the piont is shifted in the Y-direction.
         * @param angle the amount by which the piont is rotated about the origin (specified in degrees).
         */
        public void transform(float translateX, float translateY, float angle)
        {
            Matrix transformation = new Matrix();
            transformation.Translate(translateX, translateY);
            transformation.Rotate(angle);
            System.Drawing.PointF[] points = { new System.Drawing.PointF(x, y) };
            transformation.TransformPoints(points);
            x = points[0].X;
            y = points[0].Y;

        }
        /**
         * Overrides the Equals function.
         * Returns true if the x and y coordinates of two points are equal.
         * 
         * @param obj the Object being compared to this instance.
         * @return whether obj has the same coordinates as this instance.
         */

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Point p = (Point)obj;
            return (x == p.x && y == p.y);
        }


        /**
         * Returns true if the x and y coordinates of two points are closer than epsilon
         * 
         * @param point the point to compare to
         * @param epsilon the acceptable error
         */
        public bool isCloseTo(Point point, double epsilon)
        {
            if (point == null)
                return false;
            return (CoordinateCalculator.hasDifferenceLessThan(this.X, point.X, epsilon)
                && CoordinateCalculator.hasDifferenceLessThan(this.Y, point.Y, epsilon));
        }

    }
}
