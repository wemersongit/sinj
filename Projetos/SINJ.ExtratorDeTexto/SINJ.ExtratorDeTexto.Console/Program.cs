using System;
using BRLight.Logger;
using BRLight.Util;

namespace SINJ.ExtratorDeTexto.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string stringDeConexao = Configuracao.ValorChave("StringDeConexao");
                var manager = new ManagerExtratorDeTexto(stringDeConexao);
                string bases = Configuracao.ValorChave("bases");
                string[] basesSplit = bases.Split(',');
                for (int i = 0; i < basesSplit.Length; i++)
                {
                    string nm_base = Configuracao.ValorChave(basesSplit[i] + ".nm_base");
                    string nm_coluba_id = Configuracao.ValorChave(basesSplit[i] + ".nm_coluna_id");
                    string nm_coluna_texto = Configuracao.ValorChave(basesSplit[i] + ".nm_coluna_texto");
                    string nm_coluna_path_file = Configuracao.ValorChave(basesSplit[i] + ".nm_coluna_path_file");
                    string path_repository_files = Configuracao.ValorChave(basesSplit[i] + ".path_repository_files");
                    string[] ids = manager.ListarIdsSemTexto(nm_base, nm_coluba_id, nm_coluna_texto);
                    System.Console.WriteLine("Base: " + nm_base);
                    System.Console.WriteLine("Total de registros sem texto: " + ids.Length);
                    foreach (var id in ids)
                    {
                        try
                        {
                            System.Console.WriteLine("Extraindo e salvando id: " + id);
                            manager.ExtrairTexto(id, nm_base, nm_coluba_id, nm_coluna_texto, nm_coluna_path_file, path_repository_files);
                            ManagerLog.GravaLog(LogType.Information, LogLayer.View, "", "", "Salvando Texto", "Base " + nm_base + ", Id " + id, "Sucesso ao gravar texto.");
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("Erro ao salvar texto: " + id);
                            System.Console.WriteLine("Mensagem da exceção: " + ex.Message);
                            ManagerLog.GravaLog(LogType.Error, LogLayer.View, "", "", "Salvando Texto", "Base " + nm_base + ", Id " + id, "", ex);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                System.Console.WriteLine("Erro :" + ex.Message);
                ManagerLog.GravaLog(LogType.Error, LogLayer.View, "", "", "", "", "", ex);
            }
        }
    }
}
