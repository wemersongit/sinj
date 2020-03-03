using System;

namespace util.BRLight
{
    public class ValorString : Attribute
    {
        public string Valor { get; private set; }
        public string Id { get; private set; }

        public ValorString(string valor)
        {
            Valor = valor;
        }

        public ValorString(string valor, string id)
        {
            Valor = valor;
            Id = id;
        }

        /// <summary>
        /// Pega o valor string do Enum
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string PegarValorString(Enum valor)
        {
            string output = null;
            Type type = valor.GetType();

            var fi = type.GetField(valor.ToString());
            var attrs = fi.GetCustomAttributes(typeof(ValorString), false) as ValorString[];
            if (attrs != null && attrs.Length > 0)
                output = attrs[0].Valor;

            return output;
        }

    }
}
