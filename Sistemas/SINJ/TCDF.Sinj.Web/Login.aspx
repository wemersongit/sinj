<%@ Page Title="" Language="C#" MasterPageFile="~/MasterLogin.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TCDF.Sinj.Web.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
        	$('#buttonLogin').click(function () {
                return Logar();
            });
            var cd = GetParameterValue("cd");
            var erro = MostrarErro(cd);
            if (IsNotNullOrEmpty(erro)) {
                $('#div_notificacao_login').messagelight({
                    sTitle: "Erro",
                    sContent: erro,
                    sType: "error",
                    sWidth: "",
                    iTime: null
                });
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <form id="form_login" name="formLogin" method="post" action="#">
        <div style="margin:auto; width:500px; position:relative; margin-top: 10%;">
            <div id="div_notificacao_login" style="display:none;"></div>
            <div class="loading" style="display:none;"></div>
            <div class="form loaded">
                <fieldset>
                    <div class="table">
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>
                                        Login:
                                    </label>
                                </div>
                            </div>
                            <div class="column w-60-pc">
                                <input id="login" type="text" name="login" obrigatorio="sim" label="Login" class="w-100-pc" />
                            </div>
                        </div>
                        <div class="line">
                            <div class="column w-30-pc">
                                <div class="cell fr">
                                    <label>
                                        Senha:
                                    </label>
                                </div>
                            </div>
                            <div class="column w-60-pc">
                                <input id="senha" type="password" name="senha" obrigatorio="sim" label="Senha" class="w-100-pc" />
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
                                <button id="buttonLogin" class="fr" >
                                    <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico-login-p.png" class="login" width="20px" height="20" /> Login
                                </button>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
    </form>
</asp:Content>

