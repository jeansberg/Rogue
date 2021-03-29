using Rogue.GameObjects;
using Rogue.Map;
using System.Collections.Generic;

namespace Core.Interfaces {
    public interface IMap {
        List<ICell> Cells();
        ICell GetCellAt(Point point);
        bool IsTransparent(Point point);
        bool IsDiscovered(Point point);

        bool IsWalkable(Point point);
        bool IsInBounds(Point point);
        void SetWalkable(Point point, bool walkable);
        void SetTransparent(Point point, bool transparent);
        void SetDiscovered(Point point);
        List<GameObject> GameObjects { get; set; }
        List<Actor> Actors { get; set; }
        void RemoveGameObject(GameObject gameObject);
        int Width { get; set; }
        int Height { get; set; }
        int Level { get; set; }
        List<Room> Rooms { get; set; }
    }
}
