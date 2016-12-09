using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Voronoi_Diagram.Voroni
{
    class Voroni
    {
        private static List<Seg> Output;

        struct Event
        {
            double X;
            Vector2 P;
            Arc A;
            bool IsValid;

            public Event(double x, Vector2 p, Arc a)
            {
                X = x;
                P = p;
                A = a;
                IsValid = true;
            }
        }

        class Arc
        {
            Vector2 P;
            Arc Prev, Next;
            Event E;
            Seg S0;
            Seg S1;

            public Arc(Vector2 p, Arc a = null, Arc b = null)
            {
                P = p;
                Prev = a;
                Next = b;
                S0 = new Seg();
                S1 = new Seg();
            }
        }

        internal struct Seg
        {
            private Vector2 Start;
            private Vector2 End;
            private bool Done;

            public Seg(Vector2 p)
            {
                Start = p;
                End = Vector2.Zero;
                Done = false;
                Output.Add(this);
            }

            public void Finish(Vector2 p)
            {
                if (!Done)
                {
                    End = p;
                    Done = true;
                }
            }
        }

        //TODO make Priority Queue
        private static SortedList<double, Vector2> _points = new SortedList<double, Vector2>();
        private static SortedList<double, Event> _events = new SortedList<double, Event>();
       

        public static List<Seg> Solve(IEnumerable<Vector2> points, Size boundingBox)
        {


            return null;
        }

        private static void ProcessPoint()
        {
            var p = _points.First().Value;

        }
    }
}
