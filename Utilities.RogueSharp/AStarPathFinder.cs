using Core;
using Core.Interfaces;
using Rogue.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.RogueSharp;

namespace Rogue {
    public class AStarPathFinder : IPathFinder {
        private AStarHacked<RogueSharp.Cell> implementation;
        public AStarPathFinder() {
            implementation = new AStarHacked<RogueSharp.Cell>();
        }

        public List<MapCell> FindPath(Point source, Point destination, IMap map, Func<ICell, ICell, bool> IsValidStep) {
            var rogueSharpSource = new RogueSharp.Cell(source.X, source.Y, true, true);
            var rogueSharpDestination = new RogueSharp.Cell(destination.X, destination.Y, true, true);
            var rogueSharpMap = map.ToRogueSharpMap();

            var path = implementation.FindPath(rogueSharpSource, rogueSharpDestination, rogueSharpMap, WrapIsValidStep(IsValidStep));
            return path.Select(c => new MapCell(new Point(c.X, c.Y), CellType.Maze, true, true)).ToList();
        }

        private static Func<RogueSharp.Cell, RogueSharp.Cell, bool> WrapIsValidStep(Func<ICell, ICell, bool> IsValidStep) {
            return (neighbor, destination) => { return IsValidStep(
                new MapCell(new Point(neighbor.X, neighbor.Y), CellType.Maze, neighbor.IsTransparent, neighbor.IsWalkable), 
                new MapCell(new Point(destination.X, destination.Y), CellType.Maze, destination.IsTransparent, destination.IsWalkable)); };
        }
    }
}
