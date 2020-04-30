<%@ Page Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="LoginNotifiqueme.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.LoginNotifiqueme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/login_notifiqueme.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_logar_notifiqueme').click(function () {
                return LogarNotifiqueme();
            });

            var redirecionar_notifiqueme = GetParameterValue("redirecionar_notifiqueme");
            var p = GetParameterValue("p");

            var _message = GetParameterValue("message");
            var _type = GetParameterValue("type");
            if (IsNotNullOrEmpty(_message) && IsNotNullOrEmpty(_type)) {
                $('#div_notificacao_login_notifiqueme').messagelight({
                    sContent: _message,
                    sType: _type,
                    iTime: null
                });
            }
            if (IsNotNullOrEmpty(redirecionar_notifiqueme)) {
                NotificarAoCriar(redirecionar_notifiqueme);
            }
            if (p == "FaleConosco") {
                $('div.divIdentificadorDePagina label').text('.: Fale Conosco');
            }
            $('#a_cadastrar_notifiqueme').attr('href', $('#a_cadastrar_notifiqueme').attr('href') + document.location.search);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Notifique-me</label>
	</div>
    <form id="form_login_notifiqueme" name="formLoginNotifiqueme" method="post" action="#">
        <div style="margin:auto; width:500px;">
            <div id="div_notificacao_login_notifiqueme" style="display:none;" class="mauto w-80-pc"></div>
            <div class="loading" style="display:none;"></div>
            <div class="form loaded">
                <fieldset>
                    <div class="table">
                        <div class="line">
                            <div class="column w-100-pc">
                                <div class="cell w-100-pc text-center">
                                    <label>
                                        A T E N Ç Ã O !
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-100-pc">
                                <div id="div_descricao_servico" class="cell w-100-pc">
                                    <p>Este é o serviço de notificação (PUSH) e atendimento (Fale Conosco) do sistema SINJ. Receba e-mails notificando sobre as criações e alterações de normas e criação de diários oficiais contendo um texto de seu interesse.</p><p>Acompanhe o histórico das suas mensagens enviadas pelo Serviço de atendimento 'Fale Conosco'.</p>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>
                                        E-mail:
                                    </label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <input id="email_usuario_push" type="text" name="email_usuario_push" obrigatorio="sim" label="E-mail" class="w-100-pc" />
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-20-pc">
                                <div class="cell fr">
                                    <label>
                                        Senha:
                                    </label>
                                </div>
                            </div>
                            <div class="column w-70-pc">
                                <input id="senha_usuario_push" type="password" name="senha_usuario_push" obrigatorio="sim" label="Senha" class="w-50-pc" />
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-90-pc">
                                <div class="cell fr">
                                    <label><input id="persist" type="checkbox" name="persist" value="1" checked="checked" />Mantenha-me conectado</label>
                                </div>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-80-pc">
                                <button id="button_logar_notifiqueme" class="fr" >
                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico-login-p.png" width="20px" height="20" /> Login
                                </button>
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-100-pc">
                                <div class="cell fr">
                                    <a href="./RecriarSenhaNotifiqueme" title="Criar uma senha nova">Esqueceu sua senha?</a>
                                    <br />
                                    <a id="a_cadastrar_notifiqueme" href="./CriarContaNotifiqueme" title="Criar uma conta">Não tem login?</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
    </form>
</asp:Content>
