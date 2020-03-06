using System;
using System.Collections.Generic;
using Exportador_LB_to_ES.AD.AD;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.Sub_Process
{
    public class Exportar
    {
        private ArquivoAD _arquivoAd;
        private AtualizadoAD _atualizadoAd;
        private AutoriaAD _autoriaAd;
        private DodfAD _dodfAd;
        private ExcluidoAD _excluidoAd;
        private InteressadoAD _interessadoAd;
        private NormaAD _normaAd;
        private OrgaoAD _orgaoAd;
        private PushAD _pushAd;
        private RequerenteAD _requerenteAd;
        private RequeridoAD _requeridoAd;
        private TermoAD _termoAd;
        private TipoDeNormaAD _tipoDeNormaAd;
        private TipoDeRelacaoAD _tipoDeRelacaoAd;

        public Exportar()
        {
            _arquivoAd = new ArquivoAD();
            _atualizadoAd = new AtualizadoAD();
            _autoriaAd = new AutoriaAD();
            _dodfAd = new DodfAD();
            _excluidoAd = new ExcluidoAD();
            _interessadoAd = new InteressadoAD();
            _normaAd = new NormaAD();
            _orgaoAd = new OrgaoAD();
            _pushAd = new PushAD();
            _requerenteAd = new RequerenteAD();
            _requeridoAd = new RequeridoAD();
            _termoAd = new TermoAD();
            _tipoDeNormaAd = new TipoDeNormaAD();
            _tipoDeRelacaoAd = new TipoDeRelacaoAD();
        }

        private void IndexarAtualizados(string query, bool exportFiles)
        {
            try
            {
                List<RegistrosParaExportar> registrosParaExportar = _atualizadoAd.BuscarAtualizados(query);
                string sRegistros = "";
                if (registrosParaExportar.Count > 0)
                {
                    foreach (var registroParaExportar in registrosParaExportar)
                    {
                        sRegistros += (sRegistros != "" ? ", " : "") + registroParaExportar.Nome + " " + registroParaExportar.Id;
                    }
                    Log.LogarInformacao("Buscar registros para exportar", " Registros selecionados: ");
                    _atualizadoAd.DespacharAtualizadosParaIndexacaoEExportacao(registrosParaExportar);
                    if (exportFiles)
                    {
                        new ArquivoAD().ExportarArquivos(registrosParaExportar);
                    }
                    _atualizadoAd.DeletarRegistrosBaseForIndexerAndExporter();
                }
                else
                {
                    Log.LogarInformacao("Buscar registros para exportar", "Nenhum registro encontrado.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(@"Erro no processo de atualizados...");
                Log.LogarExcecao("Exportação de Atualizados", "Erro no processo de atualizados...", ex);
            }
        }

        public void Indexar(string extent, string query, bool exportFiles)
        {
            if(extent == _orgaoAd.ExtentOrgao)
            {
                IndexarOrgaos(query);
                return;
            }
            if(extent == _tipoDeNormaAd.ExtentTipoDeNorma)
            {
                IndexarTiposDeNorma(query);
                return;
            }
            if(extent == _interessadoAd.ExtentInteressado)
            {
                IndexarInteressados(query);
                return;
            }
            if(extent == _autoriaAd.ExtentAutoria)
            {
                IndexarAutorias(query);
                return;
            }
            if(extent == _requerenteAd.ExtentRequerente)
            {
                IndexarRequerentes(query);
                return;
            }
            if(extent == _requeridoAd.ExtentRequerido)
            {
                IndexarRequeridos(query);
                return;
            }
            if(extent == _pushAd.ExtentPush)
            {
                IndexarPush(query);
                return;
            }
            if(extent == _termoAd.ExtentVocabularioControlado)
            {
                IndexarVocabularioControlado(query);
            }
            if(extent == _tipoDeRelacaoAd.ExtentTipoDeRelacao)
            {
                IndexarTiposDeRelacao(query);
            }
            if(extent == _atualizadoAd.ExtentAtualizado)
            {
                IndexarAtualizados(query, exportFiles);
                DeletarRegistrosDoElasticDeletadosDoLightBase();
            }
            if (extent == _normaAd.ExtentNorma)
            {
                IndexarNormas(query, exportFiles);
                return;
            }
            if (extent == _dodfAd.ExtentDodf)
            {
                IndexarDodfs(query, exportFiles);
                return;
            }
            if (extent == "ArquivosNormas")
            {
                ExportarArquivosDeNormas(query);
                return;
            }
            if (extent == "ArquivosDodfs")
            {
                ExportarArquivosDeDodfs(query);
                return;
            }
        }

        private void IndexarNormas(string query, bool exportarArquivos)
        {
            _normaAd.BuscarNormasEIndexar(query);
            Console.WriteLine(@"Exportar Arquivos = " + exportarArquivos);
            if (exportarArquivos)
            {
                ExportarArquivosDeNormas(query);
            }
        }

        private void IndexarDodfs(string query, bool exportarArquivos)
        {
            _dodfAd.BuscarDodfsEIndexar(query);
            if (exportarArquivos)
            {
                ExportarArquivosDeDodfs(query); 
            }
        }

        private void IndexarOrgaos(string query)
        {
            _orgaoAd.BuscarOrgaosEIndexar(query);
        }

        private void IndexarTiposDeNorma(string query)
        {
            _tipoDeNormaAd.BuscarTiposDeNormaEIndexar(query);
        }

        //private void IndexacaoDeAutoridades(string query)
        //{
        //    BuscaRegistros buscaRegistros = new BuscaRegistros();
        //    buscaRegistros.BuscaAutoridadesEIndexa(query);
        //}



        private void IndexarAutorias(string query)
        {
            _autoriaAd.BuscarAutoriasEIndexar(query);
        }

        private void IndexarRequerentes(string query)
        {
            _requerenteAd.BuscarRequerentesEIndexar(query);
        }

        private void IndexarRequeridos(string query)
        {
            _requeridoAd.BuscarRequeridosEIndexar(query);
        }

        private void IndexarInteressados(string query)
        {
            _interessadoAd.BuscarInteressadosEIndexar(query);
        }

        private void IndexarPush(string query)
        {
            _pushAd.BuscarPushEIndexar(query);
        }

        //public void IndexacaoDeDescritores(string query)
        //{
        //    BuscaRegistros buscaRegistros = new BuscaRegistros();
        //    buscaRegistros.BuscaDescritoresEIndexa(query);
        //}

        //public void IndexacaoDeEspecificadores(string query)
        //{
        //    BuscaRegistros buscaRegistros = new BuscaRegistros();
        //    buscaRegistros.BuscaEspecificadoresEIndexa(query);
        //}

        private void IndexarVocabularioControlado(string query)
        {
            _termoAd.BuscarVocabularioControladoEIndexar(query);
        }

        private void IndexarTiposDeRelacao(string query)
        {
            _tipoDeRelacaoAd.BuscarTiposDeRelacaoEIndexar(query);
        }

        private void ExportarArquivosDeNormas(string query)
        {
            List<RegistrosParaExportar> registros = _normaAd.BuscarCaminhosDeArquivosDeNorma(query);
            _arquivoAd.ExportarArquivos(registros, "normas");
        }

        private void ExportarArquivosDeDodfs(string query)
        {
            List<RegistrosParaExportar> registros = _dodfAd.BuscarCaminhosDeArquivosDeDodf(query);
            _arquivoAd.ExportarArquivos(registros, "dodfs");
        }

        ///// <summary>
        ///// Exporta arquivos para um servidor SFTP
        ///// </summary>
        ///// <param name="registros">Lista registros com os caminhos dos arquivos</param>
        ///// <param name="cont">Valor inteiro usado para indicar o início da barra de progresso</param>
        ///// <param name="total">Valor inteiro usado para indicar o total da barra de progresso</param>
        ///// <param name="nomeDaBase">Nome da base à qual os arquivos pertencem, auxilia nos feedbacks e logs</param>
        //public List<string> ExportaArquivos(List<RegistrosParaExportar> registros, int cont, int total, string nomeDaBase)
        //{
        //    List<string> idsSucess = new List<string>();
        //    string pathZipFile = AppDomain.CurrentDomain.BaseDirectory + "ArquivosParaExportar";
        //    if (!Directory.Exists(pathZipFile))
        //    {
        //        Directory.CreateDirectory(pathZipFile);
        //    }
        //    string localDosArquivos = Configuracao.NomedoExtentDePathOfTheUploadedFiles;
        //    try
        //    {
        //        foreach (RegistrosParaExportar registro in registros)
        //        {
        //            try
        //            {
        //                if (SFTP.conect_SFTP_UploadFile(Configuracao.NomeDoExtentDeSftpServer, Configuracao.NomeDoExtentDeSftpUser, Configuracao.NomeDoExtentDeSftpPassword, Configuracao.NomeDoExtentDeSftpPort))
        //                {

        //                    string nameZipFile = FileNameCreate(registro.CaminhoArquivoTexto) + ".zip";

        //                    string pathAndNameZipFile = pathZipFile + @"\" + nameZipFile;

        //                    CreateFiles(registro.CaminhoArquivoTexto, localDosArquivos + @"\" + registro.CaminhoArquivoTexto, pathAndNameZipFile);

        //                    if (SFTP.SFTP_UploadFile(pathAndNameZipFile, Configuracao.NomedoExtentDePathToSaveOnSftp,
        //                                              nameZipFile))
        //                    {
        //                        idsSucess.Add(registro.Id);
        //                        Console.WriteLine(@"---------> Arquivo exportado. " + nomeDaBase + "... Id  = " + registro.Id);
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine(@"Erro na Exportação de arquivos de " + nomeDaBase + "... Id  = " + registro.Id);
        //                    }



        //                    cont++;

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.WriterLogs.WriterException(ex, "Erro na exportação do arquivo " + registro.CaminhoArquivoTexto + " da base " + nomeDaBase);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriterLogs.WriterException(ex, "Erro na exportação de arquivos da base " + nomeDaBase);
        //    }
        //    return idsSucess;
        //}

        

        //public static string ParsePdf(string fileName)
        //{
        //    if (!File.Exists(fileName))
        //    {
        //        throw new FileNotFoundException("fileName");
        //    }
        //    PdfReader reader = new PdfReader(fileName);
        //    StringBuilder sb = new StringBuilder();

        //    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
        //    for (int page = 0; page < reader.NumberOfPages; page++)
        //    {
        //        string text = PdfTextExtractor.GetTextFromPage(reader, page + 1, strategy);
        //        if (!string.IsNullOrEmpty(text))
        //        {
        //            sb.Append(Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text))));
        //        }
        //    }
        //    reader.Close();

        //    return sb.ToString();
        //}

        

        public void DeletarRegistrosDoElasticDeletadosDoLightBase()
        {
            _excluidoAd.DeletarRegistrosDoElasticDeletadosDoLightBase();
        }

        public void IniciarProcessoMapping()
        {
            _tipoDeNormaAd.ConfigurarMapping();
            _orgaoAd.ConfigurarMapping();
            _dodfAd.ConfigurarMapping();
            _normaAd.ConfigurarMapping();

        }

        public void IniciarProcessoDelete()
        {
            Console.WriteLine("Iniciando Deletar Registros do SINJ PORTAL...");
            
            List<RegistrosParaDeletar> registrosParaDeletar = _excluidoAd.BuscarDeletadosComparandoLbwComElasticSearch();
            Console.WriteLine("Deletando os Registros no PORTAL...");
            int deletados = _excluidoAd.DespacharDeletadosParaDelecao(registrosParaDeletar);
            Console.WriteLine("Total arquivos deletados no SINJ de Pesquisa: " + deletados);
        }
    }
}
