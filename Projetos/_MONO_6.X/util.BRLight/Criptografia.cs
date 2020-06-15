using System.Text;
using System.Security.Cryptography;
using System;

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

		static string key = "A!9HHhi%XjjYY4YP2@Nob009X";

		public static byte[] GetHash(string message)
		{
			byte[] data;
			data = System.Text.UTF8Encoding.ASCII.GetBytes(message);
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			return md5.ComputeHash(data, 0, data.Length);


		}

		public static bool VerifyHash(string message, byte[] hash)
		{
			byte[] data;
			data = System.Text.UTF8Encoding.ASCII.GetBytes(message);
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] hashtemp = md5.ComputeHash(data, 0, data.Length);

			for (int x = 0; x < hash.Length; x++)
			{
				if (hash[x] != hashtemp[x])
				{
					return false;
				}
			}
			return true;
		}

		public static string Encrypt(string text)
		{
			using (var md5 = new MD5CryptoServiceProvider())
			{
				using (var tdes = new TripleDESCryptoServiceProvider())
				{
					tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
					tdes.Mode = CipherMode.ECB;
					tdes.Padding = PaddingMode.PKCS7;

					using (var transform = tdes.CreateEncryptor())
					{
						byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
						byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
						return Convert.ToBase64String(bytes, 0, bytes.Length);
					}
				}
			}
		}

		public static string Decrypt(string cipher)
		{
			using (var md5 = new MD5CryptoServiceProvider())
			{
				using (var tdes = new TripleDESCryptoServiceProvider())
				{
					tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
					tdes.Mode = CipherMode.ECB;
					tdes.Padding = PaddingMode.PKCS7;

					using (var transform = tdes.CreateDecryptor())
					{
						byte[] cipherBytes = Convert.FromBase64String(cipher);
						byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
						return UTF8Encoding.UTF8.GetString(bytes);
					}
				}
			}
		}
	}
}