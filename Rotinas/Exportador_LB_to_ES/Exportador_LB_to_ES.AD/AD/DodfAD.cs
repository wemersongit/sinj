using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;
using SINJ.ExtratorDeTexto;

namespace Exportador_LB_to_ES.AD.AD
{
    public class DodfAD : AD
    {

        private string _extentDodf;

        public string ExtentDodf
        {
            get { return _extentDodf; }
        }

        public DodfAD()
        {
            _extentDodf = Configuracao.LerValorChave(chaveBaseDodf);
        }

        /// <summary>
        /// busca todos os dodfs da base e indexa
        /// </summary>
        /// <param name="sql">consulta sql</param>
        public void BuscarDodfsEIndexar(string sql)
        {
            try
            {
                Console.WriteLine("Iniciando Processo Dodfs...");
                int total;
                int contPesquisa = 0;
                int contIndexacao = 0;
                int i = 0;
                int j = 0;
                List<Dodf> dodfs = new List<Dodf>();
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                Console.WriteLine("Conexão com banco = " + conn.GetConnectionState());
                using (var rdr = conn.ExecuteDataReader(sql))
                {
                    Console.WriteLine("Reader.Count = " + rdr.Count);
                    EsAD indexa = new EsAD();
                    List<string> idsControle = new List<string>();
                    List<string> todosIdsSucess = new List<string>();
                    List<string> idsError = new List<string>();
                    total = rdr.Count;

                    while (rdr.Read())
                    {
                        i++;
                        j++;
                        try
                        {
                            idsControle.Add(rdr["Id"].ToString()); //Pega todos os IdS
                            Dodf dodf = new Dodf();
                            dodf.Id = Convert.ToInt32(rdr["Id"]);
                            dodf.IdSileg = rdr["IdSileg"].ToString();
                            dodf.Rotulo = rdr["Rotulo"].ToString();
                            dodf.Comentario = rdr["Comentario"].ToString();
                            dodf.Comentario = rdr["Comentario"].ToString();
                            dodf.OrgaoCadastrador = rdr["OrgaoCadastrador"].ToString();
                            dodf.UsuarioQueCadastrou = rdr["UsuarioQueCadastrou"].ToString();
                            if (!(rdr["DataDaAssinatura"] is DBNull))
                            {
                                dodf.DataDaAssinatura = Convert.ToDateTime(rdr["DataDaAssinatura"]).ToString(DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern);
                            }
                            if (!(rdr["DataDoCadastro"] is DBNull))
                            {
                                string data = rdr["DataDoCadastro"].ToString();
                                dodf.DataDoCadastro = Convert.ToDateTime(data, CultureInfo.GetCultureInfo("pt-BR"));
                            }
                            dodf.UsuarioDaUltimaAlteracao = rdr["UsuarioDaUltimaALteracao"].ToString();
                            if (!(rdr["DataDaUltimaAlteracao"] is DBNull))
                            {
                                string data = rdr["DataDaUltimaAlteracao"].ToString();
                                dodf.DataDaUltimaAlteracao = Convert.ToDateTime(data, CultureInfo.GetCultureInfo("pt-BR"));
                            }
                            dodf.ChaveParaNaoDuplicacao = rdr["ChaveParaNaoDuplicacao"].ToString();
                            dodf.SituacaoQuantoAPendencia = rdr["SituacaoQuantoAPendencia"].ToString();
                            dodf.AlfaNumero = rdr["AlfaNumero"].ToString();
                            Regex digitsOnly = new Regex(@"[^\d]");
                            dodf.Numero = Convert.ToInt32(digitsOnly.Replace(dodf.AlfaNumero, ""));
                            dodf.Sessao = rdr["Sessao"].ToString();
                            dodf.CaminhoArquivoTexto = rdr["CaminhoArquivoTexto"].ToString();
                            dodf.ConteudoArquivoTexto = rdr["ConteudoArquivoTexto"].ToString();
                            try
                            {
                                bool extrairTexto = bool.Parse(Configuracao.LerValorChave(chaveFlagExtrairTextoArquivos));
                                if (extrairTexto)
                                {
                                    ManagerExtratorDeTexto managerExtratorDeTexto = new ManagerExtratorDeTexto(Configuracao.LerValorChave(chaveLightBaseConnectionString));

                                    bool salvarTexto = bool.Parse(Configuracao.LerValorChave(chaveFlagSalvarTextoArquivos));
                                    if (salvarTexto)
                                    {
                                        dodf.ConteudoArquivoTexto = managerExtratorDeTexto.ExtrairTexto(dodf.Id.ToString(),
                                                                       _extentDodf, "Id",
                                                                       "ConteudoArquivoTexto",
                                                                       "CaminhoArquivoTexto",
                                                                       Configuracao.LerValorChave(chavePathOfTheUploadedFiles));
                                    }
                                    else
                                    {
                                        dodf.ConteudoArquivoTexto = managerExtratorDeTexto.ExtrairTexto(dodf.Id.ToString(),
                                                                   _extentDodf, "Id",
                                                                   "CaminhoArquivoTexto",
                                                                   Configuracao.LerValorChave(chavePathOfTheUploadedFiles));
                                    }
                                }

                                EsAD.ValidaSerializacao(dodf);
                            }
                            catch (Exception ex)
                            {
                                Log.LogarExcecao("Exportação de DODFs", "Erro ao extrair texto de DODF. Id " + dodf.Id, ex);
                            }
                            if (rdr["NomeArquivoTexto"] != DBNull.Value)
                            {
                                dodf.NomeArquivoTexto = rdr["NomeArquivoTexto"].ToString();
                            }
                            else
                            {
                                dodf.NomeArquivoTexto = ""; ;
                            }
                            dodfs.Add(dodf);
                            Console.WriteLine("----------> Dodf montado: " + dodf.Id);
                        }
                        catch (Exception ex)
                        {
                            idsError.Add(rdr["Id"].ToString()); //Se der bronca guarda o Id para catalogar o Id dos dodfs que deram erro
                            Log.LogarExcecao("Exportação de DODFs", "Erro ao Montar DODF. Id " + rdr["Id"], ex);
                        }
                        if (i >= 50)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentDodf, dodfs, "Id");
                            todosIdsSucess.AddRange(idsSucess);
                            i = 0;
                            //Varre todos os Ids para achar os que não foram indexados e adiciona-los à lista idsError
                            foreach (string id in idsControle)
                            {
                                if (!idsSucess.Contains(id))
                                {
                                    if (!idsError.Contains(id))
                                    {
                                        idsError.Add(id);
                                    }
                                }
                            }
                            contPesquisa += idsControle.Count;
                            contIndexacao += idsSucess.Count;
                            dodfs.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();

                        }
                        else if (j == total)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentDodf, dodfs, "Id");
                            todosIdsSucess.AddRange(idsSucess);
                            i = 0;
                            //Varre todos os Ids para achar os que não foram indexados e adiciona-los à lista idsError
                            foreach (string id in idsControle)
                            {
                                if (!idsSucess.Contains(id))
                                {
                                    if (!idsError.Contains(id))
                                    {
                                        idsError.Add(id);
                                    }
                                }
                            }
                            contPesquisa += idsControle.Count;
                            contIndexacao += idsSucess.Count;
                            dodfs.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();
                        }
                    }
                    Log.LogarInformacao(todosIdsSucess, idsError, "Exportação de DODFs");

                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Erro na busca de todos os dodfs...");
                Log.LogarExcecao("Exportação de DODFs", "Erro na busca de todos os dodfs...", ex);
            }
        }

        public List<RegistrosParaExportar> BuscarCaminhosDeArquivosDeDodf(string sql)
        {
            List<RegistrosParaExportar> registrosParaExportar = new List<RegistrosParaExportar>();
            try
            {
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                using (var rdr = conn.ExecuteDataReader(sql))
                {
                    while (rdr.Read())
                    {
                        try
                        {
                            string caminhoArquivo = Configuracao.LerValorChave(chaveCampoCaminhoArquivoTextoDodfs);
                            if (rdr[caminhoArquivo].ToString() != "")
                            {
                                RegistrosParaExportar registro = new RegistrosParaExportar();
                                registro.Id = rdr["id"].ToString();
                                registro.Nome = _extentDodf;
                                registro.CaminhoArquivoTexto = rdr[caminhoArquivo].ToString();
                                registrosParaExportar.Add(registro);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.LogarExcecao("Exportação de arquivos de DODFs", "Erro ao obter arquivos da dodfs. DODF = " + rdr["Id"], ex);
                        }
                    }
                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Log.LogarExcecao("Exportação de Arquivos de DODF", "Erro ao consultar caminhos de arquivos de dodfs.", ex);
            }
            return registrosParaExportar;
        }

        public void ConfigurarMapping()
        {
            string extent = _extentDodf;
            string jsonMapping = Configuracao.LerValorChave(chaveMappingDodfs);
            string uriElasticSearch = Configuracao.LerValorChave(chaveElasticSearch);
            if (jsonMapping != "")
            {
                new EsAD().ConfigurarIndexType(uriElasticSearch, jsonMapping, extent);
            }
        }

    }
}