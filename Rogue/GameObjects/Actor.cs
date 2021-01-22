using Rogue.Actions;
using Rogue.MazeGenerator;
using RogueSharp;
using SadRogue.Primitives;
using System;

namespace Rogue.GameObjects {
    public class Actor : GameObject {
        public Actor(SadRogue.Primitives.Point location, Color color, int glyphId, int health, string name, FieldOfView<MapCell> fov) :
            base(location, color, glyphId, name) {
            Health = health;
            Fov = fov;
        }

        public int Health { get; set; }
        public bool IsAlive => Health > 0;
        public FieldOfView<MapCell> Fov { get; set; }
        public void UpdateFov(RogueMap<MapCell> map) {
            Fov.ComputeFov(Location.X, Location.Y, 5, true);
        }

        public override IAction GetAction(RogueMap<MazeGenerator.MapCell> map, Direction.Types from) {
            return new Attack(this);
        }
    }
}
