using Rogue.Consoles;
using Rogue.GameObjects;
using SadConsole;
using SadConsole.Components;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Components {
    public class KeyboardHandler : InputConsoleComponent {
        private readonly MessageConsole messageConsole;
        private InputState state;
        private MapConsole mapConsole;
        private readonly LogConsole logConsole;
        private Actor player;
        private readonly InventoryConsole inventory;
        private List<Actor> actors;
        private List<Action> actions;
        private Timer turnTimer;

        public KeyboardHandler(MapConsole mapConsole, LogConsole logConsole, MessageConsole messageConsole, Actor player, InventoryConsole inventory, List<Actor> actors) {
            this.mapConsole = mapConsole;
            this.logConsole = logConsole;
            this.messageConsole = messageConsole;
            this.player = player;
            this.inventory = inventory;
            this.actors = actors;

            state = InputState.Idle;
            mapConsole.IsFocused = true;
            actions = new List<Action>();
        }

        public override void ProcessKeyboard(IScreenObject consoleObject, Keyboard info, out bool handled) {
            if (actions.Any()) {
                handled = true;
                return;
            }

            var playerMoved = false;
            if (state == InputState.Idle) {
                if (info.IsKeyPressed(Keys.Down)) {
                    MovePlayer(Direction.Down);
                    playerMoved = true;
                } else
                if (info.IsKeyPressed(Keys.Up)) {
                    MovePlayer(Direction.Up);
                    playerMoved = true;
                } else
                if (info.IsKeyPressed(Keys.Left)) {
                    MovePlayer(Direction.Left);
                    playerMoved = true;
                } else
                if (info.IsKeyPressed(Keys.Right)) {
                    MovePlayer(Direction.Right);
                    playerMoved = true;
                } else
                if (info.IsKeyPressed(Keys.Space)) {
                    state = InputState.Targeting;
                } else
                if (info.IsKeyPressed(Keys.I)) {
                    state = InputState.Inventory;
                    inventory.IsVisible = true;
                    mapConsole.IsVisible = false;
                    messageConsole.IsVisible = false;
                    logConsole.IsVisible = false;
                }
            }
            else if (state == InputState.Targeting) {
                messageConsole.SetMessage("Select a target");

                if (info.IsKeyPressed(Keys.Down)) {
                    Target(Direction.Down);
                    playerMoved = true;
                } else
                if (info.IsKeyPressed(Keys.Up)) {
                    Target(Direction.Up);
                    playerMoved = true;
                } else
                if (info.IsKeyPressed(Keys.Left)) {
                    Target(Direction.Left);
                    playerMoved = true;
                } else
                if (info.IsKeyPressed(Keys.Right)) {
                    Target(Direction.Right);
                    playerMoved = true;
                }
            }
            else if (state == InputState.Inventory) {
                if (info.IsKeyPressed(Keys.I)) {
                    ExitInventory();
                }
                else if (info.KeysPressed.Count > 0) {
                    var item = inventory.GetItem(info.KeysPressed[0].Key);
                    if (item != null) {
                        player.Weapon = item;
                        messageConsole.SetMessage($"{player.Name} Equipped {item.Name}");
                        ExitInventory();
                    }
                }

            }

            if (playerMoved) {
                UpdateActors();
            }

            handled = true;
        }

        public void Update(SadConsole.Console console, TimeSpan delta) {
            if (!actions.Any()) {
                return;
            }

            if (turnTimer == null || turnTimer.IsPaused) {
                turnTimer = new SadConsole.Components.Timer(TimeSpan.FromSeconds(0.2));

                turnTimer.TimerElapsed += (timer, e) =>
                {
                    var action = actions.First();
                    actions.Remove(action);
                    action();
                };
            }

            turnTimer.Update(console, delta);
        }

        private void ExitInventory() {
            state = InputState.Idle;
            mapConsole.IsVisible = true;
            messageConsole.IsVisible = true;
            logConsole.IsVisible = true;
            inventory.IsVisible = false;
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
                    if (result.Outcome == Actions.Outcome.Success) {
                        logConsole.Log(result.Message);
                        messageConsole.SetMessage("");
                    }
                    else {
                        messageConsole.SetMessage(result.Message);
                    }
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
                    actions.Add(() => { MoveTo(actor, player.Location); });
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

        public override void ProcessMouse(IScreenObject host, MouseScreenObjectState state, out bool handled) {
            var cellPosition = state.CellPosition + new Point(-1, -1);

            var actor = actors
                .SingleOrDefault(a => a.Location == cellPosition && player.Fov.IsInFov(a.Location.X, a.Location.Y));
            if (actor != null) {
                messageConsole.SetMessage(actor.Name);
            }

            handled = true;
        }
    }
}