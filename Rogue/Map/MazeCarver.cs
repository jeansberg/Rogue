using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using static SadRogue.Primitives.Direction;


namespace Rogue.MazeGenerator {

    public class MazeCarver {
        private readonly RogueMap<MapCell> map;
        private List<Point> visitedPoints;

        public MazeCarver(RogueMap<MapCell> map) {
            visitedPoints = new List<SadRogue.Primitives.Point>();
            this.map = map;
        }

        public void CarveMaze(Point point, Direction direction) {
            visitedPoints.Add(point);
            map.SetCellProperties(point.X, point.Y, true, true);

            map[point.X, point.Y].Type = CellType.Maze;

            var directions = new List<Direction> { Right, Left, Up, Down }
                .OrderBy(x => Guid.NewGuid());

            foreach (var dir in directions) {
                var nextPoint = point.Increment(dir);
                if (IsValid(nextPoint, dir)) {
                    CarveMaze(nextPoint, dir);
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
                            nextNext.Right(),
                            nextNext.Left(),
                            nextNext.Up()
                        },
                Types.Down =>
                        // Check E, W, S
                        new List<Point> {
                            nextNext.Right(),
                            nextNext.Left(),
                            nextNext.Down()
                        },
                Types.Right=>
                        // Check N, E, S
                        new List<Point> {
                            nextNext.Up(),
                            nextNext.Right(),
                            nextNext.Down()
                        },
                Types.Left=>
                        // Check W, N, S
                        new List<Point> {
                            nextNext.Left(),
                            nextNext.Up(),
                            nextNext.Down(),
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