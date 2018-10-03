using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using System.Text.RegularExpressions;
using TCDF.Sinj.ES;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.Portal.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for SugestoesConsulta
    /// </summary>
    public class SugestoesConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sAction = "";
            string sRetorno = "";
            string _bbusca = context.Request["bbusca_sugestoes"];

            try
            {

                switch (_bbusca)
                {
                    case "sinj_norma":
                        sRetorno = "{\"nm_base\":\"" + _bbusca + "\",\"ds_base\":\"Normas\",\"sugestoes\":" + BuscarSugestoesDeBuscasDeNormas(context) + "}";
                        break;
                    case "sinj_diario":
                        sRetorno = "{\"nm_base\":\"" + _bbusca + "\",\"ds_base\":\"Diários\",\"sugestoes\":" + BuscarSugestoesDeBuscasDeDiarios(context) + "}";
                        break;

                }
            }
            catch (Exception ex)
            {
                sRetorno = "{\"counts\":[{\"nm_base\":\"sinj_diario\",\"ds_base\":\"Normas\",\"count\":0},{\"nm_base\":\"sinj_diario\",\"ds_base\":\"Diários\",\"count\":0}]}";
                
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                var nm_usuario = "visitante";
                var nm_login_usuario = "visitante";

                SessaoNotifiquemeOV sessao_push = Util.LerSessaoPush();
                if (sessao_push != null)
                {
                    nm_usuario = sessao_push.nm_usuario_push;
                    nm_login_usuario = sessao_push.email_usuario_push;
                }
                LogErro.gravar_erro(sAction, erro, nm_usuario, nm_login_usuario);
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(sRetorno);
            context.Response.End();
        }

        private string BuscarSugestoesDeBuscasDeNormas(HttpContext context)
        {
            string _tipo_pesquisa = context.Request["tipo_pesquisa"];

            var i = 0;
            var query = "";
            var historicoDePesquisaRn = new HistoricoDePesquisaRN();
            var pesquisa = new HistoricoDePesquisaOV();
            var sugestoes = new Dictionary<string, SugestaoDeBusca>();
            var listaSugestoes = new List<SugestaoDeBusca>();
            var nomarAd = new NormaAD();
            var result = new Result<NormaOV>();
            var _all = "";
            var facets = "";
            switch (_tipo_pesquisa)
            {
                case "norma":
                    _all = context.Request["all"];
                    if (!string.IsNullOrEmpty(_all))
                    {
                        SentencaPesquisaDiretaNormaOV sentencaOv = new SentencaPesquisaDiretaNormaOV();
                        sentencaOv.ch_tipo_norma = context.Request["ch_tipo_norma"];
                        sentencaOv.nm_tipo_norma = context.Request["nm_tipo_norma"];
                        sentencaOv.nr_norma = context.Request["nr_norma"];
                        sentencaOv.norma_sem_numero = context.Request["norma_sem_numero"];
                        sentencaOv.ano_assinatura = context.Request["ano_assinatura"];
                        sentencaOv.ch_orgao = context.Request["ch_orgao"];
                        sentencaOv.nm_orgao = context.Request["sg_hierarquia_nm_vigencia"];
                        sentencaOv.origem_por = context.Request["origem_por"];
                        sentencaOv.nm_termos = context.Request.Params.GetValues("nm_termo");
                        sentencaOv.ch_termos = context.Request.Params.GetValues("ch_termo");
                        sentencaOv.isCount = true;
                        var combinacoes = CombinarTermosPesquisa(_all);

                        foreach (var combinacao in combinacoes)
                        {
                            sentencaOv.all = combinacao;
                            historicoDePesquisaRn.MontarPesquisa(sentencaOv, pesquisa);
                            sugestoes.Add("query" + i, new SugestaoDeBusca() { count = 0, ds_sugestao = pesquisa.ds_historico, params_sugestao = pesquisa.consulta, nr_termos = pesquisa.ds_historico.Length });
                            facets += (facets != "" ? "," : "") + "\"query" + i + "\":" + new NormaBuscaEs().MontarBusca(sentencaOv).GetQuery();
                            i++;
                        }
                        facets = "{\"size\": 0, \"facets\" :{" + facets + "}}";
                        query = facets;
                        result = nomarAd.ConsultarEs(query);
                    }
                    break;
                case "avancada":
                    var argumentos = context.Request.Params.GetValues("argumento");
                    if (argumentos != null && argumentos.Length > 0)
                    {
                        SentencaPesquisaAvancadaNormaOV sentencaPesquisaAvancada = new SentencaPesquisaAvancadaNormaOV();
                        sentencaPesquisaAvancada.isCount = true;
                        sentencaPesquisaAvancada.argumentos = new string[argumentos.Length];
                        sentencaPesquisaAvancada.ch_tipo_norma = context.Request.Params.GetValues("ch_tipo_norma");
                        for (var iArg = 0; iArg < argumentos.Length; iArg++)
                        {
                            var argumento_split = argumentos[iArg].Split('#');
                            if (argumento_split.Length == 8 && argumento_split[0].Equals("text"))
                            {
                                var combinacoes = CombinarTermosPesquisa(argumento_split[5]);
                                foreach(var combinacao in combinacoes){
                                    argumento_split[5] = combinacao;
                                    argumento_split[6] = combinacao;
                                    var newArgumento = string.Join("#", argumento_split);
                                    argumentos.CopyTo(sentencaPesquisaAvancada.argumentos, 0);
                                    sentencaPesquisaAvancada.argumentos[iArg] = newArgumento;

                                    historicoDePesquisaRn.MontarPesquisa(sentencaPesquisaAvancada, pesquisa);

                                    sugestoes.Add("query" + i, new SugestaoDeBusca() { count = 0, ds_sugestao = pesquisa.ds_historico, params_sugestao = pesquisa.consulta, nr_termos = pesquisa.ds_historico.Length });
                                    facets += (facets != "" ? "," : "") + "\"query" + i + "\":" + new NormaBuscaEs().MontarBusca(sentencaPesquisaAvancada).GetQuery();
                                    i++;
                                }
                                facets = "{\"size\": 0, \"facets\" :{" + facets + "}}";
                                query = facets;
                                result = nomarAd.ConsultarEs(query);
                            }
                        }
                    }


                    break;
                default:

                    _all = context.Request["all"];
                    if (!string.IsNullOrEmpty(_all))
                    {
                        SentencaPesquisaGeralOV sentencaOv = new SentencaPesquisaGeralOV();
                        sentencaOv.isCount = true;
                        var combinacoes = CombinarTermosPesquisa(_all);
                        foreach(var combinacao in combinacoes){
                            sentencaOv.all = combinacao;
                            historicoDePesquisaRn.MontarPesquisa(sentencaOv, pesquisa);
                            sugestoes.Add("query" + i, new SugestaoDeBusca() { count = 0, ds_sugestao = pesquisa.ds_historico, params_sugestao = pesquisa.consulta, nr_termos = pesquisa.ds_historico.Length});
                            facets += (facets != "" ? "," : "") + "\"query"+i+"\":" + new NormaBuscaEs().MontarBusca(sentencaOv).GetQuery();
                            i++;
                        }
                        facets = "{\"size\": 0, \"facets\" :{" + facets + "}}";
                        query = facets;
                        result = nomarAd.ConsultarEs(query);
                    }
                    break;
            }
            if (result.facets != null && result.facets.Count > 0)
            {
                foreach (var facet in result.facets)
                {
                    if (facet.Value.count > 0)
                    {
                        sugestoes[facet.Key].count = facet.Value.count;
                        listaSugestoes.Add(sugestoes[facet.Key]);
                    }
                }

            }
            return JSON.Serialize<IOrderedEnumerable<SugestaoDeBusca>>(listaSugestoes.OrderByDescending<SugestaoDeBusca, int>(s => s.nr_termos));
        }

        private string BuscarSugestoesDeBuscasDeDiarios(HttpContext context)
        {
            string _tipo_pesquisa = context.Request["tipo_pesquisa"];

            var i = 0;
            var query = "";
            var historicoDePesquisaRn = new HistoricoDePesquisaRN();
            var pesquisa = new HistoricoDePesquisaOV();
            var sugestoes = new Dictionary<string, SugestaoDeBusca>();
            var listaSugestoes = new List<SugestaoDeBusca>();
            var diarioAd = new DiarioAD();
            var result = new Result<DiarioOV>();
            var _filetext = "";
            switch (_tipo_pesquisa)
            {
                case "diario":
                    _filetext = context.Request["filetext"];
                    if (!string.IsNullOrEmpty(_filetext))
                    {
                        SentencaPesquisaDiretaDiarioOV sentencaPesquisaDiretaDiarioOv = new SentencaPesquisaDiretaDiarioOV();
                        sentencaPesquisaDiretaDiarioOv.isCount = true;
                        sentencaPesquisaDiretaDiarioOv.ds_norma = context.Request["ds_norma"];
                        Util.rejeitarInject(context.Request["ch_tipo_fonte"]);
                        sentencaPesquisaDiretaDiarioOv.ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                        sentencaPesquisaDiretaDiarioOv.nm_tipo_fonte = context.Request["nm_tipo_fonte"];
                        sentencaPesquisaDiretaDiarioOv.ch_tipo_edicao = context.Request["ch_tipo_edicao"];
                        sentencaPesquisaDiretaDiarioOv.nm_tipo_edicao = context.Request["nm_tipo_edicao"];
                        sentencaPesquisaDiretaDiarioOv.nr_diario = context.Request["nr_diario"];
                        sentencaPesquisaDiretaDiarioOv.secao_diario = context.Request.Params.GetValues("secao_diario");
                        sentencaPesquisaDiretaDiarioOv.op_dt_assinatura = context.Request["op_dt_assinatura"];
                        sentencaPesquisaDiretaDiarioOv.dt_assinatura = context.Request.Params.GetValues("dt_assinatura");
                        var combinacoes = CombinarTermosPesquisa(_filetext);
                        var facets = "";
                        foreach (var combinacao in combinacoes)
                        {
                            sentencaPesquisaDiretaDiarioOv.filetext = combinacao;
                            historicoDePesquisaRn.MontarPesquisa(sentencaPesquisaDiretaDiarioOv, pesquisa);
                            sugestoes.Add("query" + i, new SugestaoDeBusca() { count = 0, ds_sugestao = pesquisa.ds_historico, params_sugestao = pesquisa.consulta, nr_termos = pesquisa.ds_historico.Length });
                            facets += (facets != "" ? "," : "") + "\"query" + i + "\":" + new DiarioBuscaEs().MontarBusca(sentencaPesquisaDiretaDiarioOv).GetQuery();
                            i++;
                        }
                        facets = "{\"size\": 0, \"facets\" :{" + facets + "}}";
                        query = facets;
                        result = diarioAd.ConsultarEs(query);
                    }
                    break;
                case "notifiqueme":
                    _filetext = context.Request["filetext"];
                    if (!string.IsNullOrEmpty(_filetext))
                    {
                        SentencaPesquisaNotifiquemeDiarioOV sentencaPesquisaNotifiquemeDiarioOv = new SentencaPesquisaNotifiquemeDiarioOV();
                        sentencaPesquisaNotifiquemeDiarioOv.isCount = true;
                        Util.rejeitarInject(context.Request["ch_tipo_fonte"]);
                        sentencaPesquisaNotifiquemeDiarioOv.ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                        sentencaPesquisaNotifiquemeDiarioOv.nm_tipo_fonte = context.Request["nm_tipo_fonte"];
                        sentencaPesquisaNotifiquemeDiarioOv.in_exata = context.Request["in_exata"];
                        var combinacoes = CombinarTermosPesquisa(_filetext);
                        var facets = "";
                        foreach (var combinacao in combinacoes)
                        {
                            sentencaPesquisaNotifiquemeDiarioOv.filetext = combinacao;
                            historicoDePesquisaRn.MontarPesquisa(sentencaPesquisaNotifiquemeDiarioOv, pesquisa);
                            sugestoes.Add("query" + i, new SugestaoDeBusca() { count = 0, ds_sugestao = pesquisa.ds_historico, params_sugestao = pesquisa.consulta, nr_termos = pesquisa.ds_historico.Length });
                            facets += (facets != "" ? "," : "") + "\"query" + i + "\":" + new DiarioBuscaEs().MontarBusca(sentencaPesquisaNotifiquemeDiarioOv).GetQuery();
                            i++;
                        }
                        facets = "{\"size\": 0, \"facets\" :{" + facets + "}}";
                        query = facets;
                        result = diarioAd.ConsultarEs(query);
                    }
                    break;
                default:

                    var _all = context.Request["all"];
                    if (!string.IsNullOrEmpty(_all))
                    {
                        SentencaPesquisaGeralOV sentencaOv = new SentencaPesquisaGeralOV();
                        sentencaOv.isCount = true;
                        var combinacoes = CombinarTermosPesquisa(_all);
                        var facets = "";
                        foreach (var combinacao in combinacoes)
                        {
                            sentencaOv.all = combinacao;
                            historicoDePesquisaRn.MontarPesquisa(sentencaOv, pesquisa);
                            sugestoes.Add("query" + i, new SugestaoDeBusca() { count = 0, ds_sugestao = pesquisa.ds_historico, params_sugestao = pesquisa.consulta, nr_termos = pesquisa.ds_historico.Length });
                            facets += (facets != "" ? "," : "") + "\"query" + i + "\":" + new DiarioBuscaEs().MontarBusca(sentencaOv).GetQuery();
                            i++;
                        }
                        facets = "{\"size\": 0, \"facets\" :{" + facets + "}}";
                        query = facets;
                        result = diarioAd.ConsultarEs(query);
                    }
                    break;
            }
            if (result.facets != null && result.facets.Count > 0)
            {
                foreach (var facet in result.facets)
                {
                    if (facet.Value.count > 0)
                    {
                        sugestoes[facet.Key].count = facet.Value.count;
                        listaSugestoes.Add(sugestoes[facet.Key]);
                    }
                }

            }
            return JSON.Serialize<IOrderedEnumerable<SugestaoDeBusca>>(listaSugestoes.OrderByDescending<SugestaoDeBusca, int>(s => s.nr_termos));
        }

        private List<string> CombinarTermosPesquisa(string texto)
        {
            texto = texto.Trim();
            var combinacoes = new List<string>();
            if (!string.IsNullOrEmpty(texto))
            {
                var termos = texto.Split(' ');
                if (termos.Length > 1)
                {
                    if (Regex.Match(texto, "[\"']").Success)
                    {
                        texto = Regex.Replace(texto, "[\"']", "");
                        combinacoes.Add(texto);
                    }
                    termos = texto.Split(' ');
                    var listTermos = new List<string>();
                    int iTeste = 0;
                    for (var i = 0; i < termos.Length; i++)
                    {
                        if(termos[i].Length > 2 || int.TryParse(termos[i], out iTeste)){
                            listTermos.Add(termos[i]);
                        }
                    }
                    termos = listTermos.ToArray();
                    if(termos.Length > 0){
                        for (var i = 0; i < termos.Length; i++)
                        {
                            MontarListaDeCombinacoesRecursivo(termos[i], termos.Skip(i + 1).ToArray(), combinacoes);
                        }
                    }
                }
            }
            return combinacoes;
        }

        private void MontarListaDeCombinacoesRecursivo(string texto, string[] termos, List<string> combinacoes)
        {
            if (!combinacoes.Contains(texto))
            {
                combinacoes.Add(texto);
            }
            for (var i = 0; i < termos.Length; i++)
            {
                MontarListaDeCombinacoesRecursivo(texto + " " + termos[i], termos.Skip(i + 1).ToArray(), combinacoes);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class SugestaoDeBusca
    {
        public int nr_termos { get; set; }
        public ulong count { get; set; }
        public string ds_sugestao { get; set; }
        public string params_sugestao { get; set; }
    }
}