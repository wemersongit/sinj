using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCDF.Portal.OV
{
    public class DatatableNet<T>
    {
        public List<T> aaData { get; set; }
        public string sEcho { get; set; }
        public string offset { get; set; }
        public string iTotalRecords { get; set; }
        public ulong iTotalDisplayRecords { get; set; }
    }
}
