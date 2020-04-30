<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="RecriarSenhaNotifiqueme.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.RecriarSenhaNotifiqueme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/login_notifiqueme.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    
    <script src='https://www.google.com/recaptcha/api.js?onload=onLoadCaptchaCriarContaCallback' async defer></script>

    <script type="text/javascript" language="javascript">
        var onLoadCaptchaCriarContaCallback = function () {
            $('#div_captcha_recriar_senha').remove();
            $('#form_recriar_senha input[name=tpv]').val('g');
        }
        $(document).ready(function () {
            $('#button_resetar').click(function () {
                document.getElementById("form_recriar_senha").reset();
                location.reload();
            });
            generateCaptcha('div_captcha_recriar_senha');
        });
        function enviarRecriarSenhaNotifiqueme() {
            try {
                Validar("form_recriar_senha");
                var sucesso = function (data) {
                    gComplete();
                    if (IsNotNullOrEmpty(data, 'error_message')) {
                        notificar('#form_recriar_senha', data.error_message, 'error');
                    }
                    else if (IsNotNullOrEmpty(data, 'id_doc_success')) {
                        $('#form_recriar_senha .notify').modallight({
                            sTitle: "Sucesso",
                            sContent: "Um link para recriação da senha foi enviado para seu e-mail.",
                            sType: "success",
                            oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                            fnClose: function () {
                                document.location.href = "./LoginNotifiqueme";
                            }
                        });
                    }
                }
                $.ajaxlight({
                    sFormId: "form_recriar_senha",
                    sUrl: './ashx/Push/NotifiquemeEnviarRecriarSenha.ashx',
                    sType: "POST",
                    fnSuccess: sucesso,
                    fnBeforeSubmit: gInicio,
                    fnComplete: gComplete,
                    bAsync: true
                });

            } catch (ex) {
                notificar('#form_recriar_senha', ex, 'error');
                $("html, body").animate({
                    scrollTop: 0
                }, "slow");
            }
            return false;
        }

        function recriarSenhaNotifiqueme() {
            try {
                Validar("form_nova_senha");

                var sucesso = function (data) {
                    gComplete();
                    if ((IsNotNullOrEmpty(data, 'error_message'))) {
                        notificar('#form_nova_senha', data.error_message, 'error');
                    }
                    else if (IsNotNullOrEmpty(data, 'id_doc_success')) {
                        $('#form_nova_senha .notify').modallight({
                            sTitle: "Sucesso",
                            sContent: "Senha alterada com sucesso.",
                            sType: "success",
                            oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                            fnClose: function () {
                                document.location.href = "./Notifiqueme";
                            }
                        });
                    }
                    else {
                        notificar('#form_nova_senha', "Ocorreu um interno no servidor ao tentar alterar senha.", 'error');
                    }
                };
                $.ajaxlight({
                    sFormId: "form_nova_senha",
                    sUrl: './ashx/Push/NotifiquemeRecriarSenha.ashx',
                    sType: "POST",
                    fnSuccess: sucesso,
                    fnBeforeSubmit: gInicio,
                    fnComplete: gComplete,
                    bAsync: true
                });
            } catch (ex) {
                notificar('#form_nova_senha', ex, 'error');
                $("html, body").animate({
                    scrollTop: 0
                }, "slow");
            }
            return false;
        }
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
            <form id="form_recriar_senha" name="formRecriarSenha" method="post" action="#" onsubmit="return enviarRecriarSenhaNotifiqueme();">
                <input name="tpv" type="hidden" value="c" />
                <div id="div_recriar_senha">
                    <fieldset class="w-60-pc">
                        <legend>Recriar Senha</legend>
                        <div class="mauto table">
                            <div class="notify" style="display:none;"></div>
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
                            <div id="div_captcha_recriar_senha" class="line">
                                <div class="column w-30-pc" style="padding-top:15px;">
                                    <div class="cell fr">
                                        <label>*Digite os caracteres da imagem:</label>
                                    </div>
                                </div>
                                <div class="column w-70-pc">
                                    <div class="loading_capctha" style="display:none;"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/loading003300.gif" width="25" /></div>
                                    <div class="captcha"></div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-30-pc">
                                </div>
                                <div class="column w-70-pc">
                                    <div class="g-recaptcha" data-sitekey="6LfzvVwUAAAAAHh_0bcm-_RFp7Xmn0fsf2QKLhWX"></div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-100-pc text-center">
                                    <button>
                                        <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_check.png" /> Enviar
                                    </button>
                                    <button id="button_resetar" type="reset">
                                        <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_close.png" /> Cancelar
                                    </button>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </form>
        </div>
        <div id="div_form_nova_senha" runat="server">
            <form id="form_nova_senha" name="formNovaSenha" method="post" action="#"onsubmit="return recriarSenhaNotifiqueme();">
                <input type="hidden" name="recriar" value="<%= Request["recriar"] %>" />
                <div id="div_nova_senha" class="loaded">
                    <fieldset class="w-60-pc">
                        <legend>Recriar Senha</legend>
                        <div class="mauto table">
                            <div class="notify" style="display:none;"></div>
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
                            <div class="line">
                                <div class="column w-100-pc text-center">
                                    <button>
                                        <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_check.png" /> Salvar
                                    </button>
                                    <button type="reset">
                                        <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_close.png" /> Cancelar
                                    </button>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
