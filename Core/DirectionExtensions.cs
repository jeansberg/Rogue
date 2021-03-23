using Core;
using System;

namespace Rogue {
    public static class DirectionExtensions {
        public static Direction Opposite(this Direction direction) => direction switch {
            Direction.None => Direction.None,
            Direction.Up => Direction.Down,
            Direction.UpRight => Direction.DownLeft,
            Direction.Right => Direction.Left,
            Direction.DownRight => Direction.UpLeft,
            Direction.Down => Direction.Up,
            Direction.DownLeft => Direction.UpRight,
            Direction.Left => Direction.Right,
            Direction.UpLeft => Direction.DownRight,
            _ => throw new NotImplementedException(),
        };

        public static Direction GetDirection(Point source, Point destination) {
            var difference = new Point(destination.X - source.X, destination.Y - source.Y);
            int dx = difference.X;
            int dy = difference.Y;

            if (dx == 0 && dy == 0)
                return Direction.None;

            double angle = Math.Atan2(dy, dx);
            double degree = 180 / Math.PI * angle;
            degree += 450;
            degree %= 360;

            if (degree < 45.0)
                return Direction.Up;

            if (degree < 135.0)
                return Direction.Right;

            if (degree < 225.0)
                return Direction.Down;

            if (degree < 315.0)
                return Direction.Left;

            return Direction.Up;
        }
    }
}
