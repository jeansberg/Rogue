using Rogue.GameObjects;
using SadRogue.Primitives;
using System.Collections.Generic;

namespace Rogue.Map {
    public class RoomDecorator {
        public void GetDecorations(List<Room> rooms, List<GameObject> gameObjects, System.Random rnd) {
            var decorations = new List<GameObject>();
            
            foreach(var room in rooms) {
                if (rnd.Next(2) == 1){
                    continue;
                }
                
                var size = room.Bounds.Width * room.Bounds.Height;
                
                if (size > 25) {
                    decorations.AddRange(GetDecorationsLargeRoom(room, gameObjects));
                } else if (size > 20) {
                    decorations.AddRange(GetDecorationsMediumRoom(room, gameObjects));
                } else {
                    decorations.AddRange(GetDecorationsSmallRoom(room, gameObjects));
                }
            }

            gameObjects.AddRange(decorations);
        }

        private List<GameObject> GetDecorationsLargeRoom(Room room, List<GameObject> gameObjects) {
            var midPoint = room.Bounds.Center;
            var table = new Table(midPoint);
            var chairLeft = new Chair(midPoint.Left());
            var chairRight = new Chair(midPoint.Right());
            var sword = new Sword(new Point(room.Bounds.X, room.Bounds.MaxExtentY));


            return new List<GameObject> { table, chairLeft, chairRight, sword };
        }

        private List<GameObject> GetDecorationsMediumRoom(Room room, List<GameObject> gameObjects) {
            var topLeftCorner = new Point(room.Bounds.X, room.Bounds.Y);
            var chair = new Chair(topLeftCorner);
            var sword = new Sword(new Point(room.Bounds.X, room.Bounds.MaxExtentY));

            return new List<GameObject> { chair, sword };
        }

        private List<GameObject> GetDecorationsSmallRoom(Room room, List<GameObject> gameObjects) {
            var midTopWall = new Point(room.Bounds.Center.X, room.Bounds.Y);
            var chestLeft = new Chest(midTopWall.Left());
            var chestRight = new Chest(midTopWall.Right());

            return new List<GameObject> { chestLeft, chestRight };
        }
    }
}
