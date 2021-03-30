using Core;
using System.Collections.Generic;
using System.Drawing;
using Point = Core.Point;

namespace Rogue.Map {
    public class Room {
        public List<Point> DoorLocations { get; set; } = new List<Point>();

        public Room(Rectangle bounds, bool isEntrance, bool isExit) {
            Bounds = bounds;
            IsExit = isExit;
            IsEntrance = isEntrance;
        }

        public Rectangle Bounds { get; private set; }
        public bool IsExit { get; }
        public bool IsEntrance { get; }

        public List<Direction> GetDoorSides() {
            var sides = new List<Direction>();
            foreach (var location in DoorLocations) {
                if (location.X == Bounds.X - 1) {
                    sides.Add(Direction.Left);
                }
                else if (location.X == Bounds.Right) {
                    sides.Add(Direction.Right);
                }
                else if (location.Y == Bounds.Y - 1) {
                    sides.Add(Direction.Up);
                }
                else {
                    sides.Add(Direction.Down);
                }
            }
            return sides;
        }
    }
}
