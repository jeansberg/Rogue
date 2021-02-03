using Rogue.GameObjects;
using Rogue.MazeGenerator;
using Rogue.Primitives;
using SadConsole;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace Rogue.Consoles {
    public static class ConsoleExtensions {
        public static void Draw(this Console console, MapCell cell, Visibility visibility) {
            if (visibility == Visibility.Hidden) {
                console.Cursor.RightWrap(1);
                return;
            }

            var foreground = visibility == Visibility.InFov ? cell.Color : cell.Color.GetDarker();
            console.Cursor.Position = new Point(cell.X, cell.Y);
            Draw(console, foreground, cell.GlyphId);
        }

        public static void Draw(this Console console, GameObject gameObject, Visibility visibility) {
            if (visibility == Visibility.Hidden) {
                return;
            }

            var foreground = visibility == Visibility.InFov ? gameObject.Color : gameObject.Color.GetDarker();
            console.Cursor.Position = gameObject.Location;
            Draw(console, foreground, gameObject.GlyphId);
        }

        public static void Draw(this Console console, Point point, ColoredGlyph glyph) {
            console.Cursor.Position = point;
            Draw(console, glyph.Foreground, glyph.Glyph);
        }

        private static void Draw(Console console, Color foreground, int glyphId) {
            var coloredString = new ColoredString(new ColoredGlyph(foreground, Color.Black, glyphId));
            console.Cursor.Print(coloredString);
        }
    }
}
