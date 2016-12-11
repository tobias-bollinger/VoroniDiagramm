namespace Voronoi_Diagram.Geometric
{
    public class Edge
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        /// <summary>
        /// Reves the Edge Start = End, End = Start
        /// </summary>
        public Edge Reverse => new Edge(End, Start);


        public Edge(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return $"[{Start} - {End}]";
        }

        /// <summary>
        /// Edge is also equals to it reversed counter part.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var e = (Edge)obj;
            var rE = e.Reverse;
            return (Start.Equals(e.Start) && End.Equals(e.End)) || (Start.Equals(rE.Start) && End.Equals(rE.End));
        }
    }
}
