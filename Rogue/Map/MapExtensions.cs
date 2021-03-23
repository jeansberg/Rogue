using Core;
using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Rogue {
    public static class MapExtensions {
        public static bool InBounds(this IMap map, Point p) =>
            p.X >= 0 && p.X < map.Width &&
            p.Y >= 0 && p.Y < map.Height;

        public static bool Occupied(this IMap map, Point p) => map.Actors.Any(a => a.Location == p && a.IsAlive);

        public static List<ICell> GetAdjacent(this IMap map, Point point) => 
            map.Cells().Where(x => map.InBounds(point) && x.Location.IsAdjacent(point)).ToList();
    }
}