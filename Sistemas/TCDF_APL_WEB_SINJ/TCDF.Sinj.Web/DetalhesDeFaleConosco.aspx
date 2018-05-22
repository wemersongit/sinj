<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="DetalhesDeFaleConosco.aspx.cs" Inherits="TCDF.Sinj.Web.DetalhesDeFaleConosco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var ch_chamado = GetParameterValue("ch_chamado");
            if (IsNotNullOrEmpty(ch_chamado)) {
                var sucesso = function (data) {
                    if (IsNotNullOrEmpty(data, 'error_message')) {
                        notificar('#div_chamado', data.error_message, 'error');
                    }
                    else if (IsNotNullOrEmpty(data, 'ch_chamado')) {
                        $('#div_nm_user').text(getVal(data.nm_user));
                        $('#div_ds_email').text(getVal(data.ds_email));
                        $('#div_ds_assunto').text(getVal(data.ds_assunto));
                        $('#div_ds_msg').text(getVal(data.ds_msg));
                        $('#div_dt_inclusao').text(getVal(data.dt_inclusao));
                        $('#div_st_atendimento').text(getVal(data.st_atendimento));
                        $('#span_nm_usuario_atendimento').text(getVal(data.nm_usuario_atendimento) + ' - ');
                        $('#span_dt_recebido').text(getVal(data.dt_recebido));
                        $('#div_dt_finalizado').text(getVal(data.dt_finalizado));
                        if (IsNotNullOrEmpty(data.mensagens)) {
                            var line = "";
                            var oddEven = "";
                            for (var i = 0; i < data.mensagens.length; i++) {
                                oddEven = i % 2 == 0 ? "even" : "odd";
                                line += '<div class="line ' + oddEven + '">';
                                line += '<div class="column w-30-pc"><div class="cell fr"><label>Assunto:</label></div></div>';
                                line += '<div class="column w-60-pc"><div class="cell w-60-pc">' + data.mensagens[i].ds_assunto_resposta + '</div></div>';
                                line += '</div>'

                                line += '<div class="line ' + oddEven + '">';
                                line += '<div class="column w-30-pc"><div class="cell fr"><label>Mensagem:</label></div></div>';
                                line += '<div class="column w-60-pc"><div class="cell w-60-pc">' + data.mensagens[i].ds_msg_resposta + '</div></div>';
                                line += '</div>'

                                line += '<div class="line ' + oddEven + '">';
                                line += '<div class="column w-30-pc"><div class="cell fr"><label>Usuário:</label></div></div>';
                                line += '<div class="column w-60-pc"><div class="cell w-60-pc">' + data.mensagens[i].nm_usuario_resposta + '</div></div>';
                                line += '</div>'

                                line += '<div class="line ' + oddEven + '">';
                                line += '<div class="column w-30-pc"><div class="cell fr"><label>Data:</label></div></div>';
                                line += '<div class="column w-60-pc"><div class="cell w-60-pc">' + data.mensagens[i].dt_resposta + '</div></div>';
                                line += '</div>'
                            }
                            $('#div_mensagens').html(line);
                        }
                    }
                };
                $.ajaxlight({
                    sUrl: './ashx/Visualizacao/FaleConoscoDetalhes.ashx' + window.location.search,
                    sType: "POST",
                    fnSuccess: sucesso,
                    fnComplete: gComplete,
                    fnBeforeSend: gInicio,
                    fnError: null,
                    bAsync: true
                });
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Detalhes do Chamado</label>
	</div>
    <div id="div_controls" class="control">
    </div>
    <div class="form">
        <div id="div_chamado" class="loaded">
            <fieldset class="w-60-pc">
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Nome:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_nm_user" class="cell w-60-pc">
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
                            <div id="div_ds_email" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Assunto:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_ds_assunto" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Mensagem:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_ds_msg" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Data:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_dt_inclusao" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Status:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_st_atendimento" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Recebido por:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div class="cell w-60-pc">
                                <span id="span_nm_usuario_atendimento"></span><span id="span_dt_recebido"></span>
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fr">
                                <label>Finalizado:</label>
                            </div>
                        </div>
                        <div class="column w-70-pc">
                            <div id="div_dt_finalizado" class="cell w-60-pc">
                            </div>
                        </div>
                    </div>
                    <div class="line">
                        <h2 class="text-center">Mensagens</h2>
                        <div class="column w-100-pc">
                            <div id="div_mensagens" class="table w-90-pc"></div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
