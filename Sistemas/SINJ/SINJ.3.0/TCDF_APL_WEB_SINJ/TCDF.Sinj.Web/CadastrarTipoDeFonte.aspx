<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="CadastrarTipoDeFonte.aspx.cs" Inherits="TCDF.Sinj.Web.CadastrarTipoDeFonte" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_tipo_de_fonte.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_tipo_de_fonte').click(function () {
                return fnSalvar("form_tipo_de_fonte");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Cadastrar Tipo de Fonte</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_tipo_de_fonte" name="formCadastroTipoDeFonte" action="#" method="post">
            <div id="div_tipo_de_fonte">
            <div id="div_notificacao_tipo_de_fonte" class="notify" style="display:none;"></div>
                <fieldset class="w-60-pc">
                    <legend>Tipo de Fonte</legend>
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Nome da Fonte:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Nome" obrigatorio="sim" id="nm_tipo_fonte" name="nm_tipo_fonte" type="text" value="" class="w-90-pc" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Descrição:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <textarea label="Descrição" id="ds_tipo_fonte" name="ds_tipo_fonte" class="w-90-pc" cols="100" rows="5" ></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div_buttons_tipo_de_fonte" style="width:220px; margin:auto;" class="loaded">
                        <button id="button_salvar_tipo_de_fonte">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                        </button>
                        <button type="reset">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" />Limpar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_tipo_de_fonte" class="loading" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
