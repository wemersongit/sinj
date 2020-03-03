using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;

namespace ForcarUpdateNasNormas.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ulong offset = 0;
            ulong total = 1;
            if (args.Length > 0)
            {
                offset = Convert.ToUInt64(args[0]);
                total = offset + 1;
            }
            Console.Write("nm_base: ");
            var nm_base = Console.ReadLine();
            //Console.Write("Literal de sinj_orgao: ");
            //var literal_orgao = Console.ReadLine();
            Console.Write("Literal de sinj_norma: ");
            var literal_norma = Console.ReadLine();
            
            var program = new Program();

            //switch (nm_base)
            //{
            //    case "sinj_norma":
            //        program.Normas(offset, total, literal_orgao, literal_norma);
            //        break;
            //    case "sinj_orgao":
            //        program.Orgaos(offset, total, literal_orgao);
            //        break;
            //}

            switch (nm_base)
            {
                case "sinj_norma":
                    program.AtualizarNormas(literal_norma);
                    break;
            }

            Console.Read();
        }

        private void AtualizarNormas(string literal)
        {
            StringBuilder id_doc_erro = new StringBuilder();
            var rn = new NormaRN();
            ulong offset = 0;
            ulong total = 1;
            int sucesso = 0, falha = 0, i = 0;
            while (offset < total)
            {
                try
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Consultando literal='" + literal + "' offset='"+offset+"'" );
                    var result = rn.Consultar(new Pesquisa { literal = literal, offset = offset.ToString(), limit = "100", order_by = new Order_By { asc = new string[] { "id_doc" } } });
                    total = result.result_count;
                    offset += 100;
                    foreach (var doc in result.results)
                    {
                        Console.SetCursorPosition(0, 1);
                        Console.WriteLine("Total de Normas: " + total);
                        Console.WriteLine("Normas Processadas: " + i);
                        Console.WriteLine("Norma em Execução: " + doc._metadata.id_doc);
                        Console.WriteLine("Normas com sucesso: " + sucesso);
                        Console.WriteLine("Normas com falha: " + falha);
                        try
                        {
                            if (doc.nr_norma == "0")
                            {
                                doc.nr_norma = "";
                            }
                            if (rn.Atualizar(doc._metadata.id_doc, doc))
                            {
                                sucesso++;
                            }
                            else
                            {
                                falha++;
                            }
                        }
                        catch (Exception ex)
                        {
                            falha++;
                        }
                        i++;
                    }
                }
                catch
                {

                }
            }


        }

        private void Normas(ulong offset, ulong total, string literal_orgao, string literal_norma)
        {
            Console.Clear();
            StringBuilder id_doc_erro = new StringBuilder();
            var normaRn = new NormaRN();
            try
            {
                var result_orgao = new OrgaoRN().Consultar(new Pesquisa { literal = literal_orgao, limit = null, order_by = new Order_By { asc = new string[] { "id_doc" } } });
                total = result_orgao.result_count;
                var i = 0;
                foreach (var orgao in result_orgao.results)
                {
                    ulong sucesso = 0;
                    ulong falha = 0;
                    var retorno = "";
                    offset = 0;
                    try
                    {
                        var opResult = new opResult { success = 1, failure = 1 };
                        var result_normas = normaRn.Consultar(new Pesquisa { literal = (literal_norma != "" ? literal_norma + " AND ": "") + "'" + orgao.ch_orgao + "'=any(ch_orgao)", limit = "1", order_by = new Order_By { asc = new string[] { "id_doc" } } });
                        //var result_normas = normaRn.Consultar(new Pesquisa { literal = "'" + orgao.ch_orgao + "'=any(ch_orgao) AND (array_length(ch_orgao,1) > 1)", limit = "1", order_by = new Order_By { asc = new string[] { "id_doc" } } });
                        var total_de_normas = result_normas.result_count;
                        Console.Clear();
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("Total de Orgaos: " + total);
                        Console.WriteLine("Orgaos Processados: " + i);
                        Console.WriteLine("Orgao em Execução: " + orgao._metadata.id_doc);
                        Console.WriteLine("retorno opResult: " + retorno);
                        Console.WriteLine("Total de Normas: " + total_de_normas);
                        Console.WriteLine("Normas processadas: " + (sucesso + falha));
                        Console.WriteLine("Normas com sucesso: " + sucesso);
                        Console.WriteLine("Normas com falha: " + falha);
                        while (offset < total_de_normas)
                        {
                            try
                            {
                                retorno = normaRn.PathPut<object>(new Pesquisa { literal = "'" + orgao.ch_orgao + "'=any(ch_orgao)", limit = "100", offset = offset.ToString(), order_by = new Order_By { asc = new string[] { "id_doc" } } }, new List<opMode<object>> { new opMode<object> { path = "origens/*", fn = "attr_equals", mode = "update", args = new object[] { "ch_orgao", orgao.ch_orgao, new { sg_orgao = orgao.sg_hierarquia, nm_orgao = orgao.nm_hierarquia } } } });
                                //retorno = normaRn.PathPut<object>(new Pesquisa { literal = "'" + orgao.ch_orgao + "'=any(ch_orgao) AND (array_length(ch_orgao,1) > 1)", limit = "100", offset = offset.ToString(), order_by = new Order_By { asc = new string[] { "id_doc" } } }, new List<opMode<object>> { new opMode<object> { path = "origens/*", fn = "attr_equals", mode = "update", args = new object[] { "ch_orgao", orgao.ch_orgao, new { sg_orgao = orgao.sg_hierarquia, nm_orgao = orgao.nm_hierarquia } } } });
                                opResult = JSON.Deserializa<opResult>(retorno);
                                sucesso += opResult.success;
                                falha += opResult.failure;
                                offset += 100;
                            }
                            catch (Exception ex)
                            {
                                Console.Clear();
                                Console.Write(ex.Message);
                            }
                            Console.SetCursorPosition(0, 0);
                            Console.WriteLine("Total de Orgaos: " + total);
                            Console.WriteLine("Orgaos Processados: " + i);
                            Console.WriteLine("Orgao em Execução: " + orgao._metadata.id_doc);
                            Console.WriteLine("retorno opResult: " + retorno);
                            Console.WriteLine("Total de Normas: " + total_de_normas);
                            Console.WriteLine("Normas processadas: " + (sucesso + falha));
                            Console.WriteLine("Normas com sucesso: " + sucesso);
                            Console.WriteLine("Normas com falha: " + falha);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        id_doc_erro.AppendLine(orgao._metadata.id_doc.ToString());
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Console.WriteLine("Ids dos docs que deram erro:");
            Console.WriteLine(id_doc_erro);
        }

        private void Orgaos(ulong offset, ulong total, string literal)
        {
            Console.Clear();
            var i = offset;
            StringBuilder id_doc_erro = new StringBuilder();
            var orgaoRn = new OrgaoRN();
            try
            {
                var result = orgaoRn.Consultar(new Pesquisa { literal = literal, limit = null, order_by = new Order_By { asc = new string[] { "id_doc" } } });
                total = result.result_count;
                foreach (var orgao in result.results)
                {
                    try
                    {
                        var chaves = orgao.ch_hierarquia.Split('.');
                        var nm_hierarquia = "";
                        var sg_hierarquia = "";
                        for(var j = 0; j < chaves.Length; j++){
                            var orgao_pai = result.results.Where<OrgaoOV>(o => o.ch_orgao == chaves[j]);
                            if (orgao_pai.Count() == 1)
                            {
                                nm_hierarquia += (nm_hierarquia != "" ? " > " : "") + orgao_pai.First<OrgaoOV>().nm_orgao;
                                sg_hierarquia += (sg_hierarquia != "" ? " > " : "") + orgao_pai.First<OrgaoOV>().sg_orgao;
                            }
                        }
                        if (orgao.nm_hierarquia != nm_hierarquia || orgao.sg_hierarquia != sg_hierarquia)
                        {
                            orgao.nm_hierarquia = nm_hierarquia;
                            orgao.sg_hierarquia = sg_hierarquia;
                            if (!orgaoRn.Atualizar(orgao._metadata.id_doc, orgao, false))
                            {
                                id_doc_erro.AppendLine(orgao._metadata.id_doc.ToString());
                            }
                            else
                            {
                                Console.SetCursorPosition(0, 0);
                                Console.WriteLine("Atualizados: " + i++);
                                Console.WriteLine("result_count: " + total);
                                Console.WriteLine("id_doc: " + orgao._metadata.id_doc);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        id_doc_erro.AppendLine(orgao._metadata.id_doc.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Console.WriteLine("Ids dos docs que deram erro:");
            Console.WriteLine(id_doc_erro);
        }
    }
}
