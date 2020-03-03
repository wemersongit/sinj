using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using MigradorSINJ.OV;
using util.BRLight;

namespace MigradorSINJ.AD
{
    public class SituacaoAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        public SituacaoAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw",true));
            _rest = new REST(Config.ValorChave("NmBaseSituacao", true));
        }

        internal List<SituacaoLBW> BuscarSituacoesLBW()
        {
            List<SituacaoLBW> situacoesLbw = new List<SituacaoLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select * from SituacoesDasNormas"))
            {
                while(reader.Read()){
                    situacoesLbw.Add(new SituacaoLBW { Id = Convert.ToInt32(reader["Id"]), Descricao = reader["Descricao"].ToString(), Peso = Convert.ToInt32(reader["Peso"]) });
                }
                reader.Close();
            }
            _ad.CloseConection();
            return situacoesLbw;
        }

        internal ulong Incluir(SituacaoOV situacaoOv)
        {
            return _rest.Incluir(situacaoOv);
        }
    }
}
