using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Path
{
    /// <summary>
    /// Summary description for UsuarioPath
    /// </summary>
    public class ConfiguracaoPath : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            var _ch_usuario = context.Request["ch_usuario"];
            var _path = context.Request["path"];
            var _value = context.Request["value"];
            ulong id_doc = 0;
            UsuarioOV usuarioOv = null;
            var action = AcoesDoUsuario.cfg_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (!string.IsNullOrEmpty(_ch_usuario) && !string.IsNullOrEmpty(_path) && !string.IsNullOrEmpty(_value))
                {
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);

                    UsuarioRN usuarioRn = new UsuarioRN();
                    usuarioOv = usuarioRn.Doc(_ch_usuario);
                    if (usuarioOv == null)
                    {
                        throw new Exception("Erro ao atualizar registro. ch_doc:" + _ch_usuario);
                    }
                    id_doc = usuarioOv._metadata.id_doc;
                    if (_path == "password")
                    {
                        _path = "senha_usuario";
                        _value = Criptografia.CalcularHashMD5(_value, true);
                    }
                    if (usuarioRn.PathPut(id_doc, _path, _value, null) == "UPDATED")
                    {
                        if(_path == "pagina_inicial"){
                            usuarioRn.PathPut(id_doc, "ds_pagina_inicial", context.Request["ds_pagina_inicial"], null);
						}
						if(_path == "senha_usuario"){
							usuarioRn.PathPut(id_doc, "in_alterar_senha", "false", null);
						}
                        usuarioOv = usuarioRn.Doc(id_doc);
                        new SessaoRN().AtualizarSessao(usuarioOv);
                        sRetorno = "{\"id_doc_success\":" + id_doc + ",\"path_update\":true}";
                    }
                    else
                    {
                        throw new Exception("Erro ao atualizar registro. ch_doc:" + _ch_usuario);
                    }
                }
                else
                {
                    throw new Exception("Erro ao atualizar registro. ch_doc:" + _ch_usuario);
                }
                var log_atualizar = new LogAlterar<UsuarioOV>
                {
                    id_doc = id_doc,
                    registro = usuarioOv
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_atualizar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\"}";
                }
                else
                {
                    sRetorno = Excecao.LerTodasMensagensDaExcecao(ex, false);
                    context.Response.StatusCode = 500;
                }
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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
