using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCDF.Portal
{
    public class ESException : Exception
    {
        public ESException(string mensagem) : base(mensagem)
        {

        }
        public ESException(string mensagem, Exception ex) : base(mensagem, ex)
        {

        }
    }
}
