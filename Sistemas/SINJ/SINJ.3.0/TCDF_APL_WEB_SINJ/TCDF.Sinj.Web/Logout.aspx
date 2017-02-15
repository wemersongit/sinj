<%@ Page Title="" Language="C#" MasterPageFile="~/MasterLogin.Master" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="TCDF.Sinj.Web.Logout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        ShowMessage("PanelNotificacao", " Sistema sendo finalizado com segurança!!!", "alert");
        var sucesso = function (data) {
            if (data.excluido) {
                ShowMessage("PanelNotificacao", "Sistema finalizado com sucesso!!!", "success");
                top.document.location.href = _urlApps + _urlPgLogin + "?exit=3";
                return;
            } else {
                if (data.error_message != null) {
                    ShowMessage("PanelNotificacao", " Erro: " + data.error_message, "error");
                } else {
                    ShowMessage("PanelNotificacao", " Erro ao finalizar sistema: Não foi possível determinar erro!!!", "error");
                }
            }
        };

        Ajax('./ashx/Del/SessaoEnd.ashx', "Post", null, sucesso, null, null);
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
</asp:Content>
