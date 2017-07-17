using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using System.IO;
using util.BRLight;
using TCDF.Sinj;

namespace levantamento_vides
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //new Program().GerarRelatorioVidesSemNormas();
            }
            catch (Exception ex)
            {
                var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                Console.WriteLine("Exception: " + mensagem);
                Console.Beep();
                Console.Read();
            }
        }

        private void GerarSiteMaps()
        {
            var tipoNormaRn = new TCDF.Sinj.RN.TipoDeNormaRN();

            var query = new Pesquisa();
            query.limit = null;
            var tipos = tipoNormaRn.Consultar(query);
            
            ulong from = 0;
            ulong size = 100;
            ulong result_count = 1;
            var normaRn = new TCDF.Sinj.RN.NormaRN();

            query.limit = size.ToString();
            query.order_by.asc = new string[] { "dt_assinatura" };
            query.select = new string[] { "ch_norma", "ch_tipo_norma", "nm_tipo_norma", "nr_norma", "dt_assinatura", "ar_atualizado", "ar_fonte" };
            StringBuilder sb = new StringBuilder();
            foreach(var tipo in tipos.results){
                while (from <= result_count)
                {
                    try
                    {
                        query.offset = from.ToString();

                        Console.WriteLine("offset: " + query.offset);

                        var result = normaRn.Consultar(query);
                        result_count = result.result_count;
                        foreach (var norma in result.results){
                            if(string.IsNullOrEmpty(norma.getIdFileArquivoVigente())){
                                continue;
                            }
                            sb.AppendLine("    <loc>http://www.sinj.df.gov.br/sinj/.aspx?tipo_pesquisa=norma&all=&ch_tipo_norma=10000000&nm_tipo_norma=ADC&nr_norma=&ano_assinatura=&ch_orgao=&ch_hierarquia=&sg_hierarquia_nm_vigencia=&origem_por=toda_a_hierarquia_em_qualquer_epoca</loc>");
    //                        <url>
        
    //    <changefreq>monthly</changefreq>
    //    <priority>0.5</priority>
    //</url>
                        }
                        from += size;
                    }
                    catch (Exception ex)
                    {
                        var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                        Console.WriteLine("Exception: " + mensagem);
                        Console.Beep();
                        Console.Read();
                    }

                }
            }
        }

        private void GerarRelatorioVidesSemNormas()
        {
            ulong from = 0;
            ulong size = 100;
            ulong result_count = 1;
            var query = new Pesquisa();
            var normaRn = new TCDF.Sinj.RN.NormaRN();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table><thead><tr><th>NORMAS COM VIDES INEXISTENTES</th></tr></thead><tbody>");
            while (from <= result_count)
            {
                try
                {
                    query.offset = from.ToString();
                    query.limit = size.ToString();
                    query.order_by.asc = new string[] { "dt_assinatura" };
                    query.select = new string[] { "id_doc", "nm_tipo_norma", "nr_norma", "dt_assinatura", "ch_norma", "vides" };

                    Console.WriteLine("offset: " + query.offset);

                    var result = normaRn.Consultar(query);
                    result_count = result.result_count;

                    foreach (var norma in result.results)
                    {
                        var tr_norma = "";
                        try
                        {
                            Console.WriteLine("id_doc: " + norma._metadata.id_doc);
                            if (norma.vides != null && norma.vides.Count > 0)
                            {
                                var tr_vide = "";
                                var chaves = new List<string>();
                                foreach (var vide in norma.vides)
                                {
                                    if (chaves.Count<string>(ch => ch == vide.ch_norma_vide) > 0) continue;
                                    if (vide.ch_norma_vide == "0")
                                    {
                                        chaves.Add(vide.ch_norma_vide);
                                        tr_vide += "<tr><td style='color:#FF0000;'>" + vide.nm_tipo_norma_vide + " " + vide.nr_norma_vide + " " + vide.dt_assinatura_norma_vide + " obs.: possui chave igual a zero.</td></tr>";
                                        continue;
                                    }
                                    try
                                    {
                                        var normaOv = normaRn.Doc(vide.ch_norma_vide);
                                    }
                                    catch (DocNotFoundException ex)
                                    {
                                        chaves.Add(vide.ch_norma_vide);
                                        tr_vide += "<tr><td><a href='http://www.sinj.df.gov.br/sinj/DetalhesDeNorma.aspx?id_norma=" + vide.ch_norma_vide + "'/>" + vide.nm_tipo_norma_vide + " " + vide.nr_norma_vide + " " + vide.dt_assinatura_norma_vide + "</a></td></tr>";
                                    }
                                }
                                if (tr_vide != "")
                                {
                                    tr_norma += "<tr><td><a href='http://www.sinj.df.gov.br/sinj/DetalhesDeNorma.aspx?id_norma=" + norma.ch_norma + "'/>" + norma.nm_tipo_norma + " " + norma.nr_norma + " " + norma.dt_assinatura + "</a></td><td><table><thead><tr><th>LINK QUEBRADO DO VIDE</th></tr></thead><tbody>" + tr_vide + "</tbody></table></td></tr>";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                            Console.WriteLine("Exception: " + mensagem);
                            Console.Beep();
                            Console.Read();
                        }
                        if (tr_norma != "")
                        {
                            sb.AppendLine(tr_norma);
                        }

                    }
                    from += size;
                }
                catch (Exception ex)
                {
                    var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                    Console.WriteLine("Exception: " + mensagem);
                    Console.Beep();
                    Console.Read();
                }
            }

            sb.AppendLine("</tbody></table>");

            var _file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "levantamento_vides.html");
            if (!_file.Exists)
            {
                _file.Delete();
            }
            var stream = _file.AppendText();
            stream.Write("<html><head></head><body>" + sb.ToString() + "</body></html>");
            stream.Flush();
            stream.Close();

        }
    }
}
