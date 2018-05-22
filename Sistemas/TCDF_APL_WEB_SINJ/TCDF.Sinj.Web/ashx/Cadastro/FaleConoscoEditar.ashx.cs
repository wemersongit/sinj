using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for FaleConoscoEditar
    /// </summary>
    public class FaleConoscoEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _ch_chamado = context.Request["ch_chamado"];
            var _st_atendimento = context.Request["st_atendimento"];
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                var faleConoscoRn = new FaleConoscoRN();


                var faleConosco = faleConoscoRn.Doc(_ch_chamado);

                faleConosco.st_atendimento = _st_atendimento;
                if (_st_atendimento == "Recebido")
                {
                    faleConosco.dt_recebido = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                    faleConosco.nm_usuario_atendimento = sessao_usuario.nm_usuario;
                    faleConosco.nm_login_usuario_atendimento = sessao_usuario.nm_login_usuario;
                }
                else if (_st_atendimento == "Finalizado")
                {
                    faleConosco.dt_finalizado = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                }


                if (faleConoscoRn.Atualizar(faleConosco._metadata.id_doc, faleConosco))
                {
                    sRetorno = "{\"success_message\": \"Chamado alterado com sucesso.\"}";
                }
                else
                {
                    throw new Exception("Erro ao alterar chamado.");
                }
            }
            catch (Exception ex)
            {
                var sErro = Excecao.LerTodasMensagensDaExcecao(ex, false);
                sRetorno = "{\"error_message\":\"" + sErro + "\"}";
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                LogErro.gravar_erro("FLC.EDT", erro, "visitante", "visitante");
            }
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