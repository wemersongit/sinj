using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for UsuarioEditar
    /// </summary>
    public class UsuarioEditar : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            var _id_doc = context.Request["id_doc"];
            UsuarioOV usuarioOv = null;
            ulong id_doc = 0;
            var action = AcoesDoUsuario.usr_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (!string.IsNullOrEmpty(_id_doc) && ulong.TryParse(_id_doc, out id_doc))
                {
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);

                    var _nm_usuario = context.Request["nm_usuario"];
                    var _senha_usuario = context.Request["senha_usuario"];
                    var _email_usuario = context.Request["email_usuario"];
                    var _pagina_inicial = context.Request["pagina_inicial"];
                    var _ds_pagina_inicial = context.Request["ds_pagina_inicial"];
                    var _ch_tema = context.Request["ch_tema"];
                    var _st_usuario = context.Request["st_usuario"];
                    var st_usuario = false;
                    bool.TryParse(_st_usuario, out st_usuario);
                    var _in_alterar_senha = context.Request["in_alterar_senha"];
                    var in_alterar_senha = false;
                    bool.TryParse(_in_alterar_senha, out in_alterar_senha);
                    var _ch_perfil = context.Request["ch_perfil"];
                    var _nm_perfil = context.Request["nm_perfil"];
                    var _id_orgao_cadastrador = context.Request["orgao_cadastrador"];
                    var id_orgao_cadastrador = 0;
                    int.TryParse(_id_orgao_cadastrador, out id_orgao_cadastrador);
                    var _grupos = context.Request["grupos"];

                    UsuarioRN usuarioRn = new UsuarioRN();
                    usuarioOv = usuarioRn.Doc(id_doc);
                    usuarioOv.nm_usuario = _nm_usuario;
                    if(!string.IsNullOrEmpty(_senha_usuario)){
                        usuarioOv.senha_usuario = Criptografia.CalcularHashMD5(_senha_usuario, true);
                    }
                    usuarioOv.email_usuario = _email_usuario;
                    usuarioOv.pagina_inicial = _pagina_inicial;
                    usuarioOv.ds_pagina_inicial = _ds_pagina_inicial;
                    usuarioOv.ch_tema = _ch_tema;
                    usuarioOv.st_usuario = st_usuario;
                    usuarioOv.in_alterar_senha = in_alterar_senha;
                    usuarioOv.ch_perfil = _ch_perfil;
                    usuarioOv.nm_perfil = _nm_perfil;
                    usuarioOv.nr_tentativa_login = 0;
                    if (!string.IsNullOrEmpty(_grupos))
                    {
                        usuarioOv.grupos = _grupos.Split(',').ToList();
                    }

                    if (id_orgao_cadastrador > 0)
                    {
                        var orgao_cadastrador = new OrgaoCadastradorRN().Doc(id_orgao_cadastrador);
                        usuarioOv.orgao_cadastrador = new OrgaoCadastrador { id_orgao_cadastrador = orgao_cadastrador.id_orgao_cadastrador, nm_orgao_cadastrador = orgao_cadastrador.nm_orgao_cadastrador };
                    }


                    usuarioOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                    if (usuarioRn.Atualizar(id_doc, usuarioOv))
                    {
                        sRetorno = "{\"id_doc_success\":" + id_doc + ",\"update\":true}";
                    }
                    else
                    {
                        throw new Exception("Erro ao atualizar registro. id_doc:" + id_doc);
                    }
                }
                else
                {
                    throw new Exception("Erro ao atualizar registro. id_doc:" + _id_doc);
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
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException || ex is DocValidacaoException)
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
