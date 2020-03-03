<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="Erro.aspx.cs" Inherits="TCDF.Sinj.Web.Erro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var cd = GetParameterValue("cd");
            var erro = MostrarErro(cd);
            $('#div_notificacao_erro').messagelight({
                sTitle: "Erro",
                sContent: IsNotNullOrEmpty(erro) ? erro : "Erro não identificado.",
                sType: "error",
                sWidth: "",
                iTime: null
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Erro</label>
	</div>
    <div id="div_notificacao_erro" style="display:none;">
    </div>
</asp:Content>
