using Rogue.Actors;
using Rogue.MazeGenerator;
using RogueSharp;
using System;
using System.Collections.Generic;

namespace Rogue
{
    class Program
    {
        private static Map<MapCell> map;
        private static Player player;

        static void Main(string[] args)
        {
            var generator = new MapGenerator(30, 20, 30, 3, 6, 3, 6);

            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            System.Console.SetWindowSize(100, 50);
            System.Console.CursorVisible = false;
            map = generator.GenerateMap();

            player = new Player {
                Location = Point.Zero
            };

            var actors = new List<Actor> {
                player
            };

            while (true) {
                ConsoleHelpers.DrawMap(map);
                ConsoleHelpers.DrawActor(player);

                Update();
            }

            System.Console.ReadKey();
        }

        private static void Update() {
            var input = System.Console.ReadKey();
            if (input.Key == ConsoleKey.UpArrow) {
                MoveActor(Direction.North);
            } else if (input.Key == ConsoleKey.DownArrow) {
                MoveActor(Direction.South);
            }
            else if (input.Key == ConsoleKey.LeftArrow) {
                MoveActor(Direction.West);
            }
            else if (input.Key == ConsoleKey.RightArrow) {
                MoveActor(Direction.East);
            }

        }

        private static void MoveActor(Direction dir) {
            var newPoint = player.Location.Increment(dir);
            if (map.InBounds(newPoint) && map[newPoint.X, newPoint.Y].IsWalkable) {
                player.Location = newPoint;
            }
        }
    }
}
