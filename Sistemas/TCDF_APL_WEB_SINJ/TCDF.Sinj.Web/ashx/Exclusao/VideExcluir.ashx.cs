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
                                        normaAlteradaOv = normaRn.Doc(vide.ch_norma_vide);
                                    }
                                    catch (DocNotFoundException)
                                    {

                                    }
                                }
                            }
                            break;
                        }
                    }
                    if (normaAlteradoraOv != null)
                    {
                        var videAlteradorDesfazer = normaAlteradoraOv.vides.Where(v => v.ch_vide == _ch_vide).First();

                        if (normaAlteradoraOv.vides.RemoveAll(vd => vd.ch_vide == _ch_vide) > 0)
                        {
                            normaAlteradoraOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                            if (normaRn.Atualizar(normaAlteradoraOv._metadata.id_doc, normaAlteradoraOv))
                            {
                                if (normaAlteradaOv != null)
                                {
                                    var videAlteradoDesfazer = normaAlteradaOv.vides.Where(v => v.ch_vide == _ch_vide).First();
                                    if (normaAlteradaOv.vides.RemoveAll(vd => vd.ch_vide == _ch_vide) > 0)
                                    {
                                        var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                                        var nmSituacaoAnterior = normaAlteradaOv.nm_situacao;
                                        normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                                        normaAlteradaOv.nm_situacao = situacao.nm_situacao;

                                        normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                                        if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                                        {
                                            sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                                            VerificarDispositivosEDesfazerAltercaoNosTextosDasNormas(normaAlteradoraOv, normaAlteradaOv, videAlteradorDesfazer, videAlteradoDesfazer, nmSituacaoAnterior);
                                        }
                                        else
                                        {
                                            throw new Exception("Erro ao excluir Vide na norma alterada.");
                                        }
                                    }
                                }
                                else
                                {
                                    sRetorno = "{\"id_doc_success\":" + normaAlteradoraOv._metadata.id_doc + ", \"alert_message\": \"O vide foi excluído mas a norma alterada não foi encontrada. Se há alteração de vide no arquivo da norma, a mesma deve ser desfeita manualmente.\"}";
                                }
                            }
                            else
                            {
                                throw new Exception("Erro ao excluir Vide.");
                            }
                        }
                    }
                    else if (normaAlteradaOv != null)
                    {
                        var videAlteradoDesfazer = normaAlteradaOv.vides.Where(v => v.ch_vide == _ch_vide).First();
                        if (normaAlteradaOv.vides.RemoveAll(vd => vd.ch_vide == _ch_vide) > 0)
                        {
                            var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                            var nmSituacaoAnterior = normaAlteradaOv.nm_situacao;
                            normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                            normaAlteradaOv.nm_situacao = situacao.nm_situacao;

                            normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                            if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                            {
                                sRetorno = "{\"id_doc_success\":" + normaAlteradaOv._metadata.id_doc + ", \"alert_message\": \"O vide foi excluído mas a norma alteradora não foi encontrada. Se há alteração de vide no arquivo da norma, a mesma deve ser desfeita manualmente.\"}";
                            }
                            else
                            {
                                throw new Exception("Erro ao excluir Vide na norma alterada.");
                            }
                        }
                    }
                    var log_editar = new LogAlterar<NormaOV>
                    {
                        id_doc = id_doc
                    };
                    LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ",VIDE.EXC", log_editar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
            catch (FileNotFoundException ex)
            {
                sRetorno = "{\"id_doc_success\":" + id_doc + ", \"alert_message\": \"" + ex.Message + "\"}";
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

        /// <summary>
        /// Verifica se as normas possuem alterações nos seus respectivos arquivos html, provenientes dos vides que estão sendo desfeitos
        /// </summary>
        /// <param name="normaAlteradora"></param>
        /// <param name="normaAlterada"></param>
        /// <param name="videAlteradorDesfazer"></param>
        /// <param name="videAlteradoDesfazer"></param>
        /// <param name="nmSituacaoAnterior"></param>
        /// <returns>Contém os novos id_file dos respectivos arquivos alterados e salvos novamente</returns>
        public Dictionary<string, string> VerificarDispositivosEDesfazerAltercaoNosTextosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlteradorDesfazer, Vide videAlteradoDesfazer, string nmSituacaoAnterior)
        {
            var dictionaryIdFiles = new Dictionary<string, string>();
            if (videAlteradorDesfazer.caput_norma_vide != null && videAlteradoDesfazer.caput_norma_vide != null && videAlteradorDesfazer.caput_norma_vide.caput != null &&
                videAlteradoDesfazer.caput_norma_vide.caput != null && videAlteradorDesfazer.caput_norma_vide.caput.Length > 0 && videAlteradoDesfazer.caput_norma_vide.caput.Length > 0)
            {
                dictionaryIdFiles = RemoverAlteracaoComDispositivosNosArquivosDasNormas(normaAlteradora, normaAlterada, videAlteradorDesfazer.caput_norma_vide, videAlteradoDesfazer.caput_norma_vide);
                //IncluirAlteracaoComDispositivosNosArquivosDasNormas(normaAlteradora.ch_norma, normaAlterada.ch_norma, videAlterador.caput_norma_vide, videAlterado.caput_norma_vide);
            }
            else if (videAlteradoDesfazer.caput_norma_vide != null && videAlteradoDesfazer.caput_norma_vide.caput != null && videAlteradoDesfazer.caput_norma_vide.caput.Length > 0)
            {
                dictionaryIdFiles = RemoverAlteracaoComDispositivoAlteradoNosArquivosDasNormas(normaAlterada, normaAlteradora, videAlteradoDesfazer.caput_norma_vide);
            }
            else if (videAlteradorDesfazer.caput_norma_vide != null && videAlteradorDesfazer.caput_norma_vide.caput != null && videAlteradorDesfazer.caput_norma_vide.caput.Length > 0)
            {
                dictionaryIdFiles = RemoverAlteracaoComDispositivoAlteradorNosArquivosDasNormas(normaAlteradora, normaAlterada, videAlteradorDesfazer.caput_norma_vide, nmSituacaoAnterior);
            }
            else
            {
                dictionaryIdFiles = RemoverAlteracaoSemDispositivosNosArquivosDasNormas(normaAlteradora, normaAlterada, videAlteradoDesfazer, nmSituacaoAnterior);
                //IncluirAlteracaoSemDispositivosNosArquivosDasNormas(normaAlteradora, normaAlterada, videAlterado);
            }
            return dictionaryIdFiles;
        }

        /// <summary>
        /// Remove as alterações nos arquivos das normas (Alterdora e Alterada) e em seguida insere as novas alterações
        /// </summary>
        /// <param name="normaAlteradora"></param>
        /// <param name="normaAlterada"></param>
        /// <param name="caputAlterador"></param>
        /// <param name="caputAlterado"></param>
        /// <param name="caputAlteradorDesfazer"></param>
        /// <param name="caputAlteradoDesfazer"></param>
        public Dictionary<string, string> RemoverAlteracaoComDispositivosNosArquivosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Caput caputAlteradorDesfazer, Caput caputAlteradoDesfazer)
        {
            var upload = new UploadHtml();
            var normaRn = new NormaRN();
            var pesquisa = new Pesquisa();
            var listOp = new List<opMode<object>>();

            var dictionaryIdFiles = new Dictionary<string, string>();

            var arquivoNormaAlteradora = RemoverLinkNoTextoDaNormaAlteradora(normaAlteradora, caputAlteradorDesfazer, caputAlteradoDesfazer);
            if (arquivoNormaAlteradora != "")
            {
                var retornoFileAlteradora = upload.AnexarHtml(arquivoNormaAlteradora, normaAlteradora.ar_atualizado.filename, "sinj_norma");
                var oFileAlteradora = JSON.Deserializa<ArquivoOV>(retornoFileAlteradora);
                if (!string.IsNullOrEmpty(oFileAlteradora.id_file))
                {
                    listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retornoFileAlteradora } });
                    pesquisa.literal = "ch_norma='" + normaAlteradora.ch_norma + "'";
                    var sRetornoPath = normaRn.PathPut<object>(pesquisa, listOp);
                    var oRetornoPath = JSON.Deserializa<opResult>(sRetornoPath);
                    if (oRetornoPath.success == 1)
                    {
                        dictionaryIdFiles.Add("id_file_alterador", oFileAlteradora.id_file);
                    }
                }
            }

            listOp = new List<opMode<object>>();
            var arquivoNormaAlterada = RemoverAlteracaoNoTextoDaNormaAlterada(normaAlterada, caputAlteradoDesfazer, caputAlteradorDesfazer);
            if (arquivoNormaAlterada != "")
            {
                var retornoFileAlterada = upload.AnexarHtml(arquivoNormaAlterada, normaAlterada.ar_atualizado.filename, "sinj_norma");
                var oFileAlterada = JSON.Deserializa<ArquivoOV>(retornoFileAlterada);
                if (!string.IsNullOrEmpty(oFileAlterada.id_file))
                {
                    listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retornoFileAlterada } });
                    pesquisa.literal = "ch_norma='" + normaAlterada.ch_norma + "'";
                    var sRetornoPath = normaRn.PathPut<object>(pesquisa, listOp);
                    var oRetornoPath = JSON.Deserializa<opResult>(sRetornoPath);
                    if (oRetornoPath.success == 1)
                    {
                        dictionaryIdFiles.Add("id_file_alterado", oFileAlterada.id_file);
                    }
                }
            }
            return dictionaryIdFiles;
        }

        public Dictionary<string, string> RemoverAlteracaoComDispositivoAlteradorNosArquivosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Caput caputAlteradorDesfazer, string nmSituacaoAnterior)
        {
            var pesquisa = new Pesquisa();
            var normaRn = new NormaRN();
            var upload = new UploadHtml();
            var listOp = new List<opMode<object>>();

            var dictionaryIdFiles = new Dictionary<string, string>();

            var arquivoNormaVideAlteradora = RemoverLinkNoTextoDaNormaAlteradora(normaAlteradora, caputAlteradorDesfazer, normaAlterada.ch_norma);
            
            if (arquivoNormaVideAlteradora != "")
            {
                var retornoFileAlteradora = upload.AnexarHtml(arquivoNormaVideAlteradora, normaAlteradora.ar_atualizado.filename, "sinj_norma");
                var oFileAlteradora = JSON.Deserializa<ArquivoOV>(retornoFileAlteradora);
                if (!string.IsNullOrEmpty(oFileAlteradora.id_file))
                {
                    listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retornoFileAlteradora } });
                    pesquisa.literal = "ch_norma='" + normaAlteradora.ch_norma + "'";
                    var sRetornoPath = normaRn.PathPut<object>(pesquisa, listOp);
                    var oRetornoPath = JSON.Deserializa<opResult>(sRetornoPath);
                    if (oRetornoPath.success == 1)
                    {
                        dictionaryIdFiles.Add("id_file_alterador", oFileAlteradora.id_file);
                    }
                }
            }

            var auxDsTextoParaAlterador = caputAlteradorDesfazer.ds_texto_para_alterador_aux;
            var auxNmSituacaoAlterada = normaAlterada.nm_situacao.ToLower();
            var auxNmSituacaoAnterior = nmSituacaoAnterior.ToLower();

            var idFileNormaAlterada = normaAlterada.getIdFileArquivoVigente();
            var nameFileNormaAlterada = normaAlterada.getNameFileArquivoVigente();
            if (!string.IsNullOrEmpty(idFileNormaAlterada))
            {
                var arquivoNormaVideAlterada = "";

                //Se a situação da norma foi alterada pela remoção do vide em questão, então não deve alterar o texto, pois essa alteração afeta o texto inteiro
                if (auxNmSituacaoAlterada != auxNmSituacaoAnterior && ((auxNmSituacaoAnterior == "revogado" && auxDsTextoParaAlterador == "revogado") ||
                    (auxNmSituacaoAnterior == "anulado" && auxDsTextoParaAlterador == "anulado") ||
                    (auxNmSituacaoAnterior == "extinta" && auxDsTextoParaAlterador == "extinta") ||
                    (auxNmSituacaoAnterior == "inconstitucional" && auxDsTextoParaAlterador == "declarado inconstitucional") ||
                    (auxNmSituacaoAnterior == "inconstitucional" && auxDsTextoParaAlterador == "julgado procedente") ||
                    (auxNmSituacaoAnterior == "cancelada" && auxDsTextoParaAlterador == "cancelada") ||
                    (auxNmSituacaoAnterior == "suspenso" && auxDsTextoParaAlterador == "suspenso totalmente")))
                {
                    arquivoNormaVideAlterada = RemoverAlteracaoNoTextoCompletoDaNormaAlterada(normaAlteradora.ch_norma, idFileNormaAlterada, auxDsTextoParaAlterador);
                }
                else if (auxDsTextoParaAlterador == "ratificado" ||
                    auxDsTextoParaAlterador == "reeditado" ||
                    auxDsTextoParaAlterador == "regulamentado" ||
                    auxDsTextoParaAlterador == "prorrogado" ||
                    auxDsTextoParaAlterador == "legislação correlata")
                {
                    arquivoNormaVideAlterada = RemoverInformacaoNoTextoDaNormaAlterada(normaAlteradora.ch_norma, idFileNormaAlterada, auxDsTextoParaAlterador);
                }

                if (arquivoNormaVideAlterada != "")
                {
                    var retornoFileAlterada = upload.AnexarHtml(arquivoNormaVideAlterada, nameFileNormaAlterada, "sinj_norma");
                    var oFileAlterada = JSON.Deserializa<ArquivoOV>(retornoFileAlterada);
                    if (!string.IsNullOrEmpty(oFileAlterada.id_file))
                    {
                        listOp = new List<opMode<object>>();
                        listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retornoFileAlterada } });
                        pesquisa.literal = "ch_norma='" + normaAlterada.ch_norma + "'";
                        var sRetornoPath = normaRn.PathPut<object>(pesquisa, listOp);
                        var oRetornoPath = JSON.Deserializa<opResult>(sRetornoPath);
                        if (oRetornoPath.success == 1)
                        {
                            dictionaryIdFiles.Add("id_file_alterado", oFileAlterada.id_file);
                        }
                    }
                }
            }
            return dictionaryIdFiles;
        }

        public Dictionary<string, string> RemoverAlteracaoComDispositivoAlteradoNosArquivosDasNormas(NormaOV normaAlterada, NormaOV normaAlteradora, Caput caputAlteradoDesfazer)
        {
            var upload = new UploadHtml();
            var normaRn = new NormaRN();
            var pesquisa = new Pesquisa();

            var dictionaryIdFiles = new Dictionary<string, string>();

            var nameFileNormaAlterada = normaAlterada.getNameFileArquivoVigente();

            var arquivoNormaVideAlterada = RemoverAlteracaoNoTextoDaNormaAlterada(normaAlterada, normaAlteradora, caputAlteradoDesfazer);

            var retornoFileAlterada = upload.AnexarHtml(arquivoNormaVideAlterada, caputAlteradoDesfazer.filename, "sinj_norma");
            var oFileAlterada = JSON.Deserializa<ArquivoOV>(retornoFileAlterada);
            if (!string.IsNullOrEmpty(oFileAlterada.id_file))
            {
                pesquisa.literal = "ch_norma='" + normaAlterada.ch_norma + "'";
                var listOp = new List<opMode<object>>();
                listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retornoFileAlterada } });
                var sRetornoPath = normaRn.PathPut<object>(pesquisa, listOp);
                var oRetornoPath = JSON.Deserializa<opResult>(sRetornoPath);
                if (oRetornoPath.success == 1)
                {
                    dictionaryIdFiles.Add("id_file_alterado", oFileAlterada.id_file);
                }
            }
            return dictionaryIdFiles;
        }

        public Dictionary<string, string> RemoverAlteracaoSemDispositivosNosArquivosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlteradoDesfazer, string nmSituacaoAnterior)
        {
            var upload = new UploadHtml();
            var normaRn = new NormaRN();
            var pesquisa = new Pesquisa();

            var dictionaryIdFiles = new Dictionary<string, string>();

            var idFileNormaAlteradora = normaAlteradora.getIdFileArquivoVigente();
            var nameFileNormaAlteradora = normaAlteradora.getNameFileArquivoVigente();

            var if_file_norma_alterada = normaAlterada.getIdFileArquivoVigente();
            var name_file_norma_alterada = normaAlterada.getNameFileArquivoVigente();

            if (!string.IsNullOrEmpty(if_file_norma_alterada))
            {
                var aux_nm_situacao_alterada = nmSituacaoAnterior.ToLower();
                var aux_ds_texto_alterador = videAlteradoDesfazer.ds_texto_relacao.ToLower();

                var arquivo_norma_vide_alterada = "";

                if (UtilVides.EhAlteracaoCompleta(aux_nm_situacao_alterada, aux_ds_texto_alterador))
                {
                    arquivo_norma_vide_alterada = RemoverAlteracaoNoTextoCompletoDaNormaAlterada(normaAlteradora.ch_norma, if_file_norma_alterada, aux_ds_texto_alterador);
                }
                else if (UtilVides.EhLegislacaoCorrelata(aux_ds_texto_alterador))
                {
                    arquivo_norma_vide_alterada = RemoverInformacaoNoTextoDaNormaAlterada(normaAlteradora.ch_norma, if_file_norma_alterada, aux_ds_texto_alterador);
                }

                if (!string.IsNullOrEmpty(arquivo_norma_vide_alterada))
                {
                    var retornoFileAlterada = upload.AnexarHtml(arquivo_norma_vide_alterada, name_file_norma_alterada, "sinj_norma");
                    var oFileAlterada = JSON.Deserializa<ArquivoOV>(retornoFileAlterada);
                    if (!string.IsNullOrEmpty(oFileAlterada.id_file))
                    {
                        pesquisa.literal = "ch_norma='" + normaAlterada.ch_norma + "'";
                        var listOp = new List<opMode<object>>();
                        listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retornoFileAlterada } });
                        var sRetornoPath = normaRn.PathPut<object>(pesquisa, listOp);
                        var oRetornoPath = JSON.Deserializa<opResult>(sRetornoPath);
                        if (oRetornoPath.success == 1)
                        {
                            dictionaryIdFiles.Add("id_file_alterado", oFileAlterada.id_file);
                        }

                        var arquivoNormaAlteradora = RemoverInformacaoNoTextoDaNormaAlteradora(normaAlterada, idFileNormaAlteradora, aux_ds_texto_alterador);
                        if (!string.IsNullOrEmpty(arquivoNormaAlteradora))
                        {
                            var retornoFileAlteradora = upload.AnexarHtml(arquivoNormaAlteradora, nameFileNormaAlteradora, "sinj_norma");

                            var oFileAlteradora = JSON.Deserializa<ArquivoOV>(retornoFileAlteradora);
                            if (!string.IsNullOrEmpty(oFileAlteradora.id_file))
                            {
                                pesquisa.literal = "ch_norma='" + normaAlteradora.ch_norma + "'";
                                listOp = new List<opMode<object>>();
                                listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retornoFileAlteradora } });
                                sRetornoPath = normaRn.PathPut<object>(pesquisa, listOp);
                                oRetornoPath = JSON.Deserializa<opResult>(sRetornoPath);
                                if (oRetornoPath.success == 1)
                                {
                                    dictionaryIdFiles.Add("id_file_alterador", oFileAlteradora.id_file);
                                }
                            }
                        }
                    }
                }
            }
            return dictionaryIdFiles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="norma_alteradora"></param>
        /// <param name="_caput_alteradora"></param>
        /// <param name="_caput_alterada"></param>
        /// <returns></returns>
        public string RemoverLinkNoTextoDaNormaAlteradora(NormaOV norma_alteradora, Caput _caput_alteradora, Caput _caput_alterada)
        {
            var htmlFile = new HtmlFileEncoded();
            var arquivo_norma_vide_alteradora = htmlFile.GetHtmlFile(norma_alteradora.ar_atualizado.id_file, "sinj_norma", null);

            var pattern_caput = "";
            switch (_caput_alterada.nm_relacao_aux)
            {
                case "acrescimo":
                    pattern_caput = _caput_alterada.caput[0] + "_add_0";
                    break;
                case "renumeração":
                    pattern_caput = _caput_alterada.caput[0] + "_renum";
                    break;
                case "revogação":
                    pattern_caput = _caput_alterada.caput[0] + "_replaced";
                    break;
                default:
                    pattern_caput = _caput_alterada.caput[0];
                    break;
            }
            var pattern = "(<p.+?linkname=\"" + _caput_alteradora.caput[0] + "\".*?>)(.*?)<a href=\"\\(_link_sistema_\\)Norma/" + _caput_alterada.ch_norma + '/' + _caput_alterada.filename + "#" + pattern_caput + "\" >" + UtilVides.EscapeCharsInToPattern(_caput_alteradora.link) + "</a>(.*?)</p>";
            var matches = Regex.Matches(arquivo_norma_vide_alteradora, pattern);
            if (matches.Count == 1)
            {
                var replacement = matches[0].Groups[1].Value + matches[0].Groups[2].Value + _caput_alteradora.link + matches[0].Groups[3].Value + "</p>";
                arquivo_norma_vide_alteradora = Regex.Replace(arquivo_norma_vide_alteradora, pattern, replacement);
            }
            else
            {
                arquivo_norma_vide_alteradora = "";
            }
            return arquivo_norma_vide_alteradora;
        }

        public string RemoverLinkNoTextoDaNormaAlteradora(NormaOV normaAlteradora, Caput caputAlteradoraDesfazer, string chNormaAlterada)
        {
            var htmlFile = new HtmlFileEncoded();
            var arquivo_norma_vide_alteradora = htmlFile.GetHtmlFile(normaAlteradora.ar_atualizado.id_file, "sinj_norma", null);
            var pattern = "(<p.+?linkname=\"" + caputAlteradoraDesfazer.caput[0] + "\".*?>)(.*?)<a href=\"\\(_link_sistema_\\)Norma/" + chNormaAlterada + "/.+?\" >" + UtilVides.EscapeCharsInToPattern(caputAlteradoraDesfazer.link) + "</a>(.*?)</p>";
            var matches = Regex.Matches(arquivo_norma_vide_alteradora, pattern);
            if (matches.Count == 1)
            {
                var replacement = matches[0].Groups[1].Value + matches[0].Groups[2].Value + caputAlteradoraDesfazer.link + matches[0].Groups[3].Value + "</p>";
                arquivo_norma_vide_alteradora = Regex.Replace(arquivo_norma_vide_alteradora, pattern, replacement);
            }
            else
            {
                arquivo_norma_vide_alteradora = "";
            }
            return arquivo_norma_vide_alteradora;
        }

        public string RemoverAlteracaoNoTextoDaNormaAlterada(NormaOV norma_alterada, Caput _caput_alterada_desfazer, Caput _caput_alteradora_desfazer)
        {
            var texto = "";
            if (_caput_alterada_desfazer.caput.Length == _caput_alterada_desfazer.texto_antigo.Length)
            {
                texto = new HtmlFileEncoded().GetHtmlFile(norma_alterada.ar_atualizado.id_file, "sinj_norma", null);
                var bAlterou = false;
                var pattern = "";
                var replacement = "";
                var ds_link_alterador = "";
                for (var i = 0; i < _caput_alterada_desfazer.caput.Length; i++)
                {
                    switch (_caput_alterada_desfazer.nm_relacao_aux)
                    {
                        case "acrescimo":
                            pattern = "<p.+?linkname=\"" + _caput_alterada_desfazer.caput[i] + "_add_.+?\".*?>.*? <a class=\"link_vide\".*?>.+?</a></p>";
                            replacement = "";
                            break;
                        case "renumeração":
                            ds_link_alterador = "\\(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada_desfazer.caput[i]) + _caput_alterada_desfazer.ds_texto_para_alterador_aux + " pelo\\(a\\) " + _caput_alteradora_desfazer.ds_norma + "\\)";
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada_desfazer.caput[i]) + "_renum(\".*?>.*?<a.+?name=\")" + _caput_alterada_desfazer.caput[i] + "_renum(\".*?></a>.*?)" + UtilVides.EscapeCharsInToPattern(_caput_alterada_desfazer.texto_novo[i]) + "(.*?) <a class=\"link_vide\" href=\"\\(_link_sistema_\\)Norma/" + _caput_alteradora_desfazer.ch_norma + '/' + _caput_alteradora_desfazer.filename + "#" + _caput_alteradora_desfazer.caput[0] + "\">" + ds_link_alterador + "</a>(.*?)</p>";
                            replacement = "$1" + _caput_alterada_desfazer.caput[i] + "$2" + _caput_alterada_desfazer.caput[i] + "$3" + _caput_alterada_desfazer.texto_antigo[i] + "$4$5</p>";
                            break;
                        case "prorrogação":
                        case "ratificação":
                        case "regulamentação":
                        case "ressalva":
                            ds_link_alterador = "\\(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada_desfazer.caput[i]) + _caput_alterada_desfazer.ds_texto_para_alterador_aux + " pelo\\(a\\) " + _caput_alteradora_desfazer.ds_norma + "\\)";
                            pattern = "(<p.+?linkname=\"" + _caput_alterada_desfazer.caput[i] + "\".*?<a.+?name=\"" + _caput_alterada_desfazer.caput[i] + "\".*?></a>.*?) <a class=\"link_vide\" href=\"\\(_link_sistema_\\)Norma/" + _caput_alteradora_desfazer.ch_norma + '/' + _caput_alteradora_desfazer.filename + "#" + _caput_alteradora_desfazer.caput[0] + "\">" + ds_link_alterador + "</a>(.*?)</p>";
                            replacement = "$1$2</p>";
                            break;
                        default:
                            ds_link_alterador = "\\(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada_desfazer.caput[i]) + _caput_alterada_desfazer.ds_texto_para_alterador_aux + " pelo\\(a\\) " + _caput_alteradora_desfazer.ds_norma + "\\)";
                            if (!string.IsNullOrEmpty(_caput_alterada_desfazer.texto_novo[i]))
                            {
                                pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada_desfazer.caput[i]) + "_replaced(\".*?)replaced_by=\"" + _caput_alteradora_desfazer.ch_norma + "\"(.*?)<s>(.*?)<a.+?name=\"" + _caput_alterada_desfazer.caput[i] + "_replaced\".*?></a>(.*?)</s>(.*?)</p>\r\n<p.+?linkname=\"" + _caput_alterada_desfazer.caput[i] + "\".*?>.*?" + UtilVides.EscapeCharsInToPattern(_caput_alterada_desfazer.texto_novo[i]) + ".*? <a class=\"link_vide\".*?>.+?</a></p>";
                                replacement = "$1" + _caput_alterada_desfazer.caput[i] + "$2$3$4<a name=\"" + _caput_alterada_desfazer.caput[i] + "\"></a>" + _caput_alterada_desfazer.texto_antigo[i] + "$6</p>";
                            }
                            else
                            {
                                pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada_desfazer.caput[i]) + "_replaced(\".*?)replaced_by=\"" + _caput_alteradora_desfazer.ch_norma + "\"(.*?)<s>(.*?)<a.+?name=\"" + _caput_alterada_desfazer.caput[i] + "_replaced\".*?></a>(.*?)</s>(.*?) <a class=\"link_vide\" href=\"\\(_link_sistema_\\)Norma/" + _caput_alteradora_desfazer.ch_norma + '/' + _caput_alteradora_desfazer.filename + "#" + _caput_alteradora_desfazer.caput[0] + "\">" + ds_link_alterador + "</a>(.*?)</p>";
                                replacement = "$1" + _caput_alterada_desfazer.caput[i] + "$2$3$4<a name=\"" + _caput_alterada_desfazer.caput[i] + "\"></a>" + _caput_alterada_desfazer.texto_antigo[i] + "$6$7</p>";
                            }
                            break;
                    }
                    if (Regex.Matches(texto, pattern).Count == 1)
                    {
                        texto = Regex.Replace(texto, pattern, replacement);
                        bAlterou = true;
                    }
                }
                if (!bAlterou)
                {
                    texto = "";
                }
            }
            return texto;
        }

        public string RemoverAlteracaoNoTextoDaNormaAlterada(NormaOV normaAlterada, NormaOV normaAlteradora, Caput caputAlteradoDesfazer)
        {
            var nameFileNormaAlteradora = normaAlteradora.getNameFileArquivoVigente();
            var dsNormaAlteradora = normaAlteradora.getDescricaoDaNorma();

            var pattern = "";
            var replacement = "";
            var ds_link_alterador = "";
            //define o link da norma alteradora, se possui nameFileNormaAlteradora então a norma tem arquivo se não então o link é para os detalhes da norma
            var aux_href = UtilVides.EscapeCharsInToPattern(!string.IsNullOrEmpty(nameFileNormaAlteradora) ? ("(_link_sistema_)Norma/" + normaAlteradora.ch_norma + '/' + nameFileNormaAlteradora) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + normaAlteradora.ch_norma);
            
            var texto = new HtmlFileEncoded().GetHtmlFile(normaAlterada.ar_atualizado.id_file, "sinj_norma", null);

            var bAlterou = false;

            for (var i = 0; i < caputAlteradoDesfazer.caput.Length; i++)
            {
                switch (caputAlteradoDesfazer.nm_relacao_aux)
                {
                    case "acrescimo":
                        pattern = "<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i] + "_add_.+?\".*?>.*? <a class=\"link_vide\".*?>.+?</a></p>";
                        replacement = "";
                        break;
                    case "renumeração":
                        ds_link_alterador = "\\(" + UtilVides.gerarDescricaoDoCaput(caputAlteradoDesfazer.caput[i]) + caputAlteradoDesfazer.ds_texto_para_alterador_aux + " pelo\\(a\\) .+?\\)";
                        pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "_renum(\".*?>.*?<a.+?name=\")" + caputAlteradoDesfazer.caput[i] + "_renum(\".*?></a>.*?)" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.texto_novo[i]) + "(.*?) <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a>(.*?)</p>";
                        replacement = "$1" + caputAlteradoDesfazer.caput[i] + "$2" + caputAlteradoDesfazer.caput[i] + "$3" + caputAlteradoDesfazer.texto_antigo[i] + "$4$5</p>";
                        break;
                    case "prorrogação":
                    case "ratificação":
                    case "regulamentação":
                    case "ressalva":
                        ds_link_alterador = "\\(" + UtilVides.gerarDescricaoDoCaput(caputAlteradoDesfazer.caput[i]) + caputAlteradoDesfazer.ds_texto_para_alterador_aux + " pelo\\(a\\) " + dsNormaAlteradora + "\\)";
                        pattern = "(<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i] + "\".*?<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "\".*?></a>.*?) <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a>(.*?)</p>";
                        replacement = "$1$2</p>";
                        break;
                    default:
                        ds_link_alterador = "\\(" + UtilVides.gerarDescricaoDoCaput(caputAlteradoDesfazer.caput[i]) + caputAlteradoDesfazer.ds_texto_para_alterador_aux + " pelo\\(a\\) " + dsNormaAlteradora + "\\)";
                        if (!string.IsNullOrEmpty(caputAlteradoDesfazer.texto_novo[i]))
                        {
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "_replaced(\".*?)replaced_by=\"" + normaAlteradora.ch_norma + "\"(.*?)<s>(.*?)<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "_replaced\".*?></a>(.*?)</s>(.*?)</p>\r\n<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i] + "\".*?>.*?" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.texto_novo[i]) + ".*? <a class=\"link_vide\".*?>.+?</a></p>";
                            replacement = "$1" + caputAlteradoDesfazer.caput[i] + "$2$3$4<a name=\"" + caputAlteradoDesfazer.caput[i] + "\"></a>" + caputAlteradoDesfazer.texto_antigo[i] + "$6</p>";
                        }
                        else
                        {
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "_replaced(\".*?)replaced_by=\"" + normaAlteradora.ch_norma + "\"(.*?)<s>(.*?)<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "_replaced\".*?></a>(.*?)</s>(.*?) <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a>(.*?)</p>";
                            replacement = "$1" + caputAlteradoDesfazer.caput[i] + "$2$3$4<a name=\"" + caputAlteradoDesfazer.caput[i] + "\"></a>" + caputAlteradoDesfazer.texto_antigo[i] + "$6$7</p>";
                        }
                        break;
                }
                if (Regex.Matches(texto, pattern).Count == 1)
                {
                    texto = Regex.Replace(texto, pattern, replacement);
                    bAlterou = true;
                }
            }
            if (!bAlterou)
            {
                texto = "";
            }
            return texto;
        }

        //public void DesfazerCaputDosArquivos(NormaOV normaAlteradora, NormaOV normaAlterada, Caput caput_norma_vide_alteradora_desfazer, Caput caput_norma_vide_alterada_desfazer)
        //{
        //    var pesquisa = new Pesquisa();
        //    var normaRn = new NormaRN();
        //    var upload = new UploadHtml();
        //    var listOp = new List<opMode<object>>();

        //    var arquivo_norma_vide_alteradora = RemoverLinkNoTextoDaNormaAlteradora(normaAlteradora, caput_norma_vide_alteradora_desfazer, caput_norma_vide_alterada_desfazer);
        //    if (arquivo_norma_vide_alteradora != "")
        //    {
        //        var retorno_file_alteradora = upload.AnexarHtml(arquivo_norma_vide_alteradora, normaAlteradora.ar_atualizado.filename, "sinj_norma");
        //        if (retorno_file_alteradora.IndexOf("id_file") > -1)
        //        {
        //            listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alteradora } });
        //            pesquisa.literal = "ch_norma='" + normaAlteradora.ch_norma + "'";
        //            normaRn.PathPut<object>(pesquisa, listOp);
        //        }
        //    }

        //    var arquivo_norma_vide_alterada = RemoverAlteracaoNoTextoDaNormaAlterada(normaAlterada, caput_norma_vide_alterada_desfazer, caput_norma_vide_alteradora_desfazer);
        //    if (arquivo_norma_vide_alterada != "")
        //    {
        //        var retorno_file_alterada = upload.AnexarHtml(arquivo_norma_vide_alterada, normaAlterada.ar_atualizado.filename, "sinj_norma");
        //        if (retorno_file_alterada.IndexOf("id_file") > -1)
        //        {
        //            listOp = new List<opMode<object>>();
        //            listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alterada } });
        //            pesquisa.literal = "ch_norma='" + normaAlterada.ch_norma + "'";
        //            normaRn.PathPut<object>(pesquisa, listOp);
        //        }
        //    }
        //}

        //public void DesfazerCaputDosArquivos(NormaOV norma_alteradora, NormaOV norma_alterada, Caput caput_norma_vide_alteradora_desfazer, string _ds_texto_para_alterador)
        //{
        //    var pesquisa = new Pesquisa();
        //    var normaRn = new NormaRN();
        //    var upload = new UploadHtml();
        //    var listOp = new List<opMode<object>>();

        //    var arquivo_norma_vide_alteradora = RemoverLinkNoTextoDaNormaAlteradora(norma_alteradora, caput_norma_vide_alteradora_desfazer, norma_alterada.ch_norma);

        //    if (arquivo_norma_vide_alteradora != "")
        //    {
        //        var retorno_file_alteradora = upload.AnexarHtml(arquivo_norma_vide_alteradora, norma_alteradora.ar_atualizado.filename, "sinj_norma");
        //        if (retorno_file_alteradora.IndexOf("id_file") > -1)
        //        {
        //            listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alteradora } });
        //            pesquisa.literal = "ch_norma='" + norma_alteradora.ch_norma + "'";
        //            normaRn.PathPut<object>(pesquisa, listOp);
        //        }
        //    }

        //    var aux_ds_texto_para_alterador = _ds_texto_para_alterador.ToLower();
        //    var aux_nm_situacao_alterada = norma_alterada.nm_situacao.ToLower();
        //    var if_file_norma_alterada = norma_alterada.getIdFileArquivoVigente();
        //    var name_file_norma_alterada = norma_alterada.getNameFileArquivoVigente();
        //    var arquivo_norma_vide_alterada = "";

        //    if ((aux_nm_situacao_alterada == "revogado" && aux_ds_texto_para_alterador == "revogado") ||
        //        (aux_nm_situacao_alterada == "anulado" && aux_ds_texto_para_alterador == "anulado") ||
        //        (aux_nm_situacao_alterada == "extinta" && aux_ds_texto_para_alterador == "extinta") ||
        //        (aux_nm_situacao_alterada == "inconstitucional" && aux_ds_texto_para_alterador == "declarado inconstitucional") ||
        //        (aux_nm_situacao_alterada == "cancelada" && aux_ds_texto_para_alterador == "cancelada") ||
        //        (aux_nm_situacao_alterada == "suspenso" && aux_ds_texto_para_alterador == "suspenso totalmente"))
        //    {
        //        arquivo_norma_vide_alterada = RemoverAlteracaoNoTextoCompletoDaNormaAlterada(norma_alterada.ch_norma, if_file_norma_alterada, _ds_texto_para_alterador);
        //    }
        //    else if (aux_ds_texto_para_alterador == "ratificado" ||
        //        aux_ds_texto_para_alterador == "reeditado" ||
        //        aux_ds_texto_para_alterador == "regulamentado" ||
        //        aux_ds_texto_para_alterador == "prorrogado" ||
        //        aux_ds_texto_para_alterador == "legislação correlata")
        //    {
        //        arquivo_norma_vide_alterada = RemoverInformacaoNoTextoDaNormaAlterada(norma_alterada.ch_norma, if_file_norma_alterada, _ds_texto_para_alterador);
        //    }

        //    if (arquivo_norma_vide_alterada != "")
        //    {
        //        var retorno_file_alterada = upload.AnexarHtml(arquivo_norma_vide_alterada, name_file_norma_alterada, "sinj_norma");
        //        if (retorno_file_alterada.IndexOf("id_file") > -1)
        //        {
        //            listOp = new List<opMode<object>>();
        //            listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alterada } });
        //            pesquisa.literal = "ch_norma='" + norma_alterada.ch_norma + "'";
        //            normaRn.PathPut<object>(pesquisa, listOp);
        //        }
        //    }
        //}

        public string RemoverAlteracaoNoTextoCompletoDaNormaAlterada(string chNormaAlteradora, string idFileNormaAlterada, string dsTextoParaAlterador)
        {
            var texto = new HtmlFileEncoded().GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);

            var pattern1 = "(?!<p.+replaced_by=.+>)(<p.+?>)<s>(.+?)</s></p>";
            Regex rx1 = new Regex(pattern1);

            var pattern2 = "(<h1.+?epigrafe=.+?>.+?</h1>)\r\n<p.*?><a.+?/" + chNormaAlteradora + "/.+?>\\(" + dsTextoParaAlterador + ".+?\\)</a></p>";
            Regex rx2 = new Regex(pattern2, RegexOptions.Singleline);

            if (rx1.Matches(texto).Count > 0 || rx2.Matches(texto).Count == 1)
            {
                var replacement1 = "$1$2</p>";
                var replacement2 = "$1";
                texto = rx1.Replace(texto, replacement1);
                texto = rx2.Replace(texto, replacement2);
            }
            else
            {
                texto = "";
            }
            return texto;
        }

        public string RemoverInformacaoNoTextoDaNormaAlterada(string chNormaAlteradora, string idFileNormaAlterada, string dsTextoParaAlterador)
        {
            var texto = new HtmlFileEncoded().GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);
            var pattern = "(<h1.+?epigrafe=.+?>.+?</h1>)\r\n<p.*?><a.+?/" + chNormaAlteradora + "/.+?>\\(" + dsTextoParaAlterador + ".+?\\)</a></p>";
            if (dsTextoParaAlterador.ToLower() == "legislação correlata")
            {
                pattern = "<p.*?><a.+?/" + chNormaAlteradora + "/.+?>\\(Legislação correlata.+?\\)</a></p>\r\n(<h1.+?epigrafe=.+?>.+?</h1>)";
            }
            if (Regex.Matches(texto, pattern).Count == 1)
            {
                var replacement = "$1";
                texto = Regex.Replace(texto, pattern, replacement);
            }
            else
            {
                texto = "";
            }
            return texto;
        }

        public string RemoverInformacaoNoTextoDaNormaAlteradora(NormaOV normaAlterada, string idFileNormaAlteradora, string dsTextoRelacao)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = "";
            if (dsTextoRelacao == "legislação correlata")
            {
                htmlFile.GetHtmlFile(idFileNormaAlteradora, "sinj_norma", null);
                texto = RemoverInformacaoNoTextoDaNormaAlteradora(texto, normaAlterada, dsTextoRelacao);
            }
            return texto;
        }

        public string RemoverInformacaoNoTextoDaNormaAlteradora(string texto, NormaOV normaAlterada, string dsTextoRelacao)
        {
            var htmlFile = new HtmlFileEncoded();
            
            var nameFileNormaAlterada = normaAlterada.getNameFileArquivoVigente();
            var dsNormaAlterada = normaAlterada.getDescricaoDaNorma();
            
            var aux_href = !string.IsNullOrEmpty(nameFileNormaAlterada) ? ("(_link_sistema_)Norma/" + normaAlterada.ch_norma + "/" + nameFileNormaAlterada) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + normaAlterada.ch_norma;

            var pattern = "<p><a href=\"" + aux_href + "\" >Legislação correlata - " + dsNormaAlterada + "</a></p>\r\n";
            if (Regex.Matches(texto, pattern).Count == 1)
            {
                texto = Regex.Replace(texto, pattern, "");
            }
            else
            {
                texto = "";
            }
            return texto;
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