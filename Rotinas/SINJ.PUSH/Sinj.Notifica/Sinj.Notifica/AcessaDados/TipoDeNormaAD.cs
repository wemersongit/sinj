using System;
using System.Collections.Generic;
using LightInfocon.Data.LightBaseProvider;
using Sinj.Notifica.Objetos;

namespace Sinj.Notifica.AcessaDados
{
    public class TipoDeNormaAD
    {
        private BRLight.DataAccess.LBW.Provider.AcessaDados _conn;

        public TipoDeNormaAD(string stringConnection)
        {
            _conn = new BRLight.DataAccess.LBW.Provider.AcessaDados(stringConnection);
        }

        public List<TipoDeNorma> BuscaTiposDeNorma()
        {
            string sql = string.Format("select * from TiposDeNorma");
            List<TipoDeNorma> lista = new List<TipoDeNorma>();
            _conn.OpenConnection();
            using (LightBaseDataReader rdr = _conn.ExecuteDataReader(sql))
            {
                while (rdr.Read())
                {
                    TipoDeNorma tipoDeNorma = new TipoDeNorma();
                    tipoDeNorma.Id = Convert.ToInt32(rdr["Id"]);
                    tipoDeNorma.Nome = rdr["Nome"].ToString();
                    lista.Add(tipoDeNorma);
                }
                rdr.Close();
            }
            _conn.CloseConection();
            return lista;
        }

        public TipoDeNorma BuscaTipoDeNorma(string sql)
        {
            TipoDeNorma tipoDeNorma = new TipoDeNorma();
            _conn.OpenConnection();
            using (var rdr = _conn.ExecuteDataReader(sql))
            {
                while (rdr.Read())
                {
                    tipoDeNorma.Id = Convert.ToInt32(rdr["Id"]);
                    tipoDeNorma.Nome = rdr["Nome"].ToString();
                }
            }
            _conn.CloseConection();
            return tipoDeNorma;
        }
    }
}