using System;
using System.Collections.Generic;
using System.Data;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class InteressadoAD : AD
    {
        private string _extentInteressado;

        public string ExtentInteressado
        {
            get { return _extentInteressado; }
        }

        public InteressadoAD()
        {
            _extentInteressado = Configuracao.LerValorChave(chaveBaseInteressado);
        }

        public void BuscarInteressadosEIndexar(string sql)
        {
            try
            {
                Console.WriteLine("Iniciando Thread " + _extentInteressado + "...");
                int total;
                int contPesquisa = 0;
                int contIndexacao = 0;
                int i = 0;
                int j = 0;
                List<Interessado> lista = new List<Interessado>();
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
                            Interessado interessado = new Interessado()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nome = Convert.ToString(reader["Nomenclatura"])
                            };
                            lista.Add(interessado);
                            Console.WriteLine("----------> Interessado montado: " + interessado.Id);
                        }
                        catch (Exception ex)
                        {
                            idsError.Add(reader["Id"].ToString()); //Se der bronca guarda o Id para catalogar o Id das normas deram erro
                        }
                        if (i >= 50)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentInteressado, lista, "Id");
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
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentInteressado, lista, "Id");
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
                    Log.LogarInformacao(todosIdsSucess, idsError, "Exportação de Interessados");
                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Erro na busca de todos os interessados...");
                Log.LogarExcecao("Exportação de Interessados", "Erro na busca de todos os interessados...", ex);
            }
        }

        public List<Interessado> ObtemInteressados(object[] interessados)
        {
            List<Interessado> lista = new List<Interessado>();
            string sql = string.Format("select * from {0}", _extentInteressado);
            var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
            conn.OpenConnection();
            using (var reader = conn.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    Interessado interessado = CarregaInteressado(reader);
                    foreach (int idInteressado in interessados)
                    {
                        if (idInteressado == interessado.Id)
                        {
                            lista.Add(interessado);
                            break;
                        }
                    }
                }
            }
            conn.CloseConection();
            return lista;
        }

        private static Interessado CarregaInteressado(IDataRecord reader)
        {
            Interessado interessado = new Interessado()
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = Convert.ToString(reader["Nomenclatura"])
            };

            return interessado;
        }
        
    }
}