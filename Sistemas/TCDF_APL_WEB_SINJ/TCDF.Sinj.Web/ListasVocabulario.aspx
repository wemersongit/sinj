<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="ListasVocabulario.aspx.cs" Inherits="TCDF.Sinj.Web.ListasVocabulario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vocabulario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            ExibirListas();
        });
    </script>
    <style type="text/css">
        #div_listas ul {margin-left:10px; list-style: disc outside none;}
        #div_listas ul li {margin-left:10px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Listas Auxiliares</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <div id="div_vocabulario" class="loaded">
            <fieldset class="w-80-pc">
                <legend id="label_tipo_termo">Listas</legend>
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-100-pc">
                            <div class="cell">
                                <div id="div_listas"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <div id="div_notificacao_vocabulario" class="notify" style="display:none;"></div>
        <div id="div_loading_vocabulario" class="loading" style="display:none;"></div>
    </div>
</asp:Content>
