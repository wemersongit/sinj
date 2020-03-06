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
        /// Esse campo funciona como um flag, é true apenas quando o tipo de relação é usada para Ações com outras normas;
        /// </summary>
        public bool RelacaoDeAcao { get; set; }

    }
}