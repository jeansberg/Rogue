using System.Collections.Generic;
using System.Drawing;

namespace Rogue
{
    public static class RectangleExtensions
    {
        public static bool Touches(this Rectangle rect1, Rectangle rect2, int margin)
        {
            return rect2.Left <= rect1.Right - margin &&
                rect1.Left < rect2.Right + margin &&
                rect2.Top < rect1.Bottom + margin &&
                rect1.Top <= rect2.Bottom - margin;
        }

        public static List<Point> Points(this Rectangle rect) {
            var points = new List<Point>();

            for(var x = rect.X; x < rect.X + rect.Width; x++) {
                for (var y = rect.Y; y < rect.Y + rect.Height; y++) {
                    points.Add(new Point(x, y));
                }
            }

            return points;
        }

        public static Core.Point Center (this Rectangle rect) {
            return new Core.Point((rect.Right - rect.Width) / 2, (rect.Bottom - rect.Y) / 2);
        }
    }
}
