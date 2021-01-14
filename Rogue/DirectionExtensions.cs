using SadRogue.Primitives;
using System;

namespace Rogue {
    public static class DirectionExtensions {
        public static Direction.Types Opposite(this Direction.Types direction) => direction switch {
            Direction.Types.None => Direction.Types.None,
            Direction.Types.Up => Direction.Types.Down,
            Direction.Types.UpRight => Direction.Types.DownLeft,
            Direction.Types.Right => Direction.Types.Left,
            Direction.Types.DownRight => Direction.Types.UpLeft,
            Direction.Types.Down => Direction.Types.Up,
            Direction.Types.DownLeft => Direction.Types.UpRight,
            Direction.Types.Left => Direction.Types.Right,
            Direction.Types.UpLeft => Direction.Types.DownRight,
            _ => throw new NotImplementedException(),
        };
    }
}
