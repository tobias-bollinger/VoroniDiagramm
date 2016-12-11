using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Voronoi_Diagram.Geometric;
using Voronoi_Diagram.Voroni;

namespace Voronoi_Diagram.Delaunay
{
    internal static class Delaunay
    {
        /// <summary>
        /// Generate Delaunay Tryangels from points
        /// </summary>
        /// <param name="vertices">The Points</param>
        /// <param name="boundaries">The Canvas boundaries so it can be added to the vertices list</param>
        /// <returns>A List of Triangles</returns>
        public static List<Triangle> Triangulate(IEnumerable<Vector2> vertices, Size boundaries)
        {
            var triangles = new List<Triangle>();
            var verts = vertices.ToList();
            //Add all boundarie vetices
            verts.Add(new Vector2(0, 0));
            verts.Add(new Vector2(0, boundaries.Height));
            verts.Add(new Vector2(boundaries.Width, 0));
            verts.Add(new Vector2(boundaries.Width, boundaries.Height));
            //Calculate a Super triange which is large enouth to contais all vertices
            var superTriangle = CalcSuperTriangle(verts);
            verts.AddRange(superTriangle.GetVertices());
            triangles.Add(superTriangle);
            
            //Include each vetex one at a time in the triangulation
            foreach (var vertex in verts)
            {
                Triangulate(vertex, ref triangles);
            }
            //Remove all triangles which contais a vertex from the super triangle
            triangles.RemoveAll(triangle => triangle.GetVertices().Any(v => superTriangle.GetVertices().Contains(v)));

            return triangles;
        }

        /// <summary>
        /// Updates a triangle list with a new added vertex
        /// </summary>
        /// <param name="vertex">The new vertex</param>
        /// <param name="triangles">The triangle list</param>
        public static void Triangulate(Vector2 vertex, ref List<Triangle> triangles)
        {
            var polygon = new List<Edge>();

            //Search for a triangle which Circumcicle is on the vertex
            //Add the found triangle edged to the polygon and remove it form the triangle list
            for (int i = triangles.Count - 1; i >= 0; i--)
            {
                if (triangles[i].IsInCircumcircle(vertex))
                {
                    polygon.AddRange(triangles[i].GetEdges());
                    triangles.RemoveAt(i);
                }
            }

            //Remove all doubly specifed edges form the polygon
            for (int j = polygon.Count - 2; j >= 0; j--)
            {
                for (int k = polygon.Count - 1; k >= j + 1; k--)
                {
                    if (polygon[j].Equals(polygon[k]))
                    {
                        polygon.RemoveAt(k);
                        polygon.RemoveAt(j);
                        k--;
                        continue;
                    }
                }
            }

            //Make new Triangles witch the polygon edges and the vertex
            triangles.AddRange(polygon.Select(e => new Triangle(e.Start, e.End, vertex)));
        }

        /// <summary>
        /// Calclualtes the Super Triangle
        /// </summary>
        /// <param name="points">Points which need to be in the triangle</param>
        /// <returns>A super Triangle</returns>
        private static Triangle CalcSuperTriangle(List<Vector2> points)
        {
            var m = points[0].X;

            for (var i = 1; i < points.Count; i++)
            {
                var xAbs = Math.Abs(points[i].X);
                var yAbs = Math.Abs(points[i].Y);
                if (xAbs > m) m = xAbs;
                if (yAbs > m) m = yAbs;
            }

            var sp1 = new Vector2(10 * m, 0);
            var sp2 = new Vector2(0, 10 * m);
            var sp3 = new Vector2(-10 * m, -10 * m);
            return new Triangle(sp1, sp2, sp3);
        }
    }
}
