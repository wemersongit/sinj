<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaixarArquivoNorma.aspx.cs" Inherits="TCDF.Sinj.Web.BaixarArquivoNorma" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta name="robots" content="noindex,nofollow" />
    <meta name="googlebot" content="noindex,nofollow" />
    <meta http-equiv="Content-Language" content="pt-br" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>Arquivo da Norma</title>
    <asp:PlaceHolder runat="server" id="placeHolderHeader"></asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/platform.js"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/jquery.printElement.js"></script>

    <script type="text/javascript" language="javascript">
        function Print() {
            $('#div_texto').printElement();
        }
        $(document).ready(function () {
            if ($('#div_erro').length > 0) {
                $('#a_print').hide();
            }

            if ($('#div_texto').length == 1 && window.location.href.indexOf('#') > -1) {
                console.log("entrou no if 1");
                var i = window.location.href.split('#');
                var cap;
                console.log(i);
                if (typeof i[1] != undefined) {
                    console.log("entrou no if 2");
                    if (i[1] != "") {
                        console.log("entrou em outro if");
                        var disAfetado = $(`#${i[1]}`);
                        if (disAfetado.length > 1 && disAfetado.is(':visible')) {
                            cap = i[1];
                            console.log(cap);
                        } else {
                            var divToCap;
                            divToCap = i[1].split('_');
                            cap = divToCap[0];
                            console.log(cap);
                        }
                    }
                }

                $('html, body').animate({
                    scrollTop: $(`#${cap}`).offset().top
                }, 600);
            }
        });
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
