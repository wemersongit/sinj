using System;
using System.Configuration;

namespace BRLight.Util
{
    public static class Configuracao {

        public static string ValorChave(string sChave)
        {
            var configuracao = new AppSettingsReader();
            try
            {
                const string valor = "";
                return (string) configuracao.GetValue(sChave, valor.GetType());
            }
            catch (Exception) {
                return "-1";
            }
        }

    }
}
