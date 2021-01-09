using Rogue.MazeGenerator;
using RogueSharp;
using System;
using static Rogue.MazeGenerator.Direction;

namespace Rogue {
    public static class PointExtensions {
        public static Point East(this Point point) => new Point(point.X + 1, point.Y);
        public static Point North(this Point point) => new Point(point.X, point.Y - 1);
        public static Point West(this Point point) => new Point(point.X - 1, point.Y);
        public static Point South(this Point point) => new Point(point.X, point.Y + 1);

        public static Point Increment(this Point point, Direction dir) => dir switch {
            Direction.North => new Point(point.X, point.Y - 1),
            Direction.East => new Point(point.X + 1, point.Y),
            Direction.South => new Point(point.X, point.Y + 1),
            Direction.West => new Point(point.X - 1, point.Y),
            _ => throw new NotImplementedException(),
        };
    }
}
