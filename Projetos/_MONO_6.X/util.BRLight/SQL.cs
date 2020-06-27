using System;

namespace util.BRLight
{
    public static class SQL
    {
        static SQL()
        {
        }

        public static string getOperadorDeDataLB(string op,bool isAny)
        {
            switch (op)
            {
                case "Igual":
                    op = "=";
                    break;

                case "Maior_que":
                    op = (!isAny ? ">":"<");
                    break;

                case "Maior_ou_igual":
                    op = (!isAny ? ">=" : "<=");
                    break;

                case "Menor_que":
                    op = (!isAny ? "<":">");
                    break;

                case "Menor_ou_igual":
                    op = (!isAny ? "<=":">=");
                    break;

                case "Intervalo":
                    op = "><";
                    break;

                case "Diferente":
                    op = "!=";
                    break;
                default:
                    op = "=";
                    break;
            }
            return op;
        }
        public static string Translate(string sCampo, string sTexto)
        {
            return string.Format("translate(Upper({0}), 'áàâãäåaaaÁÂÃÄÅAAAÀéèêeeeeeEEEÉEEÈìíîïìiiiÌÍÎÏÌIIIóôõöoooòÒÓÔÕÖOOOùúûüuuuuÙÚÛÜUUUUçÇñÑýÝ', 'aaaaaaaaaAAAAAAAAAeeeeeeeeEEEEEEEiiiiiiiiIIIIIIIIooooooooOOOOOOOOuuuuuuuuUUUUUUUUcCnNyY') like translate('%{1}%', 'áàâãäåaaaÁÂÃÄÅAAAÀéèêëeeeeeEEEÉEEÈìíîïìiiiÌÍÎÏÌIIIóôõöoooòÒÓÔÕÖOOOùúûüuuuuÙÚÛÜUUUUçÇñÑýÝ', 'aaaaaaaaaAAAAAAAAAeeeeeeeeeEEEEEEEiiiiiiiiIIIIIIIIooooooooOOOOOOOOuuuuuuuuUUUUUUUUcCnNyY')", sCampo, Util.RemoveSpecialCharacters(sTexto).ToUpper().Replace("'", "''"));
        }

        public static string Data(string _dt_inicio, string _dt_fim, string _nm_campo_base, string _operador)
        {
            var literal = "";
            var operador = getOperadorDeDataLB(_operador,false);
            if (operador == "><" && !string.IsNullOrEmpty(_dt_fim))
            {
                literal += (literal != "" ? " and " : "") + string.Format("{0} >= '{1}' AND {0} <= '{2}'", _nm_campo_base, _dt_inicio, _dt_fim);
            } else {
                literal += (literal != "" ? " and " : "") + string.Format("{0} {1} '{2}'", _nm_campo_base, operador, _dt_inicio);
            }

            return literal;
        }

        public static string DataMultiValor(string _dt_inicio, string _dt_fim, string _nm_campo_base, string _operador)
        {
            var literal = "";
            var operador = getOperadorDeDataLB(_operador,true);
            if (operador == "><" && !string.IsNullOrEmpty(_dt_fim))
            {
                literal += (literal != "" ? " and " : "") + string.Format("'{1}' <= any ({0}) AND '{2}' >= any ({0})", _nm_campo_base, _dt_inicio, _dt_fim);
            }
            else
            {
                literal += (literal != "" ? " and " : "") + string.Format("'{2}' {1} any ({0})", _nm_campo_base, operador, _dt_inicio);
            }

            return literal;
        }

    }
}
