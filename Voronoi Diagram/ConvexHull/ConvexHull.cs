/*  -----------------------------------------------------------------
 *  -------  Effiziente Algorithmen                  ----------------
 *  -------                                          ----------------
 *  -------  helper class for the calculation of     ----------------
 *  -------  the convex hull of points               ----------------
 *  -------  in the xy-plane                         ----------------
 *  -------                                          ----------------
 *  -------  @author  Manfred Vogel                  ----------------
 *  -------  2016 november                           ----------------
 *  -----------------------------------------------------------------
 */

namespace Voronoi_Diagram.ConvexHull
{
    /// <summary>
    /// Rewritten in C# by Tobias Bollinger
    /// </summary>
    public abstract class ConvexHull
    {
        protected static Vector2[] P;
        protected static int N;
        public static int H;

        public static void SetPoints(Vector2[] p0)
        {
            P = p0;
            N = P.Length;
            H = 0;
        }

        protected static void Exchange(int i, int j)
        {
            var t = P[i];
            P[i] = P[j];
            P[j] = t;
        }

        protected static void MakeRelTo(Vector2 p0)
        {
            int i;
            var p1 = new Vector2(p0); // necessary, because p0 migth be in p[] 
            for (i = 0; i < N; i++)
                P[i] -= p1; //p[i].makeRelTo(p1);
        }

        protected static int IndexOfLowestPoint()
        {
            int i, min = 0;
            for (i = 1; i < N; i++)
                if (P[i].Y < P[min].Y || P[i].Y == P[min].Y && P[i].X < P[min].Y)
                    min = i;
            return min;
        }

        protected static bool IsConvex(int i)
        {
            return P[i].IsConvex(P[i - 1], P[i + 1]);
        }
    }
}