using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightInfocon.GoldenAccess.General;

namespace MigradorSINJ.OV
{
    public class UsuarioOV
    {
        public UsuarioOV()
        {
            grupos = new List<string>();
            alteracoes = new List<AlteracaoOV>();
        }

        public string nm_login_usuario { get; set; }
        public string nm_usuario { get; set; }
        public string senha_usuario { get; set; }
        public string email_usuario { get; set; }
        public string pagina_inicial { get; set; }
        public string ds_pagina_inicial { get; set; }
        public string ch_tema { get; set; }
        public string dt_ultimo_login { get; set; }
        public string ch_push_usuario { get; set; }

        public bool st_usuario { get; set; }
        public bool in_alterar_senha { get; set; }

        public string ch_perfil { get; set; }
        public string nm_perfil { get; set; }
        public List<string> grupos { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }

        public int nr_tentativa_login { get; set; }

        public OrgaoCadastradorOV orgao_cadastrador { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }

    public class PermissaoMigracao
    {
        public static string[] GetPermissoes(string grupo)
        {
            switch (grupo)
            {
                case "ADM":
                    return new string[] { "NOR.INC", "NOR.EDT", "NOR.PES", "NOR.VIS", "NOR.EXC", "DIO.INC", "DIO.EDT", "DIO.PES", "DIO.VIS", "DIO.EXC", "AUT.INC", "AUT.EDT", "AUT.PES", "AUT.VIS", "AUT.EXC", "ORG.INC", "ORG.EDT", "ORG.PES", "ORG.VIS", "ORG.EXC", "TDF.INC", "TDF.EDT", "TDF.PES", "TDF.VIS", "TDF.EXC", "TDN.INC", "TDN.EDT", "TDN.PES", "TDN.VIS", "TDN.EXC", "TDP.INC", "TDP.EDT", "TDP.PES", "TDP.VIS", "TDP.EXC", "TDD.INC", "TDD.EDT", "TDD.PES", "TDD.VIS", "TDD.EXC", "INT.INC", "INT.EDT", "INT.PES", "INT.VIS", "INT.EXC", "SIT.INC", "SIT.EDT", "SIT.PES", "SIT.VIS", "SIT.EXC", "TDR.INC", "TDR.EDT", "TDR.PES", "TDR.VIS", "TDR.EXC", "RQE.INC", "RQE.EDT", "RQE.PES", "RQE.VIS", "RQE.EXC", "RQI.INC", "RQI.EDT", "RQI.PES", "RQI.VIS", "RQI.EXC", "REL.INC", "REL.EDT", "REL.PES", "REL.VIS", "REL.EXC", "RIO.PES", "PRO.INC", "PRO.EDT", "PRO.PES", "PRO.VIS", "PRO.EXC", "VOC.INC", "VOC.EDT", "VOC.PES", "VOC.VIS", "VOC.EXC", "VOC.GER", "USR.INC", "USR.EDT", "USR.PES", "USR.VIS", "USR.EXC", "CFG.EDT", "CFG.VIS" };
                case "SINJ_USUARIOS_MASTER":
                    return new string[] { "NOR.INC", "NOR.EDT", "NOR.PES", "NOR.VIS", "NOR.EXC", "DIO.INC", "DIO.EDT", "DIO.PES", "DIO.VIS", "DIO.EXC", "AUT.INC", "AUT.EDT", "AUT.PES", "AUT.VIS", "AUT.EXC", "ORG.INC", "ORG.EDT", "ORG.PES", "ORG.VIS", "ORG.EXC", "TDF.INC", "TDF.EDT", "TDF.PES", "TDF.VIS", "TDF.EXC", "TDN.INC", "TDN.EDT", "TDN.PES", "TDN.VIS", "TDN.EXC", "TDP.INC", "TDP.EDT", "TDP.PES", "TDP.VIS", "TDP.EXC", "TDD.INC", "TDD.EDT", "TDD.PES", "TDD.VIS", "TDD.EXC", "INT.INC", "INT.EDT", "INT.PES", "INT.VIS", "INT.EXC", "SIT.INC", "SIT.EDT", "SIT.PES", "SIT.VIS", "SIT.EXC", "TDR.INC", "TDR.EDT", "TDR.PES", "TDR.VIS", "TDR.EXC", "RQE.INC", "RQE.EDT", "RQE.PES", "RQE.VIS", "RQE.EXC", "RQI.INC", "RQI.EDT", "RQI.PES", "RQI.VIS", "RQI.EXC", "REL.INC", "REL.EDT", "REL.PES", "REL.VIS", "REL.EXC", "RIO.PES", "PRO.INC", "PRO.EDT", "PRO.PES", "PRO.VIS", "PRO.EXC", "VOC.INC", "VOC.EDT", "VOC.PES", "VOC.VIS", "VOC.EXC", "VOC.GER", "USR.INC", "USR.EDT", "USR.PES", "USR.VIS", "USR.EXC", "CFG.EDT", "CFG.VIS" };
                case "SINJ_GESTORES":
                    return new string[] { "NOR.INC", "NOR.EDT", "NOR.PES", "NOR.VIS", "NOR.EXC", "DIO.INC", "DIO.EDT", "DIO.PES", "DIO.VIS", "DIO.EXC", "AUT.INC", "AUT.EDT", "AUT.PES", "AUT.VIS", "AUT.EXC", "ORG.INC", "ORG.EDT", "ORG.PES", "ORG.VIS", "ORG.EXC", "TDF.INC", "TDF.EDT", "TDF.PES", "TDF.VIS", "TDF.EXC", "TDN.INC", "TDN.EDT", "TDN.PES", "TDN.VIS", "TDN.EXC", "TDP.INC", "TDP.EDT", "TDP.PES", "TDP.VIS", "TDP.EXC", "TDD.INC", "TDD.EDT", "TDD.PES", "TDD.VIS", "TDD.EXC", "INT.INC", "INT.EDT", "INT.PES", "INT.VIS", "INT.EXC", "SIT.INC", "SIT.EDT", "SIT.PES", "SIT.VIS", "SIT.EXC", "TDR.INC", "TDR.EDT", "TDR.PES", "TDR.VIS", "TDR.EXC", "RQE.INC", "RQE.EDT", "RQE.PES", "RQE.VIS", "RQE.EXC", "RQI.INC", "RQI.EDT", "RQI.PES", "RQI.VIS", "RQI.EXC", "REL.INC", "REL.EDT", "REL.PES", "REL.VIS", "REL.EXC", "RIO.PES", "PRO.INC", "PRO.EDT", "PRO.PES", "PRO.VIS", "PRO.EXC", "VOC.INC", "VOC.EDT", "VOC.PES", "VOC.VIS", "VOC.EXC", "CFG.EDT", "CFG.VIS" };
                case "SINJ_ADMINISTRADORES_VOCABULARIO":
                    return new string[] { "NOR.PES", "NOR.VIS", "DIO.PES", "DIO.VIS", "AUT.PES", "AUT.VIS", "ORG.PES", "ORG.VIS", "TDF.PES", "TDF.VIS", "TDN.PES", "TDN.VIS", "TDP.PES", "TDP.VIS", "TDD.PES", "TDD.VIS", "INT.PES", "INT.VIS", "SIT.PES", "SIT.VIS", "TDR.PES", "TDR.VIS", "RQE.PES", "RQE.VIS", "RQI.PES", "RQI.VIS", "REL.PES", "REL.VIS", "PRO.PES", "PRO.VIS", "VOC.INC", "VOC.EDT", "VOC.PES", "VOC.VIS", "VOC.EXC", "VOC.GER", "CFG.EDT", "CFG.VIS" };
                case "SINJ_USUARIOS_COMUNS":
                    return new string[] { "NOR.INC", "NOR.EDT", "NOR.PES", "NOR.VIS", "NOR.EXC", "DIO.INC", "DIO.EDT", "DIO.PES", "DIO.VIS", "DIO.EXC", "AUT.PES", "AUT.VIS", "ORG.PES", "ORG.VIS", "TDF.PES", "TDF.VIS", "TDN.PES", "TDN.VIS", "TDP.PES", "TDP.VIS", "INT.PES", "INT.VIS", "SIT.PES", "SIT.VIS", "TDR.PES", "TDR.VIS", "RQE.PES", "RQE.VIS", "RQI.PES", "RQI.VIS", "REL.PES", "REL.VIS", "PRO.PES", "PRO.VIS", "VOC.PES", "VOC.VIS", "CFG.EDT", "CFG.VIS" };
                default:
                    return new string[] { "NOR.INC", "NOR.EDT", "NOR.PES", "NOR.VIS", "NOR.EXC", "DIO.INC", "DIO.EDT", "DIO.PES", "DIO.VIS", "DIO.EXC", "AUT.PES", "AUT.VIS", "ORG.PES", "ORG.VIS", "TDF.PES", "TDF.VIS", "TDN.PES", "TDN.VIS", "TDP.PES", "TDP.VIS", "INT.PES", "INT.VIS", "SIT.PES", "SIT.VIS", "TDR.PES", "TDR.VIS", "RQE.PES", "RQE.VIS", "RQI.PES", "RQI.VIS", "REL.PES", "REL.VIS", "PRO.PES", "PRO.VIS", "VOC.PES", "VOC.VIS", "CFG.EDT", "CFG.VIS" };
            }
        }
    }
}
