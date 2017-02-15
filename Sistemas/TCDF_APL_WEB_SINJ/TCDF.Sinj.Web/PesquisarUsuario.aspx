<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarUsuario.aspx.cs" Inherits="TCDF.Sinj.Web.PesquisarUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_usuario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/UsuarioDatatable.ashx',
                aoColumns: _columns_usuario,
                bFilter: true
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Usuário</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div id="div_resultado" class="w-90-pc mauto">
                
    </div>
    <div id="div_modal_filtro_status" style="display:none;">
        <label><input type="checkbox" name="filtro_st_usuario" value="true" />Ativo</label>
        <label><input type="checkbox" name="filtro_st_usuario" value="false" />Inativo</label>
    </div>
</asp:Content>
