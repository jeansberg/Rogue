using RogueSharp;

namespace Utilities.RogueSharp {
    public static class MapExtensions {
        public static Map<Cell> ToRogueSharpMap(this Core.Interfaces.IMap map) {
            var rogueSharpMap = new Map<Cell>(map.Width, map.Height);
            for (var x = 0; x < map.Width; x++) {
                for (var y = 0; y < map.Height; y++) {
                    var cell = map.GetCellAt(new Core.Point(x, y));
                    var rogueCell = rogueSharpMap.GetCell(x, y);
                    rogueCell.IsTransparent = cell.IsTransparent;
                    rogueCell.IsWalkable = cell.IsWalkable;
                }
            }

            return rogueSharpMap;
        }
    }
}
