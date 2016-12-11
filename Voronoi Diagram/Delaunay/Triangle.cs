using System;
using System.Collections.Generic;
using System.Linq;
using Voronoi_Diagram.Geometric;
using Voronoi_Diagram.Voroni;

namespace Voronoi_Diagram.Delaunay
{
    public class Triangle
    {
        public readonly List<Vector2> _vertices;

        //All three vertices
        public readonly Vector2 A;
        public readonly Vector2 B;
        public readonly Vector2 C;

        //All three Edges
        public Edge AB => new Edge(A, B);
        public Edge BC => new Edge(B, C);
        public Edge CA => new Edge(C, A);

        /// <summary>
        /// Radius of the circumcirclle
        /// </summary>
        public readonly double R;

        /// <summary>
        /// The circumcenter
        /// </summary>
        public Vector2 CenterPoint { get; }

        public Triangle(Vector2 a, Vector2 b, Vector2 c)
        {
            _vertices = new List<Vector2>() {a,b,c};
            A = a;
            B = b;
            C = c;

            CenterPoint = Circle(A, B, C);
            R = Math.Sqrt(Math.Pow(CenterPoint.X - A.X, 2) + Math.Pow(CenterPoint.Y - A.Y, 2));
        }

        /// <summary>
        /// Checks if a point is in the circuncircle of the Triangle
        /// </summary>
        /// <param name="p">Point to check</param>
        /// <returns>True if it is in the circuncircle</returns>
        public bool IsInCircumcircle(Vector2 p)
        {
            return Vector2.Distance(CenterPoint, p) <= R;
        }

        /// <summary>
        /// Retruns the Vertices of a triangle
        /// </summary>
        /// <returns></returns>
        public List<Vector2> GetVertices()
        {
            return _vertices;
        }

        /// <summary>
        /// Returns the Edges of the triangle
        /// </summary>
        /// <returns></returns>
        public List<Edge> GetEdges()
        {
            return new List<Edge>() { AB, BC, CA};
        }

        public override bool Equals(object obj)
        {
            var tri = (Triangle) obj;
            return tri != null && (tri.A.Equals(A) && tri.B.Equals(B) && tri.C.Equals(C));
        }

        /// <summary>
        /// Calculates the circumcenter of the triangle
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns>Circumcenter as an Vector</returns>
        private Vector2 Circle(Vector2 a, Vector2 b, Vector2 c)
        {
            double A = b.X - a.X,
                   B = b.Y - a.Y,
                   C = c.X - a.X,
                   D = c.Y - a.Y,
                   E = A * (a.X + b.X) + B * (a.Y + b.Y),
                   F = C * (a.X + c.X) + D * (a.Y + c.Y),
                   G = 2 * (A * (c.Y - b.Y) - B * (c.X - b.X));

            if (G == 0)
            {
                //Points are co-linear
                return new Vector2(0, 0);
            }

            return new Vector2
            {
                X = (D*E - B*F)/G,
                Y = (A*F - C*E)/G
            };
        }

        /// <summary>
        /// Check if the triange is a neigbour
        /// </summary>
        /// <param name="tri"></param>
        /// <returns>True if it is a neighbout</returns>
        public bool IsNeighbour(Triangle tri)
        {
            return GetEdges().Any(edge => tri.GetEdges().Any(edge.Equals));
        }

        /// <summary>
        /// Not used in the code
        /// http://paulbourke.net/geometry/circlesphere/
        /// </summary>
        /// <param name="tri"></param>
        /// <returns></returns>
        public bool CircleIntersection(Triangle tri, out Vector2[] intesections)
        {
            var p0 = CenterPoint;
            var p1 = tri.CenterPoint;
            var d = Vector2.Distance(p0, p1);
            var r0 = R;
            var r1 = tri.R;

            if (d > r0 + r1 || d < Math.Abs(r0 - r1) || d == 0)
            {
                intesections = new Vector2[0];
                return false;
            }

            var x0 = CenterPoint.X;
            var y0 = CenterPoint.Y;
            var x1 = tri.CenterPoint.X;
            var y1 = tri.CenterPoint.Y;
            
            var a = (r0*r0 - r1*r1 + d*d)/(2*d);
            var h = Math.Sqrt(r0*r0 - a*a);
            var p2 = p0 + a * (p1 - p0) / d;
            var x2 = p2.X;
            var y2 = p2.Y;

            var x3 = x2 - h*(y1 - y0)/d;
            var y3 = y2 + h*(x1 - x0)/d;

            var x4 = x2 + h * (y1 - y0)/d;
            var y4 = y2 - h * (x1 - x0)/d;

            if (double.IsNaN(x3) || double.IsNaN(x4) || double.IsNaN(y3) || double.IsNaN(y4))
            {
                intesections = new Vector2[0];
                return false;
            }

            intesections = new Vector2[] {new Vector2(x3, y3), new Vector2(x4, y4) };
            return true;
        }
    }
}
