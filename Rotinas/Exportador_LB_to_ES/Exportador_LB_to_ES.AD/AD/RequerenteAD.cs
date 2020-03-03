using System;
using System.Collections.Generic;
using System.Data;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class RequerenteAD : AD
    {
        private string _extentRequerente;
        public string ExtentRequerente
        {
            get { return _extentRequerente; }
        }
        public RequerenteAD()
        {
            _extentRequerente = Configuracao.LerValorChave(chaveBaseRequerente);
        }
        public void BuscarRequerentesEIndexar(string sql)
        {
            try
            {
                Console.WriteLine("Iniciando Processo Requerentes...");
                int total;
                int contPesquisa = 0;
                int contIndexacao = 0;
                int i = 0;
                int j = 0;
                List<Requerente> requerentes = new List<Requerente>();
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
                            Requerente requerente = new Requerente
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nome = Convert.ToString(reader["Nomenclatura"])
                            };
                            requerentes.Add(requerente);
                            Console.WriteLine("----------> Requerente montado: " + requerente.Id);
                        }
                        catch (Exception ex)
                        {
                            idsError.Add(reader["Id"].ToString()); //Se der bronca guarda o Id para catalogar o Id das normas deram erro
                        }
                        if (i >= 50)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentRequerente, requerentes, "Id");
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
                            requerentes.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();

                        }
                        else if (j == total)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentRequerente, requerentes, "Id");
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
                            requerentes.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();
                        }
                    }
                    Log.LogarInformacao(todosIdsSucess, idsError, "Exportação de Requerentes");
                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Erro na busca de todos os requentes...");
                Log.LogarExcecao("Exportação de Requerentes", "Erro na busca de todos requerentes...", ex);
            }
        }
        public List<Requerente> ObtemRequerentes(object[] requerentes)
        {
            List<Requerente> lista = new List<Requerente>();
            string sql = string.Format("select * from {0}", _extentRequerente);
            var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
            conn.OpenConnection();
            using (var reader = conn.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    Requerente requerente = CarregaRequerente(reader);
                    foreach (int idRequerente in requerentes)
                    {
                        if (idRequerente == requerente.Id)
                        {
                            lista.Add(requerente);
                            break;
                        }
                    }
                }
            }
            conn.CloseConection();
            return lista;
        }

        private static Requerente CarregaRequerente(IDataRecord reader)
        {
            Requerente requerente = new Requerente
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = Convert.ToString(reader["Nomenclatura"])
            };

            return requerente;
        }
    }
}