using System;
using System.Collections.Generic;
using LightInfocon.Data.LightBaseProvider;
using Sinj.Notifica.Objetos;

namespace Sinj.Notifica.AcessaDados
{
    public class OrgaoAD
    {
        private BRLight.DataAccess.LBW.Provider.AcessaDados _conn;

        public OrgaoAD(string stringConnection)
        {
            _conn = new BRLight.DataAccess.LBW.Provider.AcessaDados(stringConnection);
        }

        public List<OrgaoSinj> BuscaOrgaos()
        {
            string sql = "select * from SinjOrgaos";
            List<OrgaoSinj> lista = new List<OrgaoSinj>();
            _conn.OpenConnection();
            using (LightBaseDataReader rdr = _conn.ExecuteDataReader(sql))
            {
                while (rdr.Read())
                {
                    OrgaoSinj orgao = new OrgaoSinj();
                    orgao.Id = Convert.ToInt32(rdr["Id"]);
                    orgao.Descricao = rdr["Descricao"].ToString();
                    orgao.Sigla = rdr["Sigla"].ToString().Replace("/", ",");
                    orgao.CodigoAssociacao = rdr["CodigoAssociacao"].ToString();
                    orgao.DtVigencia = rdr["DtVigencia"].ToString().Replace("00:00:00", "");
                    orgao.DtFimVigencia = rdr["DtFimDeVigencia"].ToString().Replace("00:00:00", "");
                    string valorCodigo = rdr["Codigo"].ToString();
                    if (!string.IsNullOrEmpty(valorCodigo))
                    {
                        orgao.Codigo = valorCodigo;
                    }
                    orgao.Ambito = rdr["Ambito"].ToString();
                    orgao.Status = rdr["Status"].ToString();
                    lista.Add(orgao);
                }
                rdr.Close();
            }
            _conn.CloseConection();
            return lista;
        }
    }
}