using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.GameObjects;
using Rogue.Map;
using Rogue.MazeGenerator;
using RogueSharp;
using SadConsole;
using SadRogue.Primitives;
using Rectangle = SadRogue.Primitives.Rectangle;
using Point = SadRogue.Primitives.Point;

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
        private List<Room> Rooms;
        private readonly RoomDecorator roomDecorator;

        public MapGenerator(RoomDecorator roomDecorator, int width, int height, int roomAttempts, int roomMinWidth, int roomMaxWidth, int roomMinHeight, int roomMaxHeight) {
            Width = width;
            Height = height;
            RoomAttempts = roomAttempts;
            RoomMinWidth = roomMinWidth;
            RoomMaxWidth = roomMaxWidth;
            RoomMinHeight = roomMinHeight;
            RoomMaxHeight = roomMaxHeight;
            Rnd = new Random();
            this.roomDecorator = roomDecorator;
        }

        public RogueMap<MapCell> GenerateMap()
        {
            var map = new RogueMap<MapCell>(Width, Height);
            foreach(var cell in map.GetAllCells()) {
                cell.Type = CellType.Wall;
            }

            PlaceRooms(map);

            var maze = new MazeCarver(map);

            maze.CarveMaze(new SadRogue.Primitives.Point(0, 0), Direction.Up);

            ConnectRooms(map);
            DecorateRooms(map);
 
            return map;
        }

        private void DecorateRooms(RogueMap<MapCell> map) {
            roomDecorator.GetDecorations(Rooms, map.GameObjects, Rnd);
        }

        private void ConnectRooms(RogueMap<MapCell> map) {
            var connectorCandidates = new List<MapCell>();
            foreach (var cell in map.GetAllCells().Where(IsWall())) {
                if (IsConnectorCandidate(cell, map)) {
                    connectorCandidates.Add(cell);
                }
            }

            foreach (var room in Rooms) {
                var bounds = room.Bounds;
                var connectors = bounds.Positions()
                    .SelectMany(p => map.GetAdjacentCells(p.X, p.Y))
                    .Where(c => connectorCandidates.Contains(c))
                    .ToList();

                if (connectors.Any()) {
                    var connector = connectors[Rnd.Next(0, connectors.Count)];
                    
                    CreateDoor(map, bounds, connector);
                    if (connectors.Count > 1 && Rnd.Next(0, 2) == 1) {
                        connectors.Remove(connector);
                        var secondConnector = connectors[Rnd.Next(0, connectors.Count)];
                        if (map.GetAdjacentCells(secondConnector.X, secondConnector.Y).Any(HasDoor(map))) {
                            continue;
                        }
                        CreateDoor(map, bounds, secondConnector);
                    }
                }
                else {
                    // Remove non-connected room
                    // Todo: Fix the maze generation issue that causes this
                    bounds.Expand(2, 2).Positions()
                        .ToList()
                        .ForEach(p =>
                    {
                        var mapCell = map[p.X, p.Y];
                        mapCell.Type = CellType.Wall;
                        map.GameObjects.RemoveAll(g => g.Location == new Point(mapCell.X, mapCell.Y));
                    });
                }
            }

        }

        private static Func<MapCell, bool> HasDoor(RogueMap<MapCell> map) {
            return c => map.GameObjects.Any(g => g is Door && g.Location.X == c.X && g.Location.Y == c.Y);
        }

        private static Func<MapCell, bool> IsWall() {
            return c => c.Type == CellType.Wall || c.Type == CellType.RoomWallHorizontal || c.Type == CellType.RoomWallVertical;
        }

        private void CreateDoor(RogueMap<MapCell> map, Rectangle room, MapCell connector) {
            connector.Type = CellType.Maze;
            var door = new Door(
                new SadRogue.Primitives.Point(connector.X, connector.Y), 
                (connector.X == room.X - 1 || connector.X == room.MaxExtentX + 1) ? Orientation.Vertical : Orientation.Horizontal);

            map.GameObjects.Add(door);

            map.SetCellProperties(connector.X, connector.Y, false, true);
        }

        private bool IsConnectorCandidate(MapCell cell, Map<MapCell> map) {
            var adjacent = map.GetAdjacentCells(cell.X, cell.Y);
            if (adjacent.Any(adjacent => adjacent.Type == CellType.RoomFloor) 
                && adjacent.Any(a => a.Type == CellType.Maze)) {
                return true;
            }

            if (adjacent.Count(a => a.Type == CellType.RoomFloor) > 1) {
                return true;
            }

            return false;
        }

        private void PlaceRooms(RogueMap<MapCell> map)
        {
            Rooms = new List<Room>();

            for (int i = 0; i < RoomAttempts; i++) {
                var width = GetOddNumber(RoomMinWidth, RoomMaxWidth);
                var height = GetOddNumber(RoomMinHeight, RoomMaxHeight);
                int xPos = GetOddNumber(2, Width - width);
                var yPos = GetOddNumber(2, Height - height);

                Rectangle newRoom = new Rectangle(xPos, yPos, width, height);
                if (!OverLapsRoom(newRoom)) {
                    PlaceRoom(map, newRoom);
                    Rooms.Add(new Room(newRoom));
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
                var bounds = room.Bounds;
                if (newRoom.Expand(4, 4).Intersects(bounds))
                {
                    return true;
                }
            }
            return false;
        }

        private void PlaceRoom(RogueMap<MapCell> map, Rectangle room)
        {
            for (int x = room.X - 1; x < room.X + room.Width + 1; x++)
            {
                for (int y = room.Y - 1; y < room.Y + room.Height + 1; y++)
                {
                    if (!map.InBounds(new SadRogue.Primitives.Point(x, y))) {
                        continue;
                    }

                    if (y == room.Y - 1) {
                        map[x, y].Type = CellType.RoomWallHorizontal;
                    }
                    else if (y == room.Y + room.Height) {
                        map[x, y].Type = CellType.RoomWallHorizontal;
                    }
                    else if (x == room.X - 1 || x == room.X + room.Width) {
                        map[x, y].Type = CellType.RoomWallVertical;
                    }
                    else {
                        map.SetCellProperties(x, y, true, true);
                        map[x, y].Type = CellType.RoomFloor;
                    }
                }
            }
        }
    }
}
