using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBGM
{
    public class SongReader : ContentTypeReader<Song>
    {
        protected override Song Read(ContentReader input, Song existingInstance)
        {
            string fileName = input.ReadString();
            long loopstart = input.ReadInt64();
            long loopend = input.ReadInt64();

            return new Song(Path.Combine(input.ContentManager.RootDirectory, fileName))
            {
                LoopRegionStart = loopstart,
                LoopRegionEnd = loopend
            };
        }
    }
}