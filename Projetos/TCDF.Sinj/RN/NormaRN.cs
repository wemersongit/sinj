using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;
using util.BRLight;
using TCDF.Sinj.ES;
using System.Web;

namespace TCDF.Sinj.RN
{
    public class NormaRN
    {
        private NormaAD _normaAd;

        public NormaRN()
        {
            _normaAd = new NormaAD();
        }

        public Results<NormaOV> Consultar(Pesquisa query)
        {
            return _normaAd.Consultar(query);
        }

        public NormaOV Doc(ulong id_doc)
        {
            return _normaAd.Doc(id_doc);
        }

        public NormaOV Doc(string ch_norma)
        {
            return _normaAd.Doc(ch_norma);
        }

        public string JsonReg(Pesquisa query)
        {
            return _normaAd.JsonReg(query);
        }

        public string JsonReg(ulong id_doc)
        {
            return _normaAd.JsonReg(id_doc);
        }

        public string PathPut(ulong id_doc, string path, string value, string retorno)
        {
            return _normaAd.PathPut(id_doc, path, value, retorno);
        }

        public string PathPut<T>(Pesquisa pesquisa, List<opMode<T>> listopMode)
        {
            return _normaAd.PathPut<T>(pesquisa, listopMode);
        }

        public ulong Incluir(NormaOV normaOv)
        {
            ValidarGerarChaveEAuxDeRankeamentoDaNorma(normaOv);

            if (!normaOv.st_situacao_forcada)
            {
                DefinirSituacaoDaNorma(normaOv, true);
            }
            
            //GerarRankeamentoDeNorma(normaOv);
            
            normaOv.ch_norma = Guid.NewGuid().ToString("N");
			normaOv.st_nova = true;
            return _normaAd.Incluir(normaOv);
        }

        public bool Excluir(NormaOV normaOv)
        {
            if (RemoverVides(normaOv))
            {
                return _normaAd.Excluir(normaOv._metadata.id_doc);
            }
            else return false;
        }

        private void ValidarGerarChaveEAuxDeRankeamentoDaNorma(NormaOV normaOv)
        {
            var tipoDeNorma = new TipoDeNormaRN().Doc(normaOv.ch_tipo_norma);
            if (string.IsNullOrEmpty(normaOv.ch_tipo_norma) || string.IsNullOrEmpty(normaOv.nm_tipo_norma))
            {
				throw new DocValidacaoException("Tipo de Norma não informado.");
            }
            if (normaOv.origens == null || normaOv.origens.Count <= 0)
            {
				throw new DocValidacaoException("Origem não informada.");
            }
            if (string.IsNullOrEmpty(normaOv.dt_assinatura))
            {
				throw new DocValidacaoException("Data de assinatura não informada.");
            }
            if (normaOv.id_ambito <= 0 || string.IsNullOrEmpty(normaOv.nm_ambito))
            {
				throw new DocValidacaoException("Âmbito invállido.");
            }
            if (!tipoDeNorma.in_conjunta && normaOv.origens.Count > 1)
            {
                throw new DocValidacaoException("A norma deve conter apenas uma Origem.");
            }
            List<string> chaves = new List<string>();
            StringBuilder chave = new StringBuilder();
            if (normaOv.nr_norma.Equals("0"))
            {
                normaOv.nr_norma = "";
            }
            chave.Append(normaOv.ch_tipo_norma + "#");
            chave.Append(normaOv.nr_norma + "#");

            if (!tipoDeNorma.in_numeracao_por_orgao)
            {
                chave.Append(normaOv.cr_norma + "#");
                chave.Append(normaOv.nr_sequencial);
                chaves.Add(chave.ToString());
            }
            else
            {
                var ano = "0000";
                // Se a norma for sem número, deve ser usada a data de assinatura completa e não somente o ano
                if (string.IsNullOrEmpty(normaOv.nr_norma))
                {
                    ano = normaOv.dt_assinatura;
                }
                else
                {
                    var dt_assinatura_split = normaOv.dt_assinatura.Split('/');

                    if (dt_assinatura_split.Length == 3)
                    {
                        ano = dt_assinatura_split[2];
                    }
                }
                chave.Append(ano + "#");
                chave.Append(normaOv.cr_norma + "#");
                chave.Append(normaOv.nr_sequencial + "#");
                foreach (var orgao in normaOv.origens)
                {
                    chaves.Add(orgao.ch_orgao + "|" + chave.ToString());
                }
            }
            //normaOv.ch_para_nao_duplicacao = GerarChaveParaNaoDuplicacaoDaNorma(normaOv.ch_tipo_norma, normaOv.nr_norma, normaOv.nr_sequencial, normaOv.cr_norma, normaOv.dt_assinatura, normaOv.origens.Select<Orgao, string>(o => o.ch_orgao).ToArray<string>());
            normaOv.ch_para_nao_duplicacao = chaves;
            GerarRankeamentoDeNorma(normaOv, tipoDeNorma);
        }
		
		public bool Atualizar(ulong id_doc, NormaOV normaOv)
        {
            ValidarGerarChaveEAuxDeRankeamentoDaNorma(normaOv);
            if (!normaOv.st_situacao_forcada)
            {
                DefinirSituacaoDaNorma(normaOv, false);
            }
			normaOv.st_atualizada = true;

            return _normaAd.Atualizar(id_doc, normaOv);
        }

        public List<NormaOV> BuscarNormasDoTermo(string ch_termo)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("'{0}'=any(ch_termo)", ch_termo);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDoOrgao(string ch_orgao)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("'{0}'=any(ch_orgao)", ch_orgao);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDaAutoria(string ch_autoria)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("'{0}'=any(ch_autoria)", ch_autoria);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDoInteressado(string ch_interessado)
        { 
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("'{0}'=any(ch_interessado)", ch_interessado);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDoProcurador(string ch_procurador)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("'{0}'=any(ch_procurador)", ch_procurador);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDoRelator(string ch_relator)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("'{0}'=any(ch_relator)", ch_relator);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDoRequerente(string ch_requerente)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("'{0}'=any(ch_requerente)", ch_requerente);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDoRequerido(string ch_requerido)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("'{0}'=any(ch_requerido)", ch_requerido);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDoTipoDeFonte(string ch_tipo_fonte)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("'{0}'=any(ch_tipo_fonte)", ch_tipo_fonte);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDoTipoDeNorma(string ch_tipo_norma)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("ch_tipo_norma='{0}'", ch_tipo_norma);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDoTipoDePublicacao(string ch_tipo_publicacao)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("'{0}'=any(ch_tipo_publicacao)", ch_tipo_publicacao);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDoTipoDeRelacao(string ch_tipo_relacao)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("'{0}'=any(ch_tipo_relacao)", ch_tipo_relacao);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDoUsuario(string nm_login_usuario)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("nm_login_usuario_cadastro='{0}' or '{0}'=any(nm_login_usuario_alteracao)", nm_login_usuario);
            return Consultar(query).results;
        }

        public List<NormaOV> BuscarNormasDaSituacao(string ch_situacao)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("ch_situacao='{0}'", ch_situacao);
            return Consultar(query).results;
        }

        public void DefinirSituacaoDaNorma(NormaOV normaOv, bool ehNova)
        {
            var tipodenorma = new TipoDeNormaRN().Doc(normaOv.ch_tipo_norma);
            var situacaoRn = new SituacaoRN();
            var situacaoOv = new SituacaoOV { ch_situacao = normaOv.ch_situacao, nm_situacao = normaOv.nm_situacao };
            if (tipodenorma.in_g2)
            {
                if (normaOv.decisoes != null && normaOv.decisoes.Count > 0)
                {
                    //var decisoes_ordenadas_por_data = normaOv.decisoes;
                    var decisoes_ordenadas_por_data = new List<Decisao>();

                    foreach (var decisao in normaOv.decisoes)
                    {
                        if(!string.IsNullOrEmpty(decisao.dt_decisao)){
                            decisoes_ordenadas_por_data.Add(decisao);
                        }
                    }
                    if (decisoes_ordenadas_por_data.Count > 0)
                    {
                        decisoes_ordenadas_por_data = decisoes_ordenadas_por_data.OrderBy(d => Convert.ToDateTime(d.dt_decisao)).ToList();
                    }
                    else
                    {
                        decisoes_ordenadas_por_data = normaOv.decisoes;
                    }
                    switch (decisoes_ordenadas_por_data.Last<Decisao>().in_decisao)
                    {
                        case TipoDeDecisaoEnum.LiminarDeferida:
                            situacaoOv = situacaoRn.Doc("aguardandojulgamentolnardeferida");
                            break;
                        case TipoDeDecisaoEnum.LiminarIndeferida:
                            situacaoOv = situacaoRn.Doc("aguardandojulgamentolnarindeferida");
                            break;
                        case TipoDeDecisaoEnum.MeritoImprocedente:
                            situacaoOv = situacaoRn.Doc("julgadoimprocedente");
                            break;
                        case TipoDeDecisaoEnum.MeritoProcedente:
                            situacaoOv = situacaoRn.Doc("julgadoprocedente");
                            break;
                        case TipoDeDecisaoEnum.Extinta:
                            situacaoOv = situacaoRn.Doc("extinta");
                            break;
                        case TipoDeDecisaoEnum.Ajuizado:
                            situacaoOv = situacaoRn.Doc("aguardandojulgamentoajuizado");
                            break;
                        default:
                            situacaoOv = situacaoRn.Doc("aguardandojulgamento");
                            break;
                    }
                }
            }
            else
            {
                //Se for uma norma nova sua situação inicial é sem revogação expressa
                if (ehNova)
                {
                    situacaoOv = situacaoRn.Doc("semrevogacaoexpressa");
                }
            }
            normaOv.ch_situacao = situacaoOv.ch_situacao;
            normaOv.nm_situacao = situacaoOv.nm_situacao;
        }

        private void GerarRankeamentoDeNorma(NormaOV normaOv, TipoDeNormaOV tipoDeNorma)
        {
            var ano = "";
            var nr_norma = "";
            var dt_assinatura = "";
            normaOv.rankeamentos = new List<string>();
            normaOv.rankeamentos.Add(normaOv.nm_tipo_norma);

            if (!string.IsNullOrEmpty(normaOv.nr_norma))
            {
                nr_norma = normaOv.nr_norma;
                normaOv.rankeamentos.Add(normaOv.nr_norma);
                int testeNr = 0;
                if (int.TryParse(normaOv.nr_norma, out testeNr) && nr_norma.Length > 3)
                {
                    normaOv.rankeamentos.Add(normaOv.nr_norma.Insert(nr_norma.Length - 3 , "."));
                }
            }
            if (!string.IsNullOrEmpty(normaOv.dt_assinatura))
            {
                dt_assinatura = normaOv.dt_assinatura;
                var dt_splited = normaOv.dt_assinatura.Split('/');
                if (dt_splited.Length == 3 && dt_splited[2].Length == 4)
                {
                    ano = dt_splited[2];
                    normaOv.rankeamentos.Add(ano);
                }
                normaOv.rankeamentos.Add(normaOv.dt_assinatura);
            }
            foreach (var origem in normaOv.origens)
            {
                normaOv.rankeamentos.Add(origem.sg_orgao);
                normaOv.rankeamentos.Add(origem.nm_orgao);
            }
            foreach (var sg_tipo_norma in tipoDeNorma.sgs_tipo_norma)
            {
                normaOv.rankeamentos.Add(sg_tipo_norma);
                if (!string.IsNullOrEmpty(nr_norma))
                {
                    normaOv.rankeamentos.Add(sg_tipo_norma + " " + nr_norma);
                }
                normaOv.rankeamentos.Add(sg_tipo_norma + " " + nr_norma + " " + dt_assinatura);
                if (!string.IsNullOrEmpty(ano))
                {
                    normaOv.rankeamentos.Add(sg_tipo_norma + " " + nr_norma + " " + ano);
                    normaOv.rankeamentos.Add(sg_tipo_norma + " " + nr_norma + " " + ano.Substring(2));
                }
            }
        }

        public List<string> GerarChaveParaNaoDuplicacaoDaNorma(string ch_tipo_norma, string nr_norma, int nr_sequencial, string cr_norma, string dt_assinatura, string[] chs_orgao)
        {
            List<string> chaves = new List<string>();
            StringBuilder chave = new StringBuilder();

            chave.Append(ch_tipo_norma + "#");
            chave.Append(nr_norma + "#");

            var tipodenorma = new TipoDeNormaRN().Doc(ch_tipo_norma);
            if (!tipodenorma.in_numeracao_por_orgao)
            {
                chave.Append(cr_norma + "#");
                chave.Append(nr_sequencial);
                chaves.Add(chave.ToString());
            }
            else
            {
                var ano = "0000";
                // Se a norma for sem número, deve ser usada a data de assinatura completa e não somente o ano
                if (string.IsNullOrEmpty(nr_norma) || nr_norma == "0")
                {
                    ano = dt_assinatura;
                }
                else
                {
                    var dt_assinatura_split = dt_assinatura.Split('/');

                    if (dt_assinatura_split.Length == 3)
                    {
                        ano = dt_assinatura_split[2];
                    }
                }
                chave.Append(ano + "#");
                chave.Append(cr_norma + "#");
                chave.Append(nr_sequencial + "#");
                foreach (var ch_orgao in chs_orgao)
                {
                    chaves.Add(ch_orgao + "|" + chave.ToString());
                }
            }
            return chaves;
        }

        public TipoDeRelacaoOV ObterRelacaoParcial(string ch_relacao)
        {
            TipoDeRelacaoOV relacao = null;
            switch (ch_relacao)
            {
                case "13":
                    relacao = new TipoDeRelacaoRN().Doc("14");
                    break;
                case "18":
                    relacao = new TipoDeRelacaoRN().Doc("19");
                    break;
                case "21":
                    relacao = new TipoDeRelacaoRN().Doc("22");
                    break;
                default:
                    relacao = new TipoDeRelacaoRN().Doc(ch_relacao);
                    break;
            }
            return relacao;
        }

        public TipoDeRelacaoOV ObterRelacao(string ch_relacao)
        {
            return new TipoDeRelacaoRN().Doc(ch_relacao);
        }

        public SituacaoOV ObterSituacao(List<Vide> vides)
        {
            var pesquisa = new Pesquisa { limit = null };
            var relacoes = new TipoDeRelacaoRN().Consultar(pesquisa);
            var situacoes = new SituacaoRN().Consultar(pesquisa);
            int importanciaFinal = Int32.MaxValue;
            bool existeRevigoracao = false;
            bool ajuizado = false;
            var vides_que_afetam_a_norma = vides.Where<Vide>(v => v.in_norma_afetada).ToList<Vide>();

            var i_revigoracao = 0;
            foreach (var vide in vides_que_afetam_a_norma)
            {
                var relacao = relacoes.results.Where<TipoDeRelacaoOV>(tdr => tdr.ch_tipo_relacao == vide.ch_tipo_relacao).First<TipoDeRelacaoOV>();
                int importanciaDaVez = relacao.nr_importancia;

                // Liminar Deferida   12(*8)
                if (relacao.nm_tipo_relacao.ToLower() == "liminar deferida")
                {
                    if (!EhModificacaoTotal(vide))
                    {
                        importanciaDaVez = 8;
                    }
                    else if (importanciaDaVez > importanciaFinal && ajuizado)
                    {
                        importanciaFinal = importanciaDaVez;
                        ajuizado = false;
                    }
                }
                // Inconstitucional 6(*8)
                if (relacao.nm_tipo_relacao.ToLower() == "inconstitucionalidade")
                {
                    if (!EhModificacaoTotal(vide))
                    {
                        importanciaDaVez = 8;
                    }
                }
                // Julgada Procedente			6(*8)
                if (relacao.nm_tipo_relacao.ToLower() == "julgada procedente")
                {
                    if (!EhModificacaoTotal(vide))
                    {
                        importanciaDaVez = 8;
                    }
                }
                // SUSTAÇÃO TOTAL               3 (*8)
                if (relacao.nm_tipo_relacao.ToLower() == "sustação total")
                {
                    if (!EhModificacaoTotal(vide))
                    {
                        importanciaDaVez = 8;
                    }
                }
                // ANULAÇÃO		    		    4 (*8)
                if (relacao.nm_tipo_relacao.ToLower() == "anulação")
                {
                    if (!EhModificacaoTotal(vide))
                    {
                        importanciaDaVez = 8;
                    }
                }

                // REVOGAÇÃO		            2 (*8)
                // REVOGAÇÃO TOTAL				2 (*8)
                //Caso seja uma revogação ou revogação total
                if (relacao.nm_tipo_relacao.ToLower() == "revogação" || relacao.nm_tipo_relacao.ToLower() == "revogação total")
                {
                    //Caso seja uma revogação parcial a importancia deve ser 8.
                    if (EhModificacaoTotal(vide))
                    {
                        var chegou_na_posicao_atual = false;
                        for (var i = i_revigoracao; i < vides_que_afetam_a_norma.Count; i++)
                        {
                            if (vide.ch_vide == vides_que_afetam_a_norma[i].ch_vide)
                            {
                                chegou_na_posicao_atual = true;
                                continue;
                            }
                            if(chegou_na_posicao_atual){
                                var relacao_revigoracao = relacoes.results.Where<TipoDeRelacaoOV>(tdr => tdr.ch_tipo_relacao == vides_que_afetam_a_norma[i].ch_tipo_relacao).First<TipoDeRelacaoOV>();
                                if ((relacao_revigoracao.nm_tipo_relacao.ToLower() == "revigoração" || relacao_revigoracao.nm_tipo_relacao.ToLower() == "revigoração total"))
                                {
                                    existeRevigoracao = true;
                                    break;
                                }
                            }
                        }
                        if (existeRevigoracao)
                        {
                            existeRevigoracao = false;
                            importanciaDaVez = 8;
                        }
                    }
                    else
                    {
                        importanciaDaVez = 8;
                    }
                }

                //if ((relacao.nm_tipo_relacao.ToLower() == "revigoração" || relacao.nm_tipo_relacao.ToLower() == "revigoração total") && EhModificacaoTotal(vide))
                //{
                //    existeRevigoracao = true;
                //}

                if (importanciaDaVez < importanciaFinal)
                {
                    importanciaFinal = importanciaDaVez;
                    //ajuda a modificar de ajuizado para suspenso quando entre os vides há um ajuizado seguido de um liminar deferida
                    if (relacao.nm_tipo_relacao.ToLower() == "ajuizado")
                    {
                        ajuizado = true;
                    }
                    else
                    {
                        ajuizado = false;
                    }
                }
                i_revigoracao++;
            }
            if (importanciaFinal == Int32.MaxValue) importanciaFinal = 10;

            return situacoes.results.Where<SituacaoOV>(st => st.nr_peso_situacao == importanciaFinal).First<SituacaoOV>();
        }

        private static bool EhModificacaoTotal(Vide vide)
        {
            //Se todos os campos estiverem vazios deve retornar TRUE pois trata-se de uma modificação total.
            return string.IsNullOrEmpty(vide.alinea_norma_vide) && string.IsNullOrEmpty(vide.anexo_norma_vide) && string.IsNullOrEmpty(vide.artigo_norma_vide) && string.IsNullOrEmpty(vide.paragrafo_norma_vide) && string.IsNullOrEmpty(vide.inciso_norma_vide) && string.IsNullOrEmpty(vide.item_norma_vide) && (vide.caput_norma_vide == null || vide.caput_norma_vide.caput == null || vide.caput_norma_vide.caput.Length <= 0);
        }

        public bool RemoverVides(NormaOV normaOv)
        {
            var ch_norma = normaOv.ch_norma;
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("'{0}'=any(ch_norma_vide)", ch_norma);
            var results = Consultar(query);
            foreach(var norma in results.results)
            {
                norma.vides.RemoveAll(v => v.ch_norma_vide == ch_norma);
                if(PathPut(norma._metadata.id_doc, "vides", JSON.Serialize(norma.vides), null) != "UPDATED")
                {
                    return false;
                }
            }
            return true;
        }
	
        #region Arquivo

        public string AnexarArquivo(util.BRLight.FileParameter fileParameter)
        {
            return _normaAd.AnexarArquivo(fileParameter);
        }

        public string GetDoc(string id_file)
        {
            return _normaAd.GetDoc(id_file);
        }

        public byte[] Download(string id_file)
        {
            return _normaAd.Download(id_file);
        }

        #endregion

        #region ES
        

        public Result<NormaOV> ConsultaEs(string query)
        {
            return _normaAd.ConsultarEs(query);
        }

        public string PesquisarTotalEs(string query)
        {
            return _normaAd.PesquisarTotalEs(query);
        }

        #endregion

    }
}
