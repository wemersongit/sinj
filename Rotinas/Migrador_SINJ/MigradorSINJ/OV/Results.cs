using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class Results<T>
    {
        public Results()
        {
            results = new List<T>();
        }
        public ulong result_count { get; set; }
        public List<T> results { get; set; }
        public ulong offset { get; set; }
        public ulong limit { get; set; }
    }
}
