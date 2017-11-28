<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DesativarNormaPush.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.DesativarNormaPush" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
    <%
        if (_ok)
        {
    %>
        $(document).ready(function(){
            $('#imgOk').show();
        });
    <%            
        }
    %>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    
    <div class="divIdentificadorDePagina">
	    <label>.: Desativar Notificação</label>
	</div>
    <div class="form">
	    <div id="div_excluir" class="loaded">
		    <fieldset class="w-80-pc">
			    <div class="mauto table">
			        <div class="line">
			            <div class="column w-100-pc">
			                <div class="cell" style="padding:80px;">
                                <img id="imgOk" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_hand_ok.png" style="display:none; vertical-align:text-bottom;" />
                                <label runat="server" id="label_retorno" style="font-size:20px; margin-left: 10px;"></label>
			                </div>
			            </div>
			        </div>
			    </div>
		    </fieldset>
	    </div>
    </div>
</asp:Content>