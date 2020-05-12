using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BRLight.Util;
using TCDF_REPORT.OV;
using TCDF_REPORT.RN;

namespace APP_DELETE_TERMOS_NAO_USADOS
{
    class Program
    {
        static void Main(string[] args)
        {
            string stringConnection = Configuracao.ValorChave("StringDeConexao");
            TermoRN termoRn = new TermoRN(stringConnection);
            var termos = termoRn.BuscarTodosOsTermos().OrderBy(t => t.Nm_Termo);
            Console.WriteLine("Total de Termos: " + termos.Count());
            List<TermoRelatorio> termosDeletados = new List<TermoRelatorio>();
            NormaRN normaRn = new NormaRN(stringConnection);
            int i = 1;
            foreach (var termo in termos)
            {
                //Só estou deletando descritores e especificadores
                if(termo.In_TipoTermo == 3 || termo.In_TipoTermo == 4) continue;
                int total = normaRn.ContarNormasQueContemOTermo(termo);
                Console.WriteLine(i + ") " + termo.getTipoTermo() + "............" + termo.Nm_Termo + "............" + total);
                if (total == 0)
                {
                    var deleted = termoRn.DeletarTermoNaoUsado(termo.Id_Termo);
                    termosDeletados.Add(new TermoRelatorio { nm_termo = termo.Nm_Termo, in_tipo = termo.In_TipoTermo, deleted = deleted > 0});
                }
                i++;
            }
            StringBuilder preparelogFile = new StringBuilder();

            foreach (var termo in termosDeletados)
            {
                preparelogFile.AppendLine(termo.nm_termo + " .............tipo:" + termo.in_tipo + " ...........deletado:" + termo.deleted);
            }

            string dir = AppDomain.CurrentDomain.BaseDirectory + @"output\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string file = dir + "Deleteds-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + @".txt";
            using (TextWriter tw = new StreamWriter(file, false))
            {
                tw.WriteLine(preparelogFile);
                tw.Close();
            }
        }

        class TermoRelatorio
        {
            public string nm_termo { get; set; }
            public int in_tipo { get; set; }
            public bool deleted { get; set; }
        }
    }
}
