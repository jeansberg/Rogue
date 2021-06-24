using Core;
using System;
using System.Collections.Generic;

namespace Rogue.Map {
    public class MazeSection {
        public readonly List<Point> Points;
        public readonly ConsoleColor Color;
        public readonly bool startOfMaze;
        public List<Room> AdjacentRooms;

        public MazeSection(Point point, ConsoleColor color, bool startOfMaze = false) {
            this.Points = new List<Point> { point };
            this.Color = color;
            this.startOfMaze = startOfMaze;
            AdjacentRooms = new List<Room>();
        }
    }
}
