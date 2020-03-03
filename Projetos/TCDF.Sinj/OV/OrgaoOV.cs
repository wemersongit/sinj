using System;
using System.Collections.Generic;
using System.Text;
using neo.BRLightREST;
using util.BRLight;

namespace TCDF.Sinj.OV
{
    public class OrgaoOV : metadata
    {
        public OrgaoOV()
        {
            ch_cronologia = new List<string>();
            ch_orgao_anterior = new string[0];
            orgaos_cadastradores = new List<OrgaoCadastrador>();
            ambito = new Ambito();
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_orgao { get; set; }
        public string nm_orgao { get; set; }
        public string sg_orgao { get; set; }
        public string sg_hierarquia { get; set; }
        public string nm_hierarquia { get; set; }

        public string ch_hierarquia { get; set; }
        public List<string> ch_cronologia { get; set; }

        public string ch_orgao_pai { get; set; }
        public string[] ch_orgao_anterior { get; set; }

        public bool st_orgao { get; set; }
        public bool st_autoridade { get; set; }
        
        public string ds_nota_de_escopo { get; set; }

        public Ambito ambito { get; set; }

        public List<OrgaoCadastrador> orgaos_cadastradores { get; set; }

        public string dt_inicio_vigencia { get; set; }
        public string dt_fim_vigencia { get; set; }


        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }

        public string ch_norma_inicio_vigencia { get; set; }
        public string ds_norma_inicio_vigencia { get; set; }

        public string ch_norma_fim_vigencia { get; set; }
        public string ds_norma_fim_vigencia { get; set; }
        
    }

    public class Orgao
    {
        public string ch_orgao { get; set; }
        public string nm_orgao { get; set; }
        public string sg_orgao { get; set; }
    }

    public class OrgaoDetalhado : OrgaoOV
    {
        public string get_sg_hierarquia_nm_vigencia
        {
            get
            {
                var ano_inicio_vigencia = !string.IsNullOrEmpty(dt_inicio_vigencia) ? dt_inicio_vigencia.Split('/')[2] : "";
                var ano_fim_vigencia = !string.IsNullOrEmpty(dt_fim_vigencia) ? dt_fim_vigencia.Split('/')[2] : "";
                var vigencia = "";
                if(ano_inicio_vigencia != "" || ano_fim_vigencia != ""){
                    vigencia = " (" + ano_inicio_vigencia + "-" + ano_fim_vigencia + ")";
                }
                return string.Format("{0} - {1}{2}", sg_hierarquia, nm_orgao, vigencia);
            }
        }
        public string get_orgaos_cadastradores
        {
            get
            {
                var sOrgaosCadastradores = "";
				for (var i = 0; i < orgaos_cadastradores.Count; i++)
                {
                    sOrgaosCadastradores += (sOrgaosCadastradores != "" ? ", " : "") + orgaos_cadastradores[i].nm_orgao_cadastrador;
                }
                return sOrgaosCadastradores;
            }
        }
        public string get_st_orgao
        {
            get
            {
                return st_orgao ? "Ativo" : "Inativo";
            }
        }
        private OrgaoDetalhado _orgao_superior { get; set; }
        private List<OrgaoDetalhado> _orgaos_inferiores { get; set; }
        private List<OrgaoDetalhado> _orgaos_anteriores { get; set; }
        private List<OrgaoDetalhado> _orgaos_posteriores { get; set; }

        public OrgaoDetalhado get_orgao_superior { get { return _orgao_superior; } }
        public OrgaoDetalhado set_orgao_superior { set { _orgao_superior = value; } }
        public List<OrgaoDetalhado> get_orgaos_inferiores { get { return _orgaos_inferiores; } }
        public List<OrgaoDetalhado> set_orgaos_inferiores { set { _orgaos_inferiores = value; } }
        public List<OrgaoDetalhado> get_orgaos_anteriores { get { return _orgaos_anteriores; } }
        public List<OrgaoDetalhado> set_orgaos_anteriores { set { _orgaos_anteriores = value; } }
        public List<OrgaoDetalhado> get_orgaos_posteriores { get { return _orgaos_posteriores; } }
        public List<OrgaoDetalhado> set_orgaos_posteriores { set { _orgaos_posteriores = value; } }

    }

    public class ValidacaoFilhos
    {
        public string ch_orgao;
        public string nm_orgao;
        public string error_msg;
    }
}
