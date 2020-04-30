using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.AD;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;

namespace TCDF.Sinj.RN
{
    public class VocabularioRN
    {
        private VocabularioAD _vocabularioAd;

        public VocabularioRN()
        {
            _vocabularioAd = new VocabularioAD();
        }

        public Results<VocabularioOV> Consultar(Pesquisa query)
        {
            return _vocabularioAd.Consultar(query);
        }

        public VocabularioOV Doc(ulong id_doc)
        {
            var vocabularioOv = _vocabularioAd.Doc(id_doc);
            if (vocabularioOv == null)
            {
                throw new DocNotFoundException("Registro não Encontrado.");
            }
            return vocabularioOv;
        }

        public VocabularioOV Doc(string ch_termo)
        {
            return _vocabularioAd.Doc(ch_termo);
        }

        public string JsonReg(ulong id_doc)
        {
            return _vocabularioAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _vocabularioAd.JsonReg(query);
        }

        public string PathPut(ulong id_doc, string path, string value, string retorno)
        {
            return _vocabularioAd.PathPut(id_doc, path, value, retorno);
        }

        public ulong Incluir(VocabularioOV vocabularioOv)
        {
            OrgaoOV orgao = null;
            vocabularioOv.ch_termo = Guid.NewGuid().ToString("N");
            if (vocabularioOv.EhTipoAutoridade() && !string.IsNullOrEmpty(vocabularioOv.ch_orgao))
            {
                orgao = new OrgaoRN().Doc(vocabularioOv.ch_orgao);
                var ano_inicio_vigencia = "";
                var ano_fim_vigencia = "";
                if (!string.IsNullOrEmpty(orgao.dt_inicio_vigencia))
                {
                    ano_inicio_vigencia = orgao.dt_inicio_vigencia.Split('/')[2];
                }
                if (!string.IsNullOrEmpty(orgao.dt_fim_vigencia))
                {
                    ano_fim_vigencia = orgao.dt_fim_vigencia.Split('/')[2];
                }
                vocabularioOv.nm_termo = orgao.nm_orgao + " (" + orgao.sg_orgao + (!string.IsNullOrEmpty(ano_inicio_vigencia) ? " " + ano_inicio_vigencia + (!string.IsNullOrEmpty(ano_fim_vigencia) ? "-" + ano_fim_vigencia : "") : "") + ")";
            }
            Validar(vocabularioOv);
            if (!vocabularioOv.EhTipoEspecificador())
            {
                vocabularioOv.nm_termo = vocabularioOv.nm_termo.ToUpper();
            }
            else
            {
                vocabularioOv.nm_termo = vocabularioOv.nm_termo.ToLower();
            }
            vocabularioOv._metadata.id_doc = _vocabularioAd.Incluir(vocabularioOv);
            if (vocabularioOv.EhTipoAutoridade() && !string.IsNullOrEmpty(vocabularioOv.ch_orgao) && orgao != null)
            {
                var vocabularioOv_sg_orgao = new VocabularioOV
                {
                    ch_tipo_termo = vocabularioOv.ch_tipo_termo,
                    ch_orgao = vocabularioOv.ch_orgao,
                    ch_termo = Guid.NewGuid().ToString("N"),
                    nm_termo = orgao.sg_orgao,
                    ds_fontes_pesquisadas = vocabularioOv.ds_fontes_pesquisadas,
                    ds_nota_explicativa = vocabularioOv.ds_nota_explicativa,
                    ds_texto_fonte = vocabularioOv.ds_texto_fonte,
                    dt_cadastro = vocabularioOv.dt_cadastro,
                    nm_login_usuario_cadastro = vocabularioOv.nm_login_usuario_cadastro,
                    st_aprovado = vocabularioOv.st_aprovado,
                    st_ativo = vocabularioOv.st_ativo,
                    st_excluir = vocabularioOv.st_excluir
                };
                _vocabularioAd.Incluir(vocabularioOv_sg_orgao);
                vocabularioOv.termos_nao_autorizados.Add(new Vocabulario_TNA { ch_termo_nao_autorizado = vocabularioOv_sg_orgao.ch_termo, nm_termo_nao_autorizado = vocabularioOv_sg_orgao.nm_termo });
            }
            AtualizarRelacionamentos(vocabularioOv);
            return vocabularioOv._metadata.id_doc;
        }

        private void AtualizarRelacionamentos(VocabularioOV vocabularioOv)
        {
            AtualizarTermosGerais(vocabularioOv);
            AtualizarTermosEspecificos(vocabularioOv);
            AtualizarTermosRelacionados(vocabularioOv);
            AtualizarTermosNaoAutorizados(vocabularioOv);
            AtualizarTermoUse(vocabularioOv);
        }

        /// <summary>
        /// Atualiza o campo termos_especificos dos termos gerais do vocabulario
        /// </summary>
        /// <param name="vocabularioOv"></param>
        private void AtualizarTermosGerais(VocabularioOV vocabularioOv)
        {
            if (vocabularioOv.termos_gerais != null && vocabularioOv.termos_gerais.Count > 0)
            {
                var query = new Pesquisa();
                query.limit = null;
                foreach (var termo_geral in vocabularioOv.termos_gerais)
                {
                    query.literal += (!string.IsNullOrEmpty(query.literal) ? " or " : "") + string.Format("ch_termo='{0}'", termo_geral.ch_termo_geral);
                }
                var termos_result = Consultar(query);
                foreach (var termo_geral in termos_result.results)
                {
                    var termos_especificos = termo_geral.termos_especificos;
                    termos_especificos.RemoveAll(te => te.ch_termo_especifico == vocabularioOv.ch_termo);
                    termos_especificos.Add(new Vocabulario_TE { ch_termo_especifico = vocabularioOv.ch_termo, nm_termo_especifico = vocabularioOv.nm_termo });
                    _vocabularioAd.PathPut(termo_geral._metadata.id_doc, "termos_especificos", JSON.Serialize<List<Vocabulario_TE>>(termos_especificos), null);
                }
            }
        }

        /// <summary>
        /// Atualiza o campo termos_gerais dos termos especificos do vocabulario
        /// </summary>
        /// <param name="vocabularioOv"></param>
        private void AtualizarTermosEspecificos(VocabularioOV vocabularioOv)
        {
            if (vocabularioOv.termos_especificos != null && vocabularioOv.termos_especificos.Count > 0)
            {
                var query = new Pesquisa();
                query.limit = null;
                foreach (var termo_especifico in vocabularioOv.termos_especificos)
                {
                    query.literal += (!string.IsNullOrEmpty(query.literal) ? " or " : "") + string.Format("ch_termo='{0}'", termo_especifico.ch_termo_especifico);
                }
                var termos_result = Consultar(query);
                foreach (var termo_especifico in termos_result.results)
                {
                    var termos_gerais = termo_especifico.termos_gerais;
                    termos_gerais.RemoveAll(tg => tg.ch_termo_geral == vocabularioOv.ch_termo);
                    termos_gerais.Add(new Vocabulario_TG { ch_termo_geral = vocabularioOv.ch_termo, nm_termo_geral = vocabularioOv.nm_termo });
                    _vocabularioAd.PathPut(termo_especifico._metadata.id_doc, "termos_gerais", JSON.Serialize<List<Vocabulario_TG>>(termos_gerais), null);
                }
            }
        }

        /// <summary>
        /// Atualiza o campo termos_gerais dos termos especificos do vocabulario
        /// </summary>
        /// <param name="vocabularioOv"></param>
        private void AtualizarTermosRelacionados(VocabularioOV vocabularioOv)
        {
            if (vocabularioOv.termos_relacionados != null && vocabularioOv.termos_relacionados.Count > 0)
            {
                var query = new Pesquisa();
                query.limit = null;
                foreach (var termo_relacionado in vocabularioOv.termos_relacionados)
                {
                    query.literal += (!string.IsNullOrEmpty(query.literal) ? " or " : "") + string.Format("ch_termo='{0}'", termo_relacionado.ch_termo_relacionado);
                }
                var termos_result = Consultar(query);
                foreach (var termo_relacionado in termos_result.results)
                {
                    var termos_relacionados = termo_relacionado.termos_relacionados;
                    termos_relacionados.RemoveAll(tg => tg.ch_termo_relacionado == vocabularioOv.ch_termo);
                    termos_relacionados.Add(new Vocabulario_TR { ch_termo_relacionado = vocabularioOv.ch_termo, nm_termo_relacionado = vocabularioOv.nm_termo });
                    _vocabularioAd.PathPut(termo_relacionado._metadata.id_doc, "termos_relacionados", JSON.Serialize<List<Vocabulario_TR>>(termos_relacionados), null);
                }
            }
        }

        /// <summary>
        /// Atualiza o campo termos_gerais dos termos especificos do vocabulario
        /// </summary>
        /// <param name="vocabularioOv"></param>
        private void AtualizarTermosNaoAutorizados(VocabularioOV vocabularioOv)
        {
            if (vocabularioOv.termos_nao_autorizados != null && vocabularioOv.termos_nao_autorizados.Count > 0)
            {
                var query = new Pesquisa();
                query.limit = null;
                foreach (var termo_nao_autorizado in vocabularioOv.termos_nao_autorizados)
                {
                    query.literal += (!string.IsNullOrEmpty(query.literal) ? " or " : "") + string.Format("ch_termo='{0}'", termo_nao_autorizado.ch_termo_nao_autorizado);
                }
                var termos_result = Consultar(query);
                var termos_nao_autorizados_aux = new List<Vocabulario_TNA>();
                foreach (var termo_nao_autorizado in termos_result.results)
                {
                    var termos_nao_autorizados = termo_nao_autorizado.termos_nao_autorizados;
                    if (termos_nao_autorizados != null && termos_nao_autorizados.Count > 0)
                    {
                        termos_nao_autorizados_aux.AddRange(termos_nao_autorizados);
                        foreach (var termo_nao_autorizado_do_termo_nao_autorizado in termos_nao_autorizados)
                        {
                            var termo = Doc(termo_nao_autorizado_do_termo_nao_autorizado.ch_termo_nao_autorizado);
                            termo.in_nao_autorizado = true;
                            termo.ch_termo_use = vocabularioOv.ch_termo;
                            termo.nm_termo_use = vocabularioOv.nm_termo;
                            _vocabularioAd.Atualizar(termo._metadata.id_doc, termo);
                        }
                    }
                    termo_nao_autorizado.in_nao_autorizado = true;
                    termo_nao_autorizado.ch_termo_use = vocabularioOv.ch_termo;
                    termo_nao_autorizado.nm_termo_use = vocabularioOv.nm_termo;
                    termo_nao_autorizado.termos_nao_autorizados = new List<Vocabulario_TNA>();
                    _vocabularioAd.Atualizar(termo_nao_autorizado._metadata.id_doc, termo_nao_autorizado);
                }
                foreach (var termo_nao_autorizado in termos_nao_autorizados_aux)
                {
                    if (vocabularioOv.termos_nao_autorizados.IndexOf(termo_nao_autorizado) < 0)
                    {
                        vocabularioOv.termos_nao_autorizados.Add(termo_nao_autorizado);
                    }
                }
                _vocabularioAd.PathPut(vocabularioOv._metadata.id_doc, "termos_nao_autorizados", JSON.Serialize<List<Vocabulario_TNA>>(vocabularioOv.termos_nao_autorizados), null);
            }
        }

        private void AtualizarTermoUse(VocabularioOV vocabularioOv)
        {
            if (!string.IsNullOrEmpty(vocabularioOv.ch_termo_use))
            {
                var termo = Doc(vocabularioOv.ch_termo_use);
                termo.termos_nao_autorizados.Add(new Vocabulario_TNA { ch_termo_nao_autorizado = vocabularioOv.ch_termo, nm_termo_nao_autorizado = vocabularioOv.nm_termo });
                PathPut(termo._metadata.id_doc, "termos_nao_autorizados", JSON.Serialize<List<Vocabulario_TNA>>(termo.termos_nao_autorizados), null);
            }
        }

        public VocabularioTroca TrocarTermo(VocabularioOV termo_antigo, VocabularioOV termo_novo)
        {
            VocabularioTroca vocabularioTroca = new VocabularioTroca();
            if (Atualizar(termo_novo._metadata.id_doc, termo_novo))
            {
                vocabularioTroca = TrocarTermoNasNormas(termo_antigo.ch_termo, termo_novo);
                vocabularioTroca.bAtualizado = true;

                vocabularioTroca.nm_termo_trocado = termo_antigo.nm_termo;
                if (vocabularioTroca.id_docs_erro.Count <= 0)
                {
                    vocabularioTroca.bExcluido = Excluir(termo_antigo._metadata.id_doc, true);
                }
            }
            return vocabularioTroca;
        }

        public VocabularioTroca TrocarTermoNasNormas(string ch_termo, VocabularioOV termo_novo)
        {
            VocabularioTroca vocabularioTroca = new VocabularioTroca();
            var query_normas = new Pesquisa();
            query_normas.limit = null;
            query_normas.literal = string.Format("'{0}'=any(ch_termo)", ch_termo);
            var normaRn = new NormaRN();
            var normas = normaRn.Consultar(query_normas);
            List<Indexacao> indexacoes;
            List<Vocabulario> vocabulario;
            vocabularioTroca.total_de_normas = normas.result_count;
            foreach (var norma in normas.results)
            {
                indexacoes = new List<Indexacao>();
                foreach (var indexacao in norma.indexacoes)
                {
                    vocabulario = new List<Vocabulario>();
                    foreach (var termo in indexacao.vocabulario)
                    {
                        if (termo.ch_termo != ch_termo)
                        {
                            vocabulario.Add(termo);
                        }
                        else
                        {
                            vocabulario.Add(new Vocabulario { ch_termo = termo_novo.ch_termo, ch_tipo_termo = termo_novo.ch_tipo_termo, nm_termo = termo_novo.nm_termo });
                        }
                    }
                    indexacoes.Add(new Indexacao { vocabulario = vocabulario });
                }
                if (normaRn.PathPut(norma._metadata.id_doc, "indexacoes", JSON.Serialize<List<Indexacao>>(indexacoes), null) == "UPDATED")
                {
                    vocabularioTroca.id_docs_sucesso.Add(norma._metadata.id_doc);
                }
                else
                {
                    vocabularioTroca.id_docs_erro.Add(norma._metadata.id_doc);
                }

            }
            return vocabularioTroca;
        }

        public bool Atualizar(ulong id_doc, VocabularioOV vocabularioOv)
        {
            Validar(vocabularioOv);
            if (!vocabularioOv.EhTipoEspecificador())
            {
                vocabularioOv.nm_termo = vocabularioOv.nm_termo.ToUpper();
            }
            else
            {
                vocabularioOv.nm_termo = vocabularioOv.nm_termo.ToLower();
            }
            //Excluo nos relacionamentos antes de atualizá-lo assim eu ainda sei quem são os termos que o possuem em seus relacionamentos;
            ExcluirNosRelacionamentos(id_doc);
            //Após ter excluido o termo nos relacionamentos dos outros termos eu atualizo termo e seus novos relacionamentos
            var sucesso = _vocabularioAd.Atualizar(id_doc, vocabularioOv);
            AtualizarRelacionamentos(vocabularioOv);
            return sucesso;
        }

        /// <summary>
        /// Exclue o termo da base de vocabulario
        /// </summary>
        /// <param name="id_doc">id_doc do termo</param>
        /// <param name="force">true: força a exclusão limpando o termo em todas as dependencias. false: Valida se o termo possui dependencias.</param>
        /// <returns></returns>
        public bool Excluir(ulong id_doc, bool force = false)
        {
            var vocabularioOv = Doc(id_doc);
            if (!force)
            {
                ValidarDepencias(vocabularioOv);
            }
            else
            {
                ExcluirNosRelacionamentos(id_doc);
                var query_normas = new Pesquisa();
                query_normas.limit = null;
                query_normas.literal = string.Format("'{0}'=any(ch_termo)", vocabularioOv.ch_termo);
                var normaRn = new NormaRN();
                var normas = normaRn.Consultar(query_normas);
                List<Indexacao> indexacoes;
                foreach (var norma in normas.results)
                {
                    indexacoes = new List<Indexacao>();
                    foreach (var indexacao in norma.indexacoes)
                    {
                        indexacao.vocabulario.RemoveAll(t => t.ch_termo == vocabularioOv.ch_termo);
                        indexacoes.Add(new Indexacao { vocabulario = indexacao.vocabulario });
                    }
                    normaRn.PathPut(norma._metadata.id_doc, "indexacoes", JSON.Serialize<List<Indexacao>>(indexacoes), null);
                }
            }
            return _vocabularioAd.Excluir(id_doc);
        }

        /// <summary>
        /// Recupera o termo na base e remove ele de todos os outros termos que o possuem em seus relacionamentos (TG, TE, TR, TNA, USE).
        /// </summary>
        /// <param name="id_doc"></param>
        private void ExcluirNosRelacionamentos(ulong id_doc)
        {
            VocabularioOV vocabularioOv = Doc(id_doc);
            VocabularioOV termo;
            foreach (var termo_geral in vocabularioOv.termos_gerais)
            {
                termo = Doc(termo_geral.ch_termo_geral);
                termo.termos_especificos.RemoveAll(te => te.ch_termo_especifico == vocabularioOv.ch_termo);
                PathPut(termo._metadata.id_doc, "termos_especificos", JSON.Serialize<List<Vocabulario_TE>>(termo.termos_especificos), null);
            }
            foreach (var termo_especifico in vocabularioOv.termos_especificos)
            {
                termo = Doc(termo_especifico.ch_termo_especifico);
                termo.termos_gerais.RemoveAll(tg => tg.ch_termo_geral == vocabularioOv.ch_termo);
                PathPut(termo._metadata.id_doc, "termos_gerais", JSON.Serialize<List<Vocabulario_TG>>(termo.termos_gerais), null);
            }
            foreach (var termos_relacionados in vocabularioOv.termos_relacionados)
            {
                termo = Doc(termos_relacionados.ch_termo_relacionado);
                termo.termos_relacionados.RemoveAll(tr => tr.ch_termo_relacionado == vocabularioOv.ch_termo);
                PathPut(termo._metadata.id_doc, "termos_relacionados", JSON.Serialize<List<Vocabulario_TR>>(termo.termos_relacionados), null);
            }
            foreach (var termos_nao_autorizados in vocabularioOv.termos_nao_autorizados)
            {
                termo = Doc(termos_nao_autorizados.ch_termo_nao_autorizado);
                termo.ch_termo_use = "";
                termo.nm_termo_use = "";
                termo.in_nao_autorizado = false;
                _vocabularioAd.Atualizar(termo._metadata.id_doc, termo);
            }
            if (!string.IsNullOrEmpty(vocabularioOv.ch_termo_use))
            {
                termo = Doc(vocabularioOv.ch_termo_use);
                termo.termos_nao_autorizados.RemoveAll(tna => tna.ch_termo_nao_autorizado == vocabularioOv.ch_termo);
                PathPut(termo._metadata.id_doc, "termos_nao_autorizados", JSON.Serialize<List<Vocabulario_TNA>>(termo.termos_nao_autorizados), null);
            }
        }

        public List<RelacionamentoDeVocabulario> PreencherRelacionamentoDeVocabulario(string[] termos)
        {
            List<RelacionamentoDeVocabulario> relacionamentoDeVocabulario = new List<RelacionamentoDeVocabulario>();
            if (termos != null && termos.Length > 0)
            {
                var termo = new string[0];
                var chave = "";
                var nome = "";
                for (var i = 0; i < termos.Length; i++)
                {
                    termo = termos[i].Split('#');
                    chave = termo[0];
                    nome = termo[1];
                    relacionamentoDeVocabulario.Add(new RelacionamentoDeVocabulario { chave = chave, nome = nome });
                }
            }
            return relacionamentoDeVocabulario;
        }

        public void ValidarDepencias(VocabularioOV vocabularioOv)
        {
            //ToDo:Validar se está sendo usada por alguma norma e se tem depencias com outros termos (TG, TR e TNA).
            if (vocabularioOv.termos_gerais != null && vocabularioOv.termos_gerais.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Termo possui termos gerais.");
            }
            if (vocabularioOv.termos_especificos != null && vocabularioOv.termos_especificos.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Termo possui termos específicos.");
            }
            if (vocabularioOv.termos_relacionados != null && vocabularioOv.termos_relacionados.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Termo possui termos relacionados.");
            }
            if (vocabularioOv.termos_nao_autorizados != null && vocabularioOv.termos_nao_autorizados.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Termo possui termos não autorizados.");
            }
            if (!string.IsNullOrEmpty(vocabularioOv.ch_termo_use))
            {
                throw new DocDependenciesException("Erro de dependência. Pertence à lista de termos não autorizados de outro termo.");
            }
            if (vocabularioOv.EhTipoLista())
            {
                var query = new Pesquisa();
                query.literal = string.Format("ch_lista_superior='{0}'", vocabularioOv.ch_termo);
                var termos = Consultar(query);
                if (termos.results.Count > 0)
                {
                    throw new DocDependenciesException("Erro de dependência. O Termo é uma lista que possui itens e/ou sublistas.");
                }
            }
            var normas_do_termo = new NormaRN().BuscarNormasDoTermo(vocabularioOv.ch_termo);
            if (normas_do_termo.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O termo está sendo usado por uma ou mais normas.");
            }
        }

        private void Validar(VocabularioOV vocabularioOv)
        {
            if (string.IsNullOrEmpty(vocabularioOv.nm_termo))
            {
                throw new DocValidacaoException("Nome inválido.");
            }
            ValidarDuplicidade(vocabularioOv);
        }

        private void ValidarDuplicidade(VocabularioOV vocabularioOv)
        {
            var ids = new List<ulong>();
            var result = Consultar(new Pesquisa { limit = null, literal = "UPPER(nm_termo)='" + vocabularioOv.nm_termo.ToUpper() + "' AND st_restaurado='false'", select = new string[] { "ch_termo", "nm_termo", "ch_tipo_termo", "_metadata" } });
            if (result.result_count > 0)
            {
                if (vocabularioOv.EhTipoEspecificador())
                {
                    var teste = result.results.Where<VocabularioOV>(v => v.EhTipoEspecificador() && v.ch_termo != vocabularioOv.ch_termo);
                    if (teste.Count() > 0)
                    {
                        foreach (var vocabulario in teste)
                        {
                            ids.Add(vocabulario._metadata.id_doc);
                        }
                    }
                }
                else
                {
                    var teste = result.results.Where<VocabularioOV>(v => !v.EhTipoEspecificador() && v.ch_termo != vocabularioOv.ch_termo);
                    if (teste.Count() > 0)
                    {
                        foreach (var vocabulario in teste)
                        {
                            ids.Add(vocabulario._metadata.id_doc);
                        }
                    }
                }
            }
            if (ids.Count > 0)
            {
                var dic = new Dictionary<string, object>();
                dic.Add("ids", ids);
                try
                {
                    throw new DocDuplicateKeyException("Termo com nome duplicado.");
                }
                catch (DocDuplicateKeyException ex)
                {
                    ex.Data.Add("ids", ids);
                    throw;
                }
            }
        }
    }
}
