using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;

namespace TCDF.Sinj
{
    public class LB
    {
        public Results<T> PesquisarDocs<T>(HttpContext context, SessaoUsuarioOV sessao_usuario_ov, string _bbusca)
        {
            Results<T> results = new Results<T>();
            Pesquisa pesquisa = new Pesquisa();
            try
            {
                var _iDisplayLength = context.Request["iDisplayLength"];
                var _iDisplayStart = context.Request["iDisplayStart"];
                pesquisa.literal = MontarConsulta(context, sessao_usuario_ov, _bbusca);
                if (_iDisplayLength != "-1")
                {
                    pesquisa.limit = _iDisplayLength;
                    pesquisa.offset = _iDisplayStart;
                }
                pesquisa.order_by = MontarOrdenamento(context);
                if (_bbusca == "sinj_norma")
                {
                    results = new NormaRN().Consultar(pesquisa) as Results<T>;
                }
                else if (_bbusca == "sinj_diario")
                {
                    results = new DiarioRN().Consultar(pesquisa) as Results<T>;
                }
                return results;
            }
            catch (Exception)
            {
                throw new Exception("Erro ao pesquisar Docs. Pesquisa: " + JSON.Serialize<Pesquisa>(pesquisa));
            }
        }
        public List<T> RelatorioDocs<T>(HttpContext context, SessaoUsuarioOV sessao_usuario_ov, string _bbusca)
        {
            List<T> resultados = new List<T>();
            Pesquisa pesquisa = new Pesquisa();
            try
            {
                ulong limit_max = 1000;
                var slimit = Config.ValorChave("LimiteRelatorio");
                if (slimit != "-1")
                {
                    ulong.TryParse(slimit, out limit_max);
                }
                ulong from = 0;
                ulong size = 200;
                ulong total = 0;
                pesquisa.literal = MontarConsulta(context, sessao_usuario_ov, _bbusca);
                pesquisa.order_by = MontarOrdenamento(context);
                var result = new Results<T>();
                while (from <= total && from < limit_max)
                {
                    pesquisa.offset = from.ToString();
                    pesquisa.limit = size.ToString();
                    if (_bbusca == "sinj_norma")
                    {
                        result = new NormaRN().Consultar(pesquisa) as Results<T>;
                    }
                    else if (_bbusca == "sinj_diario")
                    {
                        result = new DiarioRN().Consultar(pesquisa) as Results<T>;
                    }
                    total = result.result_count;
                    from += size;
                    resultados.AddRange(result.results);
                }
                return resultados;
            }
            catch (Exception)
            {
                throw new Exception("Erro ao pesquisar Docs. Pesquisa: " + JSON.Serialize<Pesquisa>(pesquisa));
            }
        }

        private string MontarConsulta(HttpContext context, SessaoUsuarioOV sessao_usuario_ov, string _bbusca)
        {
            string query = "";
            if (_bbusca == "sinj_norma")
            {
                var _ch_tipo_norma = context.Request["ch_tipo_norma"];
                var _ch_termo = context.Request["ch_termo"];
                var _nr_norma = context.Request["nr_norma"];
                var _nr_ano = context.Request["nr_ano"];
                int nr_ano = 0;
                var _ch_orgao = context.Request["ch_orgao"];

                var _dt_cadastro = context.Request.Params.GetValues("dt_cadastro");
                var _dt_alteracao = context.Request.Params.GetValues("dt_alteracao");
                var _dt_assinatura = context.Request["dt_assinatura"];
                var _nm_login_usuario_cadastro = context.Request["nm_login_usuario_cadastro"];
                var _nm_login_usuario_alteracao = context.Request["nm_login_usuario_alteracao"];
                var _cadastrado_por_inativo = context.Request["cadastrado_por_inativo"];
                var _alterado_por_inativo = context.Request["alterado_por_inativo"];
                var _st_pendencia = context.Request["st_pendencia"];

                var _st_habilita_pesquisa = context.Request["st_habilita_pesquisa"];

                var _orgao_cadastrador = context.Request.Params.GetValues("orgao_cadastrador");

                var _sSearch = context.Request["sSearch"];
                
                query = "";

                if (!string.IsNullOrEmpty(_ch_tipo_norma))
                {
                    query += (query != "" ? " and " : "") + "ch_tipo_norma='" + _ch_tipo_norma + "'";
                }
                if (!string.IsNullOrEmpty(_nr_norma))
                {
                    query += (query != "" ? " and " : "") + string.Format("TRANSLATE(LTRIM(nr_norma, '0'), '.-, ', '')=TRANSLATE(LTRIM('{0}', '0'), '.-, ', '')", _nr_norma);
                }
                if (!string.IsNullOrEmpty(_nr_ano) && int.TryParse(_nr_ano, out nr_ano))
                {
                    query += (query != "" ? " and " : "") + "CAST(dt_assinatura AS DATE)>='01/01/" + nr_ano + "' and CAST(dt_assinatura AS DATE)<='31/12/" + nr_ano + "'";
                }
                if (!string.IsNullOrEmpty(_ch_orgao))
                {
                    query += (query != "" ? " and " : "") + "'" + _ch_orgao + "'=any(ch_orgao)";
                }
                if (!string.IsNullOrEmpty(_ch_termo))
                {
                    query += (query != "" ? " and " : "") + "'" + _ch_termo + "'=any(ch_termo)";
                }
                if (!string.IsNullOrEmpty(_dt_assinatura))
                {
                    query += (query != "" ? " and " : "") + "CAST(dt_assinatura AS DATE)='" + _dt_assinatura + "'";
                }
                if (_dt_cadastro != null && _dt_cadastro.Length == 2)
                {
                    if (!string.IsNullOrEmpty(_dt_cadastro[0]))
                    {
                        query += (query != "" ? " and " : "") + "CAST(dt_cadastro AS DATE)>='" + _dt_cadastro[0] + "'";
                    }
                    if (!string.IsNullOrEmpty(_dt_cadastro[1]))
                    {
                        query += (query != "" ? " and " : "") + "CAST(dt_cadastro AS DATE)<='" + _dt_cadastro[1] + "'";
                    }
                }
                if (_dt_alteracao != null && _dt_alteracao.Length == 2)
                {
                    if (!string.IsNullOrEmpty(_dt_alteracao[0]) && !string.IsNullOrEmpty(_dt_alteracao[1]))
                    {
                        query += (query != "" ? " and " : "") + "id_doc=ANY(select id_doc from (select id_doc, unnest(dt_alteracao::date[]) dt) x where dt>='" + _dt_alteracao[0] + "' and dt<='" + _dt_alteracao[1] + "')";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(_dt_alteracao[0]))
                        {
                            query += (query != "" ? " and " : "") + "'" + _dt_alteracao[0] + "'<=any(dt_alteracao::date[])";
                        }
                        if (!string.IsNullOrEmpty(_dt_alteracao[1]))
                        {
                            query += (query != "" ? " and " : "") + "'" + _dt_alteracao[1] + "'>=any(dt_alteracao::date[])";
                        }
                    }
                }
                if (_orgao_cadastrador != null && _orgao_cadastrador.Length > 0)
                {
                    var query_orgao_cadastrador = "";
                    for (var i = 0; i < _orgao_cadastrador.Length; i++)
                    {
                        query_orgao_cadastrador += (query_orgao_cadastrador != "" ? " or " : "") + "id_orgao_cadastrador='" + _orgao_cadastrador[i] + "'";
                    }
                    query += (query != "" ? " and " : "") + "(" + query_orgao_cadastrador + ")";
                }
                if (!string.IsNullOrEmpty(_nm_login_usuario_cadastro))
                {
                    query += (query != "" ? " and " : "") + "(nm_login_usuario_cadastro='" + _nm_login_usuario_cadastro + "')";
                }
                if (!string.IsNullOrEmpty(_nm_login_usuario_alteracao))
                {
                    query += (query != "" ? " and " : "") + "'" + _nm_login_usuario_alteracao + "'=any(nm_login_usuario_alteracao)";
                }
                // Verifica se nm_login_usuario_cadastro está contido no resultado da busca que traz todos usuários inativos
                if (!string.IsNullOrEmpty(_cadastrado_por_inativo))
                {
                    query += (query != "" ? " and " : "") + "(nm_login_usuario_cadastro in (select nm_login_usuario from lb_doc_sinj_usuario where st_usuario=false))";
                }
                // Busca todos os usuarios inativos (st_usuario=false) e converte em um array
                // Usa o operador && que verifica a intersecção entre dois arrays (o de nm_login_usuario_alteracao) e o retornado na outra busca
                if (!string.IsNullOrEmpty(_alterado_por_inativo))
                {
                    query += (query != "" ? " and " : "") + "nm_login_usuario_alteracao && (select array(select nm_login_usuario from lb_doc_sinj_usuario where st_usuario=false)::varchar[])";
                }
                if (!string.IsNullOrEmpty(_st_pendencia))
                {
                    query += (query != "" ? " and " : "") + "st_pendencia=true";
                }

                if (!string.IsNullOrEmpty(_st_habilita_pesquisa))
                {
                    query += (query != "" ? " and " : "") + "st_habilita_pesquisa=true";
                }


                if (!string.IsNullOrEmpty(_sSearch))
                {
                    var sSearch_split = _sSearch.Split(' ');
                    for (var i = 0; i < sSearch_split.Length; i++)
                    {
                        query += (query != "" ? " and " : "") + "UPPER(document::text) like '%" + sSearch_split[i].ToUpper() + "%'";
                    }
                }
            }
            else if (_bbusca == "sinj_diario")
            {
                var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                var _nr_diario = context.Request["nr_diario"];
                var _cr_diario = context.Request["cr_diario"];
                var _secao_diario = context.Request.Form.GetValues("secao_diario");
                var _dt_assinatura = context.Request["dt_assinatura"];
                var _dt_assinatura_intervalo = context.Request["dt_assinatura_intervalo"];
                var _st_pendente = context.Request["st_pendente"];
                var st_pendente = false;
                var _op_dt_assinatura = context.Request["op_dt_assinatura"];

                var _sSearch = context.Request.Params["sSearch"];

                if (!string.IsNullOrEmpty(_ch_tipo_fonte))
                {
                    query += (query != "" ? " and " : "") + "ch_tipo_fonte='" + _ch_tipo_fonte + "'";
                }
                if (!string.IsNullOrEmpty(_st_pendente) && bool.TryParse(_st_pendente, out st_pendente))
                {
                    query += (query != "" ? " and " : "") + "st_pendente=" + _st_pendente;
                }
                if (!string.IsNullOrEmpty(_nr_diario))
                {
                    query += (query != "" ? " and " : "") + "nr_diario=" + _nr_diario + "";
                }
                if (!string.IsNullOrEmpty(_cr_diario))
                {
                    query += (query != "" ? " and " : "") + "cr_diario='" + _cr_diario + "'";
                }
                if (_secao_diario != null && _secao_diario.Length > 0)
                {
                    var sSecao = "";
                    for (var i = 0; i < _secao_diario.Length;i++ )
                    {
                        sSecao += (sSecao != "" ? (i < (_secao_diario.Length - 1) ? ", " : " e ") : "") + _secao_diario[i];
                    }
                    query += (query != "" ? " and " : "") + "secao_diario='" + sSecao + "'";
                }


                if (_op_dt_assinatura == "intervalo")
                {
                    query += (query != "" ? " and " : "") + "CAST(dt_assinatura AS DATE) >= '" + (_dt_assinatura) + "' and CAST(dt_assinatura AS DATE) <= '" + _dt_assinatura_intervalo + "'";
                }
                else
                {
                    if (!string.IsNullOrEmpty(_dt_assinatura))
                    {
                        query += (query != "" ? " and " : "") + "CAST(dt_assinatura AS DATE)" + ReplaceOperatorToQuery(_op_dt_assinatura) + "'" + _dt_assinatura + "'";
                    }
                }
            }
            return query;
        }

        private Order_By MontarOrdenamento(HttpContext context)
        {
            Order_By order_by = new Order_By();
            var iSortCol = 0;
            int.TryParse(context.Request["iSortCol_0"], out iSortCol);
            var iSortDir = context.Request["sSortDir_0"];
            var _sColOrder = context.Request["mDataProp_" + iSortCol];
            if (!string.IsNullOrEmpty(_sColOrder))
            {
                if (("desc" == iSortDir))
                {
                    if (_sColOrder == "nm_tipo_norma")
                    {
                        order_by.desc = new[] { _sColOrder, "nr_norma" };
                    }
                    else
                    {
                        order_by.desc = new[] { _sColOrder };
                    }
                }
                else
                {
                    if (_sColOrder == "nm_tipo_norma")
                    {
                        order_by.asc = new[] { _sColOrder, "nr_norma" };
                    }
                    else
                    {
                        order_by.asc = new[] { _sColOrder };
                    }
                }
            }
            return order_by;
        }

        public static string ReplaceOperatorToQuery(string nm_operator)
        {
            var operador = "";
            switch (nm_operator)
            {
                case "menor":
                    operador = "<";
                    break;
                case "menorouigual":
                    operador = "<=";
                    break;
                case "maior":
                    operador = ">";
                    break;
                case "maiorouigual":
                    operador = ">=";
                    break;
                case "diferente":
                    operador = "<>";
                    break;
                default:
                    operador = "=";
                    break;
            }
            return operador;
        }
    }
}
