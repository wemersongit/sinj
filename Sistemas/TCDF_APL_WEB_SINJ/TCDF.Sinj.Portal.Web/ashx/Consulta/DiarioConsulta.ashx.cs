using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Portal.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for DiarioConsulta
    /// </summary>
    public class DiarioConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sAction = Util.GetEnumDescription(AcoesDoUsuario.dio_pes) + ".DIRETORIO";
            SessaoUsuarioOV sessao_usuario = null;
            string sRetorno = "";

            try
            {
                if (util.BRLight.Util.GetVariavel("Aplicacao") == "CADASTRO")
                {
                    sessao_usuario = Util.ValidarSessao();
                    //Não faz nada se o usuário não tiver sessão no portal pois essa pesquisa pode ser feita por qualquer um.
                }
                var result_diario = new DiarioRN().ConsultarEs(context);

                sRetorno = Newtonsoft.Json.JsonConvert.SerializeObject(result_diario);
            }
            catch (Exception ex)
            {
                sRetorno = "{\"hits\":null}";
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                var nm_usuario = "visitante";
                var nm_login_usuario = "visitante";
                if (sessao_usuario != null)
                {
                    nm_usuario = sessao_usuario.nm_usuario;
                    nm_login_usuario = sessao_usuario.nm_login_usuario;
                    LogErro.gravar_erro(sAction, erro, nm_usuario, nm_login_usuario);
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
