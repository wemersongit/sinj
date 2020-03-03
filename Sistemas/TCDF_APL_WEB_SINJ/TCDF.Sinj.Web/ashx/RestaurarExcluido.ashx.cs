using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx
{
    /// <summary>
    /// Summary description for RestaurarExcluido
    /// </summary>
    public class RestaurarExcluido : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var action = AcoesDoUsuario.aud_lix;
            SessaoUsuarioOV sessao_usuario = null;
            var _json_excluido = context.Request["json_excluido"];
            ulong id_doc = 0;
            object ov;
            try
            {
                if (!string.IsNullOrEmpty(_json_excluido))
                {
                    var oExcluido = JSON.Deserializa<ExcluidoOV>(_json_excluido);
                    var nm_base = oExcluido.nm_base_excluido;
                    if (!string.IsNullOrEmpty(nm_base) && !string.IsNullOrEmpty(oExcluido.json_doc_excluido))
                    {
                        switch (nm_base)
                        {
                            case "sinj_autoria":
                                ov = JSON.Deserializa<AutoriaOV>(oExcluido.json_doc_excluido);
                                id_doc = new AutoriaRN().Incluir((AutoriaOV) ov);
                                break;
                            case "sinj_diario":
                                ov = JSON.Deserializa<DiarioOV>(oExcluido.json_doc_excluido);
                                id_doc = new DiarioRN().Incluir((DiarioOV)ov);
                                break;
                            case "sinj_vocabulario":
                                ov = JSON.Deserializa<VocabularioOV>(oExcluido.json_doc_excluido);
                                id_doc = new VocabularioRN().Incluir((VocabularioOV)ov);
                                break;
                            case "sinj_orgao":
                                ov = JSON.Deserializa<OrgaoOV>(oExcluido.json_doc_excluido);
                                id_doc = new OrgaoRN().Incluir((OrgaoOV)ov);
                                break;
                            case "sinj_norma":
                                ov = JSON.Deserializa<NormaOV>(oExcluido.json_doc_excluido);
                                id_doc = new NormaRN().Incluir((NormaOV)ov);
                                break;
                        }
                        if (id_doc > 0)
                        {
                            if (new ExcluidoRN().Excluir(oExcluido._metadata.id_doc))
                            {
                                sRetorno = "{\"id_doc\": " + id_doc + ", \"base\": \"" + nm_base + "\", \"success_message\":\"Registro restaurado para a base " + nm_base + "\"}";
                            }
                            else
                            {
                                throw new Exception("Não foi possível excluir permanentemente esse registro.");
                            }
                        }
                        else
                        {
                            throw new Exception("Não foi possível restaurar esse registro.");
                        }
                        var log_excluir = new LogExcluir
                        {
                            id_doc = id_doc,
                            nm_base = nm_base
                        };
                        LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ".RES", log_excluir, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);

                    }
                }
            }
            catch (Exception ex)
            {
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


                sRetorno = "{\"error_message\":\"" + ex.Message + "\"}";
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
