using Core;
using Core.Interfaces;
using Rogue.Actions;
using System.Collections.Generic;

namespace Rogue.GameObjects {
    public abstract class Actor : GameObject {
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public bool IsAlive => Health > 0;
        public IFov Fov { get; protected set; }
        public List<GameObject> Inventory { get; set; }

        public Actor(Point location, int health, IFov fov) :
            base(location) {
            MaxHealth = health;
            Health = MaxHealth;
            Fov = fov;

            Inventory = new List<GameObject>();
        }

        public abstract void TakeDamage(int damage);

        public virtual void UpdateFov() {
            Fov.ComputeFov(Location.X, Location.Y, 5, true);
        }

        public virtual void ReplaceFov(IFov newFov) {
            Fov = newFov;
        }


        public override IAction GetAction(IMap map, Direction from) {
           return Attack(null);
        }

        public Attack Attack(Missile? missile) {
            return new Attack(this, missile);
        }
    }
}
