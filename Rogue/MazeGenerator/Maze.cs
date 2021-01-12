using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using static SadRogue.Primitives.Direction;


namespace Rogue.MazeGenerator {

    public class Maze {
        private readonly RogueMap<MapCell> map;
        private List<Point> visitedPoints;

        public Maze(RogueMap<MapCell> map) {
            visitedPoints = new List<SadRogue.Primitives.Point>();
            this.map = map;
        }

        public void CarveMaze(Point point) {
            visitedPoints.Add(point);
            map.SetCellProperties(point.X, point.Y, true, true);

            map[point.X, point.Y].Type = CellType.Maze;

            var directions = new List<Direction> { Right, Left, Up, Down }
                .OrderBy(x => Guid.NewGuid());

            foreach (var dir in directions) {
                var nextPoint = point.Increment(dir);
                if (IsValid(nextPoint, dir)) {
                    CarveMaze(nextPoint);
                }
            }
        }

        private bool IsValid(Point nextPoint, Types dir) {
            if (visitedPoints.Contains(nextPoint)) {
                return false;
            }

            if (!map.InBounds(nextPoint)) {
                return false;
            }

            var nextNext = nextPoint.Increment(dir);

            // Don't break walls between corridors
            var adjacent = (dir switch {
                Types.Up =>
                        // Check E, W, N
                        new List<Point> {
                            nextNext.East(),
                            nextNext.West(),
                            nextNext.North()
                        },
                Types.Down =>
                        // Check E, W, S
                        new List<Point> {
                            nextNext.East(),
                            nextNext.West(),
                            nextNext.South()
                        },
                Types.Right=>
                        // Check N, E, S
                        new List<Point> {
                            nextNext.North(),
                            nextNext.East(),
                            nextNext.South()
                        },
                Types.Left=>
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