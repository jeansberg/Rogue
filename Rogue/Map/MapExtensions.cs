using Rogue.MazeGenerator;
using SadRogue.Primitives;
using System.Linq;

namespace Rogue {
    public static class MapExtensions {
        public static bool InBounds(this RogueMap<MapCell> map, Point p) =>
            p.X >= 0 && p.X < map.Width &&
            p.Y >= 0 && p.Y < map.Height;

        public static bool Occupied(this RogueMap<MapCell> map, Point p) => map.Actors.Any(g => g.Location == p && g.IsAlive);
    }
}