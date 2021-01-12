using SadRogue.Primitives;
using System;

namespace Rogue {
    public static class PointExtensions {
        public static Point East(this Point point) => new Point(point.X + 1, point.Y);
        public static Point North(this Point point) => new Point(point.X, point.Y - 1);
        public static Point West(this Point point) => new Point(point.X - 1, point.Y);
        public static Point South(this Point point) => new Point(point.X, point.Y + 1);

        public static Point Increment(this Point point, Direction.Types dir) => dir switch {
            Direction.Types.Up => new Point(point.X, point.Y - 1),
            Direction.Types.Right => new Point(point.X + 1, point.Y),
            Direction.Types.Down => new Point(point.X, point.Y + 1),
            Direction.Types.Left => new Point(point.X - 1, point.Y),
            _ => throw new NotImplementedException(),
        };
    }
}
