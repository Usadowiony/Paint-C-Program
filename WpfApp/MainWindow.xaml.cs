using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfAppTest2
{
    public partial class MainWindow : Window
    {
        private bool isDown = false;
        private Point startPoint;
        public ObservableCollection<ColorInfo> AvailableColors { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ListaDoMalowania.Items.Add("Brush");
            ListaDoMalowania.Items.Add("Line");
            ListaDoMalowania.Items.Add("Circle");
            ListaDoMalowania.Items.Add("Rectangle");

            AvailableColors = new ObservableCollection<ColorInfo>
            {
                new ColorInfo("Red", Colors.Red),
                new ColorInfo("Green", Colors.Green),
                new ColorInfo("Blue", Colors.Blue),
                new ColorInfo("Yellow", Colors.Yellow),
            };

            ListaKolorów.ItemsSource = AvailableColors;
        }

        private void PaintBrush(Color brushColor, Point position)
        {
            Ellipse brush = new Ellipse();
            brush.Fill = new SolidColorBrush(brushColor);
            brush.Width = 8;
            brush.Height = 8;
            Canvas.SetLeft(brush, position.X);
            Canvas.SetTop(brush, position.Y);
            CanvasTest.Children.Add(brush);
        }

        private void PaintCircle(Color circleColor, Point center, double radius)
        {
            Ellipse circle = new Ellipse();
            circle.Fill = new SolidColorBrush(circleColor);
            circle.Width = radius * 2;
            circle.Height = radius * 2;
            Canvas.SetLeft(circle, center.X - radius);
            Canvas.SetTop(circle, center.Y - radius);
            CanvasTest.Children.Add(circle);
        }

        private void PaintRectangle(Color rectangleColor, Point start, Point end)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = new SolidColorBrush(rectangleColor);
            rectangle.Width = Math.Abs(end.X - start.X);
            rectangle.Height = Math.Abs(end.Y - start.Y);
            Canvas.SetLeft(rectangle, Math.Min(start.X, end.X));
            Canvas.SetTop(rectangle, Math.Min(start.Y, end.Y));
            CanvasTest.Children.Add(rectangle);
        }

        private void PaintLine(Color lineColor, Point start, Point end)
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(lineColor);
            line.StrokeThickness = 2;
            line.X1 = start.X;
            line.Y1 = start.Y;
            line.X2 = end.X;
            line.Y2 = end.Y;
            CanvasTest.Children.Add(line);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDown && ListaDoMalowania.SelectedItem != null)
            {
                Point mousePosition = e.GetPosition(CanvasTest);
                switch (ListaDoMalowania.SelectedItem.ToString())
                {
                    case "Brush":
                        if (ListaKolorów.SelectedItem != null && ListaKolorów.SelectedItem is ColorInfo chosenColor)
                        {
                            PaintBrush(chosenColor.Color, mousePosition);
                        }
                        break;
                    case "Rectangle":
                        break;
                    case "Line":
                        break;
                    case "Circle":
                        break;
                    default:
                        break;
                }
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ListaDoMalowania.SelectedItem != null)
            {
                isDown = true;
                startPoint = e.GetPosition(CanvasTest);
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ListaKolorów.SelectedItem != null && ListaDoMalowania.SelectedItem != null)
            {
                if (ListaDoMalowania.SelectedItem.ToString() == "Rectangle")
                {
                    Point endPoint = e.GetPosition(CanvasTest);
                    Color chosenColor = ((ColorInfo)ListaKolorów.SelectedItem).Color;
                    PaintRectangle(chosenColor, startPoint, endPoint);
                }
                else if (ListaDoMalowania.SelectedItem.ToString() == "Line")
                {
                    Point endPoint = e.GetPosition(CanvasTest);
                    Color chosenColor = ((ColorInfo)ListaKolorów.SelectedItem).Color;
                    PaintLine(chosenColor, startPoint, endPoint);
                }
                else if (ListaDoMalowania.SelectedItem.ToString() == "Circle")
                {
                    Point endPoint = e.GetPosition(CanvasTest);
                    Color chosenColor = ((ColorInfo)ListaKolorów.SelectedItem).Color;
                    double radius = Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(endPoint.Y - startPoint.Y, 2));
                    PaintCircle(chosenColor, startPoint, radius);
                }
            }
            isDown = false;
        }
    }

    public class ColorInfo
    {
        public string Name { get; set; }
        public Color Color { get; set; }

        public ColorInfo(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
