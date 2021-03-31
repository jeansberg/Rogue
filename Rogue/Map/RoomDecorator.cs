using Core;
using Core.GameObjects;
using Rogue.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Point = Core.Point;

namespace Rogue.Map {
    public class RoomDecorator {
        public List<GameObject> GetDecorations(List<Room> rooms, int level, Random rnd) {
            var decorations = new List<GameObject>();
            
            foreach(var room in rooms.Where(r => !r.IsEntrance && !r.IsExit)) {
                if (rnd.Next(2) == 1){
                    continue;
                }
                
                var size = room.Bounds.Width * room.Bounds.Height;
                
                if (size > 25) {
                    decorations.AddRange(GetDecorationsLargeRoom(room, level, rnd));
                } else if (size > 20) {
                    decorations.AddRange(GetDecorationsMediumRoom(room, level, rnd));
                } else {
                    decorations.AddRange(GetDecorationsSmallRoom(room));
                }
            }

            return decorations;
        }

        private List<GameObject> GetDecorationsLargeRoom(Room room, int level, Random rnd) {
            var midPoint = room.Bounds.Center();
            var table = new Table(midPoint);
            var chairLeft = new Chair(midPoint.Left());
            var chairRight = new Chair(midPoint.Right());

            return new List<GameObject> { table, chairLeft, chairRight };
        }

        private List<GameObject> GetDecorationsMediumRoom(Room room, int level, Random rnd) {
            var topLeftCorner = new Core.Point(room.Bounds.X, room.Bounds.Y);
            var chair = new Chair(topLeftCorner);

            var emptyPoint = room.GetRandomEmptyPoint(new List<Point> { chair.Location }, rnd);
            var weapon = SpawnWeapon(emptyPoint, level, rnd);

            return new List<GameObject> { chair, weapon };
        }

        private List<GameObject> GetDecorationsSmallRoom(Room room) {
            var doorSides = room.GetDoorSides();

            Chest chest1;
            Chest chest2;
            if (!doorSides.Contains(Direction.Up)) {
                chest1 = new Chest(room.MidTop().Left());
                chest2 = new Chest(room.MidTop().Right());
            }
            else if (!doorSides.Contains(Direction.Left)) {
                chest1 = new Chest(room.MidLeft().Up());
                chest2 = new Chest(room.MidLeft().Down());
            }
            else if (!doorSides.Contains(Direction.Down)) {
                chest1 = new Chest(room.MidBottom().Left());
                chest2 = new Chest(room.MidBottom().Right());
            }
            else {
                chest1 = new Chest(room.MidRight().Up());
                chest2 = new Chest(room.MidRight().Down());
            }

            return new List<GameObject> { chest1, chest2};
        }

        private Weapon SpawnWeapon(Point location, int level, Random rnd) {
            var weaponType = GetWeaponType(level, rnd);

            return new Weapon(location, weaponType);
        }

        private WeaponType GetWeaponType(int level, Random rnd) {
            if (level == 1) {
                return WeaponType.Mace;
            }
            else {
                var type = rnd.Next(0, 3);
                return (WeaponType)type;
            }
        }
    }
}
