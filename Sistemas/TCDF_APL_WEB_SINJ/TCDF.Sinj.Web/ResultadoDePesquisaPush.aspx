<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="ResultadoDePesquisaPush.aspx.cs" Inherits="TCDF.Sinj.Web.ResultadoDePesquisaPush" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_auditoria.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        var push_checked_all = false;
        $(document).ready(function () {
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/NotifiquemeDatatable.ashx' + window.location.search,
                aoColumns: _columns_push_auditoria
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Resultado de Pesquisa de Usuários Push</label>
	</div>
    <div id="div_controls" class="control">
        <div class="div-light-button">
            <a href="./PesquisarPush.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Usuários Push</a>
        </div>
        <div class="div-light-button">
            <a href="./PesquisarNotificacao.aspx"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png">Notificações</a>
        </div>
    </div>
    <div class="control" style="height: 30px; position: relative;">
        <div class="div-light-button" style="right: 0px; position: absolute;">
            <a href="javascript:void(0);" onclick="javascript:preencherEmail();"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_email_p.png" />Enviar E-mail</a>
        </div>
        <br />
    </div>
    <div id="div_notificacao" class="notify" style="display:none; width: 90%; margin: auto;"></div>
    <div id="div_resultado" class="w-90-pc mauto">
    </div>
    <div id="modal_email" style="display:none;">
        <form id="form_email" name="formEmail" action="#" method="post">
            <div id="div_notificacao_email" class="notify" style="display:none;"></div>
            <div id="div_email">
                <div class="mauto table w-100-pc">
                    <div class="line">
                        <div class="column w-20-pc">
                            <div class="cell fr">
                                <label>Assunto:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div class="cell w-100-pc">
                                <input label="Assunto" obrigatorio="sim" id="assunto" name="assunto" type="text" value="" class="w-90-pc" />
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-20-pc">
                            <div class="cell fr">
                                <label>Mensagem:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div class="cell w-100-pc">
                                <textarea label="Mensagem" obrigatorio="sim" id="mensagem" name="mensagem" class="w-90-pc" rows="15" cols="50"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </div>
</asp:Content>
