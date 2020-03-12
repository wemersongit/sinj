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
using System.Globalization;
using Newtonsoft.Json;
using TCDF.Sinj.Web;

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
            var _dt_controle_alteracao = context.Request["dt_controle_alteracao"];
            var _vide = context.Request["vide"];
            var vide = JsonConvert.DeserializeObject<VideInclusao>(_vide);

            var _ch_tipo_norma_vide_fora_do_sistema = context.Request["ch_tipo_norma_vide_fora_do_sistema"];
            var _nm_tipo_norma_vide_fora_do_sistema = context.Request["nm_tipo_norma_vide_fora_do_sistema"];
            var _nr_norma_vide_fora_do_sistema = context.Request["nr_norma_vide_fora_do_sistema"];
            var _ch_tipo_fonte_vide_fora_do_sistema = context.Request["ch_tipo_fonte_vide_fora_do_sistema"];
            var _nm_tipo_fonte_vide_fora_do_sistema = context.Request["nm_tipo_fonte_vide_fora_do_sistema"];
            var _dt_publicacao_norma_vide_fora_do_sistema = context.Request["dt_publicacao_norma_vide_fora_do_sistema"];
            var _nr_pagina_publicacao_norma_vide_fora_do_sistema = context.Request["nr_pagina_publicacao_norma_vide_fora_do_sistema"];
            var _nr_coluna_publicacao_norma_vide_fora_do_sistema = context.Request["nr_coluna_publicacao_norma_vide_fora_do_sistema"];

            NormaOV normaAlteradoraOv = null;
            NormaOV normaAlteradaOv = null;
            ulong id_doc = 0;

            var action = AcoesDoUsuario.nor_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);

                vide.Validate();

                var normaRn = new NormaRN();
                var dDt_controle_alteracao = Convert.ToDateTime(_dt_controle_alteracao);

                normaAlteradoraOv = normaRn.Doc(vide.NormaAlteradora.ChNorma);
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

                var vide_alterador = new Vide();
                var ch_vide = Guid.NewGuid().ToString("N");
                ///Esse vide será armazenado no documento da norma alteradora, logo devo referenciar qual a norma alterada.
                ///Assim como também devo colocar aqui todas informações que devem aparecer nos detalhes do documento.
                ///Exemplo: Revoga paragrafo tal da norma tal...
                vide_alterador.ch_vide = ch_vide;
                vide_alterador.in_relacao_de_acao = normaAlteradoraOv.st_acao;
                vide_alterador.ch_norma_vide = vide.NormaAlterada.ChNorma;

                vide_alterador.ch_tipo_relacao = vide.Relacao.ch_tipo_relacao;
                vide_alterador.nm_tipo_relacao = vide.Relacao.nm_tipo_relacao;
                vide_alterador.ds_texto_relacao = vide.Relacao.ds_texto_para_alterado; //Exemplo: Revoga Totalmente

                vide_alterador.in_norma_afetada = false;
                vide_alterador.in_relacao_de_acao = false;

                if (vide.NormaAlterada.InNormaForaSistema)
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
                else if (vide.NormaAlterada != null && !string.IsNullOrEmpty(vide.NormaAlterada.ChNorma))
                {
                    normaAlteradaOv = normaRn.Doc(vide.NormaAlterada.ChNorma);
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
                    vide_alterador.alteracao_texto_vide.dispositivos_norma_vide_outra = vide.NormaAlterada.Dispositivos;
                }
                vide_alterador.alteracao_texto_vide.dispositivos_norma_vide = vide.NormaAlteradora.Dispositivos;

                normaAlteradoraOv.vides.Add(vide_alterador);

                normaRn.SalvarTextoAntigoDaNorma(normaAlteradoraOv, vide_alterador, sessao_usuario.nm_login_usuario);

                normaAlteradoraOv.ar_atualizado = vide.NormaAlteradora.ArquivoNovo;

                var dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                normaAlteradoraOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });

                if (normaRn.Atualizar(normaAlteradoraOv._metadata.id_doc, normaAlteradoraOv))
                {
                    if (normaAlteradaOv != null)
                    {
                        var vide_alterada = new Vide();
                        vide_alterada.ch_vide = ch_vide;
                        vide_alterada.in_relacao_de_acao = normaAlteradoraOv.st_acao;

                        vide_alterada.ch_tipo_relacao = vide.Relacao.ch_tipo_relacao;
                        vide_alterada.nm_tipo_relacao = vide.Relacao.nm_tipo_relacao;
                        vide_alterada.ds_texto_relacao = vide.Relacao.ds_texto_para_alterador; //Exemplo: Revogado Totalmente

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
                        if (normaAlteradoraOv.st_vacatio_legis && !string.IsNullOrEmpty(normaAlteradoraOv.dt_inicio_vigencia))
                        {
                            vide_alterada.dt_inicio_vigencia_norma_vide = normaAlteradoraOv.dt_inicio_vigencia;
                        }

                        vide_alterada.alteracao_texto_vide.dispositivos_norma_vide = vide.NormaAlterada.Dispositivos;

                        vide_alterada.alteracao_texto_vide.dispositivos_norma_vide_outra = vide.NormaAlteradora.Dispositivos;

                        vide_alterada.ds_comentario_vide = vide.DsComentarioVide;

                        normaAlteradaOv.vides.Add(vide_alterada);
                        //Coloca o nome na situação
                        if (!normaAlteradaOv.st_situacao_forcada)
                        {
                            var situacaoRn = new SituacaoRN();
                            var situacaoOv = new SituacaoOV { ch_situacao = normaAlteradaOv.ch_situacao, nm_situacao = normaAlteradaOv.nm_situacao };
                            var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                            //Cod inserido para trocar o nome da situação caso for revigorado ele voltara a ser "sem revogação expressa"
                            situacaoOv = situacaoRn.Doc("semrevogacaoexpressa");

                            if (vide.Relacao.nm_tipo_relacao.Equals("REVIGORAÇÃO", StringComparison.CurrentCultureIgnoreCase))
                            {
                                normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                                normaAlteradaOv.nm_situacao = situacaoOv.nm_situacao;
                            }
                            else
                            {
                                normaAlteradaOv.nm_situacao = situacao.nm_situacao;
                            }

                            if (vide.Relacao.nm_tipo_relacao.Equals("INCONSTITUCIONALIDADE", StringComparison.CurrentCultureIgnoreCase))
                            {
                                normaAlteradaOv.nm_situacao = "Inconstitucional";
                            }

                            //Cod inserido para adicionar a descrição de suspenso totalmente by wemerson
                            if (vide.Relacao.nm_tipo_relacao.Equals("SUSPENSÃO TOTAL", StringComparison.CurrentCultureIgnoreCase) || vide.Relacao.nm_tipo_relacao.Equals("SUSPENSÃO", StringComparison.CurrentCultureIgnoreCase))
                            {
                                normaAlteradaOv.nm_situacao = "Suspenso";
                            }
                        }

                        normaRn.SalvarTextoAntigoDaNorma(normaAlteradaOv, vide_alterada, sessao_usuario.nm_login_usuario);

                        normaAlteradaOv.ar_atualizado = vide.NormaAlterada.ArquivoNovo;
                        normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                        if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                        {
                            sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\"" + normaAlteradoraOv.ch_norma + "\", \"dt_controle_alteracao\":\"" + DateTime.Now.AddSeconds(1).ToString("dd'/'MM'/'yyyy HH:mm:ss") + "\"}";
                            //if (!normaAlteradoraOv.st_vacatio_legis || string.IsNullOrEmpty(normaAlteradoraOv.dt_inicio_vigencia) || Convert.ToDateTime(normaAlteradoraOv.dt_inicio_vigencia, new CultureInfo("pt-BR")) <= DateTime.Now)
                            //{
                            //    normaRn.VerificarDispositivosESalvarOsTextosAntigosDasNormas(normaAlteradoraOv, normaAlteradaOv, vide_alterador, vide_alterada, sessao_usuario.nm_login_usuario);
                            //    normaRn.VerificarDispositivosEAlterarOsTextosDasNormas(normaAlteradoraOv, normaAlteradaOv, vide_alterador, vide_alterada);
                            //}
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class VideInclusao
    {
        [JsonProperty("relacao")]
        public TipoDeRelacao Relacao { get; set; }
        [JsonProperty("norma_alteradora")]
        public NormaVideInclusao NormaAlteradora { get; set; }
        [JsonProperty("norma_alterada")]
        public NormaVideInclusao NormaAlterada { get; set; }
        [JsonProperty("ds_comentario_vide")]
        public string DsComentarioVide { get; set; }

        public void Validate()
        {
            if (Relacao == null || string.IsNullOrEmpty(Relacao.ch_tipo_relacao))
            {
                throw new DocValidacaoException("Tipo de Relação não informado. Verifique se o Tipo de Relação está selecionado.");
            }
            if (NormaAlteradora == null || string.IsNullOrEmpty(NormaAlteradora.ChNorma))
            {
                throw new DocValidacaoException("Erro na norma informada.");
            }
            if (!Relacao.ch_tipo_relacao.Equals("9") && (NormaAlteradora.Dispositivos == null || !NormaAlteradora.Dispositivos.Any()))
            {
                throw new DocValidacaoException("Erro ao informar o dispositivo alterador.");
            }
            if (NormaAlterada == null || (!NormaAlterada.InNormaForaSistema && string.IsNullOrEmpty(NormaAlterada.ChNorma)))
            {
                throw new DocValidacaoException("Erro ao informar a norma alterada. Verifique se está selecionando corretamente a norma afetada pelo vide.");
            }
            if (!NormaAlterada.InAlteracaoCompleta && !Relacao.ch_tipo_relacao.Equals("9") && (NormaAlterada.Dispositivos == null || !NormaAlterada.Dispositivos.Any()))
            {
                throw new DocValidacaoException("Erro ao informar o dispositivo alterado.");
            }
        }
    }

    public class NormaVideInclusao
    {
        [JsonProperty("ch_norma")]
        public string ChNorma { get; set; }
        [JsonProperty("in_norma_fora_do_sistema")]
        public bool InNormaForaSistema { get; set; }
        [JsonProperty("in_alteracao_completa")]
        public bool InAlteracaoCompleta { get; set; }
        [JsonProperty("ds_norma")]
        public string DsNorma { get; set; }
        [JsonProperty("dt_assinatura")]
        public string DtAssinatura { get; set; }
        [JsonProperty("nm_tipo_norma")]
        public string NmTipoNorma { get; set; }
        [JsonProperty("arquivo_novo")]
        public ArquivoOV ArquivoNovo { get; set; }
        public List<DispositivoVide> Dispositivos { get; set; }

    }
}
