using Core.Interfaces;
using Rogue.Components;
using Rogue.Consoles;
using Rogue.GameObjects;
using Rogue.Services;
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
        private static Player player;
        private static List<Actor> actors;

        static void Main(string[] args) {
            Locator.RegisterAudioPlayer(new AudioPlayer());

            StartGame();
        }

        private static void StartGame() {
            var generator = new MapGenerator(new Map.RoomDecorator(), Width, Height, 100, 3, 9, 3, 9);

            map = generator.GenerateMap();

            player = new Player(new Point(0, 0), Color.Yellow, new RogueSharpFov(map));
            player.Inventory.Add(new Sword(new Point()));
            player.Inventory.Add(new Spear(new Point()));

            actors = new List<Actor> {
                player,
            };
            actors.AddRange(map.Actors);

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
            var inventory = new InventoryConsole(actors.Single(a => a is Player));

            var keyboardHandler = new KeyboardHandler(mapConsole, logConsole, messageConsole, player, inventory, actors, () => StartGame());

            var mainConsole = new MainConsole(mapConsole, logConsole, messageConsole, keyboardHandler, inventory, actors);

            mainConsole.Children.Add(mapConsole);
            mainConsole.Children.Add(logConsole);
            mainConsole.Children.Add(messageConsole);
            mainConsole.Children.Add(inventory);

            SadConsole.Game.Instance.Screen = mainConsole;
        }
    }
}
