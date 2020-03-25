<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="EditarVide.aspx.cs" Inherits="TCDF.Sinj.Web.EditarVide" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vide.js?<%= TCDF.Sinj.Util.MostrarVersao() %>" ></script>
<script type="text/javascript" language="javascript" src="<%= TCDF.Sinj.Util._urlPadrao %>/Scripts/funcoes_vide_editar.js?<%= TCDF.Sinj.Util.MostrarVersao() %>"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#button_salvar_vide').click(function () {
                return fnSalvar("form_editar_vide", "", sucesso_vide);
            });
            $('#button_cancelar').click(function () { voltar() });

            var open_modal = function (data) {
                $('<div id="modal_notificacao_modal_salvar" />').modallight({
                    sTitle: "Sucesso",
                    sContent: "Salvo com sucesso." + (IsNotNullOrEmpty(data, 'alert_message') ? '<br/>Observação:<br/>O vide foi alterado com sucesso mas houve um erro na alteração do arquivo.<br/>Mensagem do erro:<br/>' + data.alert_message : ''),
                    sType: "success",
                    oButtons: [{
                        text: "Ok",
                        click: function () {
                            $(this).dialog('close');
                        }
                    }],
                    fnClose: function () {
                        if (IsNotNullOrEmpty(data, "ch_norma")) {
                            Redirecionar('?id_norma=' + data.ch_norma);
                        } else {
                            if (!Redirecionar('?id_doc=' + data.id_doc_success + "&time=" + new Date().getTime())) {
                                location.reload();
                            }
                        }
                    }
                });
            }

            var sucesso_vide = function (data) {
                gComplete();
                if (IsNotNullOrEmpty(data)) {
                    if (IsNotNullOrEmpty(data, 'id_doc_success')) {
                        open_modal(data);
                    }
                    else if (IsNotNullOrEmpty(data, 'type_error') && data.type_error == 'RiskOfInconsistency') {
                        $('<div id="modal_notificacao_modal_salvar" />').modallight({
                            sTitle: "Erro de Inconsistência",
                            sContent: data.error_message,
                            sType: "error",
                            oButtons: [
                                {
                                    text: "Atualizar Página", click: function () {
                                        location.reload();
                                    }
                                },
                                {
                                    text: "Salvar mesmo assim", click: function () {
                                        $("#dt_controle_alteracao").val(data.dt_controle_alteracao);
                                        $('#button_salvar_vide').click();

                                        $(this).dialog('destroy');
                                    }
                                }
                            ],
                            fnClose: function () {
                                location.reload();
                            }
                        });
                    }
                    else {
                        $('#form_vide .notify').messagelight({
                            sTitle: "Erro",
                            sContent: "Erro ao salvar.<br>" + data.error_message,
                            sType: "error",
                            sWidth: "",
                            iTime: null
                        });
                    }
                }
            };

            var id_doc = GetParameterValue('id_doc');
            var ch_vide = GetParameterValue('ch_vide');
            if (IsNotNullOrEmpty(id_doc)) {
                $('#id_doc').val(id_doc);
                var sucesso = function (data) {
                    if (IsNotNullOrEmpty(data)) {
                        if (data.error_message != null && data.error_message != "") {
                            $('#div_notificacao_vide').messagelight({
                                sTitle: "Erro",
                                sContent: data.error_message,
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                            gComplete();
                        }
                        else if (IsNotNullOrEmpty(data.ch_norma)) {
                            $('#dt_controle_alteracao').val(data.dt_controle_alteracao);
                            for (var i = 0; i < data.vides.length; i++) {
                                if (data.vides[i].ch_vide == ch_vide) {
                                    vide = data.vides[i];
                                    vide.nm_tipo_relacao = getNmTipoRelacao(vide);
                                    $('#tipoDeRelacao').val(vide.nm_tipo_relacao);
                                    selecionarNormaEditar(data);
                                }
                            }
                            if (!IsNotNullOrEmpty(vide, 'ch_vide')) {
                                $('#div_notificacao_vide').messagelight({
                                    sTitle: "Erro",
                                    sContent: "O vide não existe.",
                                    sType: "error",
                                    sWidth: "",
                                    iTime: null
                                });
                                gComplete();
                            }
                        }
                    }
                };
                $.ajaxlight({
                    sUrl: './ashx/Visualizacao/NormaDetalhes.ashx' + window.location.search,
                    sType: "GET",
                    fnSuccess: sucesso,
                    fnComplete: null,
                    fnBeforeSend: gInicio,
                    fnError: null,
                    bAsync: true
                });
            }
        });
    </script>
    <style type="text/css">
        div.table div.head div.title{width:100%}
        div.table div.head div.title h2{font-size: 18px;}
        #div_tooltip_dispositivo{
            position:absolute;
            z-index:1001;
            width: 45%;
        }
        .div_conteudo_arquivo p { margin-bottom: 10px; font-size: 13px; }
        .div_conteudo_arquivo .remover { background-color: #ffa9a9; }
        .div_conteudo_arquivo .desfazer { background-color: #b4f3b4; }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Editar Vide</label>
	</div>
    <div class="form">
        <div id="div_cad_vide">
            <form id="form_vide_excluir" name="formExcluirVide" action="#" method="post">
                <input type="hidden" id="dt_controle_alteracao" name="dt_controle_alteracao" value="<%= DateTime.Now.ToString("dd'/'MM'/'yyyy HH':'mm':'ss") %>" />
                <input type="hidden" id="id_doc" name="id_doc" value="" />
                <input type="hidden" id="vide" name="vide" value="" />
                <div id="div_vide" class="loaded">
                    <fieldset class="w-95-pc">
                        <div id="div_notificacao_vide" class="w-50-pc mauto h-45-px"></div> 
                        <div class="mauto table">
                            <div class="line">
                                <div class="column w-100-pc">
                                    <div class="table w-100-pc">
                                        <div class="line">
                                            <div id="columnRelacao" class="column w-100-pc text-center bold">
                                                <label>Tipo de Relação:</label>
                                                <input type="text" id="tipoDeRelacao" value="" disabled="disabled" />
                                            </div>
                                        </div>
                                        <div id="lineNormas" class="line">
                                            <div class="column w-50-pc">
                                                <div class="cell w-100-pc">
                                                    <div class="table w-99-pc mauto">
                                                        <div class="head">
                                                            <div class="title">
                                                                <h2>Norma Alteradora</h2>
                                                            </div>
                                                        </div>
                                                        <div class="line">
                                                            <div class="column w-70-pc">
                                                                <div class="cell w-100-pc">
                                                                    <label id="label_norma_vide_alteradora"></label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="line line_vigor_dispositivo" style="display:none;">
                                                            <div class="column w-30-pc">
                                                                <div class="cell w-100-pc">
                                                                    Início da Vigência:
                                                                </div>
                                                            </div>
                                                            <div class="column w-70-pc">
                                                                <div class="cell w-100-pc">
                                                                    <input type="text" id="dt_inicio_vigor" value="" disabled="disabled" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="line">
                                                            <div id="div_cad_dispositivo_alteradora" style="display:none;">
                                                                <div class="table w-100-pc">
                                                                    <hr />
                                                                    <div class="line line_conteudo_arquivo" style="display:none;">
                                                                        <div class="column w-100-pc">
                                                                            <div class="div_conteudo_arquivo w-100-pc mauto">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="columnNormaAlterada" class="column w-50-pc">
                                                <div class="cell w-100-pc">
                                                    <div class="table w-99-pc mauto">
                                                        <div class="head">
                                                            <div class="title">
                                                                <h2>Norma Alterada</h2>
                                                            </div>
                                                        </div>
                                                        <div id="line_norma_dentro_do_sistema" class="line">
                                                            <div class="column w-30-pc">
                                                                <div class="cell w-100-pc">
                                                                    <label id="label_norma_vide_alterada"></label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div id="line_norma_fora_do_sistema" class="line hidden">
                                                            <button type="button" class="clean button-close" title="Fechar" onclick="fecharNormaForaDoSistema()"><img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_fechar.png" alt="fechar" width="12px" height="12px" /></button>
                                                            <div class="column w-100-pc">
                                                                <div class="table w-100-pc">
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Tipo:
                                                                            </div>
                                                                        </div>
                                                                        <div class="column w-70-pc">
                                                                            <div id="div_autocomplete_tipo_norma_vide_fora_de_sistema" class="cell w-100-pc">
                                                                                <input type="text" id="nm_tipo_norma_vide_fora_do_sistema" value="" class="w-80-pc" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Número:
                                                                            </div>
                                                                        </div>
                                                                        <div class="column w-70-pc">
                                                                            <div class="cell w-100-pc">
                                                                                <input type="text" id="nr_norma_vide_fora_do_sistema" value="" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Tipo de Fonte:
                                                                            </div>
                                                                        </div>
                                                                        <div class="column w-70-pc">
                                                                            <div id="div_autocomplete_tipo_fonte_vide_fora_do_sistema" class="cell w-100-pc">
                                                                                <input type="text" id="nm_tipo_fonte_vide_fora_do_sistema" value="" class="w-80-pc" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Data de Publicação:
                                                                            </div>
                                                                        </div>
                                                                        <div class="column w-70-pc">
                                                                            <div class="cell w-100-pc">
                                                                                <input type="text" id="dt_publicacao_norma_vide_fora_do_sistema" value="" class="date" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Página:
                                                                            </div>
                                                                        </div>
                                                                        <div class="column w-70-pc">
                                                                            <div class="cell w-100-pc">
                                                                                <input type="text" id="nr_pagina_publicacao_norma_vide_fora_do_sistema" value="" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Coluna:
                                                                            </div>
                                                                        </div>
                                                                        <div class="column w-70-pc">
                                                                            <div class="cell w-100-pc">
                                                                                <input type="text" id="nr_coluna_publicacao_norma_vide_fora_do_sistema" value="" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Artigo:
                                                                            </div>
                                                                        </div>
                                                                        <div class="column w-70-pc">
                                                                            <div class="cell w-100-pc">
                                                                                <input type="text" id="artigo_norma_vide_alterada" value="" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Parágrafo:
                                                                            </div>
                                                                        </div>
                                                                        <div class="column w-70-pc">
                                                                            <div class="cell w-100-pc">
                                                                                <input type="text" id="paragrafo_norma_vide_alterada" value="" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Inciso:
                                                                            </div>
                                                                        </div>
                                                                        <div class="column w-70-pc">
                                                                            <div class="cell w-100-pc">
                                                                                <input type="text" id="inciso_norma_vide_alterada" value="" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Alínea:
                                                                            </div>
                                                                        </div>
                                                                        <div class="column w-70-pc">
                                                                            <div class="cell w-100-pc">
                                                                                <input type="text" id="alinea_norma_vide_alterada" value="" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Item:
                                                                            </div>
                                                                        </div>
                                                                        <div class="column w-70-pc">
                                                                            <div class="cell w-100-pc">
                                                                                <input type="text" id="item_norma_vide_alterada" value="" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="line">
                                                                        <div class="column w-30-pc">
                                                                            <div class="cell w-100-pc">
                                                                                Anexo:
                                                                            </div>
                                                                        </div>
                                                                        <div class="column w-70-pc">
                                                                            <div class="cell w-100-pc">
                                                                                <input type="text" id="anexo_norma_vide_alterada" value="" disabled="disabled" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        
                                                        <div class="line">
                                                            <div id="div_cad_dispositivo_alterada" style="display:none;">
                                                                <div class="table w-100-pc">
                                                                    <hr />
                                                                    <div class="line line_conteudo_arquivo" style="display:none;">
                                                                        <div class="column w-100-pc">
                                                                            <div class="div_conteudo_arquivo w-100-pc mauto">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="lineComentario" class="line comentario">
                                            <div class="column w-100-pc">
                                                <div class="table w-80-pc mauto">
                                                    <div class="line">
                                                        <div class="column w-100-pc">
                                                            Comentário:
                                                        </div>
                                                        <div class="column w-100-pc">
                                                            <textarea id="ds_comentario_vide" class="w-100-pc" rows="5" cols="50" disabled="disabled"></textarea>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <div id="divButtons" class="buttons loaded text-center">
                        <button id="button_salvar_vide">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_disk_p.png" alt="salvar" />Salvar
                        </button>
                        <button id="button_cancelar">
                            <img src="<%= TCDF.Sinj.Util._urlPadrao %>/Imagens/ico_eraser_p.png" alt="limpar" />Cancelar
                        </button>
                    </div>
                </div>
                <div id="div_loading_vide" class="loading" style="display:none;"></div>
            </form>
        </div>
        <div id="div_cad_arquivos" class="hidden">
            <form id="form_arquivo_norma_alteradora" name="form_arquivo_norma_alteradora" url-ajax="./ashx/Arquivo/UploadHtml.ashx" action="#" method="post">
                <input name="nm_base" type="hidden" value="sinj_norma" />
                <input name="nm_arquivo" type="hidden" value="" />
                <textarea name="arquivo"></textarea>
            </form>
            <form id="form_arquivo_norma_alterada" name="form_arquivo_norma_alterada" url-ajax="./ashx/Arquivo/UploadHtml.ashx" action="#" method="post">
                <input name="nm_base" type="hidden" value="sinj_norma" />
                <input name="nm_arquivo" type="hidden" value="" />
                <textarea name="arquivo"></textarea>
            </form>
        </div>
    </div>
</asp:Content>
