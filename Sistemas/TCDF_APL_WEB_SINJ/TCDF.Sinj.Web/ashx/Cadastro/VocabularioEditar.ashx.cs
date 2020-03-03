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
    /// Summary description for VocabularioEditar
    /// </summary>
    public class VocabularioEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            VocabularioOV vocabularioOv = null;
            var _id_doc = context.Request["id_doc"];
            ulong id_doc = 0;
            var action = AcoesDoUsuario.voc_edt;
            var vocabularioRn = new VocabularioRN();
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                
                var _ds_nota_explicativa = context.Request["ds_nota_explicativa"];
                var _ds_fontes_pesquisadas = context.Request["ds_fontes_pesquisadas"];
                var _ds_texto_fonte = context.Request["ds_texto_fonte"];

                var _termos_gerais = context.Request.Form.GetValues("termos_gerais");
                var _termos_especificos = context.Request.Form.GetValues("termos_especificos");
                var _termos_nao_autorizados = context.Request.Form.GetValues("termos_nao_autorizados");
                var _termos_relacionados = context.Request.Form.GetValues("termos_relacionados");

                if(ulong.TryParse(_id_doc, out id_doc)){
                    vocabularioOv = vocabularioRn.Doc(id_doc);

                    vocabularioOv.ds_nota_explicativa = _ds_nota_explicativa;
                    vocabularioOv.ds_fontes_pesquisadas = _ds_fontes_pesquisadas;
                    vocabularioOv.ds_texto_fonte = _ds_texto_fonte;

                    var termos_nao_autorizados = vocabularioRn.PreencherRelacionamentoDeVocabulario(_termos_nao_autorizados);
                    vocabularioOv.termos_nao_autorizados = new List<Vocabulario_TNA>();
                    foreach (var termo_nao_autorizado in termos_nao_autorizados)
                    {
                        vocabularioOv.termos_nao_autorizados.Add(new Vocabulario_TNA { ch_termo_nao_autorizado = termo_nao_autorizado.chave, nm_termo_nao_autorizado = termo_nao_autorizado.nome });
                    }
                    if (vocabularioOv.EhTipoDescritor())
                    {
                        var termos_gerais = vocabularioRn.PreencherRelacionamentoDeVocabulario(_termos_gerais);
                        var termos_especificos = vocabularioRn.PreencherRelacionamentoDeVocabulario(_termos_especificos);
                        var termos_relacionados = vocabularioRn.PreencherRelacionamentoDeVocabulario(_termos_relacionados);
                        vocabularioOv.termos_gerais = new List<Vocabulario_TG>();
                        foreach(var termo_geral in termos_gerais)
                        {
                            vocabularioOv.termos_gerais.Add(new Vocabulario_TG { ch_termo_geral = termo_geral.chave, nm_termo_geral = termo_geral.nome});
                        }
                        vocabularioOv.termos_especificos = new List<Vocabulario_TE>();
                        foreach (var termo_especifico in termos_especificos)
                        {
                            vocabularioOv.termos_especificos.Add(new Vocabulario_TE { ch_termo_especifico = termo_especifico.chave, nm_termo_especifico = termo_especifico.nome });
                        }
                        vocabularioOv.termos_relacionados = new List<Vocabulario_TR>();
                        foreach (var termo_relacionado in termos_relacionados)
                        {
                            vocabularioOv.termos_relacionados.Add(new Vocabulario_TR { ch_termo_relacionado = termo_relacionado.chave, nm_termo_relacionado = termo_relacionado.nome });
                        }
                    }

                    vocabularioOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });

                    if (vocabularioRn.Atualizar(id_doc, vocabularioOv))
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
                var log_atualizar = new LogAlterar<VocabularioOV>
                {
                    id_doc = id_doc,
                    registro = vocabularioOv
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
