using Core.Interfaces;
using System;
using System.Drawing;

namespace Core {
    public class MapCell : ICell {
        public MapCell(Point location, CellType type, bool isTransparent, bool isWalkable) {
            Location = location;
            Type = type;
            IsTransparent = isTransparent;
            IsWalkable = isWalkable;
        }

        public Point Location { get; set; }
        public CellType Type { get; set; }
        public Color Color =>
             Type switch {
                 CellType.Wall => Color.Gray,
                 CellType.RoomFloor => Color.Gray,
                 CellType.Maze => Color.Gray,
                 CellType.Connector => Color.Gray,
                 CellType.DoorVertical => Color.SaddleBrown,
                 CellType.DoorHorizontal => Color.SaddleBrown,
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


        public bool IsTransparent { get; set; }

        public bool IsWalkable { get; set; }
        public bool IsDiscovered { get; set; }
    }
}
