using System;
using System.Collections.Generic;
using System.Data;
using TCDF_REPORT.OV;

namespace TCDF_REPORT.AD
{
    public class TermoAD
    {
        private AcessaDados _ad;
        
        public TermoAD(string stringConnection)
        {
            _ad = new AcessaDados(stringConnection);
        }
        internal List<TermoOV> BuscarTodosOsTermos()
        {
            List<TermoOV> termos = new List<TermoOV>();
            string sql = "VocabularioControlado";
            using(var dr = _ad.ExecuteDataReader(sql))
            {
                TermoOV termo;
                while(dr.Read())
                {
                    termo = new TermoOV();
                    termo.Id_Termo = dr["Id_Termo"].ToString();
                    termo.Nm_Termo = dr["Nm_Termo"].ToString();
                    termo.Nm_Auxiliar = dr["Nm_Auxiliar"].ToString();
                    termo.In_TipoTermo = Convert.ToInt32(dr["In_TipoTermo"]);
                    termos.Add(termo);
                }
                dr.Close();
            }
            return termos;
        }

        internal int DeletarTermo(string id_termo)
        {
            try
            {
                ExcluirTermosGeraisEEspecificosDoTermo(id_termo);
                ExcluirTermosRelacionadosDoTermo(id_termo);
                ExcluirTermosUseENaoAutorizadosDoTermo(id_termo);
                return ExcluirTermo(id_termo);
            }
            catch
            {
                return -1;
            }
        }

        private void ExcluirTermosGeraisEEspecificosDoTermo(string id_termo)
        {
            string delete = string.Format("delete from {0} where Id_TermoGeral=\"{1}\" or Id_TermoEspecifico=\"{1}\"", "TERMOS_RELACAO_TERMO_GERAL_E_ESPECIFICO", id_termo);
            _ad.ExecuteNonQuery(delete);
        }

        private void ExcluirTermosRelacionadosDoTermo(string id_termo)
        {
            string delete = string.Format("delete from {0} where Id_Termo=\"{1}\" or Id_TermoRelacionado=\"{1}\"", "TERMOS_RELACAO_TERMO_RELACIONADO", id_termo);
            _ad.ExecuteNonQuery(delete);
        }

        private void ExcluirTermosUseENaoAutorizadosDoTermo(string id_termo)
        {
            string delete = string.Format("delete from {0} where Id_TermoUse=\"{1}\" or Id_TermoNaoAutorizado=\"{1}\"", "TERMOS_RELACAO_TERMO_NAO_AUTORIZADO", id_termo);
            _ad.ExecuteNonQuery(delete);
        }

        private int ExcluirTermo(string id_termo)
        {
            string delete = string.Format("delete from {0} where Id_Termo=\"{1}\"", "VOCABULARIOCONTROLADO", id_termo);
            return _ad.ExecuteNonQuery(delete);
        }

        internal TermoOV BuscarTermoPorId(string id_termo)
        {
            TermoOV termo = new TermoOV();
            string sql = string.Format("select * from VocabularioControlado where Id_Termo=\"{0}\"",id_termo);
            using (var dr = _ad.ExecuteDataReader(sql))
            {
                while (dr.Read())
                {
                    termo = new TermoOV();
                    termo.Id_Termo = dr["Id_Termo"].ToString();
                    termo.Nm_Termo = dr["Nm_Termo"].ToString();
                    termo.Nm_Auxiliar = dr["Nm_Auxiliar"].ToString();
                    termo.In_TipoTermo = Convert.ToInt32(dr["In_TipoTermo"]);
                }
                dr.Close();
            }
            return termo;
        }
    }
}