<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="Reindexacao.aspx.cs" Inherits="TCDF.Sinj.Web.Reindexacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript" language="javascript">
        <% Response.Write("var url_base_rest = \"" + util.BRLight.Config.ValorChave("URLBaseRest", true) + "\";");%>
        function PreencherURL(url){
            if (url == "rest"){
                $('#url_es').val(url_base_rest);
            }
            else if (url == "limpar"){
                $('#url_es').val("");
            }
        }
        var guids_reindex = [];
        function PararAcompanhamento(guid) {
            var index = guids_reindex.indexOf(guid);
            if (index > -1) {
                guids_reindex.splice(index, 1);
                $('#acompanhar_indexacao').html('');
                $('#acompanhar_indexacao').hide();
            }
        }
        function AcompanharIndexacao(guid, url_es_antigo, url_es_novo) {
            if (guids_reindex.indexOf(guid) > -1) {
                $.ajaxlight({
                    sUrl: './ashx/Reindexacao/FollowIndexing.ashx',
                    oData: {"url_es_antigo":url_es_antigo, "url_es_novo":url_es_novo},
                    sType: "POST",
                    fnSuccess: function (data) {
                        if (IsNotNullOrEmpty(data)) {
                            if (IsNotNullOrEmpty(data.error_message)) {
                                PararAcompanhamento(guid);
                                $('#form_reindex .notify').messagelight({
                                    sTitle: "Erro",
                                    sContent: data.error_message,
                                    sType: "error",
                                    sWidth: "",
                                    iTime: null
                                });
                            }
                            else {
                                $('#acompanhar_indexacao').messagelight({
                                    sTitle: "",
                                    sContent: JSON.stringify(data, null, 4) + '<a href="javascript:void(0);" onclick="javascript:PararAcompanhamento(\'' + guid + '\');" class="fr">parar de acompanhar</a>',
                                    sType: "info pre",
                                    sWidth: "",
                                    iTime: null
                                });
                                setTimeout(function () { AcompanharIndexacao(guid, url_es_antigo, url_es_novo); }, 10000);
                            }
                        }
                    },
                    bAsync: true,
                    iTimeout: 0
                });
            }
        }
        function AdicionarValor() {
            $("#div_es div.table").append('<div class="line" key_value>' +
                '<div class="column w-20-pc">' +
                '</div>' +
                '<div class="column w-80-pc">' +
                    '<div class="cell w-100-pc">' +
                        '<input type="text" name="key_es" class="w-15-pc" placeholder="key"/>' +
                        '<input type="text" name="value_es" class="w-75-pc" placeholder="value"/>' +
                        '<a href="javascript:void(0);" onclick="javascript:RemoverValor(this);"><img valign="absmiddle" alt="Adicionar" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_delete_p.png"  /></a>' +
                    '</div>' +
                '</div>' +
            '</div>');
        }
        function RemoverValor(element) {
            $(element).closest('div[key_value]').remove();
        }

        $(document).ready(function () {
            $('#button_es').click(function () {
                var action_proxy = function () {
                    var id_form = "form_es";
                    $('#' + id_form + ' .notify').html('');
                    var sucesso = function (data) {
                        if (IsNotNullOrEmpty(data)) {
                            $('#' + id_form + ' .loading').hide();
                            $('#' + id_form + ' .loaded').show();
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "",
                                sContent: JSON.stringify(data, null, 4),
                                sType: "info pre",
                                sWidth: "",
                                iTime: null
                            });
                        }
                    }
                    var beforeSubmit = function () {
                        $('#' + id_form + ' .loading').show();
                        $('#' + id_form + ' .loaded').hide();
                    };
                    $.ajaxlight({
                        sUrl: './ashx/Reindexacao/GetEs.ashx',
                        sType: "POST",
                        fnSuccess: sucesso,
                        sFormId: id_form,
                        fnBeforeSubmit: beforeSubmit,
                        bAsync: true,
                        iTimeout: 0
                    });
                }
                if ($('#verbo_es').val() == 'delete') {
                    $("<div />").modallight({
                        sTitle: "Atenção",
                        sType: "alert",
                        sContent: "A opção DELETE está selecionada.",
                        oButtons: [
                            {
                                text: "Continuar",
                                click: function () {
                                    action_proxy();
                                    $(this).dialog("close");
                                }
                            },
                            {
                                text: 'Cancelar', click: function () {
                                    $(this).dialog("close");
                                }
                            }
                        ]
                    });
                }
                else{
                    action_proxy();
                }
                return false;
            });
            $('#button_index').click(function () {
                var id_form = "form_index";
                $('#' + id_form + ' .notify').html('');
                var sucesso = function (data) {
                    console.log(data);
                    if (IsNotNullOrEmpty(data)) {
                        $('#' + id_form + ' .loading').hide();
                        $('#' + id_form + ' .loaded').show();
                        if (IsNotNullOrEmpty(data.error_message)) {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Erro",
                                sContent: data.error_message,
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else if (IsNotNullOrEmpty(data.success_message)) {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Sucesso",
                                sContent: data.success_message,
                                sType: "success",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Alerta",
                                sContent: JSON.stringify(data, null, 4),
                                sType: "alert pre",
                                sWidth: "",
                                iTime: null
                            });
                        }
                    }
                }
                var beforeSubmit = function () {
                    $('#' + id_form + ' .loading').show();
                    $('#' + id_form + ' .loaded').hide();
                };
                $.ajaxlight({
                    sUrl: './ashx/Reindexacao/CreateIndex.ashx',
                    sType: "POST",
                    fnSuccess: sucesso,
                    sFormId: id_form,
                    fnBeforeSubmit: beforeSubmit,
                    bAsync: true,
                    iTimeout: 0
                });
                return false;
            });
            $('#button_type').click(function () {
                var id_form = "form_type";
                $('#' + id_form + ' .notify').html('');
                var sucesso = function (data) {
                    if (IsNotNullOrEmpty(data)) {
                        $('#' + id_form + ' .loading').hide();
                        $('#' + id_form + ' .loaded').show();
                        if (IsNotNullOrEmpty(data.error_message)) {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Erro",
                                sContent: data.error_message,
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else if (IsNotNullOrEmpty(data.success_message)) {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Sucesso",
                                sContent: data.success_message,
                                sType: "success",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Alerta",
                                sContent: JSON.stringify(data, null, 4),
                                sType: "alert pre",
                                sWidth: "",
                                iTime: null
                            });
                        }
                    }
                }
                var beforeSubmit = function () {
                    $('#' + id_form + ' .loading').show();
                    $('#' + id_form + ' .loaded').hide();
                };
                $.ajaxlight({
                    sUrl: './ashx/Reindexacao/CreateType.ashx',
                    sType: "POST",
                    fnSuccess: sucesso,
                    sFormId: id_form,
                    fnBeforeSubmit: beforeSubmit,
                    bAsync: true,
                    iTimeout: 0
                });
                return false;
            });
            $('#button_reindex').click(function () {
                var id_form = "form_reindex";
                $('#' + id_form + ' .notify').html('');
                var guid = Guid();
                guids_reindex.push(guid);
                var sucesso = function (data) {
                    PararAcompanhamento(guid);
                    if (IsNotNullOrEmpty(data)) {
                        $('#' + id_form + ' .loading').hide();
                        $('#' + id_form + ' .loaded').show();
                        if (IsNotNullOrEmpty(data.error_message)) {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Erro",
                                sContent: data.error_message,
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else if (IsNotNullOrEmpty(data.success_message)) {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Sucesso",
                                sContent: data.success_message,
                                sType: "success",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Alerta",
                                sContent: JSON.stringify(data, null, 4),
                                sType: "alert pre",
                                sWidth: "",
                                iTime: null
                            });
                        }
                    }
                }
                var beforeSubmit = function () {
                    $('#' + id_form + ' .loading').show();
                    $('#' + id_form + ' .loaded').hide();
                    AcompanharIndexacao(guid, $('#url_es_antigo').val(), $('#url_es_novo').val());
                };
                $.ajaxlight({
                    sUrl: './ashx/Reindexacao/Reindex.ashx',
                    sType: "POST",
                    fnSuccess: sucesso,
                    sFormId: id_form,
                    fnBeforeSubmit: beforeSubmit,
                    bAsync: true,
                    iTimeout: 0
                });
                return false;
            });
            $('#button_idx_exp_url').click(function () {
                var id_form = "form_idx_exp_url";
                $('#' + id_form + ' .notify').html('');
                var sucesso = function (data) {
                    if (IsNotNullOrEmpty(data)) {
                        $('#' + id_form + ' .loading').hide();
                        $('#' + id_form + ' .loaded').show();
                        if (IsNotNullOrEmpty(data.error_message)) {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Erro",
                                sContent: data.error_message,
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else if (IsNotNullOrEmpty(data.success_message)) {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Sucesso",
                                sContent: data.success_message,
                                sType: "success",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Alerta",
                                sContent: JSON.stringify(data, null, 4),
                                sType: "alert pre",
                                sWidth: "",
                                iTime: null
                            });
                        }
                    }
                }
                var beforeSubmit = function () {
                    $('#' + id_form + ' .loading').show();
                    $('#' + id_form + ' .loaded').hide();
                };
                $.ajaxlight({
                    sUrl: './ashx/Reindexacao/AlterIdxExpUrl.ashx',
                    sType: "POST",
                    fnSuccess: sucesso,
                    sFormId: id_form,
                    fnBeforeSubmit: beforeSubmit,
                    bAsync: true,
                    iTimeout: 0
                });
                return false;
            });
            $('#button_reset').click(function () {
                var id_form = "form_reset";
                $('#' + id_form + ' .notify').html('');
                var sucesso = function (data) {
                    console.log(data);
                    if (IsNotNullOrEmpty(data)) {
                        $('#' + id_form + ' .loading').hide();
                        $('#' + id_form + ' .loaded').show();
                        if (IsNotNullOrEmpty(data.error_message)) {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Erro",
                                sContent: data.error_message,
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else if (IsNotNullOrEmpty(data.success_message)) {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Sucesso",
                                sContent: data.success_message,
                                sType: "success",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Alerta",
                                sContent: JSON.stringify(data, null, 4),
                                sType: "alert pre",
                                sWidth: "",
                                iTime: null
                            });
                        }
                    }
                }
                var beforeSubmit = function () {
                    $('#' + id_form + ' .loading').show();
                    $('#' + id_form + ' .loaded').hide();
                };
                $.ajaxlight({
                    sUrl: './ashx/Reindexacao/ResetRest.ashx',
                    sType: "POST",
                    fnSuccess: sucesso,
                    sFormId: id_form,
                    fnBeforeSubmit: beforeSubmit,
                    bAsync: true,
                    iTimeout: 0
                });
                return false;
            });
            $('#button_exibir').click(function () {
                var id_form = "form_exibir";
                $('#' + id_form + ' .notify').html('');
                var sucesso = function (data) {
                    console.log(data);
                    if (IsNotNullOrEmpty(data)) {
                        $('#' + id_form + ' .loading').hide();
                        $('#' + id_form + ' .loaded').show();
                        if (IsNotNullOrEmpty(data.error_message)) {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Erro",
                                sContent: data.error_message,
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else if (IsNotNullOrEmpty(data.success_message)) {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Sucesso",
                                sContent: data.success_message,
                                sType: "success",
                                sWidth: "",
                                iTime: null
                            });
                        }
                        else {
                            $('#' + id_form + ' .notify').messagelight({
                                sTitle: "Alerta",
                                sContent: JSON.stringify(data, null, 4),
                                sType: "alert pre",
                                sWidth: "",
                                iTime: null
                            });
                        }
                    }
                }
                var beforeSubmit = function () {
                    $('#' + id_form + ' .loading').show();
                    $('#' + id_form + ' .loaded').hide();
                };
                $.ajaxlight({
                    sUrl: './ashx/Reindexacao/GetMetadata.ashx',
                    sType: "POST",
                    fnSuccess: sucesso,
                    sFormId: id_form,
                    fnBeforeSubmit: beforeSubmit,
                    bAsync: true,
                    iTimeout: 0
                });
                return false;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="form">
        <fieldset>
            <span>
                Para reindexar todos os regitros de uma base:<br />
                1. Crie um novo index;<br />
                &nbsp;&nbsp;Exemplo:<br />
                &nbsp;&nbsp;&nbsp;&nbsp;<b>URL Index</b>: http://&lt;ip_es&gt;:9200/&lt;index_novo&gt;<br />
                &nbsp;&nbsp;&nbsp;&nbsp;<b>Settings</b>: {"settings":{"analysis":{"analyzer":{"default":{"tokenizer":"standard","filter":["lowercase","asciifolding"]}}}}}<br />
                2. Crie um novo type;<br />
                &nbsp;&nbsp;Exemplo:<br />
                &nbsp;&nbsp;&nbsp;&nbsp;<b>URL Type</b>: http://&lt;ip_es&gt;:9200/&lt;index_novo&gt;/type<br />
                &nbsp;&nbsp;&nbsp;&nbsp;<b>Settings</b>: <br />
                3. Reindexe passando a url antiga e a nova <b>mas sem consulta</b>, verifique e anote a hora que executou a ação;<br />
                4. Altere o idx_exp_url da base reindexada para o novo endereço;<br />
                5. Resete as bases em memória;<br />
                6. Reindexe novamente mas desta vez passe uma consulta para selecionar somente os registros com dt_doc maior que data anotada.Ex: {"query":{"range":{"_metadata.dt_doc":{"gte":"01/04/2015 09:02:00"}}}}<br />
            </span>
        </fieldset>
        <div>
            <fieldset>
                <legend>ES</legend>
                <span style="float:left;">
                    1. Exibe o retorno do ES;<br />
                    &nbsp;&nbsp;Exemplo:<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>URL ES</b>: http://&lt;ip_es&gt;:9200/&lt;index_novo&gt;/_settings<br />
                </span>
                <div style="display:inline;margin-left:30px;">
                    <button onclick="javascript:PreencherURL('rest'); return false;">REST</button>
                    <button onclick="javascript:PreencherURL('limpar'); return false;">Limpar</button>
                </div>
                <form id="form_es" name="formEs" action="#" method="post">
                    <div id="div_es">
                        <div class="table w-100-pc">
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>URL Es:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <input id="url_es" name="url_es" type="text" class="w-90-pc" value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Verbo:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-20-pc">
                                        <select id="verbo_es" name="verbo_es">
                                            <option value="get">GET</option>
                                            <option value="post">POST</option>
                                            <option value="put">PUT</option>
                                            <option value="delete">DELETE</option>
                                        </select>
                                    </div>
                                    <div class="cell w-80-pc">
                                        <input type="checkbox" value="true" name="st_conditional" /> Put Condicional
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Body:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <textarea id="body_es" name="body_es" class="w-90-pc" rows="10" ></textarea>
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Campos:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <a title="Adicionar novo campo no formulário" href="javascript:void(0);" onclick="javascript:AdicionarValor();"><img valign="absmiddle" alt="Adicionar" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_add_p.png"  /> Adicionar</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="text-align:center;" class="loaded buttons">
                        <button id="button_es">
                            <img alt="ok" src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_ok_p.png" />Enviar
                        </button>
                    </div>
                    <div class="notify w-80-pc mauto" style="display:none;"></div>
                    <div class="loading" style="display:none;"></div>
                </form>
            </fieldset>
            <fieldset>
                <legend>Criar Index</legend>
                <span>
                    1. Criar um novo index;<br />
                    &nbsp;&nbsp;Exemplo:<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>URL Index</b>: http://&lt;ip_es&gt;:9200/&lt;index_novo&gt;<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>Settings</b>: {"settings":{"analysis":{"analyzer":{"default":{"tokenizer":"standard","filter":["lowercase","asciifolding"]}}}}}<br />
                    Obs.: Verifique na Pasta documentação se há um arquivo pronto. Preferível usar o \Documentação\PGFN-DOCS-2013\PORTAL\Reindexação\settings_pgfn.txt<br />
                </span>
                <form id="form_index" name="formIndex" action="#" method="post">
                    <div id="div_index">
                        <div class="table w-100-pc">
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>URL Index:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <input id="index" name="index" type="text" class="w-90-pc" value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Settings:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <textarea id="settings" name="settings" class="w-90-pc" rows="10" ></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="width:160px; margin:auto;" class="loaded buttons">
                        <button id="button_index">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Criar Index
                        </button>
                    </div>
                    <div class="notify w-80-pc mauto" style="display:none;"></div>
                    <div class="loading" style="display:none;"></div>
                </form>
            </fieldset>
        </div>
        <div>
            <fieldset>
                <legend>Criar Type</legend>
                <span>
                    1. Criar um novo Type;<br />
                    &nbsp;&nbsp;Exemplo:<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>URL Type</b>: http://&lt;ip_es&gt;:9200/&lt;index_novo&gt;/&lt;type&gt;<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>Mapping</b>: {"type":{"properties":{"_metadata":{"properties":{"dt_doc":...<br />
                    Obs.: Verifique na Pasta documentação se há um arquivo pronto. Preferível usar os mappings da pasta \Documentação\PGFN-DOCS-2013\PORTAL\Reindexação\<br />
                </span>
                <form id="form_type" name="formType" action="#" method="post">
                    <div id="div_type">
                        <div class="table w-100-pc">
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>URL Type:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <input id="type" name="type" type="text" class="w-90-pc" value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Mapping:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <textarea id="mapping" name="mapping" class="w-90-pc" value="" rows="10" ></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="width:160px; margin:auto;" class="loaded buttons">
                        <button id="button_type">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Criar Type
                        </button>
                    </div>
                    <div class="notify w-80-pc mauto" style="display:none;"></div>
                    <div class="loading" style="display:none;"></div>
                </form>
            </fieldset>
        </div>
        <div>
            <fieldset>
                <legend>Reindexar</legend>
                <span>
                    1. Reindexa os documentos;<br />
                    &nbsp;&nbsp;Exemplo:<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>URL ES Antigo</b>: http://&lt;ip_es&gt;:9200<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>Nome Index Antigo</b>: pgfn<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>Nome Type Antigo</b>: docs_pro<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>URL ES Novo</b>: http://&lt;ip_es&gt;:9200<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>Nome Index Novo</b>: pgfn_index2<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>Nome Type Novo</b>: docs_pro<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>Json Consulta</b>: Usar esse campo somente após ter feito a reindexação total, ou seja, após a reindexar todos os documentos, mudar a url no REST e resetar a memória do REST, faça uma nova indexação com consulta para enviar só os documentos mais recentes<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Exemplos de consulta:<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Todos os documentos criados a partir de 07/04/2015: {"query":{"range":{"_metadata.dt_doc":{"gte":"07/04/2015 00:00:00"}}}}<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Todos os documentos atualizados a partir de 07/04/2015: {"query":{"range":{"_metadata.dt_last_up":{"gte":"07/04/2015 00:00:00"}}}}<br />
                    2. Esse procedimento é demorado, tanto quanto a quantidade de documentos a serem indexados, assim como o tamanho de cada documento;<br />
                    3. Caso o timeout ocorra, continue verificando em outro local se a indexação está processando. Como?<br />
                    &nbsp;&nbsp;Remota ou localmente consulte a quantidade de documentos no index novo:<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;curl -XGET 'HTTP://127.0.0.1:9200/novo_index/type/_count'<br />
                    &nbsp;&nbsp;Execute o comando e fique comparando o retorno, se o total estiver sendo incrementado é porque o processo ainda está em execução. Se não continuar processando após o timeout você pode almentar esse tempo alterando a chave do web.config<br />
                </span>
                <form id="form_reindex" name="formReindex" action="#" method="post">
                    <div id="div_reindex">
                        <div class="table w-100-pc">
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>URL ES Antigo:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <input id="url_es_antigo" name="url_es_antigo" type="text" class="w-90-pc" value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>URL ES Novo:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <input id="url_es_novo" name="url_es_novo" type="text" class="w-90-pc" value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Json Consulta:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <textarea id="json_consulta" name="json_consulta" class="w-90-pc" rows="10" ></textarea>
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div id="acompanhar_indexacao" class="column w-100-pc" style="display:none;">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="width:160px; margin:auto;" class="loaded buttons">
                        <button id="button_reindex">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Reindexar
                        </button>
                    </div>
                    <div class="notify w-80-pc mauto" style="display:none;"></div>
                    <div class="loading" style="display:none;"></div>
                </form>
            </fieldset>
        </div>
        <div>
            <fieldset>
                <legend>Alterar idx_exp_url</legend>
                <span>
                    1. Altera na Base a URL de indexação;<br />
                    &nbsp;&nbsp;Exemplo:<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>Nome da Base</b>: docs_pro<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<b>idx_exp_url Novo</b>: http://&lt;ip_es&gt;:9200/pgfn_index2/docs_pro<br />
                </span>
                <form id="form_idx_exp_url" name="formIdx_exp_url" action="#" method="post">
                    <div id="div_idx_exp_url">
                        <div class="table w-100-pc">
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Nome da Base:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <input id="nm_base" name="nm_base" type="text" class="w-50-pc" value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>idx_exp_url Novo:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <input id="idx_exp_url" name="idx_exp_url" type="text" class="w-50-pc" value="" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="width:160px; margin:auto;" class="loaded buttons">
                        <button id="button_idx_exp_url">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Alterar
                        </button>
                    </div>
                    <div class="notify w-80-pc mauto" style="display:none;"></div>
                    <div class="loading" style="display:none;"></div>
                </form>
            </fieldset>
        </div>
        <div>
            <fieldset>
                <legend>Resetar Base em Memória</legend>
                <span>
                    1. Reseta as bases da memória do REST;<br />
                    1. Em ambientes balanceados, resete várias para garantir que seja resetada a memória de todos os RESTs, ou insira os IPs separados por vígula.<br />
                </span>
                <form id="form_reset" name="formReset" action="#" method="post">
                    <div id="div_reset">
                        <div class="table w-100-pc">
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>URL Rest:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <input id="url_rest" name="url_rest" type="text" class="w-50-pc" value="" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="width:160px; margin:auto;" class="loaded buttons">
                        <button id="button_reset">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Resetar
                        </button>
                    </div>
                    <div class="notify w-80-pc mauto" style="display:none;"></div>
                    <div class="loading" style="display:none;"></div>
                </form>
            </fieldset>
        </div>
        <div>
            <fieldset>
                <legend>Exibir metadata de Bases</legend>
                <span>
                    1. Exibe o metadata da base;<br />
                    1. Útil para descobrir informações como o ip do ElasticSearch.<br />
                </span>
                <form id="form_exibir" name="formExibir" action="#" method="post">
                    <div id="div_exibir">
                        <div class="table w-100-pc">
                            <div class="line">
                                <div class="column w-20-pc">
                                    <div class="cell fr">
                                        <label>Nome da Base:</label>
                                    </div>
                                </div>
                                <div class="column w-80-pc">
                                    <div class="cell w-100-pc">
                                        <input id="nm_base_metadata" name="nm_base_metadata" type="text" class="w-50-pc" value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div id="metadata" class="cell w-100-pc">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="width:160px; margin:auto;" class="loaded buttons">
                        <button id="button_exibir">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" />Exibir
                        </button>
                    </div>
                    <div class="notify w-80-pc mauto break" style="display:none;"></div>
                    <div class="loading" style="display:none;"></div>
                </form>
            </fieldset>
        </div>
    </div>
</asp:Content>
