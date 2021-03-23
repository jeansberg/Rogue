using System.Collections.Generic;

namespace Core.Interfaces {
    public interface IFov {
        bool IsInFov(int x, int y);
        IReadOnlyCollection<ICell> ComputeFov(int xOrigin, int yOrigin, int radius, bool lightWalls);
    }
}
