<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DesativarNormaPush.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.DesativarNormaPush" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Desativar Notificação</label>
	</div>
    <div class="form">
	    <div id="div_excluir" class="loaded">
		    <fieldset class="w-80-pc">
			    <legend>Desativar Monitoração</legend>
			    <div class="mauto table">
			        <div class="line">
			            <div class="column w-100-pc">
			                <div class="cell" runat="server" id="div_retorno">
			                </div>
			            </div>
			        </div>
			    </div>
		    </fieldset>
	    </div>
    </div>
</asp:Content>