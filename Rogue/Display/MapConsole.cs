﻿using Rogue.GameObjects;
using Rogue.MazeGenerator;
using RogueSharp;
using SadConsole;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Graphics {
    public class MapConsole : SadConsole.Console {
        private readonly RogueMap<MapCell> map;
        private FieldOfView<MapCell> fov;

        public MapConsole(RogueMap<MapCell> map) : base(map.Width, map.Height + 1) {
            this.map = map;
            this.Position = new SadRogue.Primitives.Point(1, 1);
            fov = new FieldOfView<MapCell>(map);
        }

        public void Update(Player player, TimeSpan delta) {
            this.Clear();

            fov.ComputeFov(player.Location.X, player.Location.Y, 5, true);

            map.GetAllCells()
                .Where(c => fov.IsInFov(c.X, c.Y))
                .ToList()
                .ForEach(x => x.Discovered = true);

            DrawMap();
            DrawObjects();
            DrawPlayer(player);

            base.Update(delta);
        }

        private void DrawObjects() {
            foreach(var gameObject in map.GameObjects) {
                var cell = map[gameObject.Location.X, gameObject.Location.Y];
                if (!fov.IsInFov(cell.X, cell.Y)) {
                    continue;
                }

                var coloredString = new ColoredString(gameObject.ToString(), gameObject.Color, Color.Black);

                Cursor.Position = new SadRogue.Primitives.Point(cell.X, cell.Y);
                Cursor.Print(coloredString);
            }
        }

        private void DrawPlayer(Player player) {
            var coloredString = new ColoredString(player.ToString(), player.Color, Color.Black);

            Cursor.Position = new SadRogue.Primitives.Point(player.Location.X, player.Location.Y);
            Cursor.Print(coloredString);
        }

        private void DrawMap() {
            Cursor.Position = new SadRogue.Primitives.Point(0, 0);

            foreach (var cell in map.GetAllCells()) {
                if (!cell.Discovered) {
                    DrawEmpty();
                } else if(fov.IsInFov(cell.X, cell.Y)) {
                    DrawCell(cell, true);
                }
                else {
                    DrawCell(cell, false);
                }
            }
        }

        private void DrawEmpty() {
            Cursor.RightWrap(1);
        }

        private void DrawCell(MapCell cell, bool inFov) {
            var foreground = inFov ? cell.Color : cell.Color.GetDarker();
            var background = Color.Black;
            var coloredString = new ColoredString(cell.ToString(), foreground, background);
            Cursor.Print(coloredString);
        }

        public IAction MoveActor(Actor actor, Direction dir) {
            var newPoint = actor.Location.Increment(dir);
            if (map.InBounds(newPoint) && map[newPoint.X, newPoint.Y].IsWalkable) {
                actor.Location = newPoint;

                foreach(var gameObject in map.GameObjects.Where(g => g.Location == actor.Location)) {
                    var action = gameObject.GetAction(dir.Type.Opposite());
                    if (action.Perform(map, actor, true)) {
                        return action;
                    }
                }
            }

            return null;
        }

        public IAction Act(Player player, Direction.Types direction) {
            var gameObject = map.GameObjects.FirstOrDefault(g => g.Location == player.Location.Increment(direction));

            if (gameObject != null) {
                var action = gameObject.GetAction(direction.Opposite());
                if (action.Perform(map, player)) {
                    return action;
                }
            }

            return null;
        }
    }
}