function excluirArquivo(el) {
    var chave = $(el).closest('tr.tr_arq').attr('chave');
    var sucesso = function (data) {
        $('#super_loading').hide();
        if (data.error_message != null && data.error_message != "") {
            notificar('#div_arquivos', data.error_message, 'error');
        }
        else if (IsNotNullOrEmpty(data, "success_message")) {
            $('#div_modal_confirme').modallight('close');
            ShowDialog({
                id_element: "modal_notificacao_success",
                sTitle: "Sucesso",
                sContent: data.success_message,
                sType: "success",
                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                fnClose: function () {
                    $(el).closest('tr.tr_arq').remove();
                    
                }
            });
        }
    };
    $.ajaxlight({
        sUrl: "./ashx/Exclusao/ArquivoExcluir.ashx?ch_arquivo=" + chave,
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: gComplete,
        fnBeforeSend: gInicio,
        bAsync: true,
        iTimeout: 40000
    });
}

function openDialogDeleteFile(el) {
    if ($('#div_modal_confirme').hasClass('ui-dialog-content')) {
        $('#div_modal_confirme').modallight('destroy');
    }
    $('#div_modal_confirme').modallight({
        sTitle: "Excluir Arquivo",
        sContent: "O arquivo será excluído permanentemente.</br>Deseja continuar?",
        oPosition: "center",
        sWidth: "400",
        oButtons: [{ html: '<img alt="ok" valign="absmiddle" src="' + _urlPadrao + '/Imagens/ico_check_p.png" width="16" height="16" /> Sim', click: function () { excluirArquivo(el); } },
                { text: 'Não', click: function () { $(this).dialog('close'); } }]
    });

    return;
}

function excluirDiretorio(el) {
    var chave = $(el).attr('ch_folder_selected');
    var nivel = $(el).attr('nr_nivel_arquivo_selected');
    var sucesso = function (data) {
        $('#super_loading').hide();
        if (data.error_message != null && data.error_message != "") {
            notificar('#div_arquivos', data.error_message, 'error');
        }
        else if (IsNotNullOrEmpty(data, "success_message")) {
            $('#div_modal_confirme').modallight('close');
            ShowDialog({
                id_element: "modal_notificacao_success",
                sTitle: "Sucesso",
                sContent: data.success_message,
                sType: "success",
                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                fnClose: function () {
                    results = [];
                    carregarArquivosProduzidos($("#ch_arquivo_raiz").val());
                }
            });
        }
    };
    $.ajaxlight({
        sUrl: "./ashx/Exclusao/ArquivoExcluir.ashx?ch_arquivo=" + chave + "&nr_nivel_arquivo=" + nivel + "&nr_tipo_arquivo=0",
        sType: "GET",
        fnSuccess: sucesso,
        fnComplete: gComplete,
        fnBeforeSend: gInicio,
        bAsync: true,
        iTimeout: 40000
    });
}

function openDialogDeleteFolder(el) {
    if ($('#div_modal_confirme').hasClass('ui-dialog-content')) {
        $('#div_modal_confirme').modallight('destroy');
    }
    $('#div_modal_confirme').modallight({
        sTitle: "Excluir Diretório",
        sContent: "O diretório será excluído permanentemente.</br>Deseja continuar?",
        sWidth: "400",
        oPosition: "center",
        oButtons: [{ html: '<img alt="ok" valign="absmiddle" src="' + _urlPadrao + '/Imagens/ico_check_p.png" /> Sim', click: function () { excluirDiretorio(el); } },
                { text: 'Não', click: function () { $(this).dialog('close'); } }]
    });

    return;
}



function fnUploadDocument(id_modal) {
    var id_form = $('#' + id_modal + ' form').attr('id');
    var sucesso = function (data) {
        $('#super_loading').hide();
        if (data.error_message != null && data.error_message != "") {
            notificar('#' + id_form, data.error_message, 'error');
        }
        else if (IsNotNullOrEmpty(data, "success_message")) {
            if ($('#' + id_modal).hasClass('ui-dialog-content')) {
                $('#' + id_modal).modallight('close');
            }
            ShowDialog({
                id_element: "modal_notificacao_success",
                sTitle: "Sucesso",
                sContent: data.success_message,
                sType: "success",
                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                fnClose: function () {
                    if (IsNotNullOrEmpty(data, 'arquivo')) {
                        if (data.action == "UPDATED") {
                            for (var i = 0; i < results.length; i++) {
                                if (results[i]._metadata.id_doc == data.arquivo._metadata.id_doc) {
                                    results[i] = data.arquivo;
                                    break;
                                }
                            }
                        }
                        else {
                            results.push(data.arquivo);
                        }
                        if (data.arquivo.nr_tipo_arquivo == 0) {
                            carregarArquivosProduzidos($("#ch_arquivo_raiz").val());
                            $('#div_list_dir .div_folder a[chave="' + data.arquivo.ch_arquivo + '"]').click();
                        }
                        else {
                            $('#div_list_dir div.div_folder.selected>a').click();
                        }
                    }
                }
            });
        }
    }
    return fnSalvarForm(id_form, sucesso);
}

function fnCloseEditorFile() {
    $('#span_titulo').text('Meus Arquivos');
    $('#div_arquivos').show();
    $('#div_editor_file').hide();
}

function fnCloseEditorLink() {
    $('#span_titulo').text('Meus Arquivos');
    $('#div_arquivos').show();
    $('#div_editor_link').hide();
}

function fnUploadEditorFile() {
    CKEDITOR.instances.textarea_arquivo_editor.updateElement();
    
    var id_form = $('#div_editor_file form').attr('id');
    var conteudoArquivo = $('#div_conteudo_arquivo').html($('#div_editor_file textarea[name="arquivo"]').val());
    var itensDoTextoaArquivo = $(conteudoArquivo).children();

    // NOTE: Remove parágrafos em branco para eviatr quebra de formatação
    // quando se adiciona uma nova Vide. By Jimmy/Lirão
    for(var i = 0; i < itensDoTextoaArquivo.length; i++) {
        // console.log(itensDoTextoaArquivo[i]);

        if (!itensDoTextoaArquivo[i].textContent.trim()) {
            // console.log("REMOVENDO");
            var img =itensDoTextoaArquivo[i].childNodes[0].nodeName;
            if(img != "IMG"){
                itensDoTextoaArquivo[i].remove();
            }
        } 
    }

    var valor = 'div_conteudo_arquivo';
    console.log(valor);
    inserirMarcacoesNosParagrafos('div_conteudo_arquivo', false);
    $('#div_editor_file textarea[name="arquivo"]').val(window.encodeURI($('#div_conteudo_arquivo').html()));

    var sucesso = function (data) {
        $('#super_loading').hide();
        if (data.error_message != null && data.error_message != "") {
            notificar('#' + id_form, data.error_message, 'error');
        }
        else if (IsNotNullOrEmpty(data, "success_message")) {
            ShowDialog({
                id_element: "modal_notificacao_success",
                sTitle: "Sucesso",
                sContent: data.success_message,
                sType: "success",
                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                fnClose: function () {
                    fnCloseEditorFile();
                    if (IsNotNullOrEmpty(data, 'arquivo')) {
                        if (data.action == "UPDATED") {
                            for (var i = 0; i < results.length; i++) {
                                if (results[i]._metadata.id_doc == data.arquivo._metadata.id_doc) {
                                    results[i] = data.arquivo;
                                    break;
                                }
                            }
                        }
                        else {
                            results.push(data.arquivo);
                        }
                        $('#div_list_dir div.div_folder.selected>a').click();
                    }
                }
            });
        }
    }
    return fnSalvarForm(id_form, sucesso);
}

function openDialogFolder(el, id_doc) {
    var ch_dir = el.getAttribute('ch_folder_selected');
    var nr_nivel = el.getAttribute('nr_nivel_arquivo_selected');

    $('#div_modal_folder input[name="ch_arquivo_superior"]').val(ch_dir);
    $('#div_modal_folder input[name="nr_nivel_arquivo_superior"]').val(nr_nivel);
    $('#div_modal_folder input[name="nm_arquivo"]').val('');
    $('#div_modal_folder input[name="id_doc"]').val('');

    if (ch_dir == "raiz") {
        $('#div_modal_folder input[name="ch_arquivo_superior"]').val('');
    }

    if (IsNotNullOrEmpty(id_doc)) {
        var sucesso = function (data) {
            if (data.error_message != null && data.error_message != "") {
                notificar('#div_modal_arquivo', data.error_message, 'error');
            }
            else if (IsNotNullOrEmpty(data, "_metadata.id_doc")) {
                $('#div_modal_folder input[name="ch_arquivo_superior"]').val(data.ch_arquivo_superior);
                $('#div_modal_folder input[name="nm_arquivo"]').val(data.nm_arquivo);
                $('#div_modal_folder input[name="id_doc"]').val(data._metadata.id_doc);
            }
        };

        $.ajaxlight({
            sUrl: "./ashx/Consulta/ArquivosConsulta.ashx?id_doc=" + id_doc,
            sType: "GET",
            fnSuccess: sucesso,
            fnComplete: gComplete,
            fnBeforeSend: gInicio,
            bAsync: true,
            iTimeout: 40000
        });

    }
    if (!$('#div_modal_folder').hasClass('ui-dialog-content')) {
        $('#div_modal_folder').modallight({
            sTitle: "Cadastrar/Editar Diretório",
            sHeight: "auto",
            sWidth: "500",
            oPosition: "center",
            oButtons: [
                        { html: '<img alt="salvar" valign="absmiddle" src="' + _urlPadrao + '/Imagens/ico_save.png" width="16" height="16" /> Salvar', click: function () { form_add_folder.onsubmit(); } },
                        { html: '<img alt="cancelar" valign="absmiddle" src="' + _urlPadrao + '/Imagens/ico_fechar.png" width="16" height="16" /> Cancelar', click: function () { $('#div_modal_folder').modallight('close'); } }
                    ]
        });
    }
    else {
        $('#div_modal_folder').modallight('open');
    }
}



function openDialogEditFile(el) {
    $('#span_titulo').text('Editor de Arquivo');
    $('#super_loading').show();

    var ch_dir = $(el).attr('ch_folder_selected');
    var nr_nivel = $(el).attr('nr_nivel_arquivo_selected');

    $('#div_editor_file input[name="ch_arquivo_superior"]').val(ch_dir);
    $('#div_editor_file input[name="nr_nivel_arquivo_superior"]').val(nr_nivel);
    $('#div_editor_file input[name="nm_arquivo"]').val('');
    $('#div_editor_file input[name="id_doc"]').val('');
    $('#div_editor_file input[name="id_file"]').val('');
    $('#div_editor_file input[name="in_editar"]').val('');



    //QUANDO O CKEDITOR JÁ POSSUIR UMA INSTANCIA DO TEXTAREA ARQUIVO, DESTROI A INSTANCIA E LIMPA OS CAMPOS
    if (IsNotNullOrEmpty(CKEDITOR.instances['textarea_arquivo_editor'])) {
        CKEDITOR.instances['textarea_arquivo_editor'].destroy(true);
        $('#div_editor_file textarea[name="arquivo"]').val('');
        $('#div_editor_file textarea[name="arquivo"]').hide();
    }

    var chave = $(el).closest('tr.tr_arq').attr('chave');
    var id_file = $(el).closest('tr.tr_arq').attr('id_file');
    var nm_arquivo = $(el).closest('tr.tr_arq').attr('nm_arquivo');

    //QUANDO POSSUIR ID_FILE É UMA EDIÇÃO, ENTÃO DEVE SER FEITO UM AJAX PARA RECUPERAR O ARQUIVO
    if (IsNotNullOrEmpty(id_file)) {
        var sucesso = function (data) {
            $('#div_editor_file input[name="id_doc"]').val(data.file.id_doc);
            $('#div_editor_file input[name="id_file"]').val(id_file);
            $('#div_editor_file input[name="in_editar"]').val('1');
            $('#div_editor_file input[name="nm_arquivo"]').val(nm_arquivo);
            $('#div_editor_file textarea[name="arquivo"]').val(window.unescape(data.fileencoded));
            setTimeout(function () {
                loadCkeditor('textarea_arquivo_editor');
                $('#div_arquivos').hide();
                $('#div_editor_file').show();
                gComplete();
            }, 2000);

        }
        $.ajaxlight({
            sUrl: "./ashx/Arquivo/HtmlFileEncoded.ashx?nm_base=sinj_arquivo&id_file=" + id_file,
            sType: "GET",
            fnSuccess: sucesso,
            fnBeforeSend: gInicio,
            bAsync: true
        });
        //Ajax({ url: "./Ashx/Det/FileDet.ashx?id_file=" + id_file, type: "GET", success: sucesso, beforeSend: gInicio });
    }
    else {
        setTimeout(function () {
            loadCkeditor('textarea_arquivo_editor');
            //CKEDITOR.config.plugins = "dialogui,dialog,basicstyles,panel,floatpanel,colorbutton,colordialog,templates,menu,contextmenu,div,resize,toolbar,elementspath,enterkey,entities,popup,filebrowser,fakeobjects,floatingspace,listblock,font,format,horizontalrule,htmlwriter,wysiwygarea,indent,indentblock,indentlist,justify,menubutton,list,liststyle,magicline,maximize,newpage,pagebreak,pastetext,pastefromword,preview,print,removeformat,selectall,showblocks,showborders,sourcearea,specialchar,scayt,stylescombo,tab,table,tabletools,undo,wsc";
            $('#div_arquivos').hide();
            $('#div_editor_file').show();
            gComplete();
        }, 2000);
    }


}
function openDialogAttachFile(el) {
    var ch_dir = $(el).attr('ch_folder_selected');
    var nr_nivel = $(el).attr('nr_nivel_arquivo_selected');
    var div_attach = $(el).attr('div_attach');
    $('#' + div_attach + ' input[name="ch_arquivo_superior"]').val(ch_dir);
    $('#' + div_attach + ' input[name="nr_nivel_arquivo_superior"]').val(nr_nivel);
    $('#' + div_attach + ' input[name="nm_arquivo"]').val('');
    $('#' + div_attach + ' input[name="id_doc"]').val('');
    $('#' + div_attach + ' input[type="file"]').click();
}

function selecionarArquivo(el) {
    if (IsNotNullOrEmpty(el, 'value')) {
        var id_form = el.getAttribute('targetform');
        var nm_file_splited = el.value.split('\\');
        var nm_arquivo = nm_file_splited[nm_file_splited.length - 1];
        if (nm_arquivo.indexOf('.') > -1) {
            nm_file_splited = nm_arquivo.split('.');
            nm_arquivo = '';
            for (var i = 0; i < nm_file_splited.length; i++) {
                if (i == (nm_file_splited.length - 1) || (nm_file_splited[i].length == 3 || nm_file_splited[i].length == 4)) {
                    continue;
                }
                nm_arquivo += nm_file_splited[i];
            }
        }
        $('#' + id_form + ' input[name="nm_arquivo"]').val(nm_arquivo);
        document.getElementById(id_form).onsubmit();
    }
}
var bProduzir = true;


/*==========Editor de Links===============
==========================================*/
function openDialogEditLink(el) {
    $('#span_titulo').text('Editor de Marcação');

    var ch_dir = $(el).attr('ch_folder_selected');
    var nr_nivel = $(el).attr('nr_nivel_arquivo_selected');

    var id_file = $(el).closest('tr.tr_arq').attr('id_file');
    var nm_arquivo = $(el).closest('tr.tr_arq').attr('nm_arquivo');

    var sucesso = function (data) {

        $('#form_editar_link input[name="id_doc"]').val(data.file.id_doc);
        $('#form_editar_link input[name="id_file"]').val(data.file.id_file);
        $('#form_editar_link input[name="in_editar"]').val('1');
        $('#form_editar_link input[name="nm_base"]').val('sinj_arquivo');
        $('#form_editar_link input[name="ch_arquivo_raiz"]').val('meus_arquivos');
        $('#form_editar_link input[name="nr_tipo_arquivo"]').val('1');

        $('#form_editar_link input[name="nm_arquivo"]').val(nm_arquivo);
        $('#form_editar_link textarea[name="arquivo"]').val('');
        $('#div_conteudo_arquivo').html(window.unescape(data.fileencoded).replaceAll('&nbsp;', ' '));

        if ($('#div_conteudo_arquivo p').length > 0) {
            inserirMarcacoesNosParagrafos('div_conteudo_arquivo', true);
            var title =
            '<div class="table">' +
                '<div class="line">' +
                    '<div class="column w-20-pc">' +
                        '<label>Nome: </label>' +
                    '</div>' +
                    '<div class="column w-70-pc">' +
                        '<input name="nome" type="text" class="w-80-pc" value="" />' +
                    '</div>' +
                '</div>' +
                '<div class="line">' +
                    '<div class="column w-100-pc text-center">' +
                        '<button type="button" onclick="javascript:salvarParagrafo(this)">Salvar</button>' +
                    '</div>' +
                '</div>' +
            '</div>';
            $('#div_conteudo_arquivo p button.buttontooltip').tooltip({ title: title, container: "#div_tooltip", html: true, trigger: "manual", placement: "auto top", template: '<div class="tooltip" role="tooltip"><button class="tooltip-close clean">x</button><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>' });
        }


        $('#div_arquivos').hide();
        $('#div_editor_link').show();

    }
    $.ajaxlight({
        sUrl: "./ashx/Arquivo/HtmlFileEncoded.ashx?nm_base=sinj_arquivo&id_file=" + id_file,
        sType: "GET",
        fnSuccess: sucesso,
        bAsync: true
    });
}

function salvarParagrafo(el) {
    var $tooltip = $(el).closest('div.tooltip');
    var novo_linkname = $('input[name="nome"]', $tooltip).val();

    var id_tooltip = $tooltip.attr('id');
    var $button = $('button[aria-describedby="' + id_tooltip + '"]');
    var $p = $button.closest('p');
    var linkname = $p.attr('linkname');
    $('a[name="' + linkname + '"]', $p).remove();
    $p.removeAttr("linkname");

    if (IsNotNullOrEmpty(novo_linkname)) {
        $p.prepend('<a id="' + novo_linkname + '" name="' + novo_linkname + '" class="linkname"></a>');
        $p.attr('linkname', novo_linkname);
    }
    $button.html('<img src="'+_urlPadrao+'/Imagens/ico_anchor_p.png" alt="editar" />');
    $button.click();
}
function clickTooltip(el) {
    var id_tooltip = $(el).attr('aria-describedby');
    $('button.buttontooltip').tooltip('hide');
    if (isNullOrEmpty(id_tooltip)) {
        $(el).tooltip('show');
        id_tooltip = $(el).attr('aria-describedby');

        var $p = $(el).closest('p');
        var linkname = $p.attr('linkname');

        if (IsNotNullOrEmpty(linkname)) {
            $('#' + id_tooltip + ' input[name="nome"]').val(linkname);
        }

        $('#' + id_tooltip + ' button.tooltip-close').bind('click', function () { $(el).tooltip('hide'); });
    }
    else {
        $('#' + id_tooltip + ' button.tooltip-close').unbind('click');
    }
}

function fnCloseEditarLink() {
    $('#span_titulo').text('Meus Arquivos');
    $('#div_arquivos').show();
    $('#div_editor_link').hide();
}

function fnSubmitEditarLink(id_form) {
    var $html_clonado = $('#div_conteudo_arquivo').clone();
    $html_clonado.find('button.buttontooltip').remove();
    $('input[name="arquivo"]').val(window.encodeURI($html_clonado.html()));
    var sucesso = function (data) {
        gComplete();
        if (data.error_message != null && data.error_message != "") {
            notificar('#' + id_form, data.error_message, 'error');
        }
        else if (IsNotNullOrEmpty(data, "success_message")) {
            ShowDialog({
                id_element: "modal_notificacao_success",
                sTitle: "Sucesso",
                sContent: data.success_message,
                sType: "success",
                oButtons: [{ text: "Ok", click: function () { $(this).dialog('close'); } }],
                fnClose: function () {
                    fnCloseEditarLink();
                    if (IsNotNullOrEmpty(data, 'arquivo')) {
                        if (data.action == "UPDATED") {
                            for (var i = 0; i < results.length; i++) {
                                if (results[i]._metadata.id_doc == data.arquivo._metadata.id_doc) {
                                    results[i] = data.arquivo;
                                    break;
                                }
                            }
                        }
                        else {
                            results.push(data.arquivo);
                        }
                        $('#div_list_dir div.div_folder.selected a').click();
                    }
                }
            });
        }
    }
    return fnSalvarForm(id_form, sucesso);
}
