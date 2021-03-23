using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Rogue.MazeGenerator {

    public class MazeCarver {
        public readonly IMap map;
        private List<Point> visitedPoints;

        public MazeCarver(IMap map) {
            visitedPoints = new List<Point>();
            this.map = map;
        }

        public void CarveMaze(Point point, Direction direction) {
            visitedPoints.Add(point);
            map.SetWalkable(point, true);
            map.SetTransparent(point, true);

            map.GetCellAt(point).Type = CellType.Maze;

            var directions = new List<Direction> { Direction.Right, Direction.Left, Direction.Up, Direction.Down }
                .OrderBy(x => Guid.NewGuid());

            foreach (var dir in directions) {
                var nextPoint = point.Increment(dir);
                if (IsValid(nextPoint, dir)) {
                    CarveMaze(nextPoint, dir);
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
                Direction.Up =>
                        // Check E, W, N
                        new List<Point> {
                            nextNext.Right(),
                            nextNext.Left(),
                            nextNext.Up()
                        },
                Direction.Down =>
                        // Check E, W, S
                        new List<Point> {
                            nextNext.Right(),
                            nextNext.Left(),
                            nextNext.Down()
                        },
                Direction.Right=>
                        // Check N, E, S
                        new List<Point> {
                            nextNext.Up(),
                            nextNext.Right(),
                            nextNext.Down()
                        },
                Direction.Left=>
                        // Check W, N, S
                        new List<Point> {
                            nextNext.Left(),
                            nextNext.Up(),
                            nextNext.Down(),
                        },
                _ => throw new NotImplementedException()
            }).Where(a => map.InBounds(a));

            if (adjacent.Any(x => map.GetCellAt(x).IsWalkable)) {
                return false;
            }

            return true;
        }
    }
}