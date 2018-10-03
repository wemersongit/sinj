using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;
using neo.BRLightREST;

namespace TCDF.Sinj.Web.ashx.Visualizacao
{
    /// <summary>
    /// Summary description for VocabularioDetalhes
    /// </summary>
    public class VocabularioDetalhes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _id_doc = context.Request["id_doc"];
            var _ch_termo = context.Request["ch_termo"];
            ulong id_doc = 0;
            var vocabularioRn = new VocabularioRN();
            VocabularioOV vocabularioOv = null;
            var action = AcoesDoUsuario.voc_vis;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                if (ulong.TryParse(_id_doc, out id_doc))
                {
                    vocabularioOv = vocabularioRn.Doc(id_doc);
                }
                else if (!string.IsNullOrEmpty(_ch_termo))
                {
                    vocabularioOv = vocabularioRn.Doc(_ch_termo);
                }
                else
                {
                    throw new ParametroInvalidoException("Não foi passado parametro para a busca.");
                }
                if (vocabularioOv != null)
                {
                    var sVocabularioOv = JSON.Serialize<VocabularioOV>(vocabularioOv);
                    var vocabularioDetalhado = JSON.Deserializa<VocabularioDetalhado>(sVocabularioOv);        
                    if (vocabularioDetalhado.EhTipoLista())
                    {
                        PreencherCamposDeLista(vocabularioDetalhado);
                    }
                    sRetorno = JSON.Serialize<VocabularioDetalhado>(vocabularioDetalhado);
                }
                else
                {
                    sRetorno = "{\"error_message\":\"registro não encontrado.\"}";
                }
                var log_visualizar = new LogVisualizar
                {
                    id_doc = id_doc,
                    ch_doc = _ch_termo
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_visualizar, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocNotFoundException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"id_doc_error\":" + _id_doc + "}";
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
            context.Response.ContentType = "application/json";
            context.Response.Write(sRetorno);
            context.Response.End();
        }

        private void PreencherCamposDeLista(VocabularioDetalhado vocabularioDetalhado)
        {
            var query = new Pesquisa();
            query.limit = null;
            query.literal = string.Format("ch_lista_superior='{0}'", vocabularioDetalhado.ch_termo);
            var result = new VocabularioRN().Consultar(query);
            vocabularioDetalhado.sublistas = new List<Vocabulario_Lista> ();
            vocabularioDetalhado.itens = new List<Vocabulario_Lista> ();

            foreach (var termo in result.results)
            {
                if (termo.in_lista)
                {
                    vocabularioDetalhado.sublistas.Add(new Vocabulario_Lista { ch_termo = termo.ch_termo, nm_termo = termo.nm_termo });
                }
                else
                {
                    vocabularioDetalhado.itens.Add(new Vocabulario_Lista { ch_termo = termo.ch_termo, nm_termo = termo.nm_termo });
                }
            }
            if (!string.IsNullOrEmpty(vocabularioDetalhado.ch_lista_superior))
            {
                vocabularioDetalhado.lista = new Vocabulario_Lista ();
                var lista_superior = new VocabularioRN().Doc(vocabularioDetalhado.ch_lista_superior);
                vocabularioDetalhado.lista.ch_termo = lista_superior.ch_termo;
                vocabularioDetalhado.lista.nm_termo = lista_superior.nm_termo;
            }
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