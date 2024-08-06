using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace MusicManager.Comps
{
    internal static class SilenceDetector
    {
        public static int GetSilenceStartMS(string path, sbyte silenceThreshold = -40)
        {
            try
            {
                using (AudioFileReader reader = new AudioFileReader(path))
                {
                    return (int)GetSilenceStartDurationCore(
                        reader,
                        SilenceLocation.Start,
                        silenceThreshold
                    );
                }
            }
            catch { }
            return -1;
        }

        enum SilenceLocation
        {
            Start,
            End
        }

        static bool IsSilence(float amplitude, sbyte threshold)
        {
            double dB = 20 * Math.Log10(Math.Abs(amplitude));
            return dB < threshold;
        }

        static double GetSilenceStartDurationCore(
            AudioFileReader reader,
            SilenceLocation location,
            sbyte silenceThreshold
        )
        {
            int counter = 0;
            bool volumeFound = false;
            bool eof = false;
            long oldPosition = reader.Position;

            var buffer = new float[reader.WaveFormat.SampleRate * 4];
            while (!volumeFound && !eof)
            {
                int samplesRead = reader.Read(buffer, 0, buffer.Length);
                if (samplesRead == 0)
                    eof = true;

                for (int n = 0; n < samplesRead; n++)
                {
                    if (IsSilence(buffer[n], silenceThreshold))
                    {
                        counter++;
                    }
                    else
                    {
                        if (location == SilenceLocation.Start)
                        {
                            volumeFound = true;
                            break;
                        }
                        else if (location == SilenceLocation.End)
                        {
                            counter = 0;
                        }
                    }
                }
            }

            // reset position
            reader.Position = oldPosition;

            double silenceSamples = (double)counter / reader.WaveFormat.Channels;
            double silenceDuration = (silenceSamples / reader.WaveFormat.SampleRate) * 1000;
            return silenceDuration;
        }
    }
}
