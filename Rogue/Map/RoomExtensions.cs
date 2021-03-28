using Core;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static Point GetRandomEmptyPoint(this Room room, List<Point> occupiedPoints, Random rnd) {
            var points = room.Bounds.Points()
                .Select(p => new Point(p.X, p.Y)).Except(occupiedPoints)
                .ToList();

            return points[rnd.Next(0, points.Count())];
        }
    }
}
