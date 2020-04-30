using System;
using System.Collections.Generic;
using Exportador_LB_to_ES.ManagerProcesses;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES
{
    class Program
    {
        static void Main(string[] args)
        {
            var sArgs = "";
            try
            {
                Log.FileName = "-Pr-" + (args.Length > 0 ? args[0].Replace("\\", "") : Guid.NewGuid().ToString()) + ".log";
                Console.WriteLine("Iniciando....");
                for (int i = 0; i < args.Length; i++ )
                {
                    sArgs += args[i] + " ";
                }
                Console.WriteLine("Parametros: " + sArgs);
                Log.LogarInformacao("Iniciando Processo", "Parametros: " + sArgs);
                execute(args);
                //execute(new string[1]{"regs"});
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                Log.LogarExcecao("Iniciando Processo", "Erro ao iniciar processo. Parametros: " + sArgs, ex);
            }
        }

        private static void execute(string[] args)
        {
            ManagerProcess managerProcess = new ManagerProcess();
            if(args.Length == 0)
            {
                List<KeyValuePair<string, string>> listKeyValue = RecebeEntradas();
                if (listKeyValue.Count > 0)
                {
                    Console.Write("Exportar Arquivos? (s/n)");
                    char key = Console.ReadKey().KeyChar;
                    var exportarArquivos = "";
                    exportarArquivos = key == 's' ? "true" : "false";
                    if (listKeyValue.Count > 0)
                    {
                        managerProcess.StartQuery(listKeyValue, exportarArquivos);
                    }
                }
            }
            else
            {
                if (args.Length == 1)
                {
                    switch (args[0])
                    {
                        case "atlz":
                            managerProcess.StartAtlz();
                            break;
                        case "regs":
                            managerProcess.StartRegs("");
                            break;
                        case "files":
                            managerProcess.StartFiles("");
                            break;
                        case "full":
                            managerProcess.StartFull("");
                            break;
                        case "mapping":
                            managerProcess.StartMapping();
                            break;
                        case "delete":
                            managerProcess.StartDelete();
                            break;
                    }
                }
                else if (args.Length == 2)
                {
                    if (args[1] == "mapping")
                    {
                        managerProcess.StartMapping();
                    }
                    switch (args[0])
                    {
                        case "atlz":
                            managerProcess.StartAtlz();
                            break;
                        case "regs":
                            managerProcess.StartRegs(args[1]);
                            break;
                        case "files":
                            managerProcess.StartFiles(args[1]);
                            break;
                        case "full":
                            managerProcess.StartFull(args[1]);
                            break;
                    }
                }
            }
        }

        private static List<KeyValuePair<string, string>> RecebeEntradas()
        {
            List<KeyValuePair<string,string>> listKeyValues = new List<KeyValuePair<string, string>>();
            while(RecebeTrueOuFalse("Deseja inserir uma query de busca? (s/n)"))
            {
                string @base = RecebeEntrada("Informe a Base:");
                if(@base != "")
                {
                    string query = RecebeEntrada("Informe a query:");
                    if(query != "")
                    {
                        KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(@base, query);
                        listKeyValues.Add(keyValuePair);
                    }
                }
            }
            return listKeyValues;
        }

        private static bool RecebeTrueOuFalse(string textoParaExibir)
        {
            System.Console.WriteLine(textoParaExibir);
            char key = System.Console.ReadKey().KeyChar;
            if (key
                == 's')
            {
                return true;
            }
            return false;
        }

        private static string RecebeEntrada(string textoParaExibir)
        {
            System.Console.WriteLine(textoParaExibir);
            return System.Console.ReadLine();
        }
    }
}
