using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCDF.Portal.OV
{
    public class Hits<T>
    {
        public ulong total { get; set; }

        public Double? max_score { get; set; }

        public List<Resultados<T>> hits { get; set; }

        public string LastId()
        {
            return null;
        }
    }
}
