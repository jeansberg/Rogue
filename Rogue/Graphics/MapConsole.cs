using Rogue.Actors;
using Rogue.MazeGenerator;
using RogueSharp;
using SadConsole;
using SadConsole.Input;
using System;

namespace Rogue.Graphics {
    public class MapConsole : SadConsole.Console {
        private readonly Map<MapCell> map;
        private readonly Player player;

        public MapConsole(Map<MapCell> map, Player player) : base(map.Width, map.Height + 1) {
            this.map = map;
            this.player = player;

            this.Position = new SadRogue.Primitives.Point(1, 1);
        }

        public override void Update(TimeSpan delta) {
            if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Down)) {
                MoveActor(Direction.South);
            }
            else
            if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Up)) {
                MoveActor(Direction.North);
            }
            else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Left)) {
                MoveActor(Direction.West);
            }
            else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Right)) {
                MoveActor(Direction.East);
            }

            this.Clear();
            DrawMap();
            DrawPlayer();

            base.Update(delta);
        }

        private void DrawPlayer() {
            Cursor.Position = new SadRogue.Primitives.Point(player.Location.X, player.Location.Y);
            Cursor.Print(player.ToString());
        }

        private void DrawMap() {
            Cursor.Position = new SadRogue.Primitives.Point(0, 0);

            foreach (var cell in map.GetAllCells()) {
                if (cell.Y < map.Height) {
                    DrawCell(cell);
                }
            }
        }

        private void DrawCell(MapCell cell) {
            var coloredString = new ColoredString(cell.ToString(), cell.Color, SadRogue.Primitives.Color.Black);
            Cursor.Print(coloredString);
        }

        private void MoveActor(Direction dir) {
            var newPoint = player.Location.Increment(dir);
            if (map.InBounds(newPoint) && map[newPoint.X, newPoint.Y].IsWalkable) {
                player.Location = newPoint;
            }
        }
    }
}
