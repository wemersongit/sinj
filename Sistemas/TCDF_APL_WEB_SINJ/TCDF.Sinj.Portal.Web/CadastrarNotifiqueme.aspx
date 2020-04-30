<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="CadastrarNotifiqueme.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.CadastrarNotifiqueme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Captcha/css/epicaptcha.css?<%= TCDF.Sinj.Util.MostrarVersao() %>" />
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Captcha/js/epicaptcha.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    
    <script src='https://www.google.com/recaptcha/api.js?onload=onLoadCaptchaCriarContaCallback' async defer></script>

    <script type="text/javascript" language="javascript">
        var onLoadCaptchaCriarContaCallback = function () {
            $('#div_captcha_criar_conta').remove();
            $('#form_notifiqueme input[name=tpv]').val('g');
        }
        function criarContaNotifiqueme() {
            var redirecionar_notifiqueme = GetParameterValue("redirecionar_notifiqueme");
            var p = GetParameterValue("p");
            var ch_norma = '';

            if (IsNotNullOrEmpty(redirecionar_notifiqueme)) {
                redirecionar_notifiqueme = redirecionar_notifiqueme.replace(/__EQUAL__/g, "=");
                redirecionar_notifiqueme = redirecionar_notifiqueme.replace(/__AND__/g, "&");
                redirecionar_notifiqueme = redirecionar_notifiqueme.replace(/__QUERY__/g, "?");
                var array_ch_norma = redirecionar_notifiqueme.split("&");
                ch_norma = array_ch_norma[array_ch_norma.length - 1];
            }
            else if (IsNotNullOrEmpty(p)) {
            redirecionar_notifiqueme = './' + p;
            }
            else {
                redirecionar_notifiqueme = './Notifiqueme'
            }
            try {
                Validar("form_notifiqueme");
                var sucesso = function (data) {
                    if (IsNotNullOrEmpty(data)) {
                        if (data.error_message != null && data.error_message != "") {
                            notificar('#form_notifiqueme', data.error_message, 'error');
                        }
                        else if (IsNotNullOrEmpty(data, 'id_doc_success')) {
                            if (IsNotNullOrEmpty(ch_norma)) {
                                $.ajaxlight({
                                    sUrl: "./ashx/Push/NotifiquemeNormaIncluir.ashx?" + ch_norma,
                                    sType: "GET",
                                    fnError: null,
                                    iTimeout: 40000
                                });
                            }
                            
                            top.document.location.href = redirecionar_notifiqueme;
                        }
                    }
                }
                $.ajaxlight({
                    sFormId: "form_notifiqueme",
                    sUrl: './ashx/Push/NotifiquemeIncluir.ashx',
                    sType: "POST",
                    fnSuccess: sucesso,
                    fnBeforeSubmit: gInicio,
                    fnComplete: gComplete,
                    bAsync: true
                });

            } catch (ex) {
                notificar('#form_notifiqueme', ex, 'error');
                $("html, body").animate({
                    scrollTop: 0
                }, "slow");
            }
            return false;
        }
        $(document).ready(function () {
            
            generateCaptcha('div_captcha_criar_conta');
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
        <form id="form_notifiqueme" name="formCadastroNotifiqueme" post="post" action="#" onsubmit="return criarContaNotifiqueme();">
            <input name="tpv" type="hidden" value="c" />
            <div id="div_notifiqueme">
                <fieldset class="w-60-pc">
                    <div class="mauto table">
                        <div id="div_notificacao_notifiqueme" class="notify" style="display:none;"></div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Nome Completo:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Nome Completo" obrigatorio="sim" id="nm_usuario_push" name="nm_usuario_push" type="text" value="" class="w-90-pc" />
                                </div>
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
                                    <label>Corfirme o E-mail:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Confirme o E-mail" obrigatorio="sim" id="email_usuario_push_confirme" name="email_usuario_push" type="text" value="" class="w-90-pc" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Senha:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Senha" obrigatorio="sim" id="senha_usuario_push" name="senha_usuario_push" type="password" value="" class="w-30-pc" />
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>Confirme a Senha:</label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc">
                                    <input label="Confirme a Senha" obrigatorio="sim" id="senha_usuario_push_confirme" name="senha_usuario_push" type="password" value="" class="w-30-pc" />
                                </div>
                            </div>
                        </div>
                        <div id="div_captcha_criar_conta" class="line">
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
                            <div class="column w-30-pc">
                                &nbsp;
                            </div>
                            <div class="column w-70-pc">
                                <div class="cell w-60-pc" style="text-align: justify !important;word-break: normal !important;word-wrap: normal !important;">
                                    <label><input label="Estou ciente..." obrigatorio="sim" id="checkbox_ciente" type="checkbox" value="true" />"Estou ciente de que o serviço SINJ-DF - Notifique-Me(PUSH) - é meramente informativo, não tendo, portanto, cunho oficial"</label>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-100-pc text-center">
                                <button>
                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_check.png" width="20px" height="20" /> Enviar
                                </button>
                                <button type="reset">
                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_close.png" width="20px" height="20" /> Cancelar
                                </button>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
        </form>
    </div>
</asp:Content>
