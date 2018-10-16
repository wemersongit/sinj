using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;
using neo.BRLightREST;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for VocabularioTrocar
    /// </summary>
    public class VocabularioTrocar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            List<VocabularioOV> termos_antigos = null;
            List<VocabularioTroca> termos_trocados = new List<VocabularioTroca>();
            ulong total_de_normas_usando_o_termo_novo = 0;
            string ch_termo_novo_apos_troca = "";
            VocabularioOV termo_novo = null;
            var _id_doc_termo_antigo = context.Request["id_doc_termo_antigo"];
            var _ch_termo_antigo_restaurados = context.Request.Form.GetValues("ch_termo_antigo_restaurados");
            ulong id_doc_termo_antigo = 0;
            var _id_doc_termo_novo = context.Request["id_doc_termo_novo"];
            ulong id_doc_termo_novo = 0;
            var _nm_termo_novo = context.Request["nm_termo_novo"];
            var _ch_tipo_termo_novo = context.Request["ch_tipo_termo_novo"];
            var action = AcoesDoUsuario.voc_ger;
            var vocabularioRn = new VocabularioRN();
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);

                var _ds_nota_explicativa = context.Request["ds_nota_explicativa"];
                var _ds_fontes_pesquisadas = context.Request["ds_fontes_pesquisadas"];
                var _ds_texto_fonte = context.Request["ds_texto_fonte"];

                var _termos_gerais = new string[0];
                var _termos_especificos = new string[0];
                var _termos_nao_autorizados = new string[0];
                var _termos_relacionados = new string[0];


                var _termos_gerais_antigo = context.Request.Form.GetValues("termos_gerais_antigo");
                if (_termos_gerais_antigo == null)_termos_gerais_antigo=new string[0];
                var _termos_especificos_antigo = context.Request.Form.GetValues("termos_especificos_antigo");
                if (_termos_especificos_antigo == null) _termos_especificos_antigo = new string[0];
                var _termos_nao_autorizados_antigo = context.Request.Form.GetValues("termos_nao_autorizados_antigo");
                if (_termos_nao_autorizados_antigo == null) _termos_nao_autorizados_antigo = new string[0];
                var _termos_relacionados_antigo = context.Request.Form.GetValues("termos_relacionados_antigo");
                if (_termos_relacionados_antigo == null) _termos_relacionados_antigo = new string[0];

                var _termos_gerais_novo = context.Request.Form.GetValues("termos_gerais_novo");
                if (_termos_gerais_novo == null) _termos_gerais_novo = new string[0];
                var _termos_especificos_novo = context.Request.Form.GetValues("termos_especificos_novo");
                if (_termos_especificos_novo == null) _termos_especificos_novo = new string[0];
                var _termos_nao_autorizados_novo = context.Request.Form.GetValues("termos_nao_autorizados_novo");
                if (_termos_nao_autorizados_novo == null) _termos_nao_autorizados_novo = new string[0];
                var _termos_relacionados_novo = context.Request.Form.GetValues("termos_relacionados_novo");
                if (_termos_relacionados_novo == null) _termos_relacionados_novo = new string[0];

                _termos_gerais = new string[_termos_gerais_antigo.Length + _termos_gerais_novo.Length];
                Array.Copy(_termos_gerais_antigo, _termos_gerais, _termos_gerais_antigo.Length);
                Array.Copy(_termos_gerais_novo, 0, _termos_gerais, _termos_gerais_antigo.Length, _termos_gerais_novo.Length);

                _termos_especificos = new string[_termos_especificos_antigo.Length + _termos_especificos_novo.Length];
                Array.Copy(_termos_especificos_antigo, _termos_especificos, _termos_especificos_antigo.Length);
                Array.Copy(_termos_especificos_novo, 0, _termos_especificos, _termos_especificos_antigo.Length, _termos_especificos_novo.Length);

                _termos_nao_autorizados = new string[_termos_nao_autorizados_antigo.Length + _termos_nao_autorizados_novo.Length];
                Array.Copy(_termos_nao_autorizados_antigo, _termos_nao_autorizados, _termos_nao_autorizados_antigo.Length);
                Array.Copy(_termos_nao_autorizados_novo, 0, _termos_nao_autorizados, _termos_nao_autorizados_antigo.Length, _termos_nao_autorizados_novo.Length);

                _termos_relacionados = new string[_termos_relacionados_antigo.Length + _termos_relacionados_novo.Length];
                Array.Copy(_termos_relacionados_antigo, _termos_relacionados, _termos_relacionados_antigo.Length);
                Array.Copy(_termos_relacionados_novo, 0, _termos_relacionados, _termos_relacionados_antigo.Length, _termos_relacionados_novo.Length);

                if (!string.IsNullOrEmpty(_id_doc_termo_antigo) && ulong.TryParse(_id_doc_termo_antigo, out id_doc_termo_antigo))
                {
                    termos_antigos = new List<VocabularioOV>();
                    termos_antigos.Add(vocabularioRn.Doc(id_doc_termo_antigo));
                }
                else if (_ch_termo_antigo_restaurados != null && _ch_termo_antigo_restaurados.Length > 0)
                {
                    Pesquisa pesquisa = new Pesquisa();
                    pesquisa.literal = "";
                    foreach(var chave in _ch_termo_antigo_restaurados){
                        pesquisa.literal += (pesquisa.literal != "" ? " OR " : "") + "ch_termo='"+chave+"'";
                    }
                    termos_antigos = vocabularioRn.Consultar(pesquisa).results;
                }

                if (termos_antigos != null && termos_antigos.Count > 0)
                {
                    foreach (var termo_antigo in termos_antigos)
                    {
                        VocabularioTroca vocabularioTroca = new VocabularioTroca();
                        vocabularioTroca.nm_termo_trocado = termo_antigo.nm_termo;
                        try
                        {
                            //Note: Quando for troca entre tipos de termos tem que verificar as dependências
                            if (_ch_tipo_termo_novo != termo_antigo.ch_tipo_termo)
                            {
                                if (_termos_gerais_antigo.Length > 0)
                                {
                                    throw new DocValidacaoException("A troca entre tipos diferentes não pôde ser realizada porque há dependência de TERMOS GERAIS.");
                                }
                                if (_termos_especificos_antigo.Length > 0)
                                {
                                    throw new DocValidacaoException("A troca entre tipos diferentes não pôde ser realizada porque há dependência de TERMOS ESPECÍFICOS.");
                                }
                                if (_termos_relacionados_antigo.Length > 0)
                                {
                                    throw new DocValidacaoException("A troca entre tipos diferentes não pôde ser realizada porque há dependência de TERMOS RELACIONADOS.");
                                }
                                if (_termos_nao_autorizados_antigo.Length > 0)
                                {
                                    throw new DocValidacaoException("A troca entre tipos diferentes não pôde ser realizada porque há dependência de TERMOS NÃO AUTORIZADOS.");
                                }
                                if (!string.IsNullOrEmpty(termo_antigo.ch_termo_use))
                                {
                                    throw new DocValidacaoException("A troca entre tipos diferentes não pôde ser realizada porque o termo trocado é NÃO USE de outro termo.");
                                }
                                if (_ch_tipo_termo_novo == "ES")
                                {
                                    var normas_que_usam_o_termo = new NormaRN().Consultar(new Pesquisa { limit = null, literal = "'" + termo_antigo.ch_termo + "'=any(ch_termo)", select = new string[] { "id_doc", "indexacoes" } });
                                    if (normas_que_usam_o_termo.results.Count<NormaOV>(n => n.indexacoes.Count<Indexacao>(i => i.vocabulario[0].ch_termo == termo_antigo.ch_termo) > 0) > 0)
                                    {
                                        throw new DocValidacaoException("A troca entre tipos diferentes não pôde ser realizada porque não é permitido especificadores iniciando indexação em norma.");
                                    }
                                }
                                if (_ch_tipo_termo_novo == "LA")
                                {
                                    if (_termos_gerais.Length > 0)
                                    {
                                        throw new DocValidacaoException("A troca entre tipos diferentes não pôde ser realizada porque não é permitido LISTA com TERMOS GERAIS.");
                                    }
                                    if (_termos_especificos.Length > 0)
                                    {
                                        throw new DocValidacaoException("A troca entre tipos diferentes não pôde ser realizada porque não é permitido LISTA com TERMOS ESPECÍFICOS.");
                                    }
                                    if (_termos_relacionados.Length > 0)
                                    {
                                        throw new DocValidacaoException("A troca entre tipos diferentes não pôde ser realizada porque não é permitido LISTA com TERMOS RELACIONADOS.");
                                    }
                                }
                                else if (termo_antigo.in_lista)
                                {
                                    var itens_testar = new VocabularioRN().Consultar(new Pesquisa { literal = "ch_lista_superior='" + termo_antigo.ch_termo + "'", select = new string[] { "id_doc" } });
                                    if (itens_testar.results.Count > 0)
                                    {
                                        throw new DocValidacaoException("Os ITENS do Termo '" + termo_antigo.nm_termo + "' não podem ser atribuídos para o Termo '" + _nm_termo_novo + "' porque ele não é LISTA.");
                                    }
                                }
                            }
                            var aTermos_nao_autorizados = new List<Vocabulario_TNA>();
                            var aTermos_gerais = new List<Vocabulario_TG>();
                            var aTermos_especificos = new List<Vocabulario_TE>();
                            var aTermos_relacionados = new List<Vocabulario_TR>();
                            var termos_nao_autorizados = vocabularioRn.PreencherRelacionamentoDeVocabulario(_termos_nao_autorizados);
                            foreach (var termo_nao_autorizado in termos_nao_autorizados)
                            {
                                aTermos_nao_autorizados.Add(new Vocabulario_TNA { ch_termo_nao_autorizado = termo_nao_autorizado.chave, nm_termo_nao_autorizado = termo_nao_autorizado.nome });
                            }
                            var termos_gerais = vocabularioRn.PreencherRelacionamentoDeVocabulario(_termos_gerais);
                            foreach (var termo_geral in termos_gerais)
                            {
                                aTermos_gerais.Add(new Vocabulario_TG { ch_termo_geral = termo_geral.chave, nm_termo_geral = termo_geral.nome });
                            }
                            var termos_especificos = vocabularioRn.PreencherRelacionamentoDeVocabulario(_termos_especificos);
                            foreach (var termo_especifico in termos_especificos)
                            {
                                aTermos_especificos.Add(new Vocabulario_TE { ch_termo_especifico = termo_especifico.chave, nm_termo_especifico = termo_especifico.nome });
                            }
                            var termos_relacionados = vocabularioRn.PreencherRelacionamentoDeVocabulario(_termos_relacionados);
                            foreach (var termo_relacionado in termos_relacionados)
                            {
                                aTermos_relacionados.Add(new Vocabulario_TR { ch_termo_relacionado = termo_relacionado.chave, nm_termo_relacionado = termo_relacionado.nome });
                            }

                            List<VocabularioOV> itens = new List<VocabularioOV>();

                            if (!string.IsNullOrEmpty(_id_doc_termo_novo) && ulong.TryParse(_id_doc_termo_novo, out id_doc_termo_novo))
                            {
                                termo_novo = vocabularioRn.Doc(id_doc_termo_novo);
                                ch_termo_novo_apos_troca = termo_novo.ch_termo;
                                termo_novo.ds_nota_explicativa = _ds_nota_explicativa;
                                termo_novo.ds_fontes_pesquisadas = _ds_fontes_pesquisadas;
                                termo_novo.ds_texto_fonte = _ds_texto_fonte;
                                if (termo_novo.EhTipoDescritor())
                                {
                                    termo_novo.termos_gerais = aTermos_gerais;
                                    termo_novo.termos_especificos = aTermos_especificos;
                                    termo_novo.termos_relacionados = aTermos_relacionados;
                                }
                                else if (termo_novo.EhTipoLista())
                                {
                                    if (termo_novo.in_lista)
                                    {
                                        itens.AddRange(new VocabularioRN().Consultar(new Pesquisa { limit = null, literal = "ch_lista_superior='" + termo_novo.ch_termo + "'" }).results);
                                    }
                                    itens.AddRange(new VocabularioRN().Consultar(new Pesquisa { limit = null, literal = "ch_lista_superior='" + termo_antigo.ch_termo + "'" }).results);
                                    termo_novo.in_lista = termo_novo.in_lista || itens.Count > 0;
                                }

                                termo_novo.termos_nao_autorizados = aTermos_nao_autorizados;

                                if (termo_novo.alteracoes == null)
                                {
                                    termo_novo.alteracoes = new List<AlteracaoOV>();
                                }
                                termo_novo.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });

                                vocabularioTroca = vocabularioRn.TrocarTermo(termo_antigo, termo_novo);
                                if (itens.Count > 0)
                                {
                                    foreach (var item in itens)
                                    {
                                        try
                                        {
                                            item.ch_lista_superior = termo_novo.ch_termo;
                                            item.nm_lista_superior = termo_novo.nm_termo;
                                            vocabularioRn.Atualizar(item._metadata.id_doc, item);
                                        }
                                        catch
                                        {
                                            //ToDo:implementar algo aqui para tratar esses erros....
                                        }
                                    }
                                }
                            }
                            else if (!string.IsNullOrEmpty(_nm_termo_novo))
                            {
                                ch_termo_novo_apos_troca = termo_antigo.ch_termo;
                                var nome_antigo = termo_antigo.nm_termo;
                                termo_antigo.nm_termo = _nm_termo_novo;
                                termo_antigo.ds_nota_explicativa = _ds_nota_explicativa;
                                termo_antigo.ds_fontes_pesquisadas = _ds_fontes_pesquisadas;
                                termo_antigo.ds_texto_fonte = _ds_texto_fonte;
                                if (termo_antigo.EhTipoDescritor())
                                {
                                    termo_antigo.termos_gerais = aTermos_gerais;
                                    termo_antigo.termos_especificos = aTermos_especificos;
                                    termo_antigo.termos_relacionados = aTermos_relacionados;
                                }
                                else if (termo_antigo.in_lista)
                                {
                                    itens.AddRange(new VocabularioRN().Consultar(new Pesquisa { limit = null, literal = "ch_lista_superior='" + termo_antigo.ch_termo + "'" }).results);
                                }
                                termo_antigo.termos_nao_autorizados = aTermos_nao_autorizados;
                                if (termo_antigo.alteracoes == null)
                                {
                                    termo_antigo.alteracoes = new List<AlteracaoOV>();
                                }
                                termo_antigo.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                                if (vocabularioRn.Atualizar(termo_antigo._metadata.id_doc, termo_antigo))
                                {
                                    if (!nome_antigo.Equals(_nm_termo_novo, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        vocabularioTroca = vocabularioRn.TrocarTermoNasNormas(termo_antigo.ch_termo, termo_antigo);
                                        if (itens.Count > 0)
                                        {
                                            foreach (var item in itens)
                                            {
                                                try
                                                {
                                                    item.nm_lista_superior = termo_novo.nm_termo;
                                                    vocabularioRn.PathPut(item._metadata.id_doc, "nm_lista_superior", item.nm_lista_superior, null);
                                                }
                                                catch
                                                {
                                                    //ToDo:implementar algo aqui para tratar esses erros....
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("Erro ao atualizar registro. id_doc:" + id_doc_termo_novo);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            vocabularioTroca.error_message = ex.Message;
                        }

                        termos_trocados.Add(vocabularioTroca);
                    }
                    try
                    {
                        Pesquisa pesquisa_normas = new Pesquisa();
                        pesquisa_normas.literal = "'" + ch_termo_novo_apos_troca + "'=any(ch_termo)";
                        pesquisa_normas.select = new string[0];
                        total_de_normas_usando_o_termo_novo = new NormaRN().Consultar(pesquisa_normas).result_count;

                    }
                    catch
                    {

                    }
                    sRetorno = "{\"id_doc_success\":" + id_doc_termo_antigo + ",\"update\":true, \"termos_trocados\":" + JSON.Serialize<List<VocabularioTroca>>(termos_trocados) + ", \"total_de_normas_usando_o_termo_novo\":"+total_de_normas_usando_o_termo_novo+"}";
                }
                else
                {
                    throw new Exception("Erro ao atualizar registro. id_doc_termo_antigo:" + _id_doc_termo_antigo + ". ch_termos_antigos:" + (_ch_termo_antigo_restaurados != null ? String.Join(",", _ch_termo_antigo_restaurados) : ""));
                }
                var log_atualizar = new LogAlterar<VocabularioOV>
                {
                    id_doc = id_doc_termo_novo,
                    registro = termo_novo
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_atualizar, id_doc_termo_novo, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                //Note:Desabilitei o log nos termos_antigos para pensar em como gravar essa operação feota em multiplos documentos em um unico request
                //if (termos_antigos != null & termos_antigos.Count > 0)
                //{
                //    var log_excluir = new LogExcluir<VocabularioOV>
                //    {
                //        id_doc = termos_antigos.Select<VocabularioOV, ulong>(v => v._metadata.id_doc);
                //    };
                //    LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_excluir, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                //}
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
