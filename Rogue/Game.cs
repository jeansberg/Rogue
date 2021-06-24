using Core.Interfaces;
using Rogue.GameObjects;
using System.Collections.Generic;
using System.Linq;
using Utilities.RogueSharp;

namespace Rogue {
    public class Game {
        private List<IMap> maps;
        public bool HasAmulet { get; set; }

        public IMap Map { get; private set; }
        public Game(List<IMap> maps) {
            this.maps = maps;
            Map = maps[0];
        }

        public void Descend() {
            var currentMap = Map;
            var nextMap = maps[Map.Level];
            Map = nextMap;

            MovePlayer(currentMap, nextMap);
        }

        public void Ascend() {
            Map = maps[Map.Level - 2];

            MovePlayer(maps[Map.Level - 2], Map);
        }

        public void MovePlayer(IMap source, IMap destination) {
            var player = Getplayer(source);
            source.Actors.Remove(player);
            destination.Actors.Add(player);

            player.ReplaceFov(new RogueSharpFov(destination));
        }

        private Player Getplayer(IMap map) { return (Player)map.Actors.Single(a => a is Player); }

    }
}
