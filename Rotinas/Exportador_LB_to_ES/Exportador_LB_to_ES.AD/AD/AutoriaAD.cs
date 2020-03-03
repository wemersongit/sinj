using System;
using System.Collections.Generic;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class AutoriaAD : AD
    {
        private string _extentAutoria;
        public string ExtentAutoria
        {
            get { return _extentAutoria; }
        }
        public AutoriaAD()
        {
            _extentAutoria = Configuracao.LerValorChave(chaveBaseAutoria);
        }

        public void BuscarAutoriasEIndexar(string sql)
        {
            try
            {
                Console.WriteLine("Iniciando Processo Autorias...");
                int total;
                int contPesquisa = 0;
                int contIndexacao = 0;
                int i = 0;
                int j = 0;
                List<Autoria> autorias = new List<Autoria>();
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
                            idsControle.Add(reader["Id"].ToString()); //Pega todos os IdS
                            Autoria autoria = new Autoria
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nome = Convert.ToString(reader["Nome"])
                            };
                            autorias.Add(autoria);
                            Console.WriteLine("----------> Autoria montada: " + autoria.Id);
                        }
                        catch (Exception ex)
                        {
                            idsError.Add(reader["Id"].ToString()); //Se der bronca guarda o Id para catalogar o Id das normas deram erro
                        }
                        if (i >= 50)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentAutoria, autorias, "Id");
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
                            autorias.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();

                        }
                        else if (j == total)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentAutoria, autorias, "Id");
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
                            autorias.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();
                        }
                    }
                    Log.LogarInformacao(todosIdsSucess, idsError, "Exportação de Autorias");
                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Erro na busca de todas as Autorias... " + ex.Message);
                Console.WriteLine(@"Exception Message: " + ex.Message);
                Console.Read();
                Log.LogarExcecao("Exportação de Autorias", "Erro na busca de todas Autorias...", ex);
            }
        }
    }
}