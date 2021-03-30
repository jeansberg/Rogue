using Core;
using Core.Interfaces;
using Rogue.GameObjects;
using Rogue.Map;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue {
    public class RogueMap : IMap {
        private List<ICell> cells;
        public RogueMap(int width, int height, IMap? previousMap) {
            Width = width;
            Height = height;
            PreviousMap = previousMap;
            cells = new List<ICell>();
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    cells.Add(new MapCell(new Point(x, y), CellType.Wall, false, false));
                }
            }

            GameObjects = new List<GameObject>();
            Actors = new List<Actor>();
            Rooms = new List<Room>();
        }

        public List<Actor> Actors { get; set; }

        public List<ICell> Cells() {
            return cells;
        }

        public List<GameObject> GameObjects { get; set; }

        public ICell GetCellAt(Point point) {
            return cells[point.Y * Width + point.X];
        }

        public int Height { get; set; }
        public IMap? PreviousMap { get; }

        public bool IsDiscovered(Point point) {
            return GetCellAt(point).IsDiscovered;
        }

        public bool IsInBounds(Point point) {
            return point.X >= 0 && point.X < Width &&
            point.Y >= 0 && point.Y < Height;
        }

        public bool IsTransparent(Point point) {
            return GetCellAt(point).IsTransparent;
        }

        public bool IsWalkable(Point point) {
            return GetCellAt(point).IsWalkable;
        }

        public void RemoveGameObject(GameObject gameObject) {
            GameObjects.Remove(gameObject);
        }

        public void SetDiscovered(Point point) {
            GetCellAt(point).IsDiscovered = true;
        }

        public void SetTransparent(Point point, bool transparent) {
            GetCellAt(point).IsTransparent = transparent;
        }

        public void SetWalkable(Point point, bool walkable) {
            GetCellAt(point).IsWalkable = walkable;
        }

        public int Width { get; set; }
        public int Level { get; set; }

        public List<Room> Rooms { get; set; }

        public override string ToString() {
            var builder = new StringBuilder();
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++){
                    var cell = GetCellAt(new Point(x, y));
                    var cellCharacter = cell.Type switch {
                        CellType.Wall => "#",
                        CellType.RoomFloor => ".",
                        CellType.Maze => ".",
                        CellType.Connector => "X",
                        CellType.RoomWallVertical => "|",
                        CellType.RoomWallHorizontal => "-",
                        CellType.StairCaseDown => ">",
                        CellType.StairCaseUp => "<",
                        _ => throw new System.NotImplementedException(),
                    };

                    var gameObjectCharacter = GameObjects.SingleOrDefault(g => g is Door && g.Location == new Point(x, y));

                    builder.Append(gameObjectCharacter == null ? cellCharacter : "X");
                    if (x == Width - 1)
                        builder.AppendLine();
                }
            }

            return builder.ToString();
        }
    }
}

