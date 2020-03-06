<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="CadastrarRelator.aspx.cs" Inherits="TCDF.Sinj.Web.CadastrarRelator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_relator.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_relator').click(function () {
                return fnSalvar("form_relator");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Editar Relator</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_relator" name="formEdicaoRelator" action="#" method="post">
            <div id="div_relator">
            <div id="div_notificacao_relator" class="notify" style="display:none;"></div>
                <fieldset class="w-60-pc">
                    <!--<legend>Relator</legend>-->
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Nome do Relator:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Nome" obrigatorio="sim" id="nm_relator" name="nm_relator" type="text" value="" class="w-90-pc" />
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
                                    <textarea label="Descrição" id="ds_relator" name="ds_relator" class="w-90-pc" cols="100" rows="5" ></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div_buttons_relator" style="width:220px; margin:auto;" class="loaded">
                        <button id="button_salvar_relator">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                        </button>
                        <button type="reset">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" />Limpar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_relator" class="loading" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
