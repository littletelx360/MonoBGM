using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;
using NVorbis;

namespace MonoBGM.ContentPipeline
{
    [ContentProcessor(DisplayName = "MonoBGM Song Processor")]
    public class BGMSongProcessor : ContentProcessor<string, SongDetails>
    {
        public override SongDetails Process(string input, ContentProcessorContext context)
        {
            var path = input;
            var reader = new VorbisReader(path);
            long loopstart = 0;
            long loopend = reader.TotalSamples;

            foreach (var c in reader.Comments)
            {
                if (c.StartsWith("loop_start"))
                {
                    long.TryParse(c.Substring("loop_start=".Length), out loopstart);
                }
                else if (c.StartsWith("loop_end"))
                {
                    long.TryParse(c.Substring("loop_end=".Length), out loopend);
                }
            }

            reader.Dispose();

            return new SongDetails
            {
                Filename = input,
                LoopRegionStart = loopstart,
                LoopRegionEnd = loopend
            };
        }
    }
}