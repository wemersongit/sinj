using System;
using System.Linq;
using System.Xml.Linq;

namespace Exportador_LB_to_ES.util
{
    public class Configuracao
    {
        public static string LerValorChave(string chave)
        {
            return ValorChave(chave);
        }

        private static string ValorChave(string sChave)
        {
            return GetValueFromXml(sChave);
        }

        private static string GetValueFromXml(string chave)
        {
            XElement xml = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + @"config.xml");
            var element = from x in xml.Elements(chave) select x.Element("value");
            if(element.Count() > 0) return element.First().Value;
            return "";
        }
    }
}
