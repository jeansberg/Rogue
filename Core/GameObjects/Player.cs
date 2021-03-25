using Core;
using Core.Interfaces;
using Color = System.Drawing.Color;

namespace Rogue.GameObjects {
    public class Player : Actor {
        private string name;
        private int Experience;
        public int Level { get {
                if (Experience < 10) {
                    return 1;
                }
                else if (Experience < 30) {
                    return 2;
                }
                else if (Experience < 100) {
                    return 3;
                }
                else if (Experience < 300) {
                    return 4;
                }
                else {
                    return 5;
                }

            }}

        public Player(Point location, Color color, IFov fov, string name = "Player") : base(location, 10, fov) {
            this.name = name;
        }

        public override Color Color() {
            return System.Drawing.Color.Yellow;
        }

        public override int GlyphId() {
            return 64;
        }

        public override string Name() {
            return name;
        }
    };
}

