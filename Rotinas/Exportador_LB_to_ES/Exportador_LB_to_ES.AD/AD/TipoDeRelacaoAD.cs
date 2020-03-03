using System;
using System.Collections.Generic;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class TipoDeRelacaoAD : AD
    {
        private string _extentTipoDeRelacao;
        public string ExtentTipoDeRelacao
        {
            get { return _extentTipoDeRelacao; }
        }
        public TipoDeRelacaoAD()
        {
            _extentTipoDeRelacao = Configuracao.LerValorChave(chaveBaseTipoDeRelacao);
        }
        public void BuscarTiposDeRelacaoEIndexar(string sql)
        {
            try
            {
                Console.WriteLine("Iniciando Processo " + _extentTipoDeRelacao + "...");
                int total;
                int contPesquisa = 0;
                int contIndexacao = 0;
                int i = 0;
                int j = 0;
                List<TipoDeRelacaoDeVinculo> lista = new List<TipoDeRelacaoDeVinculo>();
                var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
                conn.OpenConnection();
                Console.WriteLine("Conexão com banco = " + conn.GetConnectionState());
                using (var reader = conn.ExecuteDataReader(sql))
                {
                    EsAD indexa = new EsAD();
                    List<string> idsControle = new List<string>();
                    List<string> todosIdsSucess = new List<string>();
                    List<string> idsError = new List<string>();
                    total = reader.Count;

                    while (reader.Read())
                    {
                        i++;
                        j++;
                        try
                        {
                            idsControle.Add(reader["oid"].ToString()); //Pega todos os IdS
                            TipoDeRelacaoDeVinculo tipo = new TipoDeRelacaoDeVinculo();
                            tipo.Conteudo = Convert.ToString(reader["Conteudo"]);
                            tipo.Descricao = Convert.ToString(reader["Descricao"]);
                            tipo.Importancia = Convert.ToInt32(reader["Importancia"]);
                            tipo.Oid = Convert.ToInt32(reader["oid"]);
                            tipo.TextoParaAlterado = Convert.ToString(reader["TextoParaAlterado"]);
                            tipo.TextoParaAlterador = Convert.ToString(reader["TextoParaAlterador"]);
                            bool relacaoDeAcao = reader["RelacaoDeAcao"] is bool ? Convert.ToBoolean(reader["RelacaoDeAcao"]) : false;
                            tipo.RelacaoDeAcao = relacaoDeAcao;
                            lista.Add(tipo);
                            Console.WriteLine("----------> TipoDeRelacaoDeVinculo montado: " + tipo.Oid);
                        }
                        catch (Exception ex)
                        {
                            idsError.Add(reader["oid"].ToString()); //Se der bronca guarda o Id para catalogar o Id das normas deram erro
                        }
                        if (i >= 50)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentTipoDeRelacao, lista, "Oid");
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
                            lista.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();

                        }
                        else if (j == total)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentTipoDeRelacao, lista, "Oid");
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
                            lista.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();
                        }
                    }
                    Log.LogarInformacao(todosIdsSucess, idsError, "Exportação de TiposDeRelacao");
                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Erro na busca de TipoDeRelacaoDeVinculo...");
                Log.LogarExcecao("Exportação de TiposDeRelacao", "Erro na busca de TipoDeRelacaoDeVinculo...", ex);
            }
        }
    }
}