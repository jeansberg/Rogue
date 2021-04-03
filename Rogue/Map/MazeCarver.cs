using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Point = Core.Point;

namespace Rogue.MazeGenerator {

    public class MazeCarver {
        public readonly IMap map;
        private readonly Random rnd;
        private List<Point> visitedPoints;
        private List<(List<Point>, ConsoleColor)> Sections;

        public MazeCarver(Random rnd, IMap map) {
            this.map = map;
            this.rnd = rnd;
            Sections = new List<(List<Point>, ConsoleColor)>();
            visitedPoints = new List<Point>();
        }

        public void CarveMaze(Point point) {
            visitedPoints.Add(point);
            map.SetWalkable(point, true);
            map.SetTransparent(point, true);

            map.GetCellAt(point).Type = CellType.Maze;

            var directions = new List<Direction> { Direction.Right, Direction.Left, Direction.Up, Direction.Down }
                .OrderBy(x => Guid.NewGuid());

            var mazeSection = new List<Point> { point };
            var sectionColor = new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Green }[rnd.Next(0, 5)];
            Sections.Add((mazeSection, sectionColor));

            foreach (var dir in directions) {
                var nextPoint = point.Increment(dir);
                if (IsValid(nextPoint, dir)) {
                    mazeSection.Add(nextPoint);
                    CarveMaze(nextPoint);
                }
            }

            //Draw();
        }

        private void Draw() {
            foreach (var section in Sections) {
                foreach(var point in section.Item1) {
                    Console.SetCursorPosition(point.X, point.Y);
                    Console.ForegroundColor = section.Item2;
                    Console.Write(".");
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