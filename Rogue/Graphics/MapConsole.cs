using Rogue.GameObjects;
using Rogue.MazeGenerator;
using RogueSharp;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System;

namespace Rogue.Graphics {
    public class MapConsole : SadConsole.Console {
        private readonly RogueMap<MapCell> map;
        private readonly Player player;

        public MapConsole(RogueMap<MapCell> map, Player player) : base(map.Width, map.Height + 1) {
            this.map = map;
            this.player = player;

            this.Position = new SadRogue.Primitives.Point(1, 1);
        }

        public override void Update(TimeSpan delta) {
            if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Down)) {
                MoveActor(Direction.Down);
            }
            else
            if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Up)) {
                MoveActor(Direction.Up);
            }
            else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Left)) {
                MoveActor(Direction.Left);
            }
            else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Right)) {
                MoveActor(Direction.Right);
            }

            this.Clear();
            DrawMap();
            DrawObjects();
            DrawPlayer();

            base.Update(delta);
        }

        private void DrawObjects() {
            foreach(var gameObject in map.GameObjects) {
                var coloredString = new ColoredString(gameObject.ToString(), gameObject.Color, Color.Black);

                Cursor.Position = new SadRogue.Primitives.Point(gameObject.Location.X, gameObject.Location.Y);
                Cursor.Print(coloredString);
            }
        }

        private void DrawPlayer() {
            var coloredString = new ColoredString(player.ToString(), player.Color, Color.Black);

            Cursor.Position = new SadRogue.Primitives.Point(player.Location.X, player.Location.Y);
            Cursor.Print(coloredString);
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
