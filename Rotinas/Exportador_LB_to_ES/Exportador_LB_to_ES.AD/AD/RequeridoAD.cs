using System;
using System.Collections.Generic;
using System.Data;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class RequeridoAD : AD
    {
        private string _extentRequerido;
        public string ExtentRequerido
        {
            get { return _extentRequerido; }
        }
        public RequeridoAD()
        {
            _extentRequerido = Configuracao.LerValorChave(chaveBaseRequerido);
        }

        public void BuscarRequeridosEIndexar(string sql)
        {
            try
            {
                Console.WriteLine("Iniciando Processo Requeridos...");
                int total;
                int contPesquisa = 0;
                int contIndexacao = 0;
                int i = 0;
                int j = 0;
                List<Requerido> requeridos = new List<Requerido>();
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
                            Requerido requerido = new Requerido
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nome = Convert.ToString(reader["Nomenclatura"])
                            };
                            requeridos.Add(requerido);
                            Console.WriteLine("----------> Requerido montado: " + requerido.Id);
                        }
                        catch (Exception ex)
                        {
                            idsError.Add(reader["Id"].ToString()); //Se der bronca guarda o Id para catalogar o Id das normas deram erro
                        }
                        if (i >= 50)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentRequerido, requeridos, "Id");
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
                            requeridos.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();

                        }
                        else if (j == total)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentRequerido, requeridos, "Id");
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
                            requeridos.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();
                        }
                    }
                    Log.LogarInformacao(todosIdsSucess, idsError, "Exportação de Requeridos");
                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Erro na busca de todos os Requeridos...");
                Log.LogarExcecao("Exportação de Requeridos", "Erro na busca de todos Requeridos...", ex);
            }
        }

        public List<Requerido> ObtemRequeridos(object[] requeridos)
        {
            List<Requerido> lista = new List<Requerido>();
            string sql = string.Format("select * from {0}", _extentRequerido);
            var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
            conn.OpenConnection();
            using (var reader = conn.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    Requerido requerido = CarregaRequerido(reader);
                    foreach (int idRequerido in requeridos)
                    {
                        if (idRequerido == requerido.Id)
                        {
                            lista.Add(requerido);
                            break;
                        }
                    }
                }
            }
            conn.CloseConection();
            return lista;
        }

        private static Requerido CarregaRequerido(IDataRecord reader)
        {   
            Requerido requerido = new Requerido
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = Convert.ToString(reader["Nomenclatura"])
            };

            return requerido;
        }
    }
}