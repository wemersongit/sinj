using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using util.BRLight;
using MigradorSINJ.OV;

namespace MigradorSINJ.AD
{
    public class TipoDeFonteAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        public TipoDeFonteAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw", true));
            _rest = new REST(Config.ValorChave("NmBaseTipoDeFonte", true));
        }

        internal List<TipoDeFonteLBW> BuscarTiposDeFonteLBW()
        {
            List<TipoDeFonteLBW> tiposDeFonteLbw = new List<TipoDeFonteLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select * from TiposDeFonte"))
            {
                while (reader.Read())
                {
                    tiposDeFonteLbw.Add(new TipoDeFonteLBW {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nome = reader["Nome"].ToString()
                    });
                }
                reader.Close();
            }
            _ad.CloseConection();
            return tiposDeFonteLbw;
        }

        internal ulong Incluir(TipoDeFonteOV tipoDeFonteOv)
        {
            return _rest.Incluir(tipoDeFonteOv);
        }
    }
}
