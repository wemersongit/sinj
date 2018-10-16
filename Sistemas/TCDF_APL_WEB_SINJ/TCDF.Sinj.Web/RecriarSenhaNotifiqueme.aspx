<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="RecriarSenhaNotifiqueme.aspx.cs" Inherits="TCDF.Sinj.Web.RecriarSenhaNotifiqueme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Captcha/css/epicaptcha.css?<%= TCDF.Sinj.Util.MostrarVersao() %>" />
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/login_notifiqueme.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Captcha/js/epicaptcha.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_resetar').click(function () {
                document.getElementById("form_recriar_senha").reset();
                location.reload();
            });
            $('#button_salvar_senha').click(function () {
                var sucesso = function (data) {
                    gComplete();
                    if (IsNotNullOrEmpty(data)) {
                        if (data.error_message != null && data.error_message != "") {
                            $('#form_nova_senha .notify').messagelight({
                                sTitle: "Erro",
                                sContent: data.error_message,
                                sType: "error",
                                sWidth: "",
                                iTime: null,
                                fnClose: function () {
                                    if(IsNotNullOrEmpty(data.DocNotFoundException) && data.DocNotFoundException){
                                        document.location.href = "./RecriarSenhaNotifiqueme.aspx";
                                    }
                                }
                            });
                        }
                        else if (IsNotNullOrEmpty(data, 'id_doc_success')) {
                            $('#form_nova_senha .notify').modallight({
                                sTitle: "Sucesso",
                                sContent: "Senha alterada com sucesso.",
                                sType: "success",
                                oButtons: [{ text: "Ok", click: function (){$(this).dialog('close');}}],
                                fnClose: function () {
                                    document.location.href = "./Notifiqueme.aspx";
                                }
                            });
                        }
                        else {
                            $('#form_nova_senha .notify').messagelight({
                                sTitle: "Erro",
                                sContent: "Erro ao alterar senha.",
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                        }
                    }
                };
                return fnSalvar('form_nova_senha','SAL', sucesso);
            });
            var sucesso_captcha = function (data) {
                gComplete();
                if (IsNotNullOrEmpty(data)) {
                    if (data.error_message != null && data.error_message != "") {
                        $('#form_recriar_senha .notify').modallight({
                            sTitle: "Erro",
                            sContent: data.error_message,
                            sType: "error",
                            oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                            fnClose: function () {
                                $(this).dialog("destroy");
                            }
                        });
                    }
                    else if (IsNotNullOrEmpty(data, 'id_doc_success')) {
                        $('#form_recriar_senha .notify').modallight({
                            sTitle: "Sucesso",
                            sContent: "Um link para recriação da senha foi enviado para seu e-mail.",
                            sType: "success",
                            oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                            fnClose: function () {
                                document.location.href = "./LoginNotifiqueme.aspx";
                            }
                        });
                    }
                }
            }
            $("#captcha").Epicaptcha({
                buttonID: "button_confirmar",
                theFormID: "form_recriar_senha",
                submitUrl: "./ashx/Push/NotifiquemeEnviarRecriarSenha.ashx",
                fnSuccess: sucesso_captcha
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Notifique-me</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <div id="div_form_recriar_senha" runat="server">
            <form id="form_recriar_senha" name="formRecriarSenha" method="post" action="#">
                <div id="div_recriar_senha">
                    <fieldset class="w-60-pc">
                        <legend>Recriar Senha</legend>
                        <div class="mauto table">
                            <div class="line">
                                <div id="div_info_recriar_senha" runat="server" class="column w-100-pc" style="text-align:center;">
                                    Informe seu email para confirmação.
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-30-pc">
                                    <div class="cell fr">
                                        <label>E-mail:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell w-60-pc">
                                        <input label="E-mail" obrigatorio="sim" id="email_usuario_push" name="email_usuario_push" type="text" value="" class="w-90-pc" />
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-30-pc">
                                    <div class="cell fr">
                                        <label>Validação de Segurança:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell" style="margin-left:25px;">
                                        <div id="captcha" class="mauto word-no-break text-center"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="width:220px; margin:auto;" class="loaded">
                            <input type="button" id="button_confirmar" value="Confirmar" />
                            <input type="button" id="button_resetar" value="Limpar" />
                        </div>
                    </fieldset>
                </div>
                <div class="notify" style="display:none;"></div>
                <div class="loading" style="display:none;"></div>
            </form>
        </div>
        <div id="div_form_nova_senha" runat="server">
            <form id="form_nova_senha" name="formNovaSenha" method="post" action="#">
                <div id="div_nova_senha" class="loaded">
                    <fieldset class="w-60-pc">
                        <legend>Recriar Senha</legend>
                        <div class="mauto table">
                            <div class="line">
                                <div class="column w-30-pc">
                                    <div class="cell fr">
                                        <label>Nova senha:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell w-60-pc">
                                        <input id="senha_usuario_push" name="senha_usuario_push" type="password" value="" class="w-30-pc" />
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-30-pc">
                                    <div class="cell fr">
                                        <label>Redigite a senha:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="cell w-60-pc">
                                        <input id="senha_usuario_push_confirme" name="senha_usuario_push" type="password" value="" class="w-30-pc" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="width:220px; margin:auto;" class="loaded">
                            <button id="button_salvar_senha">
                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Salvar
                            </button>
                            <button type="reset">
                                <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" />Limpar
                            </button>
                        </div>
                    </fieldset>
                </div>
                <div class="notify" style="display:none;"></div>
                <div class="loading" style="display:none;"></div>
            </form>
        </div>
    </div>
</asp:Content>
