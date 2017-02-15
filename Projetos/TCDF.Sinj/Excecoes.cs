using System;
using System.Collections.Generic;

namespace TCDF.Sinj
{
    public class DocValidacaoException : ApplicationException
    {
        public DocValidacaoException(string mensagem) : base(mensagem)
        {

        }
    }
    public class DocNotFoundException : ApplicationException
    {
        public DocNotFoundException(string mensagem)
            : base(mensagem)
        {

        }
    }
    public class DocDependenciesException : ApplicationException
    {
        public DocDependenciesException(string mensagem)
            : base(mensagem)
        {

        }
    }
    public class DocDuplicateKeyException : ApplicationException
    {
        public DocDuplicateKeyException(string mensagem): base(mensagem)
        {

        }
    }
    public class SessionNotFoundException : ApplicationException
    {
        public SessionNotFoundException(string mensagem)
            : base(mensagem)
        {

        }
    }
    public class SessionExpiredException : ApplicationException
    {
        public SessionExpiredException(string mensagem) : base(mensagem)
        {

        }
    }
    public class PermissionException : ApplicationException
    {
        public PermissionException(string mensagem) : base(mensagem)
        {

        }
    }
}