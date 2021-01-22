using Rogue.Display;
using Rogue.GameObjects;
using SadConsole;
using SadConsole.Host;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using Console = SadConsole.Console;

namespace Rogue.Graphics {
    public class MainConsole : Console {
        private readonly MapConsole mapConsole;
        private readonly LogConsole logConsole;
        private readonly MessageConsole messageConsole;
        private readonly List<Actor> actors;
        private readonly Actor player;
        private InputState state;

        public MainConsole(MapConsole mapConsole, LogConsole logConsole, MessageConsole messageConsole, List<Actor> actors) : base(80, 40) {
            this.mapConsole = mapConsole;
            this.logConsole = logConsole;
            this.messageConsole = messageConsole;
            this.actors = actors;
            this.player = actors.Single(a => a is Player);
        }

        public override void Update(TimeSpan delta) {
            var playerMoved = UpdatePlayer();

            if (playerMoved) {
                UpdateActors();
            }

            mapConsole.Update(actors, delta);
            messageConsole.Update(delta);
            logConsole.Update(delta);
        }

        private void UpdateActors() {
            foreach (var actor in actors.Where(a => a != player)) {
                actor.Fov.ComputeFov(actor.Location.X, actor.Location.Y, 5, true);
                if (player.IsAlive && actor.Fov.IsInFov(player.Location.X, player.Location.Y)) {
                    MoveTo(actor, player.Location);
                }
            }
        }

        private bool UpdatePlayer() {
            var moved = false;

            if (state == InputState.Idle) {
                if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Down)) {
                    Move(Direction.Down);
                    moved = true;
                }
                else
                if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Up)) {
                    Move(Direction.Up);
                    moved = true;
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Left)) {
                    Move(Direction.Left);
                    moved = true;
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Right)) {
                    Move(Direction.Right);
                    moved = true;
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
                    moved = true;
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Up)) {
                    Target(Direction.Up);
                    moved = true;
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Left)) {
                    Target(Direction.Left);
                    moved = true;
                }
                else if (GameHost.Instance.Keyboard.IsKeyPressed(Keys.Right)) {
                    Target(Direction.Right);
                    moved = true;
                }
            }

            return moved;
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

        private void Move(Direction direction) {
            messageConsole.SetMessage("");

            var action = mapConsole.GetAction(player, actors, direction);
            if (action != null) {
                var result = action.Perform(player, true);
                if (result.Outcome != Actions.Outcome.Canceled) {
                    logConsole.Log(result.Message);
                }
                if (result.KeepMoving) {
                    mapConsole.MoveActor(player, direction);
                }
            }
            else {
                mapConsole.MoveActor(player, direction);
            }
        }

        private void MoveTo(Actor actor, Point target) {
            Point nextStep;

            if (!actor.Location.IsAdjacent(target)) {
                nextStep = mapConsole.NextStep(actor, target);
            }
            else {
                nextStep = target;
            }

            var direction = Direction.GetDirection(actor.Location, nextStep);

            var action = mapConsole.GetAction(actor, actors, direction);
            if (action != null) {
                var result = action.Perform(actor, true);
                if (result.Outcome != Actions.Outcome.Canceled) {
                    logConsole.Log(result.Message);
                }
                if (result.KeepMoving) {
                    mapConsole.MoveActor(actor, direction);
                }
            }
            else {
                mapConsole.MoveActor(actor, direction);
            }
        }
    }
}
