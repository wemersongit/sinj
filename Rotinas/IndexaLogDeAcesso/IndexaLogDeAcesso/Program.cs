using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using util.BRLight;
using TCDF.Sinj.ESUtil;
using TCDF.Sinj.OV;
using System.Threading;

namespace IndexaLogDeAcesso
{
    class Program
    {
        static void Main(string[] args)
        {
            List<LogSinj> logs = new List<LogSinj>();
            NormaEs normaEs = new NormaEs();
            LogEs logEs = new LogEs();
            string line;
            string lineReplaced;
            LogSinj log;
            string pattern = ".+?\\[(.*?) -.+?\\] \"[GET|POST].+? /sinj/detalhesdenorma.aspx\\?id_norma=(.+?) .*";
            string replacement = "{\"data\":\"$1\", \"id_norma\":\"$2\"}";
            StreamReader streamReader;
            try
            {
                var files = Directory.GetFiles(util.BRLight.Config.ValorChave("PathLog"));
                Console.WriteLine("Caminho dos arquivos: " + files);
                foreach (var file in files)
                {
                    try
                    {
                        Console.WriteLine("Processando log: " + file);
                        streamReader = new StreamReader(file);
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            try
                            {
                                //Console.WriteLine("linha: " + line);
                                //10.9.1.7 - - [14/Aug/2017:09:05:48 -0300] "GET /sinj/DetalhesDeNorma.aspx?id_norma=5c8fb2bfee8d48f99f4888d7de87be17 HTTP/1.1" 200 9014 "-" "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)"
                                if (Regex.Matches(line, pattern, RegexOptions.IgnoreCase).Count == 1)
                                {
                                    lineReplaced = Regex.Replace(line, pattern, replacement, RegexOptions.IgnoreCase);
                                    //Console.WriteLine("linha convertida: " + lineReplaced);
                                    log = util.BRLight.JSON.Deserializa<LogSinj>(lineReplaced);
                                    log.data = DateTime.ParseExact(log.data, "dd/MMM/yyyy:HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
                                    if (logs.Count<LogSinj>(l => l.id_norma == log.id_norma) > 0)
                                    {
                                        log.ds_norma = logs.Where<LogSinj>(l => l.id_norma == log.id_norma).First().ds_norma;
                                    }
                                    else
                                    {
                                        log.ds_norma = normaEs.GetDsNorma(log.id_norma);
                                        logs.Add(log);
                                    }
                                    logEs.incluirLog(log);
                                }
                            }
                            catch (Exception ex) {
                                Console.WriteLine("Erro: " + util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false));
                                Console.WriteLine("Retornando em segundos");
                                Thread.Sleep(5000);
                            }
                        }
                        streamReader.Close();
                    }
                    catch (Exception ex) {
                        Console.WriteLine("Erro: " + util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false));
                        Console.WriteLine("Retornando em segundos");
                        Thread.Sleep(5000);
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Erro: " + util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false));
                Console.WriteLine("...");
                Console.Read();
            }
        }
    }

    public class NormaEs
    {
        private DocEs docEs;
        private string urlEs;

        public NormaEs()
        {
            docEs = new DocEs();
            urlEs = util.BRLight.Config.ValorChave("UrlNorma");
        }

        public string GetDsNorma(string id_norma)
        {
            var ds_norma = "sem descricao";
            
            try
            {
                //Console.WriteLine("Pesquisando: " + id_norma);
                var result =  docEs.Pesquisar<NormaOV>("{\"query\":{\"term\":{\"ch_norma\":\""+id_norma+"\"}}, \"_source\":{\"include\":[\"nm_tipo_norma\",\"dt_assinatura\",\"nr_norma\"]}}", urlEs + "/_search");
                if (result.hits.hits.Count == 1)
                {
                    ds_norma = result.hits.hits[0]._source.getDescricaoDaNorma();
                    //Console.WriteLine("ds_norma: " + ds_norma);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false));
                Console.WriteLine("Retornando em segundos");
                Thread.Sleep(5000);
            }


            return ds_norma;
        }
    }

    public class LogSinj
    {
        public string data { get; set; }
        public string id_norma { get; set; }
        public string ds_norma { get; set; }
    }

    public class LogEs
    {
        private DocEs docEs;
        private string urlEs;

        public LogEs()
        {
            docEs = new DocEs();
            urlEs = util.BRLight.Config.ValorChave("UrlLog");
        }

        public void incluirLog(LogSinj logSinj)
        {
            try
            {
                //Console.WriteLine("Indexando: " + logSinj.data + " " + logSinj.id_norma);
                docEs.IncluirDoc(JSON.Serialize<LogSinj>(logSinj), urlEs);
            }
            catch (Exception ex){
                Console.WriteLine("Erro: " + util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false));
                Console.WriteLine("Retornando em segundos");
                Thread.Sleep(5000);
            }
        }
    }
}
