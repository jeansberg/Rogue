using Core;
using Core.Interfaces;
using Rogue.GameObjects;
using Rogue.Primitives;
using Rogue.Services;
using SadConsole;
using SadConsole.Host;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Utilities.SadConsole;
using Font = SadConsole.Font;
using Point = Core.Point;

namespace Rogue.Consoles {
    public class MapConsole : SadConsole.Console {
        public readonly Game game;
        public IMap map { get { return game.Map; } }
        public readonly IPathFinder pathFinder;
        public List<Point> overlayPoints;
        private readonly bool showEverything;

        public MapConsole(Game game, bool showEverything, IPathFinder pathFinder) : base(game.Map.Width, game.Map.Height + 1) {
            this.Font = new Font(12, 12, 0, 16, 16, 0, new GameTexture(new SFML.Graphics.Texture("../../../Cheepicus_12x12.png")), "mapFont");

            this.game = game;
            this.overlayPoints = new List<Point>();
            this.showEverything = showEverything;
            this.Position = new SadRogue.Primitives.Point(1, 1);
            this.pathFinder = pathFinder;
        }

        public override void Update(TimeSpan delta) {
            this.Clear();

            var player = map.Actors.Single(a => a is Player) as Player;

            player.UpdateFov();

            map.Cells()
                .Where(c => player.Fov.IsInFov(c.Location.X, c.Location.Y))
                .ToList()
                .ForEach(c => map.SetDiscovered(c.Location));

            DrawMap(player.Fov);
            DrawGameObjects(map.GameObjects ,player.Fov);
            DrawGameObjects(map.Actors.Where(a => a.IsAlive), player.Fov);
            DrawOverlay();

            base.Update(delta);
        }

        public void MoveActor(Actor actor, List<Actor> actors, Direction dir) {
            var newPoint = actor.Location.Increment(dir);
            if (!map.InBounds(newPoint) || !map.IsWalkable(newPoint) || actors.Any(actor => actor.Location == newPoint)) {
                return;
            }

            actor.Location = newPoint;
            if (actor is Player) {
                var cell = map.GetCellAt(newPoint);
                if (cell.Type == CellType.StairCaseDown) {
                    game.Descend();
                }
                else if (cell.Type == CellType.StairCaseUp && game.HasAmulet) {
                    game.Ascend();
                }
            }

            var actorLocations = actors.Where(a => a.IsAlive).GroupBy(a => a.Location);
            Debug.Assert(actorLocations.All(al => al.Count() < 2));
        }

        public Point GetNextStep(Actor actor, List<Actor> actors, Point target) {
            var path = pathFinder.FindPath(actor.Location, target, map, ValidStep);
            var step = path[1];

            bool ValidStep<TCell>(TCell cell, TCell destination) where TCell : ICell {
                var reachedDestination = cell.Location.X == destination.Location.X && cell.Location.Y == destination.Location.Y;
                var cellIsOccupied = actors.Any(a => a != actor && a.Location == new Point(cell.Location.X, cell.Location.Y));
                return reachedDestination ||
                    (map.IsWalkable(cell.Location) && !cellIsOccupied);
            }

            return new Point(step.Location.X, step.Location.Y);
        }


        public IAction Act(Actor actor, Direction direction) {
            var gameObject = map.GameObjects.FirstOrDefault(g => g.Location == actor.Location.Increment(direction));

            if (gameObject != null) {
                return gameObject.GetAction(map, direction.Opposite());
            }

            return null;
        }

        public List<IAction> GetActions(Actor actor, List<Actor> actors, Direction direction) {
            var location = actor.Location.Increment(direction);

            // Check actors first since they should always block
            var otherActor = actors.FirstOrDefault(g => g.Location == location && g != actor);
            if (otherActor != null && otherActor.IsAlive) {
                return new List<IAction> { otherActor.GetAction(map, direction.Opposite()) };
            }

            var gameObjects = map.GameObjects.Where(g => g.Location == location);
            return gameObjects.Select(g => g.GetAction(map, direction.Opposite()))
                .ToList();
        }

        private void DrawGameObjects(IEnumerable<GameObject> gameObjects, IFov fov) {
            foreach (var gameObject in gameObjects) {
                var cell = map.GetCellAt(gameObject.Location);
                var visibility = GetVisibility(cell, fov);

                this.Draw(gameObject, visibility);
            }
        }

        private void DrawMap(IFov fov) {
            Cursor.Position = new SadRogue.Primitives.Point(0, 0);

            foreach (var cell in map.Cells()) {
                var visibility = GetVisibility(cell, fov);
                this.Draw(cell, visibility, game.HasAmulet);
            }
        }

        private void DrawOverlay() {
            foreach (var point in overlayPoints) {
                this.Draw(point, new ColoredGlyph(Color.Green.ToSadColor(), Color.Transparent.ToSadColor(), 7));
            }
        }

        private Visibility GetVisibility(ICell cell, IFov fov) {
            if (showEverything || fov.IsInFov(cell.Location.X, cell.Location.Y)) {
                return Visibility.InFov;
            }
            else if (map.IsDiscovered(cell.Location)) {
                return Visibility.Discovered;
            }

            return Visibility.Hidden;
        }

        public override bool ProcessMouse(MouseScreenObjectState state) {
            return false;
        }
    }
}
