namespace Rogue.Services {
    public static class Locator {
        private static IAudioPlayer audio;
        private static IDiceRoller dice;

        public static IAudioPlayer Audio => audio;
        public static IDiceRoller Dice => dice;
        public static void RegisterAudioPlayer(IAudioPlayer audioPlayer) {
            audio = audioPlayer;
        }
        public static void RegisterDiceRoller(IDiceRoller diceRoller) {
            dice = diceRoller;
        }
    }
}
