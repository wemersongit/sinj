using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class ArquivoOV
    {
        public string filename { get; set; }
        public ulong filesize { get; set; }
        public string id_file { get; set; }
        public string mimetype { get; set; }
        public string uuid { get; set; }
    }
}
