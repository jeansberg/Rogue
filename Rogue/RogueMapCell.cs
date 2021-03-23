using Core;
using RogueSharp;
using SadRogue.Primitives;
using System;

namespace Rogue.MazeGenerator {


    public class RogueMapCell : Cell {
        public CellType Type { get; set; }
        public Color Color =>
            Type switch {
            CellType.Wall => Color.Gray,
            CellType.RoomFloor => Color.Gray,
            CellType.Maze => Color.Gray,
            CellType.Connector => Color.Gray,
            CellType.DoorVertical => Color.Brown,
            CellType.DoorHorizontal => Color.Brown,
            CellType.RoomWallVertical => Color.Gray,
            CellType.RoomWallHorizontal => Color.Gray,
                _ => throw new NotImplementedException()
        };

        public int GlyphId => Type switch {
            CellType.Wall => 254,
            CellType.RoomFloor => 46,
            CellType.Maze => 46,
            CellType.Connector => 120,
            CellType.RoomWallVertical => 186,
            CellType.RoomWallHorizontal => 205,


            _ => throw new NotImplementedException()
        };

        public bool Discovered { get; set; }
    }
}
