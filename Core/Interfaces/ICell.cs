using System.Drawing;

namespace Core.Interfaces {
    public interface ICell {
        Point Location { get; set; }
        bool IsTransparent { get; set; }
        bool IsWalkable { get; set; }
        bool IsDiscovered{ get; set; }

        Color Color { get;}
        CellType Type { get; set; }
        int GlyphId { get; }
    }
}
