﻿using Rogue.MazeGenerator;
using RogueSharp;

namespace Rogue {
    public static class MapExtensions {
        public static bool InBounds(this Map<MapCell> map, Point p) => 
            p.X >= 0 && p.X < map.Width &&
            p.Y >= 0 && p.Y < map.Height;
    }
}
