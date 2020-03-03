using System;

namespace Exportador_LB_to_ES.AD.Models
{
    public class Dodf
    {
        public Dodf()
        {

        }

        public string DataDaAssinatura { get; set; }
        public string ConteudoArquivoTexto { get; set; }
        public string CaminhoArquivoTexto { get; set; }
        public string NomeArquivoTexto { get; set; }
        public byte[] DadosDoArquivo { get; set; }
        public int Id { get; set; }
        public string OrgaoCadastrador { get; set; }
        public string UsuarioQueCadastrou { get; set; }
        public DateTime DataDoCadastro { get; set; }
        public string UsuarioDaUltimaAlteracao { get; set; }
        public DateTime DataDaUltimaAlteracao { get; set; }
        public string ChaveParaNaoDuplicacao { get; set; }
        public string SituacaoQuantoAPendencia { get; set; }
        public int Numero { get; set; }
        public string AlfaNumero { get; set; }
        public string Sessao { get; set; }
        public string IdSileg { get; set; }
        public string Rotulo { get; set; }
        public string Comentario { get; set; }
    }
}
