<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="PesquisarVocabulario.aspx.cs" Inherits="TCDF.Sinj.Web.PesquisarVocabulario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vocabulario.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            ConstruirControlesDinamicos();
            var letra = GetParameterValue("letra");
            var ch_termo = GetParameterValue("ch_termo");
            var nm_termo = GetParameterValue("nm_termo");
            if (IsNotNullOrEmpty(letra) || IsNotNullOrEmpty(ch_termo) || IsNotNullOrEmpty(nm_termo)) {
                PesquisarVocabulario();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Pesquisar Vocabulário</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <br />
    <form id="form_vocabulario" name="formPesquisaVocabulario" action="./PesquisarVocabulario.aspx" method="get">
        <input id="input_letra" type="hidden" name="letra" value="" />
	    <div class="div-alfabeto">
	        <div>
	            <ul id="ul-alfabeto">
	            </ul>
	        </div>
	    </div>
        <br />
	    <div id="div_pesquisa" class="div-pesquisa">
	        <div>
	            <label>Pesquisar:</label><input id="input_ch_termo" name="ch_termo" type="hidden" value="" /><input id="input_nm_termo" name="nm_termo" type="text" value="" class="ui-autocomplete-input simularInputText" /><a id="a_pesquisar" title="Pesquisar" href="javascript:void(0)" onclick="javascript:PesquisarVocabularioClick();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png" /></a>
	        </div>
	    </div>
    </form>
    <div id="div_resultado" class="w-90-pc mauto" style="margin-top:20px;" >
        
	</div>
    <div id="div_modal_filtro" style="display:none;">
        <label><input type="checkbox" name="filtro_ch_tipo_termo" value="AU" />Autoridade</label>
        <label><input type="checkbox" name="filtro_ch_tipo_termo" value="DE" />Descritor</label>
        <label><input type="checkbox" name="filtro_ch_tipo_termo" value="ES" />Especificador</label>
        <label><input type="checkbox" name="filtro_ch_tipo_termo" value="LA" />Lista</label>
    </div>
</asp:Content>
