﻿using Rogue.Actions;
using SadConsole;
using SadRogue.Primitives;

namespace Rogue.GameObjects {
    public class Door : GameObject {
        public Door(Point location, Orientation orientation) : base(location, Color.SaddleBrown, orientation == Orientation.Vertical ? 179 : 196, "door") {
            Orientation = orientation;
            IsOpen = false;
            OriginalOrientation = orientation;
            OriginalLocation = location;
        }

        public Orientation Orientation { get; set; }
        public Orientation OriginalOrientation { get; set; }
        public Point OriginalLocation { get; set; }


        public bool IsOpen { get; set; }

        public override IAction GetAction(RogueMap<MazeGenerator.MapCell> map, Direction.Types fromDirection) {
            if (IsOpen) {
                return new CloseDoor(this, map); 
            }

            return new OpenDoor(fromDirection, this, map);
        }
    }
}
