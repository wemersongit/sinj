using System;
using System.Collections.Generic;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class TipoDeNormaAD : AD
    {
        private string _extentTipoDeNorma;

        public string ExtentTipoDeNorma
        {
            get { return _extentTipoDeNorma; }
        }

        public TipoDeNormaAD()
        {
            _extentTipoDeNorma = Configuracao.LerValorChave(chaveBaseTipoDeNorma);
        }

        public void BuscarTiposDeNormaEIndexar(string sql)
        {
            try
            {
                Console.WriteLine("Iniciando Processo TiposDeNorma...");
                int total;
                int contPesquisa = 0;
                int contIndexacao = 0;
                int i = 0;
                int j = 0;
                List<TipoDeNorma> tiposDeNorma = new List<TipoDeNorma>();
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
                            TipoDeNorma tipoDeNorma = new TipoDeNorma();
                            tipoDeNorma.Id = Convert.ToInt32(reader["Id"]);
                            tipoDeNorma.Nome = reader["Nome"].ToString();
                            tipoDeNorma.Descricao = reader["Descricao"].ToString();
                            tipoDeNorma.TCDF = Convert.ToBoolean(reader["TCDF"]);
                            tipoDeNorma.SEPLAG = Convert.ToBoolean(reader["SEPLAG"]);
                            tipoDeNorma.CLDF = Convert.ToBoolean(reader["CLDF"]);
                            tipoDeNorma.PGDF = Convert.ToBoolean(reader["PGDF"]);
                            tipoDeNorma.Grupo1 = Convert.ToBoolean(reader["Grupo1"]);
                            tipoDeNorma.Grupo2 = Convert.ToBoolean(reader["Grupo2"]);
                            tipoDeNorma.Grupo3 = Convert.ToBoolean(reader["Grupo3"]);
                            tipoDeNorma.Grupo4 = Convert.ToBoolean(reader["Grupo4"]);
                            tipoDeNorma.Grupo5 = Convert.ToBoolean(reader["Grupo5"]);
                            tipoDeNorma.Conjunta = Convert.ToBoolean(reader["Conjunta"]);
                            tipoDeNorma.Questionaveis = Convert.ToBoolean(reader["Questionaveis"]);
                            tipoDeNorma.ControleDeNumeracaoPorOrgao = Convert.ToBoolean(reader["ControleDeNumeracaoPorOrgao"]);
                            tiposDeNorma.Add(tipoDeNorma);
                            Console.WriteLine("----------> tipo de norma montada: " + tipoDeNorma.Id);
                        }
                        catch (Exception ex)
                        {
                            idsError.Add(reader["Id"].ToString()); //Se der bronca guarda o Id para catalogar o Id das normas deram erro
                        }
                        if (i >= 50)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentTipoDeNorma, tiposDeNorma, "Id");
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
                            tiposDeNorma.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();

                        }
                        else if (j == total)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentTipoDeNorma, tiposDeNorma, "Id");
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
                            tiposDeNorma.Clear();
                            idsControle.Clear();
                            idsSucess.Clear();
                        }
                    }
                    Log.LogarInformacao(todosIdsSucess, idsError, "Exportação de TipoDeNorma");
                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Erro na busca de todos os tipos de norma...");
                Log.LogarExcecao("Exportação de TipoDeNorma", "Erro na busca de todos os tipos de norma...", ex);
            }
        }

        /// <summary>
        /// Retorna o Tipo de Norma
        /// </summary>
        /// <param name="id_Tipo">Id do Tipo de Norma</param>
        /// <returns>Retorna um Tipo de Norma</returns>
        public TipoDeNorma ObtemTipoDeNorma(string id_Tipo)
        {
            TipoDeNorma tipoDeNorma = new TipoDeNorma();
            string sql = string.Format("select * from TiposDeNorma where Id = {0}", id_Tipo);
            var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
            conn.OpenConnection();
            using (var rdr = conn.ExecuteDataReader(sql))
            {
                while (rdr.Read())
                {
                    tipoDeNorma.Id = Convert.ToInt32(rdr["Id"]);
                    tipoDeNorma.Nome = rdr["Nome"].ToString();
                    tipoDeNorma.Descricao = rdr["Descricao"].ToString();
                    tipoDeNorma.TCDF = Convert.ToBoolean(rdr["TCDF"]);
                    tipoDeNorma.SEPLAG = Convert.ToBoolean(rdr["SEPLAG"]);
                    tipoDeNorma.CLDF = Convert.ToBoolean(rdr["CLDF"]);
                    tipoDeNorma.PGDF = Convert.ToBoolean(rdr["PGDF"]);
                    tipoDeNorma.Grupo1 = Convert.ToBoolean(rdr["Grupo1"]);
                    tipoDeNorma.Grupo2 = Convert.ToBoolean(rdr["Grupo2"]);
                    tipoDeNorma.Grupo3 = Convert.ToBoolean(rdr["Grupo3"]);
                    tipoDeNorma.Grupo4 = Convert.ToBoolean(rdr["Grupo4"]);
                    tipoDeNorma.Grupo5 = Convert.ToBoolean(rdr["Grupo5"]);
                    tipoDeNorma.Conjunta = Convert.ToBoolean(rdr["Conjunta"]);
                    tipoDeNorma.Questionaveis = Convert.ToBoolean(rdr["Questionaveis"]);
                    tipoDeNorma.ControleDeNumeracaoPorOrgao = Convert.ToBoolean(rdr["ControleDeNumeracaoPorOrgao"]);
                }
            }
            conn.CloseConection();
            return tipoDeNorma;
        }

        public void ConfigurarMapping()
        {
            string extent = _extentTipoDeNorma;
            string jsonMapping = Configuracao.LerValorChave(chaveMappingTipos);
            string uriElasticSearch = Configuracao.LerValorChave(chaveElasticSearch);
            if (jsonMapping != "")
            {
                new EsAD().ConfigurarIndexType(uriElasticSearch, jsonMapping, extent);
            }
        }
    }
}