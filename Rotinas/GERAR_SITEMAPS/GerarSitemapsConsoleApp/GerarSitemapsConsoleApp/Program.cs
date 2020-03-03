using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using System.Text.RegularExpressions;
using util.BRLight;

namespace GerarSitemapsConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var _sbSitemap = new StringBuilder();
            Pesquisa pesquisa = new Pesquisa();
            NormaRN normaRn = new NormaRN();
            int offset = 0;
            int count = 1;
            int limit = 100;
            int i = 0;
            int files = 0;
            pesquisa.limit = limit.ToString();
            pesquisa.select = new string[] { "ch_norma", "fontes", "ar_atualizado" };
            pesquisa.order_by = new Order_By() { desc = new string[] { "id_doc" } };
            var filename = "";
            while (count > 0)
            {
                pesquisa.offset = offset.ToString();

                var resultsNormas = normaRn.Consultar(pesquisa);
                count = resultsNormas.results.Count;
                foreach (var norma in resultsNormas.results)
                {
                    try
                    {
                        filename = norma.getNameFileArquivoVigente();
                        if (!string.IsNullOrEmpty(filename))
                        {
                            filename = Regex.Replace(filename, "[^0-9a-zA-Z.]+", "_");
                            _sbSitemap.AppendLine("http://www.sinj.df.gov.br/sinj/Norma/" + norma.ch_norma + "/" + filename);
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception: " + Excecao.LerTodasMensagensDaExcecao(ex, false));
                    }
                }
                offset += limit;
                if ((i >= 10000 || count < limit) && _sbSitemap.Length > 0)
                {
                    files++;
                    i = 0;
                    var _fileSitemap = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "sitemaps" + Path.DirectorySeparatorChar.ToString() + "sitemap" + files + ".txt");
                    if (!_fileSitemap.Directory.Exists)
                    {
                        _fileSitemap.Directory.Create();
                    }
                    Console.WriteLine(_fileSitemap.Name);
                    var streamSitemap = _fileSitemap.AppendText();
                    streamSitemap.Write(_sbSitemap.ToString());
                    streamSitemap.Flush();
                    streamSitemap.Close();
                    _sbSitemap = new StringBuilder();
                }
            }
        }
    }
}
