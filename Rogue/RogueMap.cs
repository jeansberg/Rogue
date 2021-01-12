using Rogue.GameObjects;
using Rogue.MazeGenerator;
using RogueSharp;
using System.Collections.Generic;

namespace Rogue {
    public class RogueMap<T> : Map<MapCell> {
        public ICollection<GameObject> GameObjects { get; private set; }

        public RogueMap(int width, int height) : base(width, height) {
            GameObjects = new List<GameObject>();
        }
    }
}

