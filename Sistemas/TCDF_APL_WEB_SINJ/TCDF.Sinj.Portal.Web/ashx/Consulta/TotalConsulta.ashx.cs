using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.ES;
using TCDF.Sinj.AD;
using TCDF.Sinj.Portal.Web.ashx.Datatable;

namespace TCDF.Sinj.Portal.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for TotalConsulta
    /// </summary>
    public class TotalConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sAction = "";
            string sRetorno = "";
            string _bbusca = context.Request["bbusca"];

            try
            {

                switch (_bbusca)
                {
                    case "sinj_norma":
                        sRetorno = "{\"counts\":[{\"nm_base\":\"" + _bbusca + "\",\"ds_base\":\"Normas\",\"count\":" + BuscarTotalDeNormas(context) + "}]}";
                        break;
                    case "sinj_diario":
                        sRetorno = "{\"counts\":[{\"nm_base\":\"" + _bbusca + "\",\"ds_base\":\"Diários\",\"count\":" + BuscarTotalDeDiarios(context) + "}]}";
                        break;
                    default:
                        sRetorno = "{\"counts\":[{\"nm_base\":\"sinj_norma\",\"ds_base\":\"Normas\",\"count\":" + BuscarTotalDeNormas(context) + "},{\"nm_base\":\"sinj_diario\",\"ds_base\":\"Diários\",\"count\":" + BuscarTotalDeDiarios(context) + "}]}";
                        break;
                }

            }
            catch (Exception ex)
            {
                sRetorno = "{\"counts\":[{\"nm_base\":\"sinj_norma\",\"ds_base\":\"Normas\",\"count\":0},{\"nm_base\":\"sinj_diario\",\"ds_base\":\"Diários\",\"count\":0}]}";
                
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

        private ulong BuscarTotalDeNormas(HttpContext context)
        {
            string _tipo_pesquisa = context.Request["tipo_pesquisa"];

            var query = "";

            switch (_tipo_pesquisa)
            {
                case "norma":
                    SentencaPesquisaDiretaNormaOV pesquisaDireta = new SentencaPesquisaDiretaNormaOV();
                    pesquisaDireta.all = context.Request["all"];
                    pesquisaDireta.ch_tipo_norma = context.Request["ch_tipo_norma"];
                    pesquisaDireta.nr_norma = context.Request["nr_norma"];
                    pesquisaDireta.norma_sem_numero = context.Request["norma_sem_numero"];
                    pesquisaDireta.ano_assinatura = context.Request["ano_assinatura"];
                    pesquisaDireta.ch_orgao = context.Request["ch_orgao"];
                    pesquisaDireta.ch_hierarquia = context.Request["ch_hierarquia"];
                    pesquisaDireta.nm_orgao = context.Request["sg_hierarquia_nm_vigencia"];
                    pesquisaDireta.origem_por = context.Request["origem_por"];
                    pesquisaDireta.ch_termos = context.Request.Params.GetValues("ch_termo");
                    pesquisaDireta.isCount = true;
                    var buscaDireta = new NormaBuscaEs().MontarBusca(pesquisaDireta);
                    query = buscaDireta.GetQuery();
                    break;
                case "avancada":
                    SentencaPesquisaAvancadaNormaOV pesquisaAvancada = new SentencaPesquisaAvancadaNormaOV();
                    pesquisaAvancada.ch_tipo_norma = context.Request.Params.GetValues("ch_tipo_norma");
                    pesquisaAvancada.argumentos = context.Request.Params.GetValues("argumento");
                    pesquisaAvancada.isCount = true;
                    var buscaAvancada = new NormaBuscaEs().MontarBusca(pesquisaAvancada);
                    query = buscaAvancada.GetQuery();
                    break;
                default:
                    SentencaPesquisaGeralOV pesquisaGeral = new SentencaPesquisaGeralOV();
                    pesquisaGeral.all = context.Request["all"];
                    pesquisaGeral.isCount = true;
                    var buscaGeral = new NormaBuscaEs().MontarBusca(pesquisaGeral);
                    query = buscaGeral.GetQuery();                    
                    break;
            }

            return new NormaAD().ConsultarEs(query).hits.total;
        }

        private ulong BuscarTotalDeDiarios(HttpContext context)
        {
            string _tipo_pesquisa = context.Request["tipo_pesquisa"];

            var query = "";

            switch (_tipo_pesquisa)
            {
                case "diario":
                    SentencaPesquisaDiretaDiarioOV pesquisaDireta = new SentencaPesquisaDiretaDiarioOV();
                    pesquisaDireta.ds_norma = context.Request["ds_norma"];
                    Util.rejeitarInject(context.Request["ch_tipo_fonte"]);
                    pesquisaDireta.ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                    pesquisaDireta.ch_tipo_edicao = context.Request["ch_tipo_edicao"];
                    pesquisaDireta.nr_diario = context.Request["nr_diario"];
                    pesquisaDireta.secao_diario = context.Request.Form.GetValues("secao_diario");
                    pesquisaDireta.filetext = context.Request["filetext"];
                    pesquisaDireta.op_dt_assinatura = context.Request["op_dt_assinatura"];
                    pesquisaDireta.dt_assinatura = context.Request.Form.GetValues("dt_assinatura");
                    pesquisaDireta.isCount = true;
                    var buscaDireta = new DiarioBuscaEs().MontarBusca(pesquisaDireta);
                    query = buscaDireta.GetQuery();
                    break;
                case "notifiqueme":
                    SentencaPesquisaNotifiquemeDiarioOV pesquisaNotifiqueme = new SentencaPesquisaNotifiquemeDiarioOV();
                    Util.rejeitarInject(context.Request["ch_tipo_fonte"]);
                    pesquisaNotifiqueme.ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                    pesquisaNotifiqueme.filetext = context.Request["filetext"];
                    pesquisaNotifiqueme.in_exata = context.Request["in_exata"];
                    pesquisaNotifiqueme.isCount = true;
                    var buscaNotifiqueme = new DiarioBuscaEs().MontarBusca(pesquisaNotifiqueme);
                    query = buscaNotifiqueme.GetQuery();
                    break;
                case "texto_diario":
                    SentencaPesquisaTextoDiarioOV pesquisaTexto = new SentencaPesquisaTextoDiarioOV();
                    Util.rejeitarInject(context.Request["ch_tipo_fonte"]);
                    pesquisaTexto.ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                    pesquisaTexto.filetext = context.Request["filetext"];
                    pesquisaTexto.intervalo = context.Request["intervalo"];
                    pesquisaTexto.dt_assinatura_inicio = context.Request["dt_assinatura_inicio"];
                    pesquisaTexto.dt_assinatura_termino = context.Request["dt_assinatura_termino"];
                    pesquisaTexto.ano = context.Request["ano"];
                    pesquisaTexto.isCount = true;
                    var buscaTexto = new DiarioBuscaEs().MontarBusca(pesquisaTexto);
                    query = buscaTexto.GetQuery();
                    break;
                default:
                    SentencaPesquisaGeralOV pesquisaGeral = new SentencaPesquisaGeralOV();
                    pesquisaGeral.all = context.Request["all"];
                    pesquisaGeral.isCount = true;
                    var buscaGeral = new DiarioBuscaEs().MontarBusca(pesquisaGeral);
                    query = buscaGeral.GetQuery();
                    query = query.Replace("st_habilita_pesquisa=true", "*");
                    query = query.Replace("(*)and", "");
                    break;
            }

            return new DiarioAD().ConsultarEs(query).hits.total;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
