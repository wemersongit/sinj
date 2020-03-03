using System;
using System.Configuration;

namespace util.BRLight
{
    public static class Config {

        public static string ValorChave(string sChave, bool verificarAmbiente = false)
        {
            var configuracao = new AppSettingsReader();
            try
            {
                const string valor = "";
                return verificarAmbiente
                           ? (string) configuracao.GetValue(ValorChave("Ambiente") + "." + sChave, valor.GetType())
                           : (string) configuracao.GetValue(sChave, valor.GetType());
            }
            catch (Exception) {
                return "-1";
            }
        }

    }
}

