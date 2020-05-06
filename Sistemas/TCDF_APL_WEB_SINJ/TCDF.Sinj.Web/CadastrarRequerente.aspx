<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="CadastrarRequerente.aspx.cs" Inherits="TCDF.Sinj.Web.CadastrarRequerente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_requerente.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_requerente').click(function () {
                return fnSalvar("form_requerente");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Cadastrar Requerente</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_requerente" name="formCadastroSituacao" action="#" method="post">
            <div id="div_requerente">
            <div id="div_notificacao_requerente" class="notify" style="display:none;"></div>
                <fieldset class="w-60-pc">
                    <!--<legend>Requerente</legend>-->
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
                        <button type="reset">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" />Limpar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_requerente" class="loading" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
