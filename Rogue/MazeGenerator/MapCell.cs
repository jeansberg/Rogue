using RogueSharp;
using SadRogue.Primitives;
using System;

namespace Rogue.MazeGenerator {
    public enum CellType {
        Wall,
        Room,
        Maze,
        Connector,
        DoorVertical,
        DoorHorizontal
    }

    public class MapCell : Cell {
        public CellType Type { get; set; }
        public Color Color =>
            Type switch {
            CellType.Wall => Color.Gray,
            CellType.Room => Color.Black,
            CellType.Maze => Color.Gray,
            CellType.Connector => Color.Gray,
            CellType.DoorVertical => Color.Brown,
            CellType.DoorHorizontal => Color.Brown,

            _ => throw new NotImplementedException()
        };

        public override string ToString() => Type switch {
            CellType.Wall => "#",
            CellType.Room => " ",
            CellType.Maze => ".",
            CellType.Connector => "x",
            CellType.DoorVertical => "|",
            CellType.DoorHorizontal => "-",

            _ => throw new NotImplementedException()
        };
    }
}
