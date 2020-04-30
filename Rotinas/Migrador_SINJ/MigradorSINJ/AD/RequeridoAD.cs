using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using MigradorSINJ.OV;
using util.BRLight;

namespace MigradorSINJ.AD
{
    public class RequeridoAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        public RequeridoAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw",true));
            _rest = new REST(Config.ValorChave("NmBaseRequerido", true));
        }

        internal List<RequeridoLBW> BuscarRequeridosLBW()
        {
            List<RequeridoLBW> requeridosLbw = new List<RequeridoLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select * from requeridos"))
            {
                while(reader.Read()){
                    requeridosLbw.Add(new RequeridoLBW { Id = Convert.ToInt32(reader["Id"]), Nome = reader["Nomenclatura"].ToString() });
                }
                reader.Close();
            }
            _ad.CloseConection();
            return requeridosLbw;
        }

        internal ulong Incluir(RequeridoOV requeridoOv)
        {
            return _rest.Incluir(requeridoOv);
        }
    }
}
