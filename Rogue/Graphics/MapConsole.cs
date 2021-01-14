using Rogue.GameObjects;
using Rogue.MazeGenerator;
using RogueSharp;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using System.Linq;

namespace Rogue.Graphics {
    public class MapConsole : SadConsole.Console {
        private readonly RogueMap<MapCell> map;
        private readonly Player player;
        private InputState state;

        public MapConsole(RogueMap<MapCell> map, Player player) : base(map.Width, map.Height + 1) {
            this.map = map;
            this.player = player;
            state = InputState.Idle;

            this.Position = new SadRogue.Primitives.Point(1, 1);
        }

        public override void Update(TimeSpan delta) {
            if (state == InputState.Idle) {
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
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Space)) {
                    this.state = InputState.Targeting;
                }
            } else if (state == InputState.Targeting) {
                GameObject gameObject = null;
                bool handled = false;
                Direction.Types direction = Direction.None;
                if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Down)) {
                    gameObject = map.GameObjects.FirstOrDefault(g => g.Location == player.Location.Increment(Direction.Types.Down));
                    direction = Direction.Types.Down;
                    handled = true;
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Up)) {
                    gameObject = map.GameObjects.FirstOrDefault(g => g.Location == player.Location.Increment(Direction.Types.Up));
                    direction = Direction.Types.Up;
                    handled = true;
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Left)) {
                    gameObject = map.GameObjects.FirstOrDefault(g => g.Location == player.Location.Increment(Direction.Types.Left));
                    direction = Direction.Types.Left;
                    handled = true;
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Right)) {
                    gameObject = map.GameObjects.FirstOrDefault(g => g.Location == player.Location.Increment(Direction.Types.Right));
                    direction = Direction.Types.Right;
                    handled = true;
                }

                if (handled) {
                    state = InputState.Idle;
                    if (gameObject != null) {
                        var action = gameObject.GetAction(direction.Opposite());
                        action.Perform(map);
                    };
                }
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

                foreach(var gameObject in map.GameObjects.Where(g => g.Location == player.Location)) {
                    var action = gameObject.GetAction(dir.Type.Opposite());
                    action.Perform(map, true);
                }
            }
        }
    }
}
