using Core;
using Rogue.Consoles;
using Rogue.GameObjects;
using Rogue.Services;
using SadConsole;
using SadConsole.Components;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.SadConsole;

namespace Rogue.Components {
    public class KeyboardHandler : InputConsoleComponent {
        private readonly MessageConsole messageConsole;
        private InputState state;
        private MapConsole mapConsole;
        private readonly LogConsole logConsole;
        private Actor player;
        private readonly InventoryConsole inventory;
        private List<Actor> actors;
        private readonly Action startGame;
        private List<Action> actions;
        private Timer turnTimer;
        private List<Point> trajectory;

        public KeyboardHandler(MapConsole mapConsole, LogConsole logConsole, MessageConsole messageConsole, Actor player, InventoryConsole inventory, List<Actor> actors, Action startGame) {
            this.mapConsole = mapConsole;
            this.logConsole = logConsole;
            this.messageConsole = messageConsole;
            this.player = player;
            this.inventory = inventory;
            this.actors = actors;
            this.startGame = startGame;
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
                }
                else if (info.IsKeyPressed(Keys.Up)) {
                    MovePlayer(Direction.Up);
                    playerMoved = true;
                }
                else if (info.IsKeyPressed(Keys.Left)) {
                    MovePlayer(Direction.Left);
                    playerMoved = true;
                }
                else if (info.IsKeyPressed(Keys.Right)) {
                    MovePlayer(Direction.Right);
                    playerMoved = true;
                }
                else if (info.IsKeyPressed(Keys.Space)) {
                    state = InputState.TargetingDirection;
                }
                else if (info.IsKeyPressed(Keys.T)) {
                    state = InputState.Targeting;
                }
                else if (info.IsKeyPressed(Keys.I)) {
                    state = InputState.Inventory;
                    inventory.IsVisible = true;
                    mapConsole.IsVisible = false;
                    messageConsole.IsVisible = false;
                    logConsole.IsVisible = false;
                }
            }
            else if (state == InputState.TargetingDirection) {
                messageConsole.SetMessage("Which direction?");

                if (info.IsKeyPressed(Keys.Down)) {
                    TargetDirection(Direction.Down);
                    playerMoved = true;
                }
                else
                if (info.IsKeyPressed(Keys.Up)) {
                    TargetDirection(Direction.Up);
                    playerMoved = true;
                }
                else
                if (info.IsKeyPressed(Keys.Left)) {
                    TargetDirection(Direction.Left);
                    playerMoved = true;
                }
                else
                if (info.IsKeyPressed(Keys.Right)) {
                    TargetDirection(Direction.Right);
                    playerMoved = true;
                }
            }
            else if (state == InputState.Targeting) {
                messageConsole.SetMessage("Select a target");
                mapConsole.overlayPoints = new List<Point>();

                if (info.IsKeyPressed(Keys.T)) {
                    messageConsole.SetMessage("");
                    state = InputState.Idle;
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
            if (player.Health < 1) {
                startGame();
            }

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

        private void TargetDirection(Direction direction) {
            if (direction != Direction.None) {
                var actions = mapConsole.GetAction(player, actors, direction);
                if (actions.Count == 0) {
                        messageConsole.SetMessage("Nothing to do");
                    }
                foreach(var action in actions) {
                        var result = action.Perform(player);
                        if (result.Outcome == Actions.Outcome.Success) {
                            logConsole.Log(result.Message);
                            messageConsole.SetMessage("");
                        }
                        else {
                            messageConsole.SetMessage(result.Message);
                        }
                    }
                }

                state = InputState.Idle;
        }
        

        private void FireMissile(List<Point> trajectory) {
            Locator.Audio.PlaySound("missile");
            var missile = new Missile(trajectory.First());
            mapConsole.map.GameObjects.Add(missile);
            foreach(var point in trajectory) {
                actions.Add(() => { MoveMissile(missile, point); });
            }
        }

        private void MoveMissile(Missile missile, Point point) {
            missile.Location = point;

            actors.FirstOrDefault(a => a.Location == point)?.GetAction(mapConsole.map, Direction.None).Perform(player);
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

            var direction = DirectionExtensions.GetDirection(actor.Location, nextStep);
            MoveOrAct(actor, direction);
        }

        private void MoveOrAct(Actor actor, Direction direction) {
            var actions = mapConsole.GetAction(actor, actors, direction);
            if (actions.Count == 0) {
                mapConsole.MoveActor(actor, actors, direction);
            }
            else { 
            foreach (var action in actions) {
                    var result = action.Perform(actor, true);
                    if (result.Outcome != Actions.Outcome.Canceled) {
                        logConsole.Log(result.Message);
                    }
                    if (result.KeepMoving) {
                        mapConsole.MoveActor(actor, actors, direction);
                    }
                }
            }
        }

        public override void ProcessMouse(IScreenObject host, MouseScreenObjectState mouse, out bool handled) {
            var mousePos = mouse.CellPosition.ToPoint() + new Point(-1, -1);

            if (state == InputState.Targeting) {
                trajectory = new List<Point>();
                Point playerPos = player.Location;

                Algorithms.Line(playerPos.X, playerPos.Y, mousePos.X, mousePos.Y, (int x, int y) => {
                    Point point = new Point(x, y);
                    if (!mapConsole.map.InBounds(point) || !mapConsole.map.IsWalkable(point)) {
                        trajectory.Clear();
                        return false;
                    }

                    if (playerPos != point) {
                        trajectory.Add(point);
                    }
                    return true;
                });

                mapConsole.overlayPoints = trajectory;

                if (mouse.Mouse.LeftClicked && trajectory.Any()) {
                    if (playerPos.X > mousePos.X || playerPos.Y > mousePos.Y) {
                        trajectory.Reverse();
                    }

                    FireMissile(trajectory);
                    state = InputState.Idle;
                    mapConsole.overlayPoints.Clear();
                }
            }

            var actor = actors
                .SingleOrDefault(a => a.Location == mousePos && player.Fov.IsInFov(a.Location.X, a.Location.Y));
            if (actor != null) {
                messageConsole.SetMessage(actor.Name);
            }

            handled = true;
        }
    }
}