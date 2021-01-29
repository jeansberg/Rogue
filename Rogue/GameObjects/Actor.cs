using Rogue.Actions;
using Rogue.MazeGenerator;
using RogueSharp;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;

namespace Rogue.GameObjects {
    public class Actor : GameObject {
        public int Health { get; set; }
        public bool IsAlive => Health > 0;
        public FieldOfView<MapCell> Fov { get; set; }
        public List<GameObject> Inventory { get; set; }
        public GameObject Weapon { get; set; }

        public Actor(SadRogue.Primitives.Point location, Color color, int glyphId, int health, string name, FieldOfView<MapCell> fov) :
            base(location, color, glyphId, name) {
            Health = health;
            Fov = fov;

            Inventory = new List<GameObject>();
        }

        public override IAction GetAction(RogueMap<MazeGenerator.MapCell> map, Direction.Types from) {
            return new Attack(this);
        }
    }
}
