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

        public void CarveMaze(Point point) {
            visitedPoints.Add(point);
            map.SetWalkable(point, true);
            map.SetTransparent(point, true);

            map.GetCellAt(point).Type = CellType.Maze;

            var directions = new List<Direction> { Direction.Right, Direction.Left, Direction.Up, Direction.Down }
                .OrderBy(x => Guid.NewGuid());

            foreach (var dir in directions) {
                var nextPoint = point.Increment(dir);
                if (IsValid(nextPoint, dir)) {
                    CarveMaze(nextPoint);
                }
            }
        }

        private bool IsValid(Point point, Direction dir) {
            if (!map.InBounds(point)) {
                return false;
            }
            
            if (map.GetCellAt(point).Type != CellType.Wall) {
                return false;
            }

            if (visitedPoints.Contains(point)) {
                return false;
            }

            //var point = point.Increment(dir);

            // Don't break walls between corridors
            var adjacent = (dir switch {
                Direction.Up =>
                        // Check E, W, N
                        new List<Point> {
                            point.Right(),
                            point.Left(),
                            point.Up()
                        },
                Direction.Down =>
                        // Check E, W, S
                        new List<Point> {
                            point.Right(),
                            point.Left(),
                            point.Down()
                        },
                Direction.Right=>
                        // Check N, E, S
                        new List<Point> {
                            point.Up(),
                            point.Right(),
                            point.Down()
                        },
                Direction.Left=>
                        // Check W, N, S
                        new List<Point> {
                            point.Left(),
                            point.Up(),
                            point.Down(),
                        },
                _ => throw new NotImplementedException()
            }).Where(a => map.InBounds(a));

            if (adjacent.Any(x => map.GetCellAt(x).Type == CellType.Maze || map.GetCellAt(x).Type == CellType.RoomFloor)) {
                return false;
            }

            return true;
        }
    }
}