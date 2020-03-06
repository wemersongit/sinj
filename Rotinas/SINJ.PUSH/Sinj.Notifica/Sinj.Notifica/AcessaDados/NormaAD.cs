using System;
using System.Collections.Generic;
using System.Data;
using LightInfocon.Data.LightBaseProvider;
using Sinj.Notifica.Objetos;
using Sinj.Notifica.Regras;

namespace Sinj.Notifica.AcessaDados
{
    public class NormaAD
    {
        private BRLight.DataAccess.LBW.Provider.AcessaDados _conn;
        private TipoDeNormaRN _tipoDeNormaRn;
        private OrgaoRN _orgaoRn;

        public NormaAD(string stringConnection)
        {
            _conn = new BRLight.DataAccess.LBW.Provider.AcessaDados(stringConnection);
            _tipoDeNormaRn = new TipoDeNormaRN(stringConnection);
            _orgaoRn = new OrgaoRN(stringConnection);
        }

        public List<Norma> BuscaNormasNovas()
        {
            string sql = "select * from VersoesDasNormas where NovaNorma = 1";
            List<Norma> lista = new List<Norma>();
            _conn.OpenConnection();
            using (LightBaseDataReader rdr = _conn.ExecuteDataReader(sql))
            {
                while (rdr.Read())
                {
                    lista.Add(MontaNormaSimples(rdr));
                }
                rdr.Close();
            }
            _conn.CloseConection();
            return lista;
        }

        public List<Norma> BuscaNormasAlteradas()
        {
            string sql = "select * from VersoesDasNormas where AtlzNorma = 1";
            List<Norma> lista = new List<Norma>();
            _conn.OpenConnection();
            using (LightBaseDataReader rdr = _conn.ExecuteDataReader(sql))
            {
                while (rdr.Read())
                {
                    lista.Add(MontaNormaSimples(rdr));
                }
                rdr.Close();
            }
            _conn.CloseConection();
            return lista;
        }


        private Norma MontaNormaSimples(IDataReader reader)
        {
            Norma norma = new Norma();
            if (reader["Id"] is DBNull)
            {
                throw new Exception("Norma com campo Id nulo não é permitido.");
            }
            norma.Id = Convert.ToInt32(reader["Id"]);
            norma.Tipo = _tipoDeNormaRn.BuscaTipoDeNorma(reader["Id_Tipo"].ToString());
            norma.Numero = reader["Numero"].ToString();
            if (!string.IsNullOrEmpty(reader["DataAssinatura"].ToString()))
            {
                norma.DataAssinatura = Convert.ToDateTime(reader["DataAssinatura"]);
            }
            else
            {
                throw new Exception("Norma id " + norma.Id + " sem data de assinatura.");
            }
            norma.Ementa = reader["Ementa"].ToString();
            if (reader["Origens"] != DBNull.Value)
            {
                object[] origens = (object[])reader["Origens"];
                OrgaoSinj orgaoSinj;
                foreach (object idOrigem in origens)
                {
                    orgaoSinj = _orgaoRn.BuscaOrgao(idOrigem.ToString());
                    norma.Origens.Add(orgaoSinj);
                }
            }
            norma.NeoIndexacao = MontarNeoIndexacao(reader);

            return norma;
        }

        private List<NeoIndexacao> MontarNeoIndexacao(IDataRecord reader)
        {
            List<NeoIndexacao> listaNeoIndexacao = new List<NeoIndexacao>();
            if (reader["NeoIndexacao"] is DataTable)
            {
                DataTable datatable = (DataTable)reader["NeoIndexacao"];
                foreach (DataRow dataRow in datatable.Rows)
                {
                    NeoIndexacao neoIndexacao = new NeoIndexacao();
                    neoIndexacao.InTipoTermo = Convert.ToInt32(dataRow["InTipoTermo"]);
                    neoIndexacao.NmTermo = dataRow["NmTermo"].ToString();
                    neoIndexacao.NmEspecificador = dataRow["NmEspecificador"].ToString();
                    neoIndexacao.NmTermoAuxiliar = dataRow["NmTermoAuxiliar"].ToString();
                    neoIndexacao.NmEspecificadorAuxiliar = dataRow["NmEspecificadorAuxiliar"].ToString();
                    listaNeoIndexacao.Add(neoIndexacao);
                }
            }
            return listaNeoIndexacao;
        }

        public int UpdateNormas()
        {
            string sql = "update VersoesDasNormas set NovaNorma = false, AtlzNorma = false where NovaNorma = true or AtlzNorma = true";
            _conn.OpenConnection();
            int i = _conn.ExecuteNonQuery(sql);
            _conn.CloseConection();
            return i;
        }
    }
}