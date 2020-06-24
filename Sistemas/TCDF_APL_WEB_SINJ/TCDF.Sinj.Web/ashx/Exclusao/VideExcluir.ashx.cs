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
            NormaOV normaOv = new NormaOV();
            NormaOV normaAlteradoraOv = null;
            NormaOV normaAlteradaOv = null;
            var _id_doc = context.Request["id_doc"];
            var _ch_vide = context.Request["ch_vide"];
            ulong id_doc = 0;
            var action = AcoesDoUsuario.nor_edt;
            SessaoUsuarioOV sessao_usuario = null;
            var dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                var normaRn = new NormaRN();
<<<<<<< HEAD
                if (!string.IsNullOrEmpty(_id_doc) && !string.IsNullOrEmpty(_ch_vide))
                {
                    id_doc = ulong.Parse(_id_doc);
                    normaOv = normaRn.Doc(id_doc);
                    foreach (var vide in normaOv.vides)
                    {
                        if (vide.ch_vide == _ch_vide)
                        {
                            if (vide.in_norma_afetada)
                            {
                                normaAlteradaOv = normaOv;
                                try
                                {

                                    normaAlteradoraOv = normaRn.Doc(vide.ch_norma_vide);
                                }
                                catch (DocNotFoundException)
                                {

                                }
                            }
                            else
                            {
                                normaAlteradoraOv = normaOv;
                                if (!string.IsNullOrEmpty(vide.ch_norma_vide))
                                {
                                    try
                                    {
                                        normaAlteradaOv = normaRn.Doc(vide.ch_norma_vide);//pega a norma pela chave da vide
                                    }
                                    catch (DocNotFoundException)
                                    {

                                    }
                                }
                            }
                            break;
                        }
                    }
                    if (normaAlteradoraOv != null && normaAlteradoraOv.vides.Count > 0)
                    {
                        var iEnumVideAlteradorDesfazer = normaAlteradoraOv.vides.Where(v => v.ch_vide == _ch_vide);
                        Vide videAlteradorDesfazer = null;
                        if (iEnumVideAlteradorDesfazer.Count() > 0)
=======
                var dDt_controle_alteracao = Convert.ToDateTime(_dt_controle_alteracao);
                if (vide.NormaAlteradora != null && !string.IsNullOrEmpty(vide.NormaAlteradora.ChNorma))
                {
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
                    if (!vide.NormaAlteradora.SemArquivo && vide.NormaAlteradora.ArquivoNovo != null && !string.IsNullOrEmpty(vide.NormaAlteradora.ArquivoNovo.id_file))
                    {
                        normaRn.SalvarTextoAntigoDaNorma(normaAlteradoraOv, normaAlteradoraOv.vides.Where(v => v.ch_vide.Equals(vide.ChVide)).FirstOrDefault(), sessao_usuario.nm_login_usuario);
                        normaAlteradoraOv.ar_atualizado = vide.NormaAlteradora.ArquivoNovo;
                    }
                    normaAlteradoraOv.vides.RemoveAll(v => v.ch_vide.Equals(vide.ChVide));
                    normaAlteradoraOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                    if (!normaRn.Atualizar(normaAlteradoraOv._metadata.id_doc, normaAlteradoraOv))
                    {
                        throw new Exception("Erro ao excluir Vide na norma alteradora.");
                    }
                    sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                }
                if (vide.NormaAlterada != null && !string.IsNullOrEmpty(vide.NormaAlterada.ChNorma))
                {
                    if (!vide.NormaAlterada.InNormaForaSistema)
                    {
                        var normaAlteradaOv = normaRn.Doc(vide.NormaAlterada.ChNorma);
                        if (!vide.NormaAlterada.SemArquivo && vide.NormaAlterada.ArquivoNovo != null && !string.IsNullOrEmpty(vide.NormaAlterada.ArquivoNovo.id_file))
                        {
                            normaRn.SalvarTextoAntigoDaNorma(normaAlteradaOv, normaAlteradaOv.vides.Where(v => v.ch_vide.Equals(vide.ChVide)).FirstOrDefault(), sessao_usuario.nm_login_usuario);
                            normaAlteradaOv.ar_atualizado = vide.NormaAlterada.ArquivoNovo;
                        }
                        normaAlteradaOv.vides.RemoveAll(v => v.ch_vide.Equals(vide.ChVide));
                        var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                        normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                        normaAlteradaOv.nm_situacao = situacao.nm_situacao;
                        normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                        if (!normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
                        {
                            videAlteradorDesfazer = iEnumVideAlteradorDesfazer.First();//First() Retorna o primeiro elemento de uma sequência 
                        }
<<<<<<< HEAD
                        if (normaAlteradoraOv.vides.RemoveAll(vd => vd.ch_vide == _ch_vide) > 0 && videAlteradorDesfazer != null)
=======
                        sRetorno = "{\"id_doc_success\":" + id_doc + ", \"st_habilita_pesquisa\":\"" + normaAlteradaOv.st_habilita_pesquisa + "\", \"st_habilita_email\":\"" + normaAlteradaOv.st_habilita_email + "\",  \"id_doc_vide\":\"" + normaAlteradaOv._metadata.id_doc + "\"}";

                        //NOTE: Se o usuario nao tiver permissao pra habilitar o email ou habilitar no sinj pesquisa, seta pra false os campos da norma alterada. By Victor
                        if (!Util.UsuarioTemPermissao(Sinj.oSessaoUsuario, AcoesDoUsuario.nor_eml))
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
                        {
                            normaAlteradoraOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                            if (normaRn.Atualizar(normaAlteradoraOv._metadata.id_doc, normaAlteradoraOv))
                            {
                                if (normaAlteradaOv != null && normaAlteradaOv.vides.Count > 0)
                                {

                                    var iEnumVideAlteradoDesfazer = normaAlteradaOv.vides.Where(v => v.ch_vide == _ch_vide);
                                    Vide videAlteradoDesfazer = null;
                                    if (iEnumVideAlteradoDesfazer.Count() > 0)
                                    {
                                        Console.WriteLine("Contagem "+ iEnumVideAlteradoDesfazer.Count());
                                        videAlteradoDesfazer = iEnumVideAlteradoDesfazer.First();
                                    }
                                    if (normaAlteradaOv.vides.RemoveAll(vd => vd.ch_vide == _ch_vide) > 0 && videAlteradoDesfazer != null)
                                    {
                                        Console.WriteLine("Normaalteradao "+normaAlteradaOv.vides.RemoveAll(vd => vd.ch_vide == _ch_vide));
                                        var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                                        var nmSituacaoAnterior = normaAlteradaOv.nm_situacao;
                                        normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                                        normaAlteradaOv.nm_situacao = situacao.nm_situacao;

                                        normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                                        if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                                        {
                                             sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                                             normaRn.VerificarDispositivosEDesfazerAltercaoNosTextosDasNormas(normaAlteradoraOv, normaAlteradaOv, videAlteradorDesfazer, videAlteradoDesfazer, nmSituacaoAnterior, sessao_usuario.nm_login_usuario);
                                        }
                                        else
                                        {
                                            throw new Exception("Erro ao excluir Vide na norma alterada.");
                                        }
                                    }
                                }
                                else
                                {
                                    sRetorno = "{\"id_doc_success\":" + normaAlteradoraOv._metadata.id_doc + ", \"alert_message\": \"O vide foi excluído mas a norma alterada não foi encontrada ou não possui vides. Se há alteração de vide no arquivo da norma, a mesma deve ser desfeita manualmente.\"}";
                                }
                            }
                            else
                            {
                                throw new Exception("Erro ao excluir Vide da norma alteradora e consequentemente o vido da norma alterada não foi excluído também.");
                            }
                        }
<<<<<<< HEAD
                    }
                    else if (normaAlteradaOv != null && normaAlteradaOv.vides.Count > 0)
                    {
                        if (normaAlteradaOv.vides.RemoveAll(vd => vd.ch_vide == _ch_vide) > 0)
=======
                        if (!Util.UsuarioTemPermissao(Sinj.oSessaoUsuario, AcoesDoUsuario.nor_hsp))
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
                        {
                            var nmSituacaoAnterior = normaAlteradaOv.nm_situacao;
                            if (!normaAlteradaOv.st_situacao_forcada)
                            {
                                var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                                normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                                normaAlteradaOv.nm_situacao = situacao.nm_situacao;
                            }

                            normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                            if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                            {
                                sRetorno = "{\"id_doc_success\":" + normaAlteradaOv._metadata.id_doc + ", \"alert_message\": \"O vide foi excluído mas a norma alteradora não foi encontrada ou não possui vides. Se há alteração de vide no arquivo da norma, a mesma deve ser desfeita manualmente.\"}";
                            }
                            else
                            {
                                throw new Exception("Erro ao excluir Vide na norma alterada.");
                            }
                        }
                    }
                    var log_editar = new LogAlterar<NormaOV>
                    {
<<<<<<< HEAD
                        id_doc = id_doc
                    };
                    LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ",VIDE.EXC", log_editar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
            catch (FileNotFoundException ex)
            {
                sRetorno = "{\"id_doc_success\":" + id_doc + ", \"alert_message\": \"" + ex.Message + "\"}";
=======
                        sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                    }
                }
                var log_editar = new LogAlterar<NormaOV>
                {
                    id_doc = id_doc
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ",VIDE.EXC", log_editar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
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
<<<<<<< HEAD
=======

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
            if ((NormaAlteradora == null || string.IsNullOrEmpty(NormaAlteradora.ChNorma)) && (NormaAlterada == null || string.IsNullOrEmpty(NormaAlterada.ChNorma)))
            {
                throw new DocValidacaoException("Erro na norma informada.");
            }
        }
    }

    public class NormaVideExclusao
    {
        [JsonProperty("ch_norma")]
        public string ChNorma { get; set; }
        [JsonProperty("in_norma_fora_do_sistema")]
        public bool InNormaForaSistema { get; set; }
        [JsonProperty("sem_arquivo")]
        public bool SemArquivo { get; set; }
        [JsonProperty("arquivo_novo")]
        public ArquivoOV ArquivoNovo { get; set; }

    }
>>>>>>> 85c8dc87f60e85d36be23e1c882ef5e721335e4d
}
