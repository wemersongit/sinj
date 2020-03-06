using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using util.BRLight;
using MigradorSINJ.OV;

namespace MigradorSINJ.AD
{
    public class TipoDeRelacaoAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        public TipoDeRelacaoAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw", true));
            _rest = new REST(Config.ValorChave("NmBaseTipoDeRelacao", true));
        }

        internal List<TipoDeRelacaoLBW> BuscarTiposDeRelacaoLBW()
        {
            List<TipoDeRelacaoLBW> tiposDeRelacaoLbw = new List<TipoDeRelacaoLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select * from TiposDeRelacao"))
            {
                while (reader.Read())
                {
                    tiposDeRelacaoLbw.Add(new TipoDeRelacaoLBW
                    {
                        Oid = Convert.ToInt32(reader["Oid"]),
                        Conteudo = reader["Conteudo"].ToString(),
                        Descricao = reader["Descricao"].ToString(),
                        TextoParaAlterador = reader["TextoParaAlterador"].ToString(),
                        TextoParaAlterado = reader["TextoParaAlterado"].ToString(),
                        Importancia = Convert.ToInt32(reader["Importancia"]),
                        RelacaoDeAcao = Convert.ToBoolean(reader["RelacaoDeAcao"])
                    });
                }
                reader.Close();
            }
            _ad.CloseConection();
            return tiposDeRelacaoLbw;
        }

        internal ulong Incluir(TipoDeRelacaoOV tipoDeRelacaoOv)
        {
            return _rest.Incluir(tipoDeRelacaoOv);
        }
    }
}
