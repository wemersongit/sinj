using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.Web.ashx.Cadastro;
using TCDF.Sinj.Web.ashx.Arquivo;
using neo.BRLightREST;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TCDF.Sinj.Web.ashx.Exclusao
{
    /// <summary>
    /// Summary description for VideExcluir
    /// </summary>
    public class VideExcluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _id_doc = context.Request["id_doc"];
            ulong id_doc = ulong.Parse(_id_doc);

            var _vide = context.Request["vide"];
            var vide = JsonConvert.DeserializeObject<VideExclusao>(_vide);

            var _dt_controle_alteracao = context.Request["dt_controle_alteracao"];

            var action = AcoesDoUsuario.nor_edt;
            SessaoUsuarioOV sessao_usuario = null;
            var dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                vide.Validate();

                var normaRn = new NormaRN();
                var dDt_controle_alteracao = Convert.ToDateTime(_dt_controle_alteracao);

                var normaAlteradoraOv = normaRn.Doc(vide.NormaAlteradora.ChNorma);
                if (normaAlteradoraOv.alteracoes.Count > 0)
                {
                    var dDt_alteracao = Convert.ToDateTime(normaAlteradoraOv.alteracoes.Last<AlteracaoOV>().dt_alteracao);
                    var usuario = normaAlteradoraOv.alteracoes.Last<AlteracaoOV>().nm_login_usuario_alteracao;
                    if (dDt_controle_alteracao < dDt_alteracao)
                    {
                        throw new RiskOfInconsistency("A norma do vide excluído foi modificada pelo usuário <b>" + usuario + "</b> às <b>" + dDt_alteracao + "</b> tornando a sua modificação inconsistente.<br/> É aconselhável atualizar a página e refazer as modificações ou forçar a alteração.<br/>Obs.: Clicar em 'Salvar mesmo assim' vai forçar a alteração e pode sobrescrever as modificações do usuário <b>" + usuario + "</b>.");
                    }
                }
                normaAlteradoraOv.vides.RemoveAll(v => v.ch_vide.Equals(vide.ChVide));
                if (vide.NormaAlteradora.ArquivoNovo != null)
                {
                    normaAlteradoraOv.ar_atualizado = vide.NormaAlteradora.ArquivoNovo;
                }
                normaAlteradoraOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                if (normaRn.Atualizar(normaAlteradoraOv._metadata.id_doc, normaAlteradoraOv))
                {

                    if (!vide.NormaAlterada.InNormaForaSistema)
                    {
                        var normaAlteradaOv = normaRn.Doc(vide.NormaAlterada.ChNorma);
                        normaAlteradaOv.vides.RemoveAll(v => v.ch_vide.Equals(vide.ChVide));
                        if (vide.NormaAlterada.ArquivoNovo != null)
                        {
                            normaAlteradaOv.ar_atualizado = vide.NormaAlterada.ArquivoNovo;
                        }
                        var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                        normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                        normaAlteradaOv.nm_situacao = situacao.nm_situacao;
                        sRetorno = "{\"id_doc_success\":" + id_doc + ", \"st_habilita_pesquisa\":\"" + normaAlteradaOv.st_habilita_pesquisa + "\", \"st_habilita_email\":\"" + normaAlteradaOv.st_habilita_email + "\",  \"id_doc_vide\":\"" + normaAlteradaOv._metadata.id_doc + "\"}";
                        normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                        if (!normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                        {
                            throw new Exception("Erro ao excluir Vide na norma alterada.");
                        }

                        //NOTE: Se o usuario nao tiver permissao pra habilitar o email ou habilitar no sinj pesquisa, seta pra false os campos da norma alterada. By Victor
                        if (!TCDF.Sinj.Util.UsuarioTemPermissao(TCDF.Sinj.Web.Sinj.oSessaoUsuario, TCDF.Sinj.AcoesDoUsuario.nor_eml))
                        {
                            normaRn.PathPut(normaAlteradaOv._metadata.id_doc, "st_habilita_email", "false", "");
                        }
                        if (!TCDF.Sinj.Util.UsuarioTemPermissao(TCDF.Sinj.Web.Sinj.oSessaoUsuario, TCDF.Sinj.AcoesDoUsuario.nor_hsp))
                        {
                            normaRn.PathPut(normaAlteradaOv._metadata.id_doc, "st_habilita_pesquisa", "false", "");
                        }
                    }
                    else
                    {
                        sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                    }
                }
                else
                {
                    throw new Exception("Erro ao excluir Vide na norma alteradora.");
                }
                var log_editar = new LogAlterar<NormaOV>
                {
                    id_doc = id_doc
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ",VIDE.EXC", log_editar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDependenciesException || ex is SessionExpiredException)
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
                    LogErro.gravar_erro(Util.GetEnumDescription(action) + ",VIDE.EXC", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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

    public class VideExclusao
    {
        [JsonProperty("ch_vide")]
        public string ChVide { get; set; }
        [JsonProperty("norma_alteradora")]
        public NormaVideExclusao NormaAlteradora { get; set; }
        [JsonProperty("norma_alterada")]
        public NormaVideExclusao NormaAlterada { get; set; }

        public void Validate()
        {
            if (NormaAlteradora == null || string.IsNullOrEmpty(NormaAlteradora.ChNorma))
            {
                throw new DocValidacaoException("Erro na norma informada.");
            }
            if (NormaAlterada == null)
            {
                throw new DocValidacaoException("Erro ao informar a norma alterada.");
            }
            if (!NormaAlterada.InNormaForaSistema && string.IsNullOrEmpty(NormaAlterada.ChNorma))
            {
                throw new DocValidacaoException("Erro ao informar a norma alterada. Por não se tratar de uma norma fora do sistema a chave é obrigatória.");
            }
        }
    }

    public class NormaVideExclusao
    {
        [JsonProperty("ch_norma")]
        public string ChNorma { get; set; }
        [JsonProperty("in_norma_fora_do_sistema")]
        public bool InNormaForaSistema { get; set; }
        [JsonProperty("arquivo_novo")]
        public ArquivoOV ArquivoNovo { get; set; }

    }
}
