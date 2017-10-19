<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="EditarAutoria.aspx.cs" Inherits="TCDF.Sinj.Web.EditarAutoria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_autoria.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_autoria').click(function () {
                return fnSalvar("form_autoria");
            });
            PreencherAutoriaEdicao();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Editar Autorias</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_autoria" name="formEdicaoAutoria" action="#" method="post">
            <input id="id_doc" name="id_doc" type="hidden" value="" class="w-90-pc" />
            <div id="div_autoria">
                <fieldset class="w-60-pc">
                 <div id="div_notificacao_autoria" class="notify" style="display:none;"></div>
                    <legend>Autoria</legend>
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Nome da Autoria:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Nome" obrigatorio="sim" id="nm_autoria" name="nm_autoria" type="text" value="" class="w-90-pc" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div_buttons_autoria" style="width:220px; margin:auto;" class="loaded">
                        <button id="button_salvar_autoria">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                        </button>
                        <button type="button" onclick="javascript:window.history.back();">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_autoria" class="loading" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
