﻿/* funcoes_sinj.js
===========================================*/

var aHighlight = [];

function Pesquisar(id_form) {
    $('#' + id_form + ' .notify').html('');
    try {
        ValidarPesquisa(id_form);
        Validar(id_form);
        return true;
    } catch (ex) {
        $('#' + id_form + ' .notify').messagelight({
            sTitle: "Erro",
            sContent: ex,
            sType: "error"
        });
    }
    return false;
}

function fnExcluir(id_datatable) {
    $('#div_notificacao_modal_exclusao').html('');
    var ajax = MostrarPaginaAjax('EXC');
    if (IsNotNullOrEmpty(ajax)) {
        try {
            Validar(form_exclusao);
            var sucesso = function(data) {
                if (IsNotNullOrEmpty(data)) {
                    $('#div_loading_modal_exclusao').hide();
                    $('#div_buttons_modal_exclusao').show();
                    if (data.error_message != null && data.error_message != "") {
                        $('#div_notificacao_modal_exclusao').messagelight({
                            sTitle: "Erro",
                            sContent: data.error_message,
                            sType: "error",
                            sWidth: "",
                            iTime: null
                        });
                    } else if (data.excluido != null && data.excluido == true) {
                        $('<div id="modal_notificacao_modal_exclusao" />').modallight({
                            sTitle: "Sucesso",
                            sContent: "Excluído com sucesso.",
                            sType: "success",
                            oButtons: [{
                                text: "Ok",
                                click: function() {
                                    $(this).dialog('close');
                                }
                            }],
                            fnClose: function() {
                                if (!Redirecionar()) {
                                    if (IsNotNullOrEmpty(id_datatable)) {
                                        $('#' + id_datatable + ' tr.selected').remove();
                                    } else {
                                        location.reload();
                                    }
                                }
                            }
                        });
                        $('#modal_exclusao').dialog("close");
                    } else {
                        $('#div_notificacao_modal_exclusao').messagelight({
                            sTitle: "Erro",
                            sContent: "Erro na exclusão.",
                            sType: "error",
                            sWidth: "",
                            iTime: null
                        });
                    }
                }
            }
            var beforeSubmit = function() {
                $('#div_loading_modal_exclusao').show();
                $('#div_buttons_modal_exclusao').hide();
            };
            $.ajaxlight({
                sUrl: ajax,
                sType: "POST",
                fnSuccess: sucesso,
                sFormId: "form_exclusao",
                fnBeforeSubmit: beforeSubmit
            });
        } catch (ex) {
            $("#div_notificacao_modal_exclusao").messagelight({
                sTitle: "Erro nos dados informados",
                sContent: ex,
                sType: "error"
            });
        }
    }
    return false;
}

function Excluir(id_doc, id_datatable) {
    var html = '<form id="form_exclusao" name="formExclusao" action="#" method="POST">' +
        '<input id="id_doc" type="hidden" value="' + id_doc + '" label="ID" obrigatorio="sim" name="id_doc" />' +
        '<div class="mauto table">' +
        '<div class="line">' +
        '<div class="column w-20-pc">' +
        '<div class="cell fr">' +
        '<label>Justificativa:</label>' +
        '</div>' +
        '</div>' +
        '<div class="column w-70-pc">' +
        '<div class="cell w-100-pc">' +
        '<textarea label="Justificativa" obrigatorio="sim" id="justificativa" name="justificativa" class="w-90-pc" cols="100" rows="5" style="max-width:100%;" ></textarea>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div id="div_notificacao_modal_exclusao" style="display:none;">' +
        '</div>' +
        '<div id="div_loading_modal_exclusao" class="loading" style="display:none;"></div>' +
        '<div id="div_buttons_modal_exclusao" style="width:220px; margin:auto;" class="loaded">' +
        '<button id="button_excluir" onclick="javascript:return fnExcluir(\'' + id_datatable + '\');">' +
        '<img src="' + _urlPadrao + '/Imagens/ico_disk_p.png" />Excluir' +
        '</button>' +
        '<button type="reset">' +
        '<img src="' + _urlPadrao + '/Imagens/ico_eraser_p.png" />Limpar' +
        '</button>' +
        '</div>' +
        '</form>';
    if ($('#modal_exclusao').length > 0) {
        $('#modal_exclusao').html(html);
        $('#modal_exclusao').dialog('open');
    } else {
        $('<div id="modal_exclusao" />').modallight({
            sTitle: "Excluir",
            sType: "default",
            sContent: html,
            sWidth: 500,
            oButtons: []
        });
    }
}

function fnSalvar(id_form, action, _sucesso) {
    $('#' + id_form + ' .notify').html('');
    try {
        Validar(id_form);
        ValidarRegrasEspecificas(id_form);
        var sucesso = typeof _sucesso === "function" ? _sucesso : function(data) {
            if (IsNotNullOrEmpty(data)) {
                gComplete();
                if (data.error_message != null && data.error_message != "") {
                    if (IsNotNullOrEmpty(data.ids_duplicados)) {
                        var links = "";
                        for (var i = 0; i < data.ids_duplicados.length; i++) {
                            links += '<br/><a target="_blank" href="./DetalhesDe' + MostrarPaginaSemPrefixo() + '?id_doc=' + data.ids_duplicados[i] + '">Visualizar <img src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" alt="detalhes" /></a>';
                        }
                        $('<div id="modal_notificacao_ids_duplicados" />').modallight({
                            sTitle: "Erro",
                            sContent: data.error_message + links,
                            sType: "error",
                            oButtons: [{
                                text: "Ok",
                                click: function() {
                                    $(this).dialog('close');
                                }
                            }]
                        });
                    } else {
                        $('#' + id_form + ' .notify').messagelight({
                            sTitle: "Erro",
                            sContent: data.error_message,
                            sType: "error",
                            sWidth: "",
                            iTime: null
                        });
                    }
                } else if (IsNotNullOrEmpty(data, 'id_doc_success')) {
                    if (IsNotNullOrEmpty(data, 'filhos_errados')) {
                        $('<div id="modal_notificacao_modal_salvar" />').modallight({
                            sTitle: "Atenção",
                            sContent: "Salvo com sucesso.<br/>No entanto, os filhos " + data.filhos_errados + " não foram herdados.",
                            sType: "alert",
                            oButtons: [{
                                text: "Ok",
                                click: function() {
                                    $(this).dialog('close');
                                }
                            }],
                            fnClose: function() {
                                if (!Redirecionar('?id_doc=' + data.id_doc_success + "&time=" + new Date().getTime())) {
                                    location.reload();
                                }
                            }
                        });
                    } else if (IsNotNullOrEmpty(data, 'norma_st_acao') && data.norma_st_acao) {
                        $('<div id="modal_notificacao_modal_salvar" />').modallight({
                            sTitle: "Atenção",
                            sContent: "Para finalizar o cadastro da ação, é necessário cadastrar uma Vide.",
                            sType: "alert",
                            oButtons: [{
                                text: "Adicionar Vide",
                                click: function() {
                                    $(this).dialog('close');
                                }
                            }],
                            fnClose: function() {
                                Redirecionar('?id_doc=' + data.id_doc_success + "&in_acao=true&time=" + new Date().getTime(), './CadastrarVide.aspx');
                            }
                        });
                    } else {
                        $('<div id="modal_notificacao_modal_salvar" />').modallight({
                            sTitle: "Sucesso",
                            sContent: "Salvo com sucesso.",
                            sType: "success",
                            oButtons: [{
                                text: "Ok",
                                click: function() {
                                    $(this).dialog('close');
                                }
                            }],
                            fnClose: function() {
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
                } else {
                    $('#' + id_form + ' .notify').messagelight({
                        sTitle: "Erro",
                        sContent: "Erro ao salvar.",
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
            }
        };
        var beforeSubmit = function() {
            gInicio();
        };
        $.ajaxlight({
            sUrl: MostrarPaginaAjax(IsNotNullOrEmpty(action) ? action : 'SAL') + window.location.search,
            sType: "POST",
            fnSuccess: sucesso,
            sFormId: id_form,
            fnBeforeSubmit: beforeSubmit,
            bAsync: true,
            iTimeout: 60000
        });
    } catch (ex) {
        $('#' + id_form + ' .notify').messagelight({
            sTitle: "Erro nos dados informados",
            sContent: ex,
            sType: "error"
        });
        $("html, body").animate({
            scrollTop: 0
        }, "slow");
    }
    return false;
}

function DeletarArquivo(event) {
    var parent = $(event.target).parent().parent();
    var id_hidden_container_uuid = $('.uuid', parent).attr("id");
    var id_label_container_filename = $('.name', parent).attr("id");
    var id_hidden_container_mimetype = $('.mimetype', parent).attr("id");
    var id_hidden_container_filesize = $('.filesize', parent).attr("id");
    var id_hidden_container_id_file = $('.id_file', parent).attr("id");
    var id_hidden_container_filename = $('.filename', parent).attr("id");
    if (IsNotNullOrEmpty(id_hidden_container_uuid) && IsNotNullOrEmpty(id_label_container_filename)) {
        $('#' + id_hidden_container_uuid).val('');
        $('#' + id_hidden_container_mimetype).val('');
        $('#' + id_hidden_container_filesize).val('');
        $('#' + id_hidden_container_id_file).val('');
        $('#' + id_hidden_container_filename).val('');
        $('#' + id_label_container_filename).text('');
        $('.attach', parent).show();
        $('.create', parent).show();
        $('.delete', parent).hide();
    }
}

function deletarInputFile(el) {
    var parent = $(el).parent();
    $('.name', parent).text('');
    $('.json_arquivo', parent).val('');
    $('a.attach', parent).show();
    $('a.create', parent).show();
    $('a.delete', parent).hide();
}

function salvarArquivoSelecionado(el) {
    var id_div_file = el.getAttribute('id_div_file');
    if (IsNotNullOrEmpty(id_div_file) && IsNotNullOrEmpty(el, 'value')) {
        var sucesso = function(data) {
            $('#super_loading').hide();
            if (data.error_message != null && data.error_message != "") {
                notificar('#' + id_div_file, data.error_message, 'error');
            } else if (IsNotNullOrEmpty(data, "id_file")) {
                $('#' + id_div_file + ' input.json_arquivo').val(JSON.stringify(data));
                $('#' + id_div_file + ' label.name').text(data.filename);
                $('#' + id_div_file + ' a.attach').hide();
                //                $('#' + id_div_file + ' a.edit_file').attr('id_file', '');
                //                $('#' + id_div_file + ' a.edit_file').hide();
                $('#' + id_div_file + ' a.delete').show();
            }
        }
        return fnSalvarForm('form_upload_file', sucesso);
    }
}

function anexarInputFile(el) {
    var parent = $(el).parent();
    $('#form_upload_file input[name="file"]').val('');
    $('#form_upload_file input[name="file"]').attr('id_div_file', parent.attr('id'));
    $('#form_upload_file input[name="file"]').click();

    //    var parent = $(el).closest('div');
    //    var id_hidden_container_uuid = $('.uuid', parent).attr("id");
    //    var id_label_container_filename = $('.name', parent).attr("id");
    //    var id_hidden_container_mimetype = $('.mimetype', parent).attr("id");
    //    var id_hidden_container_filesize = $('.filesize', parent).attr("id");
    //    var id_hidden_container_id_file = $('.id_file', parent).attr("id");
    //    var id_hidden_container_filename = $('.filename', parent).attr("id");
    //    if ($('.uuid', parent).length == 1 && $('.name', parent).length > 0)
    //        
    //    }
}

function AnexarArquivo(event) {
    var parent = $(event.target).parent().parent();
    var id_hidden_container_uuid = $('.uuid', parent).attr("id");
    var id_label_container_filename = $('.name', parent).attr("id");
    var id_hidden_container_mimetype = $('.mimetype', parent).attr("id");
    var id_hidden_container_filesize = $('.filesize', parent).attr("id");
    var id_hidden_container_id_file = $('.id_file', parent).attr("id");
    var id_hidden_container_filename = $('.filename', parent).attr("id");
    if (IsNotNullOrEmpty(id_hidden_container_uuid) && IsNotNullOrEmpty(id_label_container_filename)) {
        var html = '<form id="form_arquivo_' + id_hidden_container_uuid + '" name="formArquivo' + id_hidden_container_uuid + '" action="#" method="POST">' +
            '<div class="mauto table loaded">' +
            '<div class="line">' +
            '<div class="column w-20-pc">' +
            '<div class="cell fr">' +
            '<label>Arquivo:</label>' +
            '</div>' +
            '</div>' +
            '<div class="column w-70-pc">' +
            '<div class="cell w-100-pc">' +
            '<input type="file" name="file" obrigatorio="sim" label="Arquivo" />' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '<div class="notify" style="display:none;"></div>' +
            '<div class="loading" style="display:none;"></div>' +
            '</form>';
        if ($('#modal_arquivo_' + id_hidden_container_uuid).length > 0) {
            $('#modal_arquivo_' + id_hidden_container_uuid).html(html);
            $('#modal_arquivo_' + id_hidden_container_uuid).dialog('open');
        } else {
            $('<div id="modal_arquivo_' + id_hidden_container_uuid + '" />').modallight({
                sTitle: "Anexar Arquivo",
                sType: "default",
                sContent: html,
                sWidth: 400,
                oButtons: [{
                    text: "Salvar",
                    click: function() {
                        $('#form_arquivo_' + id_hidden_container_uuid + ' .notify').html('');
                        try {
                            Validar('form_arquivo_' + id_hidden_container_uuid);
                            var sucesso = function(data) {
                                if (IsNotNullOrEmpty(data)) {
                                    $('#form_arquivo_' + id_hidden_container_uuid + ' .loading').hide();
                                    $('#form_arquivo_' + id_hidden_container_uuid + ' .loaded').show();
                                    $('.ui-dialog-buttonpane', $('#modal_arquivo_' + id_hidden_container_uuid).parent()).show();
                                    if (data.error_message != null && data.error_message != "") {
                                        $('#form_arquivo_' + id_hidden_container_uuid + ' .notify').messagelight({
                                            sTitle: "Erro",
                                            sContent: data.error_message,
                                            sType: "error",
                                            sWidth: "",
                                            iTime: null
                                        });
                                    } else if (IsNotNullOrEmpty(data, 'uuid')) {
                                        $('#' + id_hidden_container_uuid).val(data.uuid);
                                        $('#' + id_hidden_container_mimetype).val(data.mimetype);
                                        $('#' + id_hidden_container_filesize).val(data.filesize);
                                        $('#' + id_hidden_container_id_file).val(data.id_file);
                                        $('#' + id_hidden_container_filename).val(data.filename);
                                        $('#' + id_label_container_filename).text(data.filename);
                                        $('.attach', parent).hide();
                                        $('.delete', parent).show();
                                        $('#modal_arquivo_' + id_hidden_container_uuid).dialog('close');
                                    } else {
                                        $('#form_arquivo_' + id_hidden_container_uuid + ' .notify').messagelight({
                                            sTitle: "Erro",
                                            sContent: "Erro ao anexar arquivo.",
                                            sType: "error",
                                            sWidth: "",
                                            iTime: null
                                        });
                                    }
                                }
                            }
                            var beforeSubmit = function() {
                                $('#form_arquivo_' + id_hidden_container_uuid + ' .loading').show();
                                $('#form_arquivo_' + id_hidden_container_uuid + ' .loaded').hide();
                                $('.ui-dialog-buttonpane', $('#modal_arquivo_' + id_hidden_container_uuid).parent()).hide();
                            };
                            $.ajaxlight({
                                sUrl: MostrarPaginaAjax('ANX'),
                                sType: "POST",
                                fnSuccess: sucesso,
                                sFormId: 'form_arquivo_' + id_hidden_container_uuid,
                                fnBeforeSubmit: beforeSubmit,
                                bAsync: true,
                                iTimeout: 240000
                            });
                        } catch (ex) {
                            $('#form_arquivo_' + id_hidden_container_uuid + ' .notify').messagelight({
                                sTitle: "Erro",
                                sContent: ex,
                                sType: "error"
                            });
                        }
                    }
                }, {
                    text: "Cancelar",
                    click: function() {
                        $(this).dialog('close');
                    }
                }]
            });
        }
    }
}

function ValidarRegrasEspecificas(id_form) {
    if (id_form == "form_orgao") {
        var radio_st_orgao = $('input[type="radio"][name="st_orgao"]:checked');
        if (radio_st_orgao.length <= 0) {
            throw "O campo " + $($('input[type=\"radio\"][name=\"st_orgao\"]')[0]).attr('label') + " é obrigatório.";
        }
        var st_orgao = radio_st_orgao.val();
        var dt_inicio_vigencia = $('#dt_inicio_vigencia').val();
        var dt_fim_vigencia = $('#dt_fim_vigencia').val();
        if (IsNotNullOrEmpty(dt_fim_vigencia)) {
            if (convertStringToDateTime(dt_inicio_vigencia) > convertStringToDateTime(dt_fim_vigencia)) {
                throw "Data de Fim de Vigência não pode ser menor que a do Início de Vigência.";
            }
            if (st_orgao == "true" && convertStringToDateTime(dt_fim_vigencia) < new Date()) {
                throw "Se o Fim da Vigência é inferior à data de hoje, a situação deve ser Inativo.";
            }
        } else if (st_orgao == "false") {
            throw "Data de Fim de Vigência Obrigatório caso o Órgão esteja Inativo.";
        }
        if ($('#id_ambito').val() == "") {
            throw "O campo Âmbito é obrigatório.";
        }
    } else if (id_form == "form_norma") {
        //É obrigatório ter Origem
        if ($("input[name='orgao']").length <= 0) {
            throw "O campo Origens é obrigatório.";
        }
        //Se tipo de norma for G2 (PGDF) é obrigatório possuir decisão
        if ($('#in_g2').val() == "true" && $("input[name='decisao']").length <= 0) {
            throw "O campo Decisões é obrigatório.";
        }
        //Se tipo de norma não for G2 (PGDF) é obrigatório possuir fontes
        if ($('#in_g2').val() != "true" && $("input[name='fonte']").length <= 0) {
            throw "O campo Fontes é obrigatório.";
        }
    } else if (id_form == "form_notifiqueme") {
        if ($('#email_usuario_push').val() != $('#email_usuario_push_confirme').val()) {
            throw "E-mail inválido. Verifique se confirmou o e-mail corretamente.";
        }
        if ($('#senha_usuario_push').val() != $('#senha_usuario_push_confirme').val()) {
            throw "Senha inválida. Verifique se confirmou a senha corretamente.";
        }
        if (!$('#checkbox_ciente').prop('checked')) {
            throw "É necessário declarar que está ciente de que o serviço SINJ-DF - Notifique-Me(PUSH) - é meramente informativo, não tendo, portanto, cunho oficial";
        }
    } else if (id_form == "form_notifiqueme_atualizar") {
        if ($('#senha_usuario_push').val() != "" || $('#senha_usuario_push_confirme').val() != "" || $('#senha_usuario_push_antiga').val() != "") {
            if ($('#senha_usuario_push').val() == "") {
                throw "Campo Nova Senha inválido.";
            }
            if ($('#senha_usuario_push_confirme').val() == "") {
                throw "Campo Confirme a Senha inválido.";
            }
            if ($('#senha_usuario_push_antiga').val() == "") {
                throw "Campo Senha Antiga inválido.";
            }
            if ($('#senha_usuario_push').val() != $('#senha_usuario_push_confirme').val()) {
                throw "Senha inválida. Verifique se confirmou a senha corretamente.";
            }
        }
    } else if (id_form == "form_vide") {
        if ($('#ch_norma_alteradora').val() == $('#ch_norma_alterada').val()) {
            throw "Uma Norma não pode aplicar uma Vide para ela mesma.";
        }
    } else if (id_form == "form_relatorio_norma") {
        // Caso um dos campos esteja preenchido e o outro não, é lancado esse erro
        if ($('#dt_cadastro').val() != "" || $('#ate_dt_cadastro').val() != "") {
            if ($('#dt_cadastro').val() == "" || $('#ate_dt_cadastro').val() == "") {
                throw "Data inválida. Preencha os dois campos.";
            }
        }
    } else if (id_form == "form_usuario") {
        if ($('#senha_usuario').val() != "" || $('#redigite_senha_usuario').val() != "") {
            if ($('#senha_usuario').val() != $('#redigite_senha_usuario').val()) {
                throw "Senha inválida. Verifique se confirmou a senha corretamente."
            }
            if ($('#senha_usuario').val().length < 6) {
                throw "O campo senha est&aacute; com um tamanho m&iacute;nimo (6) inv&aacute;lido!!!";
            }
        }
    }
}

function ConstruirOrgaosCadastradores() {
    for (var i = 0; i < _orgaos_cadastradores.length; i++) {
        $('#div_orgaos_cadastradores').append('<label style="color:#333333;"><input type="checkbox" name="orgao_cadastrador" value="' + _orgaos_cadastradores[i].id_orgao_cadastrador + '" />' + _orgaos_cadastradores[i].nm_orgao_cadastrador + '</label>');
    }
}

function ConstruirSelectOrgaosCadastradores() {
    for (var i = 0; i < _orgaos_cadastradores.length; i++) {
        $('#orgao_cadastrador').append('<option value="' + _orgaos_cadastradores[i].id_orgao_cadastrador + '">' + _orgaos_cadastradores[i].nm_orgao_cadastrador + '</option>');
    }
}

function ConstruirAmbitos() {
    for (var j = 0; j < _ambitos.length; j++) {
        $('#id_ambito').append('<option value="' + _ambitos[j].id_ambito + '">' + _ambitos[j].nm_ambito + '</option>');
    }
}

function ConstruirSelectTemas() {
    for (var i = 0; i < _temas.length; i++) {
        $('#ch_tema').append('<option value="' + _temas[i].ch_tema + '">' + _temas[i].nm_tema + '</option>');
    }
}

function ContruirDivControl() {
    if (IsNotNullOrEmpty(_user, "grupos")) {
        var prefixo_grupo = MostrarPrefixoDeGrupoDaPagina();
        var pagina = MostrarPaginaSemPrefixo();
        var titulo = MostrarTituloDaPagina();
        if (ValidarPermissao(eval('_grupos.' + prefixo_grupo + '_inc'))) {
            $('#div_controls').append('<div class="div-light-button"><a href="./Cadastrar' + pagina + '"><img src="' + _urlPadrao + '/Imagens/ico_add_p.png" >Cadastrar ' + titulo + '</a></div>');
        }
        if (prefixo_grupo == 'voc') {
            if (ValidarPermissao(eval('_grupos.' + prefixo_grupo + '_ger'))) {
                $('#div_controls').append('<div class="div-light-button"><a href="./Gerenciar' + pagina + '"><img src="' + _urlPadrao + '/Imagens/ico_manage_p.png" >Gerenciar ' + titulo + '</a></div>');
            }
        }
        if (ValidarPermissao(eval('_grupos.' + prefixo_grupo + '_pes'))) {
            $('#div_controls').append('<div class="div-light-button"><a href="./Pesquisar' + pagina + '"><img src="' + _urlPadrao + '/Imagens/ico_loupe_p.png" >Pesquisar ' + titulo + '</a></div>');
        }
        if (prefixo_grupo == 'voc') {
            if (ValidarPermissao(eval('_grupos.' + prefixo_grupo + '_pes'))) {
                $('#div_controls').append('<div class="div-light-button"><a href="./Listas' + pagina + '"><img src="' + _urlPadrao + '/Imagens/ico_tree_p.png" >Listas Auxiliares</a></div>');
            }
        }
    }
}

function Texto(url) {
    $('#modal_texto .notify').hide();
    $('#modal_texto .text').text('');
    $('#modal_texto .text').hide();
    $('#modal_texto').modallight({
        sWidth: 800,
        sHeight: 400,
        sTitle: "Texto",
        sType: "default",
        fnClose: function() {
            $('#modal_texto .text').text('');
            $('#modal_texto').dialog('destroy');
        }
    });
    var sucesso = function(data) {
        if (IsNotNullOrEmpty(data, 'filetext')) {
            $('#modal_texto .text').text(data.filetext);
        } else if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#modal_texto .notify').messagelight({
                sContent: data.error_message,
                sType: "error"
            });
        } else {
            $('#modal_texto .notify').messagelight({
                sContent: "Ocorreu um erro desconhecido.",
                sType: "error"
            });
        }
    }
    var inicio = function(data) {
        $('#modal_texto .loading').show();
        $('#modal_texto .notify').hide();
        $('#modal_texto .text').hide();
    }
    var complete = function(data) {
        $('#modal_texto .text').show();
        $('#modal_texto .loading').hide();
    }
    $.ajaxlight({
        sUrl: url,
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: complete,
        fnBeforeSend: inicio,
        bAsync: true,
        iTime: 120000
    });
}

function prepararCampoDownloadDeDocumento(id_parent, arquivoOv, urlDownload, urlPadrao) {
    if (IsNotNullOrEmpty(arquivoOv.id_file)) {
        $('#' + id_parent).html('<a id="link-' + div + '" href="' + urlDownload + '"><img src="' + (IsNotNullOrEmpty(urlPadrao) ? urlPadrao : '.') + '/Imagens/ico_download_p.png"/>' + getVal(arquivoOv.filename) + '</a>');
        $(document).on('click', 'a#link-' + div, function() {
            goDownload($(this).prop('href'), null, false);
            return false;
        });
    }
}

function goDownload(sLink, data, isRelatorio) {
    var title = "Preparando Download...";
    var msg = "Download efetuado com sucesso!";
    if (IsNotNullOrEmpty(isRelatorio)) {
        title = "Gerando Relatório...";
        msg = "Relatório gerado com sucesso!";
    }
    $.fileDownload(sLink, {
        internalCallbacks: $('<div id="modal_notificacao_preparando_download" />').modallight({
            sTitle: title,
            sContent: '<div class="ui-progressbar-value ui-corner-left ui-corner-right"></div>',
            sType: "info",
            oButtons: [{
                text: "Ok",
                click: function() {
                    $(this).dialog('close');
                }
            }],
            fnClose: function() {
                $(this).remove();
            }
        }),
        successCallback: function(url) {
            $('<div id="modal_notificacao_sucesso_download" />').modallight({
                sTitle: "Sucesso",
                sContent: msg,
                sType: "success",
                oButtons: [{
                    text: "Ok",
                    click: function() {
                        $(this).dialog('close');
                    }
                }],
                fnClose: function() {
                    $(this).remove();
                }
            });
        },
        data: data,
        failCallback: function(responseHtml, url) {
            $('<div id="modal_notificacao_erro_download" />').modallight({
                sTitle: "Erro",
                sContent: responseHtml,
                sType: "error",
                oButtons: [{
                    text: "Ok",
                    click: function() {
                        $(this).dialog('close');
                    }
                }],
                fnClose: function() {
                    $(this).remove();
                }
            });
        }
    });
    return false;
}

function DeletarLinha(event) {
    var tbody = $(event.target).closest("tbody");
    var table = $(event.target).closest("table");
    var thead = $('thead', table);
    var th = $('th', thead);
    $(event.target).closest("tr").remove();
    var trs = $('tr', tbody);
    if (trs.length <= 0) {
        $(tbody).append('<tr class="tr_vazia"><td colspan="' + th.length + '"></td></tr>');
    }
}

function DeletarLinhas(id_table) {
    var tbody = $('#' + id_table + ' tbody');
    var thead = $('#' + id_table + ' thead');
    var th = $('th', thead);
    $(tbody).html('<tr class="tr_vazia"><td colspan="' + th.length + '"></td></tr>');
}

function LimparModal(id_modal) {
    $('#' + id_modal + ' input').val('');
    var tables = $('#' + id_modal + ' table');
    for (var i = 0; i < tables.length; i++) {
        var tbody = $('tbody', tables[i]);
        var thead = $('thead', tables[i]);
        var th = $('th', thead);
        $(tbody).html('<tr class="tr_vazia"><td colspan="' + th.length + '"></td></tr>');
    }
}


// Função que preenche os span com os dados
function DetalhesCadastro(usuario_cadastro, dt_cadastro) {
    $('#span_nome_usuario_cadastro').after(usuario_cadastro);
    $('#span_data_cadastro').after(dt_cadastro);
}


// Função que preenche os span com os dados de cada alteração
function DetalhesAlteracoes(lista_alteracoes) {
    if (IsNotNullOrEmpty(lista_alteracoes)) {
        for (alteracao in lista_alteracoes) // loop de repetição para cada alteração na lista
        {
            $('#div_alteracoes').append(
                '<span>' + lista_alteracoes[alteracao].nm_login_usuario_alteracao + '</span> - &nbsp;' + // preenche o primeiro span com o nome do usuario que fez a alteração e insere um espaço em branco ao final
                '<span>' + lista_alteracoes[alteracao].dt_alteracao + '</span> <br/>'); // preenche o segundo span com a data e hora da alteração
        }
    } else {
        $('#div_alteracoes').append('<span> Não há alterações </span>');
    }

}


//Função para exibir a div que contém os detalhes do Cadastro          
function ExpandirDadosDeCadastro() {
    $('#div_detalhes_cadastro').toggle();
}

//Função para exibir a div que contém as Alterações
function ExpandirAlteracoes() {
    $('#div_alteracoes').toggle();
}

/* dados_estaticos.js
========================================================*/

var tiposBuscaOrigem = [{
    nm_tipoBuscaOrigem: "emQualquerEpoca",
    ds_tipoBuscaOrigem: "Em Qualquer Época"
}, {
    nm_tipoBuscaOrigem: "hierarquiaInferior",
    ds_tipoBuscaOrigem: "Hierarquia Inferior"
}, {
    nm_tipoBuscaOrigem: "hierarquiaSuperior",
    ds_tipoBuscaOrigem: "Hierarquia Superior"
}, {
    nm_tipoBuscaOrigem: "somenteOOrgaoSelecionado",
    ds_tipoBuscaOrigem: "Somente o Órgão Selecionado"
}, {
    nm_tipoBuscaOrigem: "todaAHierarquia",
    ds_tipoBuscaOrigem: "Toda a Hierarquia"
}, {
    nm_tipoBuscaOrigem: "todaAHierarquiaEmQualquerEpoca",
    ds_tipoBuscaOrigem: "Toda a Hierarquia em Qualquer Época"
}];

var anosDeAssinatura = new function() {
    this.minYear = 1960;
    this.maxYear = new Date().getFullYear();
    this.array = [];
    for (this.minYear; this.minYear <= this.maxYear; this.minYear++) {
        this.array.push({
            ano: this.minYear
        });
    }
    return this.array;
}

var _campos_pesquisa_avancada = [{
    ch_campo: "all",
    nm_campo: "{Qualquer Campo}",
    type: "text"
}, {
    ch_campo: "id_ambito",
    nm_campo: "Âmbito",
    type: "autocomplete",
    data: '_ambitos',
    url_ajax: null,
    sKeyDataName: "id_ambito",
    sValueDataName: "nm_ambito"
}, {
    ch_campo: "ano_assinatura",
    nm_campo: "Ano de Assinatura",
    type: "number"
}, {
    ch_campo: "nm_apelido",
    nm_campo: "Apelido",
    type: "text"
}, {
    ch_campo: "ch_autoria",
    nm_campo: "Autorias",
    type: "autocomplete",
    data: null,
    url_ajax: "./ashx/Autocomplete/AutoriaAutocomplete.ashx",
    sKeyDataName: "ch_autoria",
    sValueDataName: "nm_autoria"
}, {
    ch_campo: "dt_assinatura",
    nm_campo: "Data de Assinatura",
    type: "date"
}, {
    ch_campo: "dt_assinatura",
    nm_campo: "Data de Autuação",
    type: "date"
}, {
    ch_campo: "dt_publicacao",
    nm_campo: "Data de Publicação",
    type: "date"
}, {
    ch_campo: "ds_ementa",
    nm_campo: "Ementa",
    type: "text",
    ch_operador_padrao: "contem",
    nm_operador_padrao: "contém"
}, {
    ch_campo: "ch_termo",
    nm_campo: "Indexação",
    type: "autocomplete",
    data: null,
    url_ajax: "./ashx/Autocomplete/VocabularioAutocomplete.ashx?pes=true",
    sKeyDataName: "ch_termo",
    sValueDataName: "nm_termo"
}, {
    ch_campo: "ch_interessado",
    nm_campo: "Interessados",
    type: "autocomplete",
    data: null,
    url_ajax: "./ashx/Autocomplete/InteressadoAutocomplete.ashx",
    sKeyDataName: "ch_interessado",
    sValueDataName: "nm_interessado"
}, {
    ch_campo: "nm_pessoa_fisica_e_juridica",
    nm_campo: "Lista de Nomes",
    type: "text",
    data: null,
    url_ajax: null
}, {
    ch_campo: "nr_norma",
    nm_campo: "Número da Norma/Ação",
    type: "text",
    data: null,
    url_ajax: null
}, {
    ch_campo: "ds_observacao",
    nm_campo: "Observação da Norma",
    type: "text",
    data: null,
    url_ajax: null
}, {
    ch_campo: "ch_orgao",
    nm_campo: "Órgão de Origem",
    type: "autocomplete",
    data: null,
    url_ajax: "./ashx/Autocomplete/OrgaoAutocomplete.ashx",
    sKeyDataName: "ch_orgao",
    sValueDataName: "get_sg_hierarquia_nm_vigencia"
}, {
    ch_campo: "ch_procurador",
    nm_campo: "Procurador Responsável",
    type: "autocomplete",
    data: null,
    url_ajax: "./ashx/Autocomplete/ProcuradorAutocomplete.ashx",
    sKeyDataName: "ch_procurador",
    sValueDataName: "nm_procurador"
}, {
    ch_campo: "ch_relator",
    nm_campo: "Relator (Ação)",
    type: "autocomplete",
    data: null,
    url_ajax: "./ashx/Autocomplete/RelatorAutocomplete.ashx",
    sKeyDataName: "ch_relator",
    sValueDataName: "nm_relator"
}, {
    ch_campo: "ch_requerente",
    nm_campo: "Requerente",
    type: "autocomplete",
    data: null,
    url_ajax: "./ashx/Autocomplete/RequerenteAutocomplete.ashx",
    sKeyDataName: "ch_requerente",
    sValueDataName: "nm_requerente"
}, {
    ch_campo: "ch_requerido",
    nm_campo: "Requerido",
    type: "autocomplete",
    data: null,
    url_ajax: "./ashx/Autocomplete/RequeridoAutocomplete.ashx",
    sKeyDataName: "ch_requerido",
    sValueDataName: "nm_requerido"
}, {
    ch_campo: "ch_situacao",
    nm_campo: "Situação",
    type: "autocomplete",
    data: null,
    url_ajax: "./ashx/Autocomplete/SituacaoAutocomplete.ashx",
    sKeyDataName: "ch_situacao",
    sValueDataName: "nm_situacao"
}, {
    ch_campo: "ch_tipo_relacao",
    nm_campo: "Relação de Vides",
    type: "autocomplete",
    data: null,
    url_ajax: "./ashx/Autocomplete/TipoDeRelacaoAutocomplete.ashx",
    sKeyDataName: "ch_tipo_relacao",
    sValueDataName: "nm_tipo_relacao"
}, {
    ch_campo: "filetext",
    nm_campo: "Texto Consolidado",
    type: "text",
    ch_operador_padrao: "contem",
    nm_operador_padrao: "contém"
}, {
    ch_campo: "filetext",
    nm_campo: "Texto da Ação",
    type: "text"
}];

var _operadores_pesquisa_avancada = {
    "text": [{
        ch_operador: "igual",
        nm_operador: "igual a"
    }, {
        ch_operador: "contem",
        nm_operador: "contém"
    }],
    "number": [{
        ch_operador: "igual",
        nm_operador: "igual a"
    }, {
        ch_operador: "maior",
        nm_operador: "maior que"
    }, {
        ch_operador: "maior_ou_igual",
        nm_operador: "maior ou igual a"
    }, {
        ch_operador: "menor",
        nm_operador: "menor que"
    }, {
        ch_operador: "menor_ou_igual",
        nm_operador: "menor ou igual a"
    }, {
        ch_operador: "diferente",
        nm_operador: "diferente"
    }, {
        ch_operador: "intervalo",
        nm_operador: "intervalo"
    }],
    "date": [{
        ch_operador: "igual",
        nm_operador: "igual a"
    }, {
        ch_operador: "maior",
        nm_operador: "maior que"
    }, {
        ch_operador: "maior_ou_igual",
        nm_operador: "maior ou igual a"
    }, {
        ch_operador: "menor",
        nm_operador: "menor que"
    }, {
        ch_operador: "menor_ou_igual",
        nm_operador: "menor ou igual a"
    }, {
        ch_operador: "diferente",
        nm_operador: "diferente"
    }, {
        ch_operador: "intervalo",
        nm_operador: "intervalo"
    }],
    "autocomplete": [{
        ch_operador: "igual",
        nm_operador: "igual a"
    }, {
        ch_operador: "diferente",
        nm_operador: "diferente de"
    }]
}

function RemoverPrefixosDaPagina(pagina) {
    return pagina.replace("Cadastrar", "").replace("DetalhesDe", "").replace("Pesquisar", "").replace("ResultadoDePesquisa", "").replace("Editar", "").replace("Gerenciar", "").replace("Listas", "");
}

function MostrarPaginaSemPrefixo() {
    return RemoverPrefixosDaPagina(file);
}

function MostrarTituloDaPagina() {
    var titulo = "";
    var mapPaginaAPP = {
        'Orgao.aspx': "Órgão",
        'Autoria.aspx': "Autoria",
        'TipoDeFonte.aspx': "Tipo de Fonte",
        'TipoDeEdicao.aspx': "Tipo de Edição",
        'TipoDeNorma.aspx': "Tipo de Norma",
        'TipoDePublicacao.aspx': "Tipo de Publicação",
        'Interessado.aspx': "Interessado",
        'Situacao.aspx': "Situação",
        'TipoDeRelacao.aspx': "Tipo de Relação",
        'TipoDeDiario.aspx': "Tipo de Diário",
        'Requerente.aspx': "Requerente",
        'Requerido.aspx': "Requerido",
        'Relator.aspx': "Relator",
        'Procurador.aspx': "Procurador",
        'Usuario.aspx': "Usuário",
        'Diario.aspx': "Diário",
        'Norma.aspx': "Norma",
        'Relatorio.aspx': "Relatório",
        'Vocabulario.aspx': "Vocabulário"
    };
    var pagina = RemoverPrefixosDaPagina(file);
    titulo = mapPaginaAPP[pagina];
    if (!IsNotNullOrEmpty(titulo)) {
        titulo = "";
    }
    return titulo;
};

function MostrarPrefixoDeGrupoDaPagina() {
    var prefixo_grupo = "";
    var mapPaginaAPP = {
        'Orgao.aspx': "org",
        'Autoria.aspx': "aut",
        'TipoDeFonte.aspx': "tdf",
        'TipoDeEdicao.aspx': "tdf",
        'TipoDeNorma.aspx': "tdn",
        'TipoDePublicacao.aspx': "tdp",
        'Interessado.aspx': "int",
        'Situacao.aspx': "sit",
        'TipoDeRelacao.aspx': "tdr",
        'TipoDeDiario.aspx': "tdd",
        'Requerente.aspx': "rqe",
        'Requerido.aspx': "rqi",
        'Relator.aspx': "rel",
        'Procurador.aspx': "pro",
        'Usuario.aspx': "usr",
        'Diario.aspx': "dio",
        'Norma.aspx': "nor",
        'Relatorio.aspx': "rio",
        'Vocabulario.aspx': "voc"
    };
    var pagina = RemoverPrefixosDaPagina(file);
    prefixo_grupo = mapPaginaAPP[pagina];
    if (!IsNotNullOrEmpty(prefixo_grupo)) {
        prefixo_grupo = "";
    }
    return prefixo_grupo;
}

function MostrarNomeDoGrupo(nome) {
    var mapPaginaAPP = {
        'org': "Orgão",
        'aut': "Autoria",
        'tdf': "Tipo de Fonte",
        'tde': "Tipo de Edição",
        'tdn': "Tipo de Norma",
        'tdp': "Tipo de Publicacao",
        'tdd': "Tipo de Diário",
        'int': "Interessado",
        'sit': "Situação",
        'tdr': "Tipo de Relação",
        'rqe': "Requerente",
        'rqi': "Requerido",
        'rel': "Relator",
        'pro': "Procurador",
        'usr': "Usuário",
        'dio': "Diário",
        'nor': "Norma",
        'voc': "Vocabulário",
        'cfg': "Configuração",
        'rio': "Relatório"
    };
    return mapPaginaAPP[nome];
}

function Redirecionar(params_get, url) {
    var mapPaginaAPP = {
        //Norma
        'DetalhesDeNorma.aspx': "./PesquisarNorma.aspx",
        'EditarNorma.aspx': "./DetalhesDeNorma.aspx",
        'CadastrarNorma.aspx': "./DetalhesDeNorma.aspx",
        //Diario
        'DetalhesDeTipoDeDiario.aspx': "./PesquisarTipoDeDiario.aspx",
        'EditarTipoDeDiario.aspx': "./DetalhesDeTipoDeDiario.aspx",
        'CadastrarTipoDeDiario.aspx': "./DetalhesDeTipoDeDiario.aspx",
        //Tipo de Diario
        'DetalhesDeDiario.aspx': "./PesquisarDiario.aspx",
        'EditarDiario.aspx': "./DetalhesDeDiario.aspx",
        'CadastrarDiario.aspx': "./DetalhesDeDiario.aspx",
        //Orgão
        'DetalhesDeOrgao.aspx': "./PesquisarOrgao.aspx",
        'EditarOrgao.aspx': "./DetalhesDeOrgao.aspx",
        'CadastrarOrgao.aspx': "./DetalhesDeOrgao.aspx",
        //Autoria
        'DetalhesDeAutoria.aspx': "./PesquisarAutoria.aspx",
        'CadastrarAutoria.aspx': "./DetalhesDeAutoria.aspx",
        'EditarAutoria.aspx': "./DetalhesDeAutoria.aspx",
        //TipoDeFonte
        'DetalhesDeTipoDeFonte.aspx': "./PesquisarTipoDeFonte.aspx",
        'CadastrarTipoDeFonte.aspx': "./DetalhesDeTipoDeFonte.aspx",
        'EditarTipoDeFonte.aspx': "./DetalhesDeTipoDeFonte.aspx",
        //TipoDeEdicao
        'DetalhesDeTipoDeEdicao.aspx': "./PesquisarTipoDeEdicao.aspx",
        'CadastrarTipoDeEdicao.aspx': "./DetalhesDeTipoDeEdicao.aspx",
        'EditarTipoDeEdicao.aspx': "./DetalhesDeTipoDeEdicao.aspx",
        //TipoDeNorma
        'DetalhesDeTipoDeNorma.aspx': "./PesquisarTipoDeNorma.aspx",
        'CadastrarTipoDeNorma.aspx': "./DetalhesDeTipoDeNorma.aspx",
        'EditarTipoDeNorma.aspx': "./DetalhesDeTipoDeNorma.aspx",
        //TipoDePublicacao
        'DetalhesDeTipoDePublicacao.aspx': "./PesquisarTipoDePublicacao.aspx",
        'CadastrarTipoDePublicacao.aspx': "./DetalhesDeTipoDePublicacao.aspx",
        'EditarTipoDePublicacao.aspx': "./DetalhesDeTipoDePublicacao.aspx",
        //Interessado
        'DetalhesDeInteressado.aspx': "./PesquisarInteressado.aspx",
        'CadastrarInteressado.aspx': "./DetalhesDeInteressado.aspx",
        'EditarInteressado.aspx': "./DetalhesDeInteressado.aspx",
        //Situação
        'DetalhesDeSituacao.aspx': "./PesquisarSituacao.aspx",
        'CadastrarSituacao.aspx': "./DetalhesDeSituacao.aspx",
        'EditarSituacao.aspx': "./DetalhesDeSituacao.aspx",
        //TipoDeRelacao
        'DetalhesDeTipoDeRelacao.aspx': "./PesquisarTipoDeRelacao.aspx",
        'CadastrarTipoDeRelacao.aspx': "./DetalhesDeTipoDeRelacao.aspx",
        'EditarTipoDeRelacao.aspx': "./DetalhesDeTipoDeRelacao.aspx",
        //Requerente
        'DetalhesDeRequerente.aspx': "./PesquisarRequerente.aspx",
        'CadastrarRequerente.aspx': "./DetalhesDeRequerente.aspx",
        'EditarRequerente.aspx': "./DetalhesDeRequerente.aspx",
        //Requerido
        'DetalhesDeRequerido.aspx': "./PesquisarRequerido.aspx",
        'CadastrarRequerido.aspx': "./DetalhesDeRequerido.aspx",
        'EditarRequerido.aspx': "./DetalhesDeRequerido.aspx",
        //Relator
        'DetalhesDeRelator.aspx': "./PesquisarRelator.aspx",
        'CadastrarRelator.aspx': "./DetalhesDeRelator.aspx",
        'EditarRelator.aspx': "./DetalhesDeRelator.aspx",
        //Vocabulario
        'DetalhesDeVocabulario.aspx': "./PesquisarVocabulario.aspx",
        'CadastrarVocabulario.aspx': "./DetalhesDeVocabulario.aspx",
        'EditarVocabulario.aspx': "./DetalhesDeVocabulario.aspx",
        //Relatório
        'Relatorio.aspx': "./Relatorio.aspx",
        //Procurador
        'DetalhesDeProcurador.aspx': "./PesquisarProcurador.aspx",
        'CadastrarProcurador.aspx': "./DetalhesDeProcurador.aspx",
        'EditarProcurador.aspx': "./DetalhesDeProcurador.aspx",
        //Usuario
        'DetalhesDeUsuario.aspx': "./PesquisarUsuario.aspx",
        'CadastrarUsuario.aspx': "./DetalhesDeUsuario.aspx",
        'EditarUsuario.aspx': "./DetalhesDeUsuario.aspx",
        //Notifiqueme
        'CadastrarNotifiqueme.aspx': "./LoginNotifiqueme.aspx",
        //Vide
        'CadastrarVide.aspx': "./DetalhesDeNorma.aspx",
        'EditarVide.aspx': "./DetalhesDeNorma.aspx"
    };
    var page_redirect = IsNotNullOrEmpty(url) ? url : mapPaginaAPP[file];
    if (IsNotNullOrEmpty(page_redirect)) {
        document.location.href = page_redirect + (IsNotNullOrEmpty(params_get) ? params_get : '');
        return true;
    } else {
        return false;
    }
}

function MostrarPaginaAjax(action) {
    var mapPaginaAPP = {
        //Norma
        'ResultadoDePesquisaNormao.aspx-EXC': "./ashx/Exclusao/NormaExcluir.ashx",
        'DetalhesDeNorma.aspx-VIS': "./ashx/Visualizacao/NormaDetalhes.ashx",
        'DetalhesDeNorma.aspx-EXC': "./ashx/Exclusao/NormaExcluir.ashx",
        'EditarNorma.aspx-VIS': "./ashx/Visualizacao/NormaDetalhes.ashx",
        'CadastrarNorma.aspx-SAL': "./ashx/Cadastro/NormaIncluir.ashx",
        'CadastrarNorma.aspx-ANX': "./ashx/Arquivo/NormaIncluirArquivo.ashx",
        'EditarNorma.aspx-ANX': "./ashx/Arquivo/NormaIncluirArquivo.ashx",
        'CadastrarNorma.aspx-PES': "./ashx/Consulta/NormaConsulta.ashx",
        'EditarNorma.aspx-SAL': "./ashx/Cadastro/NormaEditar.ashx",
        //Orgão
        'PesquisarOrgao.aspx-EXC': "./ashx/Exclusao/OrgaoExcluir.ashx",
        'DetalhesDeOrgao.aspx-VIS': "./ashx/Visualizacao/OrgaoDetalhes.ashx",
        'DetalhesDeOrgao.aspx-EXC': "./ashx/Exclusao/OrgaoExcluir.ashx",
        'EditarOrgao.aspx-VIS': "./ashx/Visualizacao/OrgaoDetalhes.ashx",
        'CadastrarOrgao.aspx-SAL': "./ashx/Cadastro/OrgaoIncluir.ashx",
        'EditarOrgao.aspx-SAL': "./ashx/Cadastro/OrgaoEditar.ashx",
        //Autoria
        'CadastrarAutoria.aspx-SAL': "./ashx/Cadastro/AutoriaIncluir.ashx",
        'EditarAutoria.aspx-SAL': "./ashx/Cadastro/AutoriaEditar.ashx",
        'EditarAutoria.aspx-VIS': "./ashx/Visualizacao/AutoriaDetalhes.ashx",
        'PesquisarAutoria.aspx-PES': "./ashx/Datatable/AutoriaDatatable.ashx",
        'PesquisarAutoria.aspx-EXC': "./ashx/Exclusao/AutoriaExcluir.ashx",
        'DetalhesDeAutoria.aspx-VIS': "./ashx/Visualizacao/AutoriaDetalhes.ashx",
        'DetalhesDeAutoria.aspx-EXC': "./ashx/Exclusao/AutoriaExcluir.ashx",
        //Diário
        'CadastrarDiario.aspx-SAL': "./ashx/Cadastro/DiarioIncluir.ashx",
        'EditarDiario.aspx-SAL': "./ashx/Cadastro/DiarioEditar.ashx",
        'EditarDiario.aspx-VIS': "./ashx/Visualizacao/DiarioDetalhes.ashx",
        'PesquisarDiario.aspx-PES': "./ashx/Datatable/DiarioDatatable.ashx",
        'ResultadoDePesquisaDiario.aspx-EXC': "./ashx/Exclusao/DiarioExcluir.ashx",
        'DetalhesDeDiario.aspx-VIS': "./ashx/Visualizacao/DiarioDetalhes.ashx",
        'DetalhesDeDiario.aspx-EXC': "./ashx/Exclusao/DiarioExcluir.ashx",
        'CadastrarDiario.aspx-ANX': "./ashx/Arquivo/DiarioIncluirArquivo.ashx",
        'EditarDiario.aspx-ANX': "./ashx/Arquivo/DiarioIncluirArquivo.ashx",
        //TipoDeFonte
        'CadastrarTipoDeFonte.aspx-SAL': "./ashx/Cadastro/TipoDeFonteIncluir.ashx",
        'EditarTipoDeFonte.aspx-SAL': "./ashx/Cadastro/TipoDeFonteEditar.ashx",
        'EditarTipoDeFonte.aspx-VIS': "./ashx/Visualizacao/TipoDeFonteDetalhes.ashx",
        'PesquisarTipoDeFonte.aspx-PES': "./ashx/Datatable/TipoDeFonteDatatable.ashx",
        'PesquisarTipoDeFonte.aspx-EXC': "./ashx/Exclusao/TipoDeFonteExcluir.ashx",
        'DetalhesDeTipoDeFonte.aspx-VIS': "./ashx/Visualizacao/TipoDeFonteDetalhes.ashx",
        'DetalhesDeTipoDeFonte.aspx-EXC': "./ashx/Exclusao/TipoDeFonteExcluir.ashx",
        //TipoDeEdicao
        'CadastrarTipoDeEdicao.aspx-SAL': "./ashx/Cadastro/TipoDeEdicaoIncluir.ashx",
        'EditarTipoDeEdicao.aspx-SAL': "./ashx/Cadastro/TipoDeEdicaoEditar.ashx",
        'EditarTipoDeEdicao.aspx-VIS': "./ashx/Visualizacao/TipoDeEdicaoDetalhes.ashx",
        'PesquisarTipoDeEdicao.aspx-PES': "./ashx/Datatable/TipoDeEdicaoDatatable.ashx",
        'PesquisarTipoDeEdicao.aspx-EXC': "./ashx/Exclusao/TipoDeEdicaoExcluir.ashx",
        'DetalhesDeTipoDeEdicao.aspx-VIS': "./ashx/Visualizacao/TipoDeEdicaoDetalhes.ashx",
        'DetalhesDeTipoDeEdicao.aspx-EXC': "./ashx/Exclusao/TipoDeEdicaoExcluir.ashx",
        //TipoDeNorma
        'CadastrarTipoDeNorma.aspx-SAL': "./ashx/Cadastro/TipoDeNormaIncluir.ashx",
        'EditarTipoDeNorma.aspx-SAL': "./ashx/Cadastro/TipoDeNormaEditar.ashx",
        'EditarTipoDeNorma.aspx-VIS': "./ashx/Visualizacao/TipoDeNormaDetalhes.ashx",
        'PesquisarTipoDeNorma.aspx-PES': "./ashx/Datatable/TipoDeNormaDatatable.ashx",
        'PesquisarTipoDeNorma.aspx-EXC': "./ashx/Exclusao/TipoDeNormaExcluir.ashx",
        'DetalhesDeTipoDeNorma.aspx-VIS': "./ashx/Visualizacao/TipoDeNormaDetalhes.ashx",
        'DetalhesDeTipoDeNorma.aspx-EXC': "./ashx/Exclusao/TipoDeNormaExcluir.ashx",
        //TipoDePublicacao
        'CadastrarTipoDePublicacao.aspx-SAL': "./ashx/Cadastro/TipoDePublicacaoIncluir.ashx",
        'EditarTipoDePublicacao.aspx-SAL': "./ashx/Cadastro/TipoDePublicacaoEditar.ashx",
        'EditarTipoDePublicacao.aspx-VIS': "./ashx/Visualizacao/TipoDePublicacaoDetalhes.ashx",
        'PesquisarTipoDePublicacao.aspx-PES': "./ashx/Datatable/TipoDePublicacaoDatatable.ashx",
        'PesquisarTipoDePublicacao.aspx-EXC': "./ashx/Exclusao/TipoDePublicacaoExcluir.ashx",
        'DetalhesDeTipoDePublicacao.aspx-VIS': "./ashx/Visualizacao/TipoDePublicacaoDetalhes.ashx",
        'DetalhesDeTipoDePublicacao.aspx-EXC': "./ashx/Exclusao/TipoDePublicacaoExcluir.ashx",
        //Interessado
        'CadastrarInteressado.aspx-SAL': "./ashx/Cadastro/InteressadoIncluir.ashx",
        'EditarInteressado.aspx-SAL': "./ashx/Cadastro/InteressadoEditar.ashx",
        'EditarInteressado.aspx-VIS': "./ashx/Visualizacao/InteressadoDetalhes.ashx",
        'PesquisarInteressado.aspx-PES': "./ashx/Datatable/InteressadoDatatable.ashx",
        'PesquisarInteressado.aspx-EXC': "./ashx/Exclusao/InteressadoExcluir.ashx",
        'DetalhesDeInteressado.aspx-VIS': "./ashx/Visualizacao/InteressadoDetalhes.ashx",
        'DetalhesDeInteressado.aspx-EXC': "./ashx/Exclusao/InteressadoExcluir.ashx",
        //Situação
        'CadastrarSituacao.aspx-SAL': "./ashx/Cadastro/SituacaoIncluir.ashx",
        'EditarSituacao.aspx-SAL': "./ashx/Cadastro/SituacaoEditar.ashx",
        'EditarSituacao.aspx-VIS': "./ashx/Visualizacao/SituacaoDetalhes.ashx",
        'PesquisarSituacao.aspx-PES': "./ashx/Datatable/SituacaoDatatable.ashx",
        'PesquisarSituacao.aspx-EXC': "./ashx/Exclusao/SituacaoExcluir.ashx",
        'DetalhesDeSituacao.aspx-VIS': "./ashx/Visualizacao/SituacaoDetalhes.ashx",
        'DetalhesDeSituacao.aspx-EXC': "./ashx/Exclusao/SituacaoExcluir.ashx",
        //TipoDeRelacao
        'CadastrarTipoDeRelacao.aspx-SAL': "./ashx/Cadastro/TipoDeRelacaoIncluir.ashx",
        'EditarTipoDeRelacao.aspx-SAL': "./ashx/Cadastro/TipoDeRelacaoEditar.ashx",
        'EditarTipoDeRelacao.aspx-VIS': "./ashx/Visualizacao/TipoDeRelacaoDetalhes.ashx",
        'PesquisarTipoDeRelacao.aspx-PES': "./ashx/Datatable/TipoDeRelacaoDatatable.ashx",
        'PesquisarTipoDeRelacao.aspx-EXC': "./ashx/Exclusao/TipoDeRelacaoExcluir.ashx",
        'DetalhesDeTipoDeRelacao.aspx-VIS': "./ashx/Visualizacao/TipoDeRelacaoDetalhes.ashx",
        'DetalhesDeTipoDeRelacao.aspx-EXC': "./ashx/Exclusao/TipoDeRelacaoExcluir.ashx",
        //TipoDeDiario
        'CadastrarTipoDeDiario.aspx-SAL': "./ashx/Cadastro/TipoDeDiarioIncluir.ashx",
        'EditarTipoDeDiario.aspx-SAL': "./ashx/Cadastro/TipoDeDiarioEditar.ashx",
        'EditarTipoDeDiario.aspx-VIS': "./ashx/Visualizacao/TipoDeDiarioDetalhes.ashx",
        'PesquisarTipoDeDiario.aspx-PES': "./ashx/Datatable/TipoDeDiarioDatatable.ashx",
        'PesquisarTipoDeDiario.aspx-EXC': "./ashx/Exclusao/TipoDeDiarioExcluir.ashx",
        'DetalhesDeTipoDeDiario.aspx-VIS': "./ashx/Visualizacao/TipoDeDiarioDetalhes.ashx",
        'DetalhesDeTipoDeDiario.aspx-EXC': "./ashx/Exclusao/TipoDeDiarioExcluir.ashx",
        //Requerente
        'CadastrarRequerente.aspx-SAL': "./ashx/Cadastro/RequerenteIncluir.ashx",
        'EditarRequerente.aspx-SAL': "./ashx/Cadastro/RequerenteEditar.ashx",
        'EditarRequerente.aspx-VIS': "./ashx/Visualizacao/RequerenteDetalhes.ashx",
        'PesquisarRequerente.aspx-PES': "./ashx/Datatable/RequerenteDatatable.ashx",
        'PesquisarRequerente.aspx-EXC': "./ashx/Exclusao/RequerenteExcluir.ashx",
        'DetalhesDeRequerente.aspx-VIS': "./ashx/Visualizacao/RequerenteDetalhes.ashx",
        'DetalhesDeRequerente.aspx-EXC': "./ashx/Exclusao/RequerenteExcluir.ashx",
        //Requerido
        'CadastrarRequerido.aspx-SAL': "./ashx/Cadastro/RequeridoIncluir.ashx",
        'EditarRequerido.aspx-SAL': "./ashx/Cadastro/RequeridoEditar.ashx",
        'EditarRequerido.aspx-VIS': "./ashx/Visualizacao/RequeridoDetalhes.ashx",
        'PesquisarRequerido.aspx-PES': "./ashx/Datatable/RequeridoDatatable.ashx",
        'PesquisarRequerido.aspx-EXC': "./ashx/Exclusao/RequeridoExcluir.ashx",
        'DetalhesDeRequerido.aspx-VIS': "./ashx/Visualizacao/RequeridoDetalhes.ashx",
        'DetalhesDeRequerido.aspx-EXC': "./ashx/Exclusao/RequeridoExcluir.ashx",
        //Relator
        'CadastrarRelator.aspx-SAL': "./ashx/Cadastro/RelatorIncluir.ashx",
        'EditarRelator.aspx-SAL': "./ashx/Cadastro/RelatorEditar.ashx",
        'EditarRelator.aspx-VIS': "./ashx/Visualizacao/RelatorDetalhes.ashx",
        'PesquisarRelator.aspx-PES': "./ashx/Datatable/RelatorDatatable.ashx",
        'PesquisarRelator.aspx-EXC': "./ashx/Exclusao/RelatorExcluir.ashx",
        'DetalhesDeRelator.aspx-VIS': "./ashx/Visualizacao/RelatorDetalhes.ashx",
        'DetalhesDeRelator.aspx-EXC': "./ashx/Exclusao/RelatorExcluir.ashx",
        //Procurador
        'CadastrarProcurador.aspx-SAL': "./ashx/Cadastro/ProcuradorIncluir.ashx",
        'EditarProcurador.aspx-SAL': "./ashx/Cadastro/ProcuradorEditar.ashx",
        'EditarProcurador.aspx-VIS': "./ashx/Visualizacao/ProcuradorDetalhes.ashx",
        'PesquisarProcurador.aspx-PES': "./ashx/Datatable/ProcuradorDatatable.ashx",
        'PesquisarProcurador.aspx-EXC': "./ashx/Exclusao/ProcuradorExcluir.ashx",
        'DetalhesDeProcurador.aspx-VIS': "./ashx/Visualizacao/ProcuradorDetalhes.ashx",
        'DetalhesDeProcurador.aspx-EXC': "./ashx/Exclusao/ProcuradorExcluir.ashx",
        //Usuario
        'CadastrarUsuario.aspx-SAL': "./ashx/Cadastro/UsuarioIncluir.ashx",
        'EditarUsuario.aspx-SAL': "./ashx/Cadastro/UsuarioEditar.ashx",
        'EditarUsuario.aspx-VIS': "./ashx/Visualizacao/UsuarioDetalhes.ashx",
        'PesquisarUsuario.aspx-PES': "./ashx/Datatable/UsuarioDatatable.ashx",
        'PesquisarUsuario.aspx-EXC': "./ashx/Exclusao/UsuarioExcluir.ashx",
        'DetalhesDeUsuario.aspx-VIS': "./ashx/Visualizacao/UsuarioDetalhes.ashx",
        'DetalhesDeUsuario.aspx-EXC': "./ashx/Exclusao/UsuarioExcluir.ashx",
        //Vocabulario
        'CadastrarVocabulario.aspx-SAL': "./ashx/Cadastro/VocabularioIncluir.ashx",
        'GerenciarVocabulario.aspx-SAL': "./ashx/Cadastro/VocabularioIncluir.ashx",
        'GerenciarVocabulario.aspx-TRC': "./ashx/Cadastro/VocabularioTrocar.ashx",
        'EditarVocabulario.aspx-SAL': "./ashx/Cadastro/VocabularioEditar.ashx",
        'EditarVocabulario.aspx-VIS': "./ashx/Visualizacao/VocabularioDetalhes.ashx",
        'PesquisarVocabulario.aspx-PES': "./ashx/Datatable/VocabularioDatatable.ashx",
        'PesquisarVocabulario.aspx-EXC': "./ashx/Exclusao/VocabularioExcluir.ashx",
        'DetalhesDeVocabulario.aspx-VIS': "./ashx/Visualizacao/VocabularioDetalhes.ashx",
        'ListasVocabulario.aspx-VIS': "./ashx/Consulta/VocabularioListasConsulta.ashx",
        'DetalhesDeVocabulario.aspx-EXC': "./ashx/Exclusao/VocabularioExcluir.ashx",
        //Configuracao
        'Configuracao.aspx-SAL': "./ashx/Path/ConfiguracaoPath.ashx",
        'Configuracao.aspx-VIS': "./ashx/Visualizacao/ConfiguracaoDetalhes.ashx",
        //Relatório
        'Relatorio.aspx-PES': "./ashx/Relatorio/NormaRelatorio.ashx",
        //Notifiqueme
        'Notifiqueme.aspx-SAL': "./ashx/Push/NotifiquemeEditar.ashx",
        'DetalhesDeNotifiqueme.aspx-VIS': "./ashx/Visualizacao/NotifiquemeDetalhes.ashx",
        'RecriarSenhaNotifiqueme.aspx-SAL': "./ashx/Push/NotifiquemeRecriarSenha.ashx",
        //Vide
        'CadastrarVide.aspx-SAL': "./ashx/Cadastro/VideIncluir.ashx",
        'EditarVide.aspx-SAL': "./ashx/Cadastro/VideEditar.ashx"
    };
    var page_ajax = mapPaginaAPP[file + '-' + action];
    return page_ajax;
}

function MostrarErro(cd) {
    var mapErro = {
        '0': "Sem permissão para acessar a página.",
        '1': "A sessão expirou."
    }
    return mapErro[cd];
}

var _grupos = {
    nor_inc: "NOR.INC",
    nor_edt: "NOR.EDT",
    nor_exc: "NOR.EXC",
    nor_vis: "NOR.VIS",
    nor_pes: "NOR.PES",
    aut_inc: "AUT.INC",
    aut_edt: "AUT.EDT",
    aut_exc: "AUT.EXC",
    aut_vis: "AUT.VIS",
    aut_pes: "AUT.PES",
    org_inc: "ORG.INC",
    org_edt: "ORG.EDT",
    org_exc: "ORG.EXC",
    org_pes: "ORG.PES",
    org_vis: "ORG.VIS",
    dio_inc: "DIO.INC",
    dio_edt: "DIO.EDT",
    dio_exc: "DIO.EXC",
    dio_pes: "DIO.PES",
    dio_vis: "DIO.VIS",
    tdf_inc: "TDF.INC",
    tdf_edt: "TDF.EDT",
    tdf_exc: "TDF.EXC",
    tdf_pes: "TDF.PES",
    tdf_vis: "TDF.VIS",
    tdn_inc: "TDN.INC",
    tdn_edt: "TDN.EDT",
    tdn_exc: "TDN.EXC",
    tdn_pes: "TDN.PES",
    tdn_vis: "TDN.VIS",
    tdp_inc: "TDP.INC",
    tdp_edt: "TDP.EDT",
    tdp_exc: "TDP.EXC",
    tdp_pes: "TDP.PES",
    tdp_vis: "TDP.VIS",
    tdd_inc: "TDD.INC",
    tdd_edt: "TDD.EDT",
    tdd_exc: "TDD.EXC",
    tdd_pes: "TDD.PES",
    tdd_vis: "TDD.VIS",
    int_inc: "INT.INC",
    int_edt: "INT.EDT",
    int_exc: "INT.EXC",
    int_pes: "INT.PES",
    int_vis: "INT.VIS",
    sit_inc: "SIT.INC",
    sit_edt: "SIT.EDT",
    sit_exc: "SIT.EXC",
    sit_pes: "SIT.PES",
    sit_vis: "SIT.VIS",
    tdr_inc: "TDR.INC",
    tdr_edt: "TDR.EDT",
    tdr_exc: "TDR.EXC",
    tdr_pes: "TDR.PES",
    tdr_vis: "TDR.VIS",
    rqe_inc: "RQE.INC",
    rqe_edt: "RQE.EDT",
    rqe_exc: "RQE.EXC",
    rqe_pes: "RQE.PES",
    rqe_vis: "RQE.VIS",
    rqi_inc: "RQI.INC",
    rqi_edt: "RQI.EDT",
    rqi_exc: "RQI.EXC",
    rqi_pes: "RQI.PES",
    rqi_vis: "RQI.VIS",
    rel_inc: "REL.INC",
    rel_edt: "REL.EDT",
    rel_exc: "REL.EXC",
    rel_pes: "REL.PES",
    rel_vis: "REL.VIS",
    pro_inc: "PRO.INC",
    pro_edt: "PRO.EDT",
    pro_exc: "PRO.EXC",
    pro_pes: "PRO.PES",
    pro_vis: "PRO.VIS",
    usr_inc: "USR.INC",
    usr_edt: "USR.EDT",
    usr_exc: "USR.EXC",
    usr_pes: "USR.PES",
    usr_vis: "USR.VIS",
    usr_sai: "USR.SAI",
    rio_pes: "RIO.PES",
    voc_inc: "VOC.INC",
    voc_edt: "VOC.EDT",
    voc_exc: "VOC.EXC",
    voc_pes: "VOC.PES",
    voc_vis: "VOC.VIS",
    voc_ger: "VOC.GER",
    cfg_edt: "CFG.EDT",
    cfg_vis: "CFG.VIS"

};

var _paginas = [{
    pagina_inicial: "./Pesquisas.aspx",
    ds_pagina_inicial: "Pesquisas"
}, {
    pagina_inicial: "./CadastrarNorma.aspx",
    ds_pagina_inicial: "Cadastrar Norma"
}, {
    pagina_inicial: "./CadastrarDiario.aspx",
    ds_pagina_inicial: "Cadastrar Diário"
}, {
    pagina_inicial: "./CadastrarTipoDeFonte.aspx",
    ds_pagina_inicial: "Cadastrar Tipo de Fonte"
}, {
    pagina_inicial: "./CadastrarTipoDeEdicao.aspx",
    ds_pagina_inicial: "Cadastrar Tipo de Edição"
}, {
    pagina_inicial: "./CadastrarTipoDeNorma.aspx",
    ds_pagina_inicial: "Cadastrar Tipo de Norma"
}, {
    pagina_inicial: "./CadastrarTipoDePublicacao.aspx",
    ds_pagina_inicial: "Cadastrar Tipo de Publicação"
}, {
    pagina_inicial: "./CadastrarTipoDeInteressado.aspx",
    ds_pagina_inicial: "Cadastrar Interessado"
}, {
    pagina_inicial: "./CadastrarSituacao.aspx",
    ds_pagina_inicial: "Cadastrar Situação"
}, {
    pagina_inicial: "./CadastrarTipoDeRelacao.aspx",
    ds_pagina_inicial: "Cadastrar Relação"
}, {
    pagina_inicial: "./CadastrarAutoria.aspx",
    ds_pagina_inicial: "Cadastrar Autoria"
}, {
    pagina_inicial: "./CadastrarRequerente.aspx",
    ds_pagina_inicial: "Cadastrar Requerente"
}, {
    pagina_inicial: "./CadastrarRequerido.aspx",
    ds_pagina_inicial: "Cadastrar Requerido"
}, {
    pagina_inicial: "./CadastrarRelator.aspx",
    ds_pagina_inicial: "Cadastrar Relator"
}, {
    pagina_inicial: "./CadastrarProcurador.aspx",
    ds_pagina_inicial: "Cadastrar Procurador"
}, {
    pagina_inicial: "./CadastrarOrgao.aspx",
    ds_pagina_inicial: "Cadastrar Órgão"
}, {
    pagina_inicial: "./CadastrarVocabulario.aspx",
    ds_pagina_inicial: "Cadastrar Vocabulário"
}, {
    pagina_inicial: "./CadastrarUsuario.aspx",
    ds_pagina_inicial: "Cadastrar Usuário"
}, {
    pagina_inicial: "./PesquisarNorma.aspx",
    ds_pagina_inicial: "Pesquisar Norma"
}, {
    pagina_inicial: "./PesquisarDiario.aspx",
    ds_pagina_inicial: "Pesquisar Diário"
}, {
    pagina_inicial: "./PesquisarTipoDeFonte.aspx",
    ds_pagina_inicial: "Pesquisar Tipo de Fonte"
}, {
    pagina_inicial: "./PesquisarTipoDeEdicao.aspx",
    ds_pagina_inicial: "Pesquisar Tipo de Edição"
}, {
    pagina_inicial: "./PesquisarTipoDeNorma.aspx",
    ds_pagina_inicial: "Pesquisar Tipo de Norma"
}, {
    pagina_inicial: "./PesquisarTipoDePublicacao.aspx",
    ds_pagina_inicial: "Pesquisar Tipo de Publicação"
}, {
    pagina_inicial: "./PesquisarTipoDeInteressado.aspx",
    ds_pagina_inicial: "Pesquisar Interessado"
}, {
    pagina_inicial: "./PesquisarSituacao.aspx",
    ds_pagina_inicial: "Pesquisar Situação"
}, {
    pagina_inicial: "./PesquisarTipoDeRelacao.aspx",
    ds_pagina_inicial: "Pesquisar Relação"
}, {
    pagina_inicial: "./PesquisarAutoria.aspx",
    ds_pagina_inicial: "Pesquisar Autoria"
}, {
    pagina_inicial: "./PesquisarRequerente.aspx",
    ds_pagina_inicial: "Pesquisar Requerente"
}, {
    pagina_inicial: "./PesquisarRequerido.aspx",
    ds_pagina_inicial: "Pesquisar Requerido"
}, {
    pagina_inicial: "./PesquisarRelator.aspx",
    ds_pagina_inicial: "Pesquisar Relator"
}, {
    pagina_inicial: "./PesquisarProcurador.aspx",
    ds_pagina_inicial: "Pesquisar Procurador"
}, {
    pagina_inicial: "./PesquisarOrgao.aspx",
    ds_pagina_inicial: "Pesquisar Órgão"
}, {
    pagina_inicial: "./PesquisarVocabulario.aspx",
    ds_pagina_inicial: "Pesquisar Vocabulário"
}, {
    pagina_inicial: "./PesquisarUsuario.aspx",
    ds_pagina_inicial: "Pesquisar Usuário"
}, {
    pagina_inicial: "./GerenciarVocabulario.aspx",
    ds_pagina_inicial: "Gerenciar Vocabulário"
}, {
    pagina_inicial: "./ListasVocabulario.aspx",
    ds_pagina_inicial: "Listas"
}];

var _temas = [{
    ch_tema: "verde",
    nm_tema: "Verde"
}, {
    ch_tema: "cinza",
    nm_tema: "Cinza"
}];

function ValidarPermissao(grupo) {
    if (IsNotNullOrEmpty(_user, 'grupos')) {
        return _user.grupos.indexOf(grupo) > -1;
    }
    return false;
}

function isSuperAdmin() {
    return _user.ch_perfil == 'super_administrador';
}

function ValidarPermissaoPorTipoDeNorma(tipoDeNorma) {
    var inPode = isSuperAdmin();
    for (var i = 0; i < tipoDeNorma.orgaos_cadastradores.length; i++) {
        if (tipoDeNorma.orgaos_cadastradores[i].id_orgao_cadastrador == _user.orgao_cadastrador.id_orgao_cadastrador) {
            inPode = true;
            break;
        }
    }
    return inPode;
}

var url_json_pesquisa_avancada = "./ashx/JsonPortalCamposPesquisaAvancada.ashx";
var url_valores_pesquisa_avancada = "./ashx/JsonPortalValoresPesquisaAvancada.ashx";

/* jquery.datepicker.js
=================================================*/
$(document).ready(function() {
    inicializarDatePicker();
});

function inicializarDatePicker(options) {
    var defaults = {
        element: $('.date'),
        dateFormat: 'dd/mm/yy',
        monthNames: ['Janeiro', 'Fevereiro', 'Mar&ccedil;o', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
        dayNamesMin: ["D", "S", "T", "Q", "Q", "S", "S"],
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        nextText: 'Pr&oacute;ximo',
        prevText: 'Anterior',
        changeMonth: true,
        changeYear: true,
        yearRange: setYearRange(),
        autoSize: true,
        onSelect: null
    }
    var settings = $.extend({}, defaults, options);
    settings.element.datepicker(settings);
    settings.element.attr('onKeypress', 'mascara(this,mdata)');
    settings.element.attr('onblur', 'valida_data(this)');
}

function valida_data(o) {
    if (IsNotNullOrEmpty(o.value) && !isDate(o.value)) {
        focusElement(o);
        o.value = "";
    }
}

function setYearRange() {
    return "1900:" + new Date().getFullYear();
}

/* mask.js
=================================================*/
function mascara(o, f) {
    v_obj = o
    v_fun = f
    setTimeout("execmascara()", 1)
}

function execmascara() {
    v_obj.value = v_fun(v_obj.value)
}

function mcep(v) {
    v = v.replace(/\D/g, "") //Remove tudo o que não é dígito
    v = v.replace(/^(\d{5})(\d)/, "$1-$2") //Esse é tão fácil que não merece explicações
    return v
}

function mtel(v) {
    v = v.replace(/\D/g, "") //Remove tudo o que não é dígito
    v = v.replace(/^(\d\d)(\d)/g, "($1) $2") //Coloca parênteses em volta dos dois primeiros dígitos
    v = v.replace(/(\d{4})(\d)/, "$1-$2") //Coloca hífen entre o quarto e o quinto dígitos
    return v
}

function cnpj(v) {
    v = v.replace(/\D/g, "") //Remove tudo o que não é dígito
    v = v.replace(/^(\d{2})(\d)/, "$1.$2") //Coloca ponto entre o segundo e o terceiro dígitos
    v = v.replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3") //Coloca ponto entre o quinto e o sexto dígitos
    v = v.replace(/\.(\d{3})(\d)/, ".$1/$2") //Coloca uma barra entre o oitavo e o nono dígitos
    v = v.replace(/(\d{4})(\d)/, "$1-$2") //Coloca um hífen depois do bloco de quatro dígitos
    return v
}

function mcpf(v) {
    v = v.replace(/\D/g, "") //Remove tudo o que não é dígito
    v = v.replace(/(\d{3})(\d)/, "$1.$2") //Coloca um ponto entre o terceiro e o quarto dígitos
    v = v.replace(/(\d{3})(\d)/, "$1.$2") //Coloca um ponto entre o terceiro e o quarto dígitos
    //de novo (para o segundo bloco de números)
    v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2") //Coloca um hífen entre o terceiro e o quarto dígitos
    return v
}

function mdata(v) {
    v = v.replace(/\D/g, ""); //Remove tudo o que não é dígito
    v = v.replace(/(\d{2})(\d)/, "$1/$2");
    v = v.replace(/(\d{2})(\d)/, "$1/$2");

    v = v.replace(/(\d{2})(\d{2})$/, "$1$2");
    return v;
}

function mtempo(v) {
    v = v.replace(/\D/g, ""); //Remove tudo o que não é dígito
    v = v.replace(/(\d{1})(\d{2})(\d{2})/, "$1:$2.$3");
    return v;
}

function mhora(v) {
    v = v.replace(/\D/g, ""); //Remove tudo o que não é dígito
    v = v.replace(/(\d{2})(\d)/, "$1h$2");
    return v;
}

function mrg(v) {
    v = v.replace(/\D/g, ""); //Remove tudo o que não é dígito
    v = v.replace(/(\d)(\d{7})$/, "$1.$2"); //Coloca o . antes dos últimos 3 dígitos, e antes do verificador
    v = v.replace(/(\d)(\d{4})$/, "$1.$2"); //Coloca o . antes dos últimos 3 dígitos, e antes do verificador
    v = v.replace(/(\d)(\d)$/, "$1-$2"); //Coloca o - antes do último dígito
    return v;
}

function mnum(v) {
    v = v.replace(/\D/g, ""); //Remove tudo o que não é dígito
    return v;
}

function mvalor(v) {
    v = v.replace(/\D/g, ""); //Remove tudo o que não é dígito
    v = v.replace(/(\d)(\d{8})$/, "$1.$2"); //coloca o ponto dos milhões
    v = v.replace(/(\d)(\d{5})$/, "$1.$2"); //coloca o ponto dos milhares

    v = v.replace(/(\d)(\d{2})$/, "$1,$2"); //coloca a virgula antes dos 2 últimos dígitos
    return v;
}

function mvirgula(v) {
    v = v.replace(/\,/g, "") //Remove as vírgulas
    return v
}


/* navegador.js
================================================*/
// necessário carregar: 'jquery-1.11.0.min.js', 'js.ashx', 'Feedback.js'
var arry_navegadores_versao_suport = {
    'Opera': 15,
    'Chrome': 20,
    'Firefox': 17,
    'IE': 7,
    'Safari': 5
};

function f_navegadores_versao_suport(s_navegador) {
    var navegador;
    navegador = arry_navegadores_versao_suport[s_navegador];
    return (navegador) ? navegador : 0;
}

var arry_navegadores_testados = {
    'Opera': true,
    'Chrome': true,
    'Firefox': true,
    'IE': true,
    'Safari': true
};

function f_navegadores_testados(s_navegador) {
    var navegador;
    navegador = arry_navegadores_testados[s_navegador];
    return (navegador) ? true : false;
}

var arry_navegadores_block = {
    'Navigator': true,
    'Flock': true
};

function f_navegadores_block(s_navegador) {
    var navegador;
    navegador = arry_navegadores_block[s_navegador];
    return (navegador) ? true : false;
}

function navegador_versao() {
    var navegador = platform.name;
    var versao = platform.version;
    return "Navegador: " + ((navegador != null) ? navegador : 'N&atilde;o Reconhecido') + ' ' + ((versao != null) ? versao : '');
}

function version_browser() {
    var navegador = platform.name;
    var versao = platform.version;
    return ((navegador != null) ? navegador : 'N&atilde;o Reconhecido') + ' ' + ((versao != null) ? versao : '');
}

function blockLogin() {
    $('#login').val('');
    $('#senha').val('');
    $('#frmLogin').find(':input:not(:disabled)').prop("readonly", true);
    $('#frmLogin').find(':input:not(:disabled)').prop('disabled', true);
    $('#divLoginPrincipal').addClass('overlay');
    $("a").click(function() {
        return false;
    });
}

function verificar_navegador() {

    var navegador = platform.name;
    var versao = platform.version;
    var navegador_testado = f_navegadores_testados(navegador);
    var navegadores_versao_suport = f_navegadores_versao_suport(navegador);
    var navegadores_versao_block = f_navegadores_block(navegador);

    var msg = 'Seu navegador (' + ((navegador != null) ? navegador : 'N&atilde;o Reconhecido') + ' ' + ((versao != null) ? versao : '') + ')';
    var msg_nao_testado = ' n&atilde;o &eacute; um navegador testado para essa aplica&ccedil;&atilde;o. Podendo assim apresentar problemas para exibir partes desse aplicativo';
    var msg_nao_suportado = ' n&atilde;o &eacute; suportado para essa aplica&ccedil;&atilde;o.';
    var msg_nao_permitido = ' n&atilde;o &eacute; permitido para essa aplica&ccedil;&atilde;o. Para esse navegador (' + navegador + ') utilize vers&atilde;o ' + navegadores_versao_suport + ' ou superior.';
    var msg_block = ' n&atilde;o &eacute; permitido para essa aplica&ccedil;&atilde;o. T&ecirc;m problemas de seguran&ccedil;a e n&atilde;o acompanham os novos padr&otilde;es de desenvolvimento para a Internet.';

    var msg_final = '';

    if (navegadores_versao_block || (navegador == null)) {
        msg_final = msg + msg_block;
        blockLogin();
    } else {
        if (navegador_testado) {
            if (parseInt(versao) < navegadores_versao_suport) {
                msg_final = msg + msg_nao_permitido;
                blockLogin();
            }
        } else {
            msg_final = msg + msg_nao_testado;
        }
    }

    if (msg_final != "") {
        $('#div_notificacao_navegador').messagelight({
            sTitle: "Atenção",
            sContent: msg_final,
            sType: "alert",
            sWidth: "",
            iTime: null
        });
    }
    return;
}

/*validation.js
========================================*/
// necessÃƒÂ¡rio carregar:'jquery-1.11.0.min.js', 'jquery.modalLight.js'
// 'form', pode ser um objeto do tipo 'form', ou o nome do formulario
function Validar(form) {
    if (typeof form === 'undefined') {
        throw "Fomul&aacute;rio inv&aacute;lido !!!";
    }
    if (typeof form == 'string') {
        if (getElement(form) != false) {
            var form = getElement(form);
        } else {
            throw "Fomul&aacute;rio(" + form + ") inv&aacute;lido !!!";
        }
    }
    var e = form.elements;
    for (var i = 0; i < e.length; i++) {
        if (e[i].getAttribute("type") == "file") {
            var arquivo = e[i].getAttribute("value");
            if (IsNotNullOrEmpty(arquivo)) {
                var tipo = arquivo.substring(arquivo.indexOf('.') + 1).toLowerCase();
                var bFilePermitted = false;
                $.each(_extensoes.split(','), function(key, value) {
                    if (tipo == value) {
                        bFilePermitted = true;
                        return;
                    }
                });
                if (!bFilePermitted) {
                    throw "Arquivo n&atilde;o permitido!!!";
                }
            }

        }
        if (e[i].getAttribute("obrigatorio") == "sim") {
            //NÃ£o trata input type=file.
            if (e[i].value == "") {
                focusElement(e[i]);
                throw "O campo " + e[i].getAttribute("label") + " &eacute; obrigat&oacute;rio!!!";
            } else if (e[i].getAttribute("type") == "radio" || e[i].getAttribute("type") == "checkbox") {
                var name = e[i].getAttribute("name");
                var group = document.getElementsByName(name);
                for (var j = 0; j < group.length; j++) {
                    if (group[j].checked == true) break;
                    if (j == (group.length - 1)) {
                        throw "O campo " + e[i].getAttribute("label") + " &eacute; obrigat&oacute;rio!!!";
                    }
                }
            }
        }
        if (e[i].getAttribute("specials") == "inputfilelight") { //valida o input file criado pelo plugin inputfilelight.js
            var name = e[i].getAttribute("name");
            var group = document.getElementsByName(name);
            var hiddenGroup = document.getElementsByName('hidden_' + name); //quando passa jFiles para criar os inputfiles ele não input type file e sim hidden porque dá erro nos ies 7 e 8
            var ok = false;
            if (hiddenGroup.length > 0) {
                for (var j = 0; j < hiddenGroup.length; j++) {
                    if (hiddenGroup[j].getAttribute("inputfilevalue") != null && hiddenGroup[j].getAttribute("inputfilevalue") != "") {
                        ok = true;
                        break;
                    }
                }
            } else if (group.length > 0) {
                for (var j = 0; j < group.length; j++) {
                    if (group[j].getAttribute("inputfilevalue") != null && group[j].getAttribute("inputfilevalue") != "") {
                        ok = true;
                        break;
                    }
                }
            }
            if (ok) {
                continue;
            } else {
                focusElement(e[i]);
                throw "O campo " + e[i].getAttribute("label") + " &eacute; obrigat&oacute;rio!!!";
            }
        }
        if (e[i].value != "") {
            if (e[i].getAttribute("specials") == "tamanho_minimo") {
                if (e[i].value.length < e[i].getAttribute("tamanho")) {
                    focusElement(e[i]);
                    throw "O campo " + e[i].getAttribute("label") + " est&aacute; com um tamanho m&iacute;nimo (" + e[i].getAttribute("tamanho") + ") inv&aacute;lido!!!";
                }
            }
            if (e[i].getAttribute("specials") == "tamanho_maximo") {
                if (e[i].value.length > e[i].getAttribute("tamanho")) {
                    focusElement(e[i]);
                    throw "O campo " + e[i].getAttribute("label") + " est&aacute; com um tamanho m&aacute;ximo (" + e[i].getAttribute("tamanho") + ") inv&aacute;lido!!!";
                }
            }
            if (e[i].getAttribute("specials") == "data") {
                if (!isDate(e[i].value)) {
                    focusElement(e[i]);
                    throw "O campo " + e[i].getAttribute("label") + " est&aacute; com uma data inv&aacute;lida!!!";
                }
            }
            if (e[i].getAttribute("specials") == "cpf_cnpj") {
                if (!isCPForCNPJ(e[i].value)) {
                    focusElement(e[i]);
                    throw "O campo  " + e[i].getAttribute("label") + " est&aacute; com um CPF ou CNPJ inv&aacute;lido!!!";
                }
            }
            if (e[i].getAttribute("specials") == "cnpj") {
                if (!isCNPJ(e[i].value)) {
                    focusElement(e[i]);
                    throw "O campo  " + e[i].getAttribute("label") + " est&aacute; com um CNPJ inv&aacute;lido!!!";
                }
            }
            if (e[i].getAttribute("specials") == "cpf") {
                if (!isCPF(e[i].value)) {
                    focusElement(e[i]);
                    throw "O campo  " + e[i].getAttribute("label") + " est&aacute; com um CPF inv&aacute;lido!!!";
                }
            }

            if (e[i].getAttribute("specials") == "numerico") {
                if (isNaN(e[i].value)) {
                    focusElement(e[i]);
                    throw "O campo " + e[i].getAttribute("label") + "  deve ser preenchido com um valor num&eacute;rico!!!";
                }
            }

            if (e[i].getAttribute("specials") == "email") {
                if (!isEmail(e[i].value)) {
                    focusElement(e[i]);
                    throw "O campo  " + e[i].getAttribute("label") + " est&aacute; com um valor inv&aacute;lido para um email!!!";
                }
            }

            if (e[i].getAttribute("comparar") != null) {

                if (e[i].getAttribute("comparar") != "") {
                    var arrTipo = {
                        'igual': "==",
                        'maior': ">",
                        'maior ou igual': ">=",
                        'diferente': "!=",
                        'menor': "<",
                        'menor ou igual': "<=",
                        'e': "&",
                        'ou': "|"
                    };
                    var arrTipoSaida = {
                        'igual': "igual",
                        'maior': "maior que",
                        'maior ou igual': "maior ou igual ao",
                        'diferente': "diferente do",
                        'menor': "menor que",
                        'menor ou igual': "menor ou igual ao",
                        'e': "e",
                        'ou': " ou"
                    };
                    var arrComparar = e[i].getAttribute("comparar").split(",");
                    if (getElement(arrComparar[0]) == false) {
                        focusElement(e[i]);
                        throw "Olha, programador o campo '" + arrComparar[0] + "' simplesmente n&atilde;o existe!!!";
                    }

                    var valor = getElement(arrComparar[0]).value;
                    var Tipo = arrTipo[arrComparar[1]];
                    if ((valor == "") || (getElement(arrComparar[0]).getAttribute("obrigatorio") == 'null')) {
                        focusElement(e[i]);
                        throw "Olha, programador o campo '" + getElement(arrComparar[0]).getAttribute("label") + "' deve ser obrigat&oacute;rio!!!";
                    }
                    if (e[i].getAttribute("specials") == "data") {
                        if (!compara_data(e[i].value, Tipo, valor)) {
                            focusElement(e[i]);
                            throw "O campo " + e[i].getAttribute("label") + " deve ser '" + arrTipoSaida[arrComparar[1]] + "'  campo " + getElement(arrComparar[0]).getAttribute("label") + "!!!";
                        }
                    }
                    if (e[i].getAttribute("specials") == "numerico") {
                        if (!eval("(" + e[i].value + " " + Tipo + " " + valor + ")")) {
                            focusElement(e[i]);
                            throw "O campo " + e[i].getAttribute("label") + " deve ser '" + arrTipoSaida[arrComparar[1]] + "' campo " + getElement(arrComparar[0]).getAttribute("label") + "!!!";
                        }
                    }
                    if (!eval("('" + e[i].value + "' " + Tipo + " '" + valor + "')")) {
                        focusElement(e[i]);
                        throw "O campo " + e[i].getAttribute("label") + " deve ser '" + arrTipoSaida[arrComparar[1]] + "' campo " + getElement(arrComparar[0]).getAttribute("label") + "!!!";
                    }
                }
            }
        }
    }
    return true;
}

function ValidarPesquisa(form) {
    if (typeof form == 'string') {
        if (getElement(form) != false) {
            var form = getElement(form);
        } else {
            ShowMessage(divPanel, "Fomul&aacute;rio(" + form + ") inv&aacute;lido !!!", "error");
            throw "Fomul&aacute;rio(" + form + ") inv&aacute;lido !!!";
        }
    }
    var e = form.elements;
    for (var i = 0; i < e.length; i++) {
        if (e[i].value != "" && (e[i].getAttribute("type") == "text" || e[i].getAttribute("type") == "hidden")) {
            return true;
        }
    }
    throw "A pesquisa não pode ser vazia!";
}

function isDate(txtDate) {
    var txtDate = txtDate.replaceAll(" ", "");
    if (IsNotNullOrEmpty(txtDate)) {
        if (txtDate.length == 10) {
            if (verifica_data(txtDate)) {
                return true;
            }
        } else if (txtDate.length == 18) {

            var getHour = txtDate.substring(txtDate.length - 8, txtDate.length);
            var getDate = txtDate.substring(0, 10);

            var arrayHour = getHour.split(':');
            var arrayDate = getDate.split('/');

            if (arrayHour.length == 3 && arrayDate.length == 3) {
                if (verifica_data(getDate) && Verifica_Hora(getHour)) {
                    return true;
                } else {
                    return false;
                }
            }
            return false;
        } else {
            return false;
        }
    }
}

function Verifica_Hora(hora) {
    hrs = (hora.substring(0, 2));
    min = (hora.substring(3, 5));
    sec = (hora.substring(6, 8));

    if ((hrs < 00) || (hrs > 23) || (min < 00) || (min > 59) || (sec < 00) || (sec > 59)) {
        return false;
    }
    return true;
}

function verifica_data(txtDate) {

    if (IsNotNullOrEmpty(txtDate) && txtDate.length == 10) {
        var currVal = txtDate;

        var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
        var dtArray = currVal.match(rxDatePattern);

        if (dtArray == null)
            return false;

        //Checks for dd/mm/yyyy format.
        dtDay = dtArray[1];
        dtMonth = dtArray[3];
        dtYear = dtArray[5];

        if (dtMonth < 1 || dtMonth > 12)
            return false;
        else if (dtDay < 1 || dtDay > 31)
            return false;
        else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
            return false;
        else if (dtMonth == 2) {
            var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
            if (dtDay > 29 || (dtDay == 29 && !isleap))
                return false;
        }
        return true;
    } else {
        return false;
    }
}

function CriarModalDescricaoOrigem(ch_orgao) {
    $('#modal_descricao_origem').modallight({
        sTitle: "Descrição de Origem",
        sWidth: '600',
        sContent: '<div id="consulta_origem"></div>',
        fnCreated: function() {
            var sucesso = function(data) {
                $('#consulta_origem').html(data.ds_origem);
            }
            var inicio = function() {
                $('#consulta_origem').html('<div class="loading"></div>');
            }
            $.ajaxlight({
                sUrl: "./ashx/Consulta/OrigemDaNormaConsulta.ashx?ch_orgao=" + ch_orgao,
                fnSuccess: sucesso,
                fnBeforeSend: inicio,
                bAsync: true
            });
        },
        fnClose: function() {
            LimparModal('modal_descricao_origem');
        }
    })
}

function MontarHighlight(obj, sCampo) {
    var new_obj = {};
    var s_campo_highlight = "";
    sCampo = sCampo.replaceAll('.', '_');
    for (prop in obj) {
        new_obj[prop.replaceAll('.', '_')] = obj[prop];
    }
    if (IsNotNullOrEmpty(new_obj, sCampo)) {
        var campo_highlight = eval('new_obj.' + sCampo);
        if (isArray(campo_highlight)) {
            if (campo_highlight.length > 1) {
                s_campo_highlight = '...' + campo_highlight.join('...<br/>...') + '...';
            } else if (campo_highlight.length == 1) {
                s_campo_highlight = campo_highlight[0];
            }
        } else {
            s_campo_highlight = eval(campo_highlight + '.toString()');
        }
        if (IsNotNullOrEmpty(s_campo_highlight)) {
            var pre = /_pre_tag_highlight_/g;
            var post = /_post_tag_highlight_/g;
            s_campo_highlight = s_campo_highlight.replace(pre, "<span class='highlight'>");
            s_campo_highlight = s_campo_highlight.replace(post, "</span>");
            return s_campo_highlight;
        }
    }
    return '';
}

function fn_submit_highlight() {
    for (var i = 0; i < aHighlight.length; i++) {
        if (doc_clicked == aHighlight[i].doc) {
            if (IsNotNullOrEmpty(id_file)) {
                if (IsNotNullOrEmpty(texto)) {
                    document.form_highlight.action = "./TextoArquivoNorma.aspx?id_file=" + id_file;
                    document.form_highlight.highlight.value = window.escape(JSON.stringify(aHighlight[i].highlight));
                    return true;
                }
            }
            //          document.form_highlight.action = "./DetalhesDeNorma.aspx?id_doc=" + doc_clicked;
            document.form_highlight.action = "./DetalhesDeNorma.aspx?id_norma=" + aHighlight[i].ch_doc;
            document.form_highlight.highlight.value = window.escape(JSON.stringify(aHighlight[i].highlight));
            return true;
        }
    }
    return false;
}

function aplicarHighlight(selector, highlight, parent, className) {
    if (IsNotNullOrEmpty(highlight))
        for (var key in highlight) {
            $(selector + key, parent).highlight(highlight[key], {
                wordsOnly: false,
                className: className
            });
        }
    return;
}

function aplicarHighlightPortal(_highlight, parent, className) {
    if (IsNotNullOrEmpty(_highlight)) {
        for (var key in _highlight) {
            var value = _highlight[key];
            var valor_para_brilhar = '';
            if (isArray(value)) {
                valor_para_brilhar = value.join(' ');
            }
            var valor_splited = valor_para_brilhar.split('_pre_tag_highlight_');
            var valor_splited_post = [];
            for (var i = 0; i < valor_splited.length; i++) {
                if (valor_splited[i].indexOf('_post_tag_highlight_') > -1) {
                    valor_splited_post = valor_splited[i].split('_post_tag_highlight_');
                    if (IsNotNullOrEmpty(valor_splited_post[0])) {
                        if (IsNotNullOrEmpty(parent)) {
                            $('.' + key.replaceAll('_text', ''), parent).highlight(valor_splited_post[0], {
                                wordsOnly: false,
                                className: className
                            });
                        } else {
                            $('.' + key.replaceAll('_text', '')).highlight(valor_splited_post[0], {
                                wordsOnly: false,
                                className: className
                            });
                        }
                    }
                }
            }
        }
    }
}

function getHighlightFromDocClicked(_doc_clicked) {
    var highlightFromDocClicked = null;
    for (var i = 0; i < aHighlight.length; i++) {
        if (_doc_clicked == aHighlight[i].doc) {
            highlightFromDocClicked = aHighlight[i];
            break;
        }
    }
    return highlightFromDocClicked;
}

function getHighlight(str) {
    return ((str) ? str.replace(/_pre_tag_highlight_/g, '<span class="highlight">').replace(/_post_tag_highlight_/g, '</span>') : "");
}

function syntaxHighlightFromEs(obj_from_app, highlight_from_app) {
    if (IsNotNullOrEmpty(highlight_from_app)) {
        for (var prop in highlight_from_app) {
            //console.log(eval('obj_from_app.' + prop));
            if (prop.indexOf('.') > 0) {
                var propSplited = prop.split('.');
                var prop_ob = obj_from_app[propSplited[0]];
                for (var i = 1; i < propSplited.length; i++) {
                    if (typeof prop_ob[propSplited[i]] == 'object') {
                        prop_ob = prop_ob[propSplited[i]];
                    } else {
                        var prop_without_text = propSplited[i].replace("_text", "");
                        if (propSplited[i] != prop_without_text && IsNotNullOrEmpty(obj_from_app, prop_without_text)) {
                            prop_ob[prop_without_text] = getHighlight(highlight_from_app[prop][0]);
                        } else if (Array.isArray(prop_ob[propSplited[i]])) {
                            for (var j = 0; j < highlight_from_app[prop].length; j++) {
                                var valor_sem_highlight = highlight_from_app[prop][j].replace(/_pre_tag_highlight_/g, '').replace(/_post_tag_highlight_/g, '');
                                for (var k = 0; k < prop_ob[propSplited[i]].length; k++) {
                                    if (prop_ob[propSplited[i]][k] == valor_sem_highlight) {
                                        prop_ob[propSplited[i]][k] = getHighlight(highlight_from_app[prop][j]);
                                    }
                                }
                            }
                        } else {
                            prop_ob[propSplited[i]] = getHighlight(highlight_from_app[prop][0]);
                        }
                    }
                }
            } else {
                var prop_without_text = prop.replace("_text", "");
                if (prop != prop_without_text && IsNotNullOrEmpty(obj_from_app, prop_without_text)) {
                    obj_from_app[prop_without_text] = getHighlight(highlight_from_app[prop][0]);
                } else if (Array.isArray(obj_from_app[prop])) {
                    for (var j = 0; j < highlight_from_app[prop].length; j++) {
                        var valor_sem_highlight = highlight_from_app[prop][j].replace(/_pre_tag_highlight_/g, '').replace(/_post_tag_highlight_/g, '');
                        for (var k = 0; k < obj_from_app[prop].length; k++) {
                            if (obj_from_app[prop][k] == valor_sem_highlight) {
                                obj_from_app[prop][k] = getHighlight(highlight_from_app[prop][j]);
                            }
                        }
                    }
                } else if (IsNotNullOrEmpty(obj_from_app, prop)) {
                    obj_from_app[prop] = getHighlight(highlight_from_app[prop][0]);
                } else {
                    for (var j = 0; j < highlight_from_app[prop].length; j++) {
                        var valor_sem_highlight = highlight_from_app[prop][j].toString().replace(/_pre_tag_highlight_/g, '').replace(/_post_tag_highlight_/g, '');
                        percorreObjetoHighlight(obj_from_app, valor_sem_highlight, highlight_from_app[prop][j]);
                    }
                }
            }
        }
    }
}

function percorreObjetoHighlight(collection, valor_sem_highlight, valor) {
    if (typeof collection == 'object') {
        for (var prop_from_collection in collection) {
            if (typeof collection[prop_from_collection] == "object") {
                if (IsNotNullOrEmpty(collection[prop_from_collection])) {
                    percorreObjetoHighlight(collection[prop_from_collection], valor_sem_highlight, valor);
                }
            } else if (collection[prop_from_collection] == valor_sem_highlight) {
                collection[prop_from_collection] = getHighlight(valor);
            }
        }
    }
}

function ToggleMenu() {
    $('#div_top .menu').toggleClass('open');
}

function replaceUrlParam(url, paramName, paramValue) {
    var pattern = new RegExp('\\b(' + paramName + '=).*?(&|$)')
    if (url.search(pattern) >= 0) {
        return url.replace(pattern, '$1' + paramValue + '$2');
    }
    return url + (url.indexOf('?') > 0 ? '&' : '?') + paramName + '=' + paramValue
}

function RestaurarDefaultDatatable() {
    localStorage.clear();
    location.reload();
}

function CarregarBotaoFavoritos(selector, chave) {
    if (IsNotNullOrEmpty(_notifiqueme, 'favoritos') && _notifiqueme.favoritos.indexOf(chave) > -1) {
        $(selector).html('<a href="javascript:void(0);" title="Remover da lista de favoritos" onclick="javascript:RemoverFavoritos(\'' + selector + '\',\'' + chave + '\');" ><img width="25" alt="*" src="' + _urlPadrao + '/Imagens/ico_del_fav.png" /></a>');
    } else {
        $(selector).html('<a href="javascript:void(0);" title="Marcar como favorito" onclick="javascript:AdicionarFavoritos(\'' + selector + '\',\'' + chave + '\');" ><img width="25" alt="*" src="' + _urlPadrao + '/Imagens/ico_add_fav.png" /></a>');
    }
}

function AdicionarFavoritos(selector, chave) {
    if (!IsNotNullOrEmpty(_notifiqueme)) {
        if ($('#modal_favoritos').hasClass('ui-dialog-content')) {
            $("#modal_favoritos").modallight('open');
        } else {
            $("<div id='modal_favoritos' />").modallight({
                sTitle: "Favoritos",
                sType: "alert",
                sContent: "É necessário estar logado. Deseja relizar login?",
                oButtons: [{
                    html: "<img alt='sim' src='" + _urlPadrao + "/Imagens/ico-login-p.png'> Sim",
                    click: function() {
                        document.location.href = "./LoginNotifiqueme.aspx?addfav=1&ch=" + chave;
                    }
                }, {
                    text: "Não",
                    click: function() {
                        $(this).dialog('close');
                    }
                }]
            });
        }
        return;
    }

    var sucesso = function(data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_notificacao_norma').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
        } else if (IsNotNullOrEmpty(data, 'favoritos')) {
            _notifiqueme.favoritos = data.favoritos;
            CarregarBotaoFavoritos(selector, chave);
            $('#div_notificacao_norma').messagelight({
                sContent: "A norma foi incluída nos seus favoritos.",
                sType: "success"
            });
        } else {
            top.document.location.reload();
        }
    }
    var inicio = function() {
        $('#super_loading').show();
    }
    var complete = function() {
        $('#super_loading').hide();
    }
    $.ajaxlight({
        sUrl: "./ashx/Push/FavoritosIncluir.ashx?ch=" + chave,
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: complete,
        fnBeforeSend: inicio,
        bAsync: true,
        iTimeout: 40000
    });
}

function RemoverFavoritos(selector, chave) {
    var sucesso = function(data) {
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#div_notificacao_norma').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
        } else if (IsNotNullOrEmpty(data, 'favoritos')) {
            if ($(selector).attr("ac") == "refresh") {
                top.document.location.reload();
            } else {
                _notifiqueme.favoritos = data.favoritos;
                CarregarBotaoFavoritos(selector, chave);
                $('#div_notificacao_norma').messagelight({
                    sContent: "A norma foi removida dos seus favoritos.",
                    sType: "success"
                });
            }
        } else {
            top.document.location.reload();
        }
    }
    var inicio = function() {
        $('#super_loading').show();
    }
    var complete = function() {
        $('#super_loading').hide();
    }
    $.ajaxlight({
        sUrl: "./ashx/Push/FavoritosExcluir.ashx?ch=" + chave,
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: complete,
        fnBeforeSend: inicio,
        bAsync: true,
        iTimeout: 40000
    });
}

function openDetalhes(id_doc) {
    doc_clicked = id_doc;
    var highlight_doc_clicked = getHighlightFromDocClicked(id_doc);
    AjaxDetalhesDeNorma((IsNotNullOrEmpty(highlight_doc_clicked) ? highlight_doc_clicked.highlight : null), id_doc);
    $('#modal_detalhes_norma .campo').text('');
    if (!$('#modal_detalhes_norma').hasClass('ui-dialog-content')) {
        $('#modal_detalhes_norma').modallight({
            sTitle: "Detalhes da Norma",
            sWidth: $(window).width() - 20,
            oPosition: null,
            oButtons: [],
            autoOpen: false
        });
    }
    $('#modal_detalhes_norma').modallight(['option', 'position', {
        my: 'center top',
        at: 'center top',
        of: "#div_top .top-bottom"
    }]);
    $('#modal_detalhes_norma').modallight('open');
}

function AjaxDetalhesDeNorma(highlight, id_doc) {

    var sucesso = function(data) {
        DetalhesNorma(data, highlight);
    };
    var inicio = function() {
        $('#div_loading_norma').show();
        $('#div_norma').hide();
    }
    var complete = function() {
        $('#div_loading_norma').hide();
    }
    $.ajaxlight({
        sUrl: './ashx/Visualizacao/NormaDetalhes.ashx' + (IsNotNullOrEmpty(id_doc) ? '?id_doc=' + id_doc : window.location.search),
        sType: "POST",
        fnSuccess: sucesso,
        fnComplete: complete,
        fnBeforeSend: inicio,
        fnError: null,
        bAsync: true
    });
}

function getIdentificacaoDeNorma(jNorma) {
    return jNorma.nm_tipo_norma + " " + (jNorma.nr_norma != '0' ? jNorma.nr_norma : "") + " de " + jNorma.dt_assinatura;
}

function getTitleNorma(jNorma) {
    var ds = jNorma.nm_tipo_norma + " " + (jNorma.nr_norma != '0' ? jNorma.nr_norma : "") + "_" + jNorma.dt_assinatura;
    ds = ds.replace(/\W+|[ãÃõÕçÇêÊéÉ\/]/g, "_");
    var mimetype = '';
    if(IsNotNullOrEmpty(jNorma.ar_atualizado, 'id_file')){
        mimetype = jNorma.ar_atualizado.mimetype;
    }
    else{
        var nm_tipo_publicacao = '';
        for (var i = 0; i < jNorma.fontes.length; i++) {
            if (IsNotNullOrEmpty(jNorma.fontes[i], 'nm_tipo_publicacao') && IsNotNullOrEmpty(jNorma.fontes[i], 'ar_fonte.id_file')) {
                nm_tipo_publicacao = jNorma.fontes[i].nm_tipo_publicacao.toLowerCase();
                if (i == 0 && (nm_tipo_publicacao == 'publicação' || nm_tipo_publicacao == 'pub')) {
                    mimetype = jNorma.fontes[i].ar_fonte.mimetype;
                    continue;
                }
                if (nm_tipo_publicacao == 'republicação' || nm_tipo_publicacao == 'rep') {
                    mimetype = jNorma.fontes[i].ar_fonte.mimetype;
                }
            }
        }
    }
    if(mimetype.indexOf('htm') > -1){
        ds += '.html';
    }
    else if (mimetype.indexOf('pdf') > -1) {
    ds += '.pdf';
    }
    return ds;
}



function DetalhesNorma(data, highlight) {
    var bCadastro = _aplicacao == "CADASTRO";
    if (IsNotNullOrEmpty(data)) {
        if (IsNotNullOrEmpty(data.error_message)) {
            $('#div_notificacao_norma').messagelight({
                sTitle: "Erro",
                sContent: data.error_message,
                sType: "error",
                sWidth: "",
                iTime: null
            });
            $('#div_norma fieldset').hide();
        } else if (IsNotNullOrEmpty(data, 'ch_norma')) {
            if (bCadastro) {
                var b_pode_editar = ValidarPermissaoPorTipoDeNorma(data.tipoDeNorma);
                if (b_pode_editar) {
                    if (ValidarPermissao(_grupos.nor_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar Norma" href="./EditarNorma.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="' + _urlPadrao + '/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.nor_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir Norma" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="' + _urlPadrao + '/Imagens/ico_trash_p.png"/></a>');
                    }
                }
            }

            $('#div_identificacao').html("<H1 identificacao>" + getIdentificacaoDeNorma(data) + "</H1> <H2 situacao style='color:#009900;'>" + data.nm_situacao + "</H2>");
            //$('#button_identificacao').html("<H1 identificacao>" + data.nm_tipo_norma + " " + (data.nr_norma != '0' ? data.nr_norma : "") + " de " + data.dt_assinatura + "</H1> <H2 situacao style='color:#009900;'>" + data.nm_situacao + "</H2> <img src=" + _urlPadrao + "/Imagens/ico_link.png alt='link' />");

            exibirBotaoCesta('sinj_norma_'+data._metadata.id_doc)

            var id_file = "";
            var path = "";
            var filename = getTitleNorma(data);
            if (IsNotNullOrEmpty(data.ar_atualizado, 'id_file')) {
                id_file = data.ar_atualizado.id_file;
            } else if (data.fontes.length > 0) {
                //rever essa parte pois está pegando o texto da primeira fonte porém existe um requisito que diz que o arquivo da norma pode ser republicado, descartando assim o da publicação (fontes/0)
                for (var i = 0; i < data.fontes.length; i++ ) {
                    if (IsNotNullOrEmpty(data.fontes[i].ar_fonte.id_file)) {
                        var nm_tipo_publicacao = data.fontes[i].nm_tipo_publicacao.toLowerCase();
                        if (nm_tipo_publicacao == 'publicação' || nm_tipo_publicacao == 'pub') {
                            id_file = data.fontes[i].ar_fonte.id_file;
                            continue;
                        }
                        if (nm_tipo_publicacao == 'republicação' || nm_tipo_publicacao == 'rep') {
                            id_file = data.fontes[i].ar_fonte.id_file;
                            break;
                        }
                    }
                }
            }
            if (IsNotNullOrEmpty(id_file)) {
                $('#div_arquivo').html(
                    '<a title="Baixar Arquivo" target="_blank" href="./Norma/' + data.ch_norma + '/' + filename + '"><img src="' + _urlPadrao + '/Imagens/ico_download_m.png" alt="download" /></a>'
                );
                if (bCadastro) {
                    $('#div_arquivo').append(
                        '&nbsp;&nbsp;<a title="visualizar texto" target="_blank" href="./TextoArquivoNorma.aspx?id_file=' + id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_m.png" alt="texto" /></a>'
                    );
                }
                $('#line_arquivo').show();
            } else {
                $('#line_arquivo').hide();
            }

            for (var i = 0; i < data.origens.length; i++) {
                $('#div_ds_origem').append("<b>" + data.origens[i].sg_orgao + "</b> - " + data.origens[i].nm_orgao + "<br/>");
            }
            if (data.st_pendencia) {
                $('#div_pendencia').html('<label id="label_pendencia"> Norma pendente de revisão </label> <br/>' +
                    '<span id="span_ds_pendencia">' + GetText(data.ds_pendencia) + '</span>');
            }

            $('#div_ds_ementa').html(data.ds_ementa);
            $('#div_nm_tipo_edicao').html(data.nm_tipo_edicao);
            if (IsNotNullOrEmpty(data.ds_observacao)) {
                $('#div_observacao').text(data.ds_observacao);
            } else {
                $('#line_observacao').hide();
            }
            if (IsNotNullOrEmpty(data.nm_pessoa_fisica_e_juridica)) {
                for (var i = 0; i < data.nm_pessoa_fisica_e_juridica.length; i++) {
                    $('#div_nome').append(data.nm_pessoa_fisica_e_juridica[i] + '<br/>');
                }
            } else {
                $('#line_nomes').hide();
            }
            if (IsNotNullOrEmpty(data.nm_apelido)) {
                $('#div_nm_apelido').text(data.nm_apelido);
            } else {
                $('#line_nm_apelido').hide();
            }
            if (IsNotNullOrEmpty(data.autorias) && data.autorias.length > 0) {
                var nm_autorias = "";
                for (var i = 0; i < data.autorias.length; i++) {
                    nm_autorias += (nm_autorias != "" ? "<br/>" : "") + data.autorias[i].nm_autoria;
                }
                $('#div_autorias').html(nm_autorias);
            } else {
                $('#line_autorias').hide();
            }
            if (data.fontes.length > 0) {
                for (var i = 0; i < data.fontes.length; i++) {
                    $('#tbody_fontes').append(
                        '<tr>' +
                            '<td>' + (IsNotNullOrEmpty(data.fontes[i], 'ar_diario.id_file') ?
                                '<a title="Baixar Diário" target="_blank" href="./Diario/' + data.fontes[i].ar_diario.id_file + '/' + data.fontes[i].ar_diario.filename + '">' + GetText(data.fontes[i].ds_diario) + ' <img src="' + _urlPadrao + '/Imagens/ico_pdf.png" alt="arquivo" width="18px" /></a>' : 
                                '<a href="./ResultadoDePesquisa.aspx?tipo_pesquisa=diario&ds_norma=' + data.nm_tipo_norma + ' ' + data.nr_norma + ' de ' + data.dt_assinatura + '&ch_tipo_fonte=' + data.fontes[i].ch_tipo_fonte + '&nm_tipo_fonte=' + data.fontes[i].nm_tipo_fonte + '&op_dt_assinatura=igual&dt_assinatura=' + GetText(data.fontes[i].dt_publicacao) + '">' + data.fontes[i].nm_tipo_fonte + ' ' + GetText(data.fontes[i].dt_publicacao) + '</a>') +
                            '</td>' +
                            '<td>' + GetText(data.fontes[i].nm_tipo_publicacao) + '</td>' +
                            '<td>' + GetText(data.fontes[i].nr_pagina) + '</td>' +
                            '<td>' + GetText(data.fontes[i].nr_coluna) + '</td>' +
                            '<td>' + GetText(data.fontes[i].ds_observacao_fonte) + '</td>' +
                            '<td>' + GetText(data.fontes[i].ds_republicacao) + '</td>' +
                        '</tr>'
                    );
                }
            } else {
                $('#line_fontes').hide();
            }
            if (data.vides.length > 0) {
                OrdenarVidesPorData(data.vides);

                var ano = "";
                for (var i = 0; i < data.vides.length; i++) {
                    ano = "";
                    if (IsNotNullOrEmpty(data.vides[i].dt_assinatura_norma_vide)) {
                        var split = data.vides[i].dt_assinatura_norma_vide.split('/');
                        if (split.length == 3) {
                            ano = split[2];
                        }
                    } else if (IsNotNullOrEmpty(data.vides[i].dt_publicacao_fonte_norma_vide)) {
                        var split = data.vides[i].dt_publicacao_fonte_norma_vide.split('/');
                        if (split.length == 3) {
                            ano = split[2];
                        }
                    }

                    // O dispositivo afetado que deve aparecer sempre deve ser com os dados da norma que foi alterada.
                    // Os campos de dispositivo afetado da norma ALTERADORA nao estao mais disponiveis na tela de cadastro,
                    //  mas os valores ainda existem na base de dados.

                    //                            A exibição do Vide deve seguir a seguinte ordem e formato:
                    //                            Art. 102, § 1º, inc. VIII, ali. "c", item 4
                    //                            Art. 102, Parágrafo único, inc. VIII, ali. "c", item 4
                    //                            Art. 102, Caput
                    var dispositivo_afetado = "";
                    if (data.vides[i].in_norma_afetada) {
                        dispositivo_afetado += (IsNotNullOrEmpty(data.vides[i].artigo_norma_vide) ? "Art. " + data.vides[i].artigo_norma_vide.replace(/^0?([1-9])$/, data.vides[i].artigo_norma_vide + 'º').replace(/^0+/, '') + ", " : "");
                        dispositivo_afetado += (IsNotNullOrEmpty(data.vides[i].paragrafo_norma_vide) ? (data.vides[i].paragrafo_norma_vide.toLowerCase() == "único" || data.vides[i].paragrafo_norma_vide.toLowerCase() == "unico" ? "Parágrafo " + data.vides[i].paragrafo_norma_vide + ", " : "§ " + data.vides[i].paragrafo_norma_vide.replace(/^([1-9])$/, data.vides[i].paragrafo_norma_vide + 'º') + ", ") : "");
                        dispositivo_afetado += (IsNotNullOrEmpty(data.vides[i].inciso_norma_vide) ? "inc. " + data.vides[i].inciso_norma_vide + ", " : "");
                        dispositivo_afetado += (IsNotNullOrEmpty(data.vides[i].alinea_norma_vide) ? "ali. \"" + data.vides[i].alinea_norma_vide + "\", " : "");
                        dispositivo_afetado += ((IsNotNullOrEmpty(data.vides[i].caput_norma_vide) && (data.vides[i].caput_norma_vide == true)) ? "Caput. " : "");
                        dispositivo_afetado += (IsNotNullOrEmpty(data.vides[i].item_norma_vide) ? "item " + data.vides[i].item_norma_vide + ", " : "");
                        dispositivo_afetado += (IsNotNullOrEmpty(data.vides[i].anexo_norma_vide) ? "Anexo " + data.vides[i].anexo_norma_vide : "");
                        if (dispositivo_afetado.substring(dispositivo_afetado.length - 2) == ", ") {
                            dispositivo_afetado = dispositivo_afetado.substring(0, dispositivo_afetado.length - 2);
                        }

                        $('#tbody_vides_normas_que_afetam').append(
                            '<tr>' +
                            '<td width="25%">' + (IsNotNullOrEmpty(data.vides[i].ds_texto_relacao) ? data.vides[i].ds_texto_relacao : data.vides[i].nm_tipo_relacao) + '</td>' +
                            '<td width="25%">' + dispositivo_afetado + '</td>' +
                            '<td width="10%"> pelo(a) </td>' +
                            '<td width="30%">' + (IsNotNullOrEmpty(data.vides[i].ch_norma_vide) ? '<a href="./DetalhesDeNorma.aspx?id_norma=' + data.vides[i].ch_norma_vide + '" title="Visualizar Detalhes da Norma">' + data.vides[i].nm_tipo_norma_vide + ' ' + data.vides[i].nr_norma_vide + '</a>' : data.vides[i].nm_tipo_norma_vide + ' ' + data.vides[i].nr_norma_vide) + '/' + ano + '</td>' +
                            (bCadastro ? '<td width="10%">' + (b_pode_editar && ValidarPermissao(_grupos.nor_edt) ? '<a href="./EditarVide.aspx?id_doc=' + data._metadata.id_doc + '&ch_vide=' + data.vides[i].ch_vide + '" title="Editar Vide" ><img src="' + _urlPadrao + '/Imagens/ico_pencil_p.png" alt="editar" /></a>&nbsp;<a href="javascript:void(0);" onclick="javascript:ExcluirVide(this,' + data._metadata.id_doc + ',\'' + data.vides[i].ch_vide + '\');" title="Excluir Vide" ><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" alt="excluir" /></a>' : '') + '</td>' : '') +
                            '</tr>' +
                            (IsNotNullOrEmpty(data.vides[i].ds_comentario_vide) ? '<tr><td colspan="5" style="background-color:transparent;"><table style="width:98%; float:right;"><thead><tr><th class="text-left">Comentário</th></tr></thead><tbody id="tbody_comentario_vide"><tr><td>' + data.vides[i].ds_comentario_vide + '</td></tr></tbody></table></td></tr>' : '')
                        );
                    } else {
                        // Caso a norma afete outra, os valores exibidos estao persistidos em campos diferentes:
                        dispositivo_afetado += (IsNotNullOrEmpty(data.vides[i].artigo_norma_vide_outra) ? "Art. " + data.vides[i].artigo_norma_vide_outra.replace(/^0?([1-9])$/, data.vides[i].artigo_norma_vide_outra + 'º').replace(/^0+/, '') + ", " : "");
                        dispositivo_afetado += (IsNotNullOrEmpty(data.vides[i].paragrafo_norma_vide_outra) ? (data.vides[i].paragrafo_norma_vide_outra.toLowerCase() == "único" || data.vides[i].paragrafo_norma_vide_outra.toLowerCase() == "unico" ? "Parágrafo " + data.vides[i].paragrafo_norma_vide_outra + ", " : "§ " + data.vides[i].paragrafo_norma_vide_outra.replace(/^([1-9])$/, data.vides[i].paragrafo_norma_vide_outra + 'º') + ", ") : "");
                        dispositivo_afetado += (IsNotNullOrEmpty(data.vides[i].inciso_norma_vide_outra) ? "inc. " + data.vides[i].inciso_norma_vide_outra + ", " : "");
                        dispositivo_afetado += (IsNotNullOrEmpty(data.vides[i].alinea_norma_vide_outra) ? "ali. \"" + data.vides[i].alinea_norma_vide_outra + "\", " : "");
                        dispositivo_afetado += ((IsNotNullOrEmpty(data.vides[i].caput_norma_vide_outra) && (data.vides[i].caput_norma_vide_outra == true)) ? "Caput. " : "");
                        dispositivo_afetado += (IsNotNullOrEmpty(data.vides[i].item_norma_vide_outra) ? "item " + data.vides[i].item_norma_vide_outra + ", " : "");
                        dispositivo_afetado += (IsNotNullOrEmpty(data.vides[i].anexo_norma_vide_outra) ? "Anexo " + data.vides[i].anexo_norma_vide_outra : "");
                        if (dispositivo_afetado.substring(dispositivo_afetado.length - 2) == ", ") {
                            dispositivo_afetado = dispositivo_afetado.substring(0, dispositivo_afetado.length - 2);
                        }

                        $('#tbody_vides_normas_afetadas').append(
                            '<tr>' +
                            '<td width="25%">' + (IsNotNullOrEmpty(data.vides[i].ds_texto_relacao) ? data.vides[i].ds_texto_relacao : data.vides[i].nm_tipo_relacao) + '</td>' +
                            '<td width="25%">' + dispositivo_afetado + '</td>' +
                            '<td width="10%">' + (IsNotNullOrEmpty(dispositivo_afetado) ? "do(a)" : "&nbsp;&nbsp;&nbsp;&nbsp;") + '</td>' +
                            '<td width="30%">' + (IsNotNullOrEmpty(data.vides[i].ch_norma_vide) ? '<a href="./DetalhesDeNorma.aspx?id_norma=' + data.vides[i].ch_norma_vide + '" title="Visualizar Detalhes da Norma">' + data.vides[i].nm_tipo_norma_vide + ' ' + data.vides[i].nr_norma_vide + '</a>' : data.vides[i].nm_tipo_norma_vide + ' ' + data.vides[i].nr_norma_vide) + '/' + ano + '</td>' +
                            (bCadastro ? '<td width="10%">' + (b_pode_editar && ValidarPermissao(_grupos.nor_edt) ? '<a href="./EditarVide.aspx?id_doc=' + data._metadata.id_doc + '&ch_vide=' + data.vides[i].ch_vide + '" title="Editar Vide" ><img src="' + _urlPadrao + '/Imagens/ico_pencil_p.png" alt="editar" /></a>&nbsp;<a href="javascript:void(0);" onclick="javascript:ExcluirVide(this,' + data._metadata.id_doc + ',\'' + data.vides[i].ch_vide + '\');" title="Excluir Vide" ><img src="' + _urlPadrao + '/Imagens/ico_delete_p.png" alt="excluir" /></a>' : '') + '</td>' : '') +
                            '</tr>' +
                            (IsNotNullOrEmpty(data.vides[i].ds_comentario_vide) ? '<tr><td colspan="5" style="background-color:transparent;"><table style="width:98%; float:right;"><thead><tr><th class="text-left">Comentário</th></tr></thead><tbody id="tbody_comentario_vide"><tr><td>' + data.vides[i].ds_comentario_vide + '</td></tr></tbody></table></td></tr>' : '')
                        );
                    }
                }
            } else {
                $('#line_vides_normas_que_afetam').hide();
                $('#line_vides_normas_afetadas').hide();
            }
            if (!$("#tbody_vides_normas_que_afetam tr").length > 0) {
                $('#line_vides_normas_que_afetam').hide();
            }
            if (!$("#tbody_vides_normas_afetadas tr").length > 0) {
                $('#line_vides_normas_afetadas').hide();
            }
            if (data.id_ambito == 1) {
                $('#line_ambito').hide();
            } else {
                $('#div_nm_ambito').html(data.nm_ambito);
            }
            if (data.indexacoes.length > 0) {
                // Estrutura de repetição que percorre as indexacoes
                // e tem como indice (chamado de voc) o numero de vocabularios
                for (voc in data.indexacoes) {
                    //Quantidade de vocabularios na linha
                    var tamanho = data.indexacoes[voc].vocabulario.length - 1;
                    //Outra repetição para percorrer cada termo dos vocabularios
                    //O indice eh chamado de 'ter'
                    for (ter in data.indexacoes[voc].vocabulario) {
                        var nm_termo = data.indexacoes[voc].vocabulario[ter].nm_termo;
                        var ch_termo = data.indexacoes[voc].vocabulario[ter].ch_termo;
                        var termo = "<a href='./ResultadoDePesquisa.aspx?tipo_pesquisa=norma&ch_termo=" + ch_termo + "&nm_termo=" + nm_termo + "'>" + nm_termo + "</a>";
                        // Se o indice atual for menor que o tamanho, acrescenta virgula após o termo.
                        // Se não, quebra a linha
                        (ter < tamanho ? $('#div_indexacao').append(termo + ', ') : $('#div_indexacao').append(termo + '<br>'));
                    }
                }
            } else {
                $('#line_indexacoes').hide();
            }

            var link_norma = 'Detalhes da Norma - ' + '<a href="./DetalhesDeNorma.aspx?id_norma=' + data.ch_norma + '" target="_blank" title="link da norma">' + document.location.href.replace(document.location.href.replace(/^.*[\\\/]/, ''), 'DetalhesDeNorma.aspx?id_norma=' + data.ch_norma) + '</a>';

            var link_texto = "";
            if (IsNotNullOrEmpty(id_file)) {
                var path = document.location.pathname.split('/');
                var link = document.location.protocol + '//' + document.location.host + '/' + (path.length > 1 ? path[path.length - 2] + '/' : '') + 'BaixarArquivoNorma.aspx?id_norma=' + data.ch_norma;
                link_texto = 'Texto da Norma - ' + '<a href="' + link + '" target="_blank" title="link do arquivo da norma">' + link + '</a>';
            }

            $('#div_links').html(
                link_norma + (IsNotNullOrEmpty(link_texto) ? '<br/>' + link_texto : '')
            );

            if (IsNotNullOrEmpty(_notifiqueme) && _notifiqueme.ch_normas_monitoradas.indexOf(data.ch_norma) >= 0) {
                $('#button_notifiqueme').html('<div class="div-light-button fr"><a href="javascript:void(0);" onclick="javascript:PararNotificar(\'' + data.ch_norma + '\');" title="Para de receber e-mail sobre as edições deste ato." ><img alt="@" src="' + _urlPadrao + '/Imagens/ico_stop_email_p.png" />Parar Notificação</a></div>');
            } else {
                $('#button_notifiqueme').html('<div class="div-light-button fr"><a href="javascript:void(0);" onclick="javascript:Notificar(\'' + data.ch_norma + '\');" title="Receber e-mail quando este ato for editado." ><img alt="@" src="' + _urlPadrao + '/Imagens/ico_email_p.png" />Notificar-me</a></div>');
            }
            CarregarBotaoFavoritos('#button_favoritos', 'norma_' + data.ch_norma);
            if (bCadastro) {
                DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
                DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                if (ValidarPermissao(_grupos.nor_edt)) {
                    $('#div_controls').append('<div class="div-light-button"><a href="./CadastrarVide.aspx?id_doc=' + data._metadata.id_doc + '&in_acao=' + data.st_acao + '" title="Cadastrar Vide"><img src="' + _urlPadrao + '/Imagens/ico_add_p.png" alt="adicionar" />Adicionar Vide</a></div>');
                }
            }

            // Dados das ações só existem para normas do G2
            if (IsNotNullOrEmpty(data, "tipoDeNorma")) {
                if (!data.tipoDeNorma.in_g2) {
                    $('.dados-das-acoes').hide();
                }
            }
            if (IsNotNullOrEmpty(data.ds_procedencia)) {
                $('#ds_procedencia').text(data.ds_procedencia);
            } else {
                $('#div_procedencia').hide();
            }
            if (IsNotNullOrEmpty(data.ds_parametro_constitucional)) {
                $('#ds_parametro_constitucional').text(data.ds_parametro_constitucional);
            } else {
                $('#div_parametro_constitucional').hide();
            }
            if (IsNotNullOrEmpty(data.decisoes)) {
                for (var i = 0; i < data.decisoes.length; i++) {
                    AdicionarDecisao(data.decisoes[i].nm_decisao, data.decisoes[i].in_decisao, data.decisoes[i].ds_complemento, data.decisoes[i].dt_decisao);
                }
            } else {
                $('#div_decisoes').hide();
            }
            if (IsNotNullOrEmpty(data.interessados)) {
                for (var i = 0; i < data.interessados.length; i++) {
                    AdicionarInteressado(data.interessados[i].nm_interessado, data.interessados[i].ch_interessado);
                }
            } else {
                $('#line_interessados').hide();
            }
            if (IsNotNullOrEmpty(data.requerentes)) {
                for (var i = 0; i < data.requerentes.length; i++) {
                    AdicionarRequerente(data.requerentes[i].nm_requerente, data.requerentes[i].ch_requerente);
                }
            } else {
                $('#div_requerentes').hide();
            }
            if (IsNotNullOrEmpty(data.requeridos)) {
                for (var i = 0; i < data.requeridos.length; i++) {
                    AdicionarRequerido(data.requeridos[i].nm_requerido, data.requeridos[i].ch_requerido);
                }
            } else {
                $('#div_requeridos').hide();
            }
            if (bCadastro && IsNotNullOrEmpty(data.procuradores_responsaveis)) {
                for (var i = 0; i < data.procuradores_responsaveis.length; i++) {
                    AdicionarProcurador(data.procuradores_responsaveis[i].nm_procurador_responsavel, data.procuradores_responsaveis[i].ch_procurador_responsavel);
                }
            } else {
                $('#div_procuradores').hide();
            }
            if (IsNotNullOrEmpty(data.relatores)) {
                for (var i = 0; i < data.relatores.length; i++) {
                    AdicionarRelator(data.relatores[i].nm_relator, data.relatores[i].ch_relator);
                }
            } else {
                $('#div_relatores').hide();
            }
            if (IsNotNullOrEmpty(data, 'ar_acao.id_file')) {
                $('#div_ds_texto_da_acao').append(
                    '<a title="baixar arquivo" target="_blank" href="./BaixarArquivoNorma.aspx?id_file=' + data.ar_acao.id_file + '"><img src="' + _urlPadrao + '/Imagens/ico_download_p.png" alt="download" /></a>' +
                    '&nbsp;&nbsp;' +
                    '<a title="visualizar texto" target="_blank" href="./TextoArquivoNorma.aspx?id_file=' + data.ar_acao.id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_p.png" alt="texto" /></a>'
                );
            } else {
                $('#div_texto_da_acao').hide();
            }

            if (IsNotNullOrEmpty(data.ds_efeito_decisao)) {
                $('#ds_efeito_decisao').text(data.ds_efeito_decisao);
            } else {
                $('#div_efeito_decisao').hide();
            }
            if (IsNotNullOrEmpty(data.url_referencia)) {
                $('#ds_url_acompanhamento').html('<a href="' + data.url_referencia + '" target="_blank">' + data.url_referencia + '</a>');
            } else {
                $('#div_url_acompanhamento').hide();
            }

            $('table').cardtable({
                myClass: 'stacktable small-only'
            });
            aplicarHighlight('.', highlight, '#div_norma');
            $('#div_norma').show();
        }
    }
}

function fn_submit_arquivo(id_div_arquivo) {
    if (!IsNotNullOrEmpty(form_editar_arquivo.filename.value)) {
        $('#editar_arquivo_notificacao').messagelight({
            sContent: "É necessário informar o nome do arquivo.",
            sType: "error"
        });
        focusElement(form_editar_arquivo.filename);
        $('#form_editar_arquivo').parent().animate({
            scrollTop: '0px'
        }, 'fast');
        return false;
    }
    CKEDITOR.instances.arquivo.updateElement();
    document.getElementById('arquivo').value = window.escape(document.getElementById('arquivo').value);
    var sucesso = function(data) {
        $('#super_loading').hide();
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#editar_arquivo_notificacao').messagelight({
                sContent: data.error_message,
                sType: "error"
            });
        } else if (IsNotNullOrEmpty(data, 'success_message')) {
            $('#' + id_div_arquivo + ' .id_file').val(data.file.id_file);
            $('#' + id_div_arquivo + ' .uuid').val(data.file.uuid);
            $('#' + id_div_arquivo + ' .mimetype').val(data.file.mimetype);
            $('#' + id_div_arquivo + ' .filesize').val(data.file.filesize);
            $('#' + id_div_arquivo + ' .id_file').val(data.file.id_file);
            $('#' + id_div_arquivo + ' .filename').val(data.file.filename);
            $('#' + id_div_arquivo + ' .name').text(data.file.filename);
            $('#' + id_div_arquivo + ' .attach').hide();
            $('#' + id_div_arquivo + ' .delete').show();
            $("#modal_arquivo").modallight('close');
        }
    }
    var beforeSubmit = function() {
        $('#super_loading').show();
    }
    $.ajaxlight({
        sUrl: './ashx/Arquivo/NormaEditarArquivo.ashx' + window.location.search,
        sType: "POST",
        sFormId: "form_editar_arquivo",
        fnSuccess: sucesso,
        fnBeforeSubmit: beforeSubmit,
        bAsync: true
    });
    return false;
}

function fnSubmitInputFile(id_div_arquivo) {
    if (!IsNotNullOrEmpty(form_editar_arquivo.filename.value)) {
        $('#editar_arquivo_notificacao').messagelight({
            sContent: "É necessário informar o nome do arquivo.",
            sType: "error"
        });
        focusElement(form_editar_arquivo.filename);
        $('#form_editar_arquivo').parent().animate({
            scrollTop: '0px'
        }, 'fast');
        return false;
    }
    CKEDITOR.instances.arquivo.updateElement();
    document.getElementById('arquivo').value = window.escape(document.getElementById('arquivo').value);
    var sucesso = function(data) {
        $('#super_loading').hide();
        if (IsNotNullOrEmpty(data, 'error_message')) {
            $('#editar_arquivo_notificacao').messagelight({
                sContent: data.error_message,
                sType: "error"
            });
        } else if (IsNotNullOrEmpty(data, 'success_message')) {
            $('#' + id_div_arquivo + ' .json_arquivo').val(JSON.stringify(data.file));
            $('#' + id_div_arquivo + ' .name').text(data.file.filename);
            $('#' + id_div_arquivo + ' .attach').hide();
            $('#' + id_div_arquivo + ' .delete').show();
            $("#modal_arquivo").modallight('close');
        }
    }
    var beforeSubmit = function() {
        $('#super_loading').show();
    }
    $.ajaxlight({
        sUrl: './ashx/Arquivo/NormaEditarArquivo.ashx' + window.location.search,
        sType: "POST",
        sFormId: "form_editar_arquivo",
        fnSuccess: sucesso,
        fnBeforeSubmit: beforeSubmit,
        bAsync: true
    });
    return false;
}

function EditarArquivo(id_div_arquivo, path) {
    var id_file = $('#' + id_div_arquivo + ' .id_file').val();
    var title_modal = "Incluir Arquivo";
    if (IsNotNullOrEmpty(CKEDITOR.instances['arquivo'])) {
        CKEDITOR.instances['arquivo'].destroy(true);
        $('#arquivo').val('');
        $('#arquivo').hide();
        $('#form_editar_arquivo input').val('');
    }
    if (IsNotNullOrEmpty(id_file)) {
        title_modal = "Editar Arquivo";
        var id_doc = GetParameterValue('id_doc');
        var sucesso = function(data) {
            $('#modal_arquivo .id_file').val(id_file);
            $('#modal_arquivo .path').val(path);
            $('#modal_arquivo .id_doc').val(id_doc);
            $('#modal_arquivo .filename').val(data.filename);
            $('#arquivo').val(window.unescape(data.fileencoded));
            setTimeout(function() {
                CKEDITOR.replace('arquivo', { extraAllowedContent: 'p[linkname,replaced_by]' });
                //$('#editar_arquivo_loading').hide(); 
            }, 2000);

        }
        var inicio = function(data) {
            $('#super_loading').show();
            //$('#editar_arquivo_loading').show(); 
        }
        var complete = function() {
            setTimeout(function() {
                $('#super_loading').hide();
            }, 2000);
        }
        $.ajaxlight({
            sUrl: "./ashx/Arquivo/NormaExibirArquivo.ashx?id_file=" + id_file,
            sType: "GET",
            fnSuccess: sucesso,
            fnComplete: complete,
            fnBeforeSend: inicio,
            fnError: null,
            bAsync: true
        });
    } else {
        setTimeout(function() {
            CKEDITOR.replace('arquivo');
            //$('#editar_arquivo_loading').hide(); 
        }, 2000);
    }
    if ($("#modal_arquivo").hasClass('ui-dialog-content')) {
        $("#modal_arquivo").modallight('open');
    } else {
        $("#modal_arquivo").modallight({
            sTitle: title_modal,
            sWidth: '900',
            sHeight: '600',
            oButtons: [{
                text: "Salvar",
                click: function() {
                    fn_submit_arquivo(id_div_arquivo);
                }
            }, {
                text: "Cancelar",
                click: function() {
                    $("#modal_arquivo").modallight('close');
                }
            }]
        });
    }
}

function editarInputFile(el) {
    var parent = $(el).parent();
    var sJson_arquivo = $('.json_arquivo', parent).val();
    var path = $(el).attr('path');
    var oJson_arquivo = {};
    if (IsNotNullOrEmpty(sJson_arquivo)) {
        oJson_arquivo = JSON.parse(sJson_arquivo);
    }
    var title_modal = "Incluir Arquivo";
    if (IsNotNullOrEmpty(CKEDITOR.instances['arquivo'])) {
        CKEDITOR.instances['arquivo'].destroy(true);
        $('#arquivo').val('');
        $('#arquivo').hide();
        $('#form_editar_arquivo input').val('');
    }
    if (IsNotNullOrEmpty(oJson_arquivo, 'id_file')) {
        title_modal = "Editar Arquivo";
        var id_doc = GetParameterValue('id_doc');
        var sucesso = function(data) {
            $('#modal_arquivo .id_file').val(oJson_arquivo.id_file);
            $('#modal_arquivo .path').val(path);
            $('#modal_arquivo .id_doc').val(id_doc);
            $('#modal_arquivo .filename').val(data.file.filename);
            $('#arquivo').val(window.unescape(data.fileencoded));
            gInicio();
            setTimeout(function() {
                loadCkeditor('arquivo');
                gComplete();
            }, 2000);
        }
        $.ajaxlight({
            sUrl: "./ashx/Arquivo/HtmlFileEncoded.ashx?nm_base=sinj_norma&id_file=" + oJson_arquivo.id_file,
            //sUrl: "./ashx/Arquivo/NormaExibirArquivo.ashx?id_file=" + oJson_arquivo.id_file,
            sType: "GET",
            fnSuccess: sucesso,
            fnBeforeSend: gInicio,
            fnError: null,
            bAsync: true
        });
    } else {
        gInicio();
        setTimeout(function() {
            loadCkeditor('arquivo');
            gComplete();
            //$('#editar_arquivo_loading').hide(); 
        }, 2000);
    }
    if ($("#modal_arquivo").hasClass('ui-dialog-content')) {
        $("#modal_arquivo").modallight('destroy');
    }
    $("#modal_arquivo").modallight({
        sTitle: title_modal,
        sWidth: '900',
        sHeight: '600',
        oButtons: [{
            text: "Salvar",
            click: function() {
                fnSubmitInputFile(parent.attr('id'));
            }
        }, {
            text: "Cancelar",
            click: function() {
                $("#modal_arquivo").modallight('close');
            }
        }]
    });
}


function montarDetalhesJson(jData, nm_table) {
    if (jData instanceof Array) {
        var arrayval = "";
        for (var i = 0; i < jData.length; i++) {
            arrayval = arrayval + (arrayval != "" ? "<br/>" : "") + montarDetalhesJson(jData[i], nm_table + " " + (i + 1));
        }
        return arrayval;
    } else if (jData instanceof Object) {
        if (jData != null) {
            var id_div = Guid('N');
            var items_jData = new Array();
            if (nm_table != null && nm_table != "") {
                items_jData.push('<div class="header_table">' + nm_table + '<a href="javascript:void(0);" class="a_expand_table" onclick="javascript:expandirTabela(\'' + id_div + '\');" id="a_' + id_div + '"><img alt="ocultar" src="' + _urlPadrao + '/Imagens/ico_up.png"/></a></div>');
            }
            $.each(jData, function(key_jData, val_jData) {
                var td_value = montarDetalhesJson(val_jData, key_jData);
                if (typeof td_value == "string" && IsNotNullOrEmpty(td_value)) {
                    if (td_value.indexOf('class="div_generated_json table"') > -1) {
                        items_jData.push('<div class="line"><div class="column w-100-pc">' + td_value + '</div></div>');
                    } else {
                        items_jData.push('<div class="line"><div class="column w-30-pc"><label>' + key_jData + ':</label></div><div class="column w-70-pc ' + key_jData + '">' + td_value + '</div></div>');
                    }
                }
            });
            return '<div id="table_' + id_div + '" class="div_generated_json table">' + items_jData.join('\n') + '<div class="footer_table"></div></div>';
        }
    } else if (jData != null) {
        return jData;
    } else {
        return "";
    }
}

function montarTabelaJson(jData, nm_table) {
    if (jData instanceof Array) {
        var arrayval = "";
        for (var i = 0; i < jData.length; i++) {
            arrayval = arrayval + (arrayval != "" ? "<br/>" : "") + montarTabelaJson(jData[i], nm_table + " " + (i + 1));
        }
        return arrayval;
    } else if (jData instanceof Object) {
        if (jData != null) {
            var id_div = Guid('N');
            var items_jData = new Array();
            if (nm_table != null && nm_table != "") {
                items_jData.push('<caption>' + nm_table + '<a href="javascript:void(0);" class="a_expand_table" onclick="javascript:expandirTabela(\'' + id_div + '\');" id="a_' + id_div + '"><img alt="ocultar" src="' + _urlPadrao + '/Imagens/ico_up.png"/></a></caption>');
            }
            $.each(jData, function(key_jData, val_jData) {
                var td_value = montarTabelaJson(val_jData, key_jData);
                if (typeof td_value == "string" && IsNotNullOrEmpty(td_value)) {
                    if (td_value.indexOf('class="table_generated_json"') > -1) {
                        items_jData.push('<tr><td colspan="2">' + td_value + '</td></tr>');
                    } else {
                        items_jData.push('<tr><th>' + key_jData + '</th><td class="' + key_jData + '">' + td_value + '</td></tr>');
                    }
                }
            });
            return '<table id="table_' + id_div + '" class="table_generated_json">' + items_jData.join('\n') + '</table>';
        }
    } else if (jData != null) {
        return jData;
    } else {
        return "";
    }
}

function expandirTabela(id) {
    if ($('#a_' + id).hasClass('hide')) {
        $('#table_' + id + ' div.line').show(100);
        $('#table_' + id + ' tbody').show(100);
        $('#a_' + id).removeClass('hide');
        $('#a_' + id).html('<img alt="ocultar" src="' + _urlPadrao + '/Imagens/ico_up.png"/>');
    } else {
        $('#table_' + id + ' div.line').hide(100);
        $('#table_' + id + ' tbody').hide(100);
        $('#a_' + id).addClass('hide');
        $('#a_' + id).html('<img alt="expandir" src="' + _urlPadrao + '/Imagens/ico_down.png"/>');
    }
}

function selecionarOperador(el) {
    var id_target = el.getAttribute('id_target');
    if (el.value == "intervalo") {
        $('#' + id_target).show();
    } else {
        $('#' + id_target).hide();
    }
}

//facilita a utilização de messagelight
//Exibe uma div estilizada de acordo com o type (error, alert, info, success)
function notificar(parent, message, type, container) {
    if (isNullOrEmpty(container)) {
        if ($('.notify', $(parent)).length <= 0) {
            $(parent).prepend($('<div class="notify" style="display:none;"/>'));
        }
        container = $('.notify', $(parent));
    }
    $(container).messagelight({
        sContent: message,
        sType: type,
        bClose: true
    });
}

//facilita a utilização de modallight
//Exibe um dialog modal estilizado de acordo com o type (error, alert, info, success)
function ShowDialog(jOption) {
    var jModalOption = {
        id_element: 'simpledialog',
        sTitle: 'Atenção',
        urlRedirect: ''
    };
    $.extend(jModalOption, jOption);
    if ($('#' + jModalOption.id_element).length <= 0) {
        $('body').append($('<div id="' + jModalOption.id_element + '" />'));
    }
    if ($('#' + jModalOption.id_element).hasClass('ui-dialog-content')) {
        $('#' + jModalOption.id_element).dialog('destroy');
    }
    $('#' + jModalOption.id_element).modallight(jModalOption);
    return;
}

function fnSalvarForm(id_form, _sucesso) {
    try {
        //url para qual será feita o ajax
        var sUrl = $('#' + id_form).attr("url-ajax");
        //url para redirecionar em caso de sucesso do ajax
        var sUrlRedirect = $('#' + id_form).attr("url-redirect");
        //parametro para ser adicionado à sUrlRedirect com o valor retornado do server.
        //Ex: pagina.aspx?id_doc=30. No exemplo id_doc é o param-redirect e 30 é o valor retornado do server
        var sParamRedirect = $('#' + id_form).attr("param-redirect");
        var sButtonRedirect = $('#' + id_form).attr("button-redirect");
        var sDivNotificar = $('#' + id_form).attr("divNotificar");
        if (!IsNotNullOrEmpty(sDivNotificar)) {
            var oDivNotificar = $('#' + id_form + ' .notify');
            if (oDivNotificar.length > 0) {
                sDivNotificar = $(oDivNotificar[0]).attr('id');
                if (!IsNotNullOrEmpty(sDivNotificar)) {
                    sDivNotificar = Guid();
                    $(oDivNotificar).attr('id', sDivNotificar);
                }
            }
        }
        //determinar os botões que aparecerão no modal.
        //se type-buttons for nulo ou vazio o sucesso do ajax fará só o redirect(caso haja url-redirect)
        var sTypeButtons = $('#' + id_form).attr("type-buttons");

        Validar(id_form);

        var sucesso = typeof _sucesso === "function" ? _sucesso : function(data) {
            gComplete();
            if (IsNotNullOrEmpty(data, 'error_message')) {
                notificar('#' + id_form, data.error_message, 'error');
            } else if (IsNotNullOrEmpty(data, "success_message")) {
                var buttons = null;
                if (IsNotNullOrEmpty(sUrlRedirect)) {
                    if (IsNotNullOrEmpty(sParamRedirect)) {
                        var sParamRedirectSplited = sParamRedirect.split(',');
                        for (var i = 0; i < sParamRedirectSplited.length; i++) {
                            if (IsNotNullOrEmpty(sParamRedirectSplited[i]) && IsNotNullOrEmpty(data, sParamRedirectSplited[i])) {
                                sUrlRedirect += (sUrlRedirect.indexOf('?') > -1 ? '&' : '?') + sParamRedirectSplited[i] + '=' + escape(eval('data.' + sParamRedirectSplited[i]));
                            }
                        }
                    }
                    buttons = {
                        "salvar": [{
                            html: "<img src='" + _urlPadrao + "/Imagens/ico_add_p.png' alt='add' /> Novo",
                            click: function() {
                                $(this).dialog('close');
                            }
                        }, {
                            html: (IsNotNullOrEmpty(sButtonRedirect) ? sButtonRedirect : "<img title='Visualizar' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' alt='detalhes' /> Visualizar"),
                            click: function() {
                                hrefRedirect(sUrlRedirect);
                            }
                        }],
                        "editar": [{
                            html: (IsNotNullOrEmpty(sButtonRedirect) ? sButtonRedirect : "<img title='Visualizar' src='" + _urlPadrao + "/Imagens/ico_loupe_p.png' alt='detalhes' /> Visualizar"),
                            click: function() {
                                hrefRedirect(sUrlRedirect);
                            }
                        }],
                        "default": [{
                            text: "Ok",
                            click: function() {
                                document.location.href = sUrlRedirect;
                            }
                        }]
                    };
                } else if (IsNotNullOrEmpty(sTypeButtons)) {
                    buttons = {
                        "default": [{
                            text: "Ok",
                            click: function() {
                                $(this).dialog('close');
                            }
                        }]
                    };
                }

                if (IsNotNullOrEmpty(sTypeButtons)) {
                    ShowDialog({
                        id_element: "modal_notificacao_success",
                        sTitle: "Sucesso",
                        sContent: data.success_message,
                        sType: "success",
                        oButtons: buttons[sTypeButtons],
                        fnClose: function() {
                            location.reload();
                        }
                    });
                } else if (IsNotNullOrEmpty(sUrlRedirect)) {
                    document.location.href = sUrlRedirect;
                }
            }
        };

        //Ajax({ url: sUrl, type: "POST", idForm: id_form, success: sucesso, beforeSubmit: gInicio, complete: gComplete });
        $.ajaxlight({
            sFormId: id_form,
            sUrl: sUrl,
            sType: "POST",
            fnSuccess: sucesso,
            fnComplete: gComplete,
            fnBeforeSubmit: gInicio,
            bAsync: true
        });

    } catch (ex) {
        $('#' + id_form + ' .notify').messagelight({
            sTitle: "Erro nos dados informados",
            sContent: ex,
            sType: "error"
        });
        $("html, body").animate({
            scrollTop: 0
        }, "slow");
    }
    return false;
}

//Produção de documentos
var diretorios = [];
var results = [];

function carregarArquivosProduzidos(_diretorio) {
    $('#div_list_dir').html('');
    $('#div_list_arq').html('');

    var sucesso = function (data) {
        if (IsNotNullOrEmpty(data, 'results')) {
            results = data.results;
            for (var i = 0; i < data.results.length; i++) {
                if (data.results[i].nr_tipo_arquivo == 0 && data.results[i].nr_nivel_arquivo == 1) {
                    if (data.results[i].ch_arquivo == '000shared/images') {
                        $('#div_list_dir').append('<div class="div_folder"><a href="javascript:void(0);" nivel="' + data.results[i].nr_nivel_arquivo + '" chave="' + data.results[i].ch_arquivo + '" onclick="javascript:selecionarPasta(this)" ><span>&gt;&nbsp;</span><img src="' + _urlPadrao + '/Imagens/ico_image.png" width="18" height="18" /> ' + data.results[i].nm_arquivo + '</a></div>');
                    }
                    else {
                        $('#div_list_dir').append('<div class="div_folder"><a href="javascript:void(0);" nivel="' + data.results[i].nr_nivel_arquivo + '" chave="' + data.results[i].ch_arquivo + '" onclick="javascript:selecionarPasta(this)" ><span>&gt;&nbsp;</span><img src="' + _urlPadrao + '/Imagens/ico_folder.png" width="18" height="18" /> ' + data.results[i].nm_arquivo + '</a></div>');
                    }
                }
            }
            $('#div_list_dir .div_folder:first a').click();
        }
    }

    if (!IsNotNullOrEmpty(results)) {
        var aux_query = "";
        if(isArray(_diretorio)){
            for(var i=0;i< _diretorio.length;i++){
                aux_query += (aux_query != "" ? "&" : "") + "ch_arquivo_raiz=" + _diretorio[i];
            }
        }
        else{
            aux_query = "ch_arquivo_raiz=" + _diretorio;
        }
        $.ajaxlight({
            sUrl: "./ashx/Consulta/ArquivosConsulta.ashx?" + aux_query,
            sType: "GET",
            fnSuccess: sucesso,
            fnComplete: gComplete,
            fnBeforeSend: gInicio,
            bAsync: true
        });
    } else {
        sucesso({
            "results": results
        })
    }
}

function selecionarPasta(el) {
    var ch_dir = el.getAttribute('chave');
    var ch_nivel = el.getAttribute('nivel');
    $('a.open_dialog_folder').attr('ch_folder_selected', ch_dir);
    $('a.open_dialog_delete_folder').attr('ch_folder_selected', ch_dir);
    $('a.open_dialog_editor').attr('ch_folder_selected', ch_dir);
    $('a.open_dialog_attach').attr('ch_folder_selected', ch_dir);
    $('a.open_dialog_folder').attr('nr_nivel_arquivo_selected', ch_nivel);
    $('a.open_dialog_delete_folder').attr('nr_nivel_arquivo_selected', ch_nivel);
    $('a.open_dialog_editor').attr('nr_nivel_arquivo_selected', ch_nivel);
    $('a.open_dialog_attach').attr('nr_nivel_arquivo_selected', ch_nivel);
    $('#div_list_arq').html('');
    $(el).closest('.div_folder').find('.div_folder').remove();
    $('#div_list_dir .div_folder').removeClass('selected');

    //fechar as pastas do mesmo nível
    $('a[nivel="' + ch_nivel + '"]').not('[chave="' + ch_dir + '"]').closest('.div_folder').find('>div').remove();

    //sinal de aberto ou fechado
    $('#div_list_dir a span').html('&gt;&nbsp;');
    var aDir = ch_dir.split('/');
    var sDir = '';
    for (var i = 0; i < aDir.length; i++) {
        sDir += (sDir != '' ? '/' : '') + aDir[i];
        $('a[chave="' + sDir + '"] span').html('v&nbsp;&nbsp;');
    }

    $('#div_list_arq').attr('ch_dir', ch_dir);

    $("#div_list_arq").dataTablesLight({
        sAjaxUrl: "./ashx/Datatable/ArquivoDatatable.ashx?ch_arquivo_superior=" + ch_dir,
        aoColumns: _columns_arquivos,
        sIdTable: 'datatable_arquivos',
        responsive: null,
        bFilter: true,
        fnCreatedRow: function (nRow, aData, iDataIndex) {
            if(aData.nr_tipo_arquivo == '1'){
                nRow.className += " tr_arq";
                nRow.setAttribute('tipo', aData.ar_arquivo.mimetype.split('/')[1]);
                nRow.setAttribute('chave', aData.ch_arquivo);
                nRow.setAttribute('id_file', aData.ar_arquivo.id_file);
                nRow.setAttribute('nm_arquivo', aData.nm_arquivo);
            }
        },
    });


    $('#div_list_dir a[chave="' + ch_dir + '"]').closest('.div_folder').addClass('selected');
    $('#div_list_dir a[chave="' + ch_dir + '"] span').html('v&nbsp;&nbsp;');

    exibirControlesDocumentosProduzidos(ch_dir);
}

function abrirModalImportarArquivo(el) {
    $('#input_chave_importar').val('');
    $('#form_importar_arquivo').attr('onsubmit', 'javascript:return importarArquivo(\'' + el.getAttribute('div_arquivo') + '\',\'form_importar_arquivo\');')
    $('#div_modal_importar_arquivo').modallight({
        sTitle: "Importar Documentos Produzidos",
        sHeight: "auto",
        sWidth: "850",
        oPosition: "center",
        oButtons: [
                    { html: '<img alt="importar" valign="absmiddle" src="' + _urlPadrao + '/Imagens/ico_import.png" width="20" height="20" /> Importar', click: function () { form_importar_arquivo.onsubmit(); } },
                    { html: '<img alt="cancelar" valign="absmiddle" src="' + _urlPadrao + '/Imagens/ico_fechar.png" width="20" height="20" /> Cancelar', click: function () { $('#div_modal_importar_arquivo').modallight("close"); } }
                ],
        fnClose: function () {
            $('#div_modal_importar_arquivo').modallight('destroy');
        }
    });
    carregarArquivosProduzidos("arquivos_orgao_cadastrador");
}

function exibirControlesDocumentosProduzidos(ch_dir) {
    if ($('#div_list_dir div.div_folder.selected').length > 0) {
        if (ch_dir.indexOf('000shared/images') == 0) {
            $('a.open_dialog_delete_folder').hide();
            $('a.open_dialog_editor').hide();
            $('#div_attach_file input[type="file"]').attr('accept', 'image/*');
        }
        else {
            $('a.open_dialog_delete_folder').show();
            $('a.open_dialog_editor').show();
            $('#div_attach_file input[type="file"]').attr('accept', '');
        }
        $('#div_controls_folder').show();
    } else {
        $('#div_controls_folder').hide();
    }
}

function selecionarDocumentoImportar(el) {
    var id_file = $(el).closest('tr.tr_arq').attr('id_file');
    var file_name = $(el).closest('tr.tr_arq').attr('file_name');
    var chave = $(el).closest('tr.tr_arq').attr('chave');
    var tipo = $(el).closest('tr.tr_arq').attr('tipo');
    $('#form_importar_arquivo input[name="id_file"]').val(id_file);

    $('#form_importar_arquivo input[name="ds_diario"]').val($('#ds_diario').val());



    $('#input_chave_importar').val(chave + '.' + tipo);
    $('#input_chave_importar').closest('div.table').show();
}

function importarArquivo(id_div_file, id_form) {
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
                $('#' + id_div_file + ' input.json_arquivo').val(JSON.stringify(data));
                $('#' + id_div_file + ' label.name').text(data.filename);
                $('#' + id_div_file + ' a.attach').hide();
                $('#' + id_div_file + ' a.delete').show();
            }
            $("#input_file").val("");
            $('#div_modal_importar_arquivo').modallight('close');
        }
    }
    return fnSalvarForm(id_form, sucesso);
}

function montarDescricaoDiario(oJson_diario) {
    return oJson_diario.nm_tipo_fonte + ' nº ' +
                        oJson_diario.nr_diario +
                        (IsNotNullOrEmpty(oJson_diario.cr_diario) ? ' ' + oJson_diario.cr_diario : '') +
                        ((IsNotNullOrEmpty(oJson_diario.nm_tipo_edicao) && oJson_diario.nm_tipo_edicao != 'Normal' || IsNotNullOrEmpty(oJson_diario.nm_diferencial_edicao)) ? ', Edição ' + oJson_diario.nm_tipo_edicao + (IsNotNullOrEmpty(oJson_diario.nm_diferencial_edicao) ? ' ' + oJson_diario.nm_diferencial_edicao : '') : '') +
                        (oJson_diario.st_suplemento ? ', Suplemento' + (IsNotNullOrEmpty(oJson_diario.nm_diferencial_suplemento) ? ' ' + oJson_diario.nm_diferencial_suplemento : '') : '') +
                        ', seção ' + oJson_diario.secao_diario + ' de ' +
                        oJson_diario.dt_assinatura;
}


/*Funções para marcação de caput de arquivos de norma
================================================*/
function ehInciso(termo) {
    var chars = ["I", "V", "X", "L", "C", "D", "M"];
    for (var i = 0; i < termo.length; i++) {
        if (chars.indexOf(termo[i]) < 0) {
            return false;
        }
    }
    return true;
}

function ehAlinea(termo) {
    var chars = ["i", "v", "x", "l", "c", "d", "m"];
    for (var i = 0; i < termo.length; i++) {
        if (chars.indexOf(termo[i]) < 0) {
            return false;
        }
    }
    return true;
}

//Gera um linkname e um caput para ser usado na ancora com base no conteúdo
function generateLinkNameCaput(text) {
    var id = '';
    text = $.trim(text);
    if (IsNotNullOrEmpty(text)) {
        var palavras = text.split(' ');
        if (palavras[0] == 'TÍTULO') {
            palavras[1] = palavras[1].replace(/[^a-z0-9]/gi, '_rep_');
            id = 'tit' + palavras[1].split('_rep_')[0];
        }
        else if (palavras[0] == 'CAPÍTULO') {
            palavras[1] = palavras[1].replace(/[^a-z0-9]/gi, '_rep_');
            id = 'cap' + palavras[1].split('_rep_')[0];
        }
        else if (palavras[0] == 'Art.') {
            id = 'art' + palavras[1];
            id = id.replace(/[^a-z0-9]/gi, '');
        }
        else if (palavras[0] == 'Parágrafo') {
            id = 'par';
        }
        else if (palavras[0] == '§') {
            id = 'par' + palavras[1];
            id = id.replace(/[^a-z0-9]/gi, '');
        }
        else if (ehInciso(palavras[0])) {
            id = 'inc' + palavras[0].replace(/[^a-z0-9]/gi, '');
        }
        else if (palavras[0].length == 2 && palavras[0][1] == ")") {
            id = 'let' + palavras[0][0].replace(/[^a-z]/gi, '');
        }
        else if (ehAlinea(palavras[0])) {
            id = 'ali' + palavras[0].replace(/[^a-z]/g, '');
        }
    }
    return id;
}

//definir nivel do arquivo
function definirNivelCaput(name) {
    var nivel = 10;
    var niveis = ["tit", "cap", "art", "par", "inc", "ltr", "aln"];
    if (IsNotNullOrEmpty(name) && name.indexOf('_') > -1) {
        name = name.substring(name.lastIndexOf('_') + 1);
    }
    for (var i = 0; i < niveis.length; i++) {
        if (name.indexOf(niveis[i]) == 0) {
            nivel = i;
            break;
        }
    }
    return nivel;
}

function definirEstruturaCaput(name, niveis) {
    var old_name = name;
    if (IsNotNullOrEmpty(name)) {
        if (name.indexOf('_') > -1) {
            name = name.substring(name.lastIndexOf('_') + 1);
        }
        old_name = name;
        name = name.substring(0, 3);
    }
    var nivel = definirNivelCaput(name);
    var primeiro_nivel = 0;
    var ultimo_nivel = 0;
    if (IsNotNullOrEmpty(niveis)) {
        primeiro_nivel = definirNivelCaput(niveis[0]);
        ultimo_nivel = definirNivelCaput(niveis[niveis.length - 1]);
    }

    if (nivel < primeiro_nivel) {
        niveis = [old_name];
    }
    if (nivel > ultimo_nivel) {
        niveis.push(old_name);
    }
    else if (nivel <= ultimo_nivel) {
        var niveis2 = [];
        for (var i = 0; i < niveis.length; i++) {
            if (niveis[i].indexOf(name) == 0) {
                niveis2.push(old_name);
                break;
            }
            niveis2.push(niveis[i]);
        }
        niveis = niveis2;
    }
    return niveis;
}

function configureCkeditor(){
    CKEDITOR.config.width = '100%';
    CKEDITOR.config.height = 400;
    CKEDITOR.stylesSet.add('default',[
        {name:'Epígrafe', element: 'h1', attributes: {
            epigrafe: 'epigrafe'
            },
            styles:{'font-family':'tahoma', 'font-size':'14px', 'font-weight':'bold', 'text-align':'center'} 
        },
        {name:'Autoria', element: 'p', styles:{'text-align':'center', 'margin-top':'1px', 'font-size':'14px', 'font-family': 'Tahoma'}},
        {name:'Ementa', element: 'p', styles:{'margin-left':'50%', 'text-align':'justify', 'font-size':'14px', 'font-family': 'Tahoma'}},
        {name:'Corpo', element: 'p', styles:{'text-align':'justify', 'font-size':'14px', 'font-family': 'Tahoma'}},
        {name:'Seção', element: 'p', styles:{'text-align':'center', 'font-size':'14', 'font-weight': 'bold', 'font-family': 'Tahoma'}},
        {name:'Assinatura', element: 'p', styles:{'text-align':'center', 'font-size':'14px', 'font-weight': 'bold', 'font-family': 'Tahoma'}},
        {name:'Anexo', element: 'p', styles:{'text-align':'center', 'font-size':'14px', 'font-weight': 'bold', 'font-family': 'Tahoma'}}
    ]);
                    
}

function loadCkeditor(id_textarea){
    CKEDITOR.replace(id_textarea, {
        extraAllowedContent: 'p[linkname,replaced_by];h[epigrafe]',
        filebrowserImageBrowseUrl: './Download/imagens'
    });
}
