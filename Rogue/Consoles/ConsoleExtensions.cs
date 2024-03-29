﻿using Core.Interfaces;
using Rogue.GameObjects;
using Rogue.Primitives;
using SadConsole;
using SadRogue.Primitives;
using Utilities.SadConsole;
using Console = SadConsole.Console;
using Point = Core.Point;

namespace Rogue.Consoles {
    public static class ConsoleExtensions {
        public static void Draw(this Console console, ICell cell, Visibility visibility, bool showExits) {
            if (visibility == Visibility.Hidden) {
                console.Cursor.RightWrap(1);
                return;
            }

            Color foreground;
            int glyphId;
            if (cell.Type == Core.CellType.StairCaseUp) {
                foreground = Color.Gray;
                glyphId = 46;
            }
            else {
                foreground = cell.Color.ToSadColor();
                glyphId = cell.GlyphId;
            }

            foreground = visibility == Visibility.InFov ? foreground : foreground.GetDarker();
            console.Cursor.Position = new Point(cell.Location.X, cell.Location.Y).ToSadPoint();

            Draw(console, foreground, glyphId);
        }

        public static void Draw(this Console console, GameObject gameObject, Visibility visibility) {
            if (visibility == Visibility.Hidden) {
                return;
            }

            var foreground = visibility == Visibility.InFov ? gameObject.Color().ToSadColor() : gameObject.Color().ToSadColor().GetDarker();
            console.Cursor.Position = gameObject.Location.ToSadPoint();
            Draw(console, foreground, gameObject.GlyphId());
        }

        public static void Draw(this Console console, Point point, ColoredGlyph glyph) {
            console.Cursor.Position = point.ToSadPoint();
            Draw(console, glyph.Foreground, glyph.Glyph);
        }

        private static void Draw(Console console, SadRogue.Primitives.Color foreground, int glyphId) {
            var coloredString = new ColoredString(new ColoredGlyph(foreground, Color.Black, glyphId));
            console.Cursor.Print(coloredString);
        }
    }
}
