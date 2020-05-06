using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using MigradorSINJ.OV;
using util.BRLight;

namespace MigradorSINJ.AD
{
    public class RelatorAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        public RelatorAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw",true));
            _rest = new REST(Config.ValorChave("NmBaseRelator", true));
        }

        internal List<RelatorLBW> BuscarRelatorsLBW()
        {
            List<RelatorLBW> relatorsLbw = new List<RelatorLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select * from relatores"))
            {
                while(reader.Read()){
                    relatorsLbw.Add(new RelatorLBW { Id = Convert.ToInt32(reader["Id"]), Nome = reader["Nomenclatura"].ToString() });
                }
                reader.Close();
            }
            _ad.CloseConection();
            return relatorsLbw;
        }

        internal ulong Incluir(RelatorOV relatorOv)
        {
            return _rest.Incluir(relatorOv);
        }
    }
}
