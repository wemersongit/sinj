<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Norma.aspx.cs" Inherits="TCDF.Sinj.Web.Norma" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="robots" content="noindex,nofollow" />
    <meta name="googlebot" content="noindex,nofollow" />
    <title>Arquivo da Norma</title>
    <asp:PlaceHolder runat="server" id="placeHolderHeader"></asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/platform.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.printElement.js"></script>

    <script type="text/javascript" language="javascript">
        function Print() {
            $('#div_texto').printElement();
        }
        function exibirAlteracoes(el) {
            if (el.getAttribute('refresh')) {
                location.reload();
            }
            else {
                $('p[replaced_by]').remove();
                $(el).attr('refresh', '1');
                $(el).text('Exibir Alterações');
            }
        }
        $(document).ready(function () {
            if ($('#div_erro').length > 0) {
                $('.control').hide();
            }
            if($('p[replaced_by]').length > 0){
                $('a.compilado').show();
            }
        });
    </script>
</head>
<body>
    <div style="margin-right:50px;" class="control">
        <a href="<%= ResolveUrl("~/") %>" title="Visitar o SINJ-DF - Sistema Integrado de Normas Jurídicas do DF"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/logo_sinj.png" alt="SINJ-DF" /></a>
        <a id="a_print" runat="server" href="javascript:void(0);" onclick="javascript:Print();" title="Imprimir" style="float:right;"><img alt="print" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_print_p.png"/></a>
    </div>
    <div class="control" style="padding:20px;">
    <a class="compilado" href="javascript:void(0)" onclick="javascript:exibirAlteracoes(this);" style="display:none;">Suprimir Alterações</a>
    </div>
    <div id="div_texto" runat="server"></div>
</body>
</html>