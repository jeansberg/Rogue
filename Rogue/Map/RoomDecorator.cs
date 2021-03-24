using Core;
using Core.Interfaces;
using Rogue.GameObjects;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rogue.Map {
    public class RoomDecorator {
        public List<GameObject> GetDecorations(List<Room> rooms, System.Random rnd) {
            var decorations = new List<GameObject>();
            
            foreach(var room in rooms) {
                if (rnd.Next(2) == 1){
                    continue;
                }
                
                var size = room.Bounds.Width * room.Bounds.Height;
                
                if (size > 25) {
                    decorations.AddRange(GetDecorationsLargeRoom(room));
                } else if (size > 20) {
                    decorations.AddRange(GetDecorationsMediumRoom(room));
                } else {
                    decorations.AddRange(GetDecorationsSmallRoom(room));
                }
            }

            return decorations;
        }

        private List<GameObject> GetDecorationsLargeRoom(Room room) {
            var midPoint = room.Bounds.Center();
            var table = new Table(midPoint);
            var chairLeft = new Chair(midPoint.Left());
            var chairRight = new Chair(midPoint.Right());
            var sword = new Sword(new Core.Point(room.Bounds.X, room.Bounds.Bottom - 1));

            return new List<GameObject> { table, chairLeft, chairRight, sword };
        }

        private List<GameObject> GetDecorationsMediumRoom(Room room) {
            var topLeftCorner = new Core.Point(room.Bounds.X, room.Bounds.Y);
            var chair = new Chair(topLeftCorner);
            var sword = new Sword(new Core.Point(room.Bounds.X, room.Bounds.Bottom - 1));

            return new List<GameObject> { chair, sword };
        }

        private List<GameObject> GetDecorationsSmallRoom(Room room) {
            var midTopWall = new Point(room.Bounds.Center().X, room.Bounds.Y);
            var chestLeft = new Chest(midTopWall.Left());
            var chestRight = new Chest(midTopWall);

            return new List<GameObject> { chestLeft, chestRight };
        }
    }
}
