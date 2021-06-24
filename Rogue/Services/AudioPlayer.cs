using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Services {
    public class AudioPlayer : IAudioPlayer {
        private const string SfxPath = "../../../sfx";
        private Random rnd;
        private WaveOutEvent outputDevice;
        private MixingSampleProvider mixer;
        
        public AudioPlayer(Random rnd) {
            this.rnd = rnd;

            outputDevice = new WaveOutEvent();
            mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(48000, 2));
            mixer.ReadFully = true;
            outputDevice.Init(mixer);
            outputDevice.Play();
        }

        public void PlaySound(params string[] tags) {

            string[] matchingSoundFiles = FindMatching(tags.Select(t => t.Replace(" ", "").ToLower()));

            if (!matchingSoundFiles.Any()) {
                tags[tags.Length-1] = "basic";
                matchingSoundFiles = FindMatching(tags.Select(t => t.Replace(" ", "").ToLower()));
                if (!matchingSoundFiles.Any()) {
                    return;
                }
            }

            var soundFile = matchingSoundFiles[rnd.Next(matchingSoundFiles.Length)];

            var input = new AudioFileReader(soundFile);
            AddMixerInput(new AutoDisposeFileReader(input));
        }

        private static string[] FindMatching(IEnumerable<string> tags) {
            var matchingFiles = System.IO.Directory.GetFiles(SfxPath, $"*{tags.First()}*", new System.IO.EnumerationOptions {
            });

            return matchingFiles.Where(m => MatchesAllTags(m.ToLower(), tags)).ToArray();
        }

        private static bool MatchesAllTags(string file, IEnumerable<string> tags) {
            return tags.All(t => file.Contains(t));
        }

        private ISampleProvider ConvertToRightChannelCount(ISampleProvider input) {
            if (input.WaveFormat.Channels == mixer.WaveFormat.Channels) {
                return input;
            }
            if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2) {
                return new MonoToStereoSampleProvider(input);
            }
            throw new NotImplementedException("Not yet implemented this channel count conversion");
        }

        private void AddMixerInput(ISampleProvider input) {
            mixer.AddMixerInput(ConvertToRightChannelCount(input));
        }

        public void Dispose() {
            outputDevice.Dispose();
        }

        class AutoDisposeFileReader : ISampleProvider {
            private readonly AudioFileReader reader;
            private bool isDisposed;
            public AutoDisposeFileReader(AudioFileReader reader) {
                this.reader = reader;
                this.WaveFormat = reader.WaveFormat;
            }

            public int Read(float[] buffer, int offset, int count) {
                if (isDisposed)
                    return 0;
                int read = reader.Read(buffer, offset, count);
                if (read == 0) {
                    reader.Dispose();
                    isDisposed = true;
                }
                return read;
            }

            public WaveFormat WaveFormat { get; private set; }
        }
    }
}
