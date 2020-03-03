using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.OV;
using BRLight.DataAccess.LBW.Provider;
using util.BRLight;

namespace MigradorSINJ.AD
{
    public class AutoriaAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;


        private REST _rest;

        public AutoriaAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw",true));
            _rest = new REST(Config.ValorChave("NmBaseAutoria", true));
        }

        internal List<AutoriaLBW> BuscarAutoriasLBW()
        {
            List<AutoriaLBW> autoriasLbw = new List<AutoriaLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select * from autorias"))
            {
                while(reader.Read()){
                    autoriasLbw.Add(new AutoriaLBW { Id = Convert.ToInt32(reader["Id"]), Nome = reader["Nome"].ToString() });
                }
                reader.Close();
            }
            _ad.CloseConection();
            return autoriasLbw;
        }

        internal ulong Incluir(AutoriaOV autoria)
        {
            return _rest.Incluir(autoria);
        }
    }
}
