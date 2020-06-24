using System;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.OV;
using System.Collections.Generic;
using neo.BRLightREST;

namespace TCDF.Sinj.Web
{
    public partial class Sinj : System.Web.UI.MasterPage
    {
        public static ulong totalNovos { get; set; }
        public static SessaoUsuarioOV oSessaoUsuario { get; set; }
        private static SessaoNotifiquemeOV oSessaoNotifiqueme;
        protected override void OnInit(EventArgs e)
        {
            oSessaoUsuario = Util.ValidarAcesso(base.Page);
            var bAdministrar = false;
            var bCadastrar = false;
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdf_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdf_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdf_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdf_pes)) > -1)
            {
                bAdministrar = true;
                li_tipo_de_fonte.Visible = true;
            }
            else
            {
                li_tipo_de_fonte.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdn_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdn_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdn_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdn_pes)) > -1)
            {
                bAdministrar = true;
                li_tipo_de_norma.Visible = true;
            }
            else
            {
                li_tipo_de_norma.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdp_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdp_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdp_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdp_pes)) > -1)
            {
                bAdministrar = true;
                li_tipo_de_publicacao.Visible = true;
            }
            else
            {
                li_tipo_de_publicacao.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdr_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdr_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdr_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.tdr_pes)) > -1)
            {
                bAdministrar = true;
                li_tipo_de_relacao.Visible = true;
            }
            else
            {
                li_tipo_de_relacao.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.int_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.int_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.int_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.int_pes)) > -1)
            {
                bAdministrar = true;
                li_interessado.Visible = true;
            }
            else
            {
                li_interessado.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.sit_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.sit_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.sit_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.sit_pes)) > -1)
            {
                bAdministrar = true;
                li_situacao.Visible = true;
            }
            else
            {
                li_situacao.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.aut_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.aut_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.aut_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.aut_pes)) > -1)
            {
                bAdministrar = true;
                li_autoria.Visible = true;
            }
            else
            {
                li_autoria.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rqe_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rqe_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rqe_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rqe_pes)) > -1)
            {
                bAdministrar = true;
                li_requerente.Visible = true;
            }
            else
            {
                li_requerente.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rqi_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rqi_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rqi_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rqi_pes)) > -1)
            {
                bAdministrar = true;
                li_requerido.Visible = true;
            }
            else
            {
                li_requerido.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rel_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rel_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rel_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rel_pes)) > -1)
            {
                bAdministrar = true;
                li_relator.Visible = true;
            }
            else
            {
                li_relator.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.pro_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.pro_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.pro_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.pro_pes)) > -1)
            {
                bAdministrar = true;
                li_procurador.Visible = true;
            }
            else
            {
                li_procurador.Visible = false;
            }

            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.org_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.org_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.org_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.org_pes)) > -1)
            {
                li_orgao.Visible = true;
            }
            else
            {
                li_orgao.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.voc_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.voc_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.voc_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.voc_pes)) > -1)
            {
                li_vocabulario.Visible = true;
            }
            else
            {
                li_vocabulario.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.usr_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.usr_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.usr_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.usr_pes)) > -1)
            {
                li_usuario.Visible = true;
            }
            else
            {
                li_usuario.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.nor_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.nor_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.nor_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.nor_pes)) > -1)
            {
                bCadastrar = true;
                li_norma.Visible = true;
                li_vide.Visible = true;
            }
            else
            {
                li_norma.Visible = false;
                li_vide.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.nor_edt)) > -1)
            {
                bCadastrar = true;
                li_vide.Visible = true;
            }
            else
            {
                li_vide.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.dio_inc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.dio_edt)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.dio_exc)) > -1 ||
            oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.dio_pes)) > -1)
            {
                bCadastrar = true;
                li_diario.Visible = true;
            }
            else
            {
                li_diario.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.rio_pes)) > -1)
            {
                li_relatorio.Visible = true;
            }
            else
            {
                li_relatorio.Visible = false;
            }
            if (oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.aud_ace)) > -1 ||
                oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.aud_err)) > -1 ||
                oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.aud_lix)) > -1 ||
                oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.aud_ope)) > -1 ||
                oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.aud_pes)) > -1 ||
                oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.aud_pus)) > -1 ||
                oSessaoUsuario.grupos.IndexOf(util.BRLight.Util.GetEnumDescription(AcoesDoUsuario.aud_ses)) > -1)
            {
                li_auditoria.Visible = true;
            }
            else
            {
                li_auditoria.Visible = false;
            }
            li_administrar.Visible = bAdministrar;
            li_cadastro.Visible = bCadastrar;

            try
            {
                oSessaoNotifiqueme = new NotifiquemeRN().LerSessaoNotifiquemeOv();
            }
            catch
            {
                oSessaoNotifiqueme = null;
            }
            totalNovos = new FaleConoscoRN().Consultar(new Pesquisa() { select = new string[0], literal = "st_atendimento='Novo'" }).result_count;
        }
        private bool ValidarMenuPorGrupo(string li)
        {
            return false;
        }

        public static string MostrarTema()
        {
            if (oSessaoUsuario != null)                             
            {
                return oSessaoUsuario.ch_tema;
            }                                                      
            else                                                   
            {                                                      
                return "cinza";
            }
        }

        public static string jsValorChave()
        {
            return string.Concat(
                "  var _urlApps = '", util.BRLight.Util.GetVariavel("apps", true), "';"
                , "  var _urlPadrao = '", Util._urlPadrao, "';"
                , "  var _ambiente = '", util.BRLight.Util.GetVariavel("Ambiente"), "';"
                , "  var _aplicacao = '", util.BRLight.Util.GetVariavel("Aplicacao"), "';" //Util quando as aplicações estiverem unificadas...
                , "  var _versao = '", Util.MostrarVersao(), "';"
                , "  var _extensoes = ", util.BRLight.Util.GetVariavel("Extensoes"), ";"
                , "  var _ambitos = ", Util.GetAmbitos(), ";"
                , "  var _orgaos_cadastradores = ", Util.GetOrgaosCadastradores(), ";"
                , "  var _user = ", (oSessaoUsuario != null) ? JSON.Serialize<SessaoUsuarioOV>(oSessaoUsuario) : "null", ";"
                , "  var _notifiqueme = ", (oSessaoNotifiqueme != null) ? JSON.Serialize<SessaoNotifiquemeOV>(oSessaoNotifiqueme) : "null", ";"
                , "  var _nm_cookie_push = '", util.BRLight.Util.GetVariavel("NmCookiePush"), "';"
                , "  var _nr_total_novos_chamados = ", totalNovos, ";"
                , "  try { "
                , "     if (!(((typeof (JSON) !== 'undefined') && (typeof (JSON.stringify) === 'function') && (typeof (JSON.parse) === 'function'))) || (/MSIE [567]/.test(navigator.userAgent))) {"
                , "        var s = document.createElement('script');"
                , "        s.type = 'text/javascript';"
                , "        s.async = true;"
                , "        s.src = '", Util._urlPadrao, "/Scripts/json3.min.js' ;"
                , "        document.getElementsByTagName('head')[0].appendChild(s);"
                , "     } "
                , "  } catch (e) { "
                , "     top.document.location.href = '/errorjson.html';"
                , "  } "
                );
        }
    }
}
