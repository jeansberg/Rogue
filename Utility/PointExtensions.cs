using Core;

namespace Utilities.SadConsole {
    public static class PointExtensions {
        public static SadRogue.Primitives.Point ToSadPoint(this Point point) => new SadRogue.Primitives.Point(point.X, point.Y);
        public static Point ToPoint(this SadRogue.Primitives.Point point) => new Point(point.X, point.Y);
    }
}
