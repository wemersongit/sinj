var notifiquemeOv = null;
$(document).ready(function () {
    $('.tabs').tabs();
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, "email_usuario_push")) {
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
    var inicio = function () {
        $('#div_notifiqueme').hide();
        $('#div_loading_notifiqueme').show();
    }
    var complete = function () {
        $('#div_notifiqueme').show();
        $('#div_loading_notifiqueme').hide();
    }
    $.ajaxlight({
        sUrl: './ashx/Push/NotifiquemeDetalhes.ashx',
        sType: "POST",
        bAsync: true,
        fnSuccess: sucesso,
        fnBeforeSend: inicio,
        fnComplete: complete
    });
});

function fnDatatableNotificacaoCadastroNormas(criacao_normas_monitoradas) {
    $("#div_datatable_notificar_cadastro_normas").dataTablesLight({
        bServerSide: false,
        aoData: criacao_normas_monitoradas,
        aoColumns: _columns_notifiqueme_criacao_normas_monitoradas,
        sIdTable: 'datatable_notifiqueme_cadastro_normas'
    });
}

function fnDatatableNotificacaoNormasMonitoradas(normas_monitoradas) {
    $("#div_datatable_notificar_edicao_normas").dataTablesLight({
        bServerSide: false,
        aoData: normas_monitoradas,
        aoColumns: _columns_notifiqueme_normas_monitoradas,
        sIdTable: 'datatable_notifiqueme_edicao_normas'
    });
}

function AlterarStNormaMonitorada(event, ch_norma) {
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
        else if (!IsNotNullOrEmpty(data, 'id_doc_success')) {
            $('#div_notificacao_notifiqueme').messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro não identificado.",
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
    }
    var inicio = function () {
        $('input[type="checkbox"]', $parent).hide();
        $('.loading-p', $parent).show();
    }
    var complete = function () {
        $('input[type="checkbox"]', $parent).show();
        $('.loading-p', $parent).hide();
    }
    $.ajaxlight({
        sUrl: './ashx/Push/NotifiquemeNormaEditar.ashx?ch_norma=' + ch_norma + '&path=st_norma_monitorada&value=' + $target.prop('checked'),
        sType: "GET",
        bAsync: true,
        fnSuccess: sucesso,
        fnBeforeSend: inicio,
        fnComplete: complete
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
        else if (IsNotNullOrEmpty(data, 'id_doc_success')) {
            $('#datatable_notifiqueme_edicao_normas tr.selected').remove();
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
    var inicio = function () {
        $('a', $parent).hide();
        $('.loading-p', $parent).show();
    }
    var complete = function () {
        $('a', $parent).show();
        $('.loading-p', $parent).hide();
    }
    $.ajaxlight({
        sUrl: './ashx/Push/NotifiquemeNormaExcluir.ashx?ch_norma=' + ch_norma,
        sType: "GET",
        bAsync: true,
        fnSuccess: sucesso,
        fnBeforeSend: inicio,
        fnComplete: complete
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
                            var inicio = function () {
                                $('#div_notificacao_notificar_cadastro_normas').html('');
                                $('#div_notificacao_notificar_cadastro_normas').hide();
                                $('#div_notificar_cadastro_normas').hide();
                                $('#div_loading_notificar_cadastro_normas').show();
                            }
                            var complete = function () {
                                $('#div_notificar_cadastro_normas').show();
                                $('#div_loading_notificar_cadastro_normas').hide();
                            }
                            $.ajaxlight({
                                sUrl: './ashx/Push/NotifiquemeCriacaoNormaIncluir.ashx?ch_tipo_norma=' + ch_tipo_norma + '&nm_tipo_norma=' + nm_tipo_norma + '&primeiro_conector=' + primeiro_conector + '&ch_orgao=' + ch_orgao + '&nm_orgao=' + nm_orgao + '&segundo_conector=' + segundo_conector + '&ch_termo=' + ch_termo + '&ch_tipo_termo=' + ch_tipo_termo + '&nm_termo=' + nm_termo + '&st_criacao=' + st_criacao,
                                sType: "GET",
                                bAsync: true,
                                fnSuccess: sucesso,
                                fnBeforeSend: inicio,
                                fnComplete: complete
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
    $("#modal_notificar_cadastro_normas").modallight({
        sTitle: "Editar Monitoramento",
        sWidth: '800',
        fnCreated: function () {
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
                st_criacao = (st_criacao == "true"); // O parametro vem como string. Precisa ser convertido para boolean

                $('#ch_tipo_norma_criacao').val(ch_tipo_norma);
                $('#nm_tipo_norma_criacao').val(nm_tipo_norma);
                $('#primeiro_conector_criacao').val(primeiro_conector);
                $('#ch_orgao_criacao').val(ch_orgao);
                $('#nm_orgao_criacao').val(nm_orgao);
                $('#segundo_conector_criacao').val(segundo_conector);
                $('#ch_termo_criacao').val(ch_termo);
                $('#ch_tipo_termo_criacao').val(ch_tipo_termo);
                $('#nm_termo_criacao').val(nm_termo);
                $('#st_criacao').prop("checked", st_criacao);
            }
        },
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
                        var inicio = function () {
                            $('#div_notificacao_notificar_cadastro_normas').html('');
                            $('#div_notificacao_notificar_cadastro_normas').hide();
                            $('#div_notificar_cadastro_normas').hide();
                            $('#div_loading_notificar_cadastro_normas').show();
                        }
                        var complete = function () {
                            $('#div_notificar_cadastro_normas').show();
                            $('#div_loading_notificar_cadastro_normas').hide();
                        };
                        var criacao_norma_novo = {
                            'ch_tipo_norma_novo': ch_tipo_norma,
                            'nm_tipo_norma_novo': nm_tipo_norma,
                            'primeiro_conector_novo': primeiro_conector,
                            'ch_orgao_novo': ch_orgao,
                            'nm_orgao_novo': nm_orgao,
                            'segundo_conector_novo': segundo_conector,
                            'ch_termo_novo': ch_termo,
                            'ch_tipo_termo_novo': ch_tipo_termo,
                            'nm_termo_novo': nm_termo,
                            'st_criacao_novo': st_criacao
                        };
                        $.ajaxlight({
                            sUrl: './ashx/Push/NotifiquemeCriacaoNormaEditar.ashx?ch_criacao_norma_monitorada=' + ch_criacao_norma_monitorada,
                            sType: "POST",
                            oData: criacao_norma_novo,
                            bAsync: true,
                            fnSuccess: sucesso,
                            fnBeforeSend: inicio,
                            fnComplete: complete
                        });
                        $(this).dialog('close');
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
        else if (IsNotNullOrEmpty(data, 'id_doc_success')) {
            $('#div_datatable_notificar_cadastro_normas tr.selected').remove();
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
    var inicio = function () {
        $('a', $parent).hide();
        $('.loading-p', $parent).show();
    }
    var complete = function () {
        $('a', $parent).show();
        $('.loading-p', $parent).hide();
    }
    $.ajaxlight({
        sUrl: './ashx/Push/NotifiquemeCriacaoNormaExcluir.ashx?ch_criacao_norma_monitorada='+ ch_criacao_norma_monitorada,
        sType: "GET",
        bAsync: true,
        fnSuccess: sucesso,
        fnBeforeSend: inicio,
        fnComplete: complete
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
