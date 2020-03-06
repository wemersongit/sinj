<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="EditarSituacao.aspx.cs" Inherits="TCDF.Sinj.Web.EditarSituacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_situacao.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_situacao').click(function () {
                return fnSalvar("form_situacao");
            });
            PreencherSituacaoEdicao();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Editar Situação</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <form id="form_situacao" name="formEdicaoSituacao" action="#" method="post">
            <input id="id_doc" name="id_doc" type="hidden" value="" class="w-90-pc" />
            <div id="div_situacao">
                <fieldset class="w-60-pc">
                    <div id="div_notificacao_situacao" class="notify" style="display:none;"></div>
                    <!--<legend>Situação</legend>-->
                    <div class="mauto table">
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Nome da Situação:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Nome" obrigatorio="sim" id="nm_situacao" name="nm_situacao" type="text" value="" class="w-90-pc" />
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
                                    <textarea label="Descrição" id="ds_situacao" name="ds_situacao" class="w-90-pc" cols="100" rows="5" ></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Peso:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Peso" obrigatorio="sim" id="nr_peso_situacao" name="nr_peso_situacao" type="text" value="" class="w-30-pc" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div_buttons_situacao" style="width:220px; margin:auto;" class="loaded">
                        <button id="button_salvar_situacao">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                        </button>
                        <button type="button" onclick="javascript:window.history.back();">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Cancelar
                        </button>
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_situacao" class="loading" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
