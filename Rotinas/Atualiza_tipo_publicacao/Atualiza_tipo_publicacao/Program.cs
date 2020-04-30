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
            Console.WriteLine("======== ATUALIZAR TIPO PUBLICACAO ========");

            ulong offset = 0;
            ulong total = 1;

            StringBuilder id_doc_erro = new StringBuilder();

            var normaRn = new NormaRN();
            try
            {
                var sucesso = 0;
                var falha = 0;
                var normas_processadas = 0;
                while (offset < total)
                {
                    try
                    {
                        var result = normaRn.Consultar(new Pesquisa { limit = "100", select = new string[] { "id_doc", "fontes" }, literal = "'RVT'=any(nm_tipo_publicacao) OR 'REP'=any(nm_tipo_publicacao) OR 'PUB'=any(nm_tipo_publicacao) OR 'RET'=any(nm_tipo_publicacao)", order_by = new Order_By { asc = new string[] { "id_doc" } } });
                        total = result.result_count;

                        foreach (var norma in result.results)
                        {
                            foreach (var fonte in norma.fontes)
                            {
                                if (fonte.nm_tipo_publicacao.Equals("PUB"))
                                {
                                    fonte.nm_tipo_publicacao = "Publicação";
                                }
                                else if (fonte.nm_tipo_publicacao.Equals("REP"))
                                {
                                    fonte.nm_tipo_publicacao = "Republicação";
                                }
                                else if (fonte.nm_tipo_publicacao.Equals("RVT"))
                                {
                                    fonte.nm_tipo_publicacao = "Rejeição de Veto";
                                }
                                else if (fonte.nm_tipo_publicacao.Equals("RET"))
                                {
                                    fonte.nm_tipo_publicacao = "Retificação";
                                };
                            }
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

                                try
                                {
                                    //if (normaRn.PathPut(norma._metadata.id_doc, "fontes", JSON.Serialize<List<Fonte>>(norma.fontes), null) != "UPDATED")
                                    //{
                                    //    falha++;
                                    //    id_doc_erro.AppendLine(norma._metadata.id_doc.ToString());
                                    //    Console.WriteLine("ERRO: " + norma._metadata.id_doc);
                                    //}
                                    //else
                                    //{
                                    //    sucesso++;
                                    //    b_sucesso = true;
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    falha++;
                                    id_doc_erro.AppendLine(norma._metadata.id_doc.ToString());
                                    Console.WriteLine(ex.Message);
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
            Console.Read();
        }
    }
}
