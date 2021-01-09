using RogueSharp;
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
        public ConsoleColor Color { get; set; }

        public override string ToString() => Type switch {
            CellType.Wall => "#",
            CellType.Room => " ",
            CellType.Maze => ".",
            CellType.Connector => "x",
            CellType.DoorVertical => "|",
            CellType.DoorHorizontal => "—",

            _ => throw new NotImplementedException()
        };
    }
}
