using Core;
using Core.Interfaces;
using Rogue.Actions;
using System.Collections.Generic;

namespace Rogue.GameObjects {
    public class Actor : GameObject {
        public int Health { get; set; }
        public bool IsAlive => Health > 0;
        public IFov Fov { get; set; }
        public List<GameObject> Inventory { get; set; }
        public GameObject Weapon { get; set; }

        public virtual void Damage(int damage) {
            Health -= damage;
        }

        public Actor(Point location, System.Drawing.Color color, int glyphId, int health, string name, IFov fov) :
            base(location, color, glyphId, name) {
            Health = health;
            Fov = fov;

            Inventory = new List<GameObject>();
        }

        public override IAction GetAction(IMap map, Direction from) {
            return new Attack(this);
        }
    }
}
