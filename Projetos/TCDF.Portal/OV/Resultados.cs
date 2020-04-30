using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCDF.Portal.OV
{
    public class Resultados<T>
    {
        public static ulong num = 0;
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public string _score { get; set; }
        public T _source { get; set; }
        public Fields<T> fields { get; set; }
        public object highlight { get; set; }

    }
}
