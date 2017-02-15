using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BRLight.Util;
using TCDF_REPORT.RN;
namespace total_de_normas_usando_termo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string stringConnection = Configuracao.ValorChave("StringDeConexao");
                var termoRn = new TermoRN(stringConnection);
                var termos = termoRn.BuscarTodosOsTermos().OrderBy(t => t.Nm_Termo);
                Console.WriteLine("Total de Termos: " + termos.Count());
                List<TermoRelatorio> termosRelatorioNaoUsados = new List<TermoRelatorio>();
                List<TermoRelatorio> termosRelatorioGeral = new List<TermoRelatorio>();
                NormaRN normaRn = new NormaRN(stringConnection);
                int i = 1;
                foreach (var termo in termos)
                {
                    int total = normaRn.ContarNormasQueContemOTermo(termo);
                    Console.WriteLine(i + ") " + termo.getTipoTermo() + "............" + termo.Nm_Termo + "............" + total);
                    if (total == 0)
                    {
                        termosRelatorioNaoUsados.Add(new TermoRelatorio {nm_termo = termo.Nm_Termo, in_tipo = termo.In_TipoTermo});
                    }
                    else
                    {
                        termosRelatorioGeral.Add(new TermoRelatorio { nm_termo = termo.Nm_Termo, in_tipo = termo.In_TipoTermo, total = total});
                    }
                    i++;
                }
                StringBuilder preparelogFile = new StringBuilder();

                var descritoresNaoUsados = termosRelatorioNaoUsados.Where(t => t.in_tipo == 1);
                var especificadoresNaoUsados = termosRelatorioNaoUsados.Where(t => t.in_tipo == 2);
                var autoridadesNaoUsados = termosRelatorioNaoUsados.Where(t => t.in_tipo == 3);
                var listasNaoUsados = termosRelatorioNaoUsados.Where(t => t.in_tipo == 4);

                preparelogFile.AppendLine("Descritores:");
                foreach (var descritor in descritoresNaoUsados)
                {
                    preparelogFile.AppendLine(descritor.nm_termo);
                }

                preparelogFile.AppendLine("");
                preparelogFile.AppendLine("");
                preparelogFile.AppendLine("Especificadores:");
                foreach (var especificador in especificadoresNaoUsados)
                {
                    preparelogFile.AppendLine(especificador.nm_termo);
                }

                preparelogFile.AppendLine("");
                preparelogFile.AppendLine("");
                preparelogFile.AppendLine("Autoridades:");
                foreach (var autoridade in autoridadesNaoUsados)
                {
                    preparelogFile.AppendLine(autoridade.nm_termo);
                }

                preparelogFile.AppendLine("");
                preparelogFile.AppendLine("");
                preparelogFile.AppendLine("Listas:");
                foreach (var lista in listasNaoUsados)
                {
                    preparelogFile.AppendLine(lista.nm_termo);
                }

                string dir = AppDomain.CurrentDomain.BaseDirectory + @"output\";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string file = dir + "Nao_Usados-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + @".txt";
                using (TextWriter tw = new StreamWriter(file, false))
                {
                    tw.WriteLine(preparelogFile);
                    tw.Close();
                }

                var descritores = termosRelatorioGeral.Where(t => t.in_tipo == 1);
                var especificadores = termosRelatorioGeral.Where(t => t.in_tipo == 2);
                var autoridades = termosRelatorioGeral.Where(t => t.in_tipo == 3);
                var listas = termosRelatorioGeral.Where(t => t.in_tipo == 4);
                preparelogFile = new StringBuilder();
                preparelogFile.AppendLine("Descritores:");
                foreach (var descritor in descritores)
                {
                    preparelogFile.AppendLine(descritor.nm_termo + "\t" + descritor.total);
                }

                preparelogFile.AppendLine("");
                preparelogFile.AppendLine("");
                preparelogFile.AppendLine("Especificadores:");
                foreach (var especificador in especificadores)
                {
                    preparelogFile.AppendLine(especificador.nm_termo + "\t" + especificador.total);
                }

                preparelogFile.AppendLine("");
                preparelogFile.AppendLine("");
                preparelogFile.AppendLine("Autoridades:");
                foreach (var autoridade in autoridades)
                {
                    preparelogFile.AppendLine(autoridade.nm_termo + "\t" + autoridade.total);
                }

                preparelogFile.AppendLine("");
                preparelogFile.AppendLine("");
                preparelogFile.AppendLine("Listas:");
                foreach (var lista in listas)
                {
                    preparelogFile.AppendLine(lista.nm_termo + "\t" + lista.total);
                }

                dir = AppDomain.CurrentDomain.BaseDirectory + @"output\";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                file = dir + "Geral-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + @".txt";
                using (TextWriter tw = new StreamWriter(file, false))
                {
                    tw.WriteLine(preparelogFile);
                    tw.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.Read();
            }
        }

        class TermoRelatorio
        {
            public string nm_termo { get; set; }
            public int in_tipo { get; set; }
            public int total { get; set; }
        }
    }
}
