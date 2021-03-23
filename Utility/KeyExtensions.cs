using SadConsole.Input;
using System;

namespace Utilities.SadConsole {
    public static class KeyExtensions {
        public static ConsoleKey ToConsoleKey(this Keys key) =>
            key switch {
                Keys.Left => ConsoleKey.LeftArrow,
                Keys.Right => ConsoleKey.RightArrow,
                Keys.Up => ConsoleKey.UpArrow,
                Keys.Down => ConsoleKey.DownArrow,
                Keys.T => ConsoleKey.T,
                Keys.I => ConsoleKey.I,
                _ => throw new NotImplementedException(),
            };

        public static Keys ToSadKey(this ConsoleKey key) =>
            key switch {
                ConsoleKey.LeftArrow => Keys.Left,
                ConsoleKey.RightArrow => Keys.Right,
                ConsoleKey.UpArrow => Keys.Up,
                ConsoleKey.DownArrow => Keys.Down,
                ConsoleKey.T => Keys.T,
                ConsoleKey.I => Keys.I,
                _ => throw new NotImplementedException(),
            };
    }
}

