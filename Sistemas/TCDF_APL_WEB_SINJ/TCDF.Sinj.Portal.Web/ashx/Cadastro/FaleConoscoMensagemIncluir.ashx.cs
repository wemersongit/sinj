using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Portal.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for FaleConoscoMensagemIncluir
    /// </summary>
    public class FaleConoscoMensagemIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _k = context.Request["k"];
            var _ds_captcha = context.Request["ds_captcha"];
            try
            {
                if(string.IsNullOrEmpty(_ds_captcha) || !_k.Equals(Criptografia.CalcularHashMD5(_ds_captcha.ToUpper(), true))){
                    throw new DocValidacaoException("Os caracteres não correspondem com os da imagem.");
                }

                var _nm_user = context.Request["nm_user"];
                var _ds_email = context.Request["ds_email"];
                var _nr_cpf = context.Request["nr_cpf"];
                var _ds_assunto = context.Request["ds_assunto"];
                var _ds_msg = context.Request["ds_msg"];

                var faleConosco = new FaleConoscoOV();
                faleConosco.nm_user = _nm_user;
                faleConosco.ds_email = _ds_email;
                faleConosco.ds_assunto = _ds_assunto;
                faleConosco.ds_msg = _ds_msg;
                faleConosco.dt_inclusao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                if (new FaleConoscoRN().Incluir(faleConosco) > 0)
                {
                    sRetorno = "{\"success_message\": \"Mensagem enviada com sucesso.\"}";
                }
                else
                {
                    throw new Exception("Erro ao enviar mensagem. Tente novamente mais tarde.");
                }
            }
            catch (Exception ex)
            {
                if (ex is DocValidacaoException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\"}";
                }
                else if (ex is DocDuplicateKeyControlException)
                {
                    sRetorno = "{\"error_message\": \"Para evitar a sobrecarga de mensagens, pedimos, por gentileza, aguardar alguns minutos antes de enviar outra mensagem.\"}";
                }
                else
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
                    LogErro.gravar_erro("FLC.INC", erro, "visitante", "visitante");
                }
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