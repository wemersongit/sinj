using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using util.BRLight;

namespace TCDF.Sinj.ES
{
    public class DocEs
    {
        private AD _ad;

        public DocEs()
        {
            _ad = new AD();
        }
        public string IncluirDoc(string json_doc, string uri)
        {
            return _ad.IncluirDoc(json_doc, uri);
        }
        public string AtualizarDoc(string json_doc, string uri)
        {
            return _ad.AtualizarDoc(json_doc, uri);
        }
        public string DeletarDoc(string json_doc, string uri)
        {
            return _ad.DeletarDoc(json_doc, uri);
        }
        public Result<T> Pesquisar<T>(string query, string url_es)
        {
            return _ad.PesquisarDocs<T>(query, url_es);
        }

        public string CountEs(string query, string url_es)
        {
            return _ad.PesquisarTotal(query, url_es);
        }

        public string GetUrlEs(string nm_base)
        {
            var sProxy = Config.ValorChave("usar_proxy", true);
            if (sProxy == "true")
            {
                return Config.ValorChave("URLBaseREST", true) + "/" + nm_base + "/es";
            }
            return _ad.GetUrlEs(nm_base);
        }

        public string TratarCaracteresReservadosDoEs(string texto)
        {
            texto = texto.Trim(' ');
            var carateres_reservados = new string[] { "+", "-", "=", "&&", "||", ">", "<", "!", "{", "}", "[", "]", "?", "/", "\\" };
            for (var i = 0; i < carateres_reservados.Length; i++)
            {
                texto = texto.Replace(carateres_reservados[i], "\\" + carateres_reservados[i]);
            }
            return texto.Replace("\"", "\\\"");
        }

        public string TratarCaracteresReservadosDoEsPesquisaAvancada(string texto)
        {
            texto = texto.Trim(' ').Replace(" AND ", " ").Replace(" OR ", " ");
            var carateres_reservados = new string[] { "+", "-", "=", "&&", "||", ">", "<", "!", "{", "}", "[", "]", "?", "/", "\\" };
            for (var i = 0; i < carateres_reservados.Length; i++)
            {
                texto = texto.Replace(carateres_reservados[i], "\\" + carateres_reservados[i]);
            }
            return texto.Replace("\"", "\\\"");
        }

        public string MontarArgumentoRange(string _ch_campo, string _ch_operador, string _ch_valor)
        {
            if (_ch_valor != ",")
            {
                var ch_valor = _ch_valor.Split(',');
                if (_ch_operador == "intervalo")
                {
                    if (ch_valor.Length == 2)
                    {
                        return "{\"range\":{\"" + _ch_campo + "\":{ \"gte\":\"" + ch_valor[0] + "\",\"lte\":\"" + ch_valor[1] + "\"}}}";
                    }
                }
                else if (_ch_operador == "menor")
                {
                    return "{\"range\":{\"" + _ch_campo + "\":{\"lt\":\"" + ch_valor[0] + "\"}}}";

                }
                else if (_ch_operador == "menorouigual")
                {
                    return "{\"range\":{\"" + _ch_campo + "\":{\"lte\":\"" + ch_valor[0] + "\"}}}";

                }
                else if (_ch_operador == "maior")
                {
                    return "{\"range\":{\"" + _ch_campo + "\":{\"gt\":\"" + ch_valor[0] + "\"}}}";

                }
                else if (_ch_operador == "maiorouigual")
                {
                    return "{\"range\":{\"" + _ch_campo + "\":{\"gte\":\"" + ch_valor[0] + "\"}}}";

                }
                else if (_ch_operador == "diferente")
                {
                    return "{\"not\":{\"term\":{\"" + _ch_campo + "\":{\"value\":\"" + ch_valor[0] + "\"}}}}";
                }
                else
                {
                    return "{\"term\":{\"" + _ch_campo + "\":{\"value\":\"" + ch_valor[0] + "\"}}}";
                }
            }
            return "";
        }

        public string MontarArgumentoRangeQueryString(string _ch_campo, string _ch_operador, string _ch_valor)
        {
            if (_ch_valor != ",")
            {
                var ch_valor = _ch_valor.Split(',');
                if (_ch_operador == "intervalo")
                {
                    if (ch_valor.Length == 2)
                    {
                        return "(" + _ch_campo + ":[" + ch_valor[0] + " TO " + ch_valor[1] + "])";
                    }
                }
                else if (_ch_operador == "menor")
                {
                    return "(" + _ch_campo + ":{* TO " + ch_valor[0] + "})";

                }
                else if (_ch_operador == "menorouigual")
                {
                    return "(" + _ch_campo + ":[* TO " + ch_valor[0] + "])";

                }
                else if (_ch_operador == "maior")
                {
                    return "(" + _ch_campo + ":{" + ch_valor[0] + " TO *})";

                }
                else if (_ch_operador == "maiorouigual")
                {
                    return "(" + _ch_campo + ":[" + ch_valor[0] + " TO *])";

                }
                else if (_ch_operador == "diferente")
                {
                    return "(\"not\": " + _ch_campo + ":(\\\"" + ch_valor[0] + "\\\"))";
                }
                else
                {
                    return "(" + _ch_campo + ":(\\\"" + ch_valor[0] + "\\\"))";
                }
            }
            return "";
        }
    }
}
