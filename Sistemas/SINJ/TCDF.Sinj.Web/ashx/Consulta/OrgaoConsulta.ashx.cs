using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for OrgaoConsulta
    /// </summary>
    public class OrgaoConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            Pesquisa pesquisa = new Pesquisa();

            var _nm_orgao = context.Request["nm_orgao"];
            var _sg_orgao = context.Request["sg_orgao"];
            var _ch_orgao_anterior = context.Request["ch_orgao_anterior"];
            var ch_orgao_anterior = new string[] { };
            if(!string.IsNullOrEmpty(_ch_orgao_anterior)){
                ch_orgao_anterior = _ch_orgao_anterior.Split(',');
            }
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario= Util.ValidarSessao();
                var query = "";
                if (!string.IsNullOrEmpty(_nm_orgao))
                {
                    if(!string.IsNullOrEmpty(_sg_orgao)){
                        query = "Upper(nm_orgao)='" + _nm_orgao.ToUpper() + "' and Upper(sg_orgao)='"+_sg_orgao+"'";
                        foreach (var ch in ch_orgao_anterior)
                        {
                            query += " and ch_orgao!='"+ch+"'";
                        }
                    }
                    else{
                        query = "Upper(nm_orgao) like '%" + _nm_orgao.ToUpper() + "%'";
                    }
                    pesquisa.literal = query;
                    sRetorno = new OrgaoRN().JsonReg(pesquisa);

                    var ind_count = sRetorno.IndexOf("\"result_count\": ") + "\"result_count\": ".Length;
                    var ind_chaves = sRetorno.IndexOf("}", ind_count);
                    var ind_virgula = sRetorno.IndexOf(",", ind_count);
                    var Busca = new LogBuscar
                    {
                        RegistrosTotal = sRetorno.Substring(ind_count, (ind_chaves > 0 ? ind_chaves : ind_virgula) - ind_count),
                        PesquisaLight = pesquisa
                    };
                    LogOperacao.gravar_operacao(Util.GetEnumDescription(AcoesDoUsuario.org_pes), Busca, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
            catch (Exception ex)
            {
                sRetorno = "{\"error_message\": \"Ocorreu erro um erro na consulta de órgão.\"}";
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(AcoesDoUsuario.nor_pes), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(sRetorno);
            context.Response.End();
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