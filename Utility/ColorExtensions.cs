using SadRogue.Primitives;

namespace Utilities.SadConsole {
    public static class ColorExtensions {
        public static Color ToSadColor(this System.Drawing.Color color) => new Color(color.R, color.G, color.B, color.A);
        public static System.Drawing.Color GetDarker(this System.Drawing.Color color) => 
            System.Drawing.Color.FromArgb(255, color.R / 2, color.G / 2, color.B / 2);
    }
}
