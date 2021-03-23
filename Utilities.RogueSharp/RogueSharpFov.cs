using Core.Interfaces;
using System;
using System.Collections.Generic;
using RogueSharp;
using System.Linq;
using ICell = Core.Interfaces.ICell;

namespace Utilities.RogueSharp {
    public class RogueSharpFov : IFov {
        private FieldOfView Fov;
        private Core.Interfaces.IMap map;

        public RogueSharpFov(Core.Interfaces.IMap map) {
            this.map = map;
            var rogueSharpMap = map.ToRogueSharpMap();
            Fov = new FieldOfView(rogueSharpMap);
        }

        public IReadOnlyCollection<ICell> ComputeFov(int xOrigin, int yOrigin, int radius, bool lightWalls) {
            return Fov.ComputeFov(xOrigin, yOrigin, radius, lightWalls).Select(c => map.GetCellAt(new Core.Point(xOrigin, yOrigin))).ToList();
        }

        public bool IsInFov(int x, int y) {
            return Fov.IsInFov(x, y);
        }
    }
}
