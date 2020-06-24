using System.Collections.Generic;

namespace TCDF.Sinj.OV
{
    public class SentencaPesquisaSinjOV
    {
        public SentencaPesquisaSinjOV()
        {
            sentencaOrdenamento = new SentencaOrdenamentoOV();
            filtros = new string[0];
        }
        public SentencaOrdenamentoOV sentencaOrdenamento { get; set; }
        public string search { get; set; }
        public bool isCount { get; set; }
        public string[] filtros { get; set; }
        public ulong iDisplayStart { get; set; }
        public ulong iDisplayLength { get; set; }
    }

    public class SentencaOrdenamentoOV
    {
        public string sSortDir { get; set; }
        public string sColOrder { get; set; }
    }

    public class SentencaPesquisaGeralOV : SentencaPesquisaSinjOV
    {
        public string all { get; set; }
    }
    public class SentencaPesquisaDiretaNormaOV : SentencaPesquisaSinjOV
    {
        public string all { get; set; }
        public string ch_tipo_norma { get; set; }
        public string nm_tipo_norma { get; set; }
        public string nr_norma { get; set; }
        public string norma_sem_numero { get; set; }
        public string ano_assinatura { get; set; }
        public string dt_assinatura { get; set; }
        public string ch_orgao { get; set; }
        public string nm_orgao { get; set; }
        public string[] ch_termos { get; set; }
        public string[] nm_termos { get; set; }
        public string ch_hierarquia { get; set; }
        public string origem_por { get; set; }
<<<<<<< HEAD
=======
        public string st_habilita_pesquisa { get; set; }
        public string st_habilita_email { get; set; }
        public string nm_orgao_cadastrador { get; set; }

>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d

    }
    public class SentencaPesquisaAvancadaNormaOV : SentencaPesquisaSinjOV
    {
        public string[] ch_tipo_norma { get; set; }
        public string[] argumentos { get; set; }
    }
    public class SentencaPesquisaDiretaDiarioOV : SentencaPesquisaSinjOV
    {
        public string ds_norma { get; set; }
        public string ch_tipo_fonte { get; set; }
        public string nm_tipo_fonte { get; set; }
        public string ch_tipo_edicao { get; set; }
        public string nm_tipo_edicao { get; set; }
        public string nr_diario { get; set; }
        public string[] secao_diario { get; set; }
        public string filetext { get; set; }
        public string op_dt_assinatura { get; set; }
        public string[] dt_assinatura { get; set; }
    }
    public class SentencaPesquisaDiretorioDiarioOV : SentencaPesquisaSinjOV
    {
        public string ch_tipo_fonte { get; set; }
        public string nm_tipo_fonte { get; set; }
        public string ano { get; set; }
        public string mes { get; set; }
    }
    public class SentencaPesquisaTextoDiarioOV : SentencaPesquisaSinjOV
    {
        public string ch_tipo_fonte { get; set; }
        public string nm_tipo_fonte { get; set; }
        public string filetext { get; set; }
        public string intervalo { get; set; }
        public string ano { get; set; }
        public string dt_assinatura_inicio { get; set; }
        public string dt_assinatura_termino { get; set; }
    }
    public class SentencaPesquisaNotifiquemeDiarioOV : SentencaPesquisaSinjOV
    {
        public string ch_tipo_fonte { get; set; }
        public string nm_tipo_fonte { get; set; }
        public string filetext { get; set; }
        public string in_exata { get; set; }
    }
    public class SentencaPesquisaCestaOV : SentencaPesquisaSinjOV
    {
        public string cesta { get; set; }
        public string @base { get; set; }
    }
    public class SentencaPesquisaFavoritosOV : SentencaPesquisaSinjOV
    {
        public string[] favoritos { get; set; }
        public string @base { get; set; }
    }
}
