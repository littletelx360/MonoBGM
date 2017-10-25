using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBGM
{
    public struct SongDetails
    {
        public string Filename { get; set; }
        public long LoopRegionStart { get; set; }
        public long LoopRegionEnd { get; set; }
    }
}