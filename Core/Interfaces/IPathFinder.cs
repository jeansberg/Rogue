using System;
using System.Collections.Generic;

namespace Core.Interfaces {
    public interface IPathFinder {
        List<MapCell> FindPath(Point source, Point destination, IMap map, Func<ICell, ICell, bool> IsValidStep);
    }
}
