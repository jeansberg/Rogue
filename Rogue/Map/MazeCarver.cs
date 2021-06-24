using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Point = Core.Point;

namespace Rogue.Map {

    public class MazeCarver {
        public readonly IMap map;
        private readonly Random rnd;
        private List<Point> visitedPoints;
        private List<MazeSection> Sections;

        public MazeCarver(Random rnd, IMap map) {
            this.map = map;
            this.rnd = rnd;
            Sections = new List<MazeSection>();
            visitedPoints = new List<Point>();
        }

        public void CarveMaze(Point startingPoint) {
            var section = new MazeSection(startingPoint, ConsoleColor.White, true);

            Sections.Add(section);

            Carve(startingPoint, section);
            CleanUpDeadEnds();
        }

        private void CleanUpDeadEnds() {
            //Draw();
            var deadEndSections = Sections.Where(s => s.AdjacentRooms.Count < 2 && !s.startOfMaze)
                .ToList();

            //Draw(deadEndSections);

            foreach (var section in deadEndSections) {
                foreach (var point in section.Points) {
                    map.SetWalkable(point, false);
                    map.SetTransparent(point, false);

                    map.GetCellAt(point).Type = CellType.Wall;
                }
            }
            //Draw();
        }

        private void Carve(Point point, MazeSection? section) {
            visitedPoints.Add(point);
            map.SetWalkable(point, true);
            map.SetTransparent(point, true);

            map.GetCellAt(point).Type = CellType.Maze;

            if (section == null) {
                var sectionColor = new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Green }[rnd.Next(0, 5)];
                
                section = new MazeSection(point, ConsoleColor.DarkGray);

                Sections.Add(section);
            }

            var directions = new List<Direction> { Direction.Right, Direction.Left, Direction.Up, Direction.Down }
                .OrderBy(x => Guid.NewGuid());

            foreach (var dir in directions) {
                var nextPoint = point.Increment(dir);
                if (IsValid(nextPoint, dir, out var foundRoom)) {
                    section.Points.Add(nextPoint);
                    var adjacentRooms = GetAdjacentRooms(point);
                    section.AdjacentRooms = section.AdjacentRooms.Union(adjacentRooms)
                        .ToList();
                    if (section.AdjacentRooms.Count > 2) {
                        Carve(nextPoint, null);
                    }
                    else {
                        Carve(nextPoint, section);
                    }
                }
            }

            //Draw();
        }

        private List<Room> GetAdjacentRooms(Point point) {
            return map.Rooms.Where(r => Rectangle.Inflate(r.Bounds, 3, 3).Contains(new System.Drawing.Point(point.X, point.Y)))
                .ToList();
        }

        private void Draw(List<MazeSection> highLighted) {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            for (var x = 0; x < map.Width; x++) {
                for (var y = 0; y < map.Height; y++) {
                    Console.SetCursorPosition(x, y);
                    var type = map.GetCellAt(new Point(x, y)).Type;
                    if (type == CellType.Wall) {
                        Console.Write("X");
                    }
                    else if (type == CellType.RoomFloor) {
                        Console.Write(".");
                    }
                    else if (type == CellType.RoomWallHorizontal) {
                        Console.Write("-");
                    }
                    else if (type == CellType.RoomWallVertical) {
                        Console.Write("|");
                    }
                }
            }
            foreach (var section in Sections) {
                var color = section.Color;
                if (highLighted.Contains(section)) {
                    color = ConsoleColor.Green;
                }
                foreach (var point in section.Points) {
                    Console.SetCursorPosition(point.X, point.Y);
                    Console.ForegroundColor = color;
                    Console.Write(".");
                }
            }
        }

        private bool IsValid(Point point, Direction dir, out bool foundRoom) {
            foundRoom = false;

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
                Direction.Right =>
                        // Check N, E, S
                        new List<Point> {
                            point.Up(),
                            point.Right(),
                            point.Down()
                        },
                Direction.Left =>
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

            var hasAdjacentRoomWall = adjacent.Count(x => map.GetCellAt(x).Type == CellType.RoomWallHorizontal || map.GetCellAt(x).Type == CellType.RoomWallVertical) > 0;

            if (hasAdjacentRoomWall) {
                var adjacentRoomWall = adjacent.Single(x => map.GetCellAt(x).Type == CellType.RoomWallHorizontal || map.GetCellAt(x).Type == CellType.RoomWallVertical);

                var adjacentToRoomWall = new List<Point> {
                    adjacentRoomWall.Left(),
                    adjacentRoomWall.Right(),
                    adjacentRoomWall.Up(),
                    adjacentRoomWall.Down()
                }.Where(a => map.InBounds(a));
                var isCorner =
                    adjacentToRoomWall.Any(x => map.GetCellAt(x).Type == CellType.RoomWallHorizontal) &&
                    adjacentToRoomWall.Any(x => map.GetCellAt(x).Type == CellType.RoomWallVertical);
                if (!isCorner) {
                    foundRoom = true;
                }
            }

            return true;
        }
    }
}