<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="CadastrarNotifiqueme.aspx.cs" Inherits="TCDF.Sinj.Web.CadastrarNotifiqueme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= TCDF.Sinj.Util._urlPadrao %>/Captcha/css/epicaptcha.css?<%= TCDF.Sinj.Util.MostrarVersao() %>" />
    <script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Captcha/js/epicaptcha.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_resetar_notifiqueme').click(function () {
                document.getElementById("form_notifiqueme").reset();
                location.reload();
            });
			var sucesso;
			var redirecionar_notifiqueme = GetParameterValue("redirecionar_notifiqueme");
            if (IsNotNullOrEmpty(redirecionar_notifiqueme)){
	        	redirecionar_notifiqueme = redirecionar_notifiqueme.replace(/__EQUAL__/g, "=");
	        	redirecionar_notifiqueme = redirecionar_notifiqueme.replace(/__AND__/g, "&");
	        	redirecionar_notifiqueme = redirecionar_notifiqueme.replace(/__QUERY__/g, "?");
	        	var array_ch_norma = redirecionar_notifiqueme.split("&");
	        	var ch_norma = array_ch_norma[array_ch_norma.length -1];

				var sucesso = function(data){
					if (IsNotNullOrEmpty(data)) {
	                    $('#form_notifiqueme .loading').hide();
	                    $('#form_notifiqueme .loaded').show();
	                    if (data.error_message != null && data.error_message != "") {
	                        $('#form_notifiqueme .notify').messagelight({
	                            sTitle: "Erro",
	                            sContent: data.error_message,
	                            sType: "error",
	                            sWidth: "",
	                            iTime: null
	                        });
	                    }
	                    else if (IsNotNullOrEmpty(data, 'id_doc_success')){
						    $.ajaxlight({
						        sUrl: "./ashx/Push/NotifiquemeNormaIncluir.ashx?" + ch_norma,
						        sType: "GET",
						        fnError: null,
						        iTimeout: 40000
						    });
	                		top.document.location.href = redirecionar_notifiqueme;
						}
					}
				}
			}
            $("#captcha").Epicaptcha({
                buttonID: "button_salvar_notifiqueme",
                theFormID: "form_notifiqueme",
                submitUrl: "./ashx/Push/NotifiquemeIncluir.ashx",
                fnSuccess: sucesso
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
        <form id="form_notifiqueme" name="formCadastroNotifiqueme" post="post" action="#">
            <div id="div_notifiqueme">
            <div id="div_notificacao_notifiqueme" class="notify" style="display:none;"></div>
                <fieldset class="w-60-pc">
                    <legend>Autoria</legend>
                    <div class="mauto table">
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
                    </div>
                    <div style="width:220px; margin:auto;" class="loaded">
                        <input type="button" id="button_salvar_notifiqueme" value="Salvar" />
                        <input type="button" id="button_resetar_notifiqueme" value="Limpar" />
                    </div>
                </fieldset>
            </div>
            <div id="div_loading_notifiqueme" class="loading" style="display:none;"></div>
        </form>
    </div>
</asp:Content>
