using System;
using System.Collections.Generic;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class TipoDeNormaOV : metadata
    {
        public TipoDeNormaOV(){
            sgs_tipo_norma = new List<string>();
            orgaos_cadastradores = new List<OrgaoCadastrador>();
            alteracoes = new List<AlteracaoOV>();
        }

        public string ch_tipo_norma { get; set; }
        public string nm_tipo_norma { get; set; }
        public List<string> sgs_tipo_norma { get; set; }
        public string ds_tipo_norma { get; set; }

        public List<OrgaoCadastrador> orgaos_cadastradores { get; set; }

        public bool in_g1 { get; set; }
        public bool in_g2 { get; set; }
        public bool in_g3 { get; set; }
        public bool in_g4 { get; set; }
        public bool in_g5 { get; set; }

        public bool in_conjunta { get; set; }
        public bool in_apelidavel { get; set; }

        /// <summary>
        /// Indica quais as normas s鉶 question醰eis por a珲es. Observe que durante o 
        ///  cadastro de uma a玢o pelo PGDF, ao ser preenchido no VIDE qual a norma esta 
        ///  sendo questionada, somente estes tipos estar鉶 dispon韛eis.
        /// </summary>
        public bool in_questionavel { get; set; }

        /// <summary>
        /// Indica se esse tipo de norma deve usar uma enumera玢o a partir do orgao
        /// Valor utilizado apenas para cria玢o do numero para a chave para n鉶-duplica玢o.
        /// </summary>
        public bool in_numeracao_por_orgao { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }

    }

    public class TipoDeNorma : TipoDeNormaOV
    {

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

        public string get_grupos
        {
            get
            {
                var sGrupos = "";
                if (in_g1)
                {
                    sGrupos += (sGrupos != "" ? ", " : "") + "Grupo 1";
                }
                if (in_g2)
                {
                    sGrupos += (sGrupos != "" ? ", " : "") + "Grupo 2";
                }
                if (in_g3)
                {
                    sGrupos += (sGrupos != "" ? ", " : "") + "Grupo 3";
                }
                if (in_g4)
                {
                    sGrupos += (sGrupos != "" ? ", " : "") + "Grupo 4";
                }
                if (in_g5)
                {
                    sGrupos += (sGrupos != "" ? ", " : "") + "Grupo 5";
                }
                return sGrupos;
            }
        }

        public bool EhLei
        {
            get
            {
				return
					nm_tipo_norma.ToLower () == "decreto legislativo" ||
					nm_tipo_norma.ToLower () == "emenda a lei orgânica" ||
					nm_tipo_norma.ToLower () == "lei complementar" ||
					nm_tipo_norma.ToLower () == "lei";
            }
        }

        public bool EhDecreto
        {
            get
            {
				return nm_tipo_norma.ToLower() == "decreto";
            }
        }

        public bool EhResolucao
        {
            get
            {
				return nm_tipo_norma.ToLower() == "resolução"; 
            }
        }

        public bool EhPortaria
        {
            get
            {
				return nm_tipo_norma.ToLower() == "portaria";
            }
        }

        public override string ToString()
        {
            return nm_tipo_norma;
        }
    }
}