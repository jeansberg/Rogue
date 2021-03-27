using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Core;
using Core.Interfaces;
using Rogue.GameObjects;
using Rogue.Map;
using Rogue.MazeGenerator;
using Utilities.RogueSharp;
using Point = Core.Point;

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
        private readonly int Level;

        public Random Rnd { get; }
        private List<Room> Rooms;

        private readonly RoomDecorator RoomDecorator;

        public MapGenerator(RoomDecorator roomDecorator, int width, int height, int roomAttempts, int roomMinWidth, int roomMaxWidth, int roomMinHeight, int roomMaxHeight, int level) {
            Width = width;
            Height = height;
            RoomAttempts = roomAttempts;
            RoomMinWidth = roomMinWidth;
            RoomMaxWidth = roomMaxWidth;
            RoomMinHeight = roomMinHeight;
            RoomMaxHeight = roomMaxHeight;
            Level = level;
            Rnd = new Random();
            RoomDecorator = roomDecorator;
        }

        public IMap GenerateMap()
        {
            var map = new RogueMap(Width, Height);

            PlaceRooms(map);

            var maze = new MazeCarver(map);

            maze.CarveMaze(new Point(0, 0), Direction.Up);

            ConnectRooms(map);
            DecorateRooms(map);
            PlaceMonsters(map);
 
            return map;
        }

        private void PlaceMonsters(IMap map) {
            foreach(var room in Rooms) {
                SpawnMonster(room, map);
            }
        }

        private void SpawnMonster(Room room, IMap map) {
            var monsterType = GetMonsterType();

            Point location;
            do {
                var x = Rnd.Next(room.Bounds.X, room.Bounds.Right);
                var y = Rnd.Next(room.Bounds.Y, room.Bounds.Bottom);
                location = new Point(x, y);
            }
            while (map.GameObjects.Any(g => g.Location == location));

            map.Actors
                .Add(new Monster(monsterType, location, new RogueSharpFov(map)));
        }

        private MonsterType GetMonsterType() {
            var inLevelRange = new List<MonsterType>();
            foreach(MonsterType type in Enum.GetValues(typeof(MonsterType))) {
                var range = Monster.DungeonLevelRange(type);
                if (Level >= range.MinLevel && Level <= range.MaxLevel) {
                    inLevelRange.Add(type);
                }
            }
            return inLevelRange[Rnd.Next(0, inLevelRange.Count)];
        }

        private void DecorateRooms(IMap map) {
            var decorations = RoomDecorator.GetDecorations(Rooms, Level, Rnd);
            map.GameObjects.AddRange(decorations);


        }

        private void ConnectRooms(IMap map) {
            var connectorCandidates = new List<ICell>();
            foreach (var cell in map.Cells().Where(IsWall())) {
                if (IsConnectorCandidate(cell, map)) {
                    connectorCandidates.Add(cell);
                }
            }

            var roomsToDelete = new List<Room>();
            foreach (var room in Rooms) {
                var bounds = room.Bounds;
                var connectors = bounds.Points()
                    .SelectMany(p => map.GetAdjacent(p.ToPoint()))
                    .Where(c => connectorCandidates.Contains(c))
                    .ToList();

                if (connectors.Any()) {
                    var connector = connectors[Rnd.Next(0, connectors.Count)];
                    
                    CreateDoor(map, bounds, connector);
                    if (connectors.Count > 1 && Rnd.Next(0, 2) == 1) {
                        connectors.Remove(connector);
                        var secondConnector = connectors[Rnd.Next(0, connectors.Count)];
                        if (map.GetAdjacent(secondConnector.Location).Any(HasDoor(map))) {
                            continue;
                        }
                        CreateDoor(map, bounds, secondConnector);
                    }
                }
                else {
                    // Remove non-connected room
                    // Todo: Fix the maze generation issue that causes this
                    roomsToDelete.Add(room);

                    Rectangle.Inflate(bounds, 2, 2).Points()
                        .Select(x => x.ToPoint())
                        .ToList()
                        .ForEach(p => {
                            if (map.InBounds(p)) {
                                var mapCell = map.GetCellAt(p);
                                mapCell.Type = CellType.Wall;
                                map.SetTransparent(p, false);
                                map.SetWalkable(p, false);

                                map.GameObjects.RemoveAll(g => g.Location == new Point(mapCell.Location.X, mapCell.Location.Y));
                        }
                    });
                }
            }
            Rooms.RemoveAll(r => roomsToDelete.Contains(r));
        }

        private static Func<ICell, bool> HasDoor(IMap map) {
            return c => map.GameObjects.Any(g => g is Door && g.Location.X == c.Location.X && g.Location.Y == c.Location.Y);
        }

        private static Func<ICell, bool> IsWall() {
            return c => c.Type == CellType.Wall || c.Type == CellType.RoomWallHorizontal || c.Type == CellType.RoomWallVertical;
        }

        private void CreateDoor(IMap map, Rectangle room, ICell connector) {
            connector.Type = CellType.Maze;
            var orientation = (connector.Location.X == room.X - 1 || connector.Location.X == room.Right) ? Orientation.Vertical : Orientation.Horizontal;
            var door = new Door(
                connector.Location, 
                orientation);

            map.GameObjects.Add(door);

            map.SetWalkable(connector.Location, true);
            map.SetTransparent(connector.Location, false);
        }

        private bool IsConnectorCandidate(ICell cell, IMap map) {
            var adjacent = map.GetAdjacent(cell.Location);
            if (adjacent.Any(adjacent => adjacent.Type == CellType.RoomFloor) 
                && adjacent.Any(a => a.Type == CellType.Maze)) {
                return true;
            }

            if (adjacent.Count(a => a.Type == CellType.RoomFloor) > 1) {
                return true;
            }

            return false;
        }

        private void PlaceRooms(IMap map)
        {
            Rooms = new List<Room>();

            for (int i = 0; i < RoomAttempts; i++) {
                var width = GetOddNumber(RoomMinWidth, RoomMaxWidth);
                var height = GetOddNumber(RoomMinHeight, RoomMaxHeight);
                int xPos = GetOddNumber(1, Width - width - 2);
                var yPos = GetOddNumber(1, Height - height);

                Rectangle newRoom = new Rectangle(xPos, yPos, width, height);
                if (!OverLapsRoom(newRoom)) {
                    PlaceRoom(map, newRoom);
                    Rooms.Add(new Room(newRoom));
                }
            }
        }

        private int GetOddNumber(int min, int max) {
            if (min % 2 == 0 ||max % 2 == 0) {
                throw new Exception("Min and max size should be odd numbers.");
            }

            var number = Rnd.Next(min, max + 1);

            var upOrDown = Rnd.Next(0, 2);

            number = number % 2 == 0 ? 
                (upOrDown == 0 ? number + 1 : number - 1) :
                number;

            if (number < min) {
                return min;
            }

            if (number > max) {
                return max;
            }

            return number;
        }

        private bool OverLapsRoom(Rectangle newRoom)
        {
            foreach(var room in Rooms)
            {
                var bounds = room.Bounds;
                if (Rectangle.Inflate(newRoom, 4, 4).IntersectsWith(bounds))
                {
                    return true;
                }
            }
            return false;
        }

        private void PlaceRoom(IMap map, Rectangle room)
        {
            for (int x = room.X - 1; x < room.X + room.Width + 1; x++)
            {
                for (int y = room.Y - 1; y < room.Y + room.Height + 1; y++)
                {
                    var point = new Point(x, y);
                    if (!map.InBounds(point)) {
                        continue;
                    }

                    if (y == room.Y - 1) {
                        map.GetCellAt(point).Type = CellType.RoomWallHorizontal;
                    }
                    else if (y == room.Y + room.Height) {
                        map.GetCellAt(point).Type = CellType.RoomWallHorizontal;
                    }
                    else if (x == room.X - 1 || x == room.X + room.Width) {
                        map.GetCellAt(point).Type = CellType.RoomWallVertical;
                    }
                    else {
                        map.SetWalkable(point, true);
                        map.SetTransparent(point, true); map.GetCellAt(new Point(x, y)).Type = CellType.RoomFloor;
                    }
                }
            }
        }
    }
}
