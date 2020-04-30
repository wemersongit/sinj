using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using util.BRLight;
using System.Net;

namespace TCDF.Sinj
{
    public class ValidaCaptcha
    {
        public void ValidarCaptchaGoogle(string gRecaptchaResponse)
        {
            
            if (string.IsNullOrEmpty(gRecaptchaResponse))
            {
                throw new DocValidacaoException("Não é um robô? Então clique na caixa 'Não sou um robô' para verificarmos.");
            }
            var secretKeyRecaptcha = util.BRLight.Util.GetVariavel("secretKeyRecaptcha");
            var dic = new Dictionary<string, object>();
            dic.Add("secret", secretKeyRecaptcha);
            dic.Add("response", gRecaptchaResponse);

            ServicePointManager.ServerCertificateValidationCallback = new CertificateValidation().MyRemoteCertificateValidationCallback;
            var response = new REST("https://www.google.com/recaptcha/api/siteverify", HttpVerb.POST, dic).GetResponse();
            
            var dicResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
            if (!dicResponse.ContainsKey("success") || !bool.Parse(dicResponse["success"].ToString()))
            {
                throw new DocValidacaoException("Não é um robô? Então clique na caixa 'Não sou um robô' para verificarmos.");
            }
        }

        public void ValidarCaptcha(string dsCaptcha, string encriptedCaptcha)
        {
            if (string.IsNullOrEmpty(dsCaptcha) || !encriptedCaptcha.Equals(Criptografia.CalcularHashMD5(dsCaptcha.ToUpper(), true)))
            {
                throw new DocValidacaoException("Os caracteres não correspondem com os da imagem.");
            }
        }
    }
}
