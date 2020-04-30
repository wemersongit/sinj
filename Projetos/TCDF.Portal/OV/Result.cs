using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCDF.Portal.OV
{
    public class Result<T>
    {
        public ulong took { get; set; }
        public bool timed_out { get; set; }
        public Shards _shards { get; set; }
        public Hits<T> hits { get; set; }
    }
}
