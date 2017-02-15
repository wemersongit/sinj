using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using System.ComponentModel;
using System.Web;
using util.BRLight;
using TCDF.Sinj.Log;
using System.Reflection;

namespace TCDF.Sinj
{
    public class Util
    {
        public static string _urlPadrao
        {
            get
            {
                return util.BRLight.Util.GetVariavel("Padrao", true);
            }
        }

        public static string GetAmbitos()
        {
            try
            {
                var sVariavel = new object();
                try
                {
                    sVariavel = HttpContext.Current.Application["ambitos"];
                }
                catch
                {
                    sVariavel = null;
                }
                if (sVariavel == null)
                {
                    sVariavel = JSON.Serialize<List<AmbitoOV>>(new AmbitoRN().BuscarTodos());

                    try
                    {
                        HttpContext.Current.Application["ambitos"] = sVariavel;
                    }
                    catch
                    {

                    }
                }
                return sVariavel.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static string GetOrgaosCadastradores()
        {
            try
            {
                var sVariavel = new object();
                try
                {
                    sVariavel = HttpContext.Current.Application["orgaoscadastradores"];
                }
                catch
                {
                    sVariavel = null;
                }
                if (sVariavel == null)
                {
                    sVariavel = JSON.Serialize<List<OrgaoCadastradorOV>>(new OrgaoCadastradorRN().BuscarTodos());

                    try
                    {
                        HttpContext.Current.Application["orgaoscadastradores"] = sVariavel;
                    }
                    catch
                    {

                    }
                }
                return sVariavel.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static string MostrarVersao()
        {
            return util.BRLight.Util.GetVariavel("versao");
        }

        public static SessaoNotifiquemeOV LerSessaoPush()
        {
            return new NotifiquemeRN().LerSessaoNotifiquemeOv();
        }

        public static SessaoNotifiquemeOV ValidarSessaoPush()
        {
            SessaoNotifiquemeOV push_session = new NotifiquemeRN().LerSessaoNotifiquemeOv();
            if (push_session == null)
            {
                throw new SessionExpiredException("Sessão expirou.");
            }
            return push_session;
        }

        public static SessaoUsuarioOV LerSessao()
        {
            return new SessaoRN().LerSessaoUsuarioOv();
        }

        public static SessaoUsuarioOV ValidarSessao()
        {
            SessaoUsuarioOV usuario_session = new SessaoRN().LerSessaoUsuarioOv();
            if (usuario_session == null)
            {
                throw new SessionExpiredException("Sessão expirou.");
            }
            return usuario_session;
        }

        public static void ValidarSuperAdmin(SessaoUsuarioOV usuario_session)
        {
            if (!IsSuperAdmin(usuario_session))
            {
                throw new PermissionException("Usuário sem permissão.");
            }
        }

        public static bool IsSuperAdmin(SessaoUsuarioOV usuario_session)
        {
            return usuario_session.ch_perfil == "super_administrador";
        }

        public static void ValidarUsuario(SessaoUsuarioOV usuario_session, AcoesDoUsuario action)
        {
            if (!new UsuarioRN().ValidarPermissao(usuario_session.nm_login_usuario, GetEnumDescription(action)))
            {
                throw new PermissionException("Usuário sem permissão.");
            }
        }

        public static SessaoUsuarioOV ValidarAcessoNasPaginas(System.Web.UI.Page page, AcoesDoUsuario action)
        {
            SessaoUsuarioOV usuario_session = null;
            try
            {
                usuario_session = ValidarSessao();
                ValidarUsuario(usuario_session, action);
            }
            catch (SessionExpiredException)
            {
                page.Response.Redirect("./Login.aspx?cd=1", true);
            }
            catch (PermissionException)
            {
                page.Response.Redirect("./Erro.aspx?cd=0", true);
            }
            catch (Exception ex)
            {
                page.Response.Redirect("./Erro.aspx", true);
                var erro = new ErroRequest
                {
                    Pagina = page.Request.Path,
                    RequestQueryString = page.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                LogErro.gravar_erro(Util.GetEnumDescription(action), erro, usuario_session.nm_usuario, usuario_session.nm_login_usuario);
            }
            return usuario_session;
        }

        public static SessaoUsuarioOV ValidarAcesso(System.Web.UI.Page page)
        {
            var url_pagina = "";
            try
            {
                url_pagina = page.Request.RawUrl.ToString();
                return ValidarSessao();
            }
            catch (SessionExpiredException)
            {
                page.Response.Redirect("./Login.aspx?cd=1&redirecionar=" + url_pagina, true);
                return null;
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetUriAndPath()
        {
            return HttpContext.Current.Request.UrlReferrer.Scheme + "://" + HttpContext.Current.Request.UrlReferrer.Authority + (HttpContext.Current.Request.ApplicationPath != "/" ? HttpContext.Current.Request.ApplicationPath : "");
        }

        public static string GetInfoServerApp()
        {
            string ambiente;
            string shortIp;
            string versao;
            string port;
            try
            {
                ambiente = Config.ValorChave("Ambiente", false);
            }
            catch
            {
                ambiente = "";
            }
            try
            {
                string ip = Util.GetServerIp();
                if (!string.IsNullOrEmpty(ip))
                {
                    if (ip.LastIndexOf('.') > -1)
                    {
                        shortIp = ip.Substring(ip.LastIndexOf('.')).Replace(".", "");
                    }
                    else if (ip.LastIndexOf(':') > -1)
                    {
                        shortIp = ip.Substring(ip.LastIndexOf(':')).Replace(":", "");
                    }
                    else if (ip.LastIndexOf('/') > -1)
                    {
                        shortIp = ip.Substring(ip.LastIndexOf('/')).Replace("/", "");
                    }
                    else
                    {
                        shortIp = "";
                    }
                }
                else
                {
                    shortIp = "";
                }
            }
            catch
            {
                shortIp = "";
            }
            try
            {
                versao = MostrarVersao();
            }
            catch
            {
                versao = "";
            }
            try
            {
                port = GetServerPort();
            }
            catch
            {
                port = "";
            }
            return ambiente + "." + versao + "." + shortIp + ":" + port;
        }

        public static string GetServerIp()
        {
            try
            {
                return HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            }
            catch
            {
                return "";
            }
        }

        public static string GetServerPort()
        {
            try
            {
                return HttpContext.Current.Request.Url.Port.ToString();
            }
            catch
            {
                return "";
            }
        }
    }

    public enum AcoesDoUsuario
    {
        //Norma
        [Description("NOR.INC")]
        nor_inc,
        [Description("NOR.EDT")]
        nor_edt,
        [Description("NOR.EXC")]
        nor_exc,
        [Description("NOR.PES")]
        nor_pes,
        [Description("NOR.VIS")]
        nor_vis,
        //Diario
        [Description("DIO.INC")]
        dio_inc,
        [Description("DIO.EDT")]
        dio_edt,
        [Description("DIO.EXC")]
        dio_exc,
        [Description("DIO.PES")]
        dio_pes,
        [Description("DIO.VIS")]
        dio_vis,
        //Autoria
        [Description("AUT.INC")]
        aut_inc,
        [Description("AUT.EDT")]
        aut_edt,
        [Description("AUT.EXC")]
        aut_exc,
        [Description("AUT.VIS")]
        aut_vis,
        [Description("AUT.PES")]
        aut_pes,
        //Orgao
        [Description("ORG.INC")]
        org_inc,
        [Description("ORG.EDT")]
        org_edt,
        [Description("ORG.EXC")]
        org_exc,
        [Description("ORG.PES")]
        org_pes,
        [Description("ORG.VIS")]
        org_vis,
        //Tipo de Fonte
        [Description("TDF.INC")]
        tdf_inc,
        [Description("TDF.EDT")]
        tdf_edt,
        [Description("TDF.EXC")]
        tdf_exc,
        [Description("TDF.PES")]
        tdf_pes,
        [Description("TDF.VIS")]
        tdf_vis,
        //Tipo de Norma
        [Description("TDN.INC")]
        tdn_inc,
        [Description("TDN.EDT")]
        tdn_edt,
        [Description("TDN.EXC")]
        tdn_exc,
        [Description("TDN.PES")]
        tdn_pes,
        [Description("TDN.VIS")]
        tdn_vis,
        //Tipo de Publicacao
        [Description("TDP.INC")]
        tdp_inc,
        [Description("TDP.EDT")]
        tdp_edt,
        [Description("TDP.EXC")]
        tdp_exc,
        [Description("TDP.PES")]
        tdp_pes,
        [Description("TDP.VIS")]
        tdp_vis,
        //Interessadp
        [Description("INT.INC")]
        int_inc,
        [Description("INT.EDT")]
        int_edt,
        [Description("INT.EXC")]
        int_exc,
        [Description("INT.PES")]
        int_pes,
        [Description("INT.VIS")]
        int_vis,
        //Situacao
        [Description("SIT.INC")]
        sit_inc,
        [Description("SIT.EDT")]
        sit_edt,
        [Description("SIT.EXC")]
        sit_exc,
        [Description("SIT.PES")]
        sit_pes,
        [Description("SIT.VIS")]
        sit_vis,
        //Tipo de Relacao
        [Description("TDR.INC")]
        tdr_inc,
        [Description("TDR.EDT")]
        tdr_edt,
        [Description("TDR.EXC")]
        tdr_exc,
        [Description("TDR.PES")]
        tdr_pes,
        [Description("TDR.VIS")]
        tdr_vis,
        //Requerente
        [Description("RQE.INC")]
        rqe_inc,
        [Description("RQE.EDT")]
        rqe_edt,
        [Description("RQE.EXC")]
        rqe_exc,
        [Description("RQE.PES")]
        rqe_pes,
        [Description("RQE.VIS")]
        rqe_vis,
        //Requerido
        [Description("RQI.INC")]
        rqi_inc,
        [Description("RQI.EDT")]
        rqi_edt,
        [Description("RQI.EXC")]
        rqi_exc,
        [Description("RQI.PES")]
        rqi_pes,
        [Description("RQI.VIS")]
        rqi_vis,
        //Relator
        [Description("REL.INC")]
        rel_inc,
        [Description("REL.EDT")]
        rel_edt,
        [Description("REL.EXC")]
        rel_exc,
        [Description("REL.PES")]
        rel_pes,
        [Description("REL.VIS")]
        rel_vis,
        //Procurador
        [Description("PRO.INC")]
        pro_inc,
        [Description("PRO.EDT")]
        pro_edt,
        [Description("PRO.EXC")]
        pro_exc,
        [Description("PRO.PES")]
        pro_pes,
        [Description("PRO.VIS")]
        pro_vis,
        //Usuario
        [Description("USR.SAI")]
        usr_sai,
        [Description("USR.INC")]
        usr_inc,
        [Description("USR.EDT")]
        usr_edt,
        [Description("USR.EXC")]
        usr_exc,
        [Description("USR.PES")]
        usr_pes,
        [Description("USR.VIS")]
        usr_vis,
        //Vocabulario
        [Description("VOC.INC")]
        voc_inc,
        [Description("VOC.EDT")]
        voc_edt,
        [Description("VOC.EXC")]
        voc_exc,
        [Description("VOC.PES")]
        voc_pes,
        [Description("VOC.VIS")]
        voc_vis,
        [Description("VOC.GER")]
        voc_ger,
        //Configuracao
        [Description("CFG.EDT")]
        cfg_edt,
        [Description("CFG.VIS")]
        cfg_vis,
        //Relatorio
        [Description("RIO.PES")]
        rio_pes,
        //Push
        [Description("PUS.INC")]
        pus_inc,
        [Description("PUS.EDT")]
        pus_edt,
        [Description("PUS.PES")]
        pus_pes,
        [Description("PUS.VIS")]
        pus_vis,
        //Cesta
        [Description("CST.PES")]
        cst_pes,
        //Auditoria
        [Description("AUD.ERR")]
        aud_err,
        [Description("AUD.OPE")]
        aud_ope,
        [Description("AUD.ACE")]
        aud_ace,
        [Description("AUD.PES")]
        aud_pes,
        [Description("AUD.LIX")]
        aud_lix,
        [Description("AUD.SES")]
        aud_ses,
        [Description("AUD.PUS")]
        aud_pus,
        //Arquivos
        [Description("ARQ.PRO")]
        arq_pro
    }
}
