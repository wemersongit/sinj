using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using util.BRLight;
using MigradorSINJ.OV;

namespace MigradorSINJ.AD
{
    public class TipoDePublicacaoAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        public TipoDePublicacaoAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw", true));
            _rest = new REST(Config.ValorChave("NmBaseTipoDePublicacao", true));
        }

        internal List<TipoDePublicacaoLBW> BuscarTiposDePublicacaoLBW()
        {
            List<TipoDePublicacaoLBW> tiposDePublicacaoLbw = new List<TipoDePublicacaoLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select * from TiposDePublicacao"))
            {
                while (reader.Read())
                {
                    tiposDePublicacaoLbw.Add(new TipoDePublicacaoLBW
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nome = reader["Nome"].ToString()
                    });
                }
                reader.Close();
            }
            _ad.CloseConection();
            return tiposDePublicacaoLbw;
        }

        internal ulong Incluir(TipoDePublicacaoOV tipoDePublicacaoOv)
        {
            return _rest.Incluir(tipoDePublicacaoOv);
        }
    }
}
