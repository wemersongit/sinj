using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;
using Ionic.Zip;
using ManagerSFTP;

namespace Exportador_LB_to_ES.AD.AD
{
    public class ArquivoAD : AD
    {
        private List<RegistrosParaExportar> PreencherCaminhosDosArquivos(List<RegistrosParaExportar> lista)
        {
            string extentNorma = Configuracao.LerValorChave(chaveBaseNormas);
            string extentDodf = Configuracao.LerValorChave(chaveBaseDodf);
            List<RegistrosParaExportar> novaLista = new List<RegistrosParaExportar>();
            IEnumerable<RegistrosParaExportar> ieNormas = from l in lista where l.Nome == extentNorma select l;
            string sqlNormas = string.Format("select * from {0}", extentNorma);
            string idsNormas = "";
            foreach (RegistrosParaExportar ieNorma in ieNormas)
            {
                idsNormas = idsNormas + "Id = " + ieNorma.Id + " or ";
            }
            if (idsNormas != "")
            {
                sqlNormas = sqlNormas + " where " + idsNormas.Remove(idsNormas.Length - 4);
                novaLista.AddRange(new NormaAD().BuscarCaminhosDeArquivosDeNorma(sqlNormas));
            }
            IEnumerable<RegistrosParaExportar> ieDodfs = from l in lista where l.Nome == extentDodf select l;
            string sqlDodfs = string.Format("select * from {0}", extentDodf);
            string idsDodfs = "";
            foreach (RegistrosParaExportar ieDodf in ieDodfs)
            {
                idsDodfs = idsDodfs + "Id = " + ieDodf.Id + " or ";
            }
            if (idsDodfs != "")
            {
                sqlDodfs = sqlDodfs + " where " + idsDodfs.Remove(idsDodfs.Length - 4);
                novaLista.AddRange(new DodfAD().BuscarCaminhosDeArquivosDeDodf(sqlDodfs));
            }
            return novaLista;
        }
        /// <summary>
        /// Retorna um nome único para arquivo no formato year + "." + month + "." + day + "." + hour + "." + minute + "." + second + "." + milliSecond;. By Questor
        /// </summary>
        /// <returns></returns>
        private string CriarNomeDeArquivo(string arquivo)
        {
            if (arquivo.Contains("\\"))
            {
                string[] arquivoSplit = arquivo.Split('\\');
                arquivo = arquivoSplit[arquivoSplit.Length - 1];
            }
            arquivo = arquivo.Substring(0, arquivo.LastIndexOf('.'));
            return arquivo;
        }

        /// <summary>
        /// Cria um .zip do arquivo
        /// </summary>
        /// <param name="arquivo">Caminho do arquivo que será adicionado ao .zip</param>
        /// <param name="localDoArquivo">Caminho completo do arquivo para criar um array de bytes</param>
        /// <param name="pathAndNameZipFile">Caminho para salvar o arquivo .zip</param>
        private static void CriarArquivoZip(string arquivo, string localDoArquivo, string pathAndNameZipFile)
        {
            try
            {
                if (File.Exists(localDoArquivo))
                {
                    using (ZipFile _ZipFile = new ZipFile())
                    {
                        //Note: O fato de encodar o arquivo em utf-8 não resolve o nome do arquivo, por isso para o linux o nome dos arquivos, após a descompactação, mudam os caracteres especiais... como contorno estou aplicando algumas manobras para remover estes caracteres
                        string normalize = arquivo.Normalize(NormalizationForm.FormD);
                        StringBuilder stringBuilder = new StringBuilder();

                        for (int k = 0; k < normalize.Length; k++)
                        {
                            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(normalize[k]);
                            if (uc != UnicodeCategory.NonSpacingMark)
                            {
                                stringBuilder.Append(normalize[k]);
                            }
                        }
                        StreamReader reader = new StreamReader(localDoArquivo);
                        Stream stream = reader.BaseStream;
                        _ZipFile.AddEntry(stringBuilder.ToString(), stream);
                        _ZipFile.Save(pathAndNameZipFile);
                        stream.Close();
                    }
                }
                else
                {
                    throw new FileNotFoundException("Arquivo não encontrado. path: " + localDoArquivo);
                }
            }

            catch (Exception ex)
            {
                Log.LogarExcecao("Exportação de Arquivos", "Não foi possível criar o arquivo \"ZIP\", Registro \"nome\": " + localDoArquivo, ex);
            }

        }

        public void ExportarArquivos(List<RegistrosParaExportar> registrosParaExportar)
        {
            List<RegistrosParaExportar> arquivos = PreencherCaminhosDosArquivos(registrosParaExportar);
            Exportar(arquivos);
        }

        private void Exportar(List<RegistrosParaExportar> registrosParaExportar)
        {
            List<string> idsSucess = new List<string>();
            string pathZipFile = AppDomain.CurrentDomain.BaseDirectory + "ArquivosParaExportar";
            if (!Directory.Exists(pathZipFile))
            {
                Directory.CreateDirectory(pathZipFile);
            }
            string pathUploadedFiles = Configuracao.LerValorChave(chavePathOfTheUploadedFiles);
            string pathSftp = Configuracao.LerValorChave(chavePathToSaveOnSFTP);
            string sftpServer = Configuracao.LerValorChave(chaveSFTPServer);
            string sftpPort = Configuracao.LerValorChave(chaveSFTPServerPort);
            string sftpUser = Configuracao.LerValorChave(chaveSFTPServerUser);
            string sftpPassword = Configuracao.LerValorChave(chaveSFTPServerPassword);
            string sDeletar = Configuracao.LerValorChave(chaveFlagDeletarArquivosZip);
            bool bDeletar;
            bool.TryParse(sDeletar, out bDeletar);
            List<string> list_pathAndNameZipFile = new List<string>();
            try
            {
                IEnumerable<RegistrosParaExportar> ieRegistrosParaExportar = from reg in registrosParaExportar where reg.CaminhoArquivoTexto != "" select reg;

                foreach (RegistrosParaExportar registro in ieRegistrosParaExportar)
                {
                    string arquivo = registro.CaminhoArquivoTexto;
                    try
                    {
                        if (SFTP.conect_SFTP_UploadFile(sftpServer, sftpUser, sftpPassword, sftpPort))
                        {

                            string nameZipFile = CriarNomeDeArquivo(arquivo) + ".zip";

                            string pathAndNameZipFile = pathZipFile + @"\" + nameZipFile;

                            list_pathAndNameZipFile.Add(pathAndNameZipFile);

                            CriarArquivoZip(arquivo, pathUploadedFiles + @"\" + arquivo, pathAndNameZipFile);

                            if (SFTP.SFTP_UploadFile(pathAndNameZipFile, pathSftp, nameZipFile))
                            {
                                idsSucess.Add(registro.Id);
                                Console.WriteLine(@"---------> Arquivo exportado. " + registro.Nome + "... Id  = " + registro.Id);
                            }
                            else
                            {
                                Console.WriteLine(@"Erro na Exportação de arquivos de " + registro.Nome + "... Id  = " + registro.Id);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogarExcecao("Exportação de Arquivos", "Erro na exportação do arquivo " + arquivo + " da base " + registro.Nome, ex);
                    }
                }
                if (bDeletar)
                {
                    foreach (var _pathAndNameZipFile in list_pathAndNameZipFile)
                    {
                        try
                        {
                            File.Delete(_pathAndNameZipFile);
                            Console.WriteLine("Arquivo deletado: " + _pathAndNameZipFile);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ERRO - Arquivo não deletado: " + _pathAndNameZipFile);
                            Log.LogarExcecao("Exportação de Arquivos", "Erro na exclusão do arquivo .zip: " + _pathAndNameZipFile, ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogarExcecao("Exportação de Arquivos", "Erro na exportação de arquivos da base BaseForIndexerAndExporter ", ex);
            }
        }

        public List<string> ExportarArquivos(List<RegistrosParaExportar> registrosParaExportar, string nomeDaBase)
        {
            Console.WriteLine(@"---------> Arquivos selecionados para exportação: " + registrosParaExportar.Count);
            List<string> idsSucess = new List<string>();
            string pathZipFile = AppDomain.CurrentDomain.BaseDirectory + "ArquivosParaExportar";
            if (!Directory.Exists(pathZipFile))
            {
                Directory.CreateDirectory(pathZipFile);
            }
            string pathUploadedFiles = Configuracao.LerValorChave(chavePathOfTheUploadedFiles);
            string pathSftp = Configuracao.LerValorChave(chavePathToSaveOnSFTP);
            string sftpServer = Configuracao.LerValorChave(chaveSFTPServer);
            string sftpPort = Configuracao.LerValorChave(chaveSFTPServerPort);
            string sftpUser = Configuracao.LerValorChave(chaveSFTPServerUser);
            string sftpPassword = Configuracao.LerValorChave(chaveSFTPServerPassword);
            string sDeletar = Configuracao.LerValorChave(chaveFlagDeletarArquivosZip);
            bool bDeletar;
            bool.TryParse(sDeletar, out bDeletar);
            List<string> list_pathAndNameZipFile = new List<string>();
            try
            {
                foreach (RegistrosParaExportar registro in registrosParaExportar)
                {
                    string arquivo = registro.CaminhoArquivoTexto;
                    string pathAndNameZipFile = "";
                    try
                    {
                        bool connectionSftp = SFTP.conect_SFTP_UploadFile(sftpServer, sftpUser, sftpPassword, sftpPort);
                        if (connectionSftp)
                        {

                            string nameZipFile = CriarNomeDeArquivo(arquivo) + ".zip";

                            pathAndNameZipFile = pathZipFile + @"\" + nameZipFile;
                            list_pathAndNameZipFile.Add(pathAndNameZipFile);
                            CriarArquivoZip(arquivo, pathUploadedFiles + @"\" + arquivo, pathAndNameZipFile);

                            if (SFTP.SFTP_UploadFile(pathAndNameZipFile, pathSftp, nameZipFile))
                            {
                                idsSucess.Add(registro.Id);
                                Console.WriteLine(@"---------> Arquivo exportado. " + nomeDaBase + "... Id  = " + registro.Id);
                            }
                            else
                            {
                                Console.WriteLine(@"Erro na Exportação de arquivos de " + nomeDaBase + "... Id  = " + registro.Id);
                            }
                        }
                        else
                        {
                            throw new Exception("Erro na exportação de arquivos. Status Conexão SFTP: " + connectionSftp);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(@"Erro na exportação do arquivo: " + arquivo + ", da base " + nomeDaBase + ", Id " + registro.Id);
                        Log.LogarExcecao("Exportação de Arquivos", "Erro na exportação do arquivo: " + arquivo + ", da base " + nomeDaBase + ", Id " + registro.Id, ex);
                    }

                }
                if (bDeletar)
                {
                    foreach (var _pathAndNameZipFile in list_pathAndNameZipFile)
                    {
                        try
                        {
                            File.Delete(_pathAndNameZipFile);
                            Console.WriteLine("Arquivo deletado: " + _pathAndNameZipFile);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ERRO - Arquivo não deletado: " + _pathAndNameZipFile);
                            Log.LogarExcecao("Exportação de Arquivos", "Erro na exclusão do arquivo .zip: " + _pathAndNameZipFile + " da base " + nomeDaBase, ex);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.LogarExcecao("Exportação de Arquivos", "Erro na exportação de arquivos da base " + nomeDaBase, ex);
            }
            return idsSucess;
        }
    }
}