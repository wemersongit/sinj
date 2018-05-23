<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="captcha.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.captcha" %>
<!DOCTYPE html>
<html>
<body>
<input type="hidden" id="k" name="k" value="<%= sCriptoKey %>" />
<asp:Image runat="server" id="imgCaptcha" style="vertical-align:middle" /> <input label="Caracteres da imagem" obrigatorio="sim" id="ds_captcha" name="ds_captcha" type="text" value="" style="width:74px" /> <button type="button" class="clean" onclick="generateCaptcha()"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_restore_p.png" /></button>
</body>
</html>
