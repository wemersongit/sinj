using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using util.BRLight;
using MigradorSINJ.OV;

namespace MigradorSINJ.AD
{
    public class TipoDeNormaAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        public TipoDeNormaAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw", true));
            _rest = new REST(Config.ValorChave("NmBaseTipoDeNorma", true));
        }

        internal List<TipoDeNormaLBW> BuscarTiposDeNormaLBW()
        {
            List<TipoDeNormaLBW> tiposDeNormaLbw = new List<TipoDeNormaLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select * from TiposDeNorma"))
            {
                while (reader.Read())
                {
                    tiposDeNormaLbw.Add(new TipoDeNormaLBW {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nome = reader["Nome"].ToString(),
                        Descricao = reader["Nome"].ToString(),
                        TCDF = Convert.ToBoolean(reader["TCDF"]),
                        SEPLAG = Convert.ToBoolean(reader["SEPLAG"]),
                        CLDF = Convert.ToBoolean(reader["CLDF"]),
                        PGDF = Convert.ToBoolean(reader["PGDF"]),
                        Conjunta = Convert.ToBoolean(reader["Conjunta"]),
                        ControleDeNumeracaoPorOrgao = Convert.ToBoolean(reader["ControleDeNumeracaoPorOrgao"]),
                        Grupo1 = Convert.ToBoolean(reader["Grupo1"]),
                        Grupo2 = Convert.ToBoolean(reader["Grupo2"]),
                        Grupo3 = Convert.ToBoolean(reader["Grupo3"]),
                        Grupo4 = Convert.ToBoolean(reader["Grupo4"]),
                        Grupo5 = Convert.ToBoolean(reader["Grupo5"]),
                        Questionaveis = Convert.ToBoolean(reader["Questionaveis"])
                    });
                }
                reader.Close();
            }
            _ad.CloseConection();
            return tiposDeNormaLbw;
        }

        internal ulong Incluir(TipoDeNormaOV tipoDeNormaOv)
        {
            return _rest.Incluir(tipoDeNormaOv);
        }
    }
}
