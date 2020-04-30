using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class OrgaoLBW
    {
        public int Id_Orgao { get; set; }
        public string Ch_Hierarquica { get; set; }
        public OrgaoRelacionamento OrgaoSuperior { get; set; }
        public List<OrgaoRelacionamento> OrgaosInferiores { get; set; }
        public List<OrgaoRelacionamento> OrgaosAnteriores { get; set; }
        public List<OrgaoRelacionamento> OrgaosCronologia { get; set; }
        public List<OrgaoRelacionamento> OrgaosPosteriores { get; set; }
        public string Ch_Cronologica { get; set; }
        public string Cd_Migracao { get; set; }
        public string Id_OrgaoPai { get; set; }
        public string Id_OrgaoAnterior { get; set; }
        public string Nm_Orgao { get; set; }
        public string Ds_NotaEscopoOrgao { get; set; }
        public string Sg_Orgao { get; set; }
        public string Sg_OrgaoHierarquiaSuperior { get; set; }
        public string Sg_OrgaoHierarquiaSuperiorComDescricao
        {
            get { return string.Format("{0} - {1}", Sg_OrgaoHierarquiaSuperior, Nm_Orgao); }
        }
        public string Sg_OrgaoHierarquiaSuperiorComDescricaoEVigencia
        {
            get
            {
                var Dt_InicioVigenciaSplit = Dt_InicioVigencia.Split('/');
                var Dt_FimVigenciaSplit = Dt_FimVigencia.Split('/');
                string anoInicio = "";
                string anoFim = "";
                if (Dt_InicioVigenciaSplit.Length == 3)
                {
                    anoInicio = Dt_InicioVigenciaSplit[2];
                }
                if (Dt_FimVigenciaSplit.Length == 3)
                {
                    anoFim = Dt_FimVigenciaSplit[2];
                }
                return string.Format("{0} - {1} ({2}-{3})", Sg_OrgaoHierarquiaSuperior, Nm_Orgao, anoInicio, anoFim);
            }
        }
        public string Sg_OrgaoComDescricao
        {
            get { return string.Format("{0} - {1}", Sg_Orgao, Nm_Orgao); }
        }
        public bool In_Status { get; set; }
        public string StatusString
        {
            get
            {
                return (In_Status ? "Ativo" : "Inativo");
            }
        }
        public string Nm_OrgaoCadastrador { get; set; }
        public bool In_TCDF { get; set; }
        public bool In_CLDF { get; set; }
        public bool In_SEPLAG { get; set; }
        public bool In_PGDF { get; set; }
        public string Nm_Ambito { get; set; }
        public string Dt_Cadastro { get; set; }
        public string Dt_UltimaAlteracao { get; set; }
        public string Dt_InicioVigencia { get; set; }
        public string Dt_FimVigencia { get; set; }
        public string Nm_UsuarioCadastro { get; set; }
        public string Nm_UsuarioUltimaAlteracao { get; set; }
        public bool In_Autoridade { get; set; }
    }

    public class OrgaoRelacionamento
    {
        public int Id_Orgao { get; set; }
        public string Ch_Hierarquica { get; set; }
        public string Ch_Cronologica { get; set; }
        public string Nm_Orgao { get; set; }
        public string Sg_Orgao { get; set; }
        public string Sg_OrgaoHierarquiaSuperior { get; set; }
        public bool In_Status { get; set; }
        public string Nm_Ambito { get; set; }
        public string Dt_InicioVigencia { get; set; }
        public string Dt_FimVigencia { get; set; }
    }

    public class OrgaoOV
    {
        public OrgaoOV()
        {
            ch_cronologia = new List<string>();
            ch_orgao_anterior = new string[0];
            orgaos_cadastradores = new List<OrgaoCadastradorOV>();
            ambito = new AmbitoOV();
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_orgao { get; set; }
        public string nm_orgao { get; set; }
        public string sg_orgao { get; set; }
        public string sg_hierarquia { get; set; }

        public string ch_hierarquia { get; set; }
        public List<string> ch_cronologia { get; set; }

        public string ch_orgao_pai { get; set; }
        public string[] ch_orgao_anterior { get; set; }

        public bool st_orgao { get; set; }
        public bool st_autoridade { get; set; }

        public string ds_nota_de_escopo { get; set; }

        public AmbitoOV ambito { get; set; }

        public List<OrgaoCadastradorOV> orgaos_cadastradores { get; set; }

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

    public class NormaAssociada
    {
        public string ch_norma { get; set; }
        public string ds_norma { get; set; }
    }
}
