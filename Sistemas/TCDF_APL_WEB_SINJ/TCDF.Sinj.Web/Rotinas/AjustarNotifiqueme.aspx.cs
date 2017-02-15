using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using System.Reflection;

namespace TCDF.Sinj.Web.Rotinas
{
    public partial class AjustarNotifiqueme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    var notifiquemeOv = new NotifiquemeOV();
            //    var notifiquemeRn = new NotifiquemeRN();
            //    var tipo_de_normaRn = new TipoDeNormaRN();
            //    var tipo_de_normaOv = new TipoDeNormaOV();
            //    var orgaoRn = new OrgaoRN();
            //    var orgaoOv = new OrgaoOV();
            //    var vocabularioRn = new VocabularioRN();
            //    var vocabularioOv = new VocabularioOV();
            //    ulong id_push = 0;
            //    var pesquisa = new Pesquisa();
            //    pesquisa.limit = null;
            //    var lista_notifiqueme = notifiquemeRn.Consultar(pesquisa);
            //    StringBuilder lista_alteradas = new StringBuilder();
            //    StringBuilder lista_sucesso = new StringBuilder();
            //    StringBuilder lista_deletado = new StringBuilder();
            //    foreach (var usuario_notifiqueme in lista_notifiqueme.results)
            //    {
            //        notifiquemeOv = usuario_notifiqueme;
            //        id_push = notifiquemeOv._metadata.id_doc;
            //        var j = 0;
            //        foreach (var parametros_criacao_norma in notifiquemeOv.criacao_normas_monitoradas)
            //        {
            //            var alteracao = false;
            //            // Se algum parametro for 0, é um valor inválido. Será convertido para null
            //            if (parametros_criacao_norma.ch_orgao_criacao == "0")
            //            {
            //                parametros_criacao_norma.ch_orgao_criacao = "";
            //                lista_alteradas.Append(usuario_notifiqueme._metadata.id_doc.ToString() + " - ch_orgao_criacao");
            //                alteracao = true;
            //            }
            //            if (parametros_criacao_norma.ch_termo_criacao == "0")
            //            {
            //                parametros_criacao_norma.ch_termo_criacao = "";
            //                lista_alteradas.Append(usuario_notifiqueme._metadata.id_doc.ToString() + " - ch_termo_criacao");
            //                alteracao = true;
            //            }
            //            if (parametros_criacao_norma.ch_tipo_norma_criacao == "0")
            //            {
            //                parametros_criacao_norma.ch_tipo_norma_criacao = "";
            //                lista_alteradas.Append(usuario_notifiqueme._metadata.id_doc.ToString() + " - ch_tipo_norma_criacao");
            //                alteracao = true;
            //            }
            //            if (parametros_criacao_norma.ch_tipo_termo_criacao == "0")
            //            {
            //                parametros_criacao_norma.ch_tipo_termo_criacao = "";
            //                lista_alteradas.Append(usuario_notifiqueme._metadata.id_doc.ToString() + " - ch_tipo_termo_criacao");
            //                alteracao = true;
            //            }
            //            // Se houver o primeiro conector mas não houver valor nenhum para ser conectado,
            //            // o primeiro conector é transformado em vazio.
            //            if (!string.IsNullOrEmpty(parametros_criacao_norma.primeiro_conector_criacao))
            //            {
            //                if (string.IsNullOrEmpty(parametros_criacao_norma.ch_orgao_criacao) && string.IsNullOrEmpty(parametros_criacao_norma.ch_tipo_termo_criacao) && string.IsNullOrEmpty(parametros_criacao_norma.ch_termo_criacao))
            //                {
            //                    parametros_criacao_norma.primeiro_conector_criacao = "";
            //                    lista_alteradas.Append(usuario_notifiqueme._metadata.id_doc.ToString() + " - primeiro_conector alterado para vazio;");
            //                    alteracao = true;
            //                }
            //            }
            //            // Se o conector for null e houver valores que devem ser conectados na query de busca, 
            //            // o conector é transformado em 'E'.
            //            if (string.IsNullOrEmpty(parametros_criacao_norma.primeiro_conector_criacao))
            //            {
            //                if (!string.IsNullOrEmpty(parametros_criacao_norma.ch_tipo_norma_criacao))
            //                {
            //                    if (!string.IsNullOrEmpty(parametros_criacao_norma.ch_orgao_criacao) || !string.IsNullOrEmpty(parametros_criacao_norma.ch_termo_criacao))
            //                    {
            //                        parametros_criacao_norma.primeiro_conector_criacao = "E";
            //                        lista_alteradas.Append("<br/>" + usuario_notifiqueme._metadata.id_doc.ToString() + " - primeiro_conector alterado para 'E';");
            //                        alteracao = true;
            //                    }
            //                }
            //            }
            //            if (string.IsNullOrEmpty(parametros_criacao_norma.segundo_conector_criacao))
            //            {
            //                if (!string.IsNullOrEmpty(parametros_criacao_norma.ch_orgao_criacao) && !string.IsNullOrEmpty(parametros_criacao_norma.ch_termo_criacao))
            //                {
            //                    parametros_criacao_norma.segundo_conector_criacao = "E";
            //                    lista_alteradas.Append("<br/>" + usuario_notifiqueme._metadata.id_doc.ToString() + " - segundo_conector alterado para 'E';");
            //                    alteracao = true;
            //                }
            //            }

            //            if (!string.IsNullOrEmpty(parametros_criacao_norma.ch_tipo_norma_criacao) && string.IsNullOrEmpty(parametros_criacao_norma.nm_tipo_norma_criacao))
            //            {
            //                tipo_de_normaOv = tipo_de_normaRn.Doc(parametros_criacao_norma.ch_tipo_norma_criacao);
            //                parametros_criacao_norma.nm_tipo_norma_criacao = tipo_de_normaOv.nm_tipo_norma;
            //                lista_alteradas.Append("<br/>" + usuario_notifiqueme._metadata.id_doc.ToString() + " - nm_tipo_norma adicionado;");
            //                alteracao = true;
            //            }
            //            if (!string.IsNullOrEmpty(parametros_criacao_norma.ch_orgao_criacao) && string.IsNullOrEmpty(parametros_criacao_norma.nm_orgao_criacao))
            //            {
            //                try
            //                {
            //                    orgaoOv = orgaoRn.Doc(parametros_criacao_norma.ch_orgao_criacao);
            //                    parametros_criacao_norma.nm_orgao_criacao = orgaoOv.nm_orgao;
            //                    lista_alteradas.Append("<br/>" + usuario_notifiqueme._metadata.id_doc.ToString() + " - nm_orgao adicionado;");
            //                    alteracao = true;
            //                }
            //                catch
            //                {
            //                    parametros_criacao_norma.ch_orgao_criacao = "";
            //                }
            //            }

            //            if (!string.IsNullOrEmpty(parametros_criacao_norma.ch_termo_criacao) && string.IsNullOrEmpty(parametros_criacao_norma.nm_termo_criacao))
            //            {
            //                vocabularioOv = vocabularioRn.Doc(parametros_criacao_norma.ch_termo_criacao);
            //                parametros_criacao_norma.nm_termo_criacao = vocabularioOv.nm_termo;
            //                lista_alteradas.Append("<br/>" + usuario_notifiqueme._metadata.id_doc.ToString() + " - nm_termo adicionado;");
            //                alteracao = true;
            //            }

            //            if (notifiquemeRn.Atualizar(id_push, notifiquemeOv))
            //            {
            //                lista_sucesso.Append("<br/>" + id_push.ToString());
            //            }
            //            if (parametros_criacao_norma.ch_tipo_termo_criacao == null && parametros_criacao_norma.ch_orgao_criacao == null && parametros_criacao_norma.ch_termo_criacao == null && parametros_criacao_norma.ch_tipo_norma_criacao == null)
            //            {
            //                var retornoPath = notifiquemeRn.PathDelete(id_push, "criacao_normas_monitoradas/" + j, null);
            //                if (retornoPath == "DELETED")
            //                {
            //                    lista_deletado.Append("<br/>" + id_push.ToString());
            //                    j--;
            //                }
            //            }
            //            j++;
            //        }
            //        notifiquemeRn.Atualizar(id_push, notifiquemeOv);
            //    }
            //    div_resultado.InnerHtml = "Os seguinte registros foram atualizados:" + lista_sucesso + "<br/><br/>Os seguintes registros foram deletados:" + lista_deletado + "<br/><br/>As alterações são:" + lista_alteradas;
            //}
            //catch  (Exception ex)
            //{
            //    div_resultado.InnerHtml = ex.ToString();
            //}
        }
    }
}