using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace MusicManager.Comps
{
    internal static class SilenceDetector
    {
        public static SilenceInfo GetSilenceInfos(string path, sbyte silenceThreshold = -40)
        {
            try
            {
                using (AudioFileReader reader = new AudioFileReader(path))
                {
                    return GetSilenceInfosCore(reader, silenceThreshold);
                }
            }
            catch { }
            return null;
        }

        static bool IsSilence(float amplitude, sbyte threshold)
        {
            double dB = 20 * Math.Log10(Math.Abs(amplitude));
            return dB < threshold;
        }

        public class SilenceInfo
        {
            int silenceEndMs;
            int volumeEndMs;
            int musicEndMs;

            private readonly int channels;
            private readonly int sampleRate;

            public SilenceInfo(int channels, int sampleRat)
            {
                this.channels = channels;
                this.sampleRate = sampleRat;
            }

            public int GetTotalSilenceMs() => silenceEndMs + GetEndSilenceMs();

            public int GetStartSilenceMs() => silenceEndMs;

            public int GetEndSilenceMs() => musicEndMs - volumeEndMs;

            public int GetMusicEndMs() => musicEndMs;

            public int GetVolumeDurationMs()
            {
                return volumeEndMs - silenceEndMs;
            }

            public void MarkStartSilence(int frame)
            {
                this.silenceEndMs = this.ToMs(frame, this.channels, this.sampleRate);
            }

            public void MarkVolumeEnd(int frame)
            {
                this.volumeEndMs = this.ToMs(frame, this.channels, this.sampleRate);
            }

            public void MarkMusicEnd(int frame)
            {
                this.musicEndMs = this.ToMs(frame, this.channels, this.sampleRate);
            }

            int ToMs(int frameCount, int channels, int sampleRate)
            {
                double silenceSamples = (double)frameCount / channels;
                double silenceDuration = (silenceSamples / sampleRate) * 1000;
                return (int)silenceDuration;
            }
        }

        static SilenceInfo GetSilenceInfosCore(AudioFileReader reader, sbyte silenceThreshold)
        {
            int counter = 0;
            int lastVolume = 0;
            bool volumeStartFound = false;

            var r = new SilenceInfo(reader.WaveFormat.Channels, reader.WaveFormat.SampleRate);
            var buffer = new float[reader.WaveFormat.SampleRate * 4];
            while (true)
            {
                int samplesRead = reader.Read(buffer, 0, buffer.Length);
                if (samplesRead == 0)
                {
                    break;
                }

                for (int n = 0; n < samplesRead; n++)
                {
                    if (!IsSilence(buffer[n], silenceThreshold))
                    {
                        lastVolume = counter;
                        if (!volumeStartFound)
                        {
                            volumeStartFound = true;
                            r.MarkStartSilence(counter);
                        }
                    }
                    counter++;
                }
            }
            if (!volumeStartFound)
            {
                r.MarkStartSilence(lastVolume);
            }
            r.MarkVolumeEnd(lastVolume);
            r.MarkMusicEnd(counter);
            return r;
        }
    }
}
