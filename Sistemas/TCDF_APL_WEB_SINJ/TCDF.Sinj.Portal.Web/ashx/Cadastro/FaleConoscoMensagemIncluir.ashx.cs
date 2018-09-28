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
            var _tipoDeVerificacao = context.Request["tpv"];


            try
            {
                if (_tipoDeVerificacao.Equals("g"))
                {
                    var _gRecaptchaResponse = context.Request["g-recaptcha-response"];
                    new ValidaCaptcha().ValidarCaptchaGoogle(_gRecaptchaResponse);
                }
                else
                {
                    var _ds_captcha = context.Request["ds_captcha"];
                    var _k = context.Request["k"];
                    new ValidaCaptcha().ValidarCaptcha(_ds_captcha, _k);
                }

                var _ch_chamado = context.Request["ch_chamado"];
                var _nm_user = context.Request["nm_user"];
                var _ds_email = context.Request["ds_email"];
                var _nr_telefone = context.Request["nr_telefone"];
                var _ds_assunto = context.Request["ds_assunto"];
                var _ds_msg = context.Request["ds_msg"];
                var _print = context.Request["print"];

                var faleConosco = new FaleConoscoOV();
                if (!string.IsNullOrEmpty(_ch_chamado))
                {
                    Util.rejeitarInject(_ch_chamado);
                    faleConosco = new FaleConoscoRN().Doc(_ch_chamado);
                    if (!string.IsNullOrEmpty(faleConosco.ch_chamado))
                    {
                        if (faleConosco.mensagens != null && faleConosco.mensagens.Count > 0)
                        {
                            if (string.IsNullOrEmpty(faleConosco.mensagens.Last().nm_login_usuario_resposta))
                            {
                                var dtUltimaResposta = Convert.ToDateTime(faleConosco.mensagens.Last().dt_resposta);
                                if (dtUltimaResposta.AddMinutes(10) > DateTime.Now)
                                {
                                    throw new DocDuplicateKeyControlException();
                                }
                            }
                        }
                        var msg = new FaleConoscoMensagemResposta();
                        msg.ds_assunto_resposta = _ds_assunto;
                        msg.ds_msg_resposta = _ds_msg;
                        msg.dt_resposta = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                        faleConosco.mensagens.Add(msg);
                        faleConosco.st_atendimento = "Novo";
                        if (new FaleConoscoRN().Atualizar(faleConosco._metadata.id_doc, faleConosco))
                        {
                            sRetorno = "{\"success_message\": \"Mensagem enviada com sucesso.\", \"msg\": " + JSON.Serialize<FaleConoscoMensagemResposta>(msg) + "}";
                        }
                        else
                        {
                            throw new Exception("Erro ao enviar mensagem. Tente novamente mais tarde.");
                        }
                    }
                }
                else
                {
                    faleConosco.ds_url_pagina = context.Request.UrlReferrer.AbsoluteUri;
                    faleConosco.print = _print;
                    faleConosco.nm_user = _nm_user;
                    faleConosco.ds_email = _ds_email;
                    faleConosco.nr_telefone = _nr_telefone;
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