using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Exportador_LB_to_ES.ManagerProcesses
{
    class ExecuteProcess
    {
        public static Process ExecuteProcesses(string @base, string query, string exportarArquivos)
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string file = dir + "Exportador_LB_to_ES.Sub_Process.exe";
            //" \"\\" + @base + "\" \"\\" + query + "\" \"\\" + exportarArquivos + "\"";
            ProcessStartInfo processStartInfo;

            processStartInfo = new ProcessStartInfo(file, " \"\\" + @base + "\" \"\\" + query + "\" \"\\" + exportarArquivos + "\"");
            
            try
            {
                Process processo = Process.Start(processStartInfo);
                Thread.Sleep(5000);
                return processo;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na execução de " + file, ex);
            }
        }

        public static void ExecuteProcesses(string action)
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string file = dir + "Exportador_LB_to_ES.Sub_Process.exe";
            //" \"\\" + @base + "\" \"\\" + query + "\" \"\\" + exportarArquivos + "\"";

            ProcessStartInfo processStartInfo = new ProcessStartInfo(file, " \"\\" + action + "\"");

            try
            {
                Process.Start(processStartInfo);
                Thread.Sleep(30000);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na execução de " + file, ex);
            }
        }
    }
}
