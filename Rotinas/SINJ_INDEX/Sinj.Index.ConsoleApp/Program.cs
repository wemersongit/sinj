using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using util.BRLight;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Sinj.Index.ConsoleApp
{
    class Program
    {
        private FileInfo file_log;
        static void Main(string[] args)
        {
            if(args.Length == 0){
                Console.WriteLine("É necessário informar a base.");
                return;
                //args = new string[] { "sinj_norma", ""};
                //args = new string[] { "sinj_norma", "", "0", "500" };
            }
            var program = new Program();
            program.file_log = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "index-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".log");
            if (args.Length == 4)
            {
                program.Indexar(args[0], args[1], ulong.Parse(args[2]), ulong.Parse(args[3]));
            }
            else
            {
                program.ControlarIndexacao(args[0], args[1]);
            }
        }

        public Process ExecutarProcesso(string nm_base, string literal, ulong offset, ulong limit)
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            try
            {
                //Process processo = Process.Start(processStartInfo);
                //Console.WriteLine(arguments);
                //return processo;
                Process processo = new Process();
                processo.StartInfo.UseShellExecute = false;
                processo.StartInfo.RedirectStandardOutput = true;
                processo.StartInfo.FileName = "mono";
                processo.StartInfo.Arguments = "Sinj.Index.ConsoleApp.exe " + nm_base + " \"" + literal + "\" " + offset + " " + limit;
                processo.Start();
                Console.WriteLine(processo.StartInfo.Arguments);
                Thread.Sleep(5000);
                return processo;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na execução de Sinj.Index.ConsoleApp.exe " + nm_base + " \"" + literal + "\" " + offset + " " + limit, ex);
            }
        }

        private void ControlarIndexacao(string nm_base, string literal)
        {
            var total = new AcessoAD<metadata>(nm_base).Consultar(new Pesquisa { limit = "1", literal = literal }).result_count;
            ulong iQuant_por_processo = 500;
            ulong iQuant_de_processos = 5;
            ulong processos_iniciados = 0;
            List<Process> processos_em_execucao = new List<Process>();
            var sQuant_por_processo = Config.ValorChave("IntQuantidadePorProcesso");
            var sQuant_de_processos = Config.ValorChave("IntQuantidadeDeProcessos");
            ulong.TryParse(sQuant_por_processo, out iQuant_por_processo);
            ulong.TryParse(sQuant_de_processos, out iQuant_de_processos);

            while (processos_iniciados < (total / iQuant_por_processo))
            {
                if (processos_em_execucao.Count < 5)
                {
                    try
                    {
                        processos_em_execucao.Add(ExecutarProcesso(nm_base, literal, processos_iniciados * iQuant_por_processo, iQuant_por_processo));
                        processos_iniciados++;
                    }
                    catch (Exception ex)
                    {
                        var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                        CriarLog(mensagem + "... StackTrace:" + ex.StackTrace);
                    }
                }
                foreach (var processo in processos_em_execucao) //Para cada processo verifico se já foi finalizado, se sim eu removo da lista para que o próximo processo possa ser executado.
                {
                    if (processo.HasExited)
                    {
                        processos_em_execucao.Remove(processo);
                        break;
                    }
                }
            }
        }

        private void Indexar(string nm_base, string literal, ulong offset, ulong limit)
        {
            var results = new AcessoAD<metadata>(nm_base).Consultar(new Pesquisa { offset = offset.ToString(), limit = limit.ToString(), literal = literal, order_by = new Order_By { asc = new string[]{"id_doc"} } });
            var doc_es = new neo.BRLightES.DocEs();
            var url_idx = doc_es.GetUrlEs(nm_base);
            CriarLog("Start  " + nm_base + " " + offset + " " + limit + " " + url_idx + " " + results.result_count);
            var ok = false;
            foreach (var result in results.results)
            {
                try
                {
                    ok = false;
                    while (!ok)
                    {
                        var json_reg = new Reg(nm_base).pesquisarRegFull(result._metadata.id_doc);
                        if (json_reg.IndexOf("_metadata") > -1)
                        {
                            ok = true;
                            try
                            {
                                doc_es.IncluirDoc(json_reg, url_idx + "/" + result._metadata.id_doc);
                                CriarLog("Doc " + url_idx + "/" + result._metadata.id_doc + " indexado.");
                                new AcessoAD<metadata>(nm_base).pathPut(result._metadata.id_doc, "_metadata/dt_idx", DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), null);
                                CriarLog("Doc " + result._metadata.id_doc + " dt_idx = " + DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"));
                            }
                            catch (Exception ex)
                            {
                                var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                                CriarLog(mensagem + "... StackTrace:" + ex.StackTrace);
                            }
                        }
                        else
                        {
                            CriarLog(json_reg.Substring(0, 20) + "......" + offset + "....." + result._metadata.id_doc);
                        }

                    }
                }
                catch(Exception ex){

                }
            }
        }

        private void CriarLog(string mensagem){
            var stream_info = file_log.AppendText();
            stream_info.WriteLine(DateTime.Now.ToString() + ": " + mensagem);
            stream_info.Flush();
            stream_info.Close();
        }
    }
}
