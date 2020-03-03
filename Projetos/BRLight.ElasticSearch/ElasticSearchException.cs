using System;

namespace BRLight.ElasticSearch
{
    public class ElasticSearchException : Exception
    {
        public ElasticSearchException(string mensagem, Exception exception) : base(mensagem)
        {

        }
    }
}