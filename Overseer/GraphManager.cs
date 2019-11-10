using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Overseer
{
    struct GraphInput
    {
        public string Name { get; set; }
        public Pair ChannelStation { get; set; }

        public GraphInput(string name, Pair channelStation) : this()
        {
            Name = name;
            ChannelStation = channelStation;
        }
    }

    struct GraphOutput
    {
        public string Name { get; set; }
        public Polygon GraphPolygon { get; set; }
        public Label GraphLabel { get; set; }

        public GraphOutput(string name, Polygon polygon, Label label) : this()
        {
            Name = name;
            GraphPolygon = polygon;
            GraphLabel = label;
        }
    }

    class GraphManager
    {
        public int Origin { get; set; }
        public int ChannelWidth { get; set; }
        public int GraphWidth { get; set; }
        public int GraphHeight { get; set; }
        public SolidColorBrush BaseFillBrush { get; set; }
        public SolidColorBrush BaseStrokeBrush { get; set; }
        public SolidColorBrush HighlightedFillBrush { get; set; }
        public SolidColorBrush HighlightedStrokeBrush { get; set; }
        public SolidColorBrush BaseLabelBrush { get; set; }
        public SolidColorBrush HighlightedLabelBrush { get; set; }
        public double StrokeThickness { get; set; }
        public int FontSize { get; set; }
        List<GraphOutput> graphOutputList;

        public GraphManager() : this(0, 45, 800, 270, new SolidColorBrush(Color.FromArgb(10, 0, 255, 195)),
            new SolidColorBrush(Color.FromArgb(255, 0, 255, 195)),
            new SolidColorBrush(Color.FromArgb(10, 0, 180, 255)),
            new SolidColorBrush(Color.FromArgb(255, 0, 180, 255)),
            10)
        { }

        public GraphManager(int origin, int channelWidth, int graphWidth, int graphHeight,SolidColorBrush baseFillBrush,
            SolidColorBrush baseStrokeBrush, SolidColorBrush highlightedFillBrush, SolidColorBrush highlightedStrokeBrush,
            int fontSize)
        {
            Origin = origin;
            ChannelWidth = channelWidth;
            GraphHeight = graphHeight;
            GraphWidth = graphWidth;
            BaseFillBrush = baseFillBrush;
            BaseStrokeBrush = baseStrokeBrush;
            HighlightedFillBrush = highlightedFillBrush;
            HighlightedStrokeBrush = highlightedStrokeBrush;
            BaseLabelBrush = Brushes.White;
            HighlightedLabelBrush = Brushes.BlueViolet;
            FontSize = fontSize;
            graphOutputList = new List<GraphOutput>();
            StrokeThickness = 1.5;
        }

        public List<GraphOutput> GetGraphOutput(List<GraphInput> graphInputList)
        {
            graphOutputList.Clear();
            int orix = Origin + ChannelWidth;
            int oriy = Origin;

            foreach (var graphInput in graphInputList)
            {
                int xl = orix + (ChannelWidth * (graphInput.ChannelStation.Channel - 2));
                int xr = orix + (ChannelWidth * (graphInput.ChannelStation.Channel + 2));
                int yb = 270 - oriy;
                int yt = 270 - (2 * graphInput.ChannelStation.Strength + oriy);
                Polygon pg = new Polygon()
                {
                    Points = new PointCollection
                            {
                                new Point(xl, yb),
                                new Point(xl + 5, yt),
                                new Point(xr - 5, yt),
                                new Point(xr, yb)
                            },
                    Fill = BaseFillBrush,
                    Stroke = BaseStrokeBrush,
                    StrokeThickness = this.StrokeThickness
                };
                Label lb = new Label()
                {
                    Content = graphInput.Name,
                    Foreground = BaseLabelBrush,
                    FontSize = FontSize,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent
                };
                Canvas.SetLeft(lb, xl + 2 * ChannelWidth - graphInput.Name.Length * 10 / 2.5);
                Canvas.SetTop(lb, yt);
                graphOutputList.Add(new GraphOutput(graphInput.Name, pg, lb));
            }
            return graphOutputList;
    }

        public void HighlightGraph(string Selected, string Deselected)
        {
            if (Selected == null) Selected = "";
            if (Deselected == null) Deselected = "";
            if(graphOutputList != null && graphOutputList.Count > 0)
            {
                foreach(var graph in graphOutputList)
                {
                    if(graph.Name == Selected)
                    {
                        graph.GraphPolygon.Stroke = HighlightedStrokeBrush;
                        graph.GraphPolygon.Fill = HighlightedFillBrush;
                        graph.GraphLabel.Foreground = HighlightedLabelBrush;
                    }
                    else if(graph.Name == Deselected)
                    {
                        graph.GraphPolygon.Stroke = BaseStrokeBrush;
                        graph.GraphPolygon.Fill = BaseFillBrush;
                        graph.GraphLabel.Foreground = BaseLabelBrush;
                    }
                }
            }
        }
    }
}
