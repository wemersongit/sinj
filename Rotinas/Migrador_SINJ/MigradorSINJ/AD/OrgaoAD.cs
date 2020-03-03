using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using MigradorSINJ.OV;
using util.BRLight;

namespace MigradorSINJ.AD
{
    public class OrgaoAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        public OrgaoAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw",true));
            _rest = new REST(Config.ValorChave("NmBaseOrgao", true));
        }

        internal List<OrgaoLBW> BuscarOrgaosLBW()
        {
            List<OrgaoLBW> orgaosLbw = new List<OrgaoLBW>();
            _ad.OpenConnection();
            using (var reader = _ad.ExecuteDataReader("select * from Orgaos"))
            {
                while(reader.Read()){
                    orgaosLbw.Add(new OrgaoLBW {
                        Id_Orgao = Convert.ToInt32(reader["Id_orgao"]),
                        Nm_Orgao = reader["Nm_Orgao"].ToString(),
                        Sg_Orgao = reader["Sg_Orgao"].ToString(),
                        Sg_OrgaoHierarquiaSuperior = reader["Sg_OrgaoHierarquiaSuperior"].ToString(),
                        Ch_Cronologica = reader["Ch_Cronologica"].ToString(),
                        Ch_Hierarquica = reader["Ch_Hierarquica"].ToString(),
                        Id_OrgaoPai = reader["Id_OrgaoPai"].ToString(),
                        Id_OrgaoAnterior = reader["Id_OrgaoAnterior"].ToString(),
                        Ds_NotaEscopoOrgao = reader["Ds_NotaEscopoOrgao"].ToString(),
                        Dt_Cadastro = reader["Dt_Cadastro"].ToString(),
                        Dt_InicioVigencia = reader["Dt_InicioVigencia"].ToString(),
                        Dt_FimVigencia = reader["Dt_FimVigencia"].ToString(),
                        Dt_UltimaAlteracao = reader["Dt_UltimaAlteracao"].ToString(),
                        In_Autoridade = Convert.ToBoolean(reader["In_Autoridade"]),
                        In_CLDF = Convert.ToBoolean(reader["In_CLDF"]),
                        In_SEPLAG = Convert.ToBoolean(reader["In_SEPLAG"]),
                        In_PGDF = Convert.ToBoolean(reader["In_PGDF"]),
                        In_TCDF = Convert.ToBoolean(reader["In_TCDF"]),
                        Nm_Ambito = reader["Nm_Ambito"].ToString(),
                        Nm_UsuarioCadastro = reader["Nm_UsuarioCadastro"].ToString(),
                        Nm_UsuarioUltimaAlteracao = reader["Nm_UsuarioUltimaAlteracao"].ToString()
                    });
                }
                reader.Close();
            }
            _ad.CloseConection();
            return orgaosLbw;
        }

        internal ulong Incluir(OrgaoOV orgaoOv)
        {
            return _rest.Incluir(orgaoOv);
        }
    }
}
