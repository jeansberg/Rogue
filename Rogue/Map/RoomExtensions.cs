using Core;

namespace Rogue.Map {
    public static class RoomExtensions {
        public static Point MidTop(this Room room) {
            return new Point(room.Bounds.Center().X, room.Bounds.Y);
        }

        public static Point MidLeft(this Room room) {
            return new Point(room.Bounds.X, room.Bounds.Center().Y);
        }

        public static Point MidBottom(this Room room) {
            return new Point(room.Bounds.Center().X, room.Bounds.Bottom);
        }

        public static Point MidRight(this Room room) {
            return new Point(room.Bounds.Right, room.Bounds.Center().Y);
        }
    }
}
