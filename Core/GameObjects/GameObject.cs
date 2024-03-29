﻿using Core;
using Core.Interfaces;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public abstract class GameObject : IHasAction {
        public GameObject(Point location) {
            Location = location;
        }

        public abstract void Update();

        public abstract Color Color();
        public abstract int GlyphId();
        public abstract string Name();

        public Point Location { get; set; }
        public abstract IAction GetAction(IMap map, Direction from);
    }
}
