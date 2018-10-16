<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="ResultadoDePesquisaSessao.aspx.cs" Inherits="TCDF.Sinj.Web.ResultadoDePesquisaSessao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            pesquisarSessoes();
        });

        function pesquisarSessoes(){
            $("#div_resultado").dataTablesLight({
                sAjaxUrl: './ashx/Datatable/SessaoDatatable.ashx' + window.location.search,
                aoColumns: _columns_sessao
            });
        }

        function encerrarSessoes(app) {
            $('#div_notificacao').html('');
            $('#div_notificacao').hide();

            var sucesso = function (data) {
                $('#super_loading').hide();
                if (IsNotNullOrEmpty(data, 'error_message')) {
                    $('#div_notificacao').messagelight({
                        sContent: data.error_message,
                        sType: "error",
                        iTime: null
                    });
                }
                else if (IsNotNullOrEmpty(data, 'success_message')) {
                    $('#div_notificacao').messagelight({
                        sContent: data.success_message,
                        sType: "success",
                        iTime: null
                    });
                    pesquisarSessoes();
                }
                else {
                    $('#div_notificacao').messagelight({
                        sContent: "Erro na exclusão",
                        sType: "error",
                        iTime: null
                    });
                }
            }
            var beforeSend = function () {
                $('#super_loading').show();
            };
            $.ajaxlight({
                sUrl: './ashx/Exclusao/SessaoExcluir.ashx?app=' + app,
                sType: "GET",
                fnSuccess: sucesso,
                fnBeforeSend: beforeSend
            });
        }

        function encerrarSessao(id_doc) {
            $('#div_notificacao').html('');
            $('#div_notificacao').hide();

            var sucesso = function (data) {
                $('#super_loading').hide();
                if (IsNotNullOrEmpty(data, 'error_message')) {
                    $('#div_notificacao').messagelight({
                        sContent: data.error_message,
                        sType: "error",
                        iTime: null
                    });
                }
                else if (IsNotNullOrEmpty(data, 'success_message')) {
                    $('#div_notificacao').messagelight({
                        sContent: data.success_message,
                        sType: "success",
                        iTime: null
                    });
                    $('#datatable tr.selected').remove();
                }
                else {
                    $('#div_notificacao').messagelight({
                        sContent: "Erro na exclusão",
                        sType: "error",
                        iTime: null
                    });
                }
            }
            var beforeSend = function () {
                $('#super_loading').show();
            };
            $.ajaxlight({
                sUrl: './ashx/Exclusao/SessaoExcluir.ashx?id_doc='+id_doc,
                sType: "GET",
                fnSuccess: sucesso,
                fnBeforeSend: beforeSend
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <%
        var _app = Request["app"];
        if(string.IsNullOrEmpty(_app)){
            _app = "sistema";
        }
    %>
    <div class="divIdentificadorDePagina">
	    <label>.: Resultado de Pesquisa de Sessões do <%= _app == "push" ? "Push" : "Sistema" %></label>
	</div>
    <div id="div_controls" class="control">
        <div class="div-light-button">
            <a href="./ResultadoDePesquisaSessao.aspx?app=sistema"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png"/>Sessões do Sistema</a>
        </div>
        <div class="div-light-button">
            <a href="./ResultadoDePesquisaSessao.aspx?app=push"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_loupe_p.png"/>Sessões do Push</a>
        </div>
    </div>
    <div class="control" style="height: 30px; position: relative;">
        <div class="div-light-button" style="right: 0px; position: absolute;">
            <a href="javascript:void(0);" onclick="javascript:encerrarSessoes('<%= _app %>')"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png" />Encerrar Sessões</a>
        </div>
        <br />
    </div>
    <div id="div_notificacao" class="notify" style="display:none; width: 90%; margin: auto;"></div>
    <div id="div_resultado" class="w-90-pc mauto">
    </div>
</asp:Content>
