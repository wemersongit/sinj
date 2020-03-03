using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCDF.Portal.OV
{
    public class Shards
    {
        public ulong? total { get; set; }

        public ulong? successful { get; set; }

        public ulong? failed { get; set; }
    }
}
