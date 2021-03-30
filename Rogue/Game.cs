using Core.Interfaces;
using System.Collections.Generic;

namespace Rogue {
    public class Game {
        private List<IMap> maps;
        public IMap Map { get; private set; }
        public Game(List<IMap> maps) {
            this.maps = maps;
            Map = maps[0];
        }

        public void Descend() {
            Map = maps[Map.Level];
        }

        public void Ascend() {
            Map = maps[Map.Level - 2];
        }
    }
}
