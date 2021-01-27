using Rogue.Consoles;
using Rogue.GameObjects;
using SadConsole;
using SadConsole.Components;
using SadConsole.Input;
using SadRogue.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Components {
    public class KeyboardHandler : KeyboardConsoleComponent {
        private readonly MessageConsole messageConsole;
        private InputState state;
        private MapConsole mapConsole;
        private readonly LogConsole logConsole;
        private Actor player;
        private List<Actor> actors;

        public KeyboardHandler(MapConsole mapConsole, LogConsole logConsole, MessageConsole messageConsole, Actor player, List<Actor> actors) {
            this.mapConsole = mapConsole;
            this.logConsole = logConsole;
            this.messageConsole = messageConsole;
            this.player = player;
            this.actors = actors;

            state = InputState.Idle;
        }

        public override void ProcessKeyboard(IScreenObject consoleObject, Keyboard info, out bool handled) {
            var playerMoved = false;
            if (state == InputState.Idle) {
                if (info.IsKeyPressed(Keys.Down)) {
                    MovePlayer(Direction.Down);
                    playerMoved = true;
                } else
                if (info.IsKeyPressed(Keys.Up)) {
                    MovePlayer(Direction.Up);
                    playerMoved = true;
                }
                if (info.IsKeyPressed(Keys.Left)) {
                    MovePlayer(Direction.Left);
                    playerMoved = true;
                }
                if (info.IsKeyPressed(Keys.Right)) {
                    MovePlayer(Direction.Right);
                    playerMoved = true;
                }
                if (info.IsKeyPressed(Keys.Space)) {
                    state = InputState.Targeting;
                }
            }
            else if (state == InputState.Targeting) {
                messageConsole.SetMessage("Select a target");

                if (info.IsKeyPressed(Keys.Down)) {
                    Target(Direction.Down);
                    playerMoved = true;
                }
                if (info.IsKeyPressed(Keys.Up)) {
                    Target(Direction.Up);
                    playerMoved = true;
                }
                if (info.IsKeyPressed(Keys.Left)) {
                    Target(Direction.Left);
                    playerMoved = true;
                }
                if (info.IsKeyPressed(Keys.Right)) {
                    Target(Direction.Right);
                    playerMoved = true;
                }
            }

            if (playerMoved) {
                UpdateActors();
            }

            handled = true;
        }

        private void MovePlayer(Direction direction) {
            messageConsole.SetMessage("");

            MoveOrAct(player, direction);
        }

        private void Target(Direction.Types direction) {
            if (direction != Direction.Types.None) {
                var action = mapConsole.GetAction(player, actors, direction);
                if (action != null) {
                    var result = action.Perform(player);
                    messageConsole.SetMessage(result.Message);
                }
                else {
                    messageConsole.SetMessage("Nothing to do");
                }

                state = InputState.Idle;
            }
        }

        private void UpdateActors() {
            foreach (var actor in actors.Where(a => a != player && a.IsAlive)) {
                actor.Fov.ComputeFov(actor.Location.X, actor.Location.Y, 5, true);
                if (player.IsAlive && actor.Fov.IsInFov(player.Location.X, player.Location.Y)) {
                    MoveTo(actor, player.Location);
                }
            }
        }

        private void MoveTo(Actor actor, Point target) {
            Point nextStep;

            bool isNextToTarget = actor.Location.IsAdjacent(target);
            if (!isNextToTarget) {
                nextStep = mapConsole.GetNextStep(actor, actors, target);
            }
            else {
                nextStep = target;
            }

            var direction = Direction.GetDirection(actor.Location, nextStep);

            MoveOrAct(actor, direction);
        }

        private void MoveOrAct(Actor actor, Direction direction) {
            var action = mapConsole.GetAction(actor, actors, direction);
            if (action != null) {
                var result = action.Perform(actor, true);
                if (result.Outcome != Actions.Outcome.Canceled) {
                    logConsole.Log(result.Message);
                }
                if (result.KeepMoving) {
                    mapConsole.MoveActor(actor, actors, direction);
                }
            }
            else {
                mapConsole.MoveActor(actor, actors, direction);
            }
        }
    }
}