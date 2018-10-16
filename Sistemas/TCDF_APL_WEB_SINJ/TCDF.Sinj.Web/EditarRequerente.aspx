<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="EditarRequerente.aspx.cs" Inherits="TCDF.Sinj.Web.EditarRequerente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_requerente.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_requerente').click(function () {
                return fnSalvar("form_requerente");
            });
            PreencherRequerenteEdicao();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Editar Requerente</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_requerente" name="formEdicaoRequerente" action="#" method="post">
            <input id="id_doc" name="id_doc" type="hidden" value="" class="w-90-pc" />
            <div id="div_requerente">
                <fieldset class="w-60-pc">
                    <!--<legend>Requerente</legend>-->
                    <div id="div_notificacao_requerente" class="notify" style="display:none;"></div>
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Nome do Requerente:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Nome" obrigatorio="sim" id="nm_requerente" name="nm_requerente" type="text" value="" class="w-90-pc" />
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
                                    <textarea label="Descrição" id="ds_requerente" name="ds_requerente" class="w-90-pc" cols="100" rows="5" ></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div_buttons_requerente" style="width:220px; margin:auto;" class="loaded">
                        <button id="button_salvar_requerente">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                        </button>
                        <button type="button" onclick="javascript:window.history.back();">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_requerente" class="loading" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
