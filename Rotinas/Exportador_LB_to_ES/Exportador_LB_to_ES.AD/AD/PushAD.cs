using System;
using System.Collections.Generic;
using System.Data;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class PushAD : AD
    {
        private string _extentPush;
        public string ExtentPush
        {
            get { return _extentPush; }
        }
        public PushAD()
        {
            _extentPush = Configuracao.LerValorChave(chaveBasePush);
        }
        public void BuscarPushEIndexar(string sql)
        {
            try
            {
                Console.WriteLine("Iniciando Processo " + _extentPush + "...");
                int total;
                int contPesquisa = 0;
                int contIndexacao = 0;
                int i = 0;
                int j = 0;
                List<Push> lista = new List<Push>();
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
                            Push push = new Push();
                            push.Id = Convert.ToInt32(reader["Id"]);
                            push.Nome = reader["nome"].ToString();
                            push.Email = reader["email"].ToString();
                            push.Senha = reader["senha"].ToString();
                            push.DtCadUsuario = Convert.ToDateTime(reader["DtCadUsuario"]);
                            push.AtivoUsuario = Convert.ToBoolean(reader["AtivoUsuario"]);
                            push.NovosAtosPorCriteriosValue = new List<NovosAtosPorCriterios>();
                            if (reader["NovosAtosPorCriterios"] is DataTable)
                            {
                                DataTable tableNovosAtosPorCriterios = (DataTable)reader["NovosAtosPorCriterios"];
                                foreach (DataRow rowNovosAtosPorCriterios in tableNovosAtosPorCriterios.Rows)
                                {
                                    try
                                    {
                                        push.NovosAtosPorCriteriosValue.Add(CarregaNovosAtosPorCriterio(rowNovosAtosPorCriterios));
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.LogarExcecao("Exportação Push", "Erro Carregando NovosAtosPorCriterios " + reader["Id"], ex);
                                    }
                                }
                            }
                            push.AtosVerifAtlzcaoValue = new List<AtosVerifAtlzcao>();
                            if (reader["AtosVerifAtlzcao"] is DataTable)
                            {
                                DataTable tableAtosVerifAtlzcao = (DataTable)reader["AtosVerifAtlzcao"];
                                foreach (DataRow rowAtosVerifAtlzcao in tableAtosVerifAtlzcao.Rows)
                                {
                                    try
                                    {
                                        push.AtosVerifAtlzcaoValue.Add(CarregaAtosVerifAtlzcao(rowAtosVerifAtlzcao));
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.LogarExcecao("Exportação Push", "Erro Carregando AtosVerifAtlzcao " + reader["Id"], ex);
                                    }
                                }
                            }
                            lista.Add(push);
                            Console.WriteLine("----------> Push montado: " + push.Id);
                        }
                        catch (Exception ex)
                        {
                            idsError.Add(reader["Id"].ToString()); //Se der bronca guarda o Id para catalogar o Id das normas deram erro
                        }
                        if (i >= 50)
                        {
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentPush, lista, "Id");
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
                            List<string> idsSucess = indexa.IndexarNoElasticSearch(Configuracao.LerValorChave(chaveElasticSearch), _extentPush, lista, "Id");
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
                    Log.LogarInformacao(todosIdsSucess, idsError, "Exportação de Push");
                }
                conn.CloseConection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Erro na busca de todos push...");
                Log.LogarExcecao("Exportação de Push", "Erro na busca de todos push...", ex);
            }
        }

        private NovosAtosPorCriterios CarregaNovosAtosPorCriterio(DataRow rowNovosAtosPorCriterio)
        {
            NovosAtosPorCriterios novosAtosPorCriterios = new NovosAtosPorCriterios();
            novosAtosPorCriterios.IdNovosAtosPorCriterios =
                rowNovosAtosPorCriterio["IdNovosAtosPorCriterios"].ToString();
            novosAtosPorCriterios.TipoAto = Convert.ToInt32(rowNovosAtosPorCriterio["TipoAto"]);
            novosAtosPorCriterios.PrimeiroConec = rowNovosAtosPorCriterio["PrimeiroConec"].ToString();
            novosAtosPorCriterios.Origem = Convert.ToInt32(rowNovosAtosPorCriterio["Origem"]);
            novosAtosPorCriterios.SegundoConec = rowNovosAtosPorCriterio["SegundoConec"].ToString();
            novosAtosPorCriterios.Indexacao = rowNovosAtosPorCriterio["Indexacao"].ToString();
            novosAtosPorCriterios.DtCadNovosAtosPorCriterios = Convert.ToDateTime(rowNovosAtosPorCriterio["DtCadNovosAtosPorCriterios"]);
            novosAtosPorCriterios.AtivoItemNovosAtosPorCriterios =
                Convert.ToBoolean(rowNovosAtosPorCriterio["AtivoItemNovosAtosPorCriterios"]);
            return novosAtosPorCriterios;
        }

        private AtosVerifAtlzcao CarregaAtosVerifAtlzcao(DataRow rowAtosVerifAtlzcao)
        {
            AtosVerifAtlzcao atosVerifAtlzcao = new AtosVerifAtlzcao();
            atosVerifAtlzcao.IdAtosVerifAtlzcao = rowAtosVerifAtlzcao["IdAtosVerifAtlzcao"].ToString();
            atosVerifAtlzcao.IdNorma = rowAtosVerifAtlzcao["IdNorma"].ToString();
            atosVerifAtlzcao.DtCadAtosVerifAtlzcao = Convert.ToDateTime(rowAtosVerifAtlzcao["DtCadAtosVerifAtlzcao"]);
            atosVerifAtlzcao.AtivoItemAtosVerifAtlzcao = Convert.ToBoolean(rowAtosVerifAtlzcao["AtivoItemAtosVerifAtlzcao"]);
            Norma norma = new NormaAD().BuscarNormaPeloIdParaPush(atosVerifAtlzcao.IdNorma);

            atosVerifAtlzcao.TipoDeNorma = norma.Tipo.Nome;
            atosVerifAtlzcao.DataAssinatura = Convert.ToDateTime(norma.DataAssinatura);
            atosVerifAtlzcao.Numero = norma.Numero;

            return atosVerifAtlzcao;
        }
    }
}