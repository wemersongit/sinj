function DetalhesConfiguracao() {
    if (IsNotNullOrEmpty(_user, 'nm_login_usuario')) {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_configuracao').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (IsNotNullOrEmpty(data, 'nm_login_usuario')) {

                    $('#div_nm_usuario').text(data.nm_usuario);
                    $('#div_email_usuario').text(data.email_usuario);
                    $('#div_pagina_inicial').text(data.ds_pagina_inicial);
                    $('#div_ch_tema').text(data.ch_tema);
                    if (ValidarPermissao(_grupos.cfg_edt)) {
                        $('#div_nm_usuario').append('<a class="fr" title="Alterar Nome" href="javascript:void(0);" onclick="javascript:EditarUsuarioPath(\'nm_usuario\');" ><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a>');
                        $('#line_senha').show();
                        $('#div_senha').html('<img alt="senha" src="'+_urlPadrao+'/Imagens/ico_key_p.png"/><a class="fr" title="Alterar Senha" href="javascript:void(0);" onclick="javascript:EditarSenha();" ><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a>');
                        $('#div_email_usuario').append('<a class="fr" title="Alterar E-mail" href="javascript:void(0);" onclick="javascript:EditarUsuarioPath(\'email_usuario\');" ><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a>');
                        $('#div_pagina_inicial').append('<a class="fr" title="Alterar Página Inicial" href="javascript:void(0);" onclick="javascript:EditarPaginaInicial();" ><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a>');
                        $('#div_ch_tema').append('<a class="fr" title="Alterar Tema de Cores" href="javascript:void(0);" onclick="javascript:EditarTema();" ><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a>');
                    }
                    var nome = "";
                    var nome_aux = "";
                    var th = "";
                    var td_inc = "";
                    var td_edt = "";
                    var td_pes = "";
                    var td_vis = "";
                    var td_exc = "";
                    var td_ger = "";
                    for (var i = 0; i < data.grupos.length; i++) {
                        nome = data.grupos[i].split('.')[0].toLowerCase();
                        var acao = data.grupos[i].split('.')[1].toLowerCase();
                        if (nome != nome_aux && nome_aux != "") {
                            $('#table_grupos tbody').append(
                                '<tr>' +
                                    '<th>' + MostrarNomeDoGrupo(nome_aux) + '</th>' +
                                    '<td>' + td_inc + '</td>' +
                                    '<td>' + td_edt + '</td>' +
                                    '<td>' + td_pes + '</td>' +
                                    '<td>' + td_vis + '</td>' +
                                    '<td>' + td_exc + '</td>' +
                                    '<td>' + td_ger + '</td>' +
                                '</tr>'
                            );
                            td_inc = "";
                            td_edt = "";
                            td_pes = "";
                            td_vis = "";
                            td_exc = "";
                            td_ger = "";
                        }
                        if(i == (data.grupos.length - 1)) {//Se for o último grupo preciso de um tratamento diferente se não ele acaba não sendo inserido na tabela
                            if (acao == "inc") {
                                td_inc = "x";
                            }
                            else if (acao == "edt") {
                                td_edt = "x";
                            }
                            else if (acao == "pes") {
                                td_pes = "x";
                            }
                            else if (acao == "vis") {
                                td_vis = "x";
                            }
                            else if (acao == "exc") {
                                td_exc = "x";
                            }
                            else if (acao == "ger") {
                                td_ger = "x";
                            }
                            $('#table_grupos tbody').append(
                                '<tr>' +
                                    '<th>' + MostrarNomeDoGrupo(nome) + '</th>' +
                                    '<td>' + td_inc + '</td>' +
                                    '<td>' + td_edt + '</td>' +
                                    '<td>' + td_pes + '</td>' +
                                    '<td>' + td_vis + '</td>' +
                                    '<td>' + td_exc + '</td>' +
                                    '<td>' + td_ger + '</td>' +
                                '</tr>'
                            );
                        }
                        nome_aux = nome;
                        if (acao == "inc") {
                            td_inc = "x";
                        }
                        else if (acao == "edt") {
                            td_edt = "x";
                        }
                        else if (acao == "pes") {
                            td_pes = "x";
                        }
                        else if (acao == "vis") {
                            td_vis = "x";
                        }
                        else if (acao == "exc") {
                            td_exc = "x";
                        }
                        else if (acao == "ger") {
                            td_ger = "x";
                        }
                    }
                }

            }
        };
        var inicio = function () {
            $('#div_loading_configuracao').show();
            $('#div_configuracao').hide();
        }
        var complete = function () {
            $('#div_loading_configuracao').hide();
            $('#div_configuracao').show();
        }
        $.ajaxlight({
            sUrl: MostrarPaginaAjax("VIS") + '?ch_usuario='+_user.nm_login_usuario,
            sType: "POST",
            fnSuccess: sucesso,
            fnComplete: complete,
            fnBeforeSend: inicio,
            fnError: null,
            bAsync: true
        });
    }
	var in_alterar_senha = GetParameterValue("in_alterar_senha");
	if (in_alterar_senha == "true" && _user.in_alterar_senha === true){
		EditarSenha();
        $('#div_notificacao_path_configuracao').messagelight({
            sContent: "É necessário redefinir sua senha.",
            sType: "alert"
        });
	}
}

function EditarUsuarioPath(path) {
    var ds_path = {
        'nm_usuario':"Novo Nome",
        'email_usuario':"Novo E-mail",
        'pagina_inicial':"Nova Página Inicial"
    };
    if (IsNotNullOrEmpty(_user, 'nm_login_usuario')){
        var html = '<form id="form_path_usuario" name="formPathUsuario" action="#" method="POST">' +
            '<input id="ch_usuario" type="hidden" value="' + _user.nm_login_usuario + '" label="ID" obrigatorio="sim" name="ch_usuario" />' +
            '<input id="path" type="hidden" value="' + path + '" label="Path" obrigatorio="sim" name="path" />' +
            '<div class="mauto w-100-pc table">' +
                '<div class="line">' +
                    '<div class="column w-30-pc">' +
                        '<div class="cell fr">' +
                            '<label>'+ds_path[path]+':</label>' +
                        '</div>' +
                    '</div>' +
                    '<div class="column w-70-pc">' +
                        '<div class="cell w-100-pc">' +
                            '<input type="text" label="'+ds_path[path]+'" obrigatorio="sim" name="value" class="w-90-pc" />' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>' +
            '<div id="div_notificacao_path_configuracao" style="display:none;" class="notify">' +
            '</div>' +
            '<div id="div_loading_path_configuracao" class="loading" style="display:none;"></div>' +
            '<div id="div_buttons_path_configuracao" style="width:220px; margin:auto;" class="loaded">' +
                '<button id="button_salvar_path_configuracao" onclick="javascript:return fnSalvar(\'form_path_usuario\');">' +
                    '<img src="'+_urlPadrao+'/Imagens/ico_disk_p.png" />Salvar' +
                '</button>' +
                '<button type="reset">' +
                    '<img src="'+_urlPadrao+'/Imagens/ico_eraser_p.png" />Limpar' +
                '</button>' +
            '</div>' +
        '</form>';
        if ($('#modal_path_usuario').length > 0) {
            $('#modal_path_usuario').html(html);
            $('#modal_path_usuario').dialog('option','width', 400);
            $('#modal_path_usuario').dialog('open');
        }
        else {
            $('<div id="modal_path_usuario" />').modallight({
                sTitle: "Editar " + ds_path[path],
                sType: "default",
                sContent: html,
                sWidth: 400,
                oButtons: []
            });
        }
    }
}

function EditarPaginaInicial() {
    if (IsNotNullOrEmpty(_user, 'nm_login_usuario')) {
        var html = '<form id="form_path_usuario" name="formPathUsuario" action="#" method="POST">' +
            '<input id="ch_usuario" type="hidden" value="' + _user.nm_login_usuario + '" label="ID" obrigatorio="sim" name="ch_usuario" />' +
            '<input id="path" type="hidden" value="pagina_inicial" label="Path" obrigatorio="sim" name="path" />' +
            '<div class="mauto w-100-pc table">' +
                '<div class="line">' +
                    '<div class="column w-30-pc">' +
                        '<div class="cell fr">' +
                            '<label>Nova Página Inicial:</label>' +
                        '</div>' +
                    '</div>' +
                    '<div class="column w-70-pc">' +
                        '<div id="div_pagina_inicial" class="cell w-100-pc">' +
                            '<input id="pagina_inicial" name="value" type="hidden" value="" />' +
                            '<input id="ds_pagina_inicial" name="ds_pagina_inicial" label="Nova Página Inicial" obrigatorio="sim" type="text" value="" class="w-80-pc" /><a title="Listar" id="a_pagina_inicial"></a>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>' +
            '<div id="div_notificacao_path_configuracao" style="display:none;" class="notify">' +
            '</div>' +
            '<div id="div_loading_path_configuracao" class="loading" style="display:none;"></div>' +
            '<div id="div_buttons_path_configuracao" style="width:220px; margin:auto;" class="loaded">' +
                '<button id="button_salvar_path_configuracao" onclick="javascript:return fnSalvar(\'form_path_usuario\');">' +
                    '<img src="'+_urlPadrao+'/Imagens/ico_disk_p.png" />Salvar' +
                '</button>' +
                '<button type="reset">' +
                    '<img src="'+_urlPadrao+'/Imagens/ico_eraser_p.png" />Limpar' +
                '</button>' +
            '</div>' +
        '</form>';
        if ($('#modal_path_usuario').length > 0) {
            $('#modal_path_usuario').html(html);
            $('#div_pagina_inicial').autocompletelight({
                sKeyDataName: "pagina_inicial",
                sValueDataName: "ds_pagina_inicial",
                sInputHiddenName: "pagina_inicial",
                sInputName: "ds_pagina_inicial",
                jData: _paginas,
                bLinkAll: true,
                sLinkName: "a_pagina_inicial"
            });
            $('#modal_path_usuario').dialog('option', 'width', 500);
            $('#modal_path_usuario').dialog('open');
        }
        else {
            $('<div id="modal_path_usuario" />').modallight({
                sTitle: "Editar Senha",
                sType: "default",
                sContent: html,
                sWidth: 500,
                oButtons: []
            });
            $('#div_pagina_inicial').autocompletelight({
                sKeyDataName: "pagina_inicial",
                sValueDataName: "ds_pagina_inicial",
                sInputHiddenName: "pagina_inicial",
                sInputName: "ds_pagina_inicial",
                jData: _paginas,
                bLinkAll: true,
                sLinkName: "a_pagina_inicial"
            });
        }
        $('.ui-autocomplete').height(50);
    }
}

function EditarTema() {
    if (IsNotNullOrEmpty(_user, 'nm_login_usuario')) {
        var html = '<form id="form_path_usuario" name="formPathUsuario" action="#" method="POST">' +
            '<input id="ch_usuario" type="hidden" value="' + _user.nm_login_usuario + '" label="ID" obrigatorio="sim" name="ch_usuario" />' +
            '<input id="path" type="hidden" value="ch_tema" label="Path" obrigatorio="sim" name="path" />' +
            '<div class="mauto w-100-pc table">' +
                '<div class="line">' +
                    '<div class="column w-30-pc">' +
                        '<div class="cell fr">' +
                            '<label>Nova Página Inicial:</label>' +
                        '</div>' +
                    '</div>' +
                    '<div class="column w-70-pc">' +
                        '<div id="div_pagina_inicial" class="cell w-100-pc">' +
                            '<select id="ch_tema" name="value"></select>'+
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>' +
            '<div id="div_notificacao_path_configuracao" style="display:none;" class="notify">' +
            '</div>' +
            '<div id="div_loading_path_configuracao" class="loading" style="display:none;"></div>' +
            '<div id="div_buttons_path_configuracao" style="width:220px; margin:auto;" class="loaded">' +
                '<button id="button_salvar_path_configuracao" onclick="javascript:return fnSalvar(\'form_path_usuario\');">' +
                    '<img src="'+_urlPadrao+'/Imagens/ico_disk_p.png" />Salvar' +
                '</button>' +
                '<button type="reset">' +
                    '<img src="'+_urlPadrao+'/Imagens/ico_eraser_p.png" />Limpar' +
                '</button>' +
            '</div>' +
        '</form>';
        if ($('#modal_path_usuario').length > 0) {
            $('#modal_path_usuario').html(html);
            $('#modal_path_usuario').dialog('option', 'width', 300);
            $('#modal_path_usuario').dialog('open');
        }
        else {
            $('<div id="modal_path_usuario" />').modallight({
                sTitle: "Editar Senha",
                sType: "default",
                sContent: html,
                sWidth: 300,
                oButtons: []
            });
        }
        ConstruirSelectTemas();
    }
}

function EditarSenha() {
    if (IsNotNullOrEmpty(_user, 'nm_login_usuario')) {
        var html = '<form id="form_path_usuario" name="formPathUsuario" action="#" method="POST">' +
            '<input id="ch_usuario" type="hidden" value="' + _user.nm_login_usuario + '" label="ID" obrigatorio="sim" name="ch_usuario" />' +
            '<input id="path" type="hidden" value="password" label="Path" obrigatorio="sim" name="path" />' +
            '<div class="mauto w-100-pc table">' +
                '<div class="line">' +
                    '<div class="column w-40-pc">' +
                        '<div class="cell fr">' +
                            '<label>Nova Senha:</label>' +
                        '</div>' +
                    '</div>' +
                    '<div class="column w-60-pc">' +
                        '<div class="cell w-100-pc">' +
                            '<input id="nova_senha" type="password" label="Nova Senha" obrigatorio="sim" specials="tamanho_minimo" tamanho="6" name="value" class="w-90-pc" />' +
                        '</div>' +
                    '</div>' +
                '</div>' +
                '<div class="line">' +
                    '<div class="column w-40-pc">' +
                        '<div class="cell fr">' +
                            '<label>Redigite a Senha:</label>' +
                        '</div>' +
                    '</div>' +
                    '<div class="column w-60-pc">' +
                        '<div class="cell w-100-pc">' +
                            '<input id="redigite_senha" type="password" class="w-90-pc" />' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>' +
            '<div id="div_notificacao_path_configuracao" style="display:none;" class="notify">' +
            '</div>' +
            '<div id="div_loading_path_configuracao" class="loading" style="display:none;"></div>' +
            '<div id="div_buttons_path_configuracao" style="width:220px; margin:auto;" class="loaded">' +
                '<button id="button_salvar_path_configuracao" onclick="javascript:return SalvarSenha();">' +
                    '<img src="'+_urlPadrao+'/Imagens/ico_disk_p.png" />Salvar' +
                '</button>' +
                '<button type="reset">' +
                    '<img src="'+_urlPadrao+'/Imagens/ico_eraser_p.png" />Limpar' +
                '</button>' +
            '</div>' +
        '</form>';
        if ($('#modal_path_usuario').length > 0) {
            $('#modal_path_usuario').html(html);
            $('#modal_path_usuario').dialog('option', 'width', 400);
            $('#modal_path_usuario').dialog('open');
        }
        else {
            $('<div id="modal_path_usuario" />').modallight({
                sTitle: "Editar Senha",
                sType: "default",
                sContent: html,
                sWidth: 400,
                oButtons: []
            });
        }
    }
}

function SalvarSenha() {
    if ($('#nova_senha').val() != $('#redigite_senha').val()) {
        $('#div_notificacao_path_configuracao').messagelight({
            sTitle: "Erro",
            sContent: "Redigite a senha corretamente.",
            sType: "error"
        });
    }
    else {
        fnSalvar('form_path_usuario');
    }
    return false;
}
