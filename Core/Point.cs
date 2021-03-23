using System;
using System.Diagnostics.CodeAnalysis;

namespace Core {
    public struct Point: IEquatable<Point> {
        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj) {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        }

        public bool Equals([AllowNull] Point other) {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode() {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Point p1, Point p2) {
            return p1.Equals(p2);
        }

        public static Point operator +(Point p1, Point p2) {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static bool operator !=(Point p1, Point p2) {
            return !p1.Equals(p2);
        }
    }
}
