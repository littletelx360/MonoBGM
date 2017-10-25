using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace MonoBGM.ContentPipeline
{
    [ContentTypeWriter]
    public class BGMSongWriter : ContentTypeWriter<SongDetails>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SongReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, SongDetails value)
        {
            output.Write(value.Filename);
            output.Write(value.LoopRegionStart);
            output.Write(value.LoopRegionEnd);
        }
    }
}