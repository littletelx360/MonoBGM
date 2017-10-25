using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using NVorbis;
using MonoBGM;

namespace MonoBGM.ContentPipeline
{
    [ContentImporter(".ogg", DisplayName = "MonoBGM Song Importer", DefaultProcessor = "SongProcessor")]
    public class OggBGMImporter : ContentImporter<String>
    {
        public override String Import(string filename, ContentImporterContext context)
        {
            return Path.GetFullPath(filename);
        }
    }
}