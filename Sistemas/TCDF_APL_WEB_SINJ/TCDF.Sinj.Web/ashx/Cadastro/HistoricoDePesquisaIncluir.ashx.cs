using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for HistoricoDePesquisaIncluir
    /// </summary>
    public class HistoricoDePesquisaIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _chave = context.Request["chave"];
            var _tipo_pesquisa = context.Request["tipo_pesquisa"];
            var _consulta = context.Request.QueryString.GetValues("consulta");
            var _sTotais = context.Request.QueryString.GetValues("total");
            try
            {
                var consulta = string.Join("&", _consulta);
                consulta = HttpUtility.UrlEncode(consulta);

                var pesquisa = new HistoricoDePesquisaOV();
                pesquisa.ch_usuario = _chave;
                pesquisa.consulta = consulta;
                pesquisa.dt_historico = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                foreach (var sTotal in _sTotais)
                {
                    pesquisa.total.Add(JSON.Deserializa<TotalOV>(sTotal));
                }
                var _all = context.Request["all"];
                if (_tipo_pesquisa == "geral")
                {
                    pesquisa.nm_tipo_pesquisa = "Pesquisa Geral";
                    pesquisa.ds_historico = "(Pesquisa Geral) " + _all;
                    if (!string.IsNullOrEmpty(_all))
                    {
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "all", operador = "igual", valor = _all });
                    }
                }
                else if (_tipo_pesquisa == "norma")
                {
                    var _nm_tipo_norma = context.Request["nm_tipo_norma"];
                    var _nr_norma = context.Request["nr_norma"];
                    var _ano_assinatura = context.Request["ano_assinatura"];
                    var _nm_orgao = context.Request["sg_hierarquia_nm_vigencia"];
                    var _nm_termo = context.Request.Params.GetValues("nm_termo");
                    pesquisa.ds_historico = "";
                    if (!string.IsNullOrEmpty(_all))
                    {
                        pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Qualquer Campo=" + _all;
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "Qualquer Campo", operador = "igual", valor = _all, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    }
                    if (!string.IsNullOrEmpty(_nm_tipo_norma))
                    {
                        pesquisa.ds_historico += "Tipo=" + _nm_tipo_norma;
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "Tipo", operador = "igual", valor = _nm_tipo_norma, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    }
                    if (!string.IsNullOrEmpty(_nr_norma))
                    {
                        pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Número=" + _nr_norma;
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "Número", operador = "igual", valor = _nr_norma, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    }
                    if (!string.IsNullOrEmpty(_ano_assinatura))
                    {
                        pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Ano=" + _ano_assinatura;
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "Ano", operador = "igual", valor = _ano_assinatura, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    }
                    if (_nm_termo != null && _nm_termo.Length > 0)
                    {
                        for (var i = 0; i < _nm_termo.Length; i++)
                        {
                            pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Assunto=" + _nm_termo[i];
                            pesquisa.argumentos.Add(new ArgumentoOV { campo = "Assunto", operador = "igual", valor = _nm_termo[i], conector = (pesquisa.ds_historico != "" ? " E " : "") });
                        }
                    }
                    if (!string.IsNullOrEmpty(_nm_orgao))
                    {
                        var _origem_por = context.Request["origem_por"];
                        if (string.IsNullOrEmpty(_origem_por))
                        {
                            _origem_por = "toda_a_hierarquia_em_qualquer_epoca";
                        }
                        pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Origem=" + _nm_orgao + " E " + _origem_por.Replace("_", " ");
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "Origem", operador = "igual", valor = _nm_orgao + " E " + _origem_por.Replace("_", " "), conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    }
                    pesquisa.ds_historico = "(Pesquisa de Normas) " + pesquisa.ds_historico;
                    pesquisa.nm_tipo_pesquisa = "Pesquisa de Normas";
                }
                else if (_tipo_pesquisa == "diario")
                {
                    var _ds_norma = context.Request["ds_norma"];
                    var _nm_tipo_fonte = context.Request["nm_tipo_fonte"];
                    var _nr_diario = context.Request["nr_diario"];
                    var _secao_diario = context.Request["secao_diario"];
                    var _filetext = context.Request["filetext"];
                    var _op_dt_assinatura = context.Request["op_dt_assinatura"];
                    var _dt_assinatura = context.Request.Params.GetValues("dt_assinatura");
                    pesquisa.ds_historico = "";
                    if (!string.IsNullOrEmpty(_ds_norma))
                    {
                        pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Norma=" + _ds_norma;
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "Norma", operador = "igual", valor = _ds_norma, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    }
                    if (!string.IsNullOrEmpty(_all))
                    {
                        pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Qualquer Campo=" + _all;
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "Qualquer Campo", operador = "igual", valor = _all, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    }
                    if (!string.IsNullOrEmpty(_nm_tipo_fonte))
                    {
                        pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Tipo=" + _nm_tipo_fonte;
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "Tipo", operador = "igual", valor = _nm_tipo_fonte, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    }
                    if (!string.IsNullOrEmpty(_nr_diario))
                    {
                        pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Número=" + _nr_diario;
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "Número", operador = "igual", valor = _nr_diario, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    }
                    if (!string.IsNullOrEmpty(_secao_diario))
                    {
                        pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Seção=" + _secao_diario;
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "Seção", operador = "igual", valor = _secao_diario, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    }
                    if (!string.IsNullOrEmpty(_filetext))
                    {
                        pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Texto=" + _filetext;
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "Texto", operador = "igual", valor = _filetext, conector = (pesquisa.ds_historico != "" ? " E " : "") });
                    }
                    if (_dt_assinatura != null && _dt_assinatura.Length > 0 && !string.IsNullOrEmpty(_dt_assinatura[0]))
                    {
                        pesquisa.argumentos.Add(new ArgumentoOV { campo = "Data de Publicação", operador = _op_dt_assinatura, valor = _dt_assinatura[0] + (_dt_assinatura.Length == 2 ? " " + _dt_assinatura[1] : ""), conector = (pesquisa.ds_historico != "" ? " E " : "") });
                        if (_op_dt_assinatura == "intervalo")
                        {
                            if (_dt_assinatura.Length == 2)
                            {
                                pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação >= " + _dt_assinatura[0] + " E Data de Publicação <= " + _dt_assinatura[1];
                            }
                        }
                        else if (_op_dt_assinatura == "menor")
                        {
                            pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação < " + _dt_assinatura[0];

                        }
                        else if (_op_dt_assinatura == "menorouigual")
                        {
                            pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação <= " + _dt_assinatura[0];
                        }
                        else if (_op_dt_assinatura == "maior")
                        {
                            pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação > " + _dt_assinatura[0];
                        }
                        else if (_op_dt_assinatura == "maiorouigual")
                        {
                            pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação >= " + _dt_assinatura[0];
                        }
                        else if (_op_dt_assinatura == "diferente")
                        {
                            pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação != " + _dt_assinatura[0];
                        }
                        else
                        {
                            pesquisa.ds_historico += (pesquisa.ds_historico != "" ? " E " : "") + "Data de Publicação = " + _dt_assinatura[0];
                        }
                    }
                    pesquisa.ds_historico = "(Pesquisa de Diário) " + pesquisa.ds_historico;
                    pesquisa.nm_tipo_pesquisa = "Pesquisa de Diário";
                }
                else if (_tipo_pesquisa == "avancada")
                {
                    var _argumento = context.Request.Params.GetValues("argumento");
                    if (_argumento != null)
                    {
                        var argumento_split = new string[0];
                        var _nm_campo = "";
                        var _nm_operador = "";
                        var _nm_valor = "";
                        var _conector = "";
                        pesquisa.ds_historico = "";
                        var i = 1;
                        foreach (var argumento in _argumento)
                        {
                            argumento_split = argumento.Split('#');
                            if (argumento_split.Length == 8)
                            {
                                _nm_campo = argumento_split[2];
                                _nm_operador = argumento_split[4];
                                _nm_valor = argumento_split[6];
                                _conector = argumento_split[7];
                                pesquisa.ds_historico += _nm_campo + " " + _nm_operador + " " + _nm_valor + (i < _argumento.Length ? " " + _conector + " " : "");
                                pesquisa.argumentos.Add(new ArgumentoOV { campo = _nm_campo, operador = _nm_operador, valor = _nm_valor, conector = (i < _argumento.Length ? _conector : "") });
                                i++;
                            }
                        }
                    }
                    pesquisa.ds_historico = "(Pesquisa Avançada) " + pesquisa.ds_historico;
                    pesquisa.nm_tipo_pesquisa = "Pesquisa Avançada";
                }

                new RN.HistoricoDePesquisaRN().Incluir(pesquisa);
                
                sRetorno = "{\"success_message\":\"Histórico incluído com sucesso\", \"pesquisa\":"+JSON.Serialize<HistoricoDePesquisaOV>(pesquisa)+"}";
            }
            catch (Exception ex)
            {
                var sErro  = Excecao.LerTodasMensagensDaExcecao(ex, false);
                sRetorno = "{\"error_message\":\"" + sErro + "\"}";
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                LogErro.gravar_erro("HST.INC", erro, "", "");
            }
            context.Response.Write(sRetorno);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
