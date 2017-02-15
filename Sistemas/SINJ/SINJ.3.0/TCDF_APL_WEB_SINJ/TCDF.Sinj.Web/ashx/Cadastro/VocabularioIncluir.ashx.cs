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
    /// Summary description for VocabularioIncluir
    /// </summary>
    public class VocabularioIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            VocabularioOV vocabularioOv = null;
            var action = AcoesDoUsuario.voc_inc;
            SessaoUsuarioOV sessao_usuario = null;
            var vocabularioRn = new VocabularioRN();
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                var _ch_tipo_termo = context.Request["ch_tipo_termo"];
                var _nm_termo = context.Request["nm_termo"];
                var _ch_orgao = context.Request["ch_orgao"];

                var _ds_nota_explicativa = context.Request["ds_nota_explicativa"];
                var _ds_fontes_pesquisadas = context.Request["ds_fontes_pesquisadas"];
                var _ds_texto_fonte = context.Request["ds_texto_fonte"];

                var in_lista = false;
                var _in_lista = context.Request["in_lista"];
                bool.TryParse(_in_lista, out in_lista);
                var _ch_lista_superior = context.Request["ch_lista_superior"];
                var _nm_lista_superior = context.Request["nm_lista_superior"];

                var _termos_gerais = context.Request.Form.GetValues("termos_gerais");
                var _termos_especificos = context.Request.Form.GetValues("termos_especificos");
                var _termos_nao_autorizados = context.Request.Form.GetValues("termos_nao_autorizados");
                var _termos_relacionados = context.Request.Form.GetValues("termos_relacionados");

                vocabularioOv = new VocabularioOV();

                vocabularioOv.st_aprovado = false;
                vocabularioOv.in_nao_autorizado = false;
                vocabularioOv.st_ativo = true;
                vocabularioOv.st_excluir = false;

                vocabularioOv.ch_tipo_termo = _ch_tipo_termo;

                vocabularioOv.nm_termo = _nm_termo;

                vocabularioOv.ds_nota_explicativa = _ds_nota_explicativa;
                vocabularioOv.ds_fontes_pesquisadas = _ds_fontes_pesquisadas;
                vocabularioOv.ds_texto_fonte = _ds_texto_fonte;
                if (vocabularioOv.EhTipoDescritor())
                {
                    var termos_gerais = vocabularioRn.PreencherRelacionamentoDeVocabulario(_termos_gerais);
                    var termos_especificos = vocabularioRn.PreencherRelacionamentoDeVocabulario(_termos_especificos);
                    var termos_relacionados = vocabularioRn.PreencherRelacionamentoDeVocabulario(_termos_relacionados);
                    var termos_nao_autorizados = vocabularioRn.PreencherRelacionamentoDeVocabulario(_termos_nao_autorizados);
                    foreach(var termo_geral in termos_gerais)
                    {
                        vocabularioOv.termos_gerais.Add(new Vocabulario_TG { ch_termo_geral = termo_geral.chave, nm_termo_geral = termo_geral.nome});
                    }
                    foreach (var termo_especifico in termos_especificos)
                    {
                        vocabularioOv.termos_especificos.Add(new Vocabulario_TE { ch_termo_especifico = termo_especifico.chave, nm_termo_especifico = termo_especifico.nome });
                    }
                    foreach (var termo_relacionado in termos_relacionados)
                    {
                        vocabularioOv.termos_relacionados.Add(new Vocabulario_TR { ch_termo_relacionado = termo_relacionado.chave, nm_termo_relacionado = termo_relacionado.nome });
                    }
                    foreach (var termo_nao_autorizado in termos_nao_autorizados)
                    {
                        vocabularioOv.termos_nao_autorizados.Add(new Vocabulario_TNA { ch_termo_nao_autorizado = termo_nao_autorizado.chave, nm_termo_nao_autorizado = termo_nao_autorizado.nome });
                    }
                }
                else if (vocabularioOv.EhTipoAutoridade())
                {
                    vocabularioOv.ch_orgao = _ch_orgao;
                }
                else if (vocabularioOv.EhTipoLista())
                {
                    vocabularioOv.in_lista = in_lista;
                    vocabularioOv.ch_lista_superior = _ch_lista_superior == "" ? null : _ch_lista_superior;
                    vocabularioOv.nm_lista_superior = _nm_lista_superior == "" ? null : _nm_lista_superior;
                }

                vocabularioOv.nm_login_usuario_cadastro = sessao_usuario.nm_login_usuario;
                vocabularioOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                var id_doc = vocabularioRn.Incluir(vocabularioOv);
                if (id_doc > 0)
                {
                    sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                }
                else
                {
                    throw new Exception("Erro ao incluir registro.");
                }
                var log_incluir = new LogIncluir<VocabularioOV>
                {
                    registro = vocabularioOv
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_incluir, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);

            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is SessionExpiredException || ex is DocValidacaoException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\"}";
                }
                else if (ex is DocDuplicateKeyException)
                {
                    var ids = "";
                    if(ex.Data != null && ex.Data.Count > 0){
                        var data = new List<ulong>();
                        if(ex.Data["ids"] != null){
                            data = (List<ulong>) ex.Data["ids"];
                        }
                        foreach (var id in data)
                        {
                            ids += (ids != "" ? "," : "") + id.ToString();
                        }
                    }
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"ids_duplicados\":["+ids+"]}";
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

        //private List<RelacionamentoDeVocabulario> PreencherRelacionamentoDeVocabulario(string _termos)
        //{
        //    List<RelacionamentoDeVocabulario> relacionamentoDeVocabulario = new List<RelacionamentoDeVocabulario>();
        //    if (!string.IsNullOrEmpty(_termos))
        //    {
        //        var termos = _termos.Split(',');
        //        var termo = new string[0];
        //        var chave = "";
        //        var nome = "";
        //        for (var i = 0; i < termos.Length; i++)
        //        {
        //            termo = termos[i].Split('#');
        //            chave = termo[0];
        //            nome = termo[1];
        //            relacionamentoDeVocabulario.Add(new RelacionamentoDeVocabulario { chave = chave, nome = nome });
        //        }
        //    }
        //    return relacionamentoDeVocabulario;
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}