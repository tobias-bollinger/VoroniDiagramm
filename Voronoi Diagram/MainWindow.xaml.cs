using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Voronoi_Diagram.Delaunay;
using Voronoi_Diagram.Geometric;
using Voronoi_Diagram.Voroni;

namespace Voronoi_Diagram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Vector2> _points = new List<Vector2>();
        private List<Triangle> _triangles = new List<Triangle>();

        public MainWindow()
        {
            InitializeComponent();
            DelaunayCanvas.Visibility = Visibility.Hidden;
        }

        private void ReDraw()
        {
            _triangles.Clear();
            _triangles = Delaunay.Delaunay.Triangulate(_points, new Size(PointCanvas.ActualWidth, PointCanvas.ActualHeight));

            foreach (var triangle in _triangles)
            {
                DelaunayCanvas.Children.Add(GenerateTriangle(triangle, Colors.Green));
            }

            var edges = Voronoi.CalculateEdges(_triangles);
            VoronoiCanvas.Children.Clear();
            foreach (var edge in edges)
            {
                VoronoiCanvas.Children.Add(GenerateLine(edge.Start, edge.End, Colors.Blue));
            }
        }

        private void DrawVoroni(Vector2 addedPoint)
        {
            if (_points.Count == 1)
            {
                _triangles.Clear();
                _triangles = Delaunay.Delaunay.Triangulate(_points, new Size(PointCanvas.ActualWidth, PointCanvas.ActualHeight));
            }
            else
            {
                Delaunay.Delaunay.Triangulate(addedPoint, ref _triangles);
                DelaunayCanvas.Children.Clear();
            }

            foreach (var triangle in _triangles)
            {
                DelaunayCanvas.Children.Add(GenerateTriangle(triangle, Colors.Green));
            }

            var edges = Voronoi.CalculateEdges(_triangles);
            VoronoiCanvas.Children.Clear();
            foreach (var edge in edges)
            {
                VoronoiCanvas.Children.Add(GenerateLine(edge.Start, edge.End, Colors.Blue));
            }
        }

        #region ShapeGenerateMethodes

        private Ellipse GeneratePoint(Vector2 pos, Color color)
        {
            var elipse = new Ellipse
            {
                Width = 5,
                Height = 5,
                Fill = new SolidColorBrush(color),
                ToolTip = pos.ToString()
            };
            Canvas.SetLeft(elipse, pos.X - 2.5);
            Canvas.SetTop(elipse, pos.Y - 2.5);
            return elipse;
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

            Canvas.SetLeft(line, 0.5);
            Canvas.SetTop(line, 0.5);
            return line;
        }

        private Ellipse GenerateCircle(Vector2 middle, double radius, Color color)
        {
            var elipse = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Stroke = new SolidColorBrush(color)
            };
            var topLeft = new Vector2(middle.X-radius, middle.Y-radius);
            Canvas.SetLeft(elipse, topLeft.X);
            Canvas.SetTop(elipse, topLeft.Y);
            return elipse;
        }

        private Polygon GenerateTriangle(Triangle triangle, Color color)
        {
            return GeneratePolygon(triangle.GetVertices(), color);
        }

        private Polygon GeneratePolygon(IEnumerable<Vector2> vetices, Color color)
        {
            var poly = new Polygon
            {
                Stroke = new SolidColorBrush(color),
                StrokeThickness = 1
            };

            Canvas.SetLeft(poly, 0);
            Canvas.SetTop(poly, 0);

            poly.Points = new PointCollection(vetices.Select(v => new Point(v.X, v.Y)));
            return poly;
        }
        #endregion

        #region Events
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(PointCanvas);
            PointCanvas.Children.Add(GeneratePoint((Vector2)pos, Colors.Red));
            _points.Add((Vector2)pos);
            DrawVoroni((Vector2)pos);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            PointCanvas.Children.Clear();
            DelaunayCanvas.Children.Clear();
            VoronoiCanvas.Children.Clear();
            _triangles.Clear();
            _points.Clear();
        }

        private void Delaunay_Click(object sender, RoutedEventArgs e)
        {
            DelaunayCanvas.Visibility = (Visibility)(((int)DelaunayCanvas.Visibility + 1)%2);
        }

        private void Voronoi_Click(object sender, RoutedEventArgs e)
        {
            VoronoiCanvas.Visibility = (Visibility)(((int)VoronoiCanvas.Visibility + 1) % 2);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ReDraw();
        }
        #endregion
    }
}
