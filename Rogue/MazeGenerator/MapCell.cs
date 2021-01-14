using RogueSharp;
using SadRogue.Primitives;
using System;

namespace Rogue.MazeGenerator {
    public enum CellType {
        Wall,
        RoomFloor,
        Maze,
        Connector,
        DoorVertical,
        DoorHorizontal,
        RoomWallVertical,
        RoomWallHorizontal
    }

    public class MapCell : Cell {
        public CellType Type { get; set; }
        public Color Color =>
            Type switch {
            CellType.Wall => Color.Gray,
            CellType.RoomFloor => Color.Black,
            CellType.Maze => Color.Gray,
            CellType.Connector => Color.Gray,
            CellType.DoorVertical => Color.Brown,
            CellType.DoorHorizontal => Color.Brown,
            CellType.RoomWallVertical => Color.Gray,
            CellType.RoomWallHorizontal => Color.Gray,
                _ => throw new NotImplementedException()
        };

        public override string ToString() => Type switch {
            CellType.Wall => "#",
            CellType.RoomFloor => " ",
            CellType.Maze => ".",
            CellType.Connector => "x",
            CellType.DoorVertical => "|",
            CellType.DoorHorizontal => "-",
            CellType.RoomWallVertical => "|",
            CellType.RoomWallHorizontal => "-",


            _ => throw new NotImplementedException()
        };
    }
}
