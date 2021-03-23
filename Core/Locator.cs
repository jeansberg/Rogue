namespace Rogue.Services {
    public static class Locator {
        private static IAudioPlayer audio;

        public static IAudioPlayer Audio => audio;
        public static void RegisterAudioPlayer(IAudioPlayer audioPlayer) {
            audio = audioPlayer;
        }
    }
}
