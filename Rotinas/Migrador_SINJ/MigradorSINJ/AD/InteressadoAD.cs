using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using MigradorSINJ.OV;
using util.BRLight;

namespace MigradorSINJ.AD
{
    public class InteressadoAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        public InteressadoAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw",true));
            _rest = new REST(Config.ValorChave("NmBaseInteressado", true));
        }

        internal List<InteressadoLBW> BuscarInteressadosLBW()
        {
            List<InteressadoLBW> interessadosLbw = new List<InteressadoLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select * from interessados"))
            {
                while(reader.Read()){
                    interessadosLbw.Add(new InteressadoLBW { Id = Convert.ToInt32(reader["Id"]), Nome = reader["Nomenclatura"].ToString() });
                }
                reader.Close();
            }
            _ad.CloseConection();
            return interessadosLbw;
        }

        internal ulong Incluir(InteressadoOV interessadoOv)
        {
            return _rest.Incluir(interessadoOv);
        }
    }
}
