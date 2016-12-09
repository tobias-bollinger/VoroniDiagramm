/** -----------------------------------------------------------------
 *  -------  Effiziente Algorithmen                  ----------------
 *  -------                                          ----------------
 *  -------  implements the Jarvis & March           ----------------
 *  -------  algorithm for calculating the convex    ----------------
 *  -------  hull of points in the xy-plane          ----------------
 *  -------                                          ----------------
 *  -------  @author  Manfred Vogel                  ----------------
 *  -------  2016 november                           ----------------
 *  -----------------------------------------------------------------
 **/

using System.Windows;

namespace Voronoi_Diagram.ConvexHull
{
    /// <summary>
    /// Rewritten in C# by Tobias Bollinger
    /// </summary>
    public class JarvisMarch : ConvexHull
    {
        public static Vector2[] ComputeHull(Vector2[] p)
        {
            SetPoints(p);
            DoJarvisMarch();
            return P;
        }

        private static void DoJarvisMarch()
        {
            var i = IndexOfLowestPoint();
            do
            {
                Exchange(H, i);
                i = IndexOfRightmostPointFrom(P[H]);
                H++;
            }
            while (i > 0);
        }

        private static int IndexOfRightmostPointFrom(Vector2 q)
        {
            int i = 0, j;
            for (j = 1; j < N; j++)
                if ((P[j] - q).IsLess(P[i] - q)) //if (p[j].relTo(q).isLess(p[i].relTo(q)))
                    i = j;
            return i;
        }
    }
}
