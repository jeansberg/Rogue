using Rogue.MazeGenerator;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Rogue {
    public class MapGenerator
    {
        private int Width;
        private int Height;
        private int RoomAttempts;
        private int RoomMinWidth;
        private int RoomMaxWidth;
        private int RoomMinHeight;
        private int RoomMaxHeight;

        public Random Rnd { get; }
        private List<Rectangle> Rooms;

        public MapGenerator() : this(60, 40, 30, 3, 9, 3, 9)
        {
        }

        public MapGenerator(int width, int height, int roomAttempts, int roomMinWidth, int roomMaxWidth, int roomMinHeight, int roomMaxHeight)
        {
            Width = width;
            Height = height;
            RoomAttempts = roomAttempts;
            RoomMinWidth = roomMinWidth;
            RoomMaxWidth = roomMaxWidth;
            RoomMinHeight = roomMinHeight;
            RoomMaxHeight = roomMaxHeight;
            Rnd = new Random();
        }

        public Map<MapCell> GenerateMap()
        {
            var map = new Map<MapCell>(Width, Height);
            foreach(var cell in map.GetAllCells()) {
                cell.Type = CellType.Wall;
            }
            ConsoleHelpers.DrawMap(map);

            PlaceRooms(map);
            //System.Console.ReadKey();

            var maze = new Maze(map);

            maze.CarveMaze(Point.Zero);
            //System.Console.ReadKey();

            ConnectRooms(map);
 
            return map;
        }

        private void ConnectRooms(Map<MapCell> map) {
            foreach(var cell in map.GetAllCells().Where(c => c.Type == CellType.Wall)) {
                if (BetweenMazeAndRoom(cell, map)) {
                    cell.Type = CellType.Connector;
                    ConsoleHelpers.DrawCell(cell);
                }
            }

            //System.Console.ReadKey();

            foreach (var room in Rooms)
            {
                var connectors = room.Points()
                    .SelectMany(p => map.GetAdjacentCells(p.X, p.Y)
                    .Where(c => c.Type == CellType.Connector))
                    .ToList();

                connectors.ForEach(c => {
                    c.Type = CellType.Wall;
                    ConsoleHelpers.DrawCell(c, ConsoleColor.DarkGray);
                });
                if (connectors.Any()) {
                    var connector = connectors[Rnd.Next(0, connectors.Count)];
                    connector.Type = (connector.X == room.Left - 1 || connector.X == room.Right) ? CellType.DoorVertical : CellType.DoorHorizontal;
                    ConsoleHelpers.DrawCell(connector, ConsoleColor.Red);
                    map.SetCellProperties(connector.X, connector.Y, false, true);
                }
                else {
                    // Remove non-connected room
                    // Todo: Fix this
                    room.Points().ForEach(p =>
                    {
                        var mapCell = map[p.X, p.Y];
                        mapCell.Type = CellType.Wall;
                        ConsoleHelpers.DrawCell(mapCell, ConsoleColor.DarkGray);
                    });
                }
            }

        }

        private bool BetweenMazeAndRoom(MapCell cell, Map<MapCell> map) {
            var adjacent = map.GetAdjacentCells(cell.X, cell.Y);
            if (adjacent.Any(adjacent => adjacent.Type == CellType.Room) 
                && adjacent.Any(a => a.Type == CellType.Maze)) {
                return true;
            }
            
            return false;
        }

        private void PlaceRooms(Map<MapCell> map)
        {
            Rooms = new List<Rectangle>();

            for (int i = 0; i < RoomAttempts; i++) {
                var width = GetOddNumber(RoomMinWidth, RoomMaxWidth);
                var height = GetOddNumber(RoomMinHeight, RoomMaxHeight);
                int xPos = GetOddNumber(1, Width - width);
                var yPos = GetOddNumber(1, Height - height);

                Rectangle newRoom = new Rectangle(xPos, yPos, width, height);
                if (!OverLapsRoom(newRoom)) {
                    PlaceRoom(map, newRoom);
                    Rooms.Add(newRoom);
                }
            }
        }

        private int GetOddNumber(int min, int max) {
            var number = Rnd.Next(min, max);

            var upOrDown = Rnd.Next(0, 2);

            number = upOrDown == 0 ? number++ : number--;

            return Math.Clamp(number, 0, max);
        }

        private bool OverLapsRoom(Rectangle newRoom)
        {
            foreach(var room in Rooms)
            {
                if (newRoom.Touches(room, 2) ||
                    room.Touches(newRoom, 2))
                {
                    return true;
                }
            }
            return false;
        }

        private void PlaceRoom(Map<MapCell> map, Rectangle room)
        {
            for(int x = room.X; x < room.X + room.Width; x++)
            {
                for (int y = room.Y; y < room.Y + room.Height; y++)
                {
                    map.SetCellProperties(x, y, true, true);
                    map[x, y].Type = CellType.Room;
                    ConsoleHelpers.DrawCell(map[x, y]);
                }
            }
        }
    }
}
