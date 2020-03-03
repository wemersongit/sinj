using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.IO;
using util.BRLight;

namespace TCDF.Sinj.Portal.Web
{
    public partial class captcha : System.Web.UI.Page
    {
        protected string sCriptoKey = "";
        protected override void OnLoad(EventArgs e)
        {
			try{
	            Bitmap objBMP = new System.Drawing.Bitmap(74, 30);

	            Graphics objGraphics = System.Drawing.Graphics.FromImage(objBMP);

	            objGraphics.Clear(Color.Peru);

	            objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

	            // Fonte configurada para ser usada no texto do captcha

	            Font objFont = new Font("Times New Roman", 16, FontStyle.Strikeout);
	            
	            //Cria o valor randomicamente e adiciona ao array

	            string captchaValue = util.BRLight.ManipulaStrings.RandomString(4);

	           

	            //Adiciona o valor gerado para o captcha na sessão

	            //para ser validado posteriormente

	            //Session.Add("CaptchaValue", captchaValue);

	            //Desenha a imagem com o nosso texto

	            objGraphics.DrawString(captchaValue, objFont, Brushes.White, 3, 3);


	            //Salva em stream
	            using (MemoryStream ms = new MemoryStream())
	            {
					objBMP.Save(ms, ImageFormat.Png);
	                byte[] byteImage = ms.ToArray();
	                imgCaptcha.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(byteImage);
	            }

	            sCriptoKey = Criptografia.CalcularHashMD5(captchaValue, true);
			}
			catch(Exception ex){
				Response.Write (ex.Message);
			}
        }
    }
}
