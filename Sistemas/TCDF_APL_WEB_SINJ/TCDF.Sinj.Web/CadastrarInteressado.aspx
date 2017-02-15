<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="CadastrarInteressado.aspx.cs" Inherits="TCDF.Sinj.Web.CadastrarInteressado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_interessado.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_interessado').click(function () {
                return fnSalvar("form_interessado");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Cadastrar Interessado</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_interessado" name="formCadastroInteressado" action="#" method="post">
            <div id="div_interessado">
            <div id="div_notificacao_interessado" class="notify" style="display:none;"></div>
                <fieldset class="w-60-pc">
                    <legend>Interessado</legend>
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Nome do Interessado:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Nome" obrigatorio="sim" id="nm_interessado" name="nm_interessado" type="text" value="" class="w-90-pc" />
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
                                    <input label="Descrição" obrigatorio="sim" id="ds_interessado" name="ds_interessado" type="text" value="" class="w-90-pc" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div_buttons_interessado" style="width:220px; margin:auto;" class="loaded">
                        <button id="button_salvar_interessado">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_save_p.png" />Salvar
                        </button>
                        <button type="reset">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" />Limpar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_interessado" class="loading" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
