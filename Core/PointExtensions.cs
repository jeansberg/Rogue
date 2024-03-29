﻿using Core;
using System;

namespace Rogue {
    public static class PointExtensions {
        public static Point Right(this Point point) => new Point(point.X + 1, point.Y);
        public static Point Up(this Point point) => new Point(point.X, point.Y - 1);
        public static Point Left(this Point point) => new Point(point.X - 1, point.Y);
        public static Point Down(this Point point) => new Point(point.X, point.Y + 1);

        public static Point Increment(this Point point, Direction dir) => dir switch {
            Direction.Up => new Point(point.X, point.Y - 1),
            Direction.Right => new Point(point.X + 1, point.Y),
            Direction.Down => new Point(point.X, point.Y + 1),
            Direction.Left => new Point(point.X - 1, point.Y),
            _ => throw new NotImplementedException(),
        };

        public static bool IsAdjacent(this Point point, Point otherPoint) => 
            Math.Abs(point.X - otherPoint.X) + Math.Abs(point.Y - otherPoint.Y) == 1;

        public static Point ToPoint(this System.Drawing.Point point) => new Point(point.X, point.Y);
    }
}
