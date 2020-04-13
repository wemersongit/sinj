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
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TCDF.Sinj
{
    public class Util
    {

        // NOTE: . By Questor 
        public static byte[] FileBytesInUTF8(byte[] fileBytes)
        {
            return Encoding.Convert(
                    DetectEncoding(fileBytes),
                    Encoding.UTF8, 
                    fileBytes
                );
        }

        // NOTE: . By Questor
        public static string FileBytesInUTF8String(byte[] fileBytes)
        {
            return Encoding.UTF8.GetString(FileBytesInUTF8(fileBytes));
        }

        // NOTE: Método que verifica o "charset" que é utilizado por um arquivo 
        // através de um array de bytes referente ao mesmo. By Questor
        public static Encoding DetectEncoding(byte[] fileContent)
        {
            if (fileContent == null)
                throw new Exception("O arquivo é inválido ou inexistente.");

            MemoryStream memStream = new MemoryStream();
            memStream.Write(fileContent, 0, fileContent.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Ude.CharsetDetector cdet = new Ude.CharsetDetector();
            using (memStream)
            {
                cdet.Feed(memStream);
                cdet.DataEnd();

                // NOTE: Adicionalmente você pode usar "cdet.Confidence" para 
                // checar o nível de confiança da checagem feita pelo "Ude" 
                // ("port of Mozilla Universal Charset Detector") que varia 
                // entre '0.0~1.0'. By Questor
                if (cdet.Charset == null)
                {
                    throw new Exception("O arquivo possui formato (\"charset\") inválido.");
                }

            }

            int codePage;
            switch (cdet.Charset)
            {
                case "UTF-8":
                    // utf-8: Unicode (UTF-8)
                    codePage = 65001;
                    break;
                case "IBM855":
                    // IBM855: OEM Cyrillic
                    codePage = 855;
                    break;
                case "IBM866":
                    // cp866: Cyrillic (DOS)
                    codePage = 866;
                    break;
                case "TIS620":
                    // windows-874: Thai (Windows)
                    codePage = 874;
                    break;
                case "Shift-JIS":
                    // shift_jis: Japanese (Shift-JIS)
                    codePage = 932;
                    break;
                case "Big-5":
                    // big5: Chinese Traditional (Big5)
                    codePage = 950;
                    break;
                case "UTF-16LE":
                case "UTF-16BE":
                    // utf-16: Unicode
                    codePage = 1200;
                    break;
                case "windows-1251":
                    // windows-1251: Cyrillic (Windows)
                    codePage = 1251;
                    break;
                case "windows-1252":
                    // Windows-1252: Western European (Windows)
                    codePage = 1252;
                    break;
                case "windows-1253":
                    // windows-1253: Greek (Windows)
                    codePage = 1253;
                    break;
                case "windows-1255":
                    // windows-1255: Hebrew (Windows)
                    codePage = 1255;
                    break;
                case "x-mac-cyrillic":
                    // x-mac-cyrillic: Cyrillic (Mac)
                    codePage = 10007;
                    break;
                case "UTF-32LE":
                    // utf-32: Unicode (UTF-32)
                    codePage = 12000;
                    break;
                case "UTF-32BE":
                case "X-ISO-10646-UCS-4-3412":
                case "X-ISO-10646-UCS-4-2413":
                    // utf-32BE: Unicode (UTF-32 Big endian)
                    codePage = 12001;
                    break;
                case "ASCII":
                    // us-ascii: US-ASCII
                    codePage = 20127;
                    break;
                case "KOI8-R":
                    // koi8-r: Cyrillic (KOI8-R)
                    codePage = 20866;
                    break;
                case "ISO-8859-2":
                    // iso-8859-2: Central European (ISO)
                    codePage = 28592;
                    break;
                case "ISO-8859-5":
                    // iso-8859-5: Cyrillic (ISO)
                    codePage = 28595;
                    break;
                case "ISO-8859-7":
                    // iso-8859-7: Greek (ISO)
                    codePage = 28597;
                    break;
                case "ISO-8859-8":
                    // iso-8859-8: Hebrew (ISO-Visual)
                    codePage = 28598;
                    break;
                case "ISO-2022-JP":
                    // iso-2022-jp: Japanese (JIS)
                    codePage = 50220;
                    break;
                case "ISO-2022-KR":
                    // iso-2022-kr: Korean (ISO)
                    codePage = 50225;
                    break;
                case "ISO-2022-CN":
                case "EUC-TW":
                    // x-cp50227: Chinese Simplified (ISO-2022)
                    codePage = 50227;
                    break;
                case "EUC-JP":
                    // euc-jp: Japanese (EUC)
                    codePage = 51932;
                    break;
                case "EUC-KR":
                    // euc-kr: Korean (EUC)
                    codePage = 51949;
                    break;
                case "HZ-GB-2312":
                    // hz-gb-2312: Chinese Simplified (HZ)
                    codePage = 52936;
                    break;
                case "gb18030":
                    // GB18030: Chinese Simplified (GB18030)
                    codePage = 54936;
                    break;
                default:
                    throw new Exception("Não foi possível definir um formato (\"encoding\") adequado para o arquivo.");
            }

            return Encoding.GetEncoding(codePage);
        }

        public static void rejeitarInject(string texto)
        {
            if (!string.IsNullOrEmpty(texto) && texto.IndexOfAny(new char[] { ';', '\'', '/', '*', '_' }) > -1)
            {
                throw new ParamDangerousException("Detectada a existência de caracteres nocivos.");
            }
        }

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

        public static List<SituacaoOV> GetListaSituacoes()
        {
            List<SituacaoOV> list = new List<SituacaoOV>();
            try
            {
                var sVariavel = GetSituacoes();
                if (sVariavel != null)
                {
                    list = JSON.Deserializa<List<SituacaoOV>>(sVariavel.ToString());
                }
            }
            catch
            {
                
            }
            return list;
        }

        public static string GetSituacoes()
        {
            try
            {
                var sVariavel = new object();
                try
                {
                    sVariavel = HttpContext.Current.Application["situacoes"];
                }
                catch
                {
                    sVariavel = null;
                }
                if (sVariavel == null)
                {
                    sVariavel = JSON.Serialize<List<SituacaoOV>>(new SituacaoRN().BuscarTodos());

                    try
                    {
                        HttpContext.Current.Application["situacoes"] = sVariavel;
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

        public static bool EhCadastro()
        {
            var aplicacao = util.BRLight.Util.GetVariavel("Aplicacao");
            return aplicacao.Equals("CADASTRO", StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool ehReplica()
        {
            return util.BRLight.Util.GetVariavel("Replica") == "1";
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
            if (!UsuarioTemPermissao(usuario_session, action))
            {
                throw new PermissionException("Usuário sem permissão.");
            }
        }

        public static bool UsuarioTemPermissao(SessaoUsuarioOV usuario_session, AcoesDoUsuario action)
        {
            var ch_grupo = GetEnumDescription(action);
            if (usuario_session.grupos.IndexOf(ch_grupo) > -1)
            {
                return true;
            }
            return false;
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
                page.Response.Redirect("~/Login.aspx?cd=1", true);
            }
            catch (PermissionException)
            {
                page.Response.Redirect("~/Erro.aspx?cd=0", true);
            }
            catch (Exception ex)
            {
                page.Response.Redirect("~/Erro.aspx", true);
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
                page.Response.Redirect("~/Login.aspx?cd=1&redirecionar=" + url_pagina, true);
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
            return GetUri() + (HttpContext.Current.Request.ApplicationPath != "/" ? HttpContext.Current.Request.ApplicationPath : "");
        }

        public static string GetUri()
        {
            return HttpContext.Current.Request.UrlReferrer.Scheme + "://" + HttpContext.Current.Request.UrlReferrer.Authority;
        }



        public static string GetInfoServerApp()
        {
            string ambiente = "";
            string shortIp = "";
            string versao = "";
            string port = "";
            try
            {
                ambiente = util.BRLight.Util.GetVariavel("Ambiente");
                if(ehReplica())
                {
                    ambiente += "." + "r";
                }
            }
            catch
            {
                ambiente = "A_NV";
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
                }
            }
            catch
            {
                //Server Ip Not Verified
                shortIp = "SI_NV";
            }
            try
            {
                versao = MostrarVersao();
            }
            catch
            {
                versao = "V_NV";
            }
            try
            {
                port = GetServerPort();
            }
            catch
            {
                port = "P_NV";
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

        public static string GetFileSizeInBytes(ulong TotalBytes)
        {
            if (TotalBytes >= 1073741824) //Giga Bytes
            {
                Decimal FileSize = Decimal.Divide(TotalBytes, 1073741824);
                return String.Format("{0:##.##} GB", FileSize);
            }
            else if (TotalBytes >= 1048576) //Mega Bytes
            {
                Decimal FileSize = Decimal.Divide(TotalBytes, 1048576);
                return String.Format("{0:##.##} MB", FileSize);
            }
            else if (TotalBytes >= 1024) //Kilo Bytes
            {
                Decimal FileSize = Decimal.Divide(TotalBytes, 1024);
                return String.Format("{0:##.##} KB", FileSize);
            }
            else if (TotalBytes > 0)
            {
                Decimal FileSize = TotalBytes;
                return String.Format("{0:##.##} Bytes", FileSize);
            }
            else
            {
                return "0 Bytes";
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
        [Description("NOR.FST")]
        nor_fst,
        [Description("NOR.EML")]
        nor_eml,
        [Description("NOR.HSP")]
        nor_hsp,
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
