using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Overseer
{
    static class GraphCreator
    {
        public static Bitmap GetBlankGraph(int width, int height)
        {
            Bitmap BX = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for(int y = 0 ; y < BX.Height ; y++)
            {
                for(int x = 0 ; x < BX.Width; x++)
                {
                    BX.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 0, 0, 0));
                }
            }
            return BX;
        }

        public static void UpdateImageSourceFromBitmap(Bitmap BX, ref ImageSource imgSource)
        {
            imgSource = Imaging.CreateBitmapSourceFromHBitmap(
                BX.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        public static Bitmap GetDrawnGraph(Bitmap blankGraph, List<Pair> points, System.Drawing.Color graphColor)
        {
            Bitmap drawnGraph = new Bitmap(blankGraph);
            int channelWidth = 45;
            int origin = 45;
            int orix = origin + channelWidth;
            int oriy = origin;
            int yt;
            int xl, xr, y, i;
            foreach(var point in points)
            {
                xl = orix + (channelWidth * (point.X - 2));
                xr = orix + (channelWidth * (point.X + 2));
                y = point.Y * 2;
                yt = drawnGraph.Height - (oriy + y - 1);
                for (i = oriy; i < oriy + y; i++)
                {
                    drawnGraph.SetPixel(xl - 1, drawnGraph.Height - i, graphColor);
                    drawnGraph.SetPixel(xl, drawnGraph.Height - i, graphColor);
                    drawnGraph.SetPixel(xl + 1, drawnGraph.Height - i, graphColor);
                    drawnGraph.SetPixel(xr - 1, drawnGraph.Height - i, graphColor);
                    drawnGraph.SetPixel(xr, drawnGraph.Height - i, graphColor);
                    drawnGraph.SetPixel(xr + 1, drawnGraph.Height - i, graphColor);
                }
                for(i = xl; i <= xr; i++)
                {
                    drawnGraph.SetPixel(i, yt - 1, graphColor);
                    drawnGraph.SetPixel(i, yt, graphColor);
                    drawnGraph.SetPixel(i, yt + 1, graphColor);
                }
            }
            return drawnGraph;
        }

        public static Bitmap GetDrawnGraph(Bitmap blankGraph, Pair point, System.Drawing.Color graphColor)
        {
            Bitmap drawnGraph = new Bitmap(blankGraph);
            int channelWidth = 45;
            int origin = 45;
            int orix = origin + channelWidth;
            int oriy = origin;
            int yt;
            int xl, xr, y, i;
 
                xl = orix + (channelWidth * (point.X - 2));
                xr = orix + (channelWidth * (point.X + 2));
                y = point.Y * 2;
                yt = drawnGraph.Height - (oriy + y - 1);
                for (i = oriy; i < oriy + y; i++)
                {
                    drawnGraph.SetPixel(xl - 1, drawnGraph.Height - i, graphColor);
                    drawnGraph.SetPixel(xl, drawnGraph.Height - i, graphColor);
                    drawnGraph.SetPixel(xl + 1, drawnGraph.Height - i, graphColor);
                    drawnGraph.SetPixel(xr - 1, drawnGraph.Height - i, graphColor);
                    drawnGraph.SetPixel(xr, drawnGraph.Height - i, graphColor);
                    drawnGraph.SetPixel(xr + 1, drawnGraph.Height - i, graphColor);
                }
                for (i = xl; i <= xr; i++)
                {
                    drawnGraph.SetPixel(i, yt - 1, graphColor);
                    drawnGraph.SetPixel(i, yt, graphColor);
                    drawnGraph.SetPixel(i, yt + 1, graphColor);
                }
            return drawnGraph;
        }
    }
}
