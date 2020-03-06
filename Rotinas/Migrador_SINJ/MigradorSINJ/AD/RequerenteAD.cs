using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using MigradorSINJ.OV;
using util.BRLight;

namespace MigradorSINJ.AD
{
    public class RequerenteAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        public RequerenteAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw",true));
            _rest = new REST(Config.ValorChave("NmBaseRequerente", true));
        }

        internal List<RequerenteLBW> BuscarRequerentesLBW()
        {
            List<RequerenteLBW> requerentesLbw = new List<RequerenteLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select * from requerentes"))
            {
                while(reader.Read()){
                    requerentesLbw.Add(new RequerenteLBW { Id = Convert.ToInt32(reader["Id"]), Nome = reader["Nomenclatura"].ToString() });
                }
                reader.Close();
            }
            _ad.CloseConection();
            return requerentesLbw;
        }

        internal ulong Incluir(RequerenteOV requerenteOv)
        {
            return _rest.Incluir(requerenteOv);
        }
    }
}
