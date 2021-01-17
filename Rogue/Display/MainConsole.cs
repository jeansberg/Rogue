using Rogue.Display;
using Rogue.GameObjects;
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
                    Move(Direction.Down);
                }
                else
                if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Up)) {
                    Move(Direction.Up);
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Left)) {
                    Move(Direction.Left);
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Right)) {
                    Move(Direction.Right);
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Space)) {
                    state = InputState.Targeting;
                }
            }
            else if (state == InputState.Targeting) {
                messageConsole.SetMessage("Select a target");

                Direction.Types direction = Direction.None;
                if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Down)) {
                    Target(Direction.Down);
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Up)) {
                    Target(Direction.Up);
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Left)) {
                    Target(Direction.Left);
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Right)) {
                    Target(Direction.Right);
                }

            }

            mapConsole.Update(player, delta);
            messageConsole.Update(delta);
            logConsole.Update(delta);
        }

        private void Target(Direction.Types direction) {
            if (direction != Direction.Types.None) {
                var action = mapConsole.Act(player, direction);
                if (action != null) {
                    logConsole.Log(action.ToString());
                }

                state = InputState.Idle;
                messageConsole.SetMessage("");
            }
        }

        private void Move(Direction direction) {
            var action = mapConsole.MoveActor(player, direction);
            if (action != null) {
                logConsole.Log(action.ToString());
            }
        }
    }
}
