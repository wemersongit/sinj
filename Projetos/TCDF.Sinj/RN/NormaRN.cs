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
using System.Globalization;
using System.Text.RegularExpressions;

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
                        //Linha inserida para listar e "pegar" a ultima decisão. NormaRN.cs by Wemerson
                        decisoes_ordenadas_por_data = normaOv.decisoes;
                        //decisoes_ordenadas_por_data = decisoes_ordenadas_por_data.OrderBy(d => Convert.ToDateTime(d.dt_decisao, new CultureInfo("pt-BR"))).ToList();
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
                        case TipoDeDecisaoEnum.MeritoParcialmenteProcedente:
                            situacaoOv = situacaoRn.Doc("julgadoparcialmenteprocedente");
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
            bool existeRepristinacao = false;
            bool ajuizado = false;
            var vides_que_afetam_a_norma = vides.Where<Vide>(v => v.in_norma_afetada).ToList<Vide>();

            var i_revigoracao = 0;
            // var i_repristinacao = 0;

            foreach (var vide in vides_que_afetam_a_norma)
            {
                //verifica se a norma alteradora entrou em vigencia (vacatio legis)
                if (!string.IsNullOrEmpty(vide.dt_inicio_vigencia_norma_vide) && Convert.ToDateTime(vide.dt_inicio_vigencia_norma_vide, new CultureInfo("pt-BR")) > DateTime.Now)
                {
                    continue;
                }

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

                // Suspensão 6(*8)
                if (relacao.nm_tipo_relacao.ToLower() == "suspensão")
                {
                    if (!EhModificacaoTotal(vide))
                    {
                        importanciaDaVez = 8;
                    }
                }

                // Julgada Procedente            6(*8)
                if (relacao.nm_tipo_relacao.ToLower() == "julgada procedente")
                {
                    if (!EhModificacaoTotal(vide))
                    {
                        importanciaDaVez = 8;
                    }
                }
                // SUSTAÇÃO TOTAL               3 (*8)
                if (relacao.nm_tipo_relacao.ToLower() == "sustação" || relacao.nm_tipo_relacao.ToLower() == "sustação total")
                {
                    if (!EhModificacaoTotal(vide))
                    {
                        importanciaDaVez = 8;
                    }
                }
                // ANULAÇÃO                        4 (*8)
                if (relacao.nm_tipo_relacao.ToLower() == "anulação")
                {
                    if (!EhModificacaoTotal(vide))
                    {
                        importanciaDaVez = 8;
                    }
                }

                // REVOGAÇÃO                    2 (*8)
                // REVOGAÇÃO TOTAL              2 (*8)
                // Caso seja uma revogação ou revogação total
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
                                var relacao_repristinacao = relacoes.results.Where<TipoDeRelacaoOV>(tdr => tdr.ch_tipo_relacao == vides_que_afetam_a_norma[i].ch_tipo_relacao).First<TipoDeRelacaoOV>();
                                if ((relacao_repristinacao.nm_tipo_relacao.ToLower() == "repristinação" || relacao_repristinacao.nm_tipo_relacao.ToLower() == "repristinação total"))
                                {
                                    existeRepristinacao = true;
                                    break;
                                }
                            }
                        }
                        if (existeRevigoracao)
                        {
                            existeRevigoracao = false;
                            importanciaDaVez = 8;
                        }
                        if (existeRepristinacao)
                        {
                            existeRepristinacao = false;
                            importanciaDaVez = 8;
                        }
                    }
                    else
                    {
                        importanciaDaVez = 8;
                    }
                }

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
            return string.IsNullOrEmpty(vide.alinea_norma_vide) && 
                    string.IsNullOrEmpty(vide.anexo_norma_vide) && 
                    string.IsNullOrEmpty(vide.artigo_norma_vide) && 
                    string.IsNullOrEmpty(vide.paragrafo_norma_vide) && 
                    string.IsNullOrEmpty(vide.inciso_norma_vide) && 
                    string.IsNullOrEmpty(vide.item_norma_vide) && 
                    (
                        vide.caput_norma_vide == null || 
                        vide.caput_norma_vide.caput == null || 
                        vide.caput_norma_vide.caput.Length <= 0
                    );
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

        #region Vide Arquivos Incluir Alteração

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
            var arquivoVersionadoRn = new ArquivoVersionadoNormaRN();

            var fileNorma = norma.getFileArquivoVigente();

            if (fileNorma.id_file != null)
            {

                var byteFileNorma = Download(fileNorma.id_file);

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
            var upload = new UtilArquivoHtml();
            var pesquisa = new Pesquisa();

            var arquivo_norma_vide_alteradora = CriarLinkNoTextoDaNormaAlteradora(caput_norma_vide_alteradora, caput_norma_vide_alterada);
            if (!string.IsNullOrEmpty(arquivo_norma_vide_alteradora))
            {
                var retorno_file_alteradora = upload.AnexarHtml(arquivo_norma_vide_alteradora, caput_norma_vide_alteradora.filename, "sinj_norma");

                if (retorno_file_alteradora.IndexOf("id_file") > -1)
                {
                    PathPut(normaAlteradora._metadata.id_doc, "ar_atualizado", retorno_file_alteradora, null);
                }
            }
            var arquivo_norma_vide_alterada = AlterarDispositivosDaNormaAlterada(caput_norma_vide_alterada, caput_norma_vide_alteradora);
            if (!string.IsNullOrEmpty(arquivo_norma_vide_alterada))
            {
                var retorno_file_alterada = upload.AnexarHtml(arquivo_norma_vide_alterada, caput_norma_vide_alterada.filename, "sinj_norma");
                if (retorno_file_alterada.IndexOf("id_file") > -1)
                {
                    PathPut(normaAlterada._metadata.id_doc, "ar_atualizado", retorno_file_alterada, null);
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
            var upload = new UtilArquivoHtml();
            var pesquisa = new Pesquisa();

            var nameFileNormaAlteradora = normaAlteradora.getNameFileArquivoVigente();

            var arquivoNormaVideAlterada = AlterarDispositivosDaNormaAlterada(caputNormaVideAlterada, normaAlteradora);
            if (!string.IsNullOrEmpty(arquivoNormaVideAlterada))
            {
                var retorno_file_alterada = upload.AnexarHtml(arquivoNormaVideAlterada, caputNormaVideAlterada.filename, "sinj_norma");
                if (retorno_file_alterada.IndexOf("id_file") > -1)
                {
                    PathPut(normaAlterada._metadata.id_doc, "ar_atualizado", retorno_file_alterada, null);
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
            var upload = new UtilArquivoHtml();
            var pesquisa = new Pesquisa();

            var id_file_norma_alterada = normaAlterada.getIdFileArquivoVigente();
            var name_file_norma_alterada = normaAlterada.getNameFileArquivoVigente();

            if (!string.IsNullOrEmpty(id_file_norma_alterada))
            {
                var aux_nm_situacao_alterada = normaAlterada.nm_situacao.ToLower();

                var arquivo_norma_vide_alterada = "";
                //se a situação não é revogado e a relação de vide é revigorado deve desfazer as alterações da revogação
                if (aux_nm_situacao_alterada != "revogado" && caputNormaVideAlterador.ds_texto_para_alterador_aux == "revigorado")
                {
                    arquivo_norma_vide_alterada = AlterarTextoCompletoDaNormaRevigorada(normaAlteradora, normaAlterada, caputNormaVideAlterador);
                }
                else if (aux_nm_situacao_alterada != "revogado" && caputNormaVideAlterador.ds_texto_para_alterador_aux == "repristinado")
                {
                    arquivo_norma_vide_alterada = AlterarTextoCompletoDaNormaRepristinada(normaAlteradora, normaAlterada, caputNormaVideAlterador);
                }
                else if (UtilVides.EhAlteracaoCompleta(aux_nm_situacao_alterada, caputNormaVideAlterador.ds_texto_para_alterador_aux))
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
                        PathPut(normaAlterada._metadata.id_doc, "ar_atualizado", retorno_file_alterada, null);

                        var arquivo_norma_vide_revogadora = CriarLinkNoTextoDaNormaAlteradora(caputNormaVideAlterador, normaAlterada.ch_norma, name_file_norma_alterada);
                        if (!string.IsNullOrEmpty(arquivo_norma_vide_revogadora))
                        {
                            var retorno_file_alteradora = upload.AnexarHtml(arquivo_norma_vide_revogadora, caputNormaVideAlterador.filename, "sinj_norma");

                            if (retorno_file_alteradora.IndexOf("id_file") > -1)
                            {
                                PathPut(normaAlteradora._metadata.id_doc, "ar_atualizado", retorno_file_alteradora, null);
                            }
                        }
                    }
                }
            }

        }

        public void IncluirAlteracaoSemDispositivosNosArquivosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterado)
        {
            var upload = new UtilArquivoHtml();
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

                //se a situação não é revogado e a relação de vide é revigorado deve desfazer as alterações da revogação
                if (aux_nm_situacao_alterada != "revogado" && aux_ds_texto_alterador == "revigorado")
                {
                    arquivo_norma_vide_alterada = AlterarTextoCompletoDaNormaRevigorada(normaAlteradora, normaAlterada, aux_ds_texto_alterador);
                }
                else if (aux_nm_situacao_alterada != "revogado" && aux_ds_texto_alterador == "repristinado")
                {
                    arquivo_norma_vide_alterada = AlterarTextoCompletoDaNormaRepristinada(normaAlteradora, normaAlterada, aux_ds_texto_alterador);
                }
                //else if (aux_nm_situacao_alterada != "revogado" && aux_ds_texto_alterador == "repristinado")
                //{
                //    arquivo_norma_vide_alterada = AlterarTextoCompletoDaNormaRepristinada(normaAlteradora, normaAlterada, aux_ds_texto_alterador);
                //}
                else if (aux_nm_situacao_alterada == "suspenso" && aux_ds_texto_alterador == "suspenso(a) liminarmente")
                {
                    arquivo_norma_vide_alterada = AlterarTextoCompletoDaNormaSuspensa(normaAlteradora, if_file_norma_alterada, aux_ds_texto_alterador);
                }
                else if (UtilVides.EhAlteracaoCompleta(aux_nm_situacao_alterada, aux_ds_texto_alterador))
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
                        PathPut(normaAlterada._metadata.id_doc, "ar_atualizado", retorno_file_alterada, null);

                        var arquivoNormaAlteradora = AcrescentarInformacaoNoTextoDaNormaAlteradora(normaAlterada, idFileNormaAlteradora, aux_ds_texto_alterador);
                        if (!string.IsNullOrEmpty(arquivoNormaAlteradora))
                        {
                            var retornoFileAlteradora = upload.AnexarHtml(arquivoNormaAlteradora, nameFileNormaAlteradora, "sinj_norma");

                            if (retornoFileAlteradora.IndexOf("id_file") > -1)
                            {
                                PathPut(normaAlteradora._metadata.id_doc, "ar_atualizado", retornoFileAlteradora, null);
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
            var htmlFile = new UtilArquivoHtml();
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
            var htmlFile = new UtilArquivoHtml();
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
                var htmlFile = new UtilArquivoHtml();
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
                            // var texto_novo_splited = _caput_alterada.texto_novo[i].Split('\n');
                            var texto_novo_splited = _caput_alterada.texto_novo[i].Split('\n').Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
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
                            ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i].Replace("_replaced", "")) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + _caput_alteradora.ds_norma + ")";
                            pattern = "(<p.+?linkname=\"" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "\".*?)replaced_by=\"(.*?)\"(.*?)<s>(.*?)</s>(.*?)</p>";
                            replacement = "$1replaced_by_disabled=\"$2\"$3$4$5 <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                            if (!string.IsNullOrEmpty(_caput_alterada.texto_novo[i]))
                            {
                                pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?)replaced_by=\"(.*?)\"(.*?>)(.*?)(<a.+?name=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "(\".*?></a>)(.*?)</p>";
                                replacement = "$1" + _caput_alterada.caput[i] + "$2replaced_by=\"$3\"$4$5$6" + _caput_alterada.caput[i] + "$7$8</p>\r\n$1" + _caput_alterada.caput[i].Replace("_replaced", "") + "$2replaced_by_disabled=\"$3\"$4<a name=\"" + _caput_alterada.caput[i].Replace("_replaced", "") + "\"></a>" + _caput_alterada.texto_novo[i] + " <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\">" + ds_link_alterador + "</a></p>";
                            }
                            break;
                        case "repristinação":
                            ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i].Replace("_replaced", "")) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + _caput_alteradora.ds_norma + ")";
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
                        // Código adicionado para resolver o problema com o '$' quebrando o texto. by Wemerson
                        //replacement = replacement.Replace("$", "$ ");
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
                var htmlFile = new UtilArquivoHtml();
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
                        // var texto_novo_splited = _caput_alterada.texto_novo[i].Split('\n');
                        var texto_novo_splited = _caput_alterada.texto_novo[i].Split('\n').Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
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
                    case "repristinação":
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
                    case "suspensão":
                        ds_link_alterador = "(" + UtilVides.gerarDescricaoDoCaput(_caput_alterada.caput[i]) + UtilVides.getRelacaoParaTextoAlterador(_caput_alterada.ds_texto_para_alterador_aux) + " pelo(a) " + ds_norma_alteradora + ")";
                        if (_caput_alterada.nm_relacao_aux == "suspensão")
                        {
                            ds_link_alterador = "(Suspenso(a) liminarmente - " + ds_norma_alteradora + ")";
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

                    // NOTE: Resolve os bugs de <s><s>... By Douguete
                    texto = Regex.Replace(texto, "(<s>+)\\1+", "$1");
                    texto = Regex.Replace(texto, "(</s>+)\\1+", "$1");

                    // Código adicionado para resolver o problema com o '$' quebrando o texto. by Wemerson
                    //texto = replacement.Replace("$", "$ ");

                    bAlterou = true;
                }
                if (!bAlterou)
                {
                    texto = "";
                }
            }
            return texto;
        }

        public string AlterarTextoCompletoDaNormaAlterada(NormaOV normaAlteradora, string id_file_norma_alterada, Caput _caput_alteradora)
        {
            var htmlFile = new UtilArquivoHtml();
            var texto = htmlFile.GetHtmlFile(id_file_norma_alterada, "sinj_norma", null);
            return AlterarTextoCompletoDaNormaAlterada(texto, normaAlteradora, _caput_alteradora);
        }

        public string AlterarTextoCompletoDaNormaAlterada(string texto, NormaOV normaAlteradora, Caput _caput_alteradora)
        {
            var htmlFile = new UtilArquivoHtml();

            //ignora os paragrafos com atributos 'replaced_by' ou 'nota', aceitando todos os outros
            //replaced_by indica que um dispositivo especifico foi alterado por uma norma, então a alteração a ser feita
            //não pode mexer nesses dispositivos que já foram alterados
            //e a nota é só um texto inserido pelos cadastradores de texto e não fazer parte do texto da norma
            var pattern1 = "(?!<p.+(?:replaced_by=|nota=|ch_norma_alteracao_completa=|ch_norma_info=).+>)(<p.+?>)(.+?)</p>";
            Regex rx1 = new Regex(pattern1);

            var pattern2 = "(<h1.+?epigrafe=.+?>.+?</h1>)";
            Regex rx2 = new Regex(pattern2, RegexOptions.Singleline);

            if (rx1.Matches(texto).Count > 0 || rx2.Matches(texto).Count == 1)
            {
                var replacement1 = "$1<s>$2</s></p>";
                var replacement2 = "$1\r\n<p ch_norma_alteracao_completa=\"" + normaAlteradora.ch_norma + "\" style=\"text-align:center;\"><a href=\"(_link_sistema_)Norma/" + normaAlteradora.ch_norma + "/" + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >(" + _caput_alteradora.ds_texto_para_alterador_aux + " pelo(a) " + _caput_alteradora.ds_norma + ")</a></p>";

                var pattern = _caput_alteradora.ds_norma;

                //Verifica se no texto contém o caput e se não existir, ele adiciona.
                //Caso já contenha o caput ele ignora e retorna o texto sem adicionar o link novamente. By Wemerson
                Regex verificaSeJaExisteOCaput = new Regex(pattern);
                if (verificaSeJaExisteOCaput.Matches(texto).Count <= 0)
                {
                    texto = rx1.Replace(texto, replacement1);
                    texto = rx2.Replace(texto, replacement2);
                }

            }
            else
            {
                texto = "";
            }
            return texto;
        }

        public string AlterarTextoCompletoDaNormaRevigorada(NormaOV normaAlteradora, NormaOV normaAlterada, Caput caputAlteradora)
        {
            var texto = "";
            Vide videDaRevogacao = null;

            // NOTE: Seleciona o último vide da revogação que na ação da revigoração vai ser desfeito. É necessário saber a chave da 
            // norma que revogou para inserir o link da revigoração após o paragrafo da revogação. By Questor
            foreach (var vide in normaAlterada.vides)
            {
                if (vide.nm_tipo_relacao.Equals("REVOGAÇÃO") && vide.in_norma_afetada && (vide.caput_norma_vide == null || vide.caput_norma_vide.caput == null || vide.caput_norma_vide.caput.Length == 0))
                {
                    if (videDaRevogacao == null || (!string.IsNullOrEmpty(vide.dt_assinatura_norma_vide) && !vide.possuiDispositivoInformadoManualmente() && Convert.ToDateTime(vide.dt_assinatura_norma_vide, new CultureInfo("pt-BR")) > Convert.ToDateTime(videDaRevogacao.dt_assinatura_norma_vide, new CultureInfo("pt-BR"))))
                    {
                        videDaRevogacao = util.BRLight.objHelp.Clone<Vide>(vide);
                    }
                }
            }

            if (videDaRevogacao != null)
            {
                var htmlFile = new UtilArquivoHtml();
                var idFileNormaAlterada = normaAlterada.getIdFileArquivoVigente();
                texto = htmlFile.GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);
                var pattern1 = "(?!<p.+(?:replaced_by=|nota=).+>)(<p.+?>)<s>(.+?)</s></p>";
                var pattern2a = "";
                var pattern2b = "";
                // pattern2a = "(\r\n<p.+?ch_norma_alteracao_completa=\"" + videDaRevogacao.caput_norma_vide_outra.ch_norma + "\".*?><a.+?>\\(revogado.+?\\)</a></p>)";
                pattern2a = "(\r\n<p.+?ch_norma_alteracao_completa=\"" + videDaRevogacao.ch_norma_vide + "\".*?><a.+?>\\(revogado.+?\\)</a></p>)";
                // pattern2b = "(\r\n<p.*?><a.+?/" + videDaRevogacao.caput_norma_vide_outra.ch_norma + "/.+?>\\(revogado.+?\\)</a></p>)";
                pattern2b = "(\r\n<p.*?><a.+?/" + videDaRevogacao.ch_norma_vide + "/.+?>\\(revogado.+?\\)</a></p>)";
                Regex rx1 = new Regex(pattern1);
                Regex rx2 = new Regex(pattern2a, RegexOptions.Singleline);
                if (rx2.Matches(texto).Count <= 0)
                {
                    rx2 = new Regex(pattern2b, RegexOptions.Singleline);
                }

                if (rx1.Matches(texto).Count > 0 || rx2.Matches(texto).Count == 1)
                {
                    var replacement1 = "$1$2</p>";

                    // NOTE: Acrescenta abaixo do link, (revogado pelo(a) ....), o novo link, (revigorado pelo(a) ....). By Questors
                    var replacement2 = "$1\r\n<p ch_norma_alteracao_completa=\"" + normaAlteradora.ch_norma + "\" style=\"text-align:center;\"><a href=\"(_link_sistema_)Norma/" + normaAlteradora.ch_norma + "/" + caputAlteradora.filename + "#" + caputAlteradora.caput[0] + "\" >(" + caputAlteradora.ds_texto_para_alterador_aux + " pelo(a) " + caputAlteradora.ds_norma + ")</a></p>\r\n";

                    texto = rx1.Replace(texto, replacement1);
                    texto = rx2.Replace(texto, replacement2);
                }
                else
                {
                    texto = "";
                }
            }
            return texto;
        }

        public string AlterarTextoCompletoDaNormaRepristinada(NormaOV normaAlteradora, NormaOV normaAlterada, Caput caputAlteradora)
        {
            var texto = "";
            Vide videDaRevogacao = null;

            // NOTE: Seleciona o último vide da revogação que na ação da revigoração vai ser desfeito. É necessário saber a chave 
            // da norma que revogou para inserir o link da repristinação após o paragrafo da revogação. By Questor
            foreach (var vide in normaAlterada.vides)
            {
                if (vide.nm_tipo_relacao.Equals("REVOGAÇÃO") && vide.in_norma_afetada && (vide.caput_norma_vide == null || vide.caput_norma_vide.caput == null || vide.caput_norma_vide.caput.Length == 0))
                {
                    if (videDaRevogacao == null || (!string.IsNullOrEmpty(vide.dt_assinatura_norma_vide) && !vide.possuiDispositivoInformadoManualmente() && Convert.ToDateTime(vide.dt_assinatura_norma_vide, new CultureInfo("pt-BR")) > Convert.ToDateTime(videDaRevogacao.dt_assinatura_norma_vide, new CultureInfo("pt-BR"))))
                    {
                        videDaRevogacao = util.BRLight.objHelp.Clone<Vide>(vide);
                    }
                }
            }

            if (videDaRevogacao != null)
            {
                var htmlFile = new UtilArquivoHtml();
                var idFileNormaAlterada = normaAlterada.getIdFileArquivoVigente();
                texto = htmlFile.GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);
                var pattern1 = "(?!<p.+(?:replaced_by=|nota=).+>)(<p.+?>)<s>(.+?)</s></p>";
                var pattern2a = "";
                var pattern2b = "";
                // pattern2a = "(\r\n<p.+?ch_norma_alteracao_completa=\"" + videDaRevogacao.caput_norma_vide_outra.ch_norma + "\".*?><a.+?>\\(revogado.+?\\)</a></p>)";
                pattern2a = "(\r\n<p.+?ch_norma_alteracao_completa=\"" + videDaRevogacao.ch_norma_vide + "\".*?><a.+?>\\(revogado.+?\\)</a></p>)";
                // pattern2b = "(\r\n<p.*?><a.+?/" + videDaRevogacao.caput_norma_vide_outra.ch_norma + "/.+?>\\(revogado.+?\\)</a></p>)";
                pattern2b = "(\r\n<p.*?><a.+?/" + videDaRevogacao.ch_norma_vide + "/.+?>\\(revogado.+?\\)</a></p>)";
                Regex rx1 = new Regex(pattern1);
                Regex rx2 = new Regex(pattern2a, RegexOptions.Singleline);
                if (rx2.Matches(texto).Count <= 0)
                {
                    rx2 = new Regex(pattern2b, RegexOptions.Singleline);
                }
                if (rx1.Matches(texto).Count > 0 || rx2.Matches(texto).Count == 1)
                {
                    var replacement1 = "$1$2</p>";

                    // NOTE: Acrescenta abaixo do link, (revogado pelo(a) ....), o novo link, (revigorado pelo(a) ....). By Questor
                    var replacement2 = "$1\r\n<p ch_norma_alteracao_completa=\"" + normaAlteradora.ch_norma + "\" style=\"text-align:center;\"><a href=\"(_link_sistema_)Norma/" + normaAlteradora.ch_norma + "/" + caputAlteradora.filename + "#" + caputAlteradora.caput[0] + "\" >(" + caputAlteradora.ds_texto_para_alterador_aux + " pelo(a) " + caputAlteradora.ds_norma + ")</a></p>\r\n";
                    foreach(var vide in normaAlterada.vides) //retira a hachura do texto by Wemerson
                    {
                        texto = rx1.Replace(texto, replacement1);
                    }
                    texto = rx2.Replace(texto, replacement2);
                }
                else
                {
                    texto = "";
                }
            }
            return texto;
        }

        public string AlterarTextoCompletoDaNormaRevigorada(NormaOV normaAlteradora, NormaOV normaAlterada, string dsTextoAlterador)
        {
            var texto = "";
            Vide videDaRevogacao = null;

            // NOTE: Seleciona o último vide da revogação que na ação da revigoração vai ser desfeito. É necessário saber a chave 
            // da norma que revogou para inserir o link da revigoração após o paragrafo da revogação. By Questor
            foreach (var vide in normaAlterada.vides)
            {
                if (vide.nm_tipo_relacao.Equals("REVOGAÇÃO") && vide.in_norma_afetada && (vide.caput_norma_vide == null || vide.caput_norma_vide.caput == null || vide.caput_norma_vide.caput.Length == 0))
                {
                    if (videDaRevogacao == null || (!string.IsNullOrEmpty(vide.dt_assinatura_norma_vide) && !vide.possuiDispositivoInformadoManualmente() && Convert.ToDateTime(vide.dt_assinatura_norma_vide, new CultureInfo("pt-BR")) > Convert.ToDateTime(videDaRevogacao.dt_assinatura_norma_vide, new CultureInfo("pt-BR"))))
                    {
                        videDaRevogacao = util.BRLight.objHelp.Clone<Vide>(vide);
                    }
                }
            }

            if (videDaRevogacao != null)
            {
                var htmlFile = new UtilArquivoHtml();
                var idFileNormaAlterada = normaAlterada.getIdFileArquivoVigente();
                var nameFileNormaAlteradora = normaAlteradora.getNameFileArquivoVigente();
                var aux_href = !string.IsNullOrEmpty(nameFileNormaAlteradora) ? ("(_link_sistema_)Norma/" + normaAlteradora.ch_norma + "/" + nameFileNormaAlteradora) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + normaAlteradora.ch_norma;
                texto = htmlFile.GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);
                var dsNormaAlteradora = normaAlteradora.getDescricaoDaNorma();
                var pattern1 = "(?!<p.+(?:replaced_by=|nota=).+>)(<p.+?>)<s>(.+?)</s></p>";
                // var pattern2a = "(\r\n<p.+?ch_norma_alteracao_completa=\"" + videDaRevogacao.caput_norma_vide_outra.ch_norma + "\".*?><a.+?>\\(.+?\\)</a></p>)";
                // var pattern2b = "(\r\n<p.*?><a.+?/" + videDaRevogacao.caput_norma_vide_outra.ch_norma + "/.+?>\\(.+?\\)</a></p>)";
                var pattern2a = "(\r\n<p.+?ch_norma_alteracao_completa=\"" + videDaRevogacao.ch_norma_vide + "\".*?><a.+?>\\(.+?\\)</a></p>)";
                var pattern2b = "(\r\n<p.*?><a.+?/" + videDaRevogacao.ch_norma_vide + "/.+?>\\(.+?\\)</a></p>)";
                Regex rx1 = new Regex(pattern1);
                Regex rx2 = new Regex(pattern2a, RegexOptions.Singleline);
                if (rx2.Matches(texto).Count <= 0)
                {
                    rx2 = new Regex(pattern2b, RegexOptions.Singleline);
                }
                if (rx1.Matches(texto).Count > 0 || rx2.Matches(texto).Count == 1)
                {
                    var replacement1 = "$1$2</p>";

                    // NOTE: Acrescenta abaixo do link, (revogado pelo(a) ....), o novo link, (revigorado pelo(a) ....). By Questor
                    var replacement2 = "$1\r\n<p ch_norma_alteracao_completa=\"" + normaAlteradora.ch_norma + "\" style=\"text-align:center;\"><a href=\"" + aux_href + "\" >(" + dsTextoAlterador + " pelo(a) " + dsNormaAlteradora + ")</a></p>\r\n";

                    foreach (var vide in normaAlterada.vides)//retira a hachura do texto by Wemerson
                    {
                        texto = rx1.Replace(texto, replacement1);
                    }
                    texto = rx2.Replace(texto, replacement2);
                }
                else
                {
                    texto = "";
                }
            }
            return texto;
        }

        public string AlterarTextoCompletoDaNormaRepristinada(NormaOV normaAlteradora, NormaOV normaAlterada, string dsTextoAlterador)
        {
            var texto = "";
            Vide videDaRevogacao = null;

            // NOTE: Seleciona o último vide da revogação que na ação da repristinação vai ser desfeito. É necessário saber a chave da norma 
            // que revogou para inserir o link da repristinação após o paragrafo da revogação. By Questor
            foreach (var vide in normaAlterada.vides)
            {
                if (vide.nm_tipo_relacao.Equals("REVOGAÇÃO") && vide.in_norma_afetada && (vide.caput_norma_vide == null || vide.caput_norma_vide.caput == null || vide.caput_norma_vide.caput.Length == 0))
                {
                    if (videDaRevogacao == null || (!string.IsNullOrEmpty(vide.dt_assinatura_norma_vide) && !vide.possuiDispositivoInformadoManualmente() && Convert.ToDateTime(vide.dt_assinatura_norma_vide, new CultureInfo("pt-BR")) > Convert.ToDateTime(videDaRevogacao.dt_assinatura_norma_vide, new CultureInfo("pt-BR"))))
                    {
                        videDaRevogacao = util.BRLight.objHelp.Clone<Vide>(vide);
                    }
                }
            }
            if (videDaRevogacao != null)
            {
                var htmlFile = new UtilArquivoHtml();
                var idFileNormaAlterada = normaAlterada.getIdFileArquivoVigente();
                var nameFileNormaAlteradora = normaAlteradora.getNameFileArquivoVigente();
                var aux_href = !string.IsNullOrEmpty(nameFileNormaAlteradora) ? ("(_link_sistema_)Norma/" + normaAlteradora.ch_norma + "/" + nameFileNormaAlteradora) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + normaAlteradora.ch_norma;
                texto = htmlFile.GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);
                var dsNormaAlteradora = normaAlteradora.getDescricaoDaNorma();
                var pattern1 = "(?!<p.+(?:replaced_by=|nota=).+>)(<p.+?>)<s>(.+?)</s></p>";
                var pattern2a = "(\r\n<p.+?ch_norma_alteracao_completa=\"" + videDaRevogacao.ch_norma_vide + "\".*?><a.+?>\\(.+?\\)</a></p>)";
                var pattern2b = "(\r\n<p.*?><a.+?/" + videDaRevogacao.ch_norma_vide + "/.+?>\\(.+?\\)</a></p>)";
                Regex rx1 = new Regex(pattern1);
                Regex rx2 = new Regex(pattern2a, RegexOptions.Singleline);
                if (rx2.Matches(texto).Count <= 0)
                {
                    rx2 = new Regex(pattern2b, RegexOptions.Singleline);
                }
                if (rx1.Matches(texto).Count > 0 || rx2.Matches(texto).Count == 1)
                {
                    var replacement1 = "$1$2</p>";

                    // NOTE: Acrescenta abaixo do link, "(revogado pelo(a) ...)", o novo link, "(repristinado pelo(a) ...)". By Questor
                    var replacement2 = "$1\r\n<p ch_norma_alteracao_completa=\"" + normaAlteradora.ch_norma + "\" style=\"text-align:center;\"><a href=\"" + aux_href + "\" >(" + dsTextoAlterador + " pelo(a) " + dsNormaAlteradora + ")</a></p>\r\n";
                    foreach(var vide in normaAlterada.vides)
                    {
                        texto = rx1.Replace(texto, replacement1);
                    }
                    texto = rx2.Replace(texto, replacement2);
                }
                else
                {
                    texto = "";
                }
            }
            return texto;
        }

        public string AlterarTextoCompletoDaNormaSuspensa(NormaOV norma_alteradora, string id_file_norma_alterada, string ds_texto_alterador)
        {
            var htmlFile = new UtilArquivoHtml();
            var texto = htmlFile.GetHtmlFile(id_file_norma_alterada, "sinj_norma", null);
            return AlterarTextoCompletoDaNormaSuspensa(texto, norma_alteradora, ds_texto_alterador);
        }

        public string AlterarTextoCompletoDaNormaSuspensa(string texto, NormaOV norma_alteradora, string ds_texto_alterador)
        {
            var htmlFile = new UtilArquivoHtml();

            // NOTE: Ignora os paragrafos com atributos 'replaced_by' ou 'nota', aceitando todos os outros replaced_by indica que 
            // um dispositivo especifico foi alterado por uma norma, então a alteração a ser feita não pode mexer nesses dispositivos 
            // que já foram alterados a nota é só um texto inserido pelos cadastradores de texto e não fazer parte do texto da norma. 
            // By Questor
            var pattern1 = "(?!<p.+(?:replaced_by=|nota=|ch_norma_alteracao_completa=|ch_norma_info=).+>)(<p.+?>)(.+?)</p>";

            var name_file_norma_alteradora = norma_alteradora.getNameFileArquivoVigente();
            var ds_norma_alteradora = norma_alteradora.getDescricaoDaNorma();

            var aux_href = !string.IsNullOrEmpty(name_file_norma_alteradora) ? ("(_link_sistema_)Norma/" + norma_alteradora.ch_norma + "/" + name_file_norma_alteradora) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + norma_alteradora.ch_norma;

            var pattern2 = "(<h1.+?epigrafe=.+?>.+?</h1>)";
            var replacement2 = "$1\r\n<p ch_norma_alteracao_completa=\"" + norma_alteradora.ch_norma + "\" style=\"text-align:center;\"><a href=\"" + aux_href + "\" >(" + ds_texto_alterador + " pelo(a) " + ds_norma_alteradora + ")</a></p>";

            if (Regex.Matches(texto, pattern1).Count > 0 || Regex.Matches(texto, pattern2).Count == 1)
            {
                texto = Regex.Replace(texto, pattern2, replacement2);
            }
            else
            {
                texto = "";
            }

            return texto;
        }

        public string AlterarTextoCompletoDaNormaAlterada(NormaOV norma_alteradora, string id_file_norma_alterada, string ds_texto_alterador)
        {
            var htmlFile = new UtilArquivoHtml();
            var texto = htmlFile.GetHtmlFile(id_file_norma_alterada, "sinj_norma", null);
            return AlterarTextoCompletoDaNormaAlterada(texto, norma_alteradora, ds_texto_alterador);
        }

        public string AlterarTextoCompletoDaNormaAlterada(string texto, NormaOV norma_alteradora, string ds_texto_alterador)
        {
            var htmlFile = new UtilArquivoHtml();
            //ignora os paragrafos com atributos 'replaced_by' ou 'nota', aceitando todos os outros
            //replaced_by indica que um dispositivo especifico foi alterado por uma norma, então a alteração a ser feita
            //não pode mexer nesses dispositivos que já foram alterados
            //e a nota é só um texto inserido pelos cadastradores de texto e não fazer parte do texto da norma
            var pattern1 = "(?!<p.+(?:replaced_by=|nota=|ch_norma_alteracao_completa=|ch_norma_info=).+>)(<p.+?>)(.+?)</p>";
            var replacement1 = "$1<s>$2</s></p>";

            var name_file_norma_alteradora = norma_alteradora.getNameFileArquivoVigente();
            var ds_norma_alteradora = norma_alteradora.getDescricaoDaNorma();

            var aux_href = !string.IsNullOrEmpty(name_file_norma_alteradora) ? ("(_link_sistema_)Norma/" + norma_alteradora.ch_norma + "/" + name_file_norma_alteradora) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + norma_alteradora.ch_norma;

            var pattern2 = "(<h1.+?epigrafe=.+?>.+?</h1>)";
            var replacement2 = "$1\r\n<p ch_norma_alteracao_completa=\"" + norma_alteradora.ch_norma + "\" style=\"text-align:center;\"><a href=\"" + aux_href + "\" >(" + ds_texto_alterador + " pelo(a) " + ds_norma_alteradora + ")</a></p>";

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
            var htmlFile = new UtilArquivoHtml();
            var texto = htmlFile.GetHtmlFile(id_file_norma_alterada, "sinj_norma", null);
            return AcrescentarInformacaoNoTextoDaNormaAlterada(texto, norma_alteradora, _caput_alteradora);
        }

        public string AcrescentarInformacaoNoTextoDaNormaAlterada(string texto, NormaOV normaAlteradora, Caput _caput_alteradora)
        {
            var htmlFile = new UtilArquivoHtml();
            var pattern = "(<h1.+?epigrafe=.+?>.+?</h1>)";
            if (Regex.Matches(texto, pattern).Count == 1)
            {
                var replacement = "$1\r\n<p ch_norma_info=\"" + normaAlteradora.ch_norma + "\"><a href=\"(_link_sistema_)Norma/" + normaAlteradora.ch_norma + "/" + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >(" + _caput_alteradora.ds_texto_para_alterador_aux + " pelo(a) " + _caput_alteradora.ds_norma + ")</a></p>";
                if (_caput_alteradora.ds_texto_para_alterador_aux == "legislação correlata")
                {
                    replacement = "<p ch_norma_info=\"" + normaAlteradora.ch_norma + "\"><a href=\"(_link_sistema_)Norma/" + normaAlteradora.ch_norma + "/" + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >Legislação correlata - " + _caput_alteradora.ds_norma + "</a></p>\r\n$1";
                }


                //if (_caput_alteradora.ds_texto_para_alterador_aux == "suspenso(a) liminarmente")
                //{
                //    replacement = "<p ch_norma_info=\"" + normaAlteradora.ch_norma + "\"><a href=\"(_link_sistema_)Norma/" + normaAlteradora.ch_norma + "/" + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >Suspensão - " + _caput_alteradora.ds_norma + "</a></p>\r\n$1";
                //}



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
            var htmlFile = new UtilArquivoHtml();
            var texto = htmlFile.GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);
            var pattern = "(<h1.+?epigrafe=.+?>.+?</h1>)";

            var nameFileNormaAlteradora = normaAlteradora.getNameFileArquivoVigente();
            var dsNormaAlteradora = normaAlteradora.getDescricaoDaNorma();

            var aux_href = !string.IsNullOrEmpty(nameFileNormaAlteradora) ? ("(_link_sistema_)Norma/" + normaAlteradora.ch_norma + "/" + nameFileNormaAlteradora) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + normaAlteradora.ch_norma;


            if (Regex.Matches(texto, pattern).Count == 1)
            {
                var replacement = "$1\r\n<p ch_norma_info=\"" + normaAlteradora.ch_norma + "\"><a href=\"" + aux_href + "\" >(" + dsTextoRelacao + " pelo(a) " + dsNormaAlteradora + ")</a></p>";
                if (dsTextoRelacao == "legislação correlata")
                {
                    replacement = "<p ch_norma_info=\"" + normaAlteradora.ch_norma + "\"><a href=\"" + aux_href + "\" >Legislação correlata - " + dsNormaAlteradora + "</a></p>\r\n$1";
                }
                //else if (dsTextoRelacao == "suspenso(a) liminarmente") {
                //    replacement = "<p ch_norma_info=\"" + normaAlteradora.ch_norma + "\"><a href=\"" + aux_href + "\" >Suspensão - " + dsNormaAlteradora + "</a></p>\r\n$1";
                //}
                texto = Regex.Replace(texto, pattern, replacement);
            }

            //Caso o texto nao possua a epigrafe sera inseria para que possa ser criado o hyperlink no texto alterado. by Wemerson
            else if (Regex.Matches(texto, pattern).Count != 1)
            {

                StringBuilder textoBuid = new StringBuilder();

                textoBuid.Append("<h1 epigrafe=\"\"> </h1>").Append(texto);

                texto = textoBuid.ToString();

                Console.WriteLine("texto: " + texto);
                pattern = "(<h1.+?epigrafe=.+?>.+?</h1>)";

                if (Regex.Matches(texto, pattern).Count == 1)
                {
                    var replacement = "$1\r\n<p ch_norma_info=\"" + normaAlteradora.ch_norma + "\"><a href=\"" + aux_href + "\" >(" + dsTextoRelacao + " pelo(a) " + dsNormaAlteradora + ")</a></p>";
                    if (dsTextoRelacao == "legislação correlata")
                    {
                        replacement = "<p ch_norma_info=\"" + normaAlteradora.ch_norma + "\"><a href=\"" + aux_href + "\" >Legislação correlata - " + dsNormaAlteradora + "</a></p>\r\n$1";
                    }

                    texto = Regex.Replace(texto, pattern, replacement);
                }
            }
            else
            {
                texto = "";
            }
            return texto;
        }

        public string AcrescentarInformacaoNoTextoDaNormaAlteradora(NormaOV normaAlterada, string idFileNormaAlteradora, string dsTextoRelacao)
        {
            var htmlFile = new UtilArquivoHtml();
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
                    var replacement = "<p ch_norma_info=\"" + normaAlterada.ch_norma + "\"><a href=\"" + aux_href + "\" >Legislação correlata - " + dsNormaAlterada + "</a></p>\r\n$1";
                    texto = Regex.Replace(texto, pattern, replacement);
                } else {
                    texto = "";
                }
            } 
            //else if (dsTextoRelacao == "suspenso(a) liminarmente") {
            //    texto = htmlFile.GetHtmlFile(idFileNormaAlteradora, "sinj_norma", null);
            //    var pattern = "(<h1.+?epigrafe=.+?>.+?</h1>)";

            //    var nameFileNormaAlterada = normaAlterada.getNameFileArquivoVigente();
            //    var dsNormaAlterada = normaAlterada.getDescricaoDaNorma();

            //    var aux_href = !string.IsNullOrEmpty(nameFileNormaAlterada) ? ("(_link_sistema_)Norma/" + normaAlterada.ch_norma + "/" + nameFileNormaAlterada) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + normaAlterada.ch_norma;

            //    if (Regex.Matches(texto, pattern).Count == 1)
            //    {
            //        var replacement = "<p ch_norma_info=\"" + normaAlterada.ch_norma + "\"><a href=\"" + aux_href + "\" >Suspensão - " + dsNormaAlterada + "</a></p>\r\n$1";
            //        texto = Regex.Replace(texto, pattern, replacement);
            //    }
            //    else
            //    {
            //        texto = "";
            //    }
            //}
            return texto;
        }

        # endregion

        #region Vide Arquivos Remover Alteração

        /// <summary>
        /// Verifica se as normas possuem alterações nos seus respectivos arquivos html, provenientes dos vides que estão sendo desfeitos
        /// </summary>
        /// <param name="normaAlteradora"></param>
        /// <param name="normaAlterada"></param>
        /// <param name="videAlteradorDesfazer"></param>
        /// <param name="videAlteradoDesfazer"></param>
        /// <param name="nmSituacaoAnterior"></param>
        /// <returns>Contém os novos id_file dos respectivos arquivos alterados e salvos novamente</returns>
        public Dictionary<string, string> VerificarDispositivosEDesfazerAltercaoNosTextosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlteradorDesfazer, Vide videAlteradoDesfazer, string nmSituacaoAnterior, string nm_login_usuario)
        {
            var dictionaryIdFiles = new Dictionary<string, string>();
            if (videAlteradorDesfazer.caput_norma_vide != null && videAlteradoDesfazer.caput_norma_vide != null && videAlteradorDesfazer.caput_norma_vide.caput != null &&
                videAlteradoDesfazer.caput_norma_vide.caput != null && videAlteradorDesfazer.caput_norma_vide.caput.Length > 0 && videAlteradoDesfazer.caput_norma_vide.caput.Length > 0)
            {
                SalvarTextoAntigoDaNorma(normaAlteradora, videAlteradorDesfazer, nm_login_usuario);
                SalvarTextoAntigoDaNorma(normaAlterada, videAlteradoDesfazer, nm_login_usuario);
                dictionaryIdFiles = RemoverAlteracaoComDispositivosNosArquivosDasNormas(normaAlteradora, normaAlterada, videAlteradorDesfazer.caput_norma_vide, videAlteradoDesfazer.caput_norma_vide);
            }
            else if (videAlteradoDesfazer.caput_norma_vide != null && videAlteradoDesfazer.caput_norma_vide.caput != null && videAlteradoDesfazer.caput_norma_vide.caput.Length > 0)
            {
                SalvarTextoAntigoDaNorma(normaAlterada, videAlteradoDesfazer, nm_login_usuario);
                dictionaryIdFiles = RemoverAlteracaoComDispositivoAlteradoNosArquivosDasNormas(normaAlterada, normaAlteradora, videAlteradoDesfazer.caput_norma_vide);
            }
            else if (videAlteradorDesfazer.caput_norma_vide != null && videAlteradorDesfazer.caput_norma_vide.caput != null && videAlteradorDesfazer.caput_norma_vide.caput.Length > 0)
            {
                SalvarTextoAntigoDaNorma(normaAlteradora, videAlteradorDesfazer, nm_login_usuario);
                SalvarTextoAntigoDaNorma(normaAlterada, videAlteradoDesfazer, nm_login_usuario);
                dictionaryIdFiles = RemoverAlteracaoComDispositivoAlteradorNosArquivosDasNormas(normaAlteradora, normaAlterada, videAlteradorDesfazer.caput_norma_vide, nmSituacaoAnterior);
            }
            else
            {
                SalvarTextoAntigoDaNorma(normaAlteradora, videAlteradorDesfazer, nm_login_usuario);
                SalvarTextoAntigoDaNorma(normaAlterada, videAlteradoDesfazer, nm_login_usuario);
                dictionaryIdFiles = RemoverAlteracaoSemDispositivosNosArquivosDasNormas(normaAlteradora, normaAlterada, videAlteradoDesfazer, nmSituacaoAnterior);
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
            var upload = new UtilArquivoHtml();
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
                    if (PathPut(normaAlteradora._metadata.id_doc, "ar_atualizado", retornoFileAlteradora, null) == "UPDATED")
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
                    if (PathPut(normaAlterada._metadata.id_doc, "ar_atualizado", retornoFileAlterada, null) == "UPDATED")
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
            var upload = new UtilArquivoHtml();
            var listOp = new List<opMode<object>>();

            var dictionaryIdFiles = new Dictionary<string, string>();

            var arquivoNormaVideAlteradora = RemoverLinkNoTextoDaNormaAlteradora(normaAlteradora, caputAlteradorDesfazer, normaAlterada.ch_norma);

            if (arquivoNormaVideAlteradora != "")
            {
                var retornoFileAlteradora = upload.AnexarHtml(arquivoNormaVideAlteradora, normaAlteradora.ar_atualizado.filename, "sinj_norma");
                var oFileAlteradora = JSON.Deserializa<ArquivoOV>(retornoFileAlteradora);
                if (!string.IsNullOrEmpty(oFileAlteradora.id_file))
                {
                    if (PathPut(normaAlteradora._metadata.id_doc, "ar_atualizado", retornoFileAlteradora, null) == "UPDATED")
                    {
                        dictionaryIdFiles.Add("id_file_alterador", oFileAlteradora.id_file);
                    }
                }
            }

            var auxDsTextoParaAlteradorAnterior = caputAlteradorDesfazer.ds_texto_para_alterador_aux;
            var auxNmSituacaoAlteradaAtual = normaAlterada.nm_situacao.ToLower();
            var auxNmSituacaoAnterior = nmSituacaoAnterior.ToLower();

            var idFileNormaAlterada = normaAlterada.getIdFileArquivoVigente();
            var nameFileNormaAlterada = normaAlterada.getNameFileArquivoVigente();
            if (!string.IsNullOrEmpty(idFileNormaAlterada))
            {
                var arquivoNormaVideAlterada = "";

                // NOTE: Se a situação da norma foi alterada pela remoção do vide em questão, então não deve alterar o texto, pois essa alteração 
                // afeta o texto inteiro. By Questor
                if (auxNmSituacaoAlteradaAtual != auxNmSituacaoAnterior && 
                    (auxDsTextoParaAlteradorAnterior == "revigorado" || auxDsTextoParaAlteradorAnterior == "repristinado") &&
                    (auxNmSituacaoAlteradaAtual == "revogado" ||
                     auxNmSituacaoAlteradaAtual == "anulado" ||
                     auxNmSituacaoAlteradaAtual == "extinta" ||
                     // auxNmSituacaoAlteradaAtual == "inconstitucional" ||
                     auxNmSituacaoAlteradaAtual == "inconstitucional" ||
                     auxNmSituacaoAlteradaAtual == "cancelada" ||
                     auxNmSituacaoAlteradaAtual == "suspenso" ||
                     auxNmSituacaoAlteradaAtual == "sustado"))
                {
                    if (auxDsTextoParaAlteradorAnterior == "revigorado")
                    {
                        arquivoNormaVideAlterada = RemoverRevigoracaoNoTextoCompletoDaNormaAlterada(normaAlteradora.ch_norma, idFileNormaAlterada, auxDsTextoParaAlteradorAnterior);
                    }
                    else if (auxDsTextoParaAlteradorAnterior == "repristinado")
                    {
                        arquivoNormaVideAlterada = RemoverRepristinacaoNoTextoCompletoDaNormaAlterada(normaAlteradora.ch_norma, idFileNormaAlterada, auxDsTextoParaAlteradorAnterior);
                    }
                }
                else if (auxNmSituacaoAlteradaAtual != auxNmSituacaoAnterior && ((auxNmSituacaoAnterior == "revogado" && auxDsTextoParaAlteradorAnterior == "revogado") ||
                    (auxNmSituacaoAnterior == "anulado" && auxDsTextoParaAlteradorAnterior == "anulado") ||
                    (auxNmSituacaoAnterior == "extinta" && auxDsTextoParaAlteradorAnterior == "extinta") ||
                    (auxNmSituacaoAnterior == "inconstitucional" && auxDsTextoParaAlteradorAnterior == "declarado inconstitucional") ||
                    (auxNmSituacaoAnterior == "inconstitucional" && auxDsTextoParaAlteradorAnterior == "declarado(a) inconstitucional") ||
                    (auxNmSituacaoAnterior == "inconstitucional" && auxDsTextoParaAlteradorAnterior == "julgada procedente") ||
                    (auxNmSituacaoAnterior == "cancelada" && auxDsTextoParaAlteradorAnterior == "cancelada") ||
                    (auxNmSituacaoAnterior == "suspenso" && auxDsTextoParaAlteradorAnterior == "suspenso(a) liminarmente") ||
                    //(auxNmSituacaoAnterior == "suspenso" && auxDsTextoParaAlteradorAnterior == "suspenso totalmente") ||
                    //(auxNmSituacaoAnterior == "suspenso" && auxDsTextoParaAlteradorAnterior == "suspenso") ||
                    (auxNmSituacaoAnterior == "sustado" && auxDsTextoParaAlteradorAnterior == "sustado")))
                {
                    arquivoNormaVideAlterada = RemoverAlteracaoNoTextoCompletoDaNormaAlterada(normaAlteradora.ch_norma, idFileNormaAlterada, auxDsTextoParaAlteradorAnterior);
                }
                else if (auxDsTextoParaAlteradorAnterior == "ratificado" ||
                    auxDsTextoParaAlteradorAnterior == "reeditado" ||
                    auxDsTextoParaAlteradorAnterior == "regulamentado" ||
                    auxDsTextoParaAlteradorAnterior == "prorrogado" ||
                    auxDsTextoParaAlteradorAnterior == "legislação correlata")
                {
                    arquivoNormaVideAlterada = RemoverInformacaoNoTextoDaNormaAlterada(normaAlteradora.ch_norma, idFileNormaAlterada, auxDsTextoParaAlteradorAnterior);
                }

                if (arquivoNormaVideAlterada != "")
                {
                    var retornoFileAlterada = upload.AnexarHtml(arquivoNormaVideAlterada, nameFileNormaAlterada, "sinj_norma");
                    var oFileAlterada = JSON.Deserializa<ArquivoOV>(retornoFileAlterada);
                    if (!string.IsNullOrEmpty(oFileAlterada.id_file))
                    {
                        if (PathPut(normaAlterada._metadata.id_doc, "ar_atualizado", retornoFileAlterada, null) == "UPDATED")
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
            var upload = new UtilArquivoHtml();
            var pesquisa = new Pesquisa();

            var dictionaryIdFiles = new Dictionary<string, string>();

            var nameFileNormaAlterada = normaAlterada.getNameFileArquivoVigente();

            var arquivoNormaVideAlterada = RemoverAlteracaoNoTextoDaNormaAlterada(normaAlterada, normaAlteradora, caputAlteradoDesfazer);
            if (!string.IsNullOrEmpty(arquivoNormaVideAlterada))
            {
                var retornoFileAlterada = upload.AnexarHtml(arquivoNormaVideAlterada, caputAlteradoDesfazer.filename, "sinj_norma");
                var oFileAlterada = JSON.Deserializa<ArquivoOV>(retornoFileAlterada);
                if (!string.IsNullOrEmpty(oFileAlterada.id_file))
                {
                    if (PathPut(normaAlterada._metadata.id_doc, "ar_atualizado", retornoFileAlterada, null) == "UPDATED")
                    {
                        dictionaryIdFiles.Add("id_file_alterado", oFileAlterada.id_file);
                    }
                }
            }
            return dictionaryIdFiles;
        }

        public Dictionary<string, string> RemoverAlteracaoSemDispositivosNosArquivosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlteradoDesfazer, string nmSituacaoAnterior)
        {
            var upload = new UtilArquivoHtml();
            var pesquisa = new Pesquisa();

            var dictionaryIdFiles = new Dictionary<string, string>();

            var idFileNormaAlteradora = normaAlteradora.getIdFileArquivoVigente();
            var nameFileNormaAlteradora = normaAlteradora.getNameFileArquivoVigente();

            var idFileNormaAlterada = normaAlterada.getIdFileArquivoVigente();
            var name_file_norma_alterada = normaAlterada.getNameFileArquivoVigente();

            if (!string.IsNullOrEmpty(idFileNormaAlterada))
            {
                var auxNmSituacaoAnterior = nmSituacaoAnterior.ToLower();
                var auxDsTextoAlteradorDesfazer = videAlteradoDesfazer.ds_texto_relacao.ToLower();
                var auxNmSituacaoAtual = normaAlterada.nm_situacao.ToLower();

                var arquivoNormaVideAlterada = "";
                if (auxNmSituacaoAtual != auxNmSituacaoAnterior &&
                    (auxDsTextoAlteradorDesfazer == "revigorado" || auxDsTextoAlteradorDesfazer == "repristinado") &&
                    (auxNmSituacaoAtual == "revogado" ||
                     auxNmSituacaoAtual == "anulado" ||
                     auxNmSituacaoAtual == "extinta" ||
                     auxNmSituacaoAtual == "inconstitucional" ||
                     // auxNmSituacaoAtual == "inconstitucional" ||
                     auxNmSituacaoAtual == "cancelada" ||
                     auxNmSituacaoAtual == "suspenso" ||
                     auxNmSituacaoAtual == "sustado"))
                {
                    if (auxDsTextoAlteradorDesfazer == "revigorado")
                    {
                        arquivoNormaVideAlterada = RemoverRevigoracaoNoTextoCompletoDaNormaAlterada(normaAlteradora.ch_norma, idFileNormaAlterada, auxDsTextoAlteradorDesfazer);
                    }
                    else if (auxDsTextoAlteradorDesfazer == "repristinado")
                    {
                        arquivoNormaVideAlterada = RemoverRepristinacaoNoTextoCompletoDaNormaAlterada(normaAlteradora.ch_norma, idFileNormaAlterada, auxDsTextoAlteradorDesfazer);
                    }
                }
                else if (UtilVides.EhAlteracaoCompleta(auxNmSituacaoAnterior, auxDsTextoAlteradorDesfazer))
                {
                    arquivoNormaVideAlterada = RemoverAlteracaoNoTextoCompletoDaNormaAlterada(normaAlteradora.ch_norma, idFileNormaAlterada, auxDsTextoAlteradorDesfazer);
                }
                else if (UtilVides.EhLegislacaoCorrelata(auxDsTextoAlteradorDesfazer))
                {
                    arquivoNormaVideAlterada = RemoverInformacaoNoTextoDaNormaAlterada(normaAlteradora.ch_norma, idFileNormaAlterada, auxDsTextoAlteradorDesfazer);
                }

                if (!string.IsNullOrEmpty(arquivoNormaVideAlterada))
                {
                    var retornoFileAlterada = upload.AnexarHtml(arquivoNormaVideAlterada, name_file_norma_alterada, "sinj_norma");
                    var oFileAlterada = JSON.Deserializa<ArquivoOV>(retornoFileAlterada);
                    if (!string.IsNullOrEmpty(oFileAlterada.id_file))
                    {
                        if (PathPut(normaAlterada._metadata.id_doc, "ar_atualizado", retornoFileAlterada, null) == "UPDATED")
                        {
                            dictionaryIdFiles.Add("id_file_alterado", oFileAlterada.id_file);
                        }

                        var arquivoNormaAlteradora = RemoverInformacaoNoTextoDaNormaAlteradora(normaAlterada, idFileNormaAlteradora, auxDsTextoAlteradorDesfazer);
                        if (!string.IsNullOrEmpty(arquivoNormaAlteradora))
                        {
                            var retornoFileAlteradora = upload.AnexarHtml(arquivoNormaAlteradora, nameFileNormaAlteradora, "sinj_norma");

                            var oFileAlteradora = JSON.Deserializa<ArquivoOV>(retornoFileAlteradora);
                            if (!string.IsNullOrEmpty(oFileAlteradora.id_file))
                            {
                                if (PathPut(normaAlteradora._metadata.id_doc, "ar_atualizado", retornoFileAlteradora, null) == "UPDATED")
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
        /// <param name="normaAlteradora"></param>
        /// <param name="caputAlteradora"></param>
        /// <param name="caputAlterada"></param>
        /// <returns></returns>
        public string RemoverLinkNoTextoDaNormaAlteradora(NormaOV normaAlteradora, Caput caputAlteradora, Caput caputAlterada)
        {
            var arquivo_norma_vide_alteradora = "";
            //Na teoria se uma norma tem vide com dispositivos o ar_atualizado.id_file não deveria ser vazio, mas em alguns casos tem surgido essa anomalia devido à alguma falha na inclusão do vide com dispositivos
            //Até o momento foi percebido que quando um dispositivo selecionado possui quebra de linha, <br/>, o sistema não consegue aplicar o Regex da alteração.
            if (!string.IsNullOrEmpty(normaAlteradora.ar_atualizado.id_file))
            {
                var htmlFile = new UtilArquivoHtml();
                arquivo_norma_vide_alteradora = htmlFile.GetHtmlFile(normaAlteradora.ar_atualizado.id_file, "sinj_norma", null);

                var pattern_caput = "";
                switch (caputAlterada.nm_relacao_aux)
                {
                    case "acrescimo":
                        pattern_caput = caputAlterada.caput[0] + "_add_0";
                        break;
                    case "renumeração":
                        pattern_caput = caputAlterada.caput[0] + "_renum";
                        break;
                    case "revogação":
                        pattern_caput = caputAlterada.caput[0] + "_replaced";
                        break;
                    default:
                        pattern_caput = caputAlterada.caput[0];
                        break;
                }
                var pattern = "(<p.+?linkname=\"" + caputAlteradora.caput[0] + "\".*?>)(.*?)<a href=\"\\(_link_sistema_\\)Norma/" + caputAlterada.ch_norma + '/' + caputAlterada.filename + "#" + pattern_caput + "\">" + UtilVides.EscapeCharsInToPattern(caputAlteradora.link) + "</a>(.*?)</p>";
                var matches = Regex.Matches(arquivo_norma_vide_alteradora, pattern);
                if (matches.Count == 1)
                {
                    var replacement = matches[0].Groups[1].Value + matches[0].Groups[2].Value + caputAlteradora.link + matches[0].Groups[3].Value + "</p>";
                    arquivo_norma_vide_alteradora = Regex.Replace(arquivo_norma_vide_alteradora, pattern, replacement);
                }
                else
                {
                    arquivo_norma_vide_alteradora = "";
                }
            }
            return arquivo_norma_vide_alteradora;
        }

        public string RemoverLinkNoTextoDaNormaAlteradora(NormaOV normaAlteradora, Caput caputAlteradoraDesfazer, string chNormaAlterada)
        {
            var arquivo_norma_vide_alteradora = "";
            //Na teoria se uma norma tem vide com dispositivos o ar_atualizado.id_file não deveria ser vazio, mas em alguns casos tem surgido essa anomalia devido à alguma falha na inclusão do vide com dispositivos
            //Até o momento foi percebido que quando um dispositivo selecionado possui quebra de linha, <br/>, o sistema não consegue aplicar o Regex da alteração.
            if (!string.IsNullOrEmpty(normaAlteradora.ar_atualizado.id_file))
            {
                var htmlFile = new UtilArquivoHtml();
                arquivo_norma_vide_alteradora = htmlFile.GetHtmlFile(normaAlteradora.ar_atualizado.id_file, "sinj_norma", null);
                var pattern = "(<p.+?linkname=\"" + caputAlteradoraDesfazer.caput[0] + "\".*?>)(.*?)<a href=\"\\(_link_sistema_\\)Norma/" + chNormaAlterada + "/.+?\">" + UtilVides.EscapeCharsInToPattern(caputAlteradoraDesfazer.link) + "</a>(.*?)</p>";
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
            }
            return arquivo_norma_vide_alteradora;
        }

        public string RemoverAlteracaoNoTextoDaNormaAlterada(NormaOV norma_alterada, Caput caputAlteradoDesfazer, Caput _caput_alteradora_desfazer)
        {
            var texto = "";
            //Na teoria se uma norma tem vide com dispositivos o ar_atualizado.id_file não deveria ser vazio, mas em alguns casos tem surgido essa anomalia devido à alguma falha na inclusão do vide com dispositivos
            //Até o momento foi percebido que quando um dispositivo selecionado possui quebra de linha, <br/>, o sistema não consegue aplicar o Regex da alteração.
            if (caputAlteradoDesfazer.caput.Length == caputAlteradoDesfazer.texto_antigo.Length && !string.IsNullOrEmpty(norma_alterada.ar_atualizado.id_file))
            {
                texto = new UtilArquivoHtml().GetHtmlFile(norma_alterada.ar_atualizado.id_file, "sinj_norma", null);
                var bAlterou = false;
                var pattern = "";
                var replacement = "";
                var ds_link_alterador = "";
                for (var i = 0; i < caputAlteradoDesfazer.caput.Length; i++)
                {
                    switch (caputAlteradoDesfazer.nm_relacao_aux)
                    {
                        case "acrescimo":
                            pattern = "<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i] + "_add_.+?\".*?>.*? <a class=\"link_vide\".*?>.+?</a></p>";
                            replacement = "";
                            break;
                        case "renumeração":
                            ds_link_alterador = "\\(.*?" + caputAlteradoDesfazer.ds_texto_para_alterador_aux + ".*?pelo\\(a\\) " + _caput_alteradora_desfazer.ds_norma + "\\)";
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "_renum(\".*?>.*?<a.+?name=\")" + caputAlteradoDesfazer.caput[i] + "_renum(\".*?></a>.*?)" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.texto_novo[i]) + "(.*?) <a class=\"link_vide\".*?>" + ds_link_alterador + "</a>(.*?)</p>";
                            replacement = "$1" + caputAlteradoDesfazer.caput[i] + "$2" + caputAlteradoDesfazer.caput[i] + "$3" + caputAlteradoDesfazer.texto_antigo[i] + "$4$5</p>";
                            break;
                        case "revigoração":
                            ds_link_alterador = "\\(.*?" + caputAlteradoDesfazer.ds_texto_para_alterador_aux + ".*?pelo\\(a\\) .+?\\)";
                            pattern = "(<p.+?linkname=\"" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "\".*?)replaced_by_disabled=\"(.*?)\"(.*?)(<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "\".*?></a>.*?)( <a class=\"link_vide\".*?>.*?)<a class=\"link_vide\".*?>" + ds_link_alterador + "</a>(.*?)</p>";
                            replacement = "$1replaced_by=\"$2\"$3<s>$4</s>$5$6</p>";
                            if (!string.IsNullOrEmpty(caputAlteradoDesfazer.texto_novo[i]))
                            {
                                pattern = "<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i].Replace("_replaced", "") + "\".*?replaced_by_disabled=\".*?\".*?</p>";
                                replacement = "";
                            }
                            break;
                        case "repristinação":
                            ds_link_alterador = "\\(.*?" + caputAlteradoDesfazer.ds_texto_para_alterador_aux + ".*?pelo\\(a\\) .+?\\)";
                            pattern = "(<p.+?linkname=\"" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "\".*?)replaced_by_disabled=\"(.*?)\"(.*?)(<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "\".*?></a>.*?)( <a class=\"link_vide\".*?>.*?)<a class=\"link_vide\".*?>" + ds_link_alterador + "</a>(.*?)</p>";
                            replacement = "$1replaced_by=\"$2\"$3<s>$4</s>$5$6</p>";
                            if (!string.IsNullOrEmpty(caputAlteradoDesfazer.texto_novo[i]))
                            {
                                pattern = "<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i].Replace("_replaced", "") + "\".*?replaced_by_disabled=\".*?\".*?</p>";
                                replacement = "";
                            }
                            break;
                        case "prorrogação":
                        case "ratificação":
                        case "regulamentação":
                        case "ressalva":
                        case "recepção":
                        case "legislação correlata":
                            ds_link_alterador = "\\(.*?" + caputAlteradoDesfazer.ds_texto_para_alterador_aux + ".*pelo\\(a\\) " + _caput_alteradora_desfazer.ds_norma + "\\)";
                            if (caputAlteradoDesfazer.nm_relacao_aux == "legislação correlata")
                            {
                                ds_link_alterador = "\\(Legislação correlata - " + _caput_alteradora_desfazer.ds_norma + "\\)";
                            }
                            pattern = "(<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i] + "\".*?<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "\".*?></a>.*?) <a class=\"link_vide\".*?>" + ds_link_alterador + "</a>(.*?)</p>";
                            replacement = "$1$2</p>";
                            break;
                        default:
                            ds_link_alterador = "\\(.*?" + UtilVides.getRelacaoParaTextoAlterador(caputAlteradoDesfazer.ds_texto_para_alterador_aux, true) + "pelo\\(a\\) " + _caput_alteradora_desfazer.ds_norma + "\\)";
                            if (!string.IsNullOrEmpty(caputAlteradoDesfazer.texto_novo[i]))
                            {
                                pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "_replaced.*?(\".*?)replaced_by=\"" + _caput_alteradora_desfazer.ch_norma + "\"(.*?)<s>(.*?)<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "_replaced.*?\".*?></a>(.*?)</s>(.*?)</p>\r\n<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i] + "\".*?>.*?" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.texto_novo[i]) + ".*? <a class=\"link_vide\".*?>.+?</a></p>";
                                replacement = "$1" + caputAlteradoDesfazer.caput[i] + "$2$3$4<a id=\"" + caputAlteradoDesfazer.caput[i] + "\" name=\"" + caputAlteradoDesfazer.caput[i] + "\"></a>" + caputAlteradoDesfazer.texto_antigo[i] + "$6</p>";
                            }
                            else
                            {
                                pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "_replaced.*?(\".*?)replaced_by=\"" + _caput_alteradora_desfazer.ch_norma + "\"(.*?)<s>(.*?)<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "_replaced.*?\".*?></a>(.*?)</s>(.*?) <a class=\"link_vide\".*?>" + ds_link_alterador + "</a>(.*?)</p>";
                                replacement = "$1" + caputAlteradoDesfazer.caput[i] + "$2$3$4<a id=\"" + caputAlteradoDesfazer.caput[i] + "\" name=\"" + caputAlteradoDesfazer.caput[i] + "\"></a>" + caputAlteradoDesfazer.texto_antigo[i] + "$6$7</p>";
                            }
                            break;
                    }
                    var teste = Regex.Matches(texto, pattern).Count;
                    if (Regex.Matches(texto, pattern).Count == 1 || (caputAlteradoDesfazer.nm_relacao_aux == "acrescimo" && Regex.Matches(texto, pattern).Count > 1))
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
            var texto = "";
            //Na teoria se uma norma tem vide com dispositivos o ar_atualizado.id_file não deveria ser vazio, mas em alguns casos tem surgido essa anomalia devido à alguma falha na inclusão do vide com dispositivos
            //Até o momento foi percebido que quando um dispositivo selecionado possui quebra de linha, <br/>, o sistema não consegue aplicar o Regex da alteração.
            if (!string.IsNullOrEmpty(normaAlterada.ar_atualizado.id_file))
            {
                texto = new UtilArquivoHtml().GetHtmlFile(normaAlterada.ar_atualizado.id_file, "sinj_norma", null);

                var nameFileNormaAlteradora = normaAlteradora.getNameFileArquivoVigente();
                var dsNormaAlteradora = normaAlteradora.getDescricaoDaNorma();

                var pattern = "";
                var replacement = "";
                var ds_link_alterador = "";
                //define o link da norma alteradora, se possui nameFileNormaAlteradora então a norma tem arquivo se não então o link é para os detalhes da norma
                var aux_href = UtilVides.EscapeCharsInToPattern(!string.IsNullOrEmpty(nameFileNormaAlteradora) ? ("(_link_sistema_)Norma/" + normaAlteradora.ch_norma + '/' + nameFileNormaAlteradora) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + normaAlteradora.ch_norma);

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
                            ds_link_alterador = "\\(.*?" + caputAlteradoDesfazer.ds_texto_para_alterador_aux + ".*pelo\\(a\\) .+?\\)";
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "_renum(\".*?>.*?<a.+?name=\")" + caputAlteradoDesfazer.caput[i] + "_renum(\".*?></a>.*?)" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.texto_novo[i]) + "(.*?) <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a>(.*?)</p>";
                            replacement = "$1" + caputAlteradoDesfazer.caput[i] + "$2" + caputAlteradoDesfazer.caput[i] + "$3" + caputAlteradoDesfazer.texto_antigo[i] + "$4$5</p>";
                            break;
                        case "revigoração":
                            ds_link_alterador = "\\(.*?" + caputAlteradoDesfazer.ds_texto_para_alterador_aux + ".*pelo\\(a\\) .+?\\)";
                            pattern = "(<p.+?linkname=\"" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "\".*?)replaced_by_disabled=\"(.*?)\"(.*?)(<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "\".*?></a>.*?)( <a class=\"link_vide\".*?>.*?)<a class=\"link_vide\".*?>" + ds_link_alterador + "</a>(.*?)</p>";
                            replacement = "$1replaced_by=\"$2\"$3<s>$4</s>$5</p>";
                            if (!string.IsNullOrEmpty(caputAlteradoDesfazer.texto_novo[i]))
                            {
                                pattern = "<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i].Replace("_replaced", "") + "\".*?replaced_by_disabled=\".*?\".*?</p>";
                                replacement = "";
                            }
                            break;
                        case "repristinação":
                            ds_link_alterador = "\\(.*?" + caputAlteradoDesfazer.ds_texto_para_alterador_aux + ".*pelo\\(a\\) .+?\\)";
                            pattern = "(<p.+?linkname=\"" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "\".*?)replaced_by_disabled=\"(.*?)\"(.*?)(<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "\".*?></a>.*?)( <a class=\"link_vide\".*?>.*?)<a class=\"link_vide\".*?>" + ds_link_alterador + "</a>(.*?)</p>";
                            replacement = "$1replaced_by=\"$2\"$3<s>$4</s>$5</p>";
                            if (!string.IsNullOrEmpty(caputAlteradoDesfazer.texto_novo[i]))
                            {
                                pattern = "<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i].Replace("_replaced", "") + "\".*?replaced_by_disabled=\".*?\".*?</p>";
                                replacement = "";
                            }
                            break;
                        case "prorrogação":
                        case "ratificação":
                        case "regulamentação":
                        case "ressalva":
                        case "recepção":
                        case "legislação correlata":
                            ds_link_alterador = "\\(.*?" + caputAlteradoDesfazer.ds_texto_para_alterador_aux + ".*pelo\\(a\\) " + dsNormaAlteradora + "\\)";
                            if (caputAlteradoDesfazer.nm_relacao_aux == "legislação correlata")
                            {
                                // ds_link_alterador = "\\(Legislação correlata - " + dsNormaAlteradora + "\\)";
                                ds_link_alterador = Regex.Escape("(Legislação correlata - " + dsNormaAlteradora + ")");
                            }
                            pattern = "(<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i] + "\".*?<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "\".*?></a>.*?) <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a>(.*?)</p>";
                            replacement = "$1$2</p>";
                            break;
                        case "suspensão":
                            ds_link_alterador = "\\(.*?" + caputAlteradoDesfazer.ds_texto_para_alterador_aux + ".*pelo\\(a\\) " + dsNormaAlteradora + "\\)";
                            if (caputAlteradoDesfazer.nm_relacao_aux == "suspensão")
                            {
                                ds_link_alterador = Regex.Escape("(Suspenso(a) liminarmente - " + dsNormaAlteradora + ")");
                                // ds_link_alterador = "\\(Suspenso\\(a\\) liminarmente \\- ADI 8970\\-7 de 21\\/03\\/2017\\)";
                            }
                            pattern = "(<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i] + "\".*?<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "\".*?></a>.*?) <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a>(.*?)</p>";
                            replacement = "$1$2</p>";
                            break;
                        default:
                            ds_link_alterador = "\\(" + UtilVides.getRelacaoParaTextoAlterador(caputAlteradoDesfazer.ds_texto_para_alterador_aux, true) + "pelo\\(a\\) " + dsNormaAlteradora + "\\)";
                            if (!string.IsNullOrEmpty(caputAlteradoDesfazer.texto_novo[i]))
                            {
                                pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "_replaced(\".*?)replaced_by=\"" + normaAlteradora.ch_norma + "\"(.*?)<s>(.*?)<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "_replaced\".*?></a>(.*?)</s>(.*?)</p>\r\n<p.+?linkname=\"" + caputAlteradoDesfazer.caput[i] + "\".*?>.*?" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.texto_novo[i]) + ".*? <a class=\"link_vide\".*?>.+?</a></p>";
                                replacement = "$1" + caputAlteradoDesfazer.caput[i] + "$2$3$4<a id=\"" + caputAlteradoDesfazer.caput[i] + "\" name=\"" + caputAlteradoDesfazer.caput[i] + "\"></a>" + caputAlteradoDesfazer.texto_antigo[i] + "$6</p>";
                            }
                            else
                            {
                                pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(caputAlteradoDesfazer.caput[i]) + "_replaced(\".*?)replaced_by=\"" + normaAlteradora.ch_norma + "\"(.*?)<s>(.*?)<a.+?name=\"" + caputAlteradoDesfazer.caput[i] + "_replaced\".*?></a>(.*?)</s>(.*?) <a class=\"link_vide\" href=\"" + aux_href + "\">" + ds_link_alterador + "</a>(.*?)</p>";
                                replacement = "$1" + caputAlteradoDesfazer.caput[i] + "$2$3$4<a id=\"" + caputAlteradoDesfazer.caput[i] + "\" name=\"" + caputAlteradoDesfazer.caput[i] + "\"></a>" + caputAlteradoDesfazer.texto_antigo[i] + "$6$7</p>";
                            }
                            break;
                    }
                    var teste = Regex.Matches(texto, pattern).Count;
                    if (Regex.Matches(texto, pattern).Count == 1 || (caputAlteradoDesfazer.nm_relacao_aux == "acrescimo" && Regex.Matches(texto, pattern).Count > 1))
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

        public string RemoverAlteracaoNoTextoCompletoDaNormaAlterada(string chNormaAlteradora, string idFileNormaAlterada, string dsTextoParaAlterador)
        {
            var texto = new UtilArquivoHtml().GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);
            bool replacementOccurred = false;

            // NOTE: Essa regra serve para remover da parte inferior da "epígrafe" informação sobre a norma 
            // ("chNormaAlteradora") que ensejou a alteração completa. By Questor
            var pattern2a = "\r\n<p.+?ch_norma_alteracao_completa=\"" + chNormaAlteradora + "\".*?><a.+?>\\(.+?\\)</a></p>";

            Regex rx0 = new Regex(pattern2a);
            if (rx0.Matches(texto).Count == 1)
            {
                texto = rx0.Replace(texto, "");
                replacementOccurred = true;
            }

            // NOTE: Ignora os paragrafos com atributos 'replaced_by' ou 'nota', aceitando todos os outros replaced_by 
            // indica que um dispositivo especifico foi alterado por uma norma, então a alteração a ser desfeita deve ser 
            // a que feita no texto completo, e não as que foram feitas nos dispositivos (parcialidade) e a nota é só um 
            // texto inserido pelos cadastradores de texto e não faz parte do texto da norma. By Questor
            var pattern1 = "(?!<p.+(?:replaced_by=|nota=).+>)(<p.+?>)<s>(.+?)</s></p>";

            Regex rx1 = new Regex(pattern1);
            var pattern2 = "\r\n<p.*?><a.+?/" + chNormaAlteradora + "/.+?>\\(" + dsTextoParaAlterador + ".+?\\)</a></p>";
            Regex rx2 = new Regex(pattern2, RegexOptions.Singleline);
            if (rx1.Matches(texto).Count > 0 || rx2.Matches(texto).Count == 1)
            {
                var replacement1 = "$1$2</p>";
                var replacement2 = "";
                texto = rx1.Replace(texto, replacement1);
                texto = rx2.Replace(texto, replacement2);
                replacementOccurred = true;
            }
            if (!replacementOccurred)
            {
                texto = "";
            }
            return texto;
        }

        //public string RemoverAlteracaoNoTextoCompletoDaNormaAlterada(string chNormaAlteradora, string idFileNormaAlterada, string dsTextoParaAlterador)
        //{

        //    // TODO: A regra atual não está removendo as epígrafes. By Questor

        //    var texto = new UtilArquivoHtml().GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);

        //    // NOTE: Ignora os paragrafos com atributos 'replaced_by' ou 'nota', aceitando todos os outros replaced_by 
        //    // indica que um dispositivo especifico foi alterado por uma norma, então a alteração a ser desfeita deve ser 
        //    // a que feita no texto completo, e não as que foram feitas nos dispositivos (parcialidade) e a nota é só um 
        //    // texto inserido pelos cadastradores de texto e não faz parte do texto da norma. By Questor
        //    var pattern1 = "(?!<p.+(?:replaced_by=|nota=).+>)(<p.+?>)<s>(.+?)</s></p>";
        //    Regex rx1 = new Regex(pattern1);

        //    var pattern2 = "\r\n<p.*?><a.+?/" + chNormaAlteradora + "/.+?>\\(" + dsTextoParaAlterador + ".+?\\)</a></p>";
        //    Regex rx2 = new Regex(pattern2, RegexOptions.Singleline);

        //    if (rx1.Matches(texto).Count > 0 || rx2.Matches(texto).Count == 1)
        //    {
        //        var replacement1 = "$1$2</p>";
        //        var replacement2 = "";
        //        texto = rx1.Replace(texto, replacement1);
        //        texto = rx2.Replace(texto, replacement2);
        //    }
        //    else
        //    {
        //        texto = "";
        //    }
        //    return texto;
        //}

        public string RemoverRevigoracaoNoTextoCompletoDaNormaAlterada(string chNormaAlteradora, string idFileNormaAlterada, string dsTextoParaAlterador)
        {
            var texto = new UtilArquivoHtml().GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);

            // NOTE: Ignora os paragrafos com atributos 'replaced_by', 'nota', 'ch_norma_alteracao_completa' ou 'ch_norma_info', 
            // aceitando todos os outros.
            // . "replaced_by" indica que um dispositivo especifico foi alterado por uma norma, então a alteração a ser feita
            // não pode mexer nesses dispositivos que já foram alterados;
            // . "nota" é só um texto inserido pelos cadastradores de texto e não fazer parte do texto da norma;
            // . "ch_norma_alteracao_completa" é usado em links que quando há revogação, sustação, etc... (vides de alteração completa);
            // . "ch_norma_info" é usado em links por exemplo "leco".

            var pattern1 = "(?!<p.+(?:replaced_by=|nota=|ch_norma_alteracao_completa=|ch_norma_info=).+>)(<p.+?>)(.+?)</p>";
            Regex rx1 = new Regex(pattern1);

            var pattern2a = "\r\n<p.+?ch_norma_alteracao_completa=\"" + chNormaAlteradora + "\".*?><a.+?>\\(.+?\\)</a></p>";
            var pattern2b = "\r\n<p.*?><a.+?/" + chNormaAlteradora + "/.+?>\\(" + dsTextoParaAlterador + ".+?\\)</a></p>";

            Regex rx2 = new Regex(pattern2a);
            if (rx2.Matches(texto).Count <= 0)
            {
                rx2 = new Regex(pattern2b);
            }
            if (rx2.Matches(texto).Count == 1 || rx1.Matches(texto).Count > 0)
            {
                var replacement2 = "";
                texto = rx2.Replace(texto, replacement2);
                var replacement1 = "$1<s>$2</s></p>";
                texto = rx1.Replace(texto, replacement1);
            }
            else
            {
                texto = "";
            }
            return texto;
        }

        public string RemoverRepristinacaoNoTextoCompletoDaNormaAlterada(string chNormaAlteradora, string idFileNormaAlterada, string dsTextoParaAlterador)
        {
            var texto = new UtilArquivoHtml().GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);

            // NOTE: Ignora os paragrafos com atributos 'replaced_by', 'nota', 'ch_norma_alteracao_completa' ou 'ch_norma_info', 
            // aceitando todos os outros.
            // . "replaced_by" indica que um dispositivo especifico foi alterado por uma norma, então a alteração a ser feita
            // não pode mexer nesses dispositivos que já foram alterados;
            // . "nota" é só um texto inserido pelos cadastradores de texto e não fazer parte do texto da norma;
            // . "ch_norma_alteracao_completa" é usado em links que quando há revogação, sustação, etc... (vides de alteração completa);
            // . "ch_norma_info" é usado em links por exemplo "leco".

            var pattern1 = "(?!<p.+(?:replaced_by=|nota=|ch_norma_alteracao_completa=|ch_norma_info=).+>)(<p.+?>)(.+?)</p>";
            Regex rx1 = new Regex(pattern1);

            var pattern2a = "\r\n<p.+?ch_norma_alteracao_completa=\"" + chNormaAlteradora + "\".*?><a.+?>\\(.+?\\)</a></p>";
            var pattern2b = "\r\n<p.*?><a.+?/" + chNormaAlteradora + "/.+?>\\(" + dsTextoParaAlterador + ".+?\\)</a></p>";

            Regex rx2 = new Regex(pattern2a);
            if (rx2.Matches(texto).Count <= 0)
            {
                rx2 = new Regex(pattern2b);
            }
            if (rx2.Matches(texto).Count == 1 || rx1.Matches(texto).Count > 0)
            {
                var replacement2 = "";
                texto = rx2.Replace(texto, replacement2);
                var replacement1 = "$1<s>$2</s></p>";
                texto = rx1.Replace(texto, replacement1);
            }
            else
            {
                texto = "";
            }
            return texto;
        }

        public string RemoverInformacaoNoTextoDaNormaAlterada(string chNormaAlteradora, string idFileNormaAlterada, string dsTextoParaAlterador)
        {
            var texto = new UtilArquivoHtml().GetHtmlFile(idFileNormaAlterada, "sinj_norma", null);
            var pattern = "";
            if (dsTextoParaAlterador.ToLower() == "legislação correlata")
            {
                pattern = "<p.+?ch_norma_info=\"" + chNormaAlteradora + "\".*?><a.+?>Legislação correlata.+?</a></p>\r\n";
                if (Regex.Matches(texto, pattern).Count <= 0)
                {
                    pattern = "<p.*?><a.+?/" + chNormaAlteradora + "/.+?>Legislação correlata.+?</a></p>\r\n";
                }
            }
            else
            {
                pattern = "\r\n<p.+?ch_norma_info=\"" + chNormaAlteradora + "\".*?><a.+?>\\(.+?\\)</a></p>";
                if (Regex.Matches(texto, pattern).Count <= 0)
                {
                    pattern = "\r\n<p.*?><a.+?/" + chNormaAlteradora + "/.+?>\\(.+?\\)</a></p>";
                }
            }

            //Será retirado do texto o link se tiver mais de um link com o mesmo id_norma. by Wemerson

            if (Regex.Matches(texto, pattern).Count > 0)
            {
                texto = Regex.Replace(texto, pattern, "");
            }
            else
            {
                texto = "";
            }
            return texto;
        }

        public string RemoverInformacaoNoTextoDaNormaAlteradora(NormaOV normaAlterada, string idFileNormaAlteradora, string dsTextoRelacao)
        {
            var htmlFile = new UtilArquivoHtml();
            var texto = "";
            if (dsTextoRelacao == "legislação correlata")
            {
                texto = htmlFile.GetHtmlFile(idFileNormaAlteradora, "sinj_norma", null);
                //texto = RemoverInformacaoNoTextoDaNormaAlteradora(texto, normaAlterada, dsTextoRelacao);
                var nameFileNormaAlterada = normaAlterada.getNameFileArquivoVigente();
                var dsNormaAlterada = normaAlterada.getDescricaoDaNorma();

                var aux_href = !string.IsNullOrEmpty(nameFileNormaAlterada) ? ("(_link_sistema_)Norma/" + normaAlterada.ch_norma + "/" + nameFileNormaAlterada) : "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + normaAlterada.ch_norma;

                var pattern = "<p.+?ch_norma_info=\"" + normaAlterada.ch_norma + "\".*?><a.+?>Legislação correlata.+?</a></p>\r\n";
                if (Regex.Matches(texto, pattern).Count <= 0)
                {
                    pattern = "<p.*?><a href=\"" + aux_href + "\" >Legislação correlata.+?</a></p>\r\n";

                }

                //Será retirado do texto o link se tiver mais de um link com o mesmo id_norma. by Wemerson

                if (Regex.Matches(texto, pattern).Count > 0)
                {
                    texto = Regex.Replace(texto, pattern, "");
                }
                else
                {
                    texto = "";
                }
            }
            return texto;
        }

        #endregion

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
