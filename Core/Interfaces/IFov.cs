using Rogue.GameObjects;
using System.Collections.Generic;

namespace Core.Interfaces {
    public interface IFov {
        bool IsInFov(int x, int y);
        List<Actor> GetActorsInFov();
        IReadOnlyCollection<ICell> ComputeFov(int xOrigin, int yOrigin, int radius, bool lightWalls);
    }
}
