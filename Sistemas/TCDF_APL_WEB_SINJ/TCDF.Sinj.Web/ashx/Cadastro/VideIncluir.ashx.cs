using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.RN;
using System.Text.RegularExpressions;
using TCDF.Sinj.Web.ashx.Arquivo;
using neo.BRLightREST;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for VideIncluir
    /// </summary>
    public class VideIncluir : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            NormaOV normaAlteradoraOv = null;
            NormaOV normaAlteradaOv = null;
            var _id_doc = context.Request["id_doc"];
            ulong id_doc = 0;
            var _dt_controle_alteracao = context.Request["dt_controle_alteracao"];
            var _ch_norma_alteradora = context.Request["ch_norma_alteradora"];
            var _ch_norma_alterada = context.Request["ch_norma_alterada"];
            var _in_norma_fora_do_sistema = context.Request["in_norma_fora_do_sistema"];

            var _ch_tipo_relacao = context.Request["ch_tipo_relacao"];
            var _nm_tipo_relacao = context.Request["nm_tipo_relacao"];
            var _ds_texto_para_alterador = context.Request["ds_texto_para_alterador"];
            var _ds_texto_para_alterado = context.Request["ds_texto_para_alterado"];
            var ch_tipo_relacao_pos_verificacao = _ch_tipo_relacao;
            var nm_tipo_relacao_pos_verificacao = _nm_tipo_relacao;
            var ds_texto_para_alterador_pos_verificacao = _ds_texto_para_alterador;
            var ds_texto_para_alterado_pos_verificacao = _ds_texto_para_alterado;

            var _caput_norma_vide_alteradora = context.Request["caput_norma_vide_alteradora"];
            //var _artigo_norma_vide = context.Request["artigo_norma_vide"];
            //var _paragrafo_norma_vide = context.Request["paragrafo_norma_vide"];
            //var _inciso_norma_vide = context.Request["inciso_norma_vide"];
            //var _alinea_norma_vide = context.Request["alinea_norma_vide"];
            //var _item_norma_vide = context.Request["item_norma_vide"];
            //var _anexo_norma_vide = context.Request["anexo_norma_vide"];
            var _caput_norma_vide_alterada = context.Request["caput_norma_vide_alterada"];
            var _ds_caput_norma_alterada = context.Request["ds_caput_norma_alterada"];
            var _artigo_norma_vide_alterada = context.Request["artigo_norma_vide_alterada"];
            var _paragrafo_norma_vide_alterada = context.Request["paragrafo_norma_vide_alterada"];
            var _inciso_norma_vide_alterada = context.Request["inciso_norma_vide_alterada"];
            var _alinea_norma_vide_alterada = context.Request["alinea_norma_vide_alterada"];
            var _item_norma_vide_alterada = context.Request["item_norma_vide_alterada"];
            var _anexo_norma_vide_alterada = context.Request["anexo_norma_vide_alterada"];
            var _ds_comentario_vide = context.Request["ds_comentario_vide"];
            var _ch_tipo_norma_vide_fora_do_sistema = context.Request["ch_tipo_norma_vide_fora_do_sistema"];
            var _nm_tipo_norma_vide_fora_do_sistema = context.Request["nm_tipo_norma_vide_fora_do_sistema"];
            var _nr_norma_vide_fora_do_sistema = context.Request["nr_norma_vide_fora_do_sistema"];
            var _ch_tipo_fonte_vide_fora_do_sistema = context.Request["ch_tipo_fonte_vide_fora_do_sistema"];
            var _nm_tipo_fonte_vide_fora_do_sistema = context.Request["nm_tipo_fonte_vide_fora_do_sistema"];
            var _dt_publicacao_norma_vide_fora_do_sistema = context.Request["dt_publicacao_norma_vide_fora_do_sistema"];
            var _nr_pagina_publicacao_norma_vide_fora_do_sistema = context.Request["nr_pagina_publicacao_norma_vide_fora_do_sistema"];
            var _nr_coluna_publicacao_norma_vide_fora_do_sistema = context.Request["nr_coluna_publicacao_norma_vide_fora_do_sistema"];

            var _caput_texto_novo = context.Request.Form.GetValues("texto_novo");
            
            var action = AcoesDoUsuario.nor_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                var normaRn = new NormaRN();
                var dDt_controle_alteracao = Convert.ToDateTime(_dt_controle_alteracao);

                if (!string.IsNullOrEmpty(_ch_norma_alteradora))
                {
                    if (!string.IsNullOrEmpty(_ch_tipo_relacao))
                    {
                        normaAlteradoraOv = normaRn.Doc(_ch_norma_alteradora);
                        id_doc = normaAlteradoraOv._metadata.id_doc;

                        //Vai comparar a data da ultima alteração com a data que o usuário abriu a página de editar vides e disparar uma exceção de risco de inconsistencia
                        if (normaAlteradoraOv.alteracoes.Count > 0)
                        {
                            var dDt_alteracao = Convert.ToDateTime(normaAlteradoraOv.alteracoes.Last<AlteracaoOV>().dt_alteracao);
                            var usuario = normaAlteradoraOv.alteracoes.Last<AlteracaoOV>().nm_login_usuario_alteracao;
                            if (dDt_controle_alteracao < dDt_alteracao)
                            {
                                throw new RiskOfInconsistency("A norma alteradora foi modificada pelo usuário <b>" + usuario + "</b> às <b>" + dDt_alteracao + "</b> tornando a sua modificação inconsistente.<br/> É aconselhável atualizar a página e refazer as modificações ou forçar a alteração.<br/>Obs.: Clicar em 'Salvar mesmo assim' vai forçar a alteração e pode sobrescrever as modificações do usuário <b>" + usuario + "</b>.");
                            }
                        }

                        TipoDeRelacaoOV relacao = null;
                        //if (!string.IsNullOrEmpty(_artigo_norma_vide_alterada))
                        //{
                        //    relacao = normaRn.ObterRelacaoParcial(_ch_tipo_relacao);
                        //}
                        //else
                        //{
                        relacao = normaRn.ObterRelacao(_ch_tipo_relacao);
                        //}

                        if (relacao != null)
                        {
                            ch_tipo_relacao_pos_verificacao = relacao.ch_tipo_relacao;
                            nm_tipo_relacao_pos_verificacao = relacao.nm_tipo_relacao;
                            ds_texto_para_alterado_pos_verificacao = relacao.ds_texto_para_alterado;
                            ds_texto_para_alterador_pos_verificacao = relacao.ds_texto_para_alterador;
                        }


                        var vide_alterador = new Vide();
                        var ch_vide = Guid.NewGuid().ToString("N");
                        ///Esse vide será armazenado no documento da norma alteradora, logo devo referenciar qual a norma alterada.
                        ///Assim como também devo colocar aqui todas informações que devem aparecer nos detalhes do documento.
                        ///Exemplo: Revoga paragrafo tal da norma tal...
                        vide_alterador.ch_vide = ch_vide;
                        vide_alterador.in_relacao_de_acao = normaAlteradoraOv.st_acao;
                        vide_alterador.ch_norma_vide = _ch_norma_alterada;

                        vide_alterador.ch_tipo_relacao = ch_tipo_relacao_pos_verificacao;
                        vide_alterador.nm_tipo_relacao = nm_tipo_relacao_pos_verificacao;
                        vide_alterador.ds_texto_relacao = ds_texto_para_alterado_pos_verificacao; //Exemplo: Revoga Totalmente

                        vide_alterador.in_norma_afetada = false;
                        vide_alterador.in_relacao_de_acao = false;
                        if (_in_norma_fora_do_sistema == "1")
                        {
                            vide_alterador.ch_tipo_norma_vide = _ch_tipo_norma_vide_fora_do_sistema;
                            vide_alterador.nm_tipo_norma_vide = _nm_tipo_norma_vide_fora_do_sistema;
                            vide_alterador.ch_tipo_fonte_norma_vide = _ch_tipo_fonte_vide_fora_do_sistema;
                            vide_alterador.nm_tipo_fonte_norma_vide = _nm_tipo_fonte_vide_fora_do_sistema;
                            vide_alterador.nr_norma_vide = _nr_norma_vide_fora_do_sistema;
                            vide_alterador.dt_publicacao_fonte_norma_vide = _dt_publicacao_norma_vide_fora_do_sistema;
                            vide_alterador.pagina_publicacao_norma_vide = _nr_pagina_publicacao_norma_vide_fora_do_sistema;
                            vide_alterador.coluna_publicacao_norma_vide = _nr_coluna_publicacao_norma_vide_fora_do_sistema;
                        }
                        else if (!string.IsNullOrEmpty(_ch_norma_alterada))
                        {
                            normaAlteradaOv = normaRn.Doc(_ch_norma_alterada);
                            //Vai comparar a data da ultima alteração com a data que o usuário abriu a página de editar vides e disparar uma exceção de risco de inconsistencia
                            if (normaAlteradaOv.alteracoes.Count > 0)
                            {
                                var dDt_alteracao = Convert.ToDateTime(normaAlteradaOv.alteracoes.Last<AlteracaoOV>().dt_alteracao);
                                var usuario = normaAlteradaOv.alteracoes.Last<AlteracaoOV>().nm_login_usuario_alteracao;
                                if (dDt_controle_alteracao < dDt_alteracao)
                                {
                                    throw new RiskOfInconsistency("A norma alterada foi modificada pelo usuário <b>" + usuario + "</b> às <b>" + dDt_alteracao + "</b> tornando a sua modificação inconsistente.<br/> É aconselhável atualizar a página e refazer as modificações ou forçar a alteração.<br/>Obs.: Clicar em 'Salvar mesmo assim' vai forçar a alteração e pode sobrescrever as modificações do usuário <b>" + usuario + "</b>.");
                                }
                            }
                            vide_alterador.ch_norma_vide = normaAlteradaOv.ch_norma;
                            vide_alterador.ch_tipo_norma_vide = normaAlteradaOv.ch_tipo_norma;
                            vide_alterador.nm_tipo_norma_vide = normaAlteradaOv.nm_tipo_norma;
                            vide_alterador.nr_norma_vide = normaAlteradaOv.nr_norma;
                            if (vide_alterador.nr_norma_vide == "0")
                            {
                                vide_alterador.nr_norma_vide = "";
                            }
                            vide_alterador.dt_assinatura_norma_vide = normaAlteradaOv.dt_assinatura;
                            if (Convert.ToDateTime(normaAlteradaOv.dt_assinatura) > Convert.ToDateTime(normaAlteradoraOv.dt_assinatura))
                            {
                                // Essa regra nao se aplica caso a norma alteradora seja ADI.
                                if (normaAlteradoraOv.ch_tipo_norma != "11000000")
                                {
                                    throw new DocValidacaoException("Uma norma não pode possuir seu status alterado por uma norma anterior. Verifique as datas de assinatura das normas alteradora e alterada.");
                                }
                            }
                            if (normaAlteradoraOv.st_acao)
                            {
                                var tipo_norma_alterada = new TipoDeNormaRN().Doc(normaAlteradaOv.ch_tipo_norma);
                                if (!tipo_norma_alterada.in_questionavel)
                                {
                                    throw new DocValidacaoException("Uma ação não pode alterar uma norma que não é questionável.</br>Verifique no cadastro de Tipo de Norma se o Tipo '" + tipo_norma_alterada.nm_tipo_norma + "' está marcado como Questionável.");
                                }
                            }
                        }
                        else
                        {
                            throw new DocValidacaoException("Erro ao informar a norma alterada. Verifique se está selecionando corretamente a norma afetada pelo vide.");
                        }
                        //vide_alterador.alinea_norma_vide = _alinea_norma_vide;
                        //vide_alterador.anexo_norma_vide = _anexo_norma_vide;
                        //vide_alterador.artigo_norma_vide = _artigo_norma_vide;
                        //vide_alterador.paragrafo_norma_vide = _paragrafo_norma_vide;
                        //vide_alterador.inciso_norma_vide = _inciso_norma_vide;
                        //vide_alterador.item_norma_vide = _item_norma_vide;

                        vide_alterador.alinea_norma_vide_outra = _alinea_norma_vide_alterada;
                        vide_alterador.anexo_norma_vide_outra = _anexo_norma_vide_alterada;
                        vide_alterador.artigo_norma_vide_outra = _artigo_norma_vide_alterada;

                        if (!string.IsNullOrEmpty(_caput_norma_vide_alteradora))
                        {
                            vide_alterador.caput_norma_vide = JSON.Deserializa<Caput>(_caput_norma_vide_alteradora);
                            vide_alterador.caput_norma_vide.nm_relacao_aux = nm_tipo_relacao_pos_verificacao.ToLower();
                            vide_alterador.caput_norma_vide.ds_texto_para_alterador_aux = ds_texto_para_alterador_pos_verificacao.ToLower();
                        }
                        if (!string.IsNullOrEmpty(_caput_norma_vide_alterada))
                        {
                            vide_alterador.caput_norma_vide_outra = JSON.Deserializa<Caput>(_caput_norma_vide_alterada);
                            vide_alterador.caput_norma_vide_outra.ds_caput = _ds_caput_norma_alterada;
                            vide_alterador.caput_norma_vide_outra.texto_novo = _caput_texto_novo;
                            vide_alterador.caput_norma_vide_outra.nm_relacao_aux = nm_tipo_relacao_pos_verificacao.ToLower();
                            vide_alterador.caput_norma_vide_outra.ds_texto_para_alterador_aux = ds_texto_para_alterador_pos_verificacao.ToLower();
                        }

                        vide_alterador.paragrafo_norma_vide_outra = _paragrafo_norma_vide_alterada;
                        vide_alterador.inciso_norma_vide_outra = _inciso_norma_vide_alterada;
                        vide_alterador.item_norma_vide_outra = _item_norma_vide_alterada;

                        vide_alterador.ds_comentario_vide = _ds_comentario_vide;

                        normaAlteradoraOv.vides.Add(vide_alterador);

                        var dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                        normaAlteradoraOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                        if (normaRn.Atualizar(normaAlteradoraOv._metadata.id_doc, normaAlteradoraOv))
                        {
                            if (normaAlteradaOv != null)
                            {
                                var vide_alterada = new Vide();
                                vide_alterada.ch_vide = ch_vide;
                                vide_alterada.in_relacao_de_acao = normaAlteradoraOv.st_acao;

                                vide_alterada.ch_tipo_relacao = ch_tipo_relacao_pos_verificacao;
                                vide_alterada.nm_tipo_relacao = nm_tipo_relacao_pos_verificacao;
                                vide_alterada.ds_texto_relacao = ds_texto_para_alterador_pos_verificacao; //Exemplo: Revogado Totalmente

                                vide_alterada.in_norma_afetada = true;
                                vide_alterada.ch_norma_vide = normaAlteradoraOv.ch_norma;
                                vide_alterada.ch_tipo_norma_vide = normaAlteradoraOv.ch_tipo_norma;
                                vide_alterada.nm_tipo_norma_vide = normaAlteradoraOv.nm_tipo_norma;
                                vide_alterada.nr_norma_vide = normaAlteradoraOv.nr_norma;
                                if (vide_alterada.nr_norma_vide == "0")
                                {
                                    vide_alterada.nr_norma_vide = "";
                                }
                                vide_alterada.dt_assinatura_norma_vide = normaAlteradoraOv.dt_assinatura;

                                vide_alterada.alinea_norma_vide = _alinea_norma_vide_alterada;
                                vide_alterada.anexo_norma_vide = _anexo_norma_vide_alterada;
                                vide_alterada.artigo_norma_vide = _artigo_norma_vide_alterada;
                                vide_alterada.paragrafo_norma_vide = _paragrafo_norma_vide_alterada;
                                vide_alterada.inciso_norma_vide = _inciso_norma_vide_alterada;
                                vide_alterada.item_norma_vide = _item_norma_vide_alterada;
                                vide_alterada.caput_norma_vide = vide_alterador.caput_norma_vide_outra;

                                vide_alterada.caput_norma_vide_outra = vide_alterador.caput_norma_vide;

                                vide_alterada.ds_comentario_vide = _ds_comentario_vide;

                                normaAlteradaOv.vides.Add(vide_alterada);
                                if (!normaAlteradaOv.st_situacao_forcada)
                                {
                                    var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                                    normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                                    normaAlteradaOv.nm_situacao = situacao.nm_situacao;
                                }
                                normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                                if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                                {
                                    sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\"" + normaAlteradoraOv.ch_norma + "\", \"dt_controle_alteracao\":\"" + DateTime.Now.AddSeconds(1).ToString("dd'/'MM'/'yyyy HH:mm:ss") + "\"}";
                                    VerificarDispositivosESalvarOsTextosAntigosDasNormas(normaAlteradoraOv, normaAlteradaOv, vide_alterador, vide_alterada, sessao_usuario.nm_login_usuario);
                                    VerificarDispositivosEAlterarOsTextosDasNormas(normaAlteradoraOv, normaAlteradaOv, vide_alterador, vide_alterada);
                                }
                                else
                                {
                                    throw new Exception("Erro ao cadastrar Vide na norma alterada.");
                                }
                            }
                            else
                            {
                                sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\"" + normaAlteradoraOv.ch_norma + "\", \"dt_controle_alteracao\":\"" + DateTime.Now.AddSeconds(1).ToString("dd'/'MM'/'yyyy HH:mm:ss") + "\"}";
                            }
                        }
                        else
                        {
                            throw new Exception("Erro ao cadastrar Vide.");
                        }
                    }
                    else
                    {
                        throw new DocValidacaoException("Tipo de Relação não informado. Verifique se o Tipo de Relação está selecionado.");
                    }
                }
                else
                {
                    throw new DocValidacaoException("Erro na norma informada.");
                }


                var log_editar = new LogAlterar<NormaOV>
                {
                    registro = normaAlteradoraOv
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ",VIDE.INC", log_editar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (FileNotFoundException ex)
            {
                sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\"" + normaAlteradoraOv.ch_norma + "\", \"alert_message\": \"" + ex.Message + "\"}";
            }
            catch (Exception ex)
            {

                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException || ex is DocValidacaoException || ex is RiskOfInconsistency)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"type_error\":\"" + ex.GetType().Name + "\", \"dt_controle_alteracao\":\"" + DateTime.Now.AddSeconds(1).ToString("dd'/'MM'/'yyyy HH:mm:ss") + "\"}";
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
                LogErro.gravar_erro(Util.GetEnumDescription(action) + ",VIDE.INC", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            context.Response.Write(sRetorno);
            context.Response.End();

        }

        public void VerificarDispositivosESalvarOsTextosAntigosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterador, Vide videAlterado, string nm_login_usuario)
        {
            //Se possuir dispositivo nas duas normas
            if (videAlterador.caput_norma_vide != null && videAlterado.caput_norma_vide != null && videAlterador.caput_norma_vide.caput != null && videAlterado.caput_norma_vide.caput != null && videAlterador.caput_norma_vide.caput.Length > 0 && videAlterado.caput_norma_vide.caput.Length > 0)
            {
                SalvarTextoAntigoDaNorma(normaAlteradora, videAlterador, nm_login_usuario);
                SalvarTextoAntigoDaNorma(normaAlterada, videAlterado, nm_login_usuario);
            }
            //Se possuir dispositivo na norma alterada
            else if (videAlterado.caput_norma_vide != null && videAlterado.caput_norma_vide.caput != null && videAlterado.caput_norma_vide.caput.Length > 0)
            {
                SalvarTextoAntigoDaNorma(normaAlterada, videAlterado, nm_login_usuario);
            }
            //Se possuir dispositivo na norma alteradora
            else if (videAlterador.caput_norma_vide != null && videAlterador.caput_norma_vide.caput != null && videAlterador.caput_norma_vide.caput.Length > 0)
            {
                SalvarTextoAntigoDaNorma(normaAlteradora, videAlterador, nm_login_usuario);
                SalvarTextoAntigoDaNorma(normaAlterada, videAlterado, nm_login_usuario);
            }
            //Se não possuir dispositivo nas normas. Tem que verificar se não tem o dispositivo informado manualmente, pois se tem não aplica a alteração no arquivo
            else if (!videAlterado.possuiDispositivoInformadoManualmente())
            {
                SalvarTextoAntigoDaNorma(normaAlteradora, videAlterador, nm_login_usuario);
                SalvarTextoAntigoDaNorma(normaAlterada, videAlterado, nm_login_usuario);
            }
        }

        /// <summary>
        /// Inserir alteração nos dispositivos do arquivo da norma alterada.
        /// </summary>
        /// <param name="normaAlteradora"></param>
        /// <param name="chNorma_alterada"></param>
        /// <param name="caputNormaVideAlterada"></param>
        public void SalvarTextoAntigoDaNorma(NormaOV norma, Vide vide, string nm_login_usuario)
        {
            var normaRn = new NormaRN();
            var arquivoVersionadoRn = new ArquivoVersionadoNormaRN();
            
            var fileNorma = norma.getFileArquivoVigente();

            if(fileNorma.id_file != null){

                var byteFileNorma = normaRn.Download(fileNorma.id_file);

                if (byteFileNorma != null)
                {
                    var sRetornoFileNomra = arquivoVersionadoRn.AnexarArquivo(new FileParameter(byteFileNorma, fileNorma.filename, fileNorma.mimetype));
                    var retornoFileNomra = JSON.Deserializa<ArquivoOV>(sRetornoFileNomra);
                    if (retornoFileNomra.id_file != null)
                    {
                        var arquivoVersionado = new ArquivoVersionadoNormaOV()
                        {
                            ch_norma = norma.ch_norma,
                            ch_vide = vide.ch_vide,
                            norma = norma,
                            ar_arquivo_versionado = retornoFileNomra,
                            dt_arquivo_versionado = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"),
                            nm_login_usuario = nm_login_usuario

                        };
                        arquivoVersionadoRn.Incluir(arquivoVersionado);
                    }
                }
            }
        }

        public void VerificarDispositivosEAlterarOsTextosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterador, Vide videAlterado)
        {
            //Se possuir dispositivo nas duas normas
            if (videAlterador.caput_norma_vide != null && videAlterado.caput_norma_vide != null && videAlterador.caput_norma_vide.caput != null && videAlterado.caput_norma_vide.caput != null && videAlterador.caput_norma_vide.caput.Length > 0 && videAlterado.caput_norma_vide.caput.Length > 0)
            {
                IncluirAlteracaoComDispositivosNosArquivosDasNormas(normaAlteradora, normaAlterada, videAlterador.caput_norma_vide, videAlterado.caput_norma_vide);
            }
            //Se possuir dispositivo na norma alterada
            else if (videAlterado.caput_norma_vide != null && videAlterado.caput_norma_vide.caput != null && videAlterado.caput_norma_vide.caput.Length > 0)
            {
                IncluirAlteracaoComDispositivoAlteradoNosArquivosDasNormas(normaAlteradora, normaAlterada, videAlterado.caput_norma_vide);
            }
            //Se possuir dispositivo na norma alteradora
            else if (videAlterador.caput_norma_vide != null && videAlterador.caput_norma_vide.caput != null && videAlterador.caput_norma_vide.caput.Length > 0)
            {
                IncluirAlteracaoComDispositivoAlteradorNosArquivosDasNormas(normaAlteradora, normaAlterada, videAlterador.caput_norma_vide);
            }
            //Se não possuir dispositivo nas normas. Tem que verificar se não tem o dispositivo informado manualmente, pois se tem não aplica a alteração no arquivo
            else if (!videAlterado.possuiDispositivoInformadoManualmente())
            {
                IncluirAlteracaoSemDispositivosNosArquivosDasNormas(normaAlteradora, normaAlterada, videAlterado);
            }
        }

        /// <summary>
        /// Inserir alteração nos dispositivos dos arquivos das normas alteradora e alterada
        /// </summary>
        /// <param name="ch_norma_alteradora"></param>
        /// <param name="ch_norma_alterada"></param>
        /// <param name="caput_norma_vide_alteradora"></param>
        /// <param name="caput_norma_vide_alterada"></param>
        /// <param name="_caput_texto_novo"></param>
        public void IncluirAlteracaoComDispositivosNosArquivosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Caput caput_norma_vide_alteradora, Caput caput_norma_vide_alterada)
        {
            var upload = new UploadHtml();
            var normaRn = new NormaRN();
            var pesquisa = new Pesquisa();

            var arquivo_norma_vide_alteradora = CriarLinkNoTextoDaNormaAlteradora(caput_norma_vide_alteradora, caput_norma_vide_alterada);
            if (!string.IsNullOrEmpty(arquivo_norma_vide_alteradora))
            {
                var retorno_file_alteradora = upload.AnexarHtml(arquivo_norma_vide_alteradora, caput_norma_vide_alteradora.filename, "sinj_norma");

                if (retorno_file_alteradora.IndexOf("id_file") > -1)
                {
                    normaRn.PathPut(normaAlteradora._metadata.id_doc, "ar_atualizado", retorno_file_alteradora, null);
                }
            }
            var arquivo_norma_vide_alterada = AlterarDispositivosDaNormaAlterada(caput_norma_vide_alterada, caput_norma_vide_alteradora);
            if (!string.IsNullOrEmpty(arquivo_norma_vide_alterada))
            {
                var retorno_file_alterada = upload.AnexarHtml(arquivo_norma_vide_alterada, caput_norma_vide_alterada.filename, "sinj_norma");
                if (retorno_file_alterada.IndexOf("id_file") > -1)
                {
                    normaRn.PathPut(normaAlterada._metadata.id_doc, "ar_atualizado", retorno_file_alterada, null);
                }
            }
        }

        /// <summary>
        /// Inserir alteração nos dispositivos do arquivo da norma alterada.
        /// </summary>
        /// <param name="normaAlteradora"></param>
        /// <param name="chNorma_alterada"></param>
        /// <param name="caputNormaVideAlterada"></param>
        public void IncluirAlteracaoComDispositivoAlteradoNosArquivosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Caput caputNormaVideAlterada)
        {
            var upload = new UploadHtml();
            var normaRn = new NormaRN();
            var pesquisa = new Pesquisa();

            var nameFileNormaAlteradora = normaAlteradora.getNameFileArquivoVigente();

            var arquivoNormaVideAlterada = AlterarDispositivosDaNormaAlterada(caputNormaVideAlterada, normaAlteradora);
            if (!string.IsNullOrEmpty(arquivoNormaVideAlterada))
            {
                var retorno_file_alterada = upload.AnexarHtml(arquivoNormaVideAlterada, caputNormaVideAlterada.filename, "sinj_norma");
                if (retorno_file_alterada.IndexOf("id_file") > -1)
                {
                    normaRn.PathPut(normaAlterada._metadata.id_doc, "ar_atualizado", retorno_file_alterada, null);
                }
            }
        }
        
        /// <summary>
        /// Insere a alteração no texto da norma alterada, não tem dispositivo alterado, então pode riscar o texto inteiro (quando é revogado, cancelado, etc) ou só cria um link (quando é LECO, Ratificação, etc).
        /// </summary>
        /// <param name="norma_alteradora"></param>
        /// <param name="norma_alterada"></param>
        /// <param name="caput_norma_vide_alterada"></param>
        /// <param name="_ds_texto_para_alterador"></param>
        public void IncluirAlteracaoComDispositivoAlteradorNosArquivosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Caput caputNormaVideAlterador)
        {
            var upload = new UploadHtml();
            var normaRn = new NormaRN();
            var pesquisa = new Pesquisa();

            var id_file_norma_alterada = normaAlterada.getIdFileArquivoVigente();
            var name_file_norma_alterada = normaAlterada.getNameFileArquivoVigente();

            if (!string.IsNullOrEmpty(id_file_norma_alterada))
            {
                var aux_nm_situacao_alterada = normaAlterada.nm_situacao.ToLower();

                var arquivo_norma_vide_alterada = "";

                if (UtilVides.EhAlteracaoCompleta(aux_nm_situacao_alterada, caputNormaVideAlterador.ds_texto_para_alterador_aux))
                {
                    arquivo_norma_vide_alterada = AlterarTextoCompletoDaNormaAlterada(normaAlteradora, id_file_norma_alterada, caputNormaVideAlterador);
                }
                else if (UtilVides.EhLegislacaoCorrelata(caputNormaVideAlterador.ds_texto_para_alterador_aux))
                {
                    arquivo_norma_vide_alterada = AcrescentarInformacaoNoTextoDaNormaAlterada(normaAlteradora, id_file_norma_alterada, caputNormaVideAlterador);
                }

                if (!string.IsNullOrEmpty(arquivo_norma_vide_alterada))
                {
                    var retorno_file_alterada = upload.AnexarHtml(arquivo_norma_vide_alterada, name_file_norma_alterada, "sinj_norma");

                    if (retorno_file_alterada.IndexOf("id_file") > -1)
                    {
                        normaRn.PathPut(normaAlterada._metadata.id_doc, "ar_atualizado", retorno_file_alterada, null);

                        var arquivo_norma_vide_revogadora = CriarLinkNoTextoDaNormaAlteradora(caputNormaVideAlterador, normaAlterada.ch_norma, name_file_norma_alterada);
                        if (!string.IsNullOrEmpty(arquivo_norma_vide_revogadora))
                        {
                            var retorno_file_alteradora = upload.AnexarHtml(arquivo_norma_vide_revogadora, caputNormaVideAlterador.filename, "sinj_norma");

                            if (retorno_file_alteradora.IndexOf("id_file") > -1)
                            {
                                normaRn.PathPut(normaAlteradora._metadata.id_doc, "ar_atualizado", retorno_file_alteradora, null);
                            }
                        }
                    }
                }
            }

        }

        public void IncluirAlteracaoSemDispositivosNosArquivosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterado)
        {
            var upload = new UploadHtml();
            var normaRn = new NormaRN();
            var pesquisa = new Pesquisa();

            var idFileNormaAlteradora = normaAlteradora.getIdFileArquivoVigente();
            var nameFileNormaAlteradora = normaAlteradora.getNameFileArquivoVigente();

            var if_file_norma_alterada = normaAlterada.getIdFileArquivoVigente();
            var name_file_norma_alterada = normaAlterada.getNameFileArquivoVigente();

            if (!string.IsNullOrEmpty(if_file_norma_alterada))
            {
                var aux_nm_situacao_alterada = normaAlterada.nm_situacao.ToLower();
                var aux_ds_texto_alterador = videAlterado.ds_texto_relacao.ToLower();

                var arquivo_norma_vide_alterada = "";

                if (UtilVides.EhAlteracaoCompleta(aux_nm_situacao_alterada, aux_ds_texto_alterador))
                {
                    arquivo_norma_vide_alterada = AlterarTextoCompletoDaNormaAlterada(normaAlteradora, if_file_norma_alterada, aux_ds_texto_alterador);
                }
                else if (UtilVides.EhLegislacaoCorrelata(aux_ds_texto_alterador))
                {
                    arquivo_norma_vide_alterada = AcrescentarInformacaoNoTextoDaNormaAlterada(normaAlteradora, if_file_norma_alterada, aux_ds_texto_alterador);
                }

                if (!string.IsNullOrEmpty(arquivo_norma_vide_alterada))
                {
                    var retorno_file_alterada = upload.AnexarHtml(arquivo_norma_vide_alterada, name_file_norma_alterada, "sinj_norma");

                    if (retorno_file_alterada.IndexOf("id_file") > -1)
                    {
                        normaRn.PathPut(normaAlterada._metadata.id_doc, "ar_atualizado", retorno_file_alterada, null);

                        var arquivoNormaAlteradora = AcrescentarInformacaoNoTextoDaNormaAlteradora(normaAlterada, idFileNormaAlteradora, aux_ds_texto_alterador);
                        if (!string.IsNullOrEmpty(arquivoNormaAlteradora))
                        {
                            var retornoFileAlteradora = upload.AnexarHtml(arquivoNormaAlteradora, nameFileNormaAlteradora, "sinj_norma");

                            if (retornoFileAlteradora.IndexOf("id_file") > -1)
                            {
                                normaRn.PathPut(normaAlteradora._metadata.id_doc, "ar_atualizado", retornoFileAlteradora, null);
                            }
                        }
                    }
                }
            }
        }        

        /// <summary>
        /// Alterar o texto da norma alteradora para criar um link para o dispositivo da norma alterada
        /// </summary>
        /// <param name="_caput_alteradora"></param>
        /// <param name="_caput_alterada"></param>
        /// <returns></returns>
        public string CriarLinkNoTextoDaNormaAlteradora(Caput _caput_alteradora, Caput _caput_alterada)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = htmlFile.GetHtmlFile(_caput_alteradora.id_file, "sinj_norma", null);
            return CriarLinkNoTextoDaNormaAlteradora(texto, _caput_alteradora, _caput_alterada);
        }

        public string CriarLinkNoTextoDaNormaAlteradora(string texto, Caput _caput_alteradora, Caput _caput_alterada)
        {
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
            var pattern = "(<p.+?linkname=\"" + _caput_alteradora.caput[0] + "\".*?>)(.*?)<a(.*?)>" + UtilVides.EscapeCharsInToPattern(_caput_alteradora.link) + "</a>(.*?)</p>";
            if (Regex.Matches(texto, pattern).Count == 0)
            {
                pattern = "(<p.+?linkname=\"" + _caput_alteradora.caput[0] + "\".*?>)(.*?)" + UtilVides.EscapeCharsInToPattern(_caput_alteradora.link) + "(.*?)</p>";
                if (Regex.Matches(texto, pattern).Count == 1)
                {
                    var replacement = "$1$2" + "<a href=\"(_link_sistema_)Norma/" + _caput_alterada.ch_norma + '/' + _caput_alterada.filename + "#" + pattern_caput + "\">" + _caput_alteradora.link + "</a>" + "$3</p>";
                    texto = Regex.Replace(texto, pattern, replacement);
                }
                else
                {
                    texto = "";
                }
            }
            return texto;
        }

        public string CriarLinkNoTextoDaNormaAlteradora(Caput _caput_revogadora, string ch_norma_revogada, string name_file_norma_revogada)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = htmlFile.GetHtmlFile(_caput_revogadora.id_file, "sinj_norma", null);
            return CriarLinkNoTextoDaNormaAlteradora(texto, _caput_revogadora, ch_norma_revogada, name_file_norma_revogada);
        }

        public string CriarLinkNoTextoDaNormaAlteradora(string texto, Caput _caput_alteradora, string ch_norma_alterada, string name_file_norma_alterada)
        {
            var pattern = "(<p.+?linkname=\"" + _caput_alteradora.caput[0] + "\".*?>)(.*?)<a(.*?)>" + UtilVides.EscapeCharsInToPattern(_caput_alteradora.link) + "</a>(.*?)</p>";
            if (Regex.Matches(texto, pattern).Count == 0)
            {
                pattern = "(<p.+?linkname=\"" + _caput_alteradora.caput[0] + "\".*?>)(.*?)" + UtilVides.EscapeCharsInToPattern(_caput_alteradora.link) + "(.*?)</p>";
                if (Regex.Matches(texto, pattern).Count == 1)
                {
                    var replacement = "$1$2" + "<a href=\"(_link_sistema_)Norma/" + ch_norma_alterada + '/' + name_file_norma_alterada + "\">" + _caput_alteradora.link + "</a>" + "$3</p>";
                    texto = Regex.Replace(texto, pattern, replacement);
                }
                else
                {
                    texto = "";
                }
            }
            return texto;
        }

        /// <summary>
        /// Faz a alteração no dispositivo da norma alterada e com link para o dispositivo da norma alteradora
        /// </summary>
        /// <param name="_caput_alterada"></param>
        /// <param name="_caput_alteradora"></param>
        /// <returns></returns>
        public string AlterarDispositivosDaNormaAlterada(Caput _caput_alterada, Caput _caput_alteradora)
        {
            var texto = "";
            if (_caput_alterada.caput.Length == _caput_alterada.texto_antigo.Length)
            {
                var htmlFile = new HtmlFileEncoded();
                texto = htmlFile.GetHtmlFile(_caput_alterada.id_file, "sinj_norma", null);
                texto = AlterarDispositivosDaNormaAlterada(texto, _caput_alterada, _caput_alteradora);
            }
            return texto;
        }

        /// <summary>
        /// Recupera o texto da norma alterada e faz alterações nos seus dispositivos. As alterações possuem links para o dispositivo do texto da norma alteradora.
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="_caput_alterada"></param>
        /// <param name="_caput_alteradora"></param>
        /// <returns></returns>
        public string AlterarDispositivosDaNormaAlterada(string texto, Caput _caput_alterada, Caput _caput_alteradora)
        {
            if (_caput_alterada.caput.Length == _caput_alterada.texto_antigo.Length)
            {
                var pattern = "";
                var replacement = "";
                var ds_link_alterador = "";
                var bAlterou = false;
                for (var i = 0; i < _caput_alterada.caput.Length; i++)
                {
                    switch (_caput_alterada.nm_relacao_aux)
                    {
                        case "acrescimo":
                            var texto_novo_splited = _caput_alterada.texto_novo[i].Split('\n');
                            pattern = "(<p.+?linkname=\")" + _caput_alterada.caput[i] + "(\".*?)(replaced_by=\".*?\")(.*?>)(.*?<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?</p>)";
                            replacement = "$1" + _caput_alterada.caput[i] + "$2$3$4$5";
                            //É necessário verificar a existencia do atributo replaced_by no paragrafo antes se aplicar o regex.
                            //Quando tem o parametro replaced_by tem que evitar acrescentá-lo nos paragrafos que estão sendo acrescidos.
                            if (Regex.Matches(texto, pattern).Count == 1)
                            {
                                for (var j = 0; j < texto_novo_splited.Length; j++)
                                {
                                    ds_link_alterador = "(" + UtilVides.gerarDescricaoDoTexto(texto_novo_splited[j]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + _caput_alteradora.ds_norma + ")";
                                    replacement += "\r\n$1" + _caput_alterada.caput[i] + "_add_" + j + "$2$4<a name=\"" + _caput_alterada.caput[i] + "_add_" + j + "\"></a>" + texto_novo_splited[j] + " <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                                }
                            }
                            else
                            {
                                pattern = "(<p.+?linkname=\")" + _caput_alterada.caput[i] + "(\".*?>)(.*?<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?</p>)";
                                replacement = "$1" + _caput_alterada.caput[i] + "$2$3";
                                for (var j = 0; j < texto_novo_splited.Length; j++)
                                {
                                    ds_link_alterador = "(" + UtilVides.gerarDescricaoDoTexto(texto_novo_splited[j]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + _caput_alteradora.ds_norma + ")";
                                    replacement += "\r\n$1" + _caput_alterada.caput[i] + "_add_" + j + "$2<a name=\"" + _caput_alterada.caput[i] + "_add_" + j + "\"></a>" + texto_novo_splited[j] + " <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                                }
                            }
                            break;
                        case "renumeração":
                            ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + _caput_alteradora.ds_norma + ")";
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?)<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?(<a class=\"link_vide\".+?</a>)</p>";
                            if (Regex.Matches(texto, pattern).Count == 1)
                            {
                                replacement = "$1" + _caput_alterada.caput[i] + "_renum$2<a name=\"" + _caput_alterada.caput[i] + "_renum\"></a>" + _caput_alterada.texto_novo[i] + " $3 <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                            }
                            else
                            {
                                pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?)<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?</p>";
                                if (Regex.Matches(texto, pattern).Count == 1)
                                {
                                    replacement = "$1" + _caput_alterada.caput[i] + "_renum$2<a name=\"" + _caput_alterada.caput[i] + "_renum\"></a>" + _caput_alterada.texto_novo[i] + " <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                                }
                            }
                            break;
                        case "revigoração":
                            ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i].Replace("_replaced","")) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + _caput_alteradora.ds_norma + ")";
                            pattern = "(<p.+?linkname=\"" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "\".*?)replaced_by=\"(.*?)\"(.*?)<s>(.*?)</s>(.*?)</p>";
                            replacement = "$1replaced_by_disabled=\"$2\"$3$4$5 <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                            if (!string.IsNullOrEmpty(_caput_alterada.texto_novo[i]))
                            {
                                pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?)replaced_by=\"(.*?)\"(.*?>)(.*?)(<a.+?name=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?></a>)(.*?)</p>";
                                replacement = "$1" + _caput_alterada.caput[i] + "$2replaced_by=\"$3\"$4$5$6" + _caput_alterada.caput[i] + "$7$8</p>\r\n$1" + _caput_alterada.caput[i].Replace("_replaced", "") + "$2replaced_by_disabled=\"$3\"$4<a name=\"" + _caput_alterada.caput[i].Replace("_replaced", "") + "\"></a>" + _caput_alterada.texto_novo[i] + " <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                            }
                            break;
                        case "prorrogação":
                        case "ratificação":
                        case "regulamentação":
                        case "ressalva":
                        case "recepção":
                        case "legislação correlata":
                            ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + _caput_alteradora.ds_norma + ")";
                            if (_caput_alterada.nm_relacao_aux == "legislação correlata")
                            {
                                ds_link_alterador = "(Legislação correlata - " + _caput_alteradora.ds_norma + ")";
                            }
                            pattern = "(<p.+?linkname=\"" + _caput_alterada.caput[i] + "\".*?<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?)</p>";
                            replacement = "$1 <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                            break;
                        default:
                            //verifica primeiro quantas vezes o paragrafo já foi alterado
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(_replaced.*?\".*?>)(.*?)(<a.+?name=\"" + _caput_alterada.caput[i] + "_replaced.*?\".*?></a>)(.*?)</p>";
                            var matches = Regex.Matches(texto, pattern);
                            var iReplaceds = matches.Count;

                            //em seguida usa-se o pattern para pegar o paragrafo que está sendo alterado, e se estiver sendo alterado pela segunda vez já vai possuir class='link_vide'
                            //sendo assim deve-se manter os links do mesmo e inserir links novo no proximo vigente
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?>)(.*?)(<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>)(.*?)( <a class=\"link_vide\".*?</a>)</p>";
                            matches = Regex.Matches(texto, pattern);
                            if (matches.Count > 0)
                            {
                                ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + _caput_alteradora.ds_norma + ")";

                                var sReplaced = "_replaced";
                                for (var j = 0; j < iReplaceds; j++)
                                {
                                    sReplaced += "_replaced";
                                }

                                if (!string.IsNullOrEmpty(_caput_alterada.texto_novo[i]))
                                {
                                    replacement = matches[0].Groups[1].Value + _caput_alterada.caput[i] + sReplaced + "\" replaced_by=\"" + _caput_alteradora.ch_norma + matches[0].Groups[2].Value + "<s>" + matches[0].Groups[3].Value.Replace("<s>", "").Replace("</s>", "") + "<a name=\"" + _caput_alterada.caput[i] + sReplaced + "\"></a>" + matches[0].Groups[5].Value.Replace("<s>", "").Replace("</s>", "") + "</s>" + matches[0].Groups[6].Value + "</p>\r\n" + matches[0].Groups[1].Value + _caput_alterada.caput[i] + matches[0].Groups[2].Value + matches[0].Groups[3].Value.Replace("<s>", "").Replace("</s>", "") + matches[0].Groups[4].Value + _caput_alterada.texto_novo[i] + " <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                                }
                                else
                                {
                                    replacement = matches[0].Groups[1].Value + _caput_alterada.caput[i] + sReplaced + "\" replaced_by=\"" + _caput_alteradora.ch_norma + matches[0].Groups[2].Value + "<s>" + matches[0].Groups[3].Value.Replace("<s>", "").Replace("</s>", "") + "<a name=\"" + _caput_alterada.caput[i] + sReplaced + "\"></a>" + matches[0].Groups[5].Value.Replace("<s>", "").Replace("</s>", "") + "</s>" + matches[0].Groups[6].Value + " <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                                }
                            }
                            else
                            {
                                pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?>)(.*?)(<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>)(.*?)</p>";
                                matches = Regex.Matches(texto, pattern);
                                if (matches.Count == 1)
                                {
                                    ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + _caput_alteradora.ds_norma + ")";
                                    if (!string.IsNullOrEmpty(_caput_alterada.texto_novo[i]))
                                    {
                                        replacement = matches[0].Groups[1].Value + _caput_alterada.caput[i] + "_replaced\" replaced_by=\"" + _caput_alteradora.ch_norma + matches[0].Groups[2].Value + "<s>" + matches[0].Groups[3].Value.Replace("<s>", "").Replace("</s>", "") + "<a name=\"" + _caput_alterada.caput[i] + "_replaced\"></a>" + matches[0].Groups[5].Value.Replace("<s>", "").Replace("</s>", "") + "</s></p>\r\n" + matches[0].Groups[1].Value + _caput_alterada.caput[i] + matches[0].Groups[2].Value + matches[0].Groups[3].Value.Replace("<s>", "").Replace("</s>", "") + matches[0].Groups[4].Value + _caput_alterada.texto_novo[i] + " <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                                    }
                                    else
                                    {
                                        replacement = matches[0].Groups[1].Value + _caput_alterada.caput[i] + "_replaced\" replaced_by=\"" + _caput_alteradora.ch_norma + matches[0].Groups[2].Value + "<s>" + matches[0].Groups[3].Value.Replace("<s>", "").Replace("</s>", "") + "<a name=\"" + _caput_alterada.caput[i] + "_replaced\"></a>" + matches[0].Groups[5].Value.Replace("<s>", "").Replace("</s>", "") + "</s> <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                                    }
                                }
                            }
                            break;
                    }
                    if (Regex.Matches(texto, pattern).Count == 1)
                    {
                        texto = Regex.Replace(texto, pattern, replacement);
                        //Resolve os bugs de <s><s>....
                        texto = Regex.Replace(texto, "(<s>+)\\1+", "$1");
                        texto = Regex.Replace(texto, "(</s>+)\\1+", "$1");
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

        /// <summary>
        /// Recupera o texto da norma alterada e faz alterações nos seus dispositivos. Não possui link para para o dispositivo da norma alteradora porém aponta para o arquivo, ou detalhes, da norma alteradora
        /// </summary>
        /// <param name="_caput_alterada"></param>
        /// <param name="_caput_alteradora"></param>
        /// <returns></returns>
        public string AlterarDispositivosDaNormaAlterada(Caput _caput_alterada, NormaOV norma_alteradora)
        {
            var texto = "";
            if (_caput_alterada.caput.Length == _caput_alterada.texto_antigo.Length)
            {
                var htmlFile = new HtmlFileEncoded();
                texto = htmlFile.GetHtmlFile(_caput_alterada.id_file, "sinj_norma", null);
                texto = AlterarDispositivosDaNormaAlterada(texto, _caput_alterada, norma_alteradora);
            }
            return texto;
        }

        /// <summary>
        /// Faz a alterações nos dispositivos do texto da norma alterada e com link para o arquivo, ou detalhes, da norma alteradora
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="_caput_alterada"></param>
        /// <param name="_caput_alteradora"></param>
        /// <returns></returns>
        public string AlterarDispositivosDaNormaAlterada(string texto, Caput _caput_alterada, NormaOV norma_alteradora)
        {
            var name_file_norma_alteradora = norma_alteradora.getNameFileArquivoVigente();
            var ds_norma_alteradora = norma_alteradora.getDescricaoDaNorma();

            var bAlterou = false;

            var pattern = "";
            var replacement = "";
            var ds_link_alterador = "";
            //define o link da norma alteradora, se possui name_file_norma_alteradora então a norma tem arquivo se não então para o detalhes da norma
            var aux_href = !string.IsNullOrEmpty(name_file_norma_alteradora) ? ("(_link_sistema_)Norma/" + norma_alteradora.ch_norma + '/' + name_file_norma_alteradora) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + norma_alteradora.ch_norma;
            for (var i = 0; i < _caput_alterada.caput.Length; i++)
            {
                switch (_caput_alterada.nm_relacao_aux)
                {
                    case "acrescimo":
                        var texto_novo_splited = _caput_alterada.texto_novo[i].Split('\n');
                        pattern = "(<p.+?linkname=\")" + _caput_alterada.caput[i] + "(\".*?)(replaced_by=\".*?\")(.*?>)(.*?<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?</p>)";
                        replacement = "$1" + _caput_alterada.caput[i] + "$2$3$4$5";
                        //É necessário verificar a existencia do atributo replaced_by no paragrafo antes se aplicar o regex.
                        //Quando tem o parametro replaced_by tem que evitar acrescentá-lo nos paragrafos que estão sendo acrescidos.
                        if (Regex.Matches(texto, pattern).Count == 1)
                        {
                            for (var j = 0; j < texto_novo_splited.Length; j++)
                            {
                                ds_link_alterador = "(" + UtilVides.gerarDescricaoDoTexto(texto_novo_splited[j]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + ds_norma_alteradora + ")";
                                replacement += "\r\n$1" + _caput_alterada.caput[i] + "_add_" + j + "$2$4<a name=\"" + _caput_alterada.caput[i] + "_add_" + j + "\"></a>" + texto_novo_splited[j] + " <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a></p>";
                            }
                        }
                        else
                        {
                            pattern = "(<p.+?linkname=\")" + _caput_alterada.caput[i] + "(\".*?>)(.*?<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?</p>)";
                            replacement = "$1" + _caput_alterada.caput[i] + "$2$3";
                            for (var j = 0; j < texto_novo_splited.Length; j++)
                            {
                                ds_link_alterador = "(" + UtilVides.gerarDescricaoDoTexto(texto_novo_splited[j]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + ds_norma_alteradora + ")";
                                replacement += "\r\n$1" + _caput_alterada.caput[i] + "_add_" + j + "$2<a name=\"" + _caput_alterada.caput[i] + "_add_" + j + "\"></a>" + texto_novo_splited[j] + " <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a></p>";
                            }
                        }
                        
                        break;
                    case "renumeração":
                        ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + ds_norma_alteradora + ")";
                        pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?)<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?(<a class=\"link_vide\".+?</a>)</p>";
                        if (Regex.Matches(texto, pattern).Count == 1)
                        {
                            replacement = "$1" + _caput_alterada.caput[i] + "_renum$2<a name=\"" + _caput_alterada.caput[i] + "_renum\"></a>" + _caput_alterada.texto_novo[i] + " $3 <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a></p>";
                        }
                        else
                        {
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?)<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?</p>";
                            if (Regex.Matches(texto, pattern).Count == 1)
                            {
                                replacement = "$1" + _caput_alterada.caput[i] + "_renum$2<a name=\"" + _caput_alterada.caput[i] + "_renum\"></a>" + _caput_alterada.texto_novo[i] + " <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a></p>";
                            }
                        }
                        break;
                    case "revigoração":
                        ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i].Replace("_replaced", "")) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + ds_norma_alteradora + ")";
                        pattern = "(<p.+?linkname=\"" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "\".*?)replaced_by=\"(.*?)\"(.*?)<s>(.*?)</s>(.*?)</p>";
                        replacement = "$1replaced_by_disabled=\"$2\"$3$4$5 <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a></p>";
                        if (!string.IsNullOrEmpty(_caput_alterada.texto_novo[i]))
                        {
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?)replaced_by=\"(.*?)\"(.*?>)(.*?)(<a.+?name=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?></a>)(.*?)</p>";
                            replacement = "$1" + _caput_alterada.caput[i] + "$2replaced_by=\"$3\"$4$5$6" + _caput_alterada.caput[i] + "$7$8</p>\r\n$1" + _caput_alterada.caput[i].Replace("_replaced", "") + "$2replaced_by_disabled=\"$3\"$4<a name=\"" + _caput_alterada.caput[i].Replace("_replaced", "") + "\"></a>" + _caput_alterada.texto_novo[i] + " <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a></p>";
                        }
                        break;
                    case "prorrogação":
                    case "ratificação":
                    case "regulamentação":
                    case "ressalva":
                    case "recepção":
                    case "legislação correlata":
                        ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + ds_norma_alteradora + ")";
                        if (_caput_alterada.nm_relacao_aux == "legislação correlata")
                        {
                            ds_link_alterador = "(Legislação correlata - " + ds_norma_alteradora + ")";
                        }
                        pattern = "(<p.+?linkname=\"" + _caput_alterada.caput[i] + "\".*?<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?)</p>";
                        replacement = "$1 <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a></p>";
                        break;
                    default:
                        //verifica primeiro quantas vezes o paragrafo já foi alterado
                        pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(_replaced.*?\".*?>)(.*?)(<a.+?name=\"" + _caput_alterada.caput[i] + "_replaced.*?\".*?></a>)(.*?)</p>";
                        var matches = Regex.Matches(texto, pattern);
                        var iReplaceds = matches.Count;

                        //em seguida usa-se o pattern para pegar o paragrafo que está sendo alterado, e se estiver sendo alterado pela segunda vez já vai possuir class='link_vide'
                        //sendo assim deve-se manter os links do mesmo e inserir links novo no proximo vigente
                        pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?>)(.*?)(<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>)(.*?)( <a class=\"link_vide\".*?</a>)</p>";
                        matches = Regex.Matches(texto, pattern);
                        if (matches.Count > 0)
                        {
                            ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + ds_norma_alteradora + ")";

                            var sReplaced = "_replaced";
                            for (var j = 0; j < iReplaceds; j++)
                            {
                                sReplaced += "_replaced";
                            }
                            
                            if (!string.IsNullOrEmpty(_caput_alterada.texto_novo[i]))
                            {
                                replacement = matches[0].Groups[1].Value + _caput_alterada.caput[i] + sReplaced + "\" replaced_by=\"" + norma_alteradora.ch_norma + matches[0].Groups[2].Value + "<s>" + matches[0].Groups[3].Value.Replace("<s>", "").Replace("</s>", "") + "<a name=\"" + _caput_alterada.caput[i] + sReplaced + "\"></a>" + matches[0].Groups[5].Value.Replace("<s>", "").Replace("</s>", "") + "</s>" + matches[0].Groups[6].Value + "</p>\r\n" + matches[0].Groups[1].Value + _caput_alterada.caput[i] + matches[0].Groups[2].Value + matches[0].Groups[3].Value + matches[0].Groups[4].Value + _caput_alterada.texto_novo[i] + " <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a></p>";
                            }
                            else
                            {
                                replacement = matches[0].Groups[1].Value + _caput_alterada.caput[i] + sReplaced + "\" replaced_by=\"" + norma_alteradora.ch_norma + matches[0].Groups[2].Value + "<s>" + matches[0].Groups[3].Value.Replace("<s>", "").Replace("</s>", "") + "<a name=\"" + _caput_alterada.caput[i] + sReplaced + "\"></a>" + matches[0].Groups[5].Value.Replace("<s>", "").Replace("</s>", "") + "</s>" + matches[0].Groups[6].Value + " <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a></p>";
                            }
                        }
                        else
                        {
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?>)(.*?)(<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>)(.*?)</p>";
                            matches = Regex.Matches(texto, pattern);
                            if (matches.Count == 1)
                            {
                                ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + ds_norma_alteradora + ")";
                                if (!string.IsNullOrEmpty(_caput_alterada.texto_novo[i]))
                                {
                                    replacement = matches[0].Groups[1].Value + _caput_alterada.caput[i] + "_replaced\" replaced_by=\"" + norma_alteradora.ch_norma + matches[0].Groups[2].Value + "<s>" + matches[0].Groups[3].Value.Replace("<s>", "").Replace("</s>", "") + "<a name=\"" + _caput_alterada.caput[i] + "_replaced\"></a>" + matches[0].Groups[5].Value.Replace("<s>", "").Replace("</s>", "") + "</s></p>\r\n" + matches[0].Groups[1].Value + _caput_alterada.caput[i] + matches[0].Groups[2].Value + matches[0].Groups[3].Value.Replace("<s>", "").Replace("</s>", "") + matches[0].Groups[4].Value + _caput_alterada.texto_novo[i] + " <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a></p>";
                                }
                                else
                                {
                                    replacement = matches[0].Groups[1].Value + _caput_alterada.caput[i] + "_replaced\" replaced_by=\"" + norma_alteradora.ch_norma + matches[0].Groups[2].Value + "<s>" + matches[0].Groups[3].Value.Replace("<s>", "").Replace("</s>", "") + "<a name=\"" + _caput_alterada.caput[i] + "_replaced\"></a>" + matches[0].Groups[5].Value.Replace("<s>", "").Replace("</s>", "") + "</s> <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a></p>";
                                }
                            }
                        }
                        break;
                }
                if (Regex.Matches(texto, pattern).Count == 1)
                {
                    texto = Regex.Replace(texto, pattern, replacement);
                    //Resolve os bugs de <s><s>....
                    texto = Regex.Replace(texto, "(<s>+)\\1+", "$1");
                    texto = Regex.Replace(texto, "(</s>+)\\1+", "$1");
                    bAlterou = true;
                }
                if (!bAlterou)
                {
                    texto = "";
                }
            }
            return texto;
        }

        public string AlterarTextoCompletoDaNormaAlterada(NormaOV norma_alteradora, string id_file_norma_alterada, Caput _caput_alteradora)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = htmlFile.GetHtmlFile(id_file_norma_alterada, "sinj_norma", null);
            return AlterarTextoCompletoDaNormaAlterada(texto, norma_alteradora, _caput_alteradora);
        }

        public string AlterarTextoCompletoDaNormaAlterada(string texto, NormaOV norma_alteradora, Caput _caput_alteradora)
        {
            var htmlFile = new HtmlFileEncoded();
            
            var pattern1 = "(?!<p.+replaced_by=.+>)(<p.+?>)(.+?)</p>";
            Regex rx1 = new Regex(pattern1);

            var pattern2 = "(<h1.+?epigrafe=.+?>.+?</h1>)";
            Regex rx2 = new Regex(pattern2, RegexOptions.Singleline);

            if (rx1.Matches(texto).Count > 0 || rx2.Matches(texto).Count == 1)
            {
                var replacement1 = "$1<s>$2</s></p>";
                var replacement2 = "$1\r\n<p style=\"text-align:center;\"><a href=\"(_link_sistema_)Norma/" + norma_alteradora.ch_norma + "/" + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >(" + _caput_alteradora.ds_texto_para_alterador_aux + " pelo(a) " + _caput_alteradora.ds_norma + ")</a></p>";
                texto = rx1.Replace(texto, replacement1);
                texto = rx2.Replace(texto, replacement2);
            }
            else
            {
                texto = "";
            }
            return texto;
        }

        public string AlterarTextoCompletoDaNormaAlterada(NormaOV norma_alteradora, string id_file_norma_alterada, string ds_texto_alterador)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = htmlFile.GetHtmlFile(id_file_norma_alterada, "sinj_norma", null);
            return AlterarTextoCompletoDaNormaAlterada(texto, norma_alteradora, ds_texto_alterador);
        }

        public string AlterarTextoCompletoDaNormaAlterada(string texto, NormaOV norma_alteradora, string ds_texto_alterador)
        {
            var htmlFile = new HtmlFileEncoded();
            var pattern1 = "(?!<p.+replaced_by=.+>)(<p.+?>)(.+?)</p>";
            var replacement1 = "$1<s>$2</s></p>";

            var name_file_norma_alteradora = norma_alteradora.getNameFileArquivoVigente();
            var ds_norma_alteradora = norma_alteradora.getDescricaoDaNorma();

            var aux_href = !string.IsNullOrEmpty(name_file_norma_alteradora) ? ("(_link_sistema_)Norma/" + norma_alteradora.ch_norma + "/" + name_file_norma_alteradora) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + norma_alteradora.ch_norma;

            var pattern2 = "(<h1.+?epigrafe=.+?>.+?</h1>)";
            var replacement2 = "$1\r\n<p style=\"text-align:center;\"><a href=\"" + aux_href + "\" >(" + ds_texto_alterador + " pelo(a) " + ds_norma_alteradora + ")</a></p>";

            if (Regex.Matches(texto, pattern1).Count > 0 || Regex.Matches(texto, pattern2).Count == 1)
            {
                texto = Regex.Replace(texto, pattern1, replacement1);
                texto = Regex.Replace(texto, pattern2, replacement2);
            }
            else
            {
                texto = "";
            }

            return texto;
        }

        public string AcrescentarInformacaoNoTextoDaNormaAlterada(NormaOV norma_alteradora, string id_file_norma_alterada, Caput _caput_alteradora)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = htmlFile.GetHtmlFile(id_file_norma_alterada, "sinj_norma", null);
            return AcrescentarInformacaoNoTextoDaNormaAlterada(texto, norma_alteradora, _caput_alteradora);
        }

        public string AcrescentarInformacaoNoTextoDaNormaAlterada(string texto, NormaOV norma_alteradora, Caput _caput_alteradora)
        {
            var htmlFile = new HtmlFileEncoded();
            var pattern = "(<h1.+?epigrafe=.+?>.+?</h1>)";
            if (Regex.Matches(texto, pattern).Count == 1)
            {
                var replacement = "$1\r\n<p><a href=\"(_link_sistema_)Norma/" + norma_alteradora.ch_norma + "/" + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >(" + _caput_alteradora.ds_texto_para_alterador_aux + " pelo(a) " + _caput_alteradora.ds_norma + ")</a></p>";
                if (_caput_alteradora.ds_texto_para_alterador_aux == "legislação correlata")
                {
                    replacement = "<p><a href=\"(_link_sistema_)Norma/" + norma_alteradora.ch_norma + "/" + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >Legislação correlata - " + _caput_alteradora.ds_norma + "</a></p>\r\n$1";
                }
                texto = Regex.Replace(texto, pattern, replacement);
            }
            else
            {
                texto = "";
            }
            return texto;
        }

        public string AcrescentarInformacaoNoTextoDaNormaAlterada(NormaOV normaAlteradora, string idFileNormaAlterada, string dsTextoRelacao)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = htmlFile.GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);
            var pattern = "(<h1.+?epigrafe=.+?>.+?</h1>)";

            var nameFileNormaAlteradora = normaAlteradora.getNameFileArquivoVigente();
            var dsNormaAlteradora = normaAlteradora.getDescricaoDaNorma();

            var aux_href = !string.IsNullOrEmpty(nameFileNormaAlteradora) ? ("(_link_sistema_)Norma/" + normaAlteradora.ch_norma + "/" + nameFileNormaAlteradora) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + normaAlteradora.ch_norma;


            if (Regex.Matches(texto, pattern).Count == 1)
            {

                var replacement = "$1\r\n<p><a href=\"" + aux_href + "\" >(" + dsTextoRelacao + " pelo(a) " + dsNormaAlteradora + ")</a></p>";
                if (dsTextoRelacao == "legislação correlata")
                {
                    replacement = "<p><a href=\"" + aux_href + "\" >Legislação correlata - " + dsNormaAlteradora + "</a></p>\r\n$1";
                }
                texto = Regex.Replace(texto, pattern, replacement);
            }
            else
            {
                texto = "";
            }
            return texto;
        }

        public string AcrescentarInformacaoNoTextoDaNormaAlteradora(NormaOV normaAlterada, string idFileNormaAlteradora, string dsTextoRelacao)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = "";
            if (dsTextoRelacao == "legislação correlata")
            {
                texto = htmlFile.GetHtmlFile(idFileNormaAlteradora, "sinj_norma", null);
                var pattern = "(<h1.+?epigrafe=.+?>.+?</h1>)";

                var nameFileNormaAlterada = normaAlterada.getNameFileArquivoVigente();
                var dsNormaAlterada = normaAlterada.getDescricaoDaNorma();

                var aux_href = !string.IsNullOrEmpty(nameFileNormaAlterada) ? ("(_link_sistema_)Norma/" + normaAlterada.ch_norma + "/" + nameFileNormaAlterada) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + normaAlterada.ch_norma;

                if (Regex.Matches(texto, pattern).Count == 1)
                {
                    var replacement = "<p><a href=\"" + aux_href + "\" >Legislação correlata - " + dsNormaAlterada + "</a></p>\r\n$1";

                    texto = Regex.Replace(texto, pattern, replacement);
                }
                else
                {
                    texto = "";
                }
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

    public class UtilVides
    {
        public static string EscapeCharsInToPattern(string text)
        {
            var aChars = new string[] { "(", ")", "$", ".", "?", "=" };
            foreach(var aChar in aChars){
                text = text.Replace(aChar, "\\" + aChar);
            }
            return text;
        }

        public static string gerarDescricaoDoCaput(string _caput)
        {

            var caput_splited = _caput.Split('_');
            var last_caput = caput_splited.Last();
            if (last_caput.IndexOf("art") == 0)
            {
                _caput = "Artigo ";
            }
            else if (last_caput.IndexOf("par") == 0)
            {
                _caput = "Parágrafo ";
            }
            else if (last_caput.IndexOf("inc") == 0)
            {
                _caput = "Inciso ";
            }
            else if (last_caput.IndexOf("ali") == 0)
            {
                _caput = "Alínea ";
            }
            else if (last_caput.IndexOf("let") == 0)
            {
                _caput = "Alínea ";
            }
            else
            {
                _caput = "";
            }
            return _caput;
        }

        public static string gerarDescricaoDoTexto(string texto)
        {
            var caput = "";
            texto = texto.Trim().ToUpper();
            if (texto.IndexOf(' ') >= 0)
            {
                caput = texto.Substring(0, texto.IndexOf(' '));
            }
            if (caput == "ART" || caput == "ART.")
            {
                caput = "Artigo ";
            }
            else if (caput == "PARAGRAFO" || caput == "PARÁGRAFO" || caput == "§")
            {
                caput = "Parágrafo ";
            }
            else if (ehInciso(caput))
            {
                caput = "Inciso ";
            }
            else if (ehAlinea(caput))
            {
                caput = "Alínea ";
            }
            else
            {
                caput = "";
            }
            return caput;
        }

        public static string getRelacaoParaTextoAlterador(string relacao, bool regexExcluir=false)
        {
            string ds = relacao;

            var relacaoSplited = relacao.Split(' ');
            if (!(relacaoSplited[0].Equals("veto", StringComparison.InvariantCultureIgnoreCase) || relacaoSplited[0].Equals("texto", StringComparison.InvariantCultureIgnoreCase) || relacaoSplited[0].Equals("denominação", StringComparison.InvariantCultureIgnoreCase) || relacaoSplited[0].Equals("legislação", StringComparison.InvariantCultureIgnoreCase)))
            {
                if(relacaoSplited[0].EndsWith("o"))
                {
                    relacaoSplited[0] += regexExcluir ? ".*?" : "(a)";
                }
                else if(relacaoSplited[0].EndsWith("a"))
                {
                    relacaoSplited[0] += regexExcluir ? ".*?" : "(o)";
                }
                ds = string.Join<string>(" ", relacaoSplited);
            }

            return ds;
        }

        public static bool ehInciso(string termo)
        {
            var lastIndex = termo.IndexOf("-");
            if(lastIndex > 0){
                termo = termo.Substring(0,lastIndex);
            }
            return Regex.IsMatch(termo,"^[IVXLCDM]+$", RegexOptions.IgnoreCase);
        }

        public static bool ehAlinea(string termo)
        {
            var lastIndex = termo.IndexOf(")");
            if(lastIndex < 0){
                return false;
            }
            if(termo.Length == (lastIndex + 1)){
                termo = termo.Substring(0,lastIndex);
                return Regex.IsMatch(termo,"^[a-z]+$", RegexOptions.IgnoreCase);
            }
            return false;
        }

        public static bool ehNum(string termo)
        {
            var lastIndex = termo.IndexOf(".");
            if (lastIndex < 0)
            {
                return false;
            }
            if (termo.Length == (lastIndex + 1))
            {
                termo = termo.Substring(0, lastIndex);
                int iTermo;
                return int.TryParse(termo, out iTermo);
            }
            return false;
        }

        public static bool possuiDispositivo(Caput dispositivo)
        {
            return dispositivo != null && dispositivo.caput != null && dispositivo.caput.Length > 0;
        }

        public static bool ehDiferente(Caput a, Caput b)
        {
            var ehDiferente = false;
            if (a.caput.Length != b.caput.Length)
            {
                ehDiferente = true;
            }
            else if (a.link != b.link)
            {
                ehDiferente = true;
            }
            else if (a.nm_relacao_aux != b.nm_relacao_aux)
            {
                ehDiferente = true;
            }
            else if (a.texto_novo != null)
            {
                for (var i = 0; i < a.caput.Length; i++)
                {
                    if (a.caput[i] != b.caput[i] || a.texto_novo[i] != b.texto_novo[i])
                    {
                        ehDiferente = true;
                        break;
                    }
                }
            }
            else
            {
                for (var i = 0; i < a.caput.Length; i++)
                {
                    if (a.caput[i] != b.caput[i])
                    {
                        ehDiferente = true;
                        break;
                    }
                }
            }
            return ehDiferente;
        }

        /// <summary>
        /// Verifica, com base na situação e na descrição relação de vide, se o vide implica em alteração no texto completo da norma, e não somente em um dispositivo
        /// </summary>
        /// <param name="nmSituacaoAlterada"></param>
        /// <param name="dsTextoParaAlterador"></param>
        /// <returns></returns>
        public static bool EhAlteracaoCompleta(string nmSituacaoAlterada, string dsTextoParaAlterador)
        {
            return (nmSituacaoAlterada == "revogado" && dsTextoParaAlterador == "revogado") ||
                   (nmSituacaoAlterada == "anulado" && dsTextoParaAlterador == "anulado") ||
                   (nmSituacaoAlterada == "extinta" && dsTextoParaAlterador == "extinta") ||
                   (nmSituacaoAlterada == "inconstitucional" && dsTextoParaAlterador == "declarado inconstitucional") ||
                   (nmSituacaoAlterada == "inconstitucional" && dsTextoParaAlterador == "julgada procedente") ||
                   (nmSituacaoAlterada == "cancelada" && dsTextoParaAlterador == "cancelada") ||
                   (nmSituacaoAlterada == "suspenso" && dsTextoParaAlterador == "suspenso totalmente");
        }

        /// <summary>
        /// Verifica, com base na situação e na descrição relação de vide, se o vide implica somente em linkar as duas normas, não necessariamente precisa ser legislação correlata
        /// </summary>
        /// <param name="dsTextoParaAlterador"></param>
        /// <returns></returns>
        public static bool EhLegislacaoCorrelata(string dsTextoParaAlterador)
        {
            return dsTextoParaAlterador == "ratificado" ||
                   dsTextoParaAlterador == "reeditado" ||
                   dsTextoParaAlterador == "regulamentado" ||
                   dsTextoParaAlterador == "prorrogado" ||
                   dsTextoParaAlterador == "legislação correlata";
        }
    }
}