using Rogue.Display;
using Rogue.GameObjects;
using RogueSharp;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using Console = SadConsole.Console;

namespace Rogue.Graphics {
    public class MainConsole : Console {
        private readonly MapConsole mapConsole;
        private readonly LogConsole logConsole;
        private readonly MessageConsole messageConsole;
        private readonly Player player;
        private InputState state;

        public MainConsole(MapConsole mapConsole, LogConsole logConsole, MessageConsole messageConsole, Player player) : base(80, 40) {
            this.mapConsole = mapConsole;
            this.logConsole = logConsole;
            this.messageConsole = messageConsole;
            this.player = player;
        }

        public override void Update(TimeSpan delta) {
            if (state == InputState.Idle) {
                if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Down)) {
                    var action = mapConsole.MoveActor(player, Direction.Down);
                    if (action != null) {
                        logConsole.Log(action.ToString());
                    }
                }
                else
                if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Up)) {
                    var action = mapConsole.MoveActor(player, Direction.Up);
                    if (action != null) {
                        logConsole.Log(action.ToString());
                    }
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Left)) {
                    var action = mapConsole.MoveActor(player, Direction.Left);
                    if (action != null) {
                        logConsole.Log(action.ToString());
                    }
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Right)) {
                    var action = mapConsole.MoveActor(player, Direction.Right);
                    if (action != null) {
                        logConsole.Log(action.ToString());
                    }
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Space)) {
                    state = InputState.Targeting;
                }
            }
            else if (state == InputState.Targeting) {
                messageConsole.SetMessage("Select a target");

                Direction.Types direction = Direction.None;
                if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Down)) {
                    direction = Direction.Types.Down;
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Up)) {
                    direction = Direction.Types.Up;
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Left)) {
                    direction = Direction.Types.Left;
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Right)) {
                    direction = Direction.Types.Right;
                }

                if (direction != Direction.Types.None) {
                    var action = mapConsole.Act(player, direction);
                    if (action != null) {
                        logConsole.Log(action.ToString());
                    }

                    state = InputState.Idle;
                    messageConsole.SetMessage("");
                }
            }

            mapConsole.Update(player, delta);
            messageConsole.Update(delta);
            logConsole.Update(delta);
        }
    }
}
