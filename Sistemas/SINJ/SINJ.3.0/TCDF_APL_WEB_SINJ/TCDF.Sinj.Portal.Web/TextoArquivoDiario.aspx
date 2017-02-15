<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TextoArquivoDiario.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.TextoArquivoDiario" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Texto do Arquivo da Diário</title>
    <asp:PlaceHolder runat="server" id="placeHolderHeader"></asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/platform.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.printElement.js"></script>

    <script type="text/javascript" language="javascript">
        function Print() {
            $('#div_texto').printElement();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align:right;margin-right:50px;">
            <a id="a_print" runat="server" href="javascript:void(0);" onclick="javascript:Print();" title="Imprimir"><img alt="print" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_print_p.png"/></a>
        </div>
        <div id="div_texto" runat="server"></div>
    </form>
</body>
</html>
