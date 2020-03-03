using System.Text;
using System.Security.Cryptography;

namespace util.BRLight
{

    public static class Criptografia
    {

        public static string CalcularHashMD5(string texto) {
            return CalcularHashMD5(texto, false);
        }

        public static string CalcularHashMD5(string texto, bool utilizaHash)
        {
            var md5 = MD5.Create();
            if (utilizaHash)
                texto = string.Concat(texto.ToUpper(), "103465878902#zxXvbnmP@dZghjQweRtyuWZA0");

            byte[] inputBytes = Encoding.Default.GetBytes(texto);
            byte[] hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}