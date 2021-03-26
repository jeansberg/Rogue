using Core;
using Core.Interfaces;
using Rogue.Actions;
using System.Collections.Generic;

namespace Rogue.GameObjects {
    public abstract class Actor : GameObject {
        public int Health { get; set; }
        public bool IsAlive => Health > 0;
        public IFov Fov { get; set; }
        public List<GameObject> Inventory { get; set; }

        public virtual void Damage(int damage) {
            Health -= damage;
        }

        public Actor(Point location, int health, IFov fov) :
            base(location) {
            Health = health;
            Fov = fov;

            Inventory = new List<GameObject>();
        }

        public override IAction GetAction(IMap map, Direction from) {
           return Attack(null);
        }

        public Attack Attack(Missile? missile) {
            return new Attack(this, missile);
        }
    }
}
