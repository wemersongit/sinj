using System;
using System.Collections.Generic;
using System.Data;
using BRLight.Util;
using LightInfocon.Data.LightBaseProvider;
using Sinj.Notifica.Objetos;

namespace Sinj.Notifica.AcessaDados
{
    public class PushAD
    {
        private BRLight.DataAccess.LBW.Provider.AcessaDados _conn;

        public PushAD(string stringConnection)
        {
            Console.WriteLine(_conn = new BRLight.DataAccess.LBW.Provider.AcessaDados(stringConnection));

            Console.WriteLine(_conn);

        }
        public List<Push> BuscaAtivosPush()
        {
            List<Push> lista = new List<Push>();
            
            _conn.OpenConnection();

            string sql = "select * from push where AtivoUsuario=1";
            using (LightBaseDataReader rdr = _conn.ExecuteDataReader(sql))
            {
                while (rdr.Read())
                {
                    Push push = new Push();
                    push.Id = Convert.ToInt32(rdr["Id"]);
                    push.Nome = rdr["nome"].ToString();
                    push.Email = rdr["email"].ToString();
                    push.Senha = rdr["senha"].ToString();
                    push.DtCadUsuario = Convert.ToDateTime(rdr["DtCadUsuario"]);
                    push.AtivoUsuario = Convert.ToBoolean(rdr["AtivoUsuario"]);
                    push.NovosAtosPorCriteriosValue = new List<NovosAtosPorCriterios>();
                    if (rdr["NovosAtosPorCriterios"] is DataTable)
                    {
                        DataTable tableNovosAtosPorCriterios = (DataTable) rdr["NovosAtosPorCriterios"];
                        foreach (DataRow rowNovosAtosPorCriterios in tableNovosAtosPorCriterios.Rows)
                        {
                            try
                            {
                                push.NovosAtosPorCriteriosValue.Add(CarregaNovosAtosPorCriterio(rowNovosAtosPorCriterios));
                            }
                            catch (Exception ex)
                            {
                                Log.Writer(ex, "Carregando NovosAtosPorCriterios. Push Id: " + rdr["Id"]);
                            }
                        }
                    }
                    push.AtosVerifAtlzcaoValue = new List<AtosVerifAtlzcao>();
                    if (rdr["AtosVerifAtlzcao"] is DataTable)
                    {
                        DataTable tableAtosVerifAtlzcao = (DataTable) rdr["AtosVerifAtlzcao"];
                        foreach (DataRow rowAtosVerifAtlzcao in tableAtosVerifAtlzcao.Rows)
                        {
                            try
                            {
                                push.AtosVerifAtlzcaoValue.Add(CarregaAtosVerifAtlzcao(rowAtosVerifAtlzcao));
                            }
                            catch (Exception ex)
                            {
                                Log.Writer(ex, "Carregando AtosVerifAtlzcao. Push Id: " + rdr["Id"]);
                            }
                        }
                    }
                    lista.Add(push);
                }
                rdr.Close();
            }
            _conn.CloseConection();
            return lista;
        }

        private NovosAtosPorCriterios CarregaNovosAtosPorCriterio(DataRow rowNovosAtosPorCriterio)
        {
            NovosAtosPorCriterios novosAtosPorCriterios = new NovosAtosPorCriterios();
            novosAtosPorCriterios.IdNovosAtosPorCriterios = rowNovosAtosPorCriterio["IdNovosAtosPorCriterios"].ToString();
            novosAtosPorCriterios.TipoAto = Convert.ToInt32(rowNovosAtosPorCriterio["TipoAto"]);
            novosAtosPorCriterios.PrimeiroConec = rowNovosAtosPorCriterio["PrimeiroConec"].ToString();
            novosAtosPorCriterios.Origem = Convert.ToInt32(rowNovosAtosPorCriterio["Origem"]);
            novosAtosPorCriterios.SegundoConec = rowNovosAtosPorCriterio["SegundoConec"].ToString();
            novosAtosPorCriterios.Indexacao = rowNovosAtosPorCriterio["Indexacao"].ToString();
            novosAtosPorCriterios.DtCadNovosAtosPorCriterios = Convert.ToDateTime(rowNovosAtosPorCriterio["DtCadNovosAtosPorCriterios"]);
            novosAtosPorCriterios.AtivoItemNovosAtosPorCriterios = Convert.ToBoolean(rowNovosAtosPorCriterio["AtivoItemNovosAtosPorCriterios"]);
            return novosAtosPorCriterios;
        }

        private AtosVerifAtlzcao CarregaAtosVerifAtlzcao(DataRow rowAtosVerifAtlzcao)
        {
            AtosVerifAtlzcao atosVerifAtlzcao = new AtosVerifAtlzcao();
            atosVerifAtlzcao.IdAtosVerifAtlzcao = rowAtosVerifAtlzcao["IdAtosVerifAtlzcao"].ToString();
            atosVerifAtlzcao.IdNorma = rowAtosVerifAtlzcao["IdNorma"].ToString();
            atosVerifAtlzcao.DtCadAtosVerifAtlzcao = Convert.ToDateTime(rowAtosVerifAtlzcao["DtCadAtosVerifAtlzcao"]);
            atosVerifAtlzcao.AtivoItemAtosVerifAtlzcao = Convert.ToBoolean(rowAtosVerifAtlzcao["AtivoItemAtosVerifAtlzcao"]);
            return atosVerifAtlzcao;
        }
    }
}