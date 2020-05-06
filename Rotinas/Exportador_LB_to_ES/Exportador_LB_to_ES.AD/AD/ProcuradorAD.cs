using System;
using System.Collections.Generic;
using System.Data;
using AcessaDadosLightBase;
using Exportador_LB_to_ES.AD.Models;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class ProcuradorAD : AD
    {
        public List<ProcuradorResponsavel> ObtemProcuradoresResponsaveis(object[] procuradores)
        {
            List<ProcuradorResponsavel> lista = new List<ProcuradorResponsavel>();
            string sql = string.Format("select * from {0}", Configuracao.LerValorChave(chaveBaseProcuradoresResponsaveis));
            var conn = new AcessaDados(Configuracao.LerValorChave(chaveLightBaseConnectionString));
            conn.OpenConnection();
            using (var reader = conn.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    ProcuradorResponsavel procurador = CarregaProcuradorResponsavel(reader);
                    foreach (int idProcurador in procuradores)
                    {
                        if (idProcurador == procurador.Id)
                        {
                            lista.Add(procurador);
                            break;
                        }
                    }
                }
            }
            conn.CloseConection();
            return lista;
        }

        private static ProcuradorResponsavel CarregaProcuradorResponsavel(IDataRecord reader)
        {
            ProcuradorResponsavel procuradorResponsavel = new ProcuradorResponsavel()
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = Convert.ToString(reader["Nomenclatura"])
            };

            return procuradorResponsavel;
        }
    }
}