using System;
using System.Collections.Generic;
using System.Linq;
using Voronoi_Diagram.Delaunay;
using Voronoi_Diagram.Geometric;

namespace Voronoi_Diagram.Voroni
{
    internal static class Voronoi
    {
        /// <summary>
        /// Calculate Voronoi form Delanay
        /// </summary>
        /// <param name="delaunayTriangles">The delanay triangles</param>
        /// <returns>Voronoi edges</returns>
        public static List<Edge> CalculateEdges(IEnumerable<Triangle> delaunayTriangles)
        {
            if(delaunayTriangles == null) throw new ArgumentException("delaunayTriangles cannot be null");

            return (
                from triangle in delaunayTriangles
                from otherTriangle in delaunayTriangles
                where triangle != otherTriangle
                where triangle.IsNeighbour(otherTriangle)
                select new Edge(triangle.CenterPoint, otherTriangle.CenterPoint)
                ).ToList();
        }
    }
}
