using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Web
{
    public partial class EditarNorma : System.Web.UI.Page
    {
        protected bool isAdmin;
        protected SessaoUsuarioOV sessao_usuario;
        protected string situacoes;
        protected void Page_Load(object sender, EventArgs e)
        {
            var podeEditar = false;
            var nm_tipo_norma = "";
            sessao_usuario = Util.ValidarAcessoNasPaginas(base.Page, AcoesDoUsuario.nor_edt);
            isAdmin = Util.IsSuperAdmin(sessao_usuario);
            if (isAdmin)
            {
                podeEditar = true;
                situacoes = Util.GetSituacoes();
            }
            else
            {
                var _id_doc = Request["id_doc"];
                ulong id_doc = 0;
                var _ch_norma = Request["id_norma"];
                NormaOV normaOv = null;
                var normaRn = new NormaRN();

                if (ulong.TryParse(_id_doc, out id_doc))
                {
                    normaOv = normaRn.Doc(id_doc);
                }
                else if (!string.IsNullOrEmpty(_ch_norma))
                {
                    Util.rejeitarInject(_ch_norma);
                    normaOv = normaRn.Doc(_ch_norma);
                }
                if (normaOv != null)
                {
                    var tipoDeNormaOv = new TipoDeNormaRN().Doc(normaOv.ch_tipo_norma);
                    nm_tipo_norma = tipoDeNormaOv.nm_tipo_norma;
                    foreach (var orgaoCadastrador in tipoDeNormaOv.orgaos_cadastradores)
                    {
                        if (sessao_usuario.orgao_cadastrador.id_orgao_cadastrador == orgaoCadastrador.id_orgao_cadastrador)
                        {
                            podeEditar = true;
                            break;
                        }
                    }
                }
            }
            if (!podeEditar)
            {
                Response.Clear();
                Response.Write("<div style='width:90%;margin:auto;color:#990000; font-weight:bold; text-align:center;'>Usuário não tem permissão para editar " + (nm_tipo_norma != "" ? nm_tipo_norma : "esse tipo de ato")+". <a href='javascript:void(0);' onclick='javascript:history.back()' title=''>voltar</a></div>");
                Response.End();
            }

        }
    }
}
