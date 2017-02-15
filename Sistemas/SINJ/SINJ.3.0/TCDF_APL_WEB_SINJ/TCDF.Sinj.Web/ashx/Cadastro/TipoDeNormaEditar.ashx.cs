﻿using System;
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
    /// Summary description for TipoDeNormaEditar
    /// </summary>
    public class TipoDeNormaEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            var _id_doc = context.Request["id_doc"];
            TipoDeNormaOV tipoDeNormaOv = null;
            ulong id_doc = 0;
            var action = AcoesDoUsuario.tdn_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (!string.IsNullOrEmpty(_id_doc) && ulong.TryParse(_id_doc, out id_doc))
                {
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);
                    var _nm_tipo_norma = context.Request["nm_tipo_norma"];
                    var _ds_tipo_norma = context.Request["ds_tipo_norma"];
                    var _orgaos_cadastradores = context.Request["orgao_cadastrador"];
                    var _grupos = context.Request["grupos"];
                    var _in_conjunta = context.Request["in_conjunta"];
                    var _in_questionavel = context.Request["in_questionavel"];
                    var _in_numeracao_por_orgao = context.Request["in_numeracao_por_orgao"];
                    var _in_apelidavel = context.Request["in_apelidavel"];

                    TipoDeNormaRN tipoDeNormaRn = new TipoDeNormaRN();
                    tipoDeNormaOv = tipoDeNormaRn.Doc(id_doc);
                    tipoDeNormaOv.nm_tipo_norma = _nm_tipo_norma;
                    tipoDeNormaOv.ds_tipo_norma = _ds_tipo_norma;
                    tipoDeNormaOv.orgaos_cadastradores = new List<OrgaoCadastrador>();
                    if (!string.IsNullOrEmpty(_orgaos_cadastradores))
                    {
                        foreach (var _orgao_cadastrador in _orgaos_cadastradores.Split(','))
                        {
                            var orgao_cadastrador = new OrgaoCadastradorRN().Doc(int.Parse(_orgao_cadastrador));
                            tipoDeNormaOv.orgaos_cadastradores.Add(new OrgaoCadastrador { id_orgao_cadastrador = orgao_cadastrador.id_orgao_cadastrador, nm_orgao_cadastrador = orgao_cadastrador.nm_orgao_cadastrador });
                        }
                    }
                    tipoDeNormaOv.in_g1 = false;
                    tipoDeNormaOv.in_g2 = false;
                    tipoDeNormaOv.in_g3 = false;
                    tipoDeNormaOv.in_g4 = false;
                    tipoDeNormaOv.in_g5 = false;
                    if (!string.IsNullOrEmpty(_grupos))
                    {
                        foreach (var _grupo in _grupos.Split(','))
                        {
                            switch (_grupo)
                            {
                                case "in_g1":
                                    tipoDeNormaOv.in_g1 = true;
                                    break;
                                case "in_g2":
                                    tipoDeNormaOv.in_g2 = true;
                                    break;
                                case "in_g3":
                                    tipoDeNormaOv.in_g3 = true;
                                    break;
                                case "in_g4":
                                    tipoDeNormaOv.in_g4 = true;
                                    break;
                                case "in_g5":
                                    tipoDeNormaOv.in_g5 = true;
                                    break;
                            }
                        }
                    }

                    var in_conjunta = false;
                    bool.TryParse(_in_conjunta, out in_conjunta);
                    tipoDeNormaOv.in_conjunta = in_conjunta;

                    var in_questionavel = false;
                    bool.TryParse(_in_questionavel, out in_questionavel);
                    tipoDeNormaOv.in_questionavel = in_questionavel;

                    var in_numeracao_por_orgao = false;
                    bool.TryParse(_in_numeracao_por_orgao, out in_numeracao_por_orgao);
                    tipoDeNormaOv.in_numeracao_por_orgao = in_numeracao_por_orgao;

                    var in_apelidavel = false;
                    bool.TryParse(_in_apelidavel, out in_apelidavel);
                    tipoDeNormaOv.in_apelidavel = in_apelidavel;

                    tipoDeNormaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                    if (tipoDeNormaRn.Atualizar(id_doc, tipoDeNormaOv))
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
                var log_atualizar = new LogAlterar<TipoDeNormaOV>
                {
                    id_doc = id_doc,
                    registro = tipoDeNormaOv
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