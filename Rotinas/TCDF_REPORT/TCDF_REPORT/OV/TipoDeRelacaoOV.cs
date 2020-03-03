namespace TCDF_REPORT.OV
{
    public class TipoDeRelacaoOV
    {
        public int Oid { get; set; }
        public string Conteudo { get; set; }
        public string Descricao { get; set; }
        public string TextoParaAlterador { get; set; }
        public string TextoParaAlterado { get; set; }
        public int Importancia { get; set; }

        /// <summary>
        /// Esse campo funciona como um flag, � true apenas quando o tipo de rela��o � usada para A��es com outras normas;
        /// </summary>
        public bool RelacaoDeAcao { get; set; }

    }
}