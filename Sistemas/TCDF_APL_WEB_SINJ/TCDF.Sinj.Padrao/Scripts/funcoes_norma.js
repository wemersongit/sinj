

function Notificar(ch_norma) {
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_notificacao_norma').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
        else if (IsNotNullOrEmpty(data, 'email_usuario_push')) {
            $("<div id='modal_notifiqueme' />").modallight({
                sTitle: "Notifique-me",
                sType: "success",
                sContent: "Norma monitorada com sucesso.",
                oButtons: [
                    {
                        text: "Ok",
                        click: function () {
                            $(this).dialog('close');
                        }
                    }
                ],
                fnClose: function () {
                    document.location.reload();
                }
            });
        }
        else {
            $('#div_notificacao_norma').messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro não identificado.",
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
    }
    var inicio = function () {
        $('#div_loading_norma').show();
        $('#div_norma').hide();
    }
    var complete = function () {
        gComplete();
        if (!IsNotNullOrEmpty(_notifiqueme)) {
            $("<div id='modal_notifiqueme' />").modallight({
                sTitle: "Notifique-me",
                sType: "alert",
                sContent: "Para seguir esta norma, é necessário fazer login no Notifique-me.",
                oButtons: [
                        {
                            text: "Ok",
                            click: function () {
                                var pagina_atual = document.URL;
                                var pagina_atual_array = pagina_atual.split("/");
                                pagina_atual = pagina_atual_array[pagina_atual_array.length - 1];
                                pagina_atual = pagina_atual.replace(/\?/g, "__QUERY__");
                                pagina_atual = pagina_atual.replace(/\&/g, "__AND__");
                                pagina_atual = pagina_atual.replace(/\=/g, "__EQUAL__");
                                document.location.href = "./LoginNotifiqueme.aspx?redirecionar_notifiqueme=" + pagina_atual + "__AND__ch_norma__EQUAL__" + ch_norma;
                            }
                        }
                    ]
            });
        }
    }
    $.ajaxlight({
        sUrl: "./ashx/Push/NotifiquemeNormaIncluir.ashx?ch_norma=" + ch_norma,
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: complete,
        fnBeforeSend: gInicio,
        bAsync: true,
        iTimeout: 40000
    });
}

function PararNotificar(ch_norma) {
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_notificacao_norma').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
        else if (IsNotNullOrEmpty(data, 'email_usuario_push')) {
            $("<div id='modal_notifiqueme' />").modallight({
                sTitle: "Notifique-me",
                sType: "success",
                sContent: "A Norma não será mais monitorada.",
                oButtons: [
                    {
                        text: "Ok",
                        click: function () {
                            $(this).dialog('close');
                        }
                    }
                ],
                fnClose: function () {
                    document.location.reload();
                }
            });
        }
        else {
            $('#div_notificacao_norma').messagelight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro não identificado.",
                sType: "error",
                sWidth: "",
                iTime: null
            });
        }
    }
    $.ajaxlight({
        sUrl: "./ashx/Push/NotifiquemeNormaExcluir.ashx?ch_norma=" + ch_norma,
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: gComplete,
        fnBeforeSend: gInicio,
        bAsync: true,
        iTimeout: 40000
    });
}

function fnAutocompleteOrigem() {
    $('#div_autocomplete_origem_modal').autocompletelight({
        sKeyDataName: "ch_orgao",
        sValueDataName: "get_sg_hierarquia_nm_vigencia",
        sInputHiddenName: "ch_orgao",
        sInputName: "sg_hierarquia_nm_vigencia",
        sAjaxUrl: './ashx/Autocomplete/OrgaoAutocomplete.ashx?' + (ValidarOrigem() ? "id_orgao_cadastrador=" + _user.orgao_cadastrador.id_orgao_cadastrador : "") + ($("#checkbox_retroativo").prop("checked") ? '' : '&st_orgao=true'),
//        sAjaxUrl: './ashx/Autocomplete/OrgaoAutocomplete.ashx?' + (isSuperAdmin() ? "" : "id_orgao_cadastrador=" + _user.orgao_cadastrador.id_orgao_cadastrador) + ($("#checkbox_retroativo").prop("checked") ? '' : '&st_orgao=true'),
        bLinkAll: true,
        sLinkName: "a_origem",
        dOthersHidden: [
                        { campo_app: "label_nm_orgao_modal", campo_base: "nm_orgao" },
                        { campo_app: "label_sg_orgao_modal", campo_base: "sg_orgao" },
                        { campo_app: "label_sg_hierarquia_modal", campo_base: "sg_hierarquia" },
                        { campo_app: "label_nm_hierarquia_modal", campo_base: "nm_hierarquia" },
                        { campo_app: "label_in_ambito_modal", campo_base: "ambito.nm_ambito" },
                        { campo_app: "label_dt_inicio_vigencia_modal", campo_base: "dt_inicio_vigencia" },
                        { campo_app: "label_dt_fim_vigencia_modal", campo_base: "dt_fim_vigencia" },
                        { campo_app: "label_orgao_cadastrador_modal", campo_base: "get_orgaos_cadastradores" }
                    ]
    });
}

function fnAutocompleteSituacao(aSituacoes) {
    $('#div_autocomplete_situacao').autocompletelight({
        sKeyDataName: "ch_situacao",
        sValueDataName: "nm_situacao",
        sInputHiddenName: "ch_situacao",
        sInputName: "nm_situacao",
        jData: aSituacoes,
        bLinkAll: true,
        sLinkName: "a_situacao"
    });
}

function ValidarOrigem() {
    // Se o usuário for SuperAdmin, pode cadastrar qualquer origem
    // Se for um ato conjunto, a validação só deve ser feita para o primeiro órgão
    if (!isSuperAdmin() && ($('#tbody_origens input[name="orgao"]').length <= 0)) {
        return true;
    }
} 

function fnAutocompleteVocabulario(element) {
    var inicio_sem_coringa = "0";
    if ($(element).is(':checked')) {
        inicio_sem_coringa = "1"
    }
    $('#div_autocomplete_termo_modal').autocompletelight({
        sKeyDataName: "ch_termo",
        sValueDataName: "nm_termo",
        sInputHiddenName: "ch_termo_modal",
        sInputName: "nm_termo_modal",
        sAjaxUrl: "./ashx/Autocomplete/VocabularioAutocomplete.ashx?cad=true&inicio_sem_coringa="+inicio_sem_coringa,
        bLinkAll: true,
        sLinkName: "a_termo",
        jPagination: { bPaginate: true,
                    iDisplayLength: 10,
                    bButtonsPaginate: true
                },
        dOthersHidden: [
                        { campo_app: "ch_tipo_termo_modal", campo_base: "ch_tipo_termo" }
                    ]
    });
}

function ConstruirControlesDinamicos() {
    SelecionarTipoDeNorma();
    $('#div_autocomplete_tipo_norma').autocompletelight({
        sKeyDataName: "ch_tipo_norma",
        sValueDataName: "nm_tipo_norma",
        sInputHiddenName: "ch_tipo_norma",
        sInputName: "nm_tipo_norma",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeNormaAutocomplete.ashx" + (isSuperAdmin() ? "" : "?id_orgao_cadastrador=" + _user.orgao_cadastrador.id_orgao_cadastrador),
        bLinkAll: true,
        sLinkName: "a_tipo_norma",
        dOthersHidden: [
                            { campo_app: "in_g1", campo_base: "in_g1" },
                            { campo_app: "in_g2", campo_base: "in_g2" },
                            { campo_app: "in_g3", campo_base: "in_g3" },
                            { campo_app: "in_g4", campo_base: "in_g4" },
                            { campo_app: "in_g5", campo_base: "in_g5" },
                            { campo_app: "EhLei", campo_base: "EhLei" },
                            { campo_app: "EhDecreto", campo_base: "EhDecreto" },
                            { campo_app: "EhResolucao", campo_base: "EhResolucao" },
                            { campo_app: "EhPortaria", campo_base: "EhPortaria" },
                            { campo_app: "in_conjunta", campo_base: "in_conjunta" },
                            { campo_app: "in_numeracao_por_orgao", campo_base: "in_numeracao_por_orgao" },
                            { campo_app: "in_apelidavel", campo_base: "in_apelidavel" }
                        ]
        });
    fnAutocompleteOrigem();
    fnAutocompleteVocabulario();
    $('#div_autocomplete_autoria_modal').autocompletelight({
        sKeyDataName: "ch_autoria",
        sValueDataName: "nm_autoria",
        sInputHiddenName: "ch_autoria",
        sInputName: "nm_autoria",
        sAjaxUrl: "./ashx/Autocomplete/AutoriaAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_autoria"
    });
    $('#div_autocomplete_interessado_modal').autocompletelight({
        sKeyDataName: "ch_interessado",
        sValueDataName: "nm_interessado",
        sInputHiddenName: "ch_interessado",
        sInputName: "nm_interessado",
        sAjaxUrl: "./ashx/Autocomplete/InteressadoAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_interessado"
    });
    $('#div_autocomplete_tipo_fonte_modal').autocompletelight({
        sKeyDataName: "ch_tipo_fonte",
        sValueDataName: "nm_tipo_fonte",
        sInputHiddenName: "ch_tipo_fonte_modal",
        sInputName: "nm_tipo_fonte_modal",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeFonteAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_fonte"
    });
    $('#div_autocomplete_tipo_edicao_modal').autocompletelight({
        sKeyDataName: "ch_tipo_edicao",
        sValueDataName: "nm_tipo_edicao",
        sInputHiddenName: "ch_tipo_edicao_modal",
        sInputName: "nm_tipo_edicao_modal",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeEdicaoAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_edicao"
    });
    $('#div_autocomplete_tipo_publicacao_modal').autocompletelight({
        sKeyDataName: "ch_tipo_publicacao",
        sValueDataName: "nm_tipo_publicacao",
        sInputHiddenName: "ch_tipo_publicacao_modal",
        sInputName: "nm_tipo_publicacao_modal",
        sAjaxUrl: "./ashx/Autocomplete/TipoDePublicacaoAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_publicacao"
    });
    $('#div_autocomplete_requerente_modal').autocompletelight({
        sKeyDataName: "ch_requerente",
        sValueDataName: "nm_requerente",
        sInputHiddenName: "ch_requerente_modal",
        sInputName: "nm_requerente_modal",
        sAjaxUrl: "./ashx/Autocomplete/RequerenteAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_requerente"
    });
    $('#div_autocomplete_requerido_modal').autocompletelight({
        sKeyDataName: "ch_requerido",
        sValueDataName: "nm_requerido",
        sInputHiddenName: "ch_requerido_modal",
        sInputName: "nm_requerido_modal",
        sAjaxUrl: "./ashx/Autocomplete/RequeridoAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_requerido"
    });
    $('#div_autocomplete_procurador_modal').autocompletelight({
        sKeyDataName: "ch_procurador",
        sValueDataName: "nm_procurador",
        sInputHiddenName: "ch_procurador_modal",
        sInputName: "nm_procurador_modal",
        sAjaxUrl: "./ashx/Autocomplete/ProcuradorAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_procurador"
    });
    $('#div_autocomplete_relator_modal').autocompletelight({
        sKeyDataName: "ch_relator",
        sValueDataName: "nm_relator",
        sInputHiddenName: "ch_relator_modal",
        sInputName: "nm_relator_modal",
        sAjaxUrl: "./ashx/Autocomplete/RelatorAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_relator"
    });
    ConstruirAmbitos();
    $("#tbody_termos_modal").sortable({
        containment: "parent",
		forceHelperSize: true
    });
    $("#tbody_indexacoes").sortable({
        containment: "parent"
    });
    MarcarPendente();
}

function EditarFonte(event) {
    var tr_fonte = $(event.target).closest('tr');
    var sFonte = $($('input[name="fonte"]', $(tr_fonte))[0]).val();
    var oFonte = JSON.parse(sFonte);

    var oJsonDiario = {
        ch_tipo_fonte: oFonte.ch_tipo_fonte,
        nm_tipo_fonte: oFonte.nm_tipo_fonte,
        dt_assinatura: oFonte.dt_publicacao
    };

    $('#json_diario_fonte').val(JSON.stringify(oJsonDiario));
    $('#json_arquivo_diario_fonte').val(JSON.stringify(oFonte.ar_diario));
    $('#ds_diario').val(oFonte.ds_diario);
        
    $('#ch_tipo_publicacao_modal').val(oFonte.ch_tipo_publicacao);
    $('#nm_tipo_publicacao_modal').val(oFonte.nm_tipo_publicacao);

    $('#nr_pagina_modal').val(GetText(oFonte.nr_pagina));
    $('#nr_coluna_modal').val(GetText(oFonte.nr_coluna));
    $('#ds_observacao_fonte_modal').val(GetText(oFonte.ds_observacao_fonte));
    $('#ds_republicacao_modal').val(GetText(oFonte.ds_republicacao));
    CriarModalFonte(tr_fonte);
}

function CriarModalFonte(tr_fonte) {
    if (!IsNotNullOrEmpty(tr_fonte)) {
        $('#modal_fonte .diario').hide();
    }
    else {
        $('#modal_fonte .diario').show();
    }
    $('#modal_fonte .consultar').show();
    $('#modal_fonte .resultado').hide();

    $("#modal_fonte").modallight({
        sTitle: "Adicionar Fonte",
        sWidth: '620',
        oButtons: [
            {
                text: "Salvar",
                click: function () {
                    var oFonte = {};
                    oFonte.ch_tipo_publicacao = $('#ch_tipo_publicacao_modal').val();
                    oFonte.nm_tipo_publicacao = $('#nm_tipo_publicacao_modal').val();
                    oFonte.nr_pagina = $('#nr_pagina_modal').val();
                    oFonte.nr_coluna = $('#nr_coluna_modal').val();
                    oFonte.ds_observacao_fonte = $('#ds_observacao_fonte_modal').val();
                    oFonte.ds_republicacao = $('#ds_republicacao_modal').val();


                    var sJson_diario = $('#json_diario_fonte').val();

                    if(IsNotNullOrEmpty(sJson_diario)){
                        var oJson_diario = JSON.parse(sJson_diario);

                        oFonte.ch_tipo_fonte = oJson_diario.ch_tipo_fonte;
                        oFonte.nm_tipo_fonte = oJson_diario.nm_tipo_fonte;
                        oFonte.dt_publicacao = oJson_diario.dt_assinatura;
                        oFonte.ds_diario = $('#ds_diario').val();
                        var sJson_arquivo_diario = $('#json_arquivo_diario_fonte').val();
                        if(IsNotNullOrEmpty(sJson_arquivo_diario)){
                            oFonte.ar_diario = JSON.parse(sJson_arquivo_diario);
                        }
                    }

                    //Fonte não pode data de publicação anterior à data de assinatura da norma
                    if (IsNotNullOrEmpty(oFonte, 'dt_publicacao')) {
                        var date_publicacao = convertStringToDateTime(oFonte.dt_publicacao);
                        var date_assinatura = convertStringToDateTime($('#dt_assinatura').val());
                        if (date_publicacao < date_assinatura) {
                            $('#modal_fonte .notify').messagelight({
                                sTitle: "Erro",
                                sContent: "Fonte não pode possuir data de publicação anterior à data de assinatura da norma.",
                                sType: "error",
                                sWidth: "",
                                iTime: null
                            });
                            return false;
                        }
                    }

                    if (((IsNotNullOrEmpty(oFonte, 'ds_diario') && IsNotNullOrEmpty(oFonte, 'ar_diario')) || $('#ch_tipo_norma').val() == '11000000' ) && IsNotNullOrEmpty(oFonte, 'ch_tipo_fonte') && IsNotNullOrEmpty(oFonte, 'ch_tipo_publicacao')) {
                        $('#tbody_fontes .tr_vazia').remove();
                        var guid = Guid('N');
                        if (IsNotNullOrEmpty(tr_fonte)) {
                            $($(tr_fonte).find('td')[0]).text(oFonte.ds_diario);
                            $($(tr_fonte).find('td')[1]).text(oFonte.nm_tipo_publicacao);
                            $($(tr_fonte).find('td')[2]).text(oFonte.nr_pagina);
                            $($(tr_fonte).find('td')[3]).text(oFonte.nr_coluna);
                            $($(tr_fonte).find('td')[4]).text(oFonte.ds_observacao_fonte);
                            $($(tr_fonte).find('td')[5]).text(oFonte.ds_republicacao);
                            var json_arquivo = $('input.json_arquivo', $(tr_fonte).find('td')[6]).val();
                            if (IsNotNullOrEmpty(json_arquivo)) {
                                oFonte.ar_fonte = JSON.parse(json_arquivo);
                            }
                            $('input[name=fonte]', $(tr_fonte).find('td')[6]).val(JSON.stringify(oFonte));
                            $('a[ds_diario]', $(tr_fonte).find('td')[6]).attr('ds_diario', oFonte.ds_diario);
                        }
                        else {
                            var id_input = 'input_' + guid;
                            var id_td = 'td_' + guid;
                            var html_fonte = '<td>' + oFonte.ds_diario + '</td>' +
                                '<td>' + oFonte.nm_tipo_publicacao + '</td>' +
                                '<td>' + oFonte.nr_pagina + '</td>' +
                                '<td>' + oFonte.nr_coluna + '</td>' +
                                '<td>' + oFonte.ds_observacao_fonte + '</td>' +
                                '<td>' + oFonte.ds_republicacao + '</td>' +
                                '<td id="' + id_td + '">' +
                                    '<input id="' + id_input + '" type="hidden" name="fonte" value="" />' +
                                    '<input type="hidden" class="json_arquivo" value="" />' +
                                    '<label class="name" style="color:#000;"></label>' +
                                    '<a href="javascript:void(0);" onclick="javascript:anexarInputFile(this);" class="attach" title="Anexar um arquivo" ><img valign="absmiddle" alt="Anexar" src="' + _urlPadrao + '/Imagens/ico_attach_file2.png" width="18px" /></a>' +
                                    '<a href="javascript:void(0);" onclick="javascript:deletarInputFile(this);" class="delete" title="Remover o arquivo" style="display:none;"><img valign="absmiddle" alt="Remover" src="' + _urlPadrao + '/Imagens/ico_delete_file.png" width="18px" /></a>' +
                                    '<a path="ar_fonte" href="javascript:void(0);" onclick="javascript:editarInputFile(this);" class="create" title="Editar ou criar um arquivo"><img valign="absmiddle" alt="Editar" src="' + _urlPadrao + '/Imagens/ico_edit_file.png" width="18px" /></a>' +
                                    '<a path="ar_fonte" publicacao="' + oFonte.nm_tipo_publicacao + '" ds_diario="' + oFonte.ds_diario + '" div_arquivo="' + id_td + '" href="javascript:void(0);" onclick="javascript:abrirModalImportarArquivo(this);" class="import" title="Importar arquivo do módulo de arquivos"><img width="18px" valign="absmiddle" alt="Editar" src="' + _urlPadrao + '/Imagens/ico_import_file.png" /></a>' +
                                '</td>' +
                                '<td>' +
                                    '<a title="Editar Fonte" href="javascript:void(0);" onclick="javascript:EditarFonte(event);"><img valign="absmiddle" alt="Editar" src="' + _urlPadrao + '/Imagens/ico_pencil_p.png"  /></a>' +
                                    '<a title="Excluir Fonte" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                                '</td>';
                            $('#tbody_fontes').append('<tr>' + html_fonte + '</tr>');
                            $('#' + id_input).val(JSON.stringify(oFonte));
                        }
                        $(this).dialog('close');
                    }
                    else {
                        $('#modal_fonte .notify').messagelight({
                            sTitle: "Erro",
                            sContent: "Preencha os campos obrigatórios.",
                            sType: "error",
                            sWidth: "",
                            iTime: null
                        });
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
            LimparModal('modal_fonte');
        }
    });
}

function onblurTipoDeFonte(){
    if($('#ch_tipo_norma').val() == '11000000' && $('#ch_tipo_fonte_modal').val() == '18'){
        $('#buttonProsseguirSemDiario').show();
    }
    else{
        $('#buttonProsseguirSemDiario').hide();
    }
}

function prosseguirSemDiario(){
    if(!IsNotNullOrEmpty($('#ch_tipo_fonte_modal').val()) || !IsNotNullOrEmpty($('#dt_assinatura_fonte_modal').val())){
        Validar('form_consultar_diario');
        return false;
    }
    $('#json_diario_fonte').val('{"ch_tipo_fonte": "' + $('#ch_tipo_fonte_modal').val() + '", "nm_tipo_fonte": "' + $('#nm_tipo_fonte_modal').val() + '", "dt_assinatura": "' + $('#dt_assinatura_fonte_modal').val() + '"}');
    $('#json_arquivo_diario_fonte').val('');
    $('#ds_diario').val($('#nm_tipo_fonte_modal').val() + ' de ' + $('#dt_assinatura_fonte_modal').val());
    $('#modal_fonte .consultar').hide();
    $('#modal_fonte .resultado').hide();
    $('#modal_fonte .diario').show();
}

function CriarModalOrigem() {
    $('#modal_origem .dados_orgao').hide();
    $("#modal_origem").modallight({
        sTitle: "Adicionar Origem",
        sWidth: '800',
        oButtons: [
            {
                text: "Salvar",
                click: function () {
                    var oOrgao = {};
                    oOrgao.ch_orgao = $('#ch_orgao').val();
                    oOrgao.sg_orgao = $('#label_sg_hierarquia_modal').text();
                    oOrgao.nm_orgao = $('#label_nm_hierarquia_modal').text();
                    var nm_orgao = $('#label_nm_orgao_modal').text();
                    var sg_orgao = $('#label_sg_orgao_modal').text();
                    if (IsNotNullOrEmpty(oOrgao.ch_orgao) && IsNotNullOrEmpty(oOrgao.nm_orgao) && IsNotNullOrEmpty(oOrgao.sg_orgao)) {
                        $('#tbody_origens .tr_vazia').remove();
                        var id_input = 'input_' + Guid('N');
                        $('#tbody_origens').append(
                            '<tr>' +
                                '<td>' + nm_orgao + '<input id="' + id_input + '" type="hidden" name="orgao" value="" /></td>' +
                                '<td>' + sg_orgao + '</td>' +
                                '<td>' + $('#label_sg_hierarquia_modal').text() + '</td>' +
                                '<td>' + $('#label_in_ambito_modal').text() + '</td>' +
                                '<td>' + $('#label_dt_inicio_vigencia_modal').text() + '</td>' +
                                '<td>' + $('#label_dt_fim_vigencia_modal').text() + '</td>' +
                                '<td>' + $('#label_orgao_cadastrador_modal').text() + '</td>' +
                                '<td class="control">' +
                                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event); fnAutocompleteOrigem();"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                                '</td>' +
                            '</tr>'
                        );
                        $('#' + id_input).val(JSON.stringify(oOrgao));
                        fnAutocompleteOrigem();
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
            LimparModal('modal_origem');
        }
    });
}

function fnAppendAutoria(oAutoria) {
    $('#tbody_autorias .tr_vazia').remove();
//    var oAutoria = {ch_autoria: ch_autoria, nm_autoria: nm_autoria};
    var id_input = 'input_' + Guid('N');
    $('#tbody_autorias').append(
        '<tr>' +
            '<td><input id="' + id_input + '" type="hidden" name="autoria" value="" />' + oAutoria.nm_autoria + '</td>' +
            '<td class="control">' +
                '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
            '</td>' +
        '</tr>'
    );
    $('#' + id_input).val(JSON.stringify(oAutoria));
}

function CriarModalAutoria() {
    $("#modal_autoria").modallight({
        sTitle: "Adicionar Autoria",
        sWidth: '500',
        oButtons: [
            {
                text: "Salvar",
                click: function () {
                    var ch_autoria = $('#ch_autoria').val();
                    var nm_autoria = $('#nm_autoria').val();
                    if (IsNotNullOrEmpty(ch_autoria)) {
                        fnAppendAutoria({ ch_autoria: ch_autoria, nm_autoria: nm_autoria });
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
            LimparModal('modal_autoria');
        }
    });
}



function CriarModalInteressado() {
    $("#modal_interessado").modallight({
        sTitle: "Adicionar Interessado",
        sWidth: '500',
        oButtons: [
            {
                text: "Salvar",
                click: function () {
                    var ch_interessado = $('#ch_interessado').val();
                    var nm_interessado = $('#nm_interessado').val();
                    if (IsNotNullOrEmpty(ch_interessado) && IsNotNullOrEmpty(nm_interessado)) {
                        AdicionarInteressado(nm_interessado, ch_interessado);
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
            LimparModal('modal_interessado');
        }
    });
}

function CriarModalNome() {
    $("#modal_nome").modallight({
        sTitle: "Adicionar Nome",
        sWidth: '500',
        oButtons: [
            {
                text: "Salvar",
                click: function () {
                    var nm_nome = $('#nm_nome').val();
                    if (IsNotNullOrEmpty(nm_nome)) {
                        $('#tbody_nomes .tr_vazia').remove();
                        $('#tbody_nomes').append(
                            '<tr>' +
                                '<td>' + nm_nome + '<input type="hidden" name="nm_nome" value="' + nm_nome + '" /></td>' +
                                '<td class="control">' +
                                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="'+_urlPadrao+'/Imagens/ico_delete_p.png"  /></a>' +
                                '</td>' +
                            '</tr>'
                        );
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
            LimparModal('modal_nome');
        }
    });
}

function EditarIndexacao(event) {
    if (event != null) {
        var tr = $(event.target).closest('tr');
        var indexacao = $('input[name="indexacao"]', tr).val();
        if (IsNotNullOrEmpty(indexacao)) {
            $('#tbody_termos_modal .tr_vazia').remove();
            var indexacao_split = indexacao.split('|');
            for (var i = 0; i < indexacao_split.length; i++) {
                var termo_split = indexacao_split[i].split('#');
                $('#tbody_termos_modal').append(
                    '<tr>' +
                        '<td>' + termo_split[2] + '<input type="hidden" class="ch_termo_modal" value="' + termo_split[0] + '"/> <input type="hidden" class="ch_tipo_termo_modal" value="' + termo_split[1] + '" /><input type="hidden" class="nm_termo_modal" value="' + termo_split[2] + '" />' +
                        '<td class="control">' +
                            '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                        '</td>' +
                    '</tr>'
                );
            }
        }
        CriarModalIndexacao(tr);
    }
}

function CriarModalIndexacao(tr) {
    $("#modal_indexacao").modallight({
        sTitle: "Adicionar Indexação",
        sWidth: '700',
        oButtons: [
            {
                text: "Salvar",
                click: function () {
                    var inputs_hidden_ch_termo = $('#tbody_termos_modal .ch_termo_modal');
                    var inputs_hidden_ch_tipo_termo = $('#tbody_termos_modal .ch_tipo_termo_modal');
                    var inputs_hidden_nm_termo = $('#tbody_termos_modal .nm_termo_modal');
                    if ($(inputs_hidden_ch_tipo_termo[0]).val() == "ES") {
                        $('#modal_indexacao .notify').messagelight({
                            sContent: "Não é permitido iniciar indexação com especificador.",
                            sType: "error",
                            iTime: 5000
                        });
                        return false;
                    }
                    if (IsNotNullOrEmpty(inputs_hidden_ch_termo)) {
                        var ch_termos = "";
                        var ch_tipo_termos = "";
                        var nm_termos = "";
                        var indexacao = "";
                        var trs_indexacao = $('input[name="indexacao"]');
                        for (var i = 0; i < inputs_hidden_ch_termo.length; i++) {
                            ch_termos += (ch_termos != "" ? "#" : "") + $(inputs_hidden_ch_termo[i]).val();
                            ch_tipo_termos += (ch_tipo_termos != "" ? "#" : "") + $(inputs_hidden_ch_tipo_termo[i]).val();
                            nm_termos += (nm_termos != "" ? ", " : "") + $(inputs_hidden_nm_termo[i]).val();
                            indexacao += (indexacao != "" ? "|" : "") + $(inputs_hidden_ch_termo[i]).val() + "#"
                                    + $(inputs_hidden_ch_tipo_termo[i]).val() + "#"
                                    + $(inputs_hidden_nm_termo[i]).val();
                        }
                        if ($('input[name="indexacao"][value="' + indexacao + '"]').length > 0) {
                            $('#modal_indexacao .notify').messagelight({
                                sContent: "A indexacao já está sendo usada. Não é permitido repetir.",
                                sType: "error",
                                iTime: 5000
                            });
                            return false;
                        }
                        $('#tbody_indexacoes .tr_vazia').remove();
                        if (IsNotNullOrEmpty(tr)) {
                            $(tr).html(
                                '<td>' + nm_termos + '<input type="hidden" name="indexacao" value="' + indexacao + '"></td>' +
                                '<td>' +
                                    '<a title="Editar Termos" href="javascript:void(0);" onclick="javascript:EditarIndexacao(event);"><img valign="absmiddle" alt="Editar" src="' + _urlPadrao + '/Imagens/ico_pencil_p.png"  /></a>' +
                                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                                '</td>'
                            );
                        }
                        else {
                            $('#tbody_indexacoes').append(
                                '<tr>' +
                                    '<td>' + nm_termos + '<input type="hidden" name="indexacao" value="' + indexacao + '"></td>' +
                                    '<td>' +
                                        '<a title="Editar Termos" href="javascript:void(0);" onclick="javascript:EditarIndexacao(event);"><img valign="absmiddle" alt="Editar" src="' + _urlPadrao + '/Imagens/ico_pencil_p.png"  /></a>' +
                                        '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                                    '</td>' +
                                '</tr>'
                            );
                        }
                        $(this).dialog('close');
                    }
                    else {
                        $('#modal_indexacao .notify').messagelight({
                            sTitle: "Erro",
                            sContent: "Preencha os campos obrigatórios.",
                            sType: "error",
                            sWidth: "",
                            iTime: null
                        });
                    }
                }
            },
            {
                text: "Cancelar",
                click: function () {
                    $(this).dialog('destroy');
                }
            }
        ],
        fnClose: function () {
            LimparModal('modal_indexacao');
        }
    });
}

function AdicionarDecisao(nm_tipo_decisao, in_tipo_decisao, ds_complemento, dt_decisao) {
    $('#tbody_decisoes .tr_vazia').remove();

    var id_input = 'input_' + Guid('N');
    var tr_decisao = '<tr>' +
            '<td>' + nm_tipo_decisao + '<input id="'+id_input+'" type="hidden" name="decisao" value="" /></td>' +
            '<td>' + ds_complemento + '</td>' +
            '<td>' + dt_decisao + '</td>' +
            ($('#table_decisoes th').length > 3 ?
                '<td class="control">' +
                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                '</td>'
            : '') +
        '</tr>';

    $('#tbody_decisoes').append(tr_decisao);

    var decisao = { in_decisao: in_tipo_decisao, ds_complemento: ds_complemento, dt_decisao: dt_decisao };

    $('#' + id_input).val(JSON.stringify(decisao));

}

function AdicionarInteressado(nm_interessado, ch_interessado) {
    $('#tbody_interessados .tr_vazia').remove();
    $('#tbody_interessados').append(
        '<tr>' +
            '<td>' + nm_interessado + '<input type="hidden" name="interessado" value="' + ch_interessado + '#' + nm_interessado + '" /></td>' +
            ($('#table_interessados th').length > 1 ?
                '<td class="control">' +
                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                '</td>'
            : '') +
        '</tr>'
    );
}

function AdicionarRequerente(nm_requerente, ch_requerente) {
    $('#tbody_requerentes .tr_vazia').remove();
    $('#tbody_requerentes').append(
        '<tr>' +
            '<td>' + nm_requerente + '<input type="hidden" name="requerente" value="' + ch_requerente + '#' + nm_requerente + '" /></td>' +
            ($('#table_requerentes th').length > 1 ?
                '<td class="control">' +
                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                '</td>'
            : '') +
        '</tr>'
    );
}

function AdicionarRequerido(nm_requerido, ch_requerido) {
    $('#tbody_requeridos .tr_vazia').remove();
    $('#tbody_requeridos').append(
        '<tr>' +
            '<td>' + nm_requerido + '<input type="hidden" name="requerido" value="' + ch_requerido + '#' + nm_requerido + '" /></td>' +
            ($('#table_requeridos th').length > 1 ?
            '<td class="control">' +
                '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
            '</td>'
            : '') +
        '</tr>'
    );
}

function AdicionarProcurador(nm_procurador_responsavel, ch_procurador_responsavel) {
    $('#tbody_procuradores .tr_vazia').remove();
    $('#tbody_procuradores').append(
        '<tr>' +
            '<td>' + nm_procurador_responsavel + '<input type="hidden" name="procurador_responsavel" value="' + ch_procurador_responsavel + '#' + nm_procurador_responsavel + '" /></td>' +
            ($('#table_procuradores th').length > 1 ?
                '<td class="control">' +
                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                '</td>'
            : '') +
        '</tr>'
    );
}

function AdicionarRelator(nm_relator, ch_relator) {
    $('#tbody_relatores .tr_vazia').remove();
    $('#tbody_relatores').append(
        '<tr>' +
            '<td>' + nm_relator + '<input type="hidden" name="relator" value="' + ch_relator + '#' + nm_relator + '" /></td>' +
            ($('#table_relatores th').length > 1 ?
            '<td class="control">' +
                '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
            '</td>' 
            : '') +
        '</tr>'
    );
}

function CriarModalDecisao() {
    $("#modal_decisao").modallight({
        sTitle: "Adicionar Decisão",
        sWidth: '500',
        oButtons: [
            {
                text: "Salvar",
                click: function () {
                    var in_tipo_decisao = $('#in_tipo_decisao_modal').val();
                    var nm_tipo_decisao = $('#in_tipo_decisao_modal option[value="' + in_tipo_decisao + '"]').text();
                    var ds_complemento = $('#ds_complemento_modal').val();
                    var dt_decisao = $('#dt_decisao_modal').val();
                    if (IsNotNullOrEmpty(in_tipo_decisao)) {
                        AdicionarDecisao(nm_tipo_decisao, in_tipo_decisao, ds_complemento, dt_decisao);
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
            LimparModal('modal_decisao');
        }
    });
}


function CriarModalRequerente() {
    $("#modal_requerente").modallight({
        sTitle: "Adicionar Requerente",
        sWidth: '500',
        oButtons: [
            {
                text: "Salvar",
                click: function () {
                    var ch_requerente = $('#ch_requerente_modal').val();
                    var nm_requerente = $('#nm_requerente_modal').val();
                    if (IsNotNullOrEmpty(ch_requerente) && IsNotNullOrEmpty(nm_requerente)) {
                        $('#tbody_requerentes .tr_vazia').remove();
                        $('#tbody_requerentes').append(
                            '<tr>' +
                                '<td>' + nm_requerente + '<input type="hidden" name="requerente" value="' + ch_requerente + '#'+nm_requerente+'" /></td>' +
                                '<td class="control">' +
                                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="'+_urlPadrao+'/Imagens/ico_delete_p.png"  /></a>' +
                                '</td>' +
                            '</tr>'
                        );
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
            LimparModal('modal_requerente');
        }
    });
}


function CriarModalRequerido() {
    $("#modal_requerido").modallight({
        sTitle: "Adicionar Requerido",
        sWidth: '500',
        oButtons: [
            {
                text: "Salvar",
                click: function () {
                    var ch_requerido = $('#ch_requerido_modal').val();
                    var nm_requerido = $('#nm_requerido_modal').val();
                    if (IsNotNullOrEmpty(ch_requerido) && IsNotNullOrEmpty(nm_requerido)) {
                        $('#tbody_requeridos .tr_vazia').remove();
                        $('#tbody_requeridos').append(
                            '<tr>' +
                                '<td>' + nm_requerido + '<input type="hidden" name="requerido" value="' + ch_requerido + '#'+nm_requerido+'" /></td>' +
                                '<td class="control">' +
                                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="'+_urlPadrao+'/Imagens/ico_delete_p.png"  /></a>' +
                                '</td>' +
                            '</tr>'
                        );
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
            LimparModal('modal_requerido');
        }
    });
}


function CriarModalProcurador() {
    $("#modal_procurador").modallight({
        sTitle: "Adicionar Procuradores",
        sWidth: '500',
        oButtons: [
            {
                text: "Salvar",
                click: function () {
                    var ch_procurador = $('#ch_procurador_modal').val();
                    var nm_procurador = $('#nm_procurador_modal').val();
                    if (IsNotNullOrEmpty(ch_procurador) && IsNotNullOrEmpty(nm_procurador)) {
                        $('#tbody_procuradores .tr_vazia').remove();
                        $('#tbody_procuradores').append(
                            '<tr>' +
                                '<td>' + nm_procurador + '<input type="hidden" name="procurador_responsavel" value="' + ch_procurador + '#' + nm_procurador + '" /></td>' +
                                '<td class="control">' +
                                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="'+_urlPadrao+'/Imagens/ico_delete_p.png"  /></a>' +
                                '</td>' +
                            '</tr>'
                        );
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
            LimparModal('modal_procurador');
        }
    });
}


function CriarModalRelator() {
    $("#modal_relator").modallight({
        sTitle: "Adicionar Relatores",
        sWidth: '500',
        oButtons: [
            {
                text: "Salvar",
                click: function () {
                    var ch_relator = $('#ch_relator_modal').val();
                    var nm_relator = $('#nm_relator_modal').val();
                    if (IsNotNullOrEmpty(ch_relator) && IsNotNullOrEmpty(nm_relator)) {
                        $('#tbody_relatores .tr_vazia').remove();
                        $('#tbody_relatores').append(
                            '<tr>' +
                                '<td>' + nm_relator + '<input type="hidden" name="relator" value="' + ch_relator + '#' + nm_relator + '" /></td>' +
                                '<td class="control">' +
                                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="'+_urlPadrao+'/Imagens/ico_delete_p.png"  /></a>' +
                                '</td>' +
                            '</tr>'
                        );
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
            LimparModal('modal_relator');
        }
    });
}


function AdicionarTermo() {
    var ch_termo = $('#ch_termo_modal').val();
    var ch_tipo_termo = $('#ch_tipo_termo_modal').val();
    var nm_termo = $('#nm_termo_modal').val();
    if (IsNotNullOrEmpty(ch_termo) && IsNotNullOrEmpty(nm_termo)) {
        if($('input[value="' + ch_termo + '"]', $('#tbody_termos_modal')).length > 0){
            $('.notify', $('#modal_indexacao')).messagelight({
                sContent: "Não é permitido repitir termos.",
                sType: "error",
                iTime: 5000
            });
            return false;
        }
        $('#tbody_termos_modal .tr_vazia').remove();
        $('#tbody_termos_modal').append(
            '<tr>' +
                '<td>' + nm_termo + '<input type="hidden" class="ch_termo_modal" value="' + ch_termo + '"/> <input type="hidden" class="ch_tipo_termo_modal" value="' + ch_tipo_termo + '" /><input type="hidden" class="nm_termo_modal" value="' + nm_termo + '" />' +
                '<td class="control">' +
                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="'+_urlPadrao+'/Imagens/ico_delete_p.png"  /></a>' +
                '</td>' +
            '</tr>'
        );
        $('#ch_termo_modal').val('');
        $('#ch_tipo_termo_modal').val('');
        $('#nm_termo_modal').val('');
    }
}

function ValidarDuplicidadeDeNorma() {
    $('#div_notificacao_norma_duplicadade').html('');
    $('#div_notificacao_norma_duplicadade').hide();
    var ch_tipo_norma = $('#ch_tipo_norma').val();
    var nm_tipo_norma = $('#nm_tipo_norma').val();
    var nr_norma = $('#nr_norma').val();
    var cr_norma = $('#cr_norma').val();
    var nr_sequencial = $('#nr_sequencial').val();
    var dt_assinatura = $('#dt_assinatura').val();
    var ch_orgao = "";
    $("input:hidden[name='orgao']").each(function () {
        ch_orgao += (ch_orgao != "" ? "," : "") + $(this).val().split('#')[0];
    });
    if (IsNotNullOrEmpty(ch_tipo_norma) && IsNotNullOrEmpty(dt_assinatura) && IsNotNullOrEmpty(ch_orgao)) {
        var query = "?ch_tipo_norma=" + ch_tipo_norma + ($('#norma_sem_numero').prop("checked") ? "" : "&nr_norma=" + nr_norma) + "&cr_norma=" + cr_norma + "&nr_sequencial=" + nr_sequencial + "&dt_assinatura=" + dt_assinatura + "&ch_orgao=" + ch_orgao + "&b_consultar_norma_duplicada=1&b_norma_sem_numero=" + ($('#norma_sem_numero').prop("checked") ? "1" : "");
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (IsNotNullOrEmpty(data.error_message)) {
                    $('<div id="modal_notificacao_validar_norma_duplicidade" />').modallight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                        fnClose: function () {
                            $(this).remove();
                        }
                    });
                }
                else if (data.results != null) {
                    if (data.results.length > 0) {
                        var normas_duplicadas = "";
                        for (var i = 0; i < data.results.length; i++) {
                            normas_duplicadas += "<br/><a href='./DetalhesDeNorma.aspx?id_norma="+data.results[i].ch_norma+"' title='Detalhes da Norma'>" + data.results[i].nm_tipo_norma + " " + data.results[i].nr_norma + " " + data.results[i].dt_assinatura + "<img src='"+_urlPadrao+"/Imagens/ico_loupe_p.png' alt='detalhes' /></a>";
                        }
                        $('<div id="modal_notificacao_validar_norma_duplicidade" />').modallight({
                            sTitle: "Duplicidade",
                            sContent: "Norma Duplicada:" + normas_duplicadas,
                            sType: "error",
                            oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                            fnClose: function () {
                                $(this).remove();
                            }
                        });
                    }
                    else {
                        $('<div id="modal_notificacao_validar_norma_duplicidade" />').modallight({
                            sTitle: "Sucesso",
                            sContent: "A Norma não é duplicada.",
                            sType: "success",
                            oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                            fnClose: function () {
                                $(this).remove();
                            }
                        });
                    }
                }
                else {
                    $('<div id="modal_notificacao_validar_norma_duplicidade" />').modallight({
                        sTitle: "Erro",
                        sContent: "Ocorreu erro um erro não identificado.",
                        sType: "error",
                        oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                        fnClose: function () {
                            $(this).remove();
                        }
                    });
                }
            }
        };
        
        $.ajaxlight({
            sUrl: MostrarPaginaAjax("PES") + query,
            sType: "GET",
            fnSuccess: sucesso,
            fnComplete: gComplete,
            fnBeforeSend: gInicio,
            bAsync: true
        });
    }
    else {
        $('#div_notificacao_norma_duplicadade').messagelight({
            sTitle: "Erro",
            sContent: "Campos obrigatórios pendentes.",
            sType: "error",
            sWidth: "",
            iTime: null
        });
    }
}

function SelecionarTipoDeNorma() {
    $('.buttons').hide();
    $('#div_dados_gerais').hide();
    $('#div_publicacoes').hide();
    $('#div_indexacao').hide();
    $('#div_dados_de_acoes').hide();
    $('#line_interessados').hide();
    $('#line_autorias').hide();
    $('#line_apelido').hide();
    if ($('#ch_tipo_norma').val() != "" && IsNotNullOrEmpty(_user, 'orgao_cadastrador.nm_orgao_cadastrador')) {
        $('.buttons').show();
        $('#div_dados_gerais').show();
        $('#div_publicacoes').show();
        $('#div_indexacao').show();
        if ($('#in_g1').val() === "true") {
            $('#line_autorias').show();
        }
        if ($('#in_g2').val() === "true") {
            $('#line_interessados').show();
        }
        if ($('#in_g2').val() === "true") {
            $('#div_dados_de_acoes').show();
            
        }
        if ($('#in_g3').val() === "true") {

        }
        if ($('#in_g4').val() === "true") {
            $('#line_interessados').show();
            if (_user.orgao_cadastrador.nm_orgao_cadastrador.toUpperCase() == "PGDF") {
                $('#line_autorias').show();
            }
        }
        if ($('#in_g5').val() === "true") {

        }
        if ($('#in_conjunta').val() === "true") {
            
        }
        if ($('#EhLei').val() === "true") {

            $('#line_autorias').show();
            
        }
        if ($('#EhDecreto').val() === "true") {
            
        }
        if ($('#EhResolucao').val() === "true") {
            $('#line_autorias').show();

        }
        if ($('#EhPortaria').val() === "true") {

        }
        if ($('#in_apelidavel').val() === "true") {
            $('#line_apelido').show();
        }
    }

}

function MostrarDadosOrigem(){
	if ($('#ch_orgao').val() != "")
	{
	    $('.dados_orgao').show();
	}
	else
	{
	    $('.dados_orgao').hide();
	}
}

function MarcarPendente() {
    if ($('#st_pendencia').is(':checked')) {
        $('#line_ds_pendencia').show();
    }
    else {
        $('#line_ds_pendencia').hide();
    }
}

function PreencherNormaEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_norma').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_tipo_norma != null && data.ch_tipo_norma != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#ch_tipo_norma').val(data.ch_tipo_norma);
                    $('#nm_tipo_norma').val(data.nm_tipo_norma);
                    $('#in_g1').val(data.tipoDeNorma.in_g1);
                    $('#in_g2').val(data.tipoDeNorma.in_g2);
                    $('#in_g3').val(data.tipoDeNorma.in_g3);
                    $('#in_g4').val(data.tipoDeNorma.in_g4);
                    $('#in_g5').val(data.tipoDeNorma.in_g5);
                    $('#EhLei').val(data.tipoDeNorma.EhLei);
                    $('#EhDecreto').val(data.tipoDeNorma.EhDecreto);
                    $('#EhResolucao').val(data.tipoDeNorma.EhResolucao);
                    $('#EhPortaria').val(data.tipoDeNorma.EhPortaria);
                    $('#in_conjunta').val(data.tipoDeNorma.in_conjunta);
                    $('#in_numeracao_por_orgao').val(data.tipoDeNorma.in_numeracao_por_orgao);
                    $('#in_apelidavel').val(data.tipoDeNorma.in_apelidavel);
                    SelecionarTipoDeNorma();

                    $('#nr_norma').val(data.nr_norma);
                    $('#cr_norma').val(data.cr_norma);
                    $('#nr_sequencial').val(data.nr_sequencial);
                    $('#dt_assinatura').val(data.dt_assinatura);
                    
                    $('#st_vacatio_legis').prop('checked',data.st_vacatio_legis);
                    $('#dt_inicio_vigencia').val(getVal(data.dt_inicio_vigencia));
                    $('#ds_vacatio_legis').val(getVal(data.ds_vacatio_legis));
                    selecionarVacatioLegis();

                    $('#id_ambito').val(data.id_ambito);
                    $('#ds_ementa').val(data.ds_ementa);
                    $('#ds_observacao').val(data.ds_observacao);
                    $('#st_destaque').prop("checked", data.st_destaque);
                    $('#st_pendencia').prop("checked", data.st_pendencia);
                    $('#ds_pendencia').val(data.ds_pendencia);
                    MarcarPendente();

                    // Recebe e percorre objeto de origens
                    for (origem in data.origensOv) {
                        //Atribui variáveis com os dados recebidos do objeto origensOv
                        var ch_orgao = data.origensOv[origem].ch_orgao;
                        var nm_orgao = data.origensOv[origem].nm_orgao;
                        var sg_orgao = data.origensOv[origem].sg_orgao;
                        var sg_hierarquia = data.origensOv[origem].sg_hierarquia;
                        var nm_ambito = data.origensOv[origem].ambito.nm_ambito;
                        var dt_inicio_vigencia = data.origensOv[origem].dt_inicio_vigencia;
                        var dt_fim_vigencia = data.origensOv[origem].dt_fim_vigencia;
                        var orgaos_cadastradores = "";
                        var qtd_orgao = data.origensOv[origem].orgaos_cadastradores.length;
                        for (orgao in data.origensOv[origem].orgaos_cadastradores) {
                            var nm_orgao_cadastrador = data.origensOv[origem].orgaos_cadastradores[orgao].nm_orgao_cadastrador;
                            if ((orgao + 1) < qtd_orgao) {
                                // Se não for o ultimo orgao, concatena com uma virgula na frente
                                orgaos_cadastradores += nm_orgao_cadastrador + ', ';
                            }
                            else {
                                orgaos_cadastradores += nm_orgao_cadastrador;
                            }
                        }
                        var nm_orgao_cadastrador = data.origensOv[origem].orgaos_cadastradores[0].nm_orgao_cadastrador;

                        //Preenche a tabela com os dados
                        if (IsNotNullOrEmpty(ch_orgao) && IsNotNullOrEmpty(nm_orgao) && IsNotNullOrEmpty(sg_orgao)) {
                            $('#tbody_origens .tr_vazia').remove();
                            var id_input = 'input_' + Guid('N');
                            var oOrgao = { ch_orgao: ch_orgao, nm_orgao: nm_orgao, sg_orgao: sg_orgao };
                            $('#tbody_origens').append(
                    		 	'<tr>' +
                    		 		'<td>' + oOrgao.nm_orgao + '<input id="' + id_input + '" type="hidden" name="orgao" value="" /></td>' +
                               		'<td>' + oOrgao.sg_orgao + '</td>' +
                               		'<td>' + sg_hierarquia + '</td>' +
                               		'<td>' + nm_ambito + '</td>' +
                               		'<td>' + GetText(dt_inicio_vigencia) + '</td>' +
                               		'<td>' + GetText(dt_fim_vigencia) + '</td>' +
                               		'<td>' + orgaos_cadastradores + '</td>' +
                                    '<td class="control">' +
                                        '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                                    '</td>' +
	                            '</tr>'
                    		 );
                            $('#' + id_input).val(JSON.stringify(oOrgao));
                        }
                    }

                    if (IsNotNullOrEmpty(data.decisoes)) {
                        for (var i = 0; i < data.decisoes.length; i++) {
                            AdicionarDecisao(data.decisoes[i].nm_decisao, data.decisoes[i].in_decisao, data.decisoes[i].ds_complemento, data.decisoes[i].dt_decisao);
                        }
                    }
                    if (IsNotNullOrEmpty(data.interessados)) {
                        for (var i = 0; i < data.interessados.length; i++) {
                            AdicionarInteressado(data.interessados[i].nm_interessado, data.interessados[i].ch_interessado);
                        }
                    }
                    if (IsNotNullOrEmpty(data.requerentes)) {
                        for (var i = 0; i < data.requerentes.length; i++) {
                            AdicionarRequerente(data.requerentes[i].nm_requerente, data.requerentes[i].ch_requerente);
                        }
                    }
                    if (IsNotNullOrEmpty(data.requeridos)) {
                        for (var i = 0; i < data.requeridos.length; i++) {
                            AdicionarRequerido(data.requeridos[i].nm_requerido, data.requeridos[i].ch_requerido);
                        }
                    }
                    if (IsNotNullOrEmpty(data.procuradores_responsaveis)) {
                        for (var i = 0; i < data.procuradores_responsaveis.length; i++) {
                            AdicionarProcurador(data.procuradores_responsaveis[i].nm_procurador_responsavel, data.procuradores_responsaveis[i].ch_procurador_responsavel);
                        }
                    }
                    if (IsNotNullOrEmpty(data.relatores)) {
                        for (var i = 0; i < data.relatores.length; i++) {
                            AdicionarRelator(data.relatores[i].nm_relator, data.relatores[i].ch_relator);
                        }
                    }
                    if (IsNotNullOrEmpty(data.url_referencia)) {
                        $('input[name="url_referencia"]').val(data.url_referencia);
                    }
                    if (IsNotNullOrEmpty(data.ds_procedencia)) {
                        $('input[name="ds_procedencia"]').val(data.ds_procedencia);
                    }
                    if (IsNotNullOrEmpty(data.ds_parametro_constitucional)) {
                        $('input[name="ds_parametro_constitucional"]').val(data.ds_parametro_constitucional);
                    }
                    if (IsNotNullOrEmpty(data.ds_efeito_decisao)) {
                        $('select[name="ds_efeito_decisao"]').val(data.ds_efeito_decisao);
                    }

                    for (var i = 0; i < data.nm_pessoa_fisica_e_juridica.length; i++) {
                        $('#tbody_nomes .tr_vazia').remove();
                        $('#tbody_nomes').append(
                    		'<tr>' +
                    			'<td>' + data.nm_pessoa_fisica_e_juridica[i] + '<input type="hidden" name="nm_nome" value="' + data.nm_pessoa_fisica_e_juridica[i] + '" /></td>' +
                                '<td class="control">' +
                                    '<a title="Excluir" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                                '</td>' +
                            '</tr>'
                    	);

                    }

                    if (IsNotNullOrEmpty(data.nm_apelido)) {
                        $('#nm_apelido').val(data.nm_apelido);
                    }

                    //Preenche os dados da indexacao
                    if (IsNotNullOrEmpty(data, 'indexacoes')) {
                        $('#tbody_indexacoes .tr_vazia').remove();
                        for (var i = 0; i < data.indexacoes.length; i++) {
                            var nm_termos = "";
                            var indexacao = "";
                            for (var j = 0; j < data.indexacoes[i].vocabulario.length; j++) {
                                nm_termos += (nm_termos != "" ? ", " : "") + data.indexacoes[i].vocabulario[j].nm_termo;
                                indexacao += (indexacao != "" ? "|" : "") + data.indexacoes[i].vocabulario[j].ch_termo + "#" + data.indexacoes[i].vocabulario[j].ch_tipo_termo + "#" + data.indexacoes[i].vocabulario[j].nm_termo;
                            }
                            $('#tbody_indexacoes').append(
                        	'<tr>' +
                        		'<td>' + nm_termos + '<input type="hidden" name="indexacao" value="' + indexacao + '" /> </td>' +
                                '<td class="control">' +
                                    '<a title="Editar Termos" href="javascript:void(0);" onclick="javascript:EditarIndexacao(event);"><img valign="absmiddle" alt="Editar" src="' + _urlPadrao + '/Imagens/ico_pencil_p.png"  /></a>' +
                                    '<a title="Remover Termos" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                                '</td>' +
                             '</tr>'
                        );
                        }
                    }

                    for (fonte in data.fontes) {
                        $('#tbody_fontes .tr_vazia').remove();
                        var guid = Guid('N');
                        var id_input = 'input_' + guid;
                        var id_td = 'td_' + guid;
                        var display_btn_file = IsNotNullOrEmpty(data.fontes[fonte].ar_fonte, 'id_file');
                        $('#tbody_fontes').append(
                            '<tr>' +
                                '<td>' + data.fontes[fonte].ds_diario + '</td>' +
                                '<td>' + data.fontes[fonte].nm_tipo_publicacao + '</td>' +
                                '<td>' + GetText(data.fontes[fonte].nr_pagina) + '</td>' +
                                '<td>' + GetText(data.fontes[fonte].nr_coluna) + '</td>' +
                                '<td>' + GetText(data.fontes[fonte].ds_observacao_fonte) + '</td>' +
                                '<td>' + GetText(data.fontes[fonte].ds_republicacao) + '</td>' +
                                '<td id="' + id_td + '">' +
                                    '<input id="' + id_input + '" type="hidden" name="fonte" value="" />' +
                                    '<input type="hidden" class="json_arquivo" value="" />' +
                                    '<label class="name" style="color:#000;">' +
                                        GetText(data.fontes[fonte].ar_fonte, 'filename') +
                                    '</label>' +
                                    '<a href="javascript:void(0);" onclick="javascript:anexarInputFile(this);" class="attach" title="Anexar um arquivo" ' + (display_btn_file ? 'style="display:none"' : '') + '><img valign="absmiddle" alt="Anexar" src="' + _urlPadrao + '/Imagens/ico_attach_file2.png" width="18px" /></a>' +
                                    '<a href="javascript:void(0);" onclick="javascript:deletarInputFile(this);" class="delete" title="Remover o arquivo" ' + (!display_btn_file ? 'style="display:none"' : '') + '><img valign="absmiddle" alt="Remover" src="' + _urlPadrao + '/Imagens/ico_delete_file.png" width="18px" /></a>' +
                                    '<a path="ar_fonte" href="javascript:void(0);" onclick="javascript:editarInputFile(this);" class="create" title="Editar ou criar um arquivo"><img valign="absmiddle" alt="Editar" src="' + _urlPadrao + '/Imagens/ico_edit_file.png" width="18px" /></a>' +
                                    '<a path="ar_fonte" publicacao="' + data.fontes[fonte].nm_tipo_publicacao + '" ds_diario="' + data.fontes[fonte].ds_diario + '" div_arquivo="' + id_td + '" href="javascript:void(0);" onclick="javascript:abrirModalImportarArquivo(this);" class="import" title="Importar arquivo do módulo de arquivos" ' + (display_btn_file ? 'style="display:none"' : '') + '><img width="18" valign="absmiddle" alt="Editar" src="' + _urlPadrao + '/Imagens/ico_import_file.png" /></a>' +
                                '</td>' +
                                '<td>' +
                                    '<a title="Editar Fonte" href="javascript:void(0);" onclick="javascript:EditarFonte(event);"><img valign="absmiddle" alt="Editar" src="' + _urlPadrao + '/Imagens/ico_pencil_p.png"  /></a>' +
                                    '<a title="Excluir Fonte" href="javascript:void(0);" onclick="javascript:DeletarLinha(event);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png"  /></a>' +
                                '</td>' +
                            '</tr>'
                        );
                        $('#' + id_input).val(JSON.stringify(data.fontes[fonte]));
                        if (display_btn_file) {
                            $('#' + id_td + ' input.json_arquivo').val(JSON.stringify(data.fontes[fonte].ar_fonte));
                        }
                    }
                    if (IsNotNullOrEmpty(data.ar_atualizado, 'id_file')) {
                        $('#hidden_json_arquivo_texto_atualizado').val(JSON.stringify(data.ar_atualizado));
                        $('#label_arquivo_texto_atualizado').text(data.ar_atualizado.filename);
                        $('.attach', $("#div_texto_atualizado")).hide();
                        $('.import', $("#div_texto_atualizado")).hide();
                        $('.recovery', $("#div_texto_atualizado")).hide();
                        $('.delete', $("#div_texto_atualizado")).show();
                        if (data.ar_atualizado.mimetype != "text/html" && data.ar_atualizado.mimetype != "text/htm") {
                            $('.create', $("#div_texto_atualizado")).hide();
                        }
                        else {
                            $('.create', $("#div_texto_atualizado")).show();
                        }
                    }

                    //recuperar arquivo
                    $('#form_recuperar_arquivo > input[name="ch_norma"]').val(data.ch_norma);

                    if (IsNotNullOrEmpty(data.ar_acao, 'id_file')) {
                        $('#hidden_json_arquivo_texto_acao').val(JSON.stringify(data.ar_acao));
                        $('#label_arquivo_texto_acao').text(data.ar_acao.filename);
                        $('.attach', $("#div_dados_de_acoes")).hide();
                        $('.delete', $("#div_dados_de_acoes")).show();
                        if (data.ar_acao.mimetype != "text/html" && data.ar_acao.mimetype != "text/htm") {
                            $('.create', $("#div_dados_de_acoes")).hide();
                        }
                        else {
                            $('.create', $("#div_dados_de_acoes")).show();
                        }
                    }
                    for (var i = 0; i < data.autorias.length; i++) {
                        fnAppendAutoria(data.autorias[i])
                    }
                    if (data.st_situacao_forcada) {
                        $('#st_situacao_forcada').prop('checked', true);
                        changeStSiuacaoForcada(document.getElementById('st_situacao_forcada'));
                        $('#ch_situacao').attr('obrigatorio', 'sim');
                    }
                    $('#ch_situacao').val(data.ch_situacao);
                    $('#nm_situacao').val(data.nm_situacao);
                }
            }
        };
        $.ajaxlight({
            sUrl: MostrarPaginaAjax("VIS") + window.location.search,
            sType: "POST",
            fnSuccess: sucesso,
            fnComplete: gComplete,
            fnBeforeSend: gInicio,
            fnError: null,
            bAsync: true
        });
    }
}

function ExcluirVideConfirmado(sender, id_doc, ch_vide) {
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_notificacao_norma').modallight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }]
            });
        }
        else if (IsNotNullOrEmpty(data, 'id_doc_success')) {
            $('#div_notificacao_norma').modallight({
                sTitle: "Sucesso",
                sContent: "Vide removida com sucesso." + (IsNotNullOrEmpty(data, 'alert_message') ? ("<br/><div class='alert'><span>Observação:</span> " + data.alert_message + "</div>") : ""),
                sType: "success",
                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                fnClose: function () {
                    document.location.reload();
                }
            });
        }
        else if (IsNotNullOrEmpty(data, 'alert_message')) {
            $('#div_notificacao_norma').modallight({
                sTitle: "Atenção",
                sContent: data.alert_message,
                sType: "alert",
                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }]
            });
        }
        else {
            $('#div_notificacao_norma').modallight({
                sTitle: "Erro",
                sContent: "Ocorreu um erro desconhecido.",
                sType: "error",
                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }]
            });
        }
    }
    $.ajaxlight({
        sUrl: "./ashx/Exclusao/VideExcluir.ashx?id_doc=" + id_doc + "&ch_vide=" + ch_vide,
        fnSuccess: sucesso,
        fnBeforeSend: gInicio,
        fnComplete: gComplete,
        bAsync: true
    });
}

function ExcluirVide(sender, id_doc, ch_vide) {
    if (IsNotNullOrEmpty(id_doc) && IsNotNullOrEmpty(ch_vide)) {
        $("#modal_confirmar_excluir_vide").modallight({
            sTitle: "Excluir Vide",
            sContent: "Deseja excluir o vide?",
            sType: "alert",
            sWidth: '300',
            oButtons: [
                {
                    text: "Sim",
                    click: function () {
                        ExcluirVideConfirmado(sender, id_doc, ch_vide);
                        $(this).dialog('close');
                    }
                },
                {
                    text: "Não",
                    click: function () {
                        $(this).dialog('close');
                    }
                }
            ]
        });
    }
}

function VerificarNormaSemNumero(){
    // Se o checkbox estiver checado, o input nr_norma perde o atributo obrigatorio
	if ($('#norma_sem_numero').prop("checked")){
		$('#nr_norma').attr("obrigatorio", "");
	}
	else{
		$('#nr_norma').attr("obrigatorio", "sim");
	}
}

function preencherImpressao() {
    $('#div_impressao').append(
		'<table><thead></thead><tbody id="tbody_impressao"></tbody></table>'
	);

    var lines = $('#div_norma').find('div.line');

    for (var i = 0; i < lines.length; i++) {

        var text_label = $.trim($('label', lines[i]).text());
        var $value = $('div.value', lines[i]);
        var $br = $('br', lines[i]);
        var text_value = $.trim($value.text());
        var $tr_body = $('table:not(.small-only) tbody tr:not(.tr_vazia)', lines[i]);
        if (IsNotNullOrEmpty(text_label)) {
            if (IsNotNullOrEmpty(text_value)) {
                var $a = $('a', $value);
                var sA = '';
                for (var a = 0; a < $a.length; a++) {
                    sA += $a[a].textContent + '<br/>';
                }

                $('#tbody_impressao').append(
                    '<tr>' +
                        '<th>' + text_label + '</th>' +
                        '<td>' + (sA != '' ? sA : '<span>' + text_value + '</span>') + '</td>' +
                    '</tr>'
                );
            }
            else if ($tr_body.length > 0) {
                var sTable = '';
                var $tr_head = $('table thead tr', lines[i]);
                if ($tr_head.length == 1) {
                    var $th = $('th', $tr_head);
                    var sTh = '';
                    for (var th = 0; th < $th.length; th++) {
                        sTh += '<th>' + $th[th].textContent + '</th>';
                    }
                    sTable = '<thead><tr>' + sTh + '</tr></thead>';
                }
                sTable += '<tbody>';
                for (var tr = 0; tr < $tr_body.length; tr++) {
                    var $td = $('td', $tr_body[tr]);
                    var sTd = '';
                    for (var td = 0; td < $td.length; td++) {
                        sTd += '<td>' + $td[td].textContent + '</td>';
                    }
                    sTable += '<tr>' + sTd + '</tr>';
                }
                sTable += '</tbody>';
                $('#tbody_impressao').append(
                    '<tr>' +
                        '<th>' + text_label + '</th>' +
                        '<td>' +
                            '<table>' + sTable + '</table>' +
                        '</td>' +
                    '</tr>'
                );
            }
        }
        else if (IsNotNullOrEmpty(text_value)) {
            var text_h = '';
            var hNrs = [1, 2, 3, 4, 5, 6];
            for (var hNr in hNrs) {
                var text = $('h' + hNr, $value).text();
                if (text != "") {
                    text_h += '<tr><th colspan="2">' + text + '</th></tr>';
                }
            }
            if (text_h != '') {
                $('#tbody_impressao').append(text_h);
            }
            else {
                $('#tbody_impressao').append(
                    '<tr>' +
                        '<td colspan="2"><span>' + text_value + '</span></td>' +
                    '</tr>'
                );
            }
        }
    }
    // Estilização inline porque a funcao printElement nao le o arquivo css.
//    $('#div_impressao a').contents().unwrap();
//    $('#tbody_impressao tr').find('td:eq(0)').css("width", "45%");
//    $('#div_impressao label').css("font-weight", "bold");
//    $('#div_impressao label').css("color", "black");
//    $('#div_impressao label').css("font-size", "20px");
    $('#div_impressao span').css("font-size", "16px");
    $('#div_impressao table').css("width", "100%");
    $('#div_impressao table').css("margin", "auto");
    $('#div_impressao table').css("border-collapse", "collapse");
//    $('#div_impressao table caption').css("text-align", "center");
//    $('#div_impressao table caption').css("font-size", "20px");
//    $('#div_impressao table caption').css("font-weight", "bold");
//    $('#div_impressao table caption').css("color", "black");
    $('#div_impressao table tr td').css("padding", "10px 5px");
    $('#div_impressao table tr td').css("border-bottom", "1px outset black");
    $('#tbody_impressao table').css("border-collapse", "collapse");
    $('#tbody_impressao table tr td').css("border", "2px outset black");
    $('#tbody_impressao table tr td').css("text-align", "center");
    $('#tbody_impressao table tr td').css("font-size", "14px");
    $('#tbody_impressao table th').css("font-size", "16px");
    $('#tbody_impressao table th').css("border", "2px outset black");
    $('#tbody_impressao table th').css("background-color", "#EEEEEE");
}

//campos precisa ser um array com os nomes dos campos que devem ordenados, na sequencia.
function ordenarVides(vides) {
    
}

function OrdenarVidesPorData(vides) {
    // Os vides devem ser apresentados ordenados primeiramente pela coluna NORMA, em ordem crescente, da norma mais antiga para a mais nova.
    // O segundo critério de organização deve ser pela coluna DISPOSITIVO AFETADO, em ordem crescente.
    vides.sort(function (a, b) {
        if (IsNotNullOrEmpty(a.dt_assinatura_norma_vide) && IsNotNullOrEmpty(b.dt_assinatura_norma_vide)) {
            if (convertStringToDateTime(a.dt_assinatura_norma_vide) < convertStringToDateTime(b.dt_assinatura_norma_vide)) {
                return -1;
            }
            else if (convertStringToDateTime(a.dt_assinatura_norma_vide) > convertStringToDateTime(b.dt_assinatura_norma_vide)) {
                return 1;
            }
            else if (a.in_norma_afetada && parseInt(a.artigo_norma_vide) < parseInt(b.artigo_norma_vide)) {
                return -1;
            }
            else if (a.in_norma_afetada && parseInt(a.artigo_norma_vide) > parseInt(b.artigo_norma_vide)) {
                return 1;
            }
            else if (!a.in_norma_afetada && parseInt(a.artigo_norma_vide_outra) < parseInt(b.artigo_norma_vide_outra)) {
                return -1;
            }
            else if (!a.in_norma_afetada && parseInt(a.artigo_norma_vide_outra) > parseInt(b.artigo_norma_vide_outra)) {
                return 1;
            }
            else if (a.in_norma_afetada && parseInt(a.paragrafo_norma_vide) < parseInt(b.paragrafo_norma_vide)) {
                return -1;
            }
            else if (a.in_norma_afetada && parseInt(a.paragrafo_norma_vide) > parseInt(b.paragrafo_norma_vide)) {
                return 1;
            }
            else if (!a.in_norma_afetada && parseInt(a.paragrafo_norma_vide_outra) < parseInt(b.paragrafo_norma_vide_outra)) {
                return -1;
            }
            else if (!a.in_norma_afetada && parseInt(a.paragrafo_norma_vide_outra) > parseInt(b.paragrafo_norma_vide_outra)) {
                return 1;
            }
            else if (a.in_norma_afetada && a.inciso_norma_vide < b.inciso_norma_vide) {
                return -1;
            }
            else if (a.in_norma_afetada && a.inciso_norma_vide > b.inciso_norma_vide) {
                return 1;
            }
            else if (!a.in_norma_afetada && a.inciso_norma_vide_outra < b.inciso_norma_vide_outra) {
                return -1;
            }
            else if (!a.in_norma_afetada && a.inciso_norma_vide_outra > b.inciso_norma_vide_outra) {
                return 1;
            }
            else {
                if (a.nm_tipo_norma_vide == b.nm_tipo_norma_vide) {
                    if (parseInt(a.nr_norma_vide) && parseInt(b.nr_norma_vide)) {
                        if (parseInt(a.nr_norma_vide) < parseInt(b.nr_norma_vide)) {
                            return -1;
                        }
                        else if (parseInt(a.nr_norma_vide) > parseInt(b.nr_norma_vide)) {
                            return 1;
                        }
                    }
                    return 0;
                }
                else {
                    var a_dispositivo_afetado = "";
                    if (a.in_norma_afetada) {
                        if (IsNotNullOrEmpty(a.caput_norma_vide, 'caput')) {
                            a_dispositivo_afetado = a.caput_norma_vide.caput[0];
                        }
                        else {
                            a_dispositivo_afetado += (IsNotNullOrEmpty(a.artigo_norma_vide) ? "Art. " + a.artigo_norma_vide.replace(/^([1-9]|9)$/, a.artigo_norma_vide + 'º') + ", " : "");
                            a_dispositivo_afetado += (IsNotNullOrEmpty(a.paragrafo_norma_vide) ? "Par. " + a.paragrafo_norma_vide.replace(/^([1-9]|9)$/, a.paragrafo_norma_vide + 'º') + ", " : "");
                            a_dispositivo_afetado += (IsNotNullOrEmpty(a.anexo_norma_vide) ? "Anexo " + a.anexo_norma_vide + ", " : "");
                            a_dispositivo_afetado += (IsNotNullOrEmpty(a.inciso_norma_vide) ? "inc. " + a.inciso_norma_vide + ", " : "");
                            a_dispositivo_afetado += (IsNotNullOrEmpty(a.alinea_norma_vide) ? a.alinea_norma_vide + ", " : "");
                            a_dispositivo_afetado += ((IsNotNullOrEmpty(a.caput_norma_vide) && (a.caput_norma_vide == true)) ? "Caput. " : "");
                            a_dispositivo_afetado += (IsNotNullOrEmpty(a.item_norma_vide) ? "Item " + a.item_norma_vide : "");
                        }
                    }
                    else {
                        if (IsNotNullOrEmpty(a.caput_norma_vide_outra, 'caput')) {
                            a_dispositivo_afetado = a.caput_norma_vide_outra.caput[0];
                        }
                        else {
                            a_dispositivo_afetado += (IsNotNullOrEmpty(a.artigo_norma_vide_outra) ? "Art. " + a.artigo_norma_vide_outra.replace(/^([1-9]|9)$/, a.artigo_norma_vide_outra + 'º') + ", " : "");
                            a_dispositivo_afetado += (IsNotNullOrEmpty(a.paragrafo_norma_vide_outra) ? "Par. " + a.paragrafo_norma_vide_outra.replace(/^([1-9]|9)$/, a.paragrafo_norma_vide_outra + 'º') + ", " : "");
                            a_dispositivo_afetado += (IsNotNullOrEmpty(a.anexo_norma_vide_outra) ? "Anexo " + a.anexo_norma_vide_outra + ", " : "");
                            a_dispositivo_afetado += (IsNotNullOrEmpty(a.inciso_norma_vide_outra) ? "inc. " + a.inciso_norma_vide_outra + ", " : "");
                            a_dispositivo_afetado += (IsNotNullOrEmpty(a.alinea_norma_vide_outra) ? a.alinea_norma_vide_outra + ", " : "");
                            a_dispositivo_afetado += ((IsNotNullOrEmpty(a.caput_norma_vide_outra) && (a.caput_norma_vide_outra == true)) ? "Caput. " : "");
                            a_dispositivo_afetado += (IsNotNullOrEmpty(a.item_norma_vide_outra) ? "Item " + a.item_norma_vide_outra : "");
                        }
                    }
                    if (a_dispositivo_afetado.substring(a_dispositivo_afetado.length - 2) == ", ") {
                        a_dispositivo_afetado = a_dispositivo_afetado.substring(0, a_dispositivo_afetado.length - 2);
                    }
                    var b_dispositivo_afetado = "";
                    if (b.in_norma_afetada) {
                        if (IsNotNullOrEmpty(b.caput_norma_vide, 'caput')) {
                            b_dispositivo_afetado = b.caput_norma_vide.caput[0];
                        }
                        else {
                            b_dispositivo_afetado += (IsNotNullOrEmpty(b.artigo_norma_vide) ? "Art. " + b.artigo_norma_vide.replace(/^([1-9]|9)$/, b.artigo_norma_vide + 'º') + ", " : "");
                            b_dispositivo_afetado += (IsNotNullOrEmpty(b.paragrafo_norma_vide) ? "Par. " + b.paragrafo_norma_vide.replace(/^([1-9]|9)$/, b.paragrafo_norma_vide + 'º') + ", " : "");
                            b_dispositivo_afetado += (IsNotNullOrEmpty(b.anexo_norma_vide) ? "Anexo " + b.anexo_norma_vide + ", " : "");
                            b_dispositivo_afetado += (IsNotNullOrEmpty(b.inciso_norma_vide) ? "inc. " + b.inciso_norma_vide + ", " : "");
                            b_dispositivo_afetado += (IsNotNullOrEmpty(b.alinea_norma_vide) ? b.alinea_norma_vide + ", " : "");
                            b_dispositivo_afetado += ((IsNotNullOrEmpty(b.caput_norma_vide) && (b.caput_norma_vide == true)) ? "Caput. " : "");
                            b_dispositivo_afetado += (IsNotNullOrEmpty(b.item_norma_vide) ? "Item " + b.item_norma_vide : "");
                        }
                    }
                    else {
                        if (IsNotNullOrEmpty(b.caput_norma_vide_outra, 'caput')) {
                            b_dispositivo_afetado = b.caput_norma_vide_outra.caput[0];
                        }
                        else {
                            b_dispositivo_afetado += (IsNotNullOrEmpty(b.artigo_norma_vide_outra) ? "Art. " + b.artigo_norma_vide_outra.replace(/^([1-9]|9)$/, b.artigo_norma_vide_outra + 'º') + ", " : "");
                            b_dispositivo_afetado += (IsNotNullOrEmpty(b.paragrafo_norma_vide_outra) ? "Par. " + b.paragrafo_norma_vide_outra.replace(/^([1-9]|9)$/, b.paragrafo_norma_vide_outra + 'º') + ", " : "");
                            b_dispositivo_afetado += (IsNotNullOrEmpty(b.anexo_norma_vide_outra) ? "Anexo " + b.anexo_norma_vide_outra + ", " : "");
                            b_dispositivo_afetado += (IsNotNullOrEmpty(b.inciso_norma_vide_outra) ? "inc. " + b.inciso_norma_vide_outra + ", " : "");
                            b_dispositivo_afetado += (IsNotNullOrEmpty(b.alinea_norma_vide_outra) ? b.alinea_norma_vide_outra + ", " : "");
                            b_dispositivo_afetado += ((IsNotNullOrEmpty(b.caput_norma_vide_outra) && (b.caput_norma_vide_outra == true)) ? "Caput. " : "");
                            b_dispositivo_afetado += (IsNotNullOrEmpty(b.item_norma_vide_outra) ? "Item " + b.item_norma_vide_outra : "");
                        }
                    }
                    if (b_dispositivo_afetado.substring(b_dispositivo_afetado.length - 2) == ", ") {
                        b_dispositivo_afetado = b_dispositivo_afetado.substring(0, b_dispositivo_afetado.length - 2);
                    }
                    if (a_dispositivo_afetado < b_dispositivo_afetado) {
                        return -1;
                    }
                    if (a_dispositivo_afetado > b_dispositivo_afetado) {
                        return 1;
                    }
                    else {
                        return 0;
                    }
                }
            }
        }
        else {
            return 0;
        }
    });
}

function AdicionarNaCesta(id_doc) {
    var adicionado = false;
    if (IsNotNullOrEmpty(id_doc)) {
        if (IsNotNullOrEmpty($.cookie('sinj_basket'))) {
            var cesta = $.cookie('sinj_basket').split(',');
            if (cesta.indexOf(id_doc) <= -1) {
                cesta.push(id_doc);
                $.cookie('sinj_basket', cesta.join(), { expires: 7, path: '/' });
                adicionado = true;
            }
            else {
                $('<div id="modal_adicionar_cesta">').modallight({
                    sType: "alert",
                    sTitle: "Erro",
                    sContent: "Essa norma já está em sua cesta."
                });
            }
        }
        else {
            $.cookie('sinj_basket', id_doc, { expires: 7, path: '/' });
            adicionado = true;
        }
    }
    if (adicionado) {
        $('<div id="modal_adicionar_cesta" />').modallight({
            sType: "success",
            sTitle: "Sucesso",
            sContent: "Norma adicionada com sucesso."
        });
    }
}

function selecionarDiario(id_tr, id_input_ar) {
    var json_arquivo_diario = $('#' + id_input_ar).val();
    var json_diario = $('#' + id_tr + ' input[name="json_diario"]').val();
    var ds_diario = $('#' + id_tr + ' input[name="json_diario"]').closest('td').text();
    $('#json_diario_fonte').val(json_diario);
    $('#json_arquivo_diario_fonte').val(json_arquivo_diario);
    $('#ds_diario').val(ds_diario);
    $('#modal_fonte .consultar').hide();
    $('#modal_fonte .resultado').hide();
    $('#modal_fonte .diario').show();
}

function consultarDiario(id_form) {
    try {
        Validar(id_form);
    } catch (ex) {
        $('#modal_fonte .notify').messagelight({
            sTitle: "Erro nos dados informados",
            sContent: ex,
            sType: "error"
        });
        return false;
    }
    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            notificar('#modal_fonte', data.error_message, 'error');
        }
        else if (IsNotNullOrEmpty(data, 'results')) {
            $('#tbody_diarios tr').remove();
            for (var i = 0; i < data.results.length; i++) {
                var id_tr = 'tr_' + data.results[i].ch_diario;
                var json_arquivo = {};
                var arquivo = '';
                if (IsNotNullOrEmpty(data.results[i], 'arquivos')) {
                    for (var j = 0; j < data.results[i].arquivos.length; j++) {
                        json_arquivo['ar_' + i + '_' + j] = data.results[i].arquivos[j].arquivo_diario;
                        arquivo += '<input id="ar_' + i + '_' + j + '" type="hidden" value="" />';
                        arquivo += '<img class="middle" src="' + _urlPadrao + '/Imagens/ico_download_p.png" alt="" /> ' + data.results[i].arquivos[j].ds_arquivo + ' <a class="button" href="javascript:void(0);" onclick="javascript:selecionarDiario(\'' + id_tr + '\', \'ar_' + i + '_' + j + '\');" title="Selecionar Diário"><img src="' + _urlPadrao + '/Imagens/ico_check_p.png" width="10px" /> Selecionar Arquivo</a><br/>';
                    }
                }
                else {
                    json_arquivo['ar_' + i + '_0'] = data.results[i].ar_diario;
                    arquivo += '<input id="ar_' + i + '_0" type="hidden" value="" />';
                    arquivo += '<img class="middle" src="' + _urlPadrao + '/Imagens/ico_download_p.png" alt="" /> <a class="button" href="javascript:void(0);" onclick="javascript:selecionarDiario(\'' + id_tr + '\', \'ar_' + i + '_0\');" title="Selecionar Diário"><img src="' + _urlPadrao + '/Imagens/ico_check_p.png" width="10px" /> Selecionar Arquivo</a><br/>';
                }
                var tr = '<tr id="' + id_tr + '">' +
                            '<td><input type="hidden" name="json_diario" value="" />' + montarDescricaoDiario(data.results[i]) + '</td>' +
                            '<td>' + arquivo + '</td>' +
                        '</tr>';
                $('#tbody_diarios').append($(tr));
                $('#' + id_tr + ' input[name="json_diario"]').val(JSON.stringify(data.results[i]));
                for (var key in json_arquivo) {
                    $('#' + key).val(JSON.stringify(json_arquivo[key]));
                }
            }
            $('#modal_fonte .resultado').show();
        }
    }
    $.ajaxlight({
        sFormId: id_form,
        sUrl: './ashx/Consulta/DiarioConsulta.ashx',
        sType: "POST",
        fnSuccess: sucesso,
        fnComplete: gComplete,
        fnBeforeSubmit: gInicio,
        bAsync: true
    });
    
    return false;
}

function adicionarNormaNaCesta(id_doc) {
    if (IsNotNullOrEmpty(id_doc)) {
        if (IsNotNullOrEmpty($.cookie('sinj_basket'))) {
            var cesta = $.cookie('sinj_basket').split(',');
            if (cesta.indexOf(id_doc) <= -1) {
                cesta.push(id_doc);
                $.cookie('sinj_basket', cesta.join(), { expires: 7, path: '/' });
            }
        }
        else $.cookie('sinj_basket', id_doc, { expires: 7, path: '/' });
        ShowDialog({ sTitle: 'Cesta', sContent: 'Adicionado à cesta com sucesso.', sType: 'success' });
        exibirBotaoCesta(id_doc);
    }
}

function removerNormaDaCesta(id_doc) {
    var cookie_cesta = $.cookie('sinj_basket');
    if (IsNotNullOrEmpty(cookie_cesta)) {
        var cesta = cookie_cesta.split(',');
        var cesta_aux = [];
        if (cesta.indexOf(id_doc) > -1) {
            for (var i = 0; i < cesta.length; i++) {
                if (cesta[i] != id_doc) {
                    cesta_aux.push(cesta[i]);
                }
            }
            $.cookie('sinj_basket', cesta_aux.join(), { expires: 7, path: '/' });
            ShowDialog({ sTitle: 'Cesta', sContent: 'Removido da cesta com sucesso.', sType: 'success' });
        }
    }
    exibirBotaoCesta(id_doc);
}

function exibirBotaoCesta(id_doc) {
    var bExiste = false;
    var cookie_cesta = $.cookie('sinj_basket');
    if (IsNotNullOrEmpty(cookie_cesta)) {
        var cesta = cookie_cesta.split(',');
        if (cesta.indexOf(id_doc) > -1) {
            bExiste = true;
        }
    }
    if (bExiste) {
        $('#a_cesta').attr('title', 'Remover da Cesta');
        $('#a_cesta').attr('onclick', 'javascript:removerNormaDaCesta(\'' + id_doc + '\')');
        $('#a_cesta').html('<img width="25" alt="cesta" src="' + _urlPadrao + '/Imagens/ico_del_basket_p.png"/>');
    }
    else {
        $('#a_cesta').attr('title', 'Adicionar na Cesta');
        $('#a_cesta').attr('onclick', 'javascript:adicionarNormaNaCesta(\'' + id_doc + '\')');
        $('#a_cesta').html('<img width="25" alt="cesta" src="' + _urlPadrao + '/Imagens/ico_basket_p.png"/>');
    }
}

function changeStSiuacaoForcada(el) {
    if ($(el).prop('checked')) {
        $('.line_situacao').show();
        $('#ch_situacao').attr('obrigatorio', 'sim');
    }
    else {
        $('.line_situacao').hide();
        $('#ch_situacao').attr('obrigatorio', 'nao');
    }
}

function abrirModalRecuperarArquivo(el) {
    $('#input_arquivo_recuperar').val('');
    $('#form_recuperar_arquivo').attr('onsubmit', 'javascript:return recuperarArquivo(\'form_recuperar_arquivo\');')
    $('#div_modal_recuperar_arquivo').modallight({
        sTitle: "Recuperar Documento",
        sHeight: "auto",
        sWidth: "850",
        oPosition: "center",
        oButtons: [
                    { html: '<img alt="importar" valign="absmiddle" src="' + _urlPadrao + '/Imagens/ico_check_p.png" width="20" height="20" /> Recuperar', click: function () { form_recuperar_arquivo.onsubmit(); } },
                    { html: '<img alt="cancelar" valign="absmiddle" src="' + _urlPadrao + '/Imagens/ico_fechar.png" width="20" height="20" /> Cancelar', click: function () { $('#div_modal_recuperar_arquivo').modallight("close"); } }
                ],
        fnClose: function () {
            $('#div_modal_recuperar_arquivo').modallight('destroy');
        }
    });

    $('#div_list_dir').html('');
    $('#div_list_arq').html('');

    $("#div_list_arquivos_versionados").dataTablesLight({
        sAjaxUrl: "./ashx/Datatable/ArquivoVersionadoDatatable.ashx?ch_norma=" + $('#form_recuperar_arquivo > input[name="ch_norma"]').val(),
        aoColumns: _columns_arquivos_versionados,
        sIdTable: 'datatable_arquivos_versionados',
        responsive: null,
        bFilter: false,
        iDisplayLength: -1,
        fnCreatedRow: function (nRow, aData, iDataIndex) {
            nRow.className += " tr_arq";
            nRow.setAttribute('id_file', aData.ar_arquivo_versionado.id_file);
            nRow.setAttribute('filename', aData.ar_arquivo_versionado.filename);
            nRow.setAttribute('dt_arquivo_versionado', aData.dt_arquivo_versionado);
        },
    });
}

function selecionarDocumentoRecuperar(el) {
    var id_file = $(el).closest('tr.tr_arq').attr('id_file');
    var filename = $(el).closest('tr.tr_arq').attr('filename');
    var dt_arquivo_versionado = $(el).closest('tr.tr_arq').attr('dt_arquivo_versionado');
    $('#form_recuperar_arquivo input[name="id_file"]').val(id_file);
    $('#input_chave_arquivo_recuperar').val(filename + ', versionado em ' + dt_arquivo_versionado);
    $('#input_chave_arquivo_recuperar').closest('div.table').show();
}

function recuperarArquivo(id_form) {
    var sucesso = function (data) {
        $('#super_loading').hide();
        if (IsNotNullOrEmpty(data, 'error_message')) {
            notificar('#' + id_form, data.error_message, 'error');
        }
        else {
            if (IsNotNullOrEmpty(data, "success_message")) {
                notificar('#div_detalhes_doc', data.success_message, 'success');
            }
            if (IsNotNullOrEmpty(data, "id_file")) {
                $('#arquivo_atualizado input.json_arquivo').val(JSON.stringify(data));
                $('#arquivo_atualizado label.name').text(data.filename);
                $('#arquivo_atualizado a.attach').hide();
                $('#arquivo_atualizado a.create').hide();
                $('#arquivo_atualizado a.import').hide();
                $('#arquivo_atualizado a.recovery').hide();
                $('#arquivo_atualizado a.delete').show();
            }
            $("#input_file").val("");
            $('#div_modal_recuperar_arquivo').modallight('close');
        }
    }
    return fnSalvarForm(id_form, sucesso);
}

function selecionarVacatioLegis(){
    if($('#st_vacatio_legis').prop('checked')){
        $('#dt_inicio_vigencia').attr('obrigatorio','sim');
        $('div.line_vacatio_legis').show();
    }
    else{
        $('#dt_inicio_vigencia').val('');
        $('#dt_inicio_vigencia').attr('obrigatorio','não');
        $('div.line_vacatio_legis').hide();
    }
}