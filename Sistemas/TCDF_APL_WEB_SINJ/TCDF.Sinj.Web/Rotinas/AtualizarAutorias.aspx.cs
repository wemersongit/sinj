using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.Rotinas
{
    public partial class AtualizarAutorias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //AtualizarAutoriasPorId(68,3);

            //Server.ScriptTimeout = 14400;
            //StringBuilder id_doc_erro = new StringBuilder();
            //ulong offset = 0;
            //ulong total = 1;
            //var normaRn = new NormaRN();
            //while (offset < total)
            //{
            //    var result = normaRn.Consultar(new Pesquisa { offset = offset.ToString(), limit = "50", select = new string[] { "id_doc", "autorias" }, literal = "'d95428692da642b68145f9cb7ff4db56' = any(ch_autoria)" });
            //    total = result.result_count;
            //    offset += 50;
            //    foreach (var norma in result.results)
            //    {
            //        foreach (var autoria in norma.autorias)
            //        {
            //            if (autoria.ch_autoria == "d95428692da642b68145f9cb7ff4db56")
            //            {
            //                autoria.ch_autoria = "233";
            //                autoria.nm_autoria = "Deputada Anilceia Machado";
            //            }
            //        }
            //        if (normaRn.PathPut(norma._metadata.id_doc, "autorias", JSON.Serialize<List<Autoria>>(norma.autorias), null) != "UPDATED")
            //        {
            //            id_doc_erro.Append("<br/>" + norma._metadata.id_doc);
            //        }
            //    }
            //}

            //div_resultado.InnerHtml = "Os seguinte registros não foram atualizados:" + id_doc_erro.ToString();
        }


        // Essa funçao recebe o id da autoria certa e o id da autoria que deve ser removida
        // Ajusta todas as normas que usam a autoria errada para usarem a autoria certa
        // Depois remove a autoria errada
        public void AtualizarAutoriasPorId(ulong id_autoria_certa, ulong id_autoria_errada)
        {
            var autoriaRn = new AutoriaRN();
            var autoria_certaOv = autoriaRn.Doc(id_autoria_certa);
            var autoria_erradaOv = autoriaRn.Doc(id_autoria_errada);
            var ch_autoria_certa = autoria_certaOv.ch_autoria;
            var nm_autoria_certa = autoria_certaOv.nm_autoria;
            var ch_autoria_errada = autoria_erradaOv.ch_autoria;
            var nm_autoria_errada = autoria_erradaOv.nm_autoria;

            var normaRn = new NormaRN();
            ulong offset = 0;
            ulong total = 1;
            StringBuilder id_doc_erro = new StringBuilder();

            var result = normaRn.Consultar(new Pesquisa { offset = offset.ToString(), limit = "50", select = new string[] { "id_doc", "autorias" }, literal = "'" + ch_autoria_errada + "' = any(ch_autoria)" });
            total = result.result_count;
            offset += 50;
            foreach (var norma in result.results)
            {
                foreach (var autoria in norma.autorias)
                {
                    if (autoria.ch_autoria == ch_autoria_errada)
                    {
                        autoria.ch_autoria = ch_autoria_certa;
                        autoria.nm_autoria = nm_autoria_certa;
                    }
                }
                if (normaRn.PathPut(norma._metadata.id_doc, "autorias", JSON.Serialize<List<Autoria>>(norma.autorias), null) != "UPDATED")
                {
                    id_doc_erro.Append("<br/>" + norma._metadata.id_doc);
                }
            }
            if (String.IsNullOrEmpty(id_doc_erro.ToString()))
            {
                autoriaRn.Excluir(id_autoria_errada);
            }
        }

    }
}
