using Core.GameObjects;
using Core.Interfaces;
using Rogue.Components;
using Rogue.Consoles;
using Rogue.GameObjects;
using Rogue.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utilities.RogueSharp;
using Point = Core.Point;

namespace Rogue {
    class Program
    {
        private const int Width = 60;
        private const int Height = 40;
        private static IMap map;

        static void Main(string[] args) {
            Locator.RegisterAudioPlayer(new AudioPlayer());
            Locator.RegisterDiceRoller(new DiceRoller(new System.Random(DateTime.Now.Millisecond)));

            StartGame();
        }

        private static void StartGame() {
            var generator = new MapGenerator(new Map.RoomDecorator(), Width, Height, 100, 3, 9, 3, 9);

            var maps = new List<IMap>();
            for(var level = 1; level < 11; level++) {
                if (level == 1) {
                    var map = generator.GenerateMap();
                    maps.Add(map);
                }
                else {
                    var map = generator.GenerateMap(maps.Last());
                    maps.Add(map);
                }
            }

            var game = new Game(maps);
            map = game.Map;
            var player = new Player(new Point(0, 0), new RogueSharpFov(map));
            player.Inventory.Add(new Weapon(new Point(0, 0), WeaponType.Mace));
            map.Actors.Add(player);

            if (SadConsole.Game.Instance == null) {
                // Setup the engine and create the main window.
                SadConsole.Game.Create((int)(Width * 2) + 20 + 2, Height + 2);

                // Hook the start event so we can add consoles to the system.
                SadConsole.Game.Instance.OnStart = Init;
            }

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init() {
            // Any startup code for your game. We will use an example console for now
            var mapConsole = new MapConsole(map, true, new AStarPathFinder());
            var logConsole = new LogConsole();
            var messageConsole = new MessageConsole();

            var player = map.Actors.Single(a => a is Player) as Player;
            var inventory = new InventoryConsole(player);

            var keyboardHandler = new KeyboardHandler(mapConsole, logConsole, messageConsole, player, inventory, map.Actors, () => StartGame());

            var mainConsole = new MainConsole(mapConsole, logConsole, messageConsole, keyboardHandler, inventory, map.Actors);

            mainConsole.Children.Add(mapConsole);
            mainConsole.Children.Add(logConsole);
            mainConsole.Children.Add(messageConsole);
            mainConsole.Children.Add(inventory);

            SadConsole.Game.Instance.Screen = mainConsole;
        }
    }
}
