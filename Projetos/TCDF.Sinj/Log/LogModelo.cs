using neo.BRLightREST;

namespace TCDF.Sinj.Log
{

    public class LogAlteracao
    {
        public string Atividade { get; set; }
        public string Valor { get; set; }
    }

    public class LogGenerico
    {
        public ulong id_doc { get; set; }
        public string Mensagem { get; set; }
        public string Url { get; set; }
    }

    public class Recuperar
    {
        public string Nome_Base { get; set; }
        public ulong id_doc_na_base_excluido { get; set; }
        public ulong id_doc_antigo { get; set; }
        public ulong id_doc_novo { get; set; }
    }

    public class LogRecuperar
    {
        public LogRecuperar()
        {
            Dados = new Recuperar();
        }
        public Recuperar Dados { get; set; }
    }

    public class LogErroNET
    {
        public LogErroNET()
        {
            Erro = new ErroNET();
        }
        public ErroNET Erro { get; set; }
    }

    public class ErroAjax
    {
        public string Mensagem { get; set; }
        public string Url { get; set; }
        public string Pagina { get; set; }
    }

    public class ErroJS
    {
        public string Linha { get; set; }
        public string Mensagem { get; set; }
        public string Pagina { get; set; }
        public string Url { get; set; }
    }

    public class ErroNET
    {
        public string Code { get; set; }
        public string Mensagem { get; set; }
        public string Trace { get; set; }
        public string Source { get; set; }
        public string Pagina { get; set; }
    }

	public class Erro
	{
		public string MensagemDaExcecao { get; set; }
		public string StackTrace { get; set; }
	}

	public class ErroRequest: Erro
    {
        public string Pagina { get; set; }
        public System.Collections.Specialized.NameValueCollection RequestQueryString { get; set; }
    }

	public class ErroPush: Erro
	{
		public string NotifiquemeOv { get; set;}
		public string Pesquisa { get; set;}
		public string NormaOv { get; set;}
	}

    public class LogErroAjax
    {
        public LogErroAjax()
        {
            Erro = new ErroAjax();
        }
        public ErroAjax Erro { get; set; }
    }

    public class LogErroJS
    {
        public LogErroJS()
        {
            Erro = new ErroJS();
        }
        public ErroJS Erro { get; set; }
    }

    public class LogOperacaoOV <T>
    {
        public LogOperacaoOV()
        {
            
        }
        public string ch_tipo_operacao { get; set; }
        public T operacao { get; set; }
    }

    public class LogBuscar
    {
        public LogBuscar()
        {
            PesquisaLight = new Pesquisa();
        }
        public string RegistrosPorPagina { get; set; }
        public string RegistroInicial { get; set; }
        public string RequestNumero { get; set; }
        public string RegistrosTotal { get; set; }
        public string Pesquisa { get; set; }
        public Pesquisa PesquisaLight { get; set; }
        public string PesquisaEs { get; set; }
    }

    public class LogRelatorio
    {
        public LogRelatorio()
        {
            PesquisaLight = new Pesquisa();
        }
        public string RegistrosTotal { get; set; }
        public string Pesquisa { get; set; }
        public Pesquisa PesquisaLight { get; set; }
        public string PesquisaEs { get; set; }
    }

    public class LogIncluir<T>
    {
        public T registro { get; set; }
    }

    public class LogAlterar<T>
    {
        public ulong id_doc { get; set; }
        public T registro { get; set; }
    }

    public class LogPutPath<T>
    {
        public ulong id_doc { get; set; }
        public string path { get; set; }
        public string value { get; set; }
    }

    public class LogUpload
    {
        public ArquivoOV arquivo { get; set; }
    }

    public class LogDownload
    {
        public ArquivoOV arquivo { get; set; }
    }

    public class LogExcluir
    {
        public ulong id_doc { get; set; }
        public string nm_base { get; set; }
    }

    public class LogEmail
    {
        public string[] emails { get; set; }
        public string assunto { get; set; }
        public string mensagem { get; set; }
    }

    public class LogVisualizar
    {
        public ulong id_doc { get; set; }
        public string ch_doc { get; set; }
    }

    public class LogSair
    {
        public ulong id_doc { get; set; }
        public string usuario { get; set; }
        public ulong sessao_id { get; set; }
        public string sessao_chave { get; set; }
    }
}
