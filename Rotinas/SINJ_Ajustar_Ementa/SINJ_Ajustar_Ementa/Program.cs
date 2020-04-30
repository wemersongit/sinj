using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;
using System.Text.RegularExpressions;

namespace SINJ_Ajustar_Ementa
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("======== ATUALIZAR DS_EMENTA ======== ");

            //ulong offset = 0;
            ulong total = 1;

            StringBuilder id_doc_erro = new StringBuilder();

            AcessoAD<NormaOVOld> _acessoAdNormaOld = new AcessoAD<NormaOVOld>("sinj_norma");
            AcessoAD<NormaOV> _acessoAdNorma = new AcessoAD<NormaOV>("sinj_norma");
            try
            {
                var sucesso = 0;
                var falha = 0;
                var normas_processadas = 0;
                NormaOV normaNova = new NormaOV();
                while (total > 0)
                {
                    try
                    {
                        var result = _acessoAdNormaOld.Consultar(new Pesquisa { literal = "dt_last_up::abstime < '07/03/2017 21:20:00'", limit = "100", order_by = new Order_By { asc = new string[] { "id_doc" } } });
                        total = result.result_count;
                        //offset += 100;
                        foreach (var norma in result.results)
                        {
                            var b_sucesso = false;
                            normas_processadas++;
                            while (!b_sucesso)
                            {
                                Console.Clear();
                                Console.SetCursorPosition(0, 0);
                                Console.WriteLine("Total de Normas: " + total);
                                Console.WriteLine("Normas processadas: " + normas_processadas);
                                Console.WriteLine("Normas com sucesso: " + sucesso);
                                Console.WriteLine("Normas com falha: " + falha);
                                Console.WriteLine("Norma em Execução: " + norma._metadata.id_doc);
                                //var ds_ementa = Regex.Replace(norma.ds_ementa, "\\<[^\\>]*\\>", string.Empty);
                                try
                                {
                                    foreach (var vide in norma.vides)
                                    {
                                        if (vide.caput_norma_vide == null || vide.caput_norma_vide.GetType() == typeof(string))
                                        {
                                            vide.caput_norma_vide = new Caput();
                                        }
                                        if (vide.caput_norma_vide_outra == null || vide.caput_norma_vide_outra.GetType() == typeof(string))
                                        {
                                            vide.caput_norma_vide_outra = new Caput();
                                        }
                                    }
                                    normaNova = JSON.Deserializa<NormaOV>(JSON.Serialize<NormaOVOld>(norma));
                                    //foreach (var vide in normaNova.vides)
                                    //{
                                    //    vide.caput_norma_vide = new Caput();
                                    //}
                                    Console.WriteLine("Vides: " + normaNova.vides.Count);
                                    normaNova.ax_ajuste = "caput";
                                    if (!_acessoAdNorma.Alterar(normaNova._metadata.id_doc, normaNova))
                                    {
                                        falha++;
                                        id_doc_erro.AppendLine(normaNova._metadata.id_doc.ToString());
                                        Console.WriteLine("ERRO: " + normaNova._metadata.id_doc);
                                    }
                                    else
                                    {
                                        sucesso++;
                                        b_sucesso = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    falha++;
                                    id_doc_erro.AppendLine(normaNova._metadata.id_doc.ToString());
                                    Console.WriteLine(ex);
                                    //Console.Read();
                                }
                            }
                        }

                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine(ex);
                        //Console.Read();
                    }
                }

                Console.WriteLine("Ids dos docs que deram erro:");
                Console.WriteLine(id_doc_erro);
                //Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Console.Read();
            }
        }
    }

    public class NormaOVOld : metadata
    {
        public NormaOVOld()
        {
            nm_pessoa_fisica_e_juridica = new List<string>();
            ar_atualizado = new ArquivoOV();
            ar_acao = new ArquivoOV();
            rankeamentos = new List<string>();
            requerentes = new List<Requerente>();
            requeridos = new List<Requerido>();
            relatores = new List<Relator>();
            procuradores_responsaveis = new List<Procurador>();
            interessados = new List<Interessado>();
            alteracoes = new List<AlteracaoOV>();
            origens = new List<Orgao>();
            autorias = new List<Autoria>();
            fontes = new List<Fonte>();
            decisoes = new List<Decisao>();
            indexacoes = new List<Indexacao>();
            vides = new List<VideOld>();
            ch_para_nao_duplicacao = new List<string>();
        }

        public string ch_norma { get; set; }
        public string ds_comentario { get; set; }
        public string nr_norma { get; set; }
        public int nr_sequencial { get; set; }
        public string dt_assinatura { get; set; }
        public string cr_norma { get; set; }
        public int id_ambito { get; set; }
        public string nm_ambito { get; set; }
        public string nm_apelido { get; set; }
        public string dt_autuacao { get; set; }
        public string ds_procedencia { get; set; }
        public string ds_parametro_constitucional { get; set; }
        public string ds_acao { get; set; }
        public string ds_efeito_decisao { get; set; }
        public string url_referencia { get; set; }
        public string url_projeto_lei{ get; set; }
        public string nr_projeto_lei { get; set; }
        public int id_orgao_cadastrador { get; set; }
        public string nm_orgao_cadastrador { get; set; }
        public List<string> ch_para_nao_duplicacao { get; set; }
        public bool st_pendencia { get; set; }
        public string ds_pendencia { get; set; }
        public bool st_destaque { get; set; }
        public string ds_observacao { get; set; }
        public string ds_ementa { get; set; }
        public List<string> nm_pessoa_fisica_e_juridica { get; set; }
        public ArquivoOV ar_atualizado { get; set; }
        public ArquivoOV ar_acao { get; set; }
        public bool st_atualizada { get; set; }
        public bool st_nova { get; set; }
        public List<string> rankeamentos { get; set; }
        public bool st_acao { get; set; }
        public string ch_tipo_norma { get; set; }
        public string nm_tipo_norma { get; set; }
        public string ch_situacao { get; set; }
        public string nm_situacao { get; set; }
        public List<Requerente> requerentes { get; set; }
        public List<Requerido> requeridos { get; set; }
        public List<Relator> relatores { get; set; }
        public List<Procurador> procuradores_responsaveis { get; set; }
        public List<Interessado> interessados { get; set; }
        public List<Orgao> origens { get; set; }
        public List<Autoria> autorias { get; set; }
        public List<Fonte> fontes { get; set; }
        /// <summary>
        /// Esse campo apenas é preenchido caso seja uma ação PGDF. (G2)
        /// </summary>
        public List<Decisao> decisoes { get; set; }
        public List<Indexacao> indexacoes { get; set; }
        public int in_vides { get { return vides.Count; } }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
        public List<VideOld> vides { get; set; }

        public string ax_ajuste { get; set; }
    }

    public class VideOld
    {
        public string ch_vide { get; set; }
        public string ds_comentario_vide { get; set; }
        /// <summary>
        /// Se verdadeiro então a norma corrente é afetada pelo vide.
        /// </summary>
        public bool in_norma_afetada { get; set; }

        //Tipo de Relação
        public string ch_tipo_relacao { get; set; }
        public string nm_tipo_relacao { get; set; }
        /// <summary>
        /// Se a norma corrente for a afetada use o texto ds_texto_para_alterado, se não, use ds_texto_para_alterador, oriundos de de TipoDeRelacao
        /// </summary>
        public string ds_texto_relacao { get; set; }
        public bool in_relacao_de_acao { get; set; }

        //Dados da outra Norma
        public string dt_publicacao_fonte_norma_vide { get; set; }
        public string ch_norma_vide { get; set; }
        public string ch_tipo_norma_vide { get; set; }
        public string nm_tipo_norma_vide { get; set; }
        public string nr_norma_vide { get; set; }
        public string dt_assinatura_norma_vide { get; set; }
        public string pagina_publicacao_norma_vide { get; set; }
        public string coluna_publicacao_norma_vide { get; set; }
        public string ch_tipo_fonte_norma_vide { get; set; }
        public string nm_tipo_fonte_norma_vide { get; set; }

        public string artigo_norma_vide { get; set; }
        public string paragrafo_norma_vide { get; set; }
        public string inciso_norma_vide { get; set; }
        public string alinea_norma_vide { get; set; }
        public string item_norma_vide { get; set; }
        public object caput_norma_vide { get; set; }
        public string anexo_norma_vide { get; set; }

        public string artigo_norma_vide_outra { get; set; }
        public string paragrafo_norma_vide_outra { get; set; }
        public string inciso_norma_vide_outra { get; set; }
        public string alinea_norma_vide_outra { get; set; }
        public string item_norma_vide_outra { get; set; }
        public object caput_norma_vide_outra { get; set; }
        public string anexo_norma_vide_outra { get; set; }
    }
}
