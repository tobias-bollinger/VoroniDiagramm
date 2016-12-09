using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Voronoi_Diagram.ConvexHull;

namespace Voronoi_Diagram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Todo only point elipse is not needed
        private List<Vector2> _points = new List<Vector2>();

        struct VoronoiPoint
        {
            public Ellipse Ellipse;
            public Vector2 Pos;
            public VoronoiPoint(Ellipse ellipse, Vector2 pos)
            {
                Ellipse = ellipse;
                Pos = pos;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(PointCanvas);

            var elipse = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = new SolidColorBrush(Colors.Red),
                ToolTip = e.GetPosition(PointCanvas)
            };

            Canvas.SetLeft(elipse, pos.X);
            Canvas.SetTop(elipse, pos.Y);
            PointCanvas.Children.Add(elipse);
            _points.Add((Vector2)pos);
            CalculateFortunes();
        }

        private void CalculateFortunes()
        {
            if(_points.Count < 2) return;

            var pointQueue = new Queue<Vector2>(_points.OrderBy(vector2 => vector2.Y));
            var firstPoint = pointQueue.Dequeue();
            var secondPoint = pointQueue.Dequeue();
            var sweepLine = secondPoint.Y;
            ConvexCanvas.Children.Add(
                GenerateLine(new Vector2(0, sweepLine), new Vector2(this.ActualWidth, sweepLine), Colors.Red));

            var d = Vector2.Distance(firstPoint, secondPoint)/2;

            var pl = new Vector2(firstPoint.X - d, firstPoint.Y);
            var pr = new Vector2(firstPoint.X + d, firstPoint.Y);
            var pb = new Vector2(firstPoint.X, sweepLine);

            var pArc = new Vector2(firstPoint.X - d, firstPoint.Y);
            var rap = pr - pArc;

            ArcToPath(pl, new Size(d, d));
            //ConvexCanvas.Children.Add(pf.);
        }

        private void ArcToPath(Vector2 point, Size size)
        {
            var g = new StreamGeometry();

            using (var gc = g.Open())
            {
                gc.BeginFigure(
                    startPoint: new Point(0, 0),
                    isFilled: false,
                    isClosed: false);

                gc.ArcTo(
                    point: new Point(point.X, point.Y),
                    size: size,
                    rotationAngle: 0d,
                    isLargeArc: false,
                    sweepDirection: SweepDirection.Counterclockwise,
                    isStroked: true,
                    isSmoothJoin: false);
            }

            var path = new Path
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Data = g
            };

            Canvas.SetLeft(path, 0);
            Canvas.SetTop(path, 0);

            ConvexCanvas.Children.Add(path);
        }

        private Line GenerateLine(Vector2 start, Vector2 end, Color color)
        {
            var line = new Line
            {
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
                Fill = new SolidColorBrush(color),
                Stroke = new SolidColorBrush(color),
                StrokeThickness = 1
            };

            Canvas.SetLeft(line, 0);
            Canvas.SetTop(line, 0);
            return line;
        }

        private void CalculateConvex()
        {
            if(_points.Count < 2) return;
            
            ConvexCanvas.Children.Clear();
            var convexPoints = JarvisMarch.ComputeHull(_points.ToArray());

            for (var i = 0; i < JarvisMarch.H; i++)
            {
                var k = (i < JarvisMarch.H - 1) ? i + 1 : 0;

                var line = new Line
                {
                    X1 = convexPoints[i].X,
                    Y1 = convexPoints[i].Y,
                    X2 = convexPoints[k].X,
                    Y2 = convexPoints[k].Y,
                    Fill = new SolidColorBrush(Colors.Blue),
                    Stroke = new SolidColorBrush(Colors.Blue),
                    StrokeThickness = 1
                };

                Canvas.SetLeft(line, 0);
                Canvas.SetTop(line, 0);

                ConvexCanvas.Children.Add(line);
            }

            //for (var i = 0; i < _points.Count; i++)
            //{
            //    if (i < _points.Count - 1)
            //    {
            //        var a = _points[i].Pos;
            //        var b = _points[i + 1].Pos;

            //        Vector2.Lerp(a, b, 0.5);
            //        //y, -x
            //        var d = (a - b);
            //        var s = new Vector2(d.Y, -d.X);

            //        var l = new Line
            //        {
            //            X1 = d.X + s.X,
            //            X2 = d.X - s.X,
            //            Y1 = d.Y + s.Y,
            //            Y2 = d.Y - s.Y,
            //            Fill = new SolidColorBrush(Colors.Blue)
            //        };
            //        PointCanvas.Children.Add(l);
            //    }
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PointCanvas.Children.Clear();
            ConvexCanvas.Children.Clear();
            _points.Clear();
        }
    }
}
