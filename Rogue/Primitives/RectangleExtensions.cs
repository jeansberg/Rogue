using SadRogue.Primitives;
using System.Collections.Generic;

namespace Rogue
{
    public static class RectangleExtensions
    {
        public static bool Touches(this Rectangle rect1, Rectangle rect2, int margin)
        {
            return rect2.X <= rect1.MaxExtentX - margin &&
                rect1.X < rect2.MaxExtentX + margin &&
                rect2.Y < rect1.MaxExtentY + margin &&
                rect1.Y <= rect2.MaxExtentY - margin;
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
    }
}
