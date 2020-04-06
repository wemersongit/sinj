using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using TCDF.Sinj.ES;
using System.Web;

namespace TCDF.Sinj.RN
{
    public class HistoricoDePesquisaRN
    {
        private DocEs _docEs;
        private HistoricoDePesquisaAD _historicoDePesquisaAd;
        private string _nm_base;

        public HistoricoDePesquisaRN()
        {
            _historicoDePesquisaAd = new HistoricoDePesquisaAD();
            _docEs = new DocEs();
            _nm_base = util.BRLight.Util.GetVariavel("NmBaseLogsSearch", true);
        }

        public void MontarPesquisa(SentencaPesquisaGeralOV pesquisaGeral, HistoricoDePesquisaOV pesquisa)
        {
            pesquisa.nm_tipo_pesquisa = "Pesquisa Geral";
            pesquisa.ds_historico = "(Pesquisa Geral) " + pesquisaGeral.all;
            var consulta = "tipo_pesquisa=geral&all=";
            if (!string.IsNullOrEmpty(pesquisaGeral.all))
            {
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "all", operador = "igual", valor = pesquisaGeral.all });
                consulta += pesquisaGeral.all;
            }
            pesquisa.consulta = consulta;
        }

        public void MontarPesquisa(SentencaPesquisaDiretaNormaOV pesquisaDireta, HistoricoDePesquisaOV historicoDePesquisa)
        {
            var consulta = "tipo_pesquisa=norma";
            historicoDePesquisa.ds_historico = "";
            if (!string.IsNullOrEmpty(pesquisaDireta.all))
            {
                historicoDePesquisa.ds_historico += (historicoDePesquisa.ds_historico != "" ? " E " : "") + "Qualquer Campo=" + pesquisaDireta.all;
                historicoDePesquisa.argumentos.Add(new ArgumentoOV { campo = "Qualquer Campo", operador = "igual", valor = pesquisaDireta.all, conector = (historicoDePesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&all=" + pesquisaDireta.all;
            }
            if (!string.IsNullOrEmpty(pesquisaDireta.nm_tipo_norma))
            {
                historicoDePesquisa.ds_historico += "Tipo=" + pesquisaDireta.nm_tipo_norma;
                historicoDePesquisa.argumentos.Add(new ArgumentoOV { campo = "Tipo", operador = "igual", valor = pesquisaDireta.nm_tipo_norma, conector = (historicoDePesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&nm_tipo_norma=" + pesquisaDireta.nm_tipo_norma;
                consulta += "&ch_tipo_norma=" + pesquisaDireta.ch_tipo_norma;
            }
            if (!string.IsNullOrEmpty(pesquisaDireta.st_habilita_pesquisa))
            {
                historicoDePesquisa.ds_historico += (historicoDePesquisa.ds_historico != "" ? " E " : "") + "Número=" + pesquisaDireta.st_habilita_pesquisa;
                historicoDePesquisa.argumentos.Add(new ArgumentoOV { campo = "Pesquisa", operador = "igual", valor = pesquisaDireta.nr_norma, conector = (historicoDePesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&st_habilita_pesquisa=" + pesquisaDireta.st_habilita_pesquisa;
            }
            if (!string.IsNullOrEmpty(pesquisaDireta.nr_norma))
            {
                historicoDePesquisa.ds_historico += (historicoDePesquisa.ds_historico != "" ? " E " : "") + "Número=" + pesquisaDireta.nr_norma;
                historicoDePesquisa.argumentos.Add(new ArgumentoOV { campo = "Número", operador = "igual", valor = pesquisaDireta.nr_norma, conector = (historicoDePesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&nr_norma=" + pesquisaDireta.nr_norma;
            }
            if (!string.IsNullOrEmpty(pesquisaDireta.norma_sem_numero) && pesquisaDireta.norma_sem_numero.Equals("true"))
            {
                historicoDePesquisa.ds_historico += (historicoDePesquisa.ds_historico != "" ? " E " : "") + "Número=Sem Número";
                consulta += "&norma_sem_numero=" + pesquisaDireta.norma_sem_numero;
            }
            if (!string.IsNullOrEmpty(pesquisaDireta.ano_assinatura))
            {
                historicoDePesquisa.ds_historico += (historicoDePesquisa.ds_historico != "" ? " E " : "") + "Ano=" + pesquisaDireta.ano_assinatura;
                historicoDePesquisa.argumentos.Add(new ArgumentoOV { campo = "Ano", operador = "igual", valor = pesquisaDireta.ano_assinatura, conector = (historicoDePesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&ano_assinatura=" + pesquisaDireta.ano_assinatura;
            }
            if (pesquisaDireta.nm_termos != null && pesquisaDireta.nm_termos.Length > 0)
            {
                for (var i = 0; i < pesquisaDireta.nm_termos.Length; i++)
                {
                    historicoDePesquisa.ds_historico += (historicoDePesquisa.ds_historico != "" ? " E " : "") + "Assunto=" + pesquisaDireta.nm_termos[i];
                    historicoDePesquisa.argumentos.Add(new ArgumentoOV { campo = "Assunto", operador = "igual", valor = pesquisaDireta.nm_termos[i], conector = (historicoDePesquisa.ds_historico != "" ? " E " : "") });
                    consulta += "&nm_termo=" + pesquisaDireta.nm_termos[i];
                    consulta += "&ch_termo=" + pesquisaDireta.ch_termos[i];
                }
            }
            if (!string.IsNullOrEmpty(pesquisaDireta.nm_orgao))
            {
                if (string.IsNullOrEmpty(pesquisaDireta.origem_por))
                {
                    pesquisaDireta.origem_por = "toda_a_hierarquia_em_qualquer_epoca1";
                }
                else
                {
                    consulta += "&origem_po2r=" + pesquisaDireta.origem_por;
                }
                historicoDePesquisa.ds_historico += (historicoDePesquisa.ds_historico != "" ? " E " : "") + "Origem=" + pesquisaDireta.nm_orgao + " E " + pesquisaDireta.origem_por.Replace("_", " ");
                historicoDePesquisa.argumentos.Add(new ArgumentoOV { campo = "Origem", operador = "igual", valor = pesquisaDireta.nm_orgao + " E " + pesquisaDireta.origem_por.Replace("_", " "), conector = (historicoDePesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&ch_orgao=" + pesquisaDireta.ch_orgao;
                consulta += "&ch_hierarquia=" + pesquisaDireta.ch_hierarquia;
                consulta += "&sg_hierarquia_nm_vigencia=" + pesquisaDireta.nm_orgao;
                //consulta += "&st_habilita_pesquisa=" + pesquisaDireta.st_habilita_pesquisa;
            }

            historicoDePesquisa.consulta = consulta;
            historicoDePesquisa.ds_historico = "(Pesquisa de Normas) " + historicoDePesquisa.ds_historico;
            historicoDePesquisa.nm_tipo_pesquisa = "Pesquisa de Normas";
        }
        
        public void MontarPesquisa(SentencaPesquisaDiretaDiarioOV pesquisaDireta, HistoricoDePesquisaOV pesquisa)
        {
            var consulta = "tipo_pesquisa=diario";
            pesquisa.ds_historico = "";
            if (!string.IsNullOrEmpty(pesquisaDireta.ds_norma))
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Norma=" + pesquisaDireta.ds_norma;
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Norma", operador = "igual", valor = pesquisaDireta.ds_norma, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&ds_norma=" + pesquisaDireta.ds_norma;
            }
            if (!string.IsNullOrEmpty(pesquisaDireta.nm_tipo_fonte))
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Tipo=" + pesquisaDireta.nm_tipo_fonte;
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Tipo", operador = "igual", valor = pesquisaDireta.nm_tipo_fonte, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&nm_tipo_fonte=" + pesquisaDireta.nm_tipo_fonte;
                consulta += "&ch_tipo_fonte=" + pesquisaDireta.ch_tipo_fonte;
            }
            if (!string.IsNullOrEmpty(pesquisaDireta.nr_diario))
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Número=" + pesquisaDireta.nr_diario;
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Número", operador = "igual", valor = pesquisaDireta.nr_diario, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&nr_diario=" + pesquisaDireta.nr_diario;
            }
            if (pesquisaDireta.secao_diario != null && pesquisaDireta.secao_diario.Length > 0)
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Seção=" + string.Join(",",pesquisaDireta.secao_diario);
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Seção", operador = "igual", valor = string.Join(",", pesquisaDireta.secao_diario), conector = (pesquisa.ds_historico != "" ? " E " : "") });
                for (var i = 0; i < pesquisaDireta.secao_diario.Length; i++ )
                {
                    consulta += "&secao_diario=" + pesquisaDireta.secao_diario[i];
                }
            }
            if (!string.IsNullOrEmpty(pesquisaDireta.filetext))
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Texto=" + pesquisaDireta.filetext;
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Texto", operador = "igual", valor = pesquisaDireta.filetext, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&filetext=" + pesquisaDireta.filetext;
            }
            if (pesquisaDireta.dt_assinatura != null && pesquisaDireta.dt_assinatura.Length > 0 && !string.IsNullOrEmpty(pesquisaDireta.dt_assinatura[0]))
            {
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Data de Publicação", operador = pesquisaDireta.op_dt_assinatura, valor = pesquisaDireta.dt_assinatura[0] + (pesquisaDireta.dt_assinatura.Length == 2 ? " " + pesquisaDireta.dt_assinatura[1] : ""), conector = (pesquisa.ds_historico != "" ? " E " : "") });
                if (pesquisaDireta.op_dt_assinatura == "intervalo")
                {
                    if (pesquisaDireta.dt_assinatura.Length == 2)
                    {
                        pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação >= " + pesquisaDireta.dt_assinatura[0] + " E Data de Publicação <= " + pesquisaDireta.dt_assinatura[1];
                        consulta += "&dt_assinatura=" + pesquisaDireta.dt_assinatura[0];
                        consulta += "&dt_assinatura=" + pesquisaDireta.dt_assinatura[1];
                    }
                }
                else if (pesquisaDireta.op_dt_assinatura == "menor")
                {
                    pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação < " + pesquisaDireta.dt_assinatura[0];
                    consulta += "&dt_assinatura=" + pesquisaDireta.dt_assinatura[0];
                }
                else if (pesquisaDireta.op_dt_assinatura == "menorouigual")
                {
                    pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação <= " + pesquisaDireta.dt_assinatura[0];
                    consulta += "&dt_assinatura=" + pesquisaDireta.dt_assinatura[0];
                }
                else if (pesquisaDireta.op_dt_assinatura == "maior")
                {
                    pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação > " + pesquisaDireta.dt_assinatura[0];
                    consulta += "&dt_assinatura=" + pesquisaDireta.dt_assinatura[0];
                }
                else if (pesquisaDireta.op_dt_assinatura == "maiorouigual")
                {
                    pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação >= " + pesquisaDireta.dt_assinatura[0];
                    consulta += "&dt_assinatura=" + pesquisaDireta.dt_assinatura[0];
                }
                else if (pesquisaDireta.op_dt_assinatura == "diferente")
                {
                    pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação != " + pesquisaDireta.dt_assinatura[0];
                    consulta += "&dt_assinatura=" + pesquisaDireta.dt_assinatura[0];
                }
                else
                {
                    pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação = " + pesquisaDireta.dt_assinatura[0];
                    consulta += "&dt_assinatura=" + pesquisaDireta.dt_assinatura[0];
                }
                consulta += "&op_dt_assinatura=" + pesquisaDireta.op_dt_assinatura;
            }
            pesquisa.consulta = consulta;
            pesquisa.ds_historico = "(Pesquisa de Diário) " + pesquisa.ds_historico;
            pesquisa.nm_tipo_pesquisa = "Pesquisa de Diário";
        }

        public void MontarPesquisa(SentencaPesquisaNotifiquemeDiarioOV pesquisaDireta, HistoricoDePesquisaOV pesquisa)
        {
            var consulta = "tipo_pesquisa=notifiqueme";
            pesquisa.ds_historico = "";
            if (!string.IsNullOrEmpty(pesquisaDireta.nm_tipo_fonte))
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Tipo=" + pesquisaDireta.nm_tipo_fonte;
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Tipo", operador = "igual", valor = pesquisaDireta.nm_tipo_fonte, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&nm_tipo_fonte=" + pesquisaDireta.nm_tipo_fonte;
                consulta += "&ch_tipo_fonte=" + pesquisaDireta.ch_tipo_fonte;
            }
            if (!string.IsNullOrEmpty(pesquisaDireta.filetext))
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Texto=" + pesquisaDireta.filetext;
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Texto", operador = "igual", valor = pesquisaDireta.filetext, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&filetext=" + pesquisaDireta.filetext;
            }
            if (!string.IsNullOrEmpty(pesquisaDireta.in_exata))
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Busca=" + (pesquisaDireta.in_exata.Equals("1") ? "Exata" : "Aproximada");
                consulta += "&in_exata=" + pesquisaDireta.in_exata;
            }
            pesquisa.consulta = consulta;
            pesquisa.ds_historico = "(Pesquisa de Diário - Notifique-me) " + pesquisa.ds_historico;
            pesquisa.nm_tipo_pesquisa = "Pesquisa de Diário - Notifique-me";
        }

        public void MontarPesquisa(SentencaPesquisaDiretorioDiarioOV pesquisaDiretorio, HistoricoDePesquisaOV pesquisa)
        {
            pesquisa.ds_historico = "";
            var consulta = "tipo_pesquisa=diretorio_diario";
            if (!string.IsNullOrEmpty(pesquisaDiretorio.nm_tipo_fonte))
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Tipo=" + pesquisaDiretorio.nm_tipo_fonte;
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Tipo", operador = "igual", valor = pesquisaDiretorio.nm_tipo_fonte, conector = "" });
                consulta += "&ch_tipo_fonte=" + pesquisaDiretorio.ch_tipo_fonte;
                consulta += "&nm_tipo_fonte=" + pesquisaDiretorio.nm_tipo_fonte;
            }
            if (!string.IsNullOrEmpty(pesquisaDiretorio.ano))
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Ano=" + pesquisaDiretorio.ano;
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Ano", operador = "igual", valor = pesquisaDiretorio.ano, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&ano=" + pesquisaDiretorio.ano;
            }
            if (!string.IsNullOrEmpty(pesquisaDiretorio.mes))
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Mês=" + pesquisaDiretorio.mes;
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Mês", operador = "igual", valor = pesquisaDiretorio.mes, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&mes=" + pesquisaDiretorio.mes;
            }
            pesquisa.consulta = consulta;
            pesquisa.ds_historico = "(Pesquisa de Diário - Diretório) " + pesquisa.ds_historico;
            pesquisa.nm_tipo_pesquisa = "Pesquisa de Diário - Diretório";   
        }

        public void MontarPesquisa(SentencaPesquisaTextoDiarioOV pesquisaTexto, HistoricoDePesquisaOV pesquisa)
        {
            pesquisa.ds_historico = "";
            var consulta = "tipo_pesquisa=texto_diario";
            //tipo_pesquisa=texto_diario&filetext=lightbase&ch_tipo_fonte=1&nm_tipo_fonte=DODF&dt_assinatura_inicio=&intervalo=1&dt_assinatura_termino=
            if (!string.IsNullOrEmpty(pesquisaTexto.nm_tipo_fonte))
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Tipo=" + pesquisaTexto.nm_tipo_fonte;
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Tipo", operador = "igual", valor = pesquisaTexto.nm_tipo_fonte, conector = "" });
                consulta += "&nm_tipo_fonte=" + pesquisaTexto.nm_tipo_fonte;
                consulta += "&ch_tipo_fonte=" + pesquisaTexto.ch_tipo_fonte;
            }
            if (!string.IsNullOrEmpty(pesquisaTexto.filetext))
            {
                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Texto=" + pesquisaTexto.filetext;
                pesquisa.argumentos.Add(new ArgumentoOV { campo = "Texto", operador = "igual", valor = pesquisaTexto.filetext, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                consulta += "&filetext=" + pesquisaTexto.filetext;
            }
            if (!string.IsNullOrEmpty(pesquisaTexto.dt_assinatura_inicio))
            {
                if (pesquisaTexto.intervalo == "1")
                {
                    pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação >= " + pesquisaTexto.dt_assinatura_inicio + " E Data de Publicação <= " + pesquisaTexto.dt_assinatura_termino;
                    pesquisa.argumentos.Add(new ArgumentoOV { campo = "Data de Publicação", operador = "intervalo", valor = pesquisaTexto.dt_assinatura_inicio + (!string.IsNullOrEmpty(pesquisaTexto.dt_assinatura_termino) ? (" até " + pesquisaTexto.dt_assinatura_termino) : ""), conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    consulta += "&intervalo=1";
                    consulta += "&dt_assinatura_inicio=" + pesquisaTexto.dt_assinatura_inicio;
                    consulta += "&dt_assinatura_termino=" + pesquisaTexto.dt_assinatura_termino;
                }
                else
                {
                    pesquisa.argumentos.Add(new ArgumentoOV { campo = "Data de Publicação", operador = "igual", valor = pesquisaTexto.dt_assinatura_inicio, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação = " + pesquisaTexto.dt_assinatura_inicio;
                    consulta += "&dt_assinatura_inicio=" + pesquisaTexto.dt_assinatura_inicio;
                }
            }
            pesquisa.consulta = consulta;
            pesquisa.ds_historico = "(Pesquisa de Diário - Texto) " + pesquisa.ds_historico;
            pesquisa.nm_tipo_pesquisa = "Pesquisa de Diário - Texto";
        }

        public void MontarPesquisa(SentencaPesquisaAvancadaNormaOV pesquisaAvancada, HistoricoDePesquisaOV pesquisa)
        {
            var consulta = "tipo_pesquisa=avancada";
            if (pesquisaAvancada.ch_tipo_norma != null)
            {
                foreach (var ch in pesquisaAvancada.ch_tipo_norma)
                {
                    consulta += "&ch_tipo_norma="+ch;
                    pesquisa.argumentos.Add(new ArgumentoOV { campo = "ch_tipo_norma", operador = "igual a", valor = ch, conector = "E" });
                }
            }
            if (pesquisaAvancada.argumentos != null)
            {
                var argumento_split = new string[0];
                var _nm_campo = "";
                var _nm_operador = "";
                var _nm_valor = "";
                var _conector = "";
                pesquisa.ds_historico = "";
                var i = 1;
                foreach (var argumento in pesquisaAvancada.argumentos)
                {
                    consulta += "&argumento=" + argumento;
                    argumento_split = argumento.Split('#');

                    if (argumento_split.Length == 8)
                    {
                        _nm_campo = argumento_split[2];
                        _nm_operador = argumento_split[4];
                        _nm_valor = argumento_split[6];
                        _conector = argumento_split[7];
                        pesquisa.ds_historico += _nm_campo + " " + _nm_operador + " " + _nm_valor + (i < pesquisaAvancada.argumentos.Length ? " " + _conector + " " : "");
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = _nm_campo, operador = _nm_operador, valor = _nm_valor, conector = (i < pesquisaAvancada.argumentos.Length ? _conector : "") });
                        i++;

                    }
                }
            }
            pesquisa.consulta = consulta;
            pesquisa.ds_historico = "(Pesquisa Avançada) " + pesquisa.ds_historico;
            pesquisa.nm_tipo_pesquisa = "Pesquisa Avançada";
        }

        public Results<HistoricoDePesquisaOV> Consultar(Pesquisa query)
        {
            return _historicoDePesquisaAd.Consultar(query);
        }

        public void Incluir(HistoricoDePesquisaOV historicoDePesquisaOv)
        {
            historicoDePesquisaOv.ch_consulta = historicoDePesquisaOv.ch_usuario + "_" + historicoDePesquisaOv.ds_historico;
            Pesquisa query = new Pesquisa();
            query.literal = "ch_consulta='" + historicoDePesquisaOv.ch_consulta + "'";
            query.select = new string[] { "id_doc", "contador" };
            var results = Consultar(query);
            if (results.results.Count > 0)
            {
                historicoDePesquisaOv.contador = results.results[0].contador + 1;
                Atualizar(results.results[0]._metadata.id_doc, historicoDePesquisaOv);
            }
            else
            {
                historicoDePesquisaOv.contador++;
                _historicoDePesquisaAd.Incluir(historicoDePesquisaOv);
            }
        }

        public bool Atualizar(ulong id_doc, HistoricoDePesquisaOV historicoDePesquisaOv)
        {
            return _historicoDePesquisaAd.Atualizar(id_doc, historicoDePesquisaOv);
        }

        public Result<HistoricoDePesquisaOV> ConsultarEs(HttpContext context)
        {
            return _historicoDePesquisaAd.ConsultarEs(context);
        }

    }
}
