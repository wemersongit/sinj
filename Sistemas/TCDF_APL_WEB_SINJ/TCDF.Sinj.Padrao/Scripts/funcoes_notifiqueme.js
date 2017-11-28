﻿var notifiquemeOv = null;
$(document).ready(function () {
    $('.tabs').tabs();
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, "email_usuario_push")) {
            fnDatatableNotificacaoDiariosMonitorados(data.termos_diarios_monitorados);
            fnDatatableNotificacaoNormasMonitoradas(data.normas_monitoradas);
            fnDatatableNotificacaoCadastroNormas(data.criacao_normas_monitoradas);
            $('#nm_usuario_push').val(data.nm_usuario_push);
            $('#email_usuario_push').val(data.email_usuario_push);
            $('#st_push').prop("checked", data.st_push);
            $('#button_salvar_notifiqueme').click(function () {
                return fnSalvar('form_notifiqueme_atualizar');
            });
            $('#div_autocomplete_tipo_norma_modal').autocompletelight({
                sKeyDataName: "ch_tipo_norma",
                sValueDataName: "nm_tipo_norma",
                sInputHiddenName: "ch_tipo_norma_modal",
                sInputName: "nm_tipo_norma_modal",
                sAjaxUrl: "./ashx/Autocomplete/TipoDeNormaAutocomplete.ashx",
                bLinkAll: true,
                sLinkName: "a_tipo_norma_modal"
            });
            $('#div_autocomplete_tipo_norma_modal').autocompletelight({
                sKeyDataName: "ch_tipo_norma",
                sValueDataName: "nm_tipo_norma",
                sInputHiddenName: "ch_tipo_norma_criacao",
                sInputName: "nm_tipo_norma_criacao",
                sAjaxUrl: "./ashx/Autocomplete/TipoDeNormaAutocomplete.ashx",
                bLinkAll: true,
                sLinkName: "a_tipo_norma_criacao"
            });
            $('#div_autocomplete_orgao_modal').autocompletelight({
                sKeyDataName: "ch_orgao",
                sValueDataName: "nm_orgao",
                sInputHiddenName: "ch_orgao_criacao",
                sInputName: "nm_orgao_criacao",
                sAjaxUrl: "./ashx/Autocomplete/OrgaoAutocomplete.ashx?st_orgao=true",
                bLinkAll: true,
                sLinkName: "a_orgao_criacao"
            });
            $('#div_autocomplete_termo_modal').autocompletelight({
                sKeyDataName: "ch_termo",
                sValueDataName: "nm_termo",
                sInputHiddenName: "ch_termo_criacao",
                sInputName: "nm_termo_criacao",
                sAjaxUrl: "./ashx/Autocomplete/VocabularioAutocomplete.ashx",
                bLinkAll: true,
                sLinkName: "a_termo_criacao",
                dOthersHidden: [
                        { campo_app: "ch_tipo_termo_criacao", campo_base: "ch_tipo_termo" }
                    ]
            });
            $('#div_autocomplete_tipo_fonte').autocompletelight({
                sKeyDataName: "ch_tipo_fonte",
                sValueDataName: "nm_tipo_fonte",
                sInputHiddenName: "ch_tipo_fonte_diario_monitorado",
                sInputName: "nm_tipo_fonte_diario_monitorado",
                sAjaxUrl: './ashx/Autocomplete/TipoDeFonteAutocomplete.ashx',
                bLinkAll: true,
                sLinkName: "a_tipo_fonte"
            });
        }
        else if (IsNotNullOrEmpty(data, "id_doc_error")) {
            $('#div_notificacao_notifiqueme').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: ""
            });
        }
        else {
            $('#div_notificacao_notifiqueme').messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro desconhecido.",
                sType: "error",
                sWidth: ""
            });
        }
    }
    $.ajaxlight({
        sUrl: './ashx/Push/NotifiquemeDetalhes.ashx',
        sType: "POST",
        bAsync: true,
        fnSuccess: sucesso,
        fnBeforeSend: gInicio,
        fnComplete: gComplete
    });
});

function fnDatatableNotificacaoDiariosMonitorados(termos_diarios_monitorados) {
    $("#div_datatable_notificar_acompanhar_diario").dataTablesLight({
        bServerSide: false,
        bSorting: false,
        aoData: termos_diarios_monitorados,
        aoColumns: _columns_notifiqueme_termos_diarios_monitorados,
        sIdTable: 'datatable_notifiqueme_termos_diarios'
    });
}

function fnDatatableNotificacaoCadastroNormas(criacao_normas_monitoradas) {
    $("#div_datatable_notificar_cadastro_normas").dataTablesLight({
        bServerSide: false,
        bSorting: false,
        aoData: criacao_normas_monitoradas,
        aoColumns: _columns_notifiqueme_criacao_normas_monitoradas,
        sIdTable: 'datatable_notifiqueme_cadastro_normas'
    });
}

function fnDatatableNotificacaoNormasMonitoradas(normas_monitoradas) {
    $("#div_datatable_notificar_edicao_normas").dataTablesLight({
        bServerSide: false,
        bSorting: false,
        aoData: normas_monitoradas,
        aoColumns: _columns_notifiqueme_normas_monitoradas,
        sIdTable: 'datatable_notifiqueme_edicao_normas'
    });
}

function alterarStNormaMonitorada(event, ch_norma_monitorada) {
    var $target = $(event.target);
    var $parent = $(event.target).parent();
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_notificacao_notifiqueme').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
        else if (!IsNotNullOrEmpty(data, 'email_usuario_push')) {
            $('#div_notificacao_notifiqueme').messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro não identificado.",
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
    }
    $.ajaxlight({
        sUrl: './ashx/Push/NotifiquemeNormaEditar.ashx?ch_norma_monitorada=' + ch_norma_monitorada + '&st_norma_monitorada=' + ($target.prop('checked') ? "1" : "0"),
        sType: "GET",
        bAsync: true,
        fnSuccess: sucesso,
        fnBeforeSend: gInicio,
        fnComplete: gComplete
    });
}

function alterarStCriacaoNormaMonitorada(event, ch_criacao_norma_monitorada) {
    var $target = $(event.target);
    var $parent = $(event.target).parent();
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_notificacao_notifiqueme').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
        else if (!IsNotNullOrEmpty(data, 'email_usuario_push')) {
            $('#div_notificacao_notifiqueme').messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro não identificado.",
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
    }
    $.ajaxlight({
        sUrl: './ashx/Push/NotifiquemeCriacaoNormaEditar.ashx?ch_criacao_norma_monitorada=' + ch_criacao_norma_monitorada + '&st_criacao=' + ($target.prop('checked') ? "1" : "0"),
        sType: "GET",
        bAsync: true,
        fnSuccess: sucesso,
        fnBeforeSend: gInicio,
        fnComplete: gComplete
    });
}

function RemoverNormaMonitorada(event, ch_norma) {
    var $target = $(event.target);
    var $parent = $(event.target).closest('td');
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_notificacao_notifiqueme').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
        else if (IsNotNullOrEmpty(data, 'email_usuario_push')) {
            fnDatatableNotificacaoNormasMonitoradas(data.normas_monitoradas);
        }
        else {
            $('#div_notificacao_notifiqueme').messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro não identificado.",
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
    }
    $.ajaxlight({
        sUrl: './ashx/Push/NotifiquemeNormaExcluir.ashx?ch_norma=' + ch_norma,
        sType: "GET",
        bAsync: true,
        fnSuccess: sucesso,
        fnBeforeSend: gInicio,
        fnComplete: gComplete
    });
}

function CriarModalNotificarCadastroNormas() {
    $("#modal_notificar_cadastro_normas").modallight({
        sTitle: "Adicionar Monitoramento",
        sWidth: '800',
        oButtons: [
            {
                text: "Salvar",
                click: function () {
                    var ch_tipo_norma = $('#ch_tipo_norma_criacao').val();
                    var nm_tipo_norma = $('#nm_tipo_norma_criacao').val();
                    var primeiro_conector = $('#primeiro_conector_criacao').val();
                    var ch_orgao = $('#ch_orgao_criacao').val();
                    var nm_orgao = $('#nm_orgao_criacao').val();
                    var segundo_conector = $('#segundo_conector_criacao').val();
                    var ch_termo = $('#ch_termo_criacao').val();
                    var ch_tipo_termo = $('#ch_tipo_termo_criacao').val();
                    var nm_termo = $('#nm_termo_criacao').val();
                    var st_criacao = $('#st_criacao').is(":checked");

                    if (ValidarCriacaoNormasCadastradas()){
                        if (IsNotNullOrEmpty(ch_tipo_norma) || IsNotNullOrEmpty(ch_orgao) || IsNotNullOrEmpty(ch_termo)) {
                            var sucesso = function (data) {
                                if (IsNotNullOrEmpty(data, 'error_message')) {
                                    $('#div_notificacao_notificar_cadastro_normas').messagelight({
                                        sTitle: "Erro",
                                        sContent: data.error_message,
                                        sType: "error",
                                        sWidth: "",
                                        iTime: null
                                    });
                                }
                                else if (IsNotNullOrEmpty(data, 'criacao_normas_monitoradas')) {
                                    fnDatatableNotificacaoCadastroNormas(data.criacao_normas_monitoradas);
                                }
                                else {
                                    $('#div_notificacao_notificar_cadastro_normas').messagelight({
                                        sTitle: "Erro",
                                        sContent: "Ocorreu um erro não identificado.",
                                        sType: "error",
                                        sWidth: "",
                                        iTime: null
                                    });
                                }
                            }
                            $.ajaxlight({
                                sUrl: './ashx/Push/NotifiquemeCriacaoNormaIncluir.ashx?ch_tipo_norma=' + ch_tipo_norma + '&nm_tipo_norma=' + nm_tipo_norma + '&primeiro_conector=' + primeiro_conector + '&ch_orgao=' + ch_orgao + '&nm_orgao=' + nm_orgao + '&segundo_conector=' + segundo_conector + '&ch_termo=' + ch_termo + '&ch_tipo_termo=' + ch_tipo_termo + '&nm_termo=' + nm_termo + '&st_criacao=' + st_criacao,
                                sType: "GET",
                                bAsync: true,
                                fnSuccess: sucesso,
                                fnBeforeSend: gInicio,
                                fnComplete: gComplete
                            });
                            $(this).dialog('close');
                        }
                        else {
                            $('#modal_notificar_cadastro_normas_notificacao').messagelight({
                                sTitle: "Erro",
                                sContent: "Ocorreu um erro não identificado.",
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                        }
                    }
                }
            },
            {
                text: "Cancelar",
                click: function () {
                    $(this).dialog('close');
                }
            }
        ],
        fnClose: function () {
            LimparModal('modal_notificar_cadastro_normas');
        }
    });
}


function ValidarCriacaoNormasCadastradas() {
    var ch_tipo_norma = $('#ch_tipo_norma_criacao').val();
    var nm_tipo_norma = $('#nm_tipo_norma_criacao').val();
    var primeiro_conector = $('#primeiro_conector_criacao').val();
    var ch_orgao = $('#ch_orgao_criacao').val();
    var nm_orgao = $('#nm_orgao_criacao').val();
    var segundo_conector = $('#segundo_conector_criacao').val();
    var ch_termo = $('#ch_termo_criacao').val();
    var ch_tipo_termo = $('#ch_tipo_termo_criacao').val();
    var nm_termo = $('#nm_termo_criacao').val();
    var st_criacao = $('#st_criacao').is(":checked");

    // A diferença de parametros e conectores deve sempre ser igual a 1.
    var conectores = 0;
    var parametros = 0;
    if (IsNotNullOrEmpty(ch_tipo_norma)) { parametros++ };
    if (IsNotNullOrEmpty(ch_orgao)) { parametros++ };
    if (IsNotNullOrEmpty(ch_termo)) { parametros++ };
    if (IsNotNullOrEmpty(primeiro_conector)) { conectores++ };
    if (IsNotNullOrEmpty(segundo_conector)) { conectores++ };
    try {
        if ((parametros - conectores) != 1) {
            throw "Erro na quantidade de parâmetros e conectores.";
        }
        if (IsNotNullOrEmpty(primeiro_conector)) {
            if (!IsNotNullOrEmpty(ch_tipo_norma)) {
                throw "Verifique o primeiro conector.";
            }
        }
        if (IsNotNullOrEmpty(segundo_conector)) {
            if (!IsNotNullOrEmpty(ch_termo)) {
                throw "Verifique o segundo conector.";
            }
        }
        return true;
    }
    catch(error){
        $('#modal_notificar_cadastro_normas_notificacao').messagelight({
            sTitle: "Erro",
            sContent: "Preencha os campos corretamente.",
            sType: "error",
            sWidth: "",
            iTime: null
        });
        return false;
    }
}

function EditarCriacaoNormaMonitorada(event, sCriacao_normas_monitoradas, ch_criacao_norma_monitorada) {
    if (IsNotNullOrEmpty(sCriacao_normas_monitoradas)) {
        var criacao_normas_monitoradas_split = sCriacao_normas_monitoradas.split('#');

        var ch_tipo_norma = criacao_normas_monitoradas_split[0];
        var nm_tipo_norma = criacao_normas_monitoradas_split[1];
        var primeiro_conector = criacao_normas_monitoradas_split[2];
        var ch_orgao = criacao_normas_monitoradas_split[3];
        var nm_orgao = criacao_normas_monitoradas_split[4];
        var segundo_conector = criacao_normas_monitoradas_split[5];
        var ch_termo = criacao_normas_monitoradas_split[6];
        var ch_tipo_termo = criacao_normas_monitoradas_split[7];
        var nm_termo = criacao_normas_monitoradas_split[8];
        var st_criacao = criacao_normas_monitoradas_split[9];

        $('#ch_criacao_norma_monitorada').val(ch_criacao_norma_monitorada);
        $('#ch_tipo_norma_criacao').val(ch_tipo_norma);
        $('#nm_tipo_norma_criacao').val(nm_tipo_norma);
        $('#primeiro_conector_criacao').val(primeiro_conector);
        $('#ch_orgao_criacao').val(ch_orgao);
        $('#nm_orgao_criacao').val(nm_orgao);
        $('#segundo_conector_criacao').val(segundo_conector);
        $('#ch_termo_criacao').val(ch_termo);
        $('#ch_tipo_termo_criacao').val(ch_tipo_termo);
        $('#nm_termo_criacao').val(nm_termo);

        $('#form_notificar_cadastro_normas').attr('url-ajax', './ashx/Push/NotifiquemeCriacaoNormaEditar.ashx');
        $('#form_notificar_cadastro_normas button.add').hide();
        $('#form_notificar_cadastro_normas button.edit').show();
    }
}

function RemoverCriacaoNormaMonitorada(event, ch_criacao_norma_monitorada) {
    var $target = $(event.target);
    var $parent = $(event.target).closest('td');
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_notificacao_notificar_cadastro_normas').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
        else if (IsNotNullOrEmpty(data, 'email_usuario_push')) {
            fnDatatableNotificacaoCadastroNormas(data.criacao_normas_monitoradas);
        }
    }
    $.ajaxlight({
        sUrl: './ashx/Push/NotifiquemeCriacaoNormaExcluir.ashx?ch_criacao_norma_monitorada='+ ch_criacao_norma_monitorada,
        sType: "GET",
        bAsync: true,
        fnSuccess: sucesso,
        fnBeforeSend: gInicio,
        fnComplete: gComplete
    });
}

function CriarModalConfirmacaoRemoverNormaMonitorada(event, ch_norma) {
	$('#modal_confirmacao_excluir').modallight({
        sTitle: "Confirmação",
        sWidth: '350',
		sContent: 'Deseja realmente parar de monitorar essa norma?',
        oButtons: [
            {
                text: "Confirmar",
                click: function () {
                	RemoverNormaMonitorada(event, ch_norma);
                    $(this).dialog('close');
                }
            },
            {
                text: "Cancelar",
                click: function () {
                    $(this).dialog('close');
                }
            }
        ],
        fnClose: function () {
            LimparModal('modal_confirmacao_excluir');
        }
	})
}

function CriarModalConfirmacaoRemoverCriacaoNormaMonitorada(event, ch_criacao_norma_monitorada) {
	$('#modal_confirmacao_excluir').modallight({
        sTitle: "Confirmação",
        sWidth: '350',
		sContent: 'Deseja realmente parar de verificar novas normas com esses parâmetros?',
        oButtons: [
            {
                text: "Confirmar",
                click: function () {
                    RemoverCriacaoNormaMonitorada(event, ch_criacao_norma_monitorada);
                    $(this).dialog('close');
                }
            },
            {
                text: "Cancelar",
                click: function () {
                    $(this).dialog('close');
                }
            }
        ],
        fnClose: function () {
            LimparModal('modal_confirmacao_excluir');
        }
	})
}

function RemoverTermoDiarioMonitorado(event, ch_termo_diario_monitorado) {
    var $target = $(event.target);
    var $parent = $(event.target).closest('td');
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_notificacao_notificar_acompanhar_diario').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
        else if (IsNotNullOrEmpty(data, 'email_usuario_push')) {
            fnDatatableNotificacaoDiariosMonitorados(data.termos_diarios_monitorados);
        }
    }
    $.ajaxlight({
        sUrl: './ashx/Push/NotifiquemeTermoDiarioExcluir.ashx?ch_termo_diario_monitorado=' + ch_termo_diario_monitorado,
        sType: "GET",
        bAsync: true,
        fnSuccess: sucesso,
        fnBeforeSend: gInicio,
        fnComplete: gComplete
    });
}

function CriarModalConfirmacaoRemoverTermoDiarioMonitorado(event, ch_termo_diario_monitorado) {
    $('#modal_confirmacao_excluir').modallight({
        sTitle: "Confirmação",
        sWidth: '350',
        sContent: 'Deseja realmente parar de monitorar esses critérios?',
        oButtons: [
            {
                text: "Confirmar",
                click: function () {
                    RemoverTermoDiarioMonitorado(event, ch_termo_diario_monitorado);
                    $(this).dialog('close');
                }
            },
            {
                text: "Cancelar",
                click: function () {
                    $(this).dialog('close');
                }
            }
        ],
        fnClose: function () {
            LimparModal('modal_confirmacao_excluir');
        }
    })
}

function CriarModalPesquisarNormasMonitorada() {
    $('#modal_pesquisar_normas_monitoradas').modallight({
        sTitle: "Pesquisar Norma",
        sWidth: '800',
        oButtons: [],
        fnClose: function () {
            LimparModal('modal_pesquisar_normas_monitoradas');
            $('#datatable_normas_modal').html("");
        }
    })
}

function PesquisarNorma() {
    $("#datatable_normas_modal").dataTablesLight({
        sAjaxUrl: './ashx/Datatable/ResultadoDePesquisaDatatable.ashx?tipo_pesquisa=norma&bbusca=sinj_norma&ch_tipo_norma=' + $('#ch_tipo_norma_modal').val() + '&nr_norma=' + $('#nr_norma_modal').val() + '&dt_assinatura=' + $('#dt_assinatura_modal').val(),
        aoColumns: _columns_norma_notifiqueme,
        sIdTable: 'table_normas_modal'
    });
}

function adicionarTermoDiario(id_form) {
    var sucessoAdicionarTermoDiario = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            notificar('#form_termo_acompanhar_diario', data.error_message, 'error');
        } else if (IsNotNullOrEmpty(data, "email_usuario_push")) {
            notificar('#form_termo_acompanhar_diario', 'Critérios para monitoramento salvo com sucesso!', 'success');
            fnDatatableNotificacaoDiariosMonitorados(data.termos_diarios_monitorados);
        }
        $('#' + id_form).attr('url-ajax', './ashx/Push/NotifiquemeTermoDiarioIncluir.ashx');
        fnCancelar(id_form);
    }
    fnSalvarForm(id_form, sucessoAdicionarTermoDiario);
    return false;
}

function adicionarMonitoramentoDeCadastroDeNorma(id_form) {
    var sucessoadicionarMonitoramentoDeCadastroDeNorma = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            notificar('#form_notificar_cadastro_normas', data.error_message, 'error');
        } else if (IsNotNullOrEmpty(data, "email_usuario_push")) {
            notificar('#form_notificar_cadastro_normas', 'Critérios para monitoramento salvo com sucesso!', 'success');
            fnDatatableNotificacaoCadastroNormas(data.criacao_normas_monitoradas);
        }
        $('#' + id_form).attr('url-ajax', './ashx/Push/NotifiquemeCriacaoNormaIncluir.ashx');
        fnCancelar(id_form);
    }
    fnSalvarForm(id_form, sucessoadicionarMonitoramentoDeCadastroDeNorma);
    return false;
}

function fnCancelar(id_form) {
    $('#'+id_form)[0].reset();
    $('#'+id_form).find('button.add').show();
    $('#'+id_form).find('button.edit').hide();
    $('#'+id_form).find('input[type="hidden"]').val('');
}

function alterarStTermoDiarioMonitorado(event, ch_termo_diario_monitorado) {
    var $target = $(event.target);
    var $parent = $(event.target).parent();
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_notificacao_notificar_acompanhar_diario').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
    }
    $.ajaxlight({
        sUrl: './ashx/Push/NotifiquemeTermoDiarioEditar.ashx?ch_termo_diario_monitorado=' + ch_termo_diario_monitorado + '&st_termo_diario_monitorado=' + ($target.prop('checked') ? "1" : "0"),
        sType: "GET",
        bAsync: true,
        fnSuccess: sucesso,
        fnBeforeSend: gInicio,
        fnComplete: gComplete
    });
}

function editarTermoDiarioMonitorado(event, sTermoDiarioMonitorado, ch_termo_diario_monitorado) {
    if (IsNotNullOrEmpty(sTermoDiarioMonitorado)) {
        var termoDiarioMonitoradoSplited = sTermoDiarioMonitorado.split('#');

        var ch_tipo_fonte_diario_monitorado = termoDiarioMonitoradoSplited[0];
        var nm_tipo_fonte_diario_monitorado = termoDiarioMonitoradoSplited[1];
        var ds_termo_diario_monitorado = termoDiarioMonitoradoSplited[2];
        
        $('#ch_termo_diario_monitorado').val(ch_termo_diario_monitorado);
        $('#ch_tipo_fonte_diario_monitorado').val(ch_tipo_fonte_diario_monitorado);
        $('#nm_tipo_fonte_diario_monitorado').val(nm_tipo_fonte_diario_monitorado);
        $('#ds_termo_diario_monitorado').val(ds_termo_diario_monitorado);

        $('#form_termo_acompanhar_diario').attr('url-ajax', './ashx/Push/NotifiquemeTermoDiarioEditar.ashx');
        $('#form_termo_acompanhar_diario button.add').hide();
        $('#form_termo_acompanhar_diario button.edit').show();
    }
}

function selecionarNorma(ch_norma) {
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            notificar('#div_notificar_edicao_normas', data.error_message, 'error');
        } else if (IsNotNullOrEmpty(data, "email_usuario_push")) {
            notificar('#div_notificar_edicao_normas', 'Norma adicionada com sucesso!', 'success');
            fnDatatableNotificacaoNormasMonitoradas(data.normas_monitoradas);
        }
        $("#modal_pesquisar_normas_monitoradas").modallight('close');
    }
    $.ajaxlight({
        sUrl: "./ashx/Push/NotifiquemeNormaIncluir.ashx?ch_norma=" + ch_norma,
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: gComplete,
        fnBeforeSend: gInicio,
        bAsync: true,
        iTimeout: 40000
    });
}