using Rogue.MazeGenerator;
using RogueSharp;
using System;
using System.Threading;

namespace Rogue {
    public static class ConsoleHelpers {
        public static void DrawMap(Map<MapCell> map) {
            Console.Clear();
            Console.WriteLine(map);
        }

        public static void DrawCell(MapCell cell, ConsoleColor color = ConsoleColor.Gray, bool sleep = true) {
            Console.SetCursorPosition(cell.X, cell.Y);
            Console.ForegroundColor = color;
            Console.Write(cell);
            Console.ForegroundColor = ConsoleColor.Gray;
            Thread.Sleep(20);
        }
    }
}
