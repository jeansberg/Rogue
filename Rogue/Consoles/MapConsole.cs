using Rogue.GameObjects;
using Rogue.MazeGenerator;
using Rogue.Primitives;
using RogueSharp;
using RogueSharp.Algorithms;
using SadConsole;
using SadConsole.Host;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using Point = SadRogue.Primitives.Point;

namespace Rogue.Graphics {
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
            DrawObjects(player.Fov);
            DrawActors(actors, player.Fov);

            base.Update(delta);
        }

        private void DrawObjects(FieldOfView<MapCell> fov) {
            foreach(var gameObject in map.GameObjects) {
                var cell = map[gameObject.Location.X, gameObject.Location.Y];
                if (GetVisibility(cell, fov) != Visibility.InFov) {
                    continue;
                }

                var coloredString = new ColoredString(new ColoredGlyph(gameObject.Color, Color.Black, gameObject.GlyphId));

                Cursor.Position = new SadRogue.Primitives.Point(cell.X, cell.Y);
                Cursor.Print(coloredString);
            }
        }

        private Visibility GetVisibility(MapCell cell, FieldOfView<MapCell> fov) {
            if (showEverything || fov.IsInFov(cell.X, cell.Y)) {
                return Visibility.InFov;
            } else if (cell.Discovered) {
                return Visibility.Discovered;
            }

            return Visibility.Hidden;
        }

        private void DrawActors(List<Actor> actors, FieldOfView<MapCell> fov) {
            foreach (var actor in actors.Where(a => a.IsAlive)) {
                var cell = map[actor.Location.X, actor.Location.Y];
                if (GetVisibility(cell, fov) != Visibility.InFov) {
                    continue;
                }

                var coloredString = new ColoredString(new ColoredGlyph(actor.Color, Color.Black, actor.GlyphId));

                Cursor.Position = new SadRogue.Primitives.Point(cell.X, cell.Y);
                Cursor.Print(coloredString);
            }
        }

        private void DrawMap(FieldOfView<MapCell> fov) {
            Cursor.Position = new SadRogue.Primitives.Point(0, 0);

            foreach (var cell in map.GetAllCells()) {
                if (GetVisibility(cell, fov) == Visibility.InFov) {
                    DrawCell(cell, true);
                }
                else if (GetVisibility(cell, fov) == Visibility.Discovered) {
                    DrawCell(cell, false);
                }
                else {
                    SkipCell();
                }
            }
        }

        private void SkipCell() {
            Cursor.RightWrap(1);
        }

        private void DrawCell(MapCell cell, bool inFov) {
            var foreground = inFov ? cell.Color : cell.Color.GetDarker();
            var background = Color.Black;
            var coloredString = new ColoredString(new ColoredGlyph(foreground, background, cell.GlyphId));
            Cursor.Print(coloredString);
        }

        public bool MoveActor(Actor actor, Direction dir) {
            var newPoint = actor.Location.Increment(dir);
            if (!map.InBounds(newPoint) || !map[newPoint.X, newPoint.Y].IsWalkable) {
                return false;
            }

            actor.Location = newPoint;
            return true;
        }

        public Point NextStep(Actor actor, Point target) {
            var pathFinder = new AStarShortestPath<MapCell>();
            var path = pathFinder.FindPath(map[actor.Location.X, actor.Location.Y], map[target.X, target.Y], map);
            var step = path[1];

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
    }
}
