using System;
using Microsoft.Xna.Framework.Audio;
using NVorbis;
using System.Threading;

namespace MonoBGM
{
    public class Song : IDisposable
    {
        public enum SongState { Stopped, Playing, Stopping }

        public enum StopMode { FadeOut, Immediate }

        private DynamicSoundEffectInstance soundEffectInstance;

        public long LoopRegionStart { get; internal set; }
        public long LoopRegionEnd { get; internal set; }
        public SongState State { get; internal set; } = SongState.Stopped;
        public float Volume { get; set; } = 1.0f;

        internal VorbisReader vorbisReader;
        private Timer fadeTimer;

        public Song(string filename)
        {
            vorbisReader = new VorbisReader(filename);
            LoopRegionStart = 0;
            LoopRegionEnd = vorbisReader.TotalSamples;
        }

        public void Play()
        {
            if (vorbisReader == null)
                throw new InvalidOperationException("File hasn't been assigned");

            soundEffectInstance = new DynamicSoundEffectInstance(
                vorbisReader.SampleRate,
                vorbisReader.Channels == 1 ?
                    AudioChannels.Mono : AudioChannels.Stereo);
            FillBuffer();
            soundEffectInstance.BufferNeeded += (_, __) => { FillBuffer(); };
            soundEffectInstance.Play();
            soundEffectInstance.Volume = Volume;
            State = SongState.Playing;
        }

        public void Stop(StopMode stopMode = StopMode.FadeOut)
        {
            switch (stopMode)
            {
                case StopMode.FadeOut:
                    fadeTimer = new Timer((_) => { ChangeFadeVolume(); }, null, 0, 100);
                    State = SongState.Stopping;
                    break;

                case StopMode.Immediate:
                    Reset();
                    break;
            }
        }

        public void Pause()
        {
            soundEffectInstance?.Pause();
        }

        public void Resume()
        {
            soundEffectInstance?.Resume();
        }

        private void ChangeFadeVolume()
        {
            var volume = soundEffectInstance.Volume;
            volume -= (Volume * 0.05f);
            if (volume < 0) volume = 0;

            soundEffectInstance.Volume = volume;
            if (soundEffectInstance.Volume <= 0)
            {
                Reset();
            }
        }

        public void Dispose()
        {
            vorbisReader.Dispose();
        }

        private void Reset()
        {
            soundEffectInstance.Stop();
            soundEffectInstance.Dispose();

            vorbisReader.DecodedPosition = 0;
            State = SongState.Stopped;

            if (fadeTimer != null)
            {
                fadeTimer.Dispose();
                fadeTimer = null;
            }
        }

        private void FillBuffer()
        {
            const int BUFFER_SIZE = 64 * 1024;

            float[] samples = new float[BUFFER_SIZE];
            Array.Clear(samples, 0, samples.Length);
            int read_total = 0;

            while (read_total < BUFFER_SIZE)
            {
                var pos = vorbisReader.DecodedPosition;

                if (pos >= LoopRegionEnd)
                    vorbisReader.DecodedPosition = LoopRegionStart;

                var bufferSizeToRead = BUFFER_SIZE - read_total;
                var read = vorbisReader.ReadSamples(samples, read_total, bufferSizeToRead);

                read_total += read;
                pos = vorbisReader.DecodedPosition;
                if (pos > LoopRegionEnd)
                {
                    read_total -= (int)(pos - LoopRegionEnd);
                }
            }

            byte[] sBuffer = new byte[BUFFER_SIZE * 2];
            for (int i = 0; i < BUFFER_SIZE; i++)
            {
                var sample = samples[i];
                sample = sample > 1.0f ? 1.0f :
                    sample < -1.0f ? -1.0f :
                    sample;

                short sSample = (short)(sample * short.MaxValue);

                sBuffer[i * 2 + 1] = (byte)(sSample / 256);
                sBuffer[(i * 2)] = (byte)(sSample % 256);
            }

            soundEffectInstance.SubmitBuffer(sBuffer);
        }
    }
}