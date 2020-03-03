function PreencherRequeridoEdicao() {
    var id_doc = GetParameterValue("id_doc");
    if (id_doc != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_requerido').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_requerido != null && data.ch_requerido != "") {
                    $('#id_doc').val(data._metadata.id_doc);
                    $('#nm_requerido').val(data.nm_requerido);
                    $('#ds_requerido').val(data.ds_requerido);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_requerido').show();
            $('#div_requerido').hide();
        }
        var complete = function () {
            $('#div_loading_requerido').hide();
            $('#div_requerido').show();
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

function DetalhesRequerido() {
    var id_doc = GetParameterValue("id_doc");
    var ch_requerido = GetParameterValue("ch_requerido");
    if (id_doc != "" || ch_requerido != "") {
        var sucesso = function (data) {
            if (IsNotNullOrEmpty(data)) {
                if (data.error_message != null && data.error_message != "") {
                    $('#div_notificacao_requerido').messagelight({
                        sTitle: "Erro",
                        sContent: data.error_message,
                        sType: "error",
                        sWidth: "",
                        iTime: null
                    });
                }
                if (data.ch_requerido != null && data.ch_requerido != "") {
                    if (ValidarPermissao(_grupos.rqi_edt)) {
                        $('#div_controls_detalhes').append('<a title="Editar" href="./EditarRequerido.aspx?id_doc=' + data._metadata.id_doc + '"><img alt="editar" src="'+_urlPadrao+'/Imagens/ico_pencil_p.png"/></a> &nbsp;');
                    }
                    if (ValidarPermissao(_grupos.rqi_exc)) {
                        $('#div_controls_detalhes').append('<a title="Excluir" href="javascript:void(0);" onclick="javascript:Excluir(' + data._metadata.id_doc + ');" ><img alt="excluir" src="'+_urlPadrao+'/Imagens/ico_trash_p.png"/></a>');
                    }
                    DetalhesCadastro(data.nm_login_usuario_cadastro, data.dt_cadastro); // Chama a funçao que preenche os dados do cadastro passando o nome e a data como argumentos. (Essa funçao está em funcoes_sinj)
					DetalhesAlteracoes(data.alteracoes); // Chama a funçao que preenche as alterações passando a lista como argumento. (Essa funçao está em funcoes_sinj)
                    $('#div_nm_requerido').text(data.nm_requerido);
                    $('#div_ds_requerido').text(data.ds_requerido);
                }
            }
        };
        var inicio = function () {
            $('#div_loading_requerido').show();
            $('#div_requerido').hide();
        }
        var complete = function () {
            $('#div_loading_requerido').hide();
            $('#div_requerido').show();
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
