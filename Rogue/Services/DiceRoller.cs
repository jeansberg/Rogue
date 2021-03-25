using Rogue.Services;
using System;

namespace Rogue {
    public class DiceRoller : IDiceRoller {
        Random rnd;

        public DiceRoller(Random rnd) {
            this.rnd = rnd;
        }

        public int RollDice(int numberOfDice, int diceMaxValue) {
            var outcome = 0;

            for (int i = 0; i < numberOfDice; i++) {
                outcome += rnd.Next(1, diceMaxValue + 1);
            };

            return outcome;
        }
    }
}
