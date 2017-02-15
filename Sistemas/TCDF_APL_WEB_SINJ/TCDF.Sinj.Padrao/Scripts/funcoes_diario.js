function DetalhesDeDiario() {
    var id_doc = GetParameterValue("id_doc");
    var ch_diario = GetParameterValue("ch_diario");
    if (id_doc != "" || ch_diario != "") {
        var sucesso = function(data) {
            if (IsNotNullOrEmpty(data)) {
                if (IsNotNullOrEmpty(data.error_message)) {
                    $('#div_notificacao_diario').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                } else if (data.ch_diario != null && data.ch_diario != "") {
                    if (ValidarPermissao(_grupos.dio_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar Diário" href="./EditarDiario.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="' + _urlPadrao + '/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.dio_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir Diário" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="' + _urlPadrao + '/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
                    DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_tipo_fonte').text(data.nm_tipo_fonte);
                    $('#div_nm_tipo_edicao').text(getVal(data.nm_tipo_edicao));
                    $('#div_nm_diferencial_edicao').text(getVal(data.nm_diferencial_edicao));
                    $('#div_nr_diario').text(data.nr_diario);
                    $('#div_cr_diario').text(data.cr_diario);
                    $('#div_secao_diario').text(data.secao_diario);
                    $('#div_dt_assinatura').text(data.dt_assinatura);
                    if (!data.st_pendente) {
                        $('#div_st_pendente').text("Não");
                        $('#line_ds_pendencia').hide();
                    } else {
                        $('#div_st_pendente').text("Sim");
                        $('#div_ds_pendencia').text(data.ds_pendencia);
                        $('#line_ds_pendencia').show();
                    }
                    if (!data.st_suplemento) {
                        $('#div_st_suplemento').text("Não");
                        $('#line_nm_diferencial_suplemento').hide();
                    } else {
                        $('#div_st_suplemento').text("Sim");
                        $('#div_nm_diferencial_suplemento').text(data.nm_diferencial_suplemento);
                        $('#line_nm_diferencial_suplemento').show();
                    }
                    if (IsNotNullOrEmpty(data.ar_diario, 'id_file')) {
                        $('#div_ar_diario').html(
                            '<a title="baixar arquivo" target="_blank" href="./BaixarArquivoDiario.aspx?id_file=' + data.ar_diario.id_file + '"><img src="' + _urlPadrao + '/Imagens/ico_download_p.png" alt="download" /></a>' +
                            '&nbsp;&nbsp;' +
                            '<a title="visualizar texto" target="_blank" href="./TextoArquivoDiario.aspx?id_file=' + data.ar_diario.id_file + '" ><img src="' + _urlPadrao + '/Imagens/ico_doc_p.png" alt="texto" /></a>'
                        );
                    }
                    if (IsNotNullOrEmpty(data, 'arquivos')) {
                        for (var i = 0; i < data.arquivos.length; i++) {
                            var arquivo = data.arquivos[i];
                            $('#div_ar_diario').append(
                                '<a title="baixar arquivo" target="_blank" href="./BaixarArquivoDiario.aspx?id_file=' + arquivo.arquivo_diario.id_file + '"><img src="' + _urlPadrao + '/Imagens/ico_download_p.png" alt="download" /></a>' +
                                getVal(arquivo.ds_arquivo)
                            );
                        }
                    }
                    $('#div_diario').show();
                }
            }
        };
        var inicio = function() {
            $('#div_loading_diario').show();
            $('#div_diario').hide();
        }
        var complete = function() {
            $('#div_loading_diario').hide();
        }
        $.ajaxlight({
            sUrl: MostrarPaginaAjax("VIS") + window.location.search,
            sType: "POST",
            fnSuccess: sucesso,
            fnComplete: complete,
            fnBeforeSend: inicio,
            fnError: null,
            bAsync: true
        });
    }
}

function ConstruirControlesDinamicos() {
    $('#div_autocomplete_tipo_fonte').autocompletelight({
        sKeyDataName: "ch_tipo_fonte",
        sValueDataName: "nm_tipo_fonte",
        sInputHiddenName: "ch_tipo_fonte",
        sInputName: "nm_tipo_fonte",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeFonteAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_fonte"
    });

    $('#div_autocomplete_tipo_edicao').autocompletelight({
        sKeyDataName: "ch_tipo_edicao",
        sValueDataName: "nm_tipo_edicao",
        sInputHiddenName: "ch_tipo_edicao",
        sInputName: "nm_tipo_edicao",
        sAjaxUrl: "./ashx/Autocomplete/TipoDeEdicaoAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_tipo_edicao"
    });
    toggleCheckedShowHide('#st_pendente', '#line_ds_pendencia');
    toggleCheckedShowHide('#st_suplemento', '#line_nm_diferencial_suplemento');

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

function PreencherDiarioEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_diario').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_tipo_fonte != null && data.ch_tipo_fonte != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#ch_tipo_fonte').val(data.ch_tipo_fonte);
                    $('#nm_tipo_fonte').val(data.nm_tipo_fonte);
                    $('#ch_tipo_edicao').val(data.ch_tipo_edicao);
                    $('#nm_tipo_edicao').val(data.nm_tipo_edicao);
                    $('#nm_diferencial_edicao').val(getVal(data.nm_diferencial_edicao));
                    $('#nr_diario').val(data.nr_diario);
                    $('#cr_diario').val(data.cr_diario);
                    var secao_splited = data.secao_diario.replaceAll(', ', ' ').replaceAll(' e ', ' ').split(' ');
                    for (var i = 0; i < secao_splited.length; i++) {
                        $('#form_diario input[name="secao_diario"][value="' + secao_splited[i] + '"]').prop('checked', true);
                    }
                    $('#dt_assinatura').val(data.dt_assinatura);
                    $('#st_pendente').prop("checked", data.st_pendente);
                    $('#ds_pendencia').val(data.ds_pendencia);
                    $('#st_suplemento').prop("checked", data.st_suplemento);
                    $('#nm_diferencial_suplemento').val(getVal(data.nm_diferencial_suplemento));
                    toggleCheckedShowHide('#st_pendente', '#line_ds_pendencia');
                    toggleCheckedShowHide('#st_suplemento', '#line_nm_diferencial_suplemento');

                    if (IsNotNullOrEmpty(data, 'ar_diario') && IsNotNullOrEmpty(data.ar_diario.uuid) && IsNotNullOrEmpty(data.ar_diario.mimetype) && IsNotNullOrEmpty(data.ar_diario.filesize) && IsNotNullOrEmpty(data.ar_diario.id_file)) {
                        
                        if (data.arquivos == null) {
                            data.arquivos = [];
                        }
                        data.arquivos.push({ arquivo_diario: data.ar_diario, ds_arquivo: "" });

                    }
                    for (var i = 0; i < data.arquivos.length; i++) {
                        var j = i + 1;
                        var arquivo = data.arquivos[i];
                        $('#div_nr_arquivos').append('<input type="hidden" name="nr_arquivos" value="' + j + '" />');
                        $('#table_arquivos tbody tr.tr_vazia').remove();
                        $('#table_arquivos tbody').append(
                            '<tr nr="' + j + '">' +
                            '<td>' +
                            '<input type="hidden" name="json_arquivo_diario_' + j + '" class="json_arquivo" value=""/>' +
                            arquivo.arquivo_diario.filename +
                            '</td>' +
                            '<td>' +
                            '<input type="text" name="ds_arquivo_diario_' + j + '" value="' + arquivo.ds_arquivo + '" />' +
                            '</td>' +
                            '<td class="text-center">' +
                            '<button class="clean" title="Remover arquivo" type="button" onclick="javascript:deletarArquivoDiario(this);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></button>' +
                            '</td>' +
                            '</tr>'
                        );
                        $('#table_arquivos tbody tr[nr="' + j + '"] input.json_arquivo').val(JSON.stringify(arquivo.arquivo_diario));
                    }
                }
            }
        };
        var inicio = function() {
            $('#div_loading_diario').show();
            $('#div_diario').hide();
        }
        var complete = function() {
            $('#div_loading_diario').hide();
            $('#div_diario').show();
        }
        $.ajaxlight({
            sUrl: MostrarPaginaAjax("VIS") + window.location.search,
            sType: "POST",
            fnSuccess: sucesso,
            fnComplete: complete,
            fnBeforeSend: inicio,
            fnError: null,
            bAsync: true
        });
    }
}

function SelecionarOperador() {
    if ($('#op_dt_assinatura').val() == 'intervalo') {
        $('#div_intervalo').show();
    } else {
        $('#div_intervalo').hide();
    }
}

function salvarArquivoSelecionadoDiario(el) {
    if (IsNotNullOrEmpty(el, 'value')) {
        var sucesso = function(data) {
            $('#super_loading').hide();
            if (data.error_message != null && data.error_message != "") {
                notificar('#div_diario', data.error_message, 'error');
            } else if (IsNotNullOrEmpty(data, "id_file")) {
                var nr = 1;
                var inputs_nr = $('#div_nr_arquivos input[name="nr_arquivos"]');
                for (var i = 0; i < inputs_nr.length; i++) {
                    var sNr = inputs_nr[i].value;
                    nr = parseInt(sNr) + 1;
                }
                $('#div_nr_arquivos').append('<input type="hidden" name="nr_arquivos" value="' + nr + '" />');
                $('#table_arquivos tbody tr.tr_vazia').remove();
                $('#table_arquivos tbody').append(
                    '<tr nr="' + nr + '">' +
                    '<td>' +
                    '<input type="hidden" name="json_arquivo_diario_' + nr + '" class="json_arquivo" value=""/>' +
                    data.filename +
                    '</td>' +
                    '<td>' +
                    '<input type="text" name="ds_arquivo_diario_' + nr + '" value="" />' +
                    '</td>' +
                    '<td class="text-center">' +
                    '<button class="clean" title="Remover arquivo" type="button" onclick="javascript:deletarArquivoDiario(this);"><img valign="absmiddle" alt="Excluir" src="' + _urlPadrao + '/Imagens/ico_delete_p.png" /></button>' +
                    '</td>' +
                    '</tr>'
                );
                $('#table_arquivos tbody tr[nr="' + nr + '"] input.json_arquivo').val(JSON.stringify(data));
            }
        }
        return fnSalvarForm('form_upload_file', sucesso);
    }
}

function anexarInputFileDiario() {
    $('#form_upload_file input[name="file"]').val('');
    $('#form_upload_file input[name="file"]').click();
}

function deletarArquivoDiario(el) {
    var tr = $(el).closest('tr');
    var nr = $(tr).attr('nr');
    $(tr).remove();
    $('input[name="nr_arquivos"][value="' + nr + '"]').remove();
    if ($('#table_arquivos tbody tr').length <= 0) {
        $('#table_arquivos tbody').append(
            '<tr class="tr_vazia">' +
            '<td colspan="3">Nenhum arquivo</td>' +
            '</tr>'
        );
    }
}