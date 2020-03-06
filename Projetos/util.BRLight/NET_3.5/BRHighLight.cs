
using System.Collections.Generic;
using System.Web;

namespace util.BRLight
{
    public class BRHighLight
    {
        public static List<HighLight> CriarHighLight()
        {
            var _sParamsHighLight = HttpContext.Current.Request["paramsHighLight"];
            string[] aParamsHighLight = _sParamsHighLight.Split(',');
            List<HighLight> highLight = new List<HighLight>();
            foreach (var sParamsHighLight in aParamsHighLight)
            {
                var sValor = HttpContext.Current.Request[sParamsHighLight];
                if(!string.IsNullOrEmpty(sValor))
                {
                    highLight.Add(new HighLight { Campo = trocarNomeDoCampo(sParamsHighLight), Valor = new[]{sValor}});
                }
            }
            return highLight;
        }

        private static string trocarNomeDoCampo(string nomeCampo) {
            if (nomeCampo == "txtbuscalivre") {
                return "document";
            }
            return nomeCampo;
        }
    }

    public class HighLight {
        public string Campo { get; set; }
        public IList<string> Valor { get; set; }

    }



}
