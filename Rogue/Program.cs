﻿using Core.GameObjects;
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
        private static Game game;
        private static IMap map;
        private static Random rnd;

        static void Main(string[] args) {
            rnd = new Random(DateTime.Now.Millisecond);

            Locator.RegisterAudioPlayer(new AudioPlayer(rnd));
            Locator.RegisterDiceRoller(new DiceRoller(rnd));

            StartGame(rnd);
        }

        private static void StartGame(Random rnd) {
            var generator = new MapGenerator(rnd, roomDecorator: new Map.RoomDecorator(), width: Width, height: Height, roomAttempts: 100, roomMinWidth: 3, roomMaxWidth: 9, roomMinHeight: 3, roomMaxHeight: 9);

            var maps = new List<IMap>();
            for (var level = 1; level < 11; level++) {
                if (level == 1) {
                    var map = generator.GenerateMap();
                    maps.Add(map);
                }
                else {
                    var map = generator.GenerateMap(maps.Last());
                    maps.Add(map);
                }
            }

            game = new Game(maps);
            map = game.Map;

            var player = GetPlayer();
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

        private static Player GetPlayer() {
            var player = new Player(new Point(0, 0), new RogueSharpFov(map));
            player.Inventory.Add(new Weapon(new Point(0, 0), WeaponType.Mace));
            return player;
        }

        private static void Init() {
            var player = map.Actors.Single(a => a is Player) as Player;

            var mapConsole = new MapConsole(game, true, new AStarPathFinder());
            var logConsole = new LogConsole();
            var messageConsole = new MessageConsole();
            var statusConsole = new StatusConsole(player);
            var menuConsole = new MenuConsole(Width, Height);

            var inventory = new InventoryConsole(player);

            var keyboardHandler = new KeyboardHandler(mapConsole, logConsole, messageConsole, player, inventory, menuConsole, game, () => StartGame(rnd));

            var mainConsole = new MainConsole(mapConsole, logConsole, messageConsole, statusConsole, keyboardHandler, inventory);

            mainConsole.Children.Add(mapConsole);
            mainConsole.Children.Add(logConsole);
            mainConsole.Children.Add(messageConsole);
            mainConsole.Children.Add(statusConsole);
            mainConsole.Children.Add(inventory);
            mainConsole.Children.Add(menuConsole);

            SadConsole.Game.Instance.Screen = mainConsole;
        }
    }
}
