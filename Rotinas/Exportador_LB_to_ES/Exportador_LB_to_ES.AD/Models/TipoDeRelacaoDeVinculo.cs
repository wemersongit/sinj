namespace Exportador_LB_to_ES.AD.Models
{
    public class TipoDeRelacaoDeVinculo
    {
        public int Oid { get; set; }
        public string Conteudo { get; set; }
        public string Descricao { get; set; }
        public string TextoParaAlterador { get; set; }
        public string TextoParaAlterado { get; set; }
        public int Importancia { get; set; }

        /// <summary>
        /// Esse campo funciona como um flag, é true apenas quando o tipo de relação é usada para Ações com outras normas;
        /// </summary>
        public bool RelacaoDeAcao { get; set; }

        /// <summary>
        /// Esse campo apenas é preenchido durante a migração do SILEG.
        /// Serve para guardar informações sobre pendencia durante a conversão entre Tipo De Relação do SILEG para o Tipo do SINJ.
        /// </summary>
        public string Pendencia { get; set; }
    }
}
