using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static Rogue.MazeGenerator.Direction;
using Point = RogueSharp.Point;


namespace Rogue.MazeGenerator {

    public class Maze {
        private readonly Map<MapCell> map;
        private List<Point> visitedPoints;

        public Maze(Map<MapCell> map) {
            visitedPoints = new List<Point>();
            this.map = map;
        }

        public void CarveMaze(Point point) {
            visitedPoints.Add(point);
            map.SetCellProperties(point.X, point.Y, true, true);

            map[point.X, point.Y].Type = CellType.Maze;

            var directions = new List<Direction> { East, West, North, South }
                .OrderBy(x => Guid.NewGuid());

            foreach (var dir in directions) {
                var nextPoint = point.Increment(dir);
                if (IsValid(nextPoint, dir)) {
                    CarveMaze(nextPoint);
                }
            }
        }

        private bool IsValid(Point nextPoint, Direction dir) {
            if (visitedPoints.Contains(nextPoint)) {
                return false;
            }

            if (!map.InBounds(nextPoint)) {
                return false;
            }

            var nextNext = nextPoint.Increment(dir);

            // Don't break walls between corridors
            var adjacent = (dir switch {
                North =>
                        // Check E, W, N
                        new List<Point> {
                            nextNext.East(),
                            nextNext.West(),
                            nextNext.North()
                        },
                South =>
                        // Check E, W, S
                        new List<Point> {
                            nextNext.East(),
                            nextNext.West(),
                            nextNext.South()
                        },
                East =>
                        // Check N, E, S
                        new List<Point> {
                            nextNext.North(),
                            nextNext.East(),
                            nextNext.South()
                        },
                West =>
                        // Check W, N, S
                        new List<Point> {
                            nextNext.West(),
                            nextNext.North(),
                            nextNext.South(),
                        },
                _ => throw new NotImplementedException()
            }).Where(a => map.InBounds(a));

            if (adjacent.Any(x => map[x.X, x.Y].IsWalkable)) {
                return false;
            }

            return true;
        }
    }
}