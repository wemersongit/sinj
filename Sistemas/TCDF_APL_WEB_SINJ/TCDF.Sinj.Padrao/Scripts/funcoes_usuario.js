function PreencherUsuarioEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_usuario').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.nm_login_usuario != null && data.nm_login_usuario != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_login_usuario').val(data.nm_login_usuario);
                    $('#nm_usuario').val(data.nm_usuario);
                    $('#email_usuario').val(data.email_usuario);
                    $('#pagina_inicial').val(data.pagina_inicial);
                    $('#ds_pagina_inicial').val(data.ds_pagina_inicial);
                    $('#ch_perfil').val(data.ch_perfil);
                    $('#nm_perfil').val(data.nm_perfil);
                    $('#ch_tema option[value="' + data.ch_tema + '"]').attr('selected', 'selected');
                    $('#orgao_cadastrador option[value="' + data.orgao_cadastrador.id_orgao_cadastrador + '"]').attr('selected', 'selected');
                    $('#in_alterar_senha').prop("checked", data.in_alterar_senha);
                    $('#st_usuario').prop("checked", data.st_usuario);
                    for (var i = 0; i < data.grupos.length; i++) {
                        $('table.grupos input[value="' + data.grupos[i] + '"]').prop("checked", true);
                    }
                }
            }
        };
        var inicio = function () {
            $('#div_loading_usuario').show();
            $('#div_usuario').hide();
        }
        var complete = function () {
            $('#div_loading_usuario').hide();
            $('#div_usuario').show();
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

function DetalhesUsuario() {
    var id_doc = GetParameterValue("id_doc");
    var ch_usuario = GetParameterValue("ch_usuario");
    if (id_doc != "" || ch_usuario != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_usuario').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.nm_login_usuario != null && data.nm_login_usuario != "") {
                    if (ValidarPermissao(_grupos.usr_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar" href="./EditarUsuario.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.usr_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_login_usuario').text(data.nm_login_usuario);
                    $('#div_nm_usuario').text(data.nm_usuario);
                    $('#div_email_usuario').text(GetText(data.email_usuario));
                    $('#div_pagina_inicial').text(data.ds_pagina_inicial);
                    $('#div_tema').text(data.ch_tema);
                    $('#div_dt_ultimo_login').text(data.dt_ultimo_login != null ? data.dt_ultimo_login : "" );
                    $('#div_in_alterar_senha').text(data.in_alterar_senha ? "sim" : "não");
                    $('#div_nr_tentativa_login').text(data.nr_tentativa_login);
                    $('#div_orgao_cadastrador').text(data.orgao_cadastrador.nm_orgao_cadastrador);
                    $('#checkbox_st_usuario').prop('checked',data.st_usuario);
                    $('#div_nm_perfil').text(GetText(data.nm_perfil));
                    for (var i = 0; i < data.grupos.length; i++) {
                        $('td#' + data.grupos[i].replace('.', '_').toLowerCase()).html('<img alt="sim" src="'+_urlPadrao+'/Imagens/ico_check_p.png" />');
                    }
                }
            }
        };
        var inicio = function () {
            $('#div_loading_usuario').show();
            $('#div_usuario').hide();
        }
        var complete = function () {
            $('#div_loading_usuario').hide();
            $('#div_usuario').show();
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

function SelecionarPerfil() {
    var grupos = $('#autocomplete_grupos').val().split(',');
    let user = $('#nm_perfil').val();
    $('#grupos input[type="checkbox"]').prop("checked", false);
    // NOTE: Essa verificação abaixo é apenas uma gambiarra pois não foi encontrado um jeito de inserir novas permissoes no array de permissões.
    // O desejavel a longo prazo seria corrigir e deixar padronizado. By Victor
    if(user != "Usuário Comum"){
        $('#nor_eml').prop('checked',true);
        $('#nor_hsp').prop('checked',true);
    }else{
        $('#nor_eml').prop('checked',false);
        $('#nor_hsp').prop('checked',false);
    }
    for (var i = 0; i < grupos.length; i++) {
        $('table.grupos input[value="' + grupos[i] + '"]').prop("checked", true);
    }
}

function ConstruirControlesDinamicos() {
    ConstruirSelectOrgaosCadastradores();
    ConstruirSelectTemas();
    $('#div_pagina_inicial').autocompletelight({
        sKeyDataName: "pagina_inicial",
        sValueDataName: "ds_pagina_inicial",
        sInputHiddenName: "pagina_inicial",
        sInputName: "ds_pagina_inicial",
        jData: _paginas,
        bLinkAll: true,
        sLinkName: "a_pagina_inicial"
    });
    $('#div_perfil').autocompletelight({
        sKeyDataName: "ch_perfil",
        sValueDataName: "nm_perfil",
        sInputHiddenName: "ch_perfil",
        sInputName: "nm_perfil",
        sAjaxUrl: "./ashx/Autocomplete/PerfilAutocomplete.ashx",
        bLinkAll: true,
        sLinkName: "a_perfil",
        dOthersHidden: [
                        { campo_app: "autocomplete_grupos", campo_base: "grupos" }
                    ]
    });
}


function AbrirModalFiltrarPorStatus() {
    $('#div_modal_filtro_status').modallight({
        sTitle: "Filtrar por status",
        sType: "default",
        oButtons: [{
            text: "Filtrar",
            click: function () {
                var filtro = $('input[type="checkbox"]:checked', this).serialize();
                if (IsNotNullOrEmpty(filtro)) {
                    $("#div_resultado").dataTablesLight({
                        sAjaxUrl: './ashx/Datatable/UsuarioDatatable.ashx?' + filtro,
                        aoColumns: _columns_usuario,
                        bFilter: true
                    });
                }
                $(this).dialog("close");
            }
        }]
    });
}
