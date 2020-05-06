using System;
using System.Collections.Generic;
using System.Diagnostics;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.Sub_Process
{
    class Program
    {
        static void Main(string[] args)
        {
            var sArgs = "";
            try
            {
                Console.WriteLine("Iniciando....");
                Log.FileName = "-Pr-" + (args.Length > 0 ? args[0].Replace("\\", "") : Guid.NewGuid().ToString()) + ".log";
                for (int i = 0; i < args.Length; i++)
                {
                    sArgs += args[i] + " ";
                }
                Console.WriteLine("Parametros: " + sArgs);
                Log.LogarInformacao("Iniciando Subprocesso", "Parametros: " + sArgs);
                //args = new string[] { "delete" };
                Executar(args);
                Console.Write("Finalizando...");
            }
            catch(Exception ex)
            {
                Log.LogarExcecao("Iniciando Subprocesso", "Erro ao iniciar Subprocesso. Parametros: " + sArgs, ex);
            }
            finally
            {
                if (bool.Parse(Configuracao.LerValorChave("FlagDebug")))
                {
                    Console.Read();
                }
                Process.GetCurrentProcess().Kill();
            }
        }

        private static void Executar(string[] args)
        {
            string argumentos = "";
            foreach (var s in args)
            {
                argumentos = argumentos + s + " ";
            }
            List<string> listArgs = new List<string>();
            if(argumentos.Contains("\\"))
            {
                string[] arrayArgs = argumentos.Split('\\');
                foreach (var item in arrayArgs)
                {
                    if (item != "")
                    {
                        string argTrated = item.Substring(0, item.Length - 1);
                        listArgs.Add(argTrated);
                        Console.WriteLine(argTrated);
                    }
                }
            }
            else
            {
                listArgs.AddRange(args);
            }
            
            if(listArgs.Count > 0)
            {
                Exportar exportar = new Exportar();
                bool exportFiles = false;
                if(listArgs.Count > 1)
                {
                    if (listArgs.Count == 3)
                    {
                        bool.TryParse(listArgs[2], out exportFiles);
                    }
                    exportar.Indexar(listArgs[0], listArgs[1], exportFiles);
                }
                else
                {
                    switch (listArgs[0].ToLower())
                    {
                        case "mapping":
                            exportar.IniciarProcessoMapping();
                            break;
                        case "delete":
                            exportar.IniciarProcessoDelete();
                            break;
                    }
                }
                
            }
        }

    }
}
