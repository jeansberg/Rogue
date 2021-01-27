using Rogue.GameObjects;
using Rogue.Map;
using Rogue.MazeGenerator;
using Rogue.Primitives;
using RogueSharp;
using RogueSharp.Algorithms;
using SadConsole;
using SadConsole.Host;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Point = SadRogue.Primitives.Point;

namespace Rogue.Consoles {
    public class MapConsole : SadConsole.Console {
        private readonly RogueMap<MapCell> map;
        private readonly bool showEverything;

        public MapConsole(RogueMap<MapCell> map, bool showEverything) : base(map.Width, map.Height + 1) {
            this.Font = new Font(12, 12, 0, 16, 16, 0, new GameTexture(new SFML.Graphics.Texture("../../../Cheepicus_12x12.png")), "mapFont");

            this.map = map;
            this.showEverything = showEverything;
            this.Position = new SadRogue.Primitives.Point(1, 1);
        }

        public void Update(List<Actor> actors, TimeSpan delta) {
            this.Clear();

            var player = actors.Single(a => a is Player) as Player;

            player.Fov.ComputeFov(player.Location.X, player.Location.Y, 5, true);

            map.GetAllCells()
                .Where(c => player.Fov.IsInFov(c.X, c.Y))
                .ToList()
                .ForEach(x => x.Discovered = true);

            DrawMap(player.Fov);
            DrawGameObjects(map.GameObjects ,player.Fov);
            DrawGameObjects(actors.Where(a => a.IsAlive), player.Fov);

            base.Update(delta);
        }

        public bool MoveActor(Actor actor, List<Actor> actors, Direction dir) {
            var newPoint = actor.Location.Increment(dir);
            if (!map.InBounds(newPoint) || !map[newPoint.X, newPoint.Y].IsWalkable) {
                return false;
            }

            actor.Location = newPoint;

            var actorLocations = actors.Where(a => a.IsAlive).GroupBy(a => a.Location);
            Debug.Assert(actorLocations.All(al => al.Count() < 2));

            return true;
        }

        public Point GetNextStep(Actor actor, List<Actor> actors, Point target) {
            var pathFinder = new AStarHacked<MapCell>();
            var path = pathFinder.FindPath(map[actor.Location.X, actor.Location.Y], map[target.X, target.Y], map, ValidStep);
            var step = path[1];

            bool ValidStep<TCell>(TCell cell, TCell destination) where TCell : ICell {
                var reachedDestination = cell.X == destination.X && cell.Y == destination.Y;
                var cellIsOccupied = actors.Any(a => a != actor && a.Location == new Point(cell.X, cell.Y));
                return reachedDestination ||
                    (cell.IsWalkable && !cellIsOccupied);
            }

            return new Point(step.X, step.Y);
        }


        public IAction Act(Actor actor, Direction.Types direction) {
            var gameObject = map.GameObjects.FirstOrDefault(g => g.Location == actor.Location.Increment(direction));

            if (gameObject != null) {
                return gameObject.GetAction(map, direction.Opposite());
            }

            return null;
        }

        public IAction GetAction(Actor actor, List<Actor> actors, Direction.Types direction) {
            var location = actor.Location.Increment(direction);

            var gameObject = map.GameObjects.FirstOrDefault(g => g.Location == location);
            if (gameObject != null) {
                return gameObject.GetAction(map, direction.Opposite());
            }

            var otherActor = actors.FirstOrDefault(g => g.Location == location && g != actor);
            if (otherActor != null && otherActor.IsAlive) {
                return otherActor.GetAction(map, direction.Opposite());
            }

            return null;
        }

        public IAction GetAction(Point location, Direction.Types direction) {
            var gameObject = map.GameObjects.FirstOrDefault(g => g.Location == location);
            if (gameObject != null) {
                return gameObject.GetAction(map, direction.Opposite());
            }

            var otherActor = map.Actors.FirstOrDefault(g => g.Location == location);
            if (otherActor != null && otherActor.IsAlive) {
                return otherActor.GetAction(map, direction.Opposite());
            }

            return null;
        }

        private void DrawGameObjects(IEnumerable<GameObject> gameObjects, FieldOfView<MapCell> fov) {
            foreach (var gameObject in gameObjects) {
                var cell = map[gameObject.Location.X, gameObject.Location.Y];
                var visibility = GetVisibility(cell, fov);

                this.Draw(gameObject, visibility);
            }
        }

        private void DrawMap(FieldOfView<MapCell> fov) {
            Cursor.Position = new SadRogue.Primitives.Point(0, 0);

            foreach (var cell in map.GetAllCells()) {
                var visibility = GetVisibility(cell, fov);
                this.Draw(cell, visibility);
            }
        }

        private Visibility GetVisibility(MapCell cell, FieldOfView<MapCell> fov) {
            if (showEverything || fov.IsInFov(cell.X, cell.Y)) {
                return Visibility.InFov;
            }
            else if (cell.Discovered) {
                return Visibility.Discovered;
            }

            return Visibility.Hidden;
        }

        private void SkipCell() {
            Cursor.RightWrap(1);
        }
    }
}
